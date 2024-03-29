﻿
namespace MiddleClickDefinition.Shared.Options
{
    public enum CommandSetting
    {
        Nothing,
        PeekDefinition,
        GoToDefinition,
        GoToImplementation
    }

    public static class CommandSettingDisplayName
    {
        public const string Nothing = "Nothing";
        public const string PeekDefinition = "Peek Definition";
        public const string GoToDefinition = "Go To Definition";
        public const string GoToImplementation = "Go To Implementation";
    }
}
