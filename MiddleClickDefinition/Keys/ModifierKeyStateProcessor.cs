using Microsoft.VisualStudio.Text.Editor;

namespace MiddleClickDefinition
{
    internal sealed class ModifierKeyStateProcessor
    {
        internal static ModifierKeyStateProcessor GetStateForView(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(typeof(ModifierKeyStateProcessor), () => new ModifierKeyStateProcessor());
        }

        internal ModifierKeyState ModifierKey { get; set; } = ModifierKeyState.None;
    }

}
