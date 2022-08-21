
namespace MiddleClickDefinition.Shared.Keys
{
    internal enum ModifierKeyState
    {
        None = 0b000,
        Shift = 0b001,
        Ctrl = 0b010,
        Alt = 0b100,
        CtrlShift = Ctrl | Shift,
        AltShift = Alt | Shift,
        CtrlAlt = Ctrl | Alt,
        CtrlAltShift = Ctrl | Shift | Alt,
    }
}
