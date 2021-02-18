using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace MiddleClickDefinition
{
    [Export(typeof(IKeyProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("code")]
    [Name("MiddleClickDefinition")]
    [Order(Before = "VisualStudioKeyboardProcessor")]
    internal sealed class KeyboardProcessorFactory : IKeyProcessorProvider
    {
        public KeyProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(
                typeof(GoToDefKeyProcessor),
                () => new GoToDefKeyProcessor(ModifierKeyStateProcessor.GetStateForView(view)));
        }
    }

}
