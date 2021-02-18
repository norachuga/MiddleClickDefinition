using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Input;

namespace MiddleClickDefinition
{
    internal sealed class GoToDefKeyProcessor : KeyProcessor
    {
        private readonly ModifierKeyStateProcessor _state;

        public GoToDefKeyProcessor(ModifierKeyStateProcessor state)
        {
            _state = state;
        }

        public override void PreviewKeyDown(KeyEventArgs args)
        {
            UpdateState(args);
        }

        public override void PreviewKeyUp(KeyEventArgs args)
        {
            UpdateState(args);
        }

        private void UpdateState(KeyEventArgs args)
        {
            ModifierKeyState state = ModifierKeyState.None;

            bool ctrlDown = (args.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;
            bool shiftDown = (args.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0;

            if (ctrlDown && shiftDown)
            {
                state = ModifierKeyState.CtrlShift;
            }
            else
            {
                if (ctrlDown)
                {
                    state = ModifierKeyState.Ctrl;
                }

                if (shiftDown)
                {
                    state = ModifierKeyState.Shift;
                }
            }

            _state.ModifierKey = state;
        }


    }

}
