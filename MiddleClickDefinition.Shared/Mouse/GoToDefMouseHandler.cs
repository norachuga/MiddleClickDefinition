using MiddleClickDefinition.Shared.Keys;
using MiddleClickDefinition.Shared.Options;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using System;
using System.Windows;
using System.Windows.Input;

namespace MiddleClickDefinition.Shared.Mouse
{
    internal sealed class GoToDefMouseHandler : MouseProcessorBase
    {
        private readonly IWpfTextView _view;
        private readonly IClassifier _aggregator;
        private readonly ITextStructureNavigator _navigator;
        private readonly IOleCommandTarget _commandTarget;
        private readonly OptionsProcessor _options;
        private readonly ModifierKeyStateProcessor _state;

        //not used for handling, but essential for figuring out the command IDs
        private readonly SVsServiceProvider _globalServiceProvider;

        private Point? _mouseDownAnchorPoint;

        public GoToDefMouseHandler(
            IWpfTextView view,
            IOleCommandTarget commandTarget,
            IClassifier aggregator,
            ITextStructureNavigator navigator,
            OptionsProcessor optionsProcessor,
            ModifierKeyStateProcessor state,
            SVsServiceProvider globalServiceProvider)
        {
            _view = view;
            _commandTarget = commandTarget;
            _aggregator = aggregator;
            _navigator = navigator;
            _options = optionsProcessor;
            _state = state;
            _globalServiceProvider = globalServiceProvider;
        }

        public override void PreprocessMouseLeave(MouseEventArgs e)
        {
            _mouseDownAnchorPoint = null;
        }

        public override void PreprocessMouseUp(MouseButtonEventArgs e)
        {
            if (e.ChangedButton.ToString().Equals("Middle") && _mouseDownAnchorPoint.HasValue)
            {
                var currentMousePosition = RelativeToView(e.GetPosition(_view.VisualElement));

                if (!InDragOperation(_mouseDownAnchorPoint.Value, currentMousePosition))
                {
                    if (IsSignificantElement(RelativeToView(e.GetPosition(_view.VisualElement))))
                    {
                        switch (_state.ModifierKey)
                        {
                            case ModifierKeyState.None:
                                DispatchCommand(_options.MiddleClick());
                                break;
                            case ModifierKeyState.Ctrl:
                                DispatchCommand(_options.CtrlMiddleClick());
                                break;
                            case ModifierKeyState.Shift:
                                DispatchCommand(_options.ShiftMiddleClick());
                                break;
                            case ModifierKeyState.CtrlShift:
                                DispatchCommand(_options.CtrlShiftMiddleClick());
                                break;
                        }

                        //clear the key, since PreviewKeyUp doesn't get called if a command was dispatched
                        _state.ModifierKey = ModifierKeyState.None;
                    }

                    e.Handled = true;
                }
            }

            _mouseDownAnchorPoint = null;
        }

        public override void PreprocessMouseDown(MouseButtonEventArgs e)
        {
            MouseButton button = e.ChangedButton;

            if (button.ToString().Equals("Middle"))
            {
                var position = RelativeToView(e.GetPosition(_view.VisualElement));
                var line = _view.TextViewLines.GetTextViewLineContainingYCoordinate(position.Y);

                if (line == null)
                    return;

                _view.Caret.MoveTo(line, position.X);

                _mouseDownAnchorPoint = RelativeToView(e.GetPosition(_view.VisualElement));
                IsSignificantElement(RelativeToView(e.GetPosition(_view.VisualElement)));
            }
        }

        private bool InDragOperation(Point anchorPoint, Point currentPoint)
        {
            return Math.Abs(anchorPoint.X - currentPoint.X) >= SystemParameters.MinimumHorizontalDragDistance &&
                   Math.Abs(anchorPoint.Y - currentPoint.Y) >= SystemParameters.MinimumVerticalDragDistance;
        }

        private Point RelativeToView(Point position)
            => new Point(position.X + _view.ViewportLeft, position.Y + _view.ViewportTop);

        private bool IsSignificantElement(Point position)
        {
            try
            {
                var line = _view.TextViewLines.GetTextViewLineContainingYCoordinate(position.Y);
                if (line == null)
                    return false;

                var bufferPosition = line.GetBufferPositionFromXCoordinate(position.X);
                if (!bufferPosition.HasValue)
                    return false;

                var extent = _navigator.GetExtentOfWord(bufferPosition.Value);
                if (!extent.IsSignificant || extent.Span.IsEmpty)
                    return false;

                // Exclude 'using' lines for C#
                if (_view.TextBuffer.ContentType.IsOfType("csharp"))
                {
                    string lineText = bufferPosition.Value.GetContainingLine().GetText().Trim();
                    if (lineText.StartsWith("using", StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                foreach (var classification in _aggregator.GetClassificationSpans(extent.Span))
                {
                    var name = classification.ClassificationType.Classification.ToLowerInvariant();
                    if (
                        name.Contains("identifier") ||
                        name.Contains("navigablesymbol") ||
                        name.Contains("usertype") || // handle both "user types" and "usertype"
                        (name.Contains("keyword") &&
                            IsAppropriateKeyword(classification.Span.GetText()) &&
                            _view.TextBuffer.ContentType.IsOfType("csharp"))
                        )
                    {
                        return true;
                    }
                }

                // Fallback: if the extent is significant and not whitespace, consider it significant
                if (!string.IsNullOrWhiteSpace(extent.Span.GetText()))
                    return true;

                return false;
            }
            catch
            {
                //There's no point in safely covering all the cases where the above might not be true.
                //if it's not what we're looking for, just call it insignificant
                return false;
            }
        }

        private readonly string[] _keywords = new[] {
            "base",
            "this",
            "bool",
            "byte",
            "sbyte",
            "char",
            "decimal",
            "double",
            "float",
            "int",
            "uint",
            "long",
            "ulong",
            "object",
            "short",
            "ushort",
            "string"
        };

        private bool IsAppropriateKeyword(string keyword)
        {
            for (int i = 0; i < _keywords.Length; i++)
                if (_keywords[i] == keyword)
                    return true;

            return false;
        }

        private void DispatchCommand(CommandSetting cmdSetting)
        {
            switch (cmdSetting)
            {
                case CommandSetting.GoToDefinition:
                    ExecuteCommand(VSConstants.GUID_VSStandardCommandSet97, (uint)VSConstants.VSStd97CmdID.GotoDefn);
                    break;

                case CommandSetting.PeekDefinition:
                    ExecuteCommand(VSConstants.CMDSETID.StandardCommandSet12_guid, (uint)VSConstants.VSStd12CmdID.PeekDefinition);
                    break;

                case CommandSetting.GoToImplementation:
                    //Go To Implementation doesn't seem to be a part of VSConstants. Had to find it the hard way
                    ExecuteCommand(new Guid("B61E1A20-8C13-49A9-A727-A0EC091647DD"), 512);
                    break;

                case CommandSetting.Nothing:
                    //nothing
                    break;
            }
        }

        private void ExecuteCommand(Guid cmdGroup, uint cmdId)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                _commandTarget.Exec(
                    ref cmdGroup,
                    cmdId,
                    (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
            catch
            {
                //if we're unable to dispatch, don't raise a stink.
            }
        }
    }
}
