﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace MiddleClickDefinition.Shared.Options
{
    internal sealed class OptionsProcessor
    {
        private readonly DTE _envSvc;

        public OptionsProcessor(DTE envSvc)
        {
            _envSvc = envSvc;
        }

        public CommandSetting MiddleClick()
            => GetCommandSetting("MiddleClickSetting");

        public CommandSetting CtrlMiddleClick()
            => GetCommandSetting("CtrlMiddleClickSetting");

        public CommandSetting ShiftMiddleClick()
            => GetCommandSetting("ShiftMiddleClickSetting");

        public CommandSetting CtrlShiftMiddleClick()
            => GetCommandSetting("CtrlShiftMiddleClickSetting");

        public CommandSetting AltMiddleClick()
            => GetCommandSetting("AltMiddleClickSetting");

        public CommandSetting CtrlAltMiddleClick()
            => GetCommandSetting("CtrlAltMiddleClickSetting");

        public CommandSetting AltShiftMiddleClick()
            => GetCommandSetting("AltShiftMiddleClickSetting");

        public CommandSetting CtrlAltShiftMiddleClick()
            => GetCommandSetting("CtrlAltShiftMiddleClickSetting");

        private CommandSetting GetCommandSetting(string itemName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var props = _envSvc.get_Properties("MiddleClickDefinition", "General");
            var setting = props.Item(itemName).Value;
            return (CommandSetting)setting;
        }
    }
}
