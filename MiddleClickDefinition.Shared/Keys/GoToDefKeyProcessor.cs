using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Input;

namespace MiddleClickDefinition.Shared.Keys
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

            if ((args.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
            {
                state |= ModifierKeyState.Ctrl;
            }

            if ((args.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0)
            {
                state |= ModifierKeyState.Shift;
            }

            if ((args.KeyboardDevice.Modifiers & ModifierKeys.Alt) != 0)
            {
                state |= ModifierKeyState.Alt;
            }

            _state.ModifierKey = state;
        }


    }
}
