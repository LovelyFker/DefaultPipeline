using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class SimulateMouse
{
    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    //下面这个枚举也来自user32.dll
    [Flags]
    enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000
    }

    void Test()
    {
        //模拟键盘输入
        SendKeys.Send("{BACKSPACE}");
    }

    public static void MouseLeftClick(int x, int y)
    {
        SetCursorPos(x, y);
        mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
        mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
    }

    public static void MouseRightClick(int x, int y)
    {
        SetCursorPos(x, y);
        mouse_event(MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
        mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
    }
}
