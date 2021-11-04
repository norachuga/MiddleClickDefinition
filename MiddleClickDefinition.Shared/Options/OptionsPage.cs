using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;

namespace MiddleClickDefinition.Shared.Options
{
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public class OptionsPage : DialogPage
    {
        private const string MiddleclickActionsCategory = "Middleclick Actions";

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Middleclick")]
        [Description("Controls which action is called for Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting MiddleClickSetting { get; set; } = CommandSetting.GoToDefinition;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Ctrl-Middleclick")]
        [Description("Controls which action is called for Ctrl-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting CtrlMiddleClickSetting { get; set; } = CommandSetting.GoToImplementation;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Shift-Middleclick")]
        [Description("Controls which action is called for Shift-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting ShiftMiddleClickSetting { get; set; } = CommandSetting.PeekDefinition;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Ctrl-Shift-Middleclick")]
        [Description("Controls which action is called for Ctrl-Shift-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting CtrlShiftMiddleClickSetting { get; set; } = CommandSetting.Nothing;
    }
}
