﻿using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;

namespace MiddleClickDefinition
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

    public class EnumTypeConverter : EnumConverter
    {
        public EnumTypeConverter() : base(typeof(CommandSetting)) { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                if (str == CommandSettingDisplayName.Nothing) return CommandSetting.Nothing;
                if (str == CommandSettingDisplayName.PeekDefinition) return CommandSetting.PeekDefinition;
                if (str == CommandSettingDisplayName.GoToDefinition) return CommandSetting.GoToDefinition;
                if (str == CommandSettingDisplayName.GoToImplementation) return CommandSetting.GoToImplementation;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string result = null;
                if ((int)value == 0) result = CommandSettingDisplayName.Nothing;
                else if ((int)value == 1) result = CommandSettingDisplayName.PeekDefinition;
                else if ((int)value == 2) result = CommandSettingDisplayName.GoToDefinition;
                else if ((int)value == 3) result = CommandSettingDisplayName.GoToImplementation;

                if (result != null) return result;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
