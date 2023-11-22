using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WindowsFormsApp2
{
    /// <summary>
    /// 키보드 이벤트 열거형
    /// </summary>
    [Flags]
    public enum KeyFlag
    {
        /// <summary>
        /// 키 누름
        /// </summary>
        KE_DOWN = 0,
        /// <summary>
        /// 확장 키
        /// </summary>
        KE__EXTENDEDKEY = 1,
        /// <summary>
        /// 키 뗌
        /// </summary>
        KE_UP = 2
    }

    [Flags]
    public enum MouseFlag
    {
        /// <summary>
        /// 마우스 이동
        /// </summary>
        ME_MOVE = 1,
        /// <summary>
        /// 마우스 왼쪽 버튼 누름
        /// </summary>
        ME_LEFTDOWN = 2,
        /// <summary>
        /// 마우스 왼쪽 버튼 뗌
        /// </summary>
        ME_LEFTUP = 4,
        /// <summary>
        /// 마우스 오른쪽 버튼 누름
        /// </summary>
        ME_RIGHTDOWN = 8,
        /// <summary>
        /// 마우스 오른쪽 버튼 뗌
        /// </summary>
        ME_RIGHTUP = 0x10,
        /// <summary>
        /// 마우스 가운데 버튼 누름
        /// </summary>
        ME_MIDDLEDOWN = 0x20,
        /// <summary>
        /// 마우스 가운데 버튼 뗌
        /// </summary>
        ME_MIDDLEUP = 0x40,
        /// <summary>
        /// 마우스 휠
        /// </summary>
        ME_WHEEL = 0x800,
        /// <summary>
        /// 마우스 절대 이동
        /// </summary>
        ME_ABSOULUTE = 8000
    }


    /// <summary>
    /// Native(Win32 API) 기술 래핑 클래스(정적 클래스)
    /// </summary>
    public static class WrapNative
    {
        //static Mutex mu = new Mutex();        

        [DllImport("user32.dll")]
        static extern void mouse_event(int flag, int dx, int dy, int buttons, int extra);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point point);
        [DllImport("user32.dll")]
        static extern int SetCursorPos(int x, int y);
        [DllImport("User32.dll")]
        static extern void keybd_event(byte vk, byte scan, int flags, int extra);

        /// <summary>
        /// 키 누름(DOWN) 이벤트를 발생시키는 메서드
        /// </summary>
        /// <param name="keycode">키</param>
        public static void KeyDown(int keycode)
        {
            keybd_event((byte)keycode, 0, (int)KeyFlag.KE_DOWN, 0);
        }
        /// <summary>
        /// 키 뗌(UP) 이벤트를 발생시키는 메서드
        /// </summary>
        /// <param name="keycode">키</param>
        public static void KeyUp(int keycode)
        {
            keybd_event((byte)keycode, 0, (int)KeyFlag.KE_UP, 0);
        }
        /// <summary>
        /// 마우스 좌표를 바꾸는 메서드
        /// </summary>
        /// <param name="x">바꿀 X 좌표</param>
        /// <param name="y">바꿀 Y 좌표</param>
        public static void Move(int x, int y)
        {
            SetCursorPos(x, y);
        }
        /// <summary>
        /// 마우스 좌표를 바꾸는 메서드
        /// </summary>
        /// <param name="pt">바꿀 포인트</param>
        public static void Move(Point pt)
        {
            Move(pt.X, pt.Y);
        }
        /// <summary>
        /// 프로그램 방식으로 마우스 왼쪽 버튼 누름 이벤트 발생시키는 메서드
        /// </summary>
        public static void LeftDown()
        {
            mouse_event((int)MouseFlag.ME_LEFTDOWN, 0, 0, 0, 0);
        }
        /// <summary>
        /// 프로그램 방식으로 마우스 왼쪽 버튼 뗌 이벤트 발생시키는 메서드
        /// </summary>
        public static void LeftUp()
        {
            mouse_event((int)MouseFlag.ME_LEFTUP, 0, 0, 0, 0);
        }
    }
}