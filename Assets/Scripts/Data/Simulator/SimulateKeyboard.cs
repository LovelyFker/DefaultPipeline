using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UnityEngine;
using System;
using System.Linq;

public class SimulateKeyboard
{
    /// <summary>
    /// 模拟键盘按键信息
    /// </summary>
    /// <param name="bVk">键盘按键虚拟键码:1~254</param>
    /// <param name="bScan">定义该键的硬件扫描码，一般为0</param>
    /// <param name="dwFlags">0为按下，1为按住，2为释放</param>
    /// <param name="dwExtraInfo">定义与击键相关的附加的32位值，一般为0</param>
    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    [Serializable]
    public struct SimulatedKey
    {
        public KeyCode keyCode;
        public Keys key;
        public bool IsPressed;
    }

    //Key映射
    public static SimulatedKey[] keys =
    {
        new SimulatedKey { keyCode = KeyCode.W, key = Keys.W, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.A, key = Keys.A, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.S, key = Keys.S, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.D, key = Keys.D, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Space, key = Keys.Space, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Escape, key = Keys.Escape, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Return, key = Keys.Enter, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.LeftControl, key = Keys.LControlKey, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.LeftShift, key = Keys.LShiftKey, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.N, key = Keys.N, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.R, key = Keys.R, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Alpha1, key = Keys.D1, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Alpha2, key = Keys.D2, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Alpha3, key = Keys.D3, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Alpha4, key = Keys.D4, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.Alpha5, key = Keys.D5, IsPressed = false },
        new SimulatedKey { keyCode = KeyCode.C, key = Keys.C, IsPressed = false }
    };

    private static Keys GetKeys(KeyCode keyCode)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].keyCode == keyCode)
                return keys[i].key;
        }

        Debug.LogWarning("找不到该键值映射");
        return Keys.None;
    }

    private static bool GetIsPressed(KeyCode keyCode)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].keyCode == keyCode)
            {
                if (keys[i].IsPressed)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    private static void SetPressed(KeyCode keyCode, bool value)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].keyCode == keyCode)
            {
                keys[i].IsPressed = value;
            }
        }
    }

    /// <summary>
    /// 模拟按住按键
    /// </summary>
    /// <param name="key"></param>
    public static void KeyPress(KeyCode keyCode)
    {
        if (!GetIsPressed(keyCode))
        {
            SetPressed(keyCode, true);
            keybd_event(GetKeys(keyCode), 0, 1, 0);
        }
    }

    /// <summary>
    /// 模拟按下按键
    /// </summary>
    /// <param name="key"></param>
    public static void KeyDown(KeyCode keyCode)
    {
        keybd_event(GetKeys(keyCode), 0, 0, 0);
        keybd_event(GetKeys(keyCode), 0, 2, 0);
    }

    /// <summary>
    /// 模拟长按松开按键
    /// </summary>
    /// <param name="key"></param>
    public static void KeyUp(KeyCode keyCode)
    {
        if (GetIsPressed(keyCode))
        {
            SetPressed(keyCode, false);
            keybd_event(GetKeys(keyCode), 0, 2, 0);
        }
    }
}
