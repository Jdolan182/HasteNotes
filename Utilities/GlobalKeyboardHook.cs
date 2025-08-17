using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HasteNotes.Utilities
{
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    public class GlobalKeyboardHook
    {
        #region Constant, Structure and Delegate Definitions
        public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        public struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12;
        private const int VK_CAPITAL = 0x14;
        #endregion

        #region Instance Variables
        public List<Keys> HookedKeys = new List<Keys>();
        private nint hhook = nint.Zero;
        #endregion

        #region Events
        public event KeyEventHandler? KeyDown;
        public event KeyEventHandler? KeyUp;
        #endregion

        #region Constructors and Destructor
        public GlobalKeyboardHook()
        {
            Hook();
        }

        ~GlobalKeyboardHook()
        {
            Unhook();
        }
        #endregion

        #region Public Methods
        public void Hook()
        {
            nint hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, HookProc, hInstance, 0);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        public int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                if (HookedKeys.Contains(key))
                {
                    key = AddModifiers(key);
                    KeyEventArgs kea = new KeyEventArgs(key);

                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && KeyDown != null)
                        KeyDown(this, kea);

                    if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && KeyUp != null)
                        KeyUp(this, kea);

                    if (kea.Handled)
                        return 1;
                }
            }

            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }

        private Keys AddModifiers(Keys key)
        {
            if ((GetKeyState(VK_CAPITAL) & 0x0001) != 0) key |= Keys.CapsLock;
            if ((GetKeyState(VK_SHIFT) & 0x8000) != 0) key |= Keys.Shift;
            if ((GetKeyState(VK_CONTROL) & 0x8000) != 0) key |= Keys.Control;
            if ((GetKeyState(VK_MENU) & 0x8000) != 0) key |= Keys.Alt;

            return key;
        }
        #endregion

        #region DLL Imports
        [DllImport("user32.dll")]
        private static extern nint SetWindowsHookEx(int idHook, KeyboardHookProc callback, nint hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(nint hInstance);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(nint idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        [DllImport("kernel32.dll")]
        private static extern nint LoadLibrary(string lpFileName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern short GetKeyState(int keyCode);
        #endregion
    }
}
