using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Contrursor
{
    public static class Simulator
    {
        #region Mouse
        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public Int32 X;
            public Int32 Y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT point);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }

        //Use the values of this enum for the 'dwData' parameter
        //to specify an X button when using MouseEventFlags.XDOWN or
        //MouseEventFlags.XUP for the dwFlags parameter.
        public enum MouseEventDataXButtons : uint
        {
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        public enum MouseButton
        {
            Left,
            Middle,
            Right,
            None
        }

        public static MouseButton pressedMouseButton
        {
            get
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    return MouseButton.Left;
                } else if (Mouse.RightButton == MouseButtonState.Pressed)
                {
                    return MouseButton.Right;
                } else if (Mouse.MiddleButton == MouseButtonState.Pressed)
                {
                    return MouseButton.Middle;
                }
                return MouseButton.None;
            }
        }

        public static int MouseX
        {
            get
            {
                POINT pos = new POINT();
                GetCursorPos(out pos);
                return pos.X;
            }
            set
            {
                POINT pos = new POINT();
                GetCursorPos(out pos);
                SetCursorPos(value, pos.Y);
            }
        }

        public static int MouseY
        {
            get
            {
                POINT pos = new POINT();
                GetCursorPos(out pos);
                return pos.Y;
            }
            set
            {
                POINT pos = new POINT();
                GetCursorPos(out pos);
                SetCursorPos(pos.X, value);
            }
        }

        public static void MouseEvent(MouseEventFlags flags, int x, int y, MouseEventDataXButtons XButtons, int extraInfo = 0)
        {
            mouse_event((uint)flags, x, y, (uint)XButtons, extraInfo);
        }

        public static void MouseEvent(MouseEventFlags flags, int x, int y, uint XButtons, int extraInfo = 0)
        {
            mouse_event((uint)flags, x, y, XButtons, extraInfo);
        }

        public static void MouseEvent(MouseEventFlags flags, int x, int y, int XButtons, int extraInfo = 0)
        {
            mouse_event((uint)flags, x, y, (uint)XButtons, extraInfo);
        }
        #endregion

        #region Keys
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;

        public static void PressKey(byte keyCode)
        {
            keybd_event(keyCode, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(keyCode, 0x45, KEYEVENTF_KEYUP, 0);
        }

        public static void keyDown(byte keyCode)
        {
            keybd_event(keyCode, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void keyUp(byte keyCode)
        {
            keybd_event(keyCode, 0x45, KEYEVENTF_KEYUP, 0);
        }
        #endregion
    }
}
