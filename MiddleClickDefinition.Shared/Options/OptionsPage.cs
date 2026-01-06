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
        [DisplayName("Alt-Middleclick")]
        [Description("Controls which action is called for Alt-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting AltMiddleClickSetting { get; set; } = CommandSetting.FindReferences;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Alt-Shift-Middleclick")]
        [Description("Controls which action is called for Alt-Shift-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting AltShiftMiddleClickSetting { get; set; } = CommandSetting.Nothing;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Ctrl-Shift-Middleclick")]
        [Description("Controls which action is called for Ctrl-Shift-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting CtrlShiftMiddleClickSetting { get; set; } = CommandSetting.FindInFiles;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Ctrl-Alt-Middleclick")]
        [Description("Controls which action is called for Ctrl-Alt-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting CtrlAltMiddleClickSetting { get; set; } = CommandSetting.Nothing;

        [Category(MiddleclickActionsCategory)]
        [DisplayName("Ctrl-Alt-Shift-Middleclick")]
        [Description("Controls which action is called for Ctrl-Alt-Shift-Middleclick")]
        [TypeConverter(typeof(EnumTypeConverter))]
        public CommandSetting CtrlAltShiftMiddleClickSetting { get; set; } = CommandSetting.Nothing;
    }
}
