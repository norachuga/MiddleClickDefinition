using EnvDTE;
using MiddleClickDefinition.Shared.Keys;
using MiddleClickDefinition.Shared.Options;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace MiddleClickDefinition.Shared.Mouse
{
    [Export(typeof(IMouseProcessorProvider))]
    [ContentType("code")]
    [Name("MiddleClickDefinition")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
    [Order(Before = "WordSelection")]
    internal sealed class MouseProcessorFactory : IMouseProcessorProvider
    {
        [Import]
        private IClassifierAggregatorService _aggregatorFactory = null;

        [Import]
        private ITextStructureNavigatorSelectorService _navigatorService = null;

        [Import]
        private SVsServiceProvider _globalServiceProvider = null;

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            var buffer = view.TextBuffer;

            IOleCommandTarget shellCommandDispatcher = GetShellCommandDispatcher(view);

            if (shellCommandDispatcher == null)
                return null;

            return new GoToDefMouseHandler(
                view,
                shellCommandDispatcher,
                _aggregatorFactory.GetClassifier(buffer),
                _navigatorService.GetTextStructureNavigator(buffer),
                GetOptionsProcessor(),
                ModifierKeyStateProcessor.GetStateForView(view),
                _globalServiceProvider);
        }

        private IOleCommandTarget GetShellCommandDispatcher(ITextView view)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return _globalServiceProvider.GetService(typeof(SUIHostCommandDispatcher)) as IOleCommandTarget;
        }

        private OptionsProcessor GetOptionsProcessor()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            DTE env = (DTE)_globalServiceProvider.GetService(typeof(DTE));
            return new OptionsProcessor(env);
        }
    }
}
