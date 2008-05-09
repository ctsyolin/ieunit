#region Copyright (c) 2004-2005, VisuMap Technologies Inc.

/********************************************************************************************************************
'
' Copyright (c) 2004-2005, VisuMap Technologies Inc.
' All rights reserved.
' 
' Redistribution and use in source and binary forms, with or without modification, are permitted provided
' that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list of conditions and the
' 	following disclaimer.
' 
' * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
' 	the following disclaimer in the documentation and/or other materials provided with the distribution.
' 
' * Neither the name of the author nor the names of its contributors may be used to endorse or 
' 	promote products derived from this software without specific prior written permission.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
' WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
' PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
' ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
' INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
' OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
' IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'
'*******************************************************************************************************************/
#endregion

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Win32Dom
{
    internal enum GwType {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST  = 1,
        GW_HWNDNEXT  = 2,
        GW_HWNDPREV  = 3,
        GW_OWNER     = 4,
        GW_CHILD     = 5,
        GW_MAX       = 5
    }

    internal enum WinMsg {
        WM_CLOSE        = 0x10,
        WM_DESTORY      = 0x02,
        WM_MENUSELECT   = 0x011f,
        BM_CLICK        = 0x00F5,
        CB_INSERTSTRING = 0x014A,
        WM_SETTEXT      = 0x000C,
        WM_GETTEXT      = 0x000D,
        WM_GETTEXTLENGTH= 0x000E,
        WM_SYSKEYDOWN   = 0x0104,
        WM_SYSKEYUP     = 0x0105,
        WM_SYSCHAR      = 0x0107,
        WM_KEYDOWN      = 0x0100,
        WM_KEYUP        = 0x0101,
        WM_CHAR         = 0x0102
    }

    internal enum VirtualKey {
        VK_BACK     = 0x08,
        VK_TAB      = 0x08,
        VK_CLEAR    = 0x0C,
        VK_RETURN   = 0x0D,
        VK_SHIFT    = 0x10,
        VK_CONTROL  = 0x11,
        VK_MENU     = 0x12,
        VK_PAUSE    = 0x13,
        VK_CAPITAL  = 0x14,
        VK_ESCAPE   = 0x1B,
        VK_SPACE    = 0x20
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class RECT {
        internal int left;
        internal int top;
        internal int right;
        internal int bottom;

        internal RECT() {
            left    = 0;
            top     = 0;
            right   = 0;
            bottom  = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential )]
    internal struct POINT {
        internal int x;
        internal int y;
        
        internal POINT(int x, int y) {
            this.x = x;
            this.y = y;
        }
    };


    internal struct MouseInput {
        internal const int INPUT_MOUSE = 0;
        internal const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        internal const int MOUSEEVENTF_LEFTDOWN = 0x2;
        internal const int MOUSEEVENTF_LEFTUP = 0x4;
        internal const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        internal const int MOUSEEVENTF_MIDDLEUP = 0x40;
        internal const int MOUSEEVENTF_MOVE = 0x1;
        internal const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        internal const int MOUSEEVENTF_RIGHTUP = 0x10;
        internal const int MOUSEEVENTF_WHEEL = 0x80;
        internal const int MOUSEEVENTF_XDOWN = 0x100;
        internal const int MOUSEEVENTF_XUP = 0x200;
        internal const int WHEEL_DELTA = 120;
        internal const int XBUTTON1 = 0x1;
        internal const int XBUTTON2 = 0x2;


        public MouseInput( int mouseEvent ) {
            type = INPUT_MOUSE;
            dx = 0;
            dy = 0;
            mouseData = 0;
            dwFlags = mouseEvent;
            time = 0;
            dwExtraInfo = 0;
        }

        internal int type;
        internal int dx;
        internal int dy;
        internal int mouseData;
        internal int dwFlags;
        internal int time;
        internal int dwExtraInfo;
    };

	/// <summary>
	/// Summary description for Win32Api.
	/// </summary>
    internal class Win32Api {

        [DllImport("User32.dll",EntryPoint="FindWindow")]
        private static extern int FindWindow_(string lpClassName, string lpWindowName);

        /// <summary>
        /// Find a top level window with given caption
        /// </summary>
        /// <param name="className">The window class name</param>
        /// <param name="winCaption">The Window's caption</param>
        /// <returns></returns>
        internal static IntPtr FindWindow(string className, string winCaption) {
            return new IntPtr( FindWindow_( className, winCaption) );
        }

        /// <summary>
        /// Convert a point from client coordinator to screen coordinator.
        /// </summary>
        /// <param name="hWnd">client window handle</param>
        /// <param name="lpPoint">the point to be converted</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        /// <summary>
        /// Convert a point from screen coordinator to client coordinator.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        /// <summary>
        /// The the current cursor position.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool SetCursorPos(int X, int Y);

        [DllImport("User32.dll",EntryPoint="GetWindow")]
        private static extern int GetWindow_(IntPtr hWnd, uint uCmd);
        
        /// <summary>
        /// Get a window with given relationship to given window
        /// </summary>
        /// <param name="hWnd">Handle of the reference window</param>
        /// <param name="gwType">Relationship to the reference window</param>
        /// <returns>Handle for the window found or IntPtr.ZERO </returns>
        internal static IntPtr GetWindow(IntPtr hWnd, GwType gwType) {
            return new IntPtr( GetWindow_( hWnd, (uint) gwType) );
        }

        [DllImport("User32.dll",EntryPoint="FindWindowEx")]
        private static extern int FindWindowEx_(IntPtr hParent, IntPtr hAfterChild, string lpClassName, string lpWindowName);

        /// <summary>
        /// Find a window object within given window
        /// </summary>
        /// <param name="parent">The parent window handler</param>
        /// <param name="afterChild">The child window for starting the search.</param>
        /// <param name="className">The class name or null.</param>
        /// <param name="winCaption">The window name or null.</param>
        internal static IntPtr FindWindowEx(IntPtr parent, IntPtr afterChild, string className, string winCaption) {
            return new IntPtr( FindWindowEx_(parent, afterChild, className, winCaption) );
        }


        [DllImport("User32.dll",EntryPoint="GetWindowText")]
        private static extern int GetWindowText_(IntPtr hWin, StringBuilder windowText, int maxSize);

        /// <summary>
        /// Return a window's text component.
        /// </summary>
        /// <param name="win">The handle to the window</param>
        /// <param name="maxSize">The buffer size to receive the text</param>
        /// <returns>Window's text</returns>
        internal static string GetWindowText(IntPtr win, int maxSize) {
            StringBuilder buffer = new StringBuilder(maxSize);
            if ( GetWindowText_(win, buffer, maxSize) > 0) {
                return buffer.ToString();
            } else {
                return "";
            }
        }

        internal static string GetText(IntPtr win) {
            int len = SendMessage(win, WinMsg.WM_GETTEXTLENGTH, 0, 0).ToInt32();
            if ( len <= 0 ) {
                return "";
            }
            StringBuilder buffer = new StringBuilder(len+1);                

            int gotLen = SendMessage(win, WinMsg.WM_GETTEXT, (ushort)(len+1), buffer).ToInt32();
            return ( gotLen == len ) ? buffer.ToString() : "";
        }

        /// <summary>
        /// Retuern the text component of a window upto size 1024
        /// </summary>
        /// <param name="win">Handle to the window</param>
        /// <returns>Window's text of emptyr string</returns>
        internal static string GetWindowText(IntPtr win) {
            return GetWindowText(win, 10240);
        }

        [DllImport("User32.dll",EntryPoint="GetClassName")]
        private static extern int GetClassName_(IntPtr hWin, StringBuilder className, int maxSize);
        
        /// <summary>
        /// Get the window's class name
        /// </summary>
        /// <param name="win">Handle for the window</param>
        /// <returns>The window's class name</returns>
        internal static string GetClassName(IntPtr win) {
            StringBuilder buffer = new StringBuilder(128);
            if ( GetClassName_(win, buffer, 128) > 0 ) {
                return buffer.ToString();
            } else {
                return "";
            }
        }

        /// <summary>
        /// Post a windows message to given window object.
        /// </summary>
        /// <param name="hWnd">The handle of the target widnow.</param>
        /// <param name="Msg">The message code</param>
        /// <param name="lParam">The message parameter</param>
        /// <param name="wParam">The second message parameter</param>
        [DllImport("user32.dll",EntryPoint="PostMessage")]
        [return:MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage_(IntPtr hWnd, 
            [MarshalAs(UnmanagedType.U4)] uint Msg, IntPtr wParam, IntPtr lParam);

        internal static bool PostMessage(IntPtr win, WinMsg msg) {
            return PostMessage_(win, (uint)msg, new IntPtr(0), new IntPtr(0) );
        }

        [DllImport("USER32.DLL", EntryPoint= "SendMessage")]
        private static extern IntPtr SendMessage_(IntPtr hwnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        [DllImport("USER32.DLL", EntryPoint= "SendMessage")]
        internal static extern IntPtr SendMessage(IntPtr hwnd, WinMsg msg, ushort wParam, uint lParam);

        [DllImport("USER32.DLL", EntryPoint= "SendMessage")]
        internal static extern IntPtr SendMessage(IntPtr hwnd, WinMsg msg, ushort wParam, StringBuilder strBuffer);


        /// <summary>
        /// Send a Windows message.
        /// </summary>
        /// <param name="win">The target window handler</param>
        /// <param name="msg">The message code</param>
        /// <param name="lParam">The message parameter</param>
        /// <param name="wParam">The second message parameter</param>
        internal static IntPtr SendTextMessage(IntPtr win, WinMsg msg, [MarshalAs(UnmanagedType.LPStr)] string lParam) {
            return SendMessage_(win, (int)msg, IntPtr.Zero, lParam);
        }

        [DllImport("User32.dll",EntryPoint="GetWindowLong")]
        private static extern int GetWindowLong_(IntPtr hWin, int idx);

        [DllImport("User32.dll",EntryPoint="SetWindowLong")]
        private static extern int SetWindowLong_(IntPtr hWin, int idx, int style);

        public delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// Get the window style
        /// </summary>
        /// <param name="hWin"></param>
        /// <returns></returns>
        internal static int GetWindowStyle(IntPtr hWin) {
            int GWL_STYLE = -16;
            return GetWindowLong_(hWin, GWL_STYLE);
        }

        internal static int SetWindowStyle(IntPtr hWin, int style) {
            int GWL_STYLE = -16;
            return SetWindowLong_(hWin, GWL_STYLE, style);
        }

        [DllImport("user32.dll",EntryPoint="GetWindowRect")]
        [return:MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect_(IntPtr hWnd, 
            [MarshalAs(UnmanagedType.LPStruct)] RECT rect);


        /// <summary>
        /// Returns the rectangle geometry information of a window.
        /// </summary>
        /// <param name="win"></param>
        /// <returns></returns>
        internal static RECT GetWindowRect(IntPtr win) {
            RECT rect = new RECT();
            if ( GetWindowRect_(win, rect) ) {
                return rect;
            } else {
                return null;
            }
        }

        [StructLayout( LayoutKind.Explicit, Size=28 )]
        internal struct KeyInput {
            [FieldOffset( 0 )] internal int type;
            [FieldOffset( 4 )] internal short wVk;
            [FieldOffset( 6 )] internal short wScan;
            [FieldOffset( 8 )] internal int dwFlags;
            [FieldOffset( 12 )] internal int time;
            [FieldOffset( 16 )] internal int dwExtraInfo;
        }

        [DllImport( "user32.dll", EntryPoint="SendInput", SetLastError=true )]
        internal static extern int SendInput( int cInputs, ref KeyInput pInputs, int cbSize );

        [DllImport( "user32.dll", EntryPoint="SendInput", SetLastError=true )]
        internal static extern int SendMouseInput( int cInputs, ref MouseInput pInputs, int cbSize );

        internal static void DownKey(IntPtr win,  short key){
            KeyInput ki = new KeyInput();
            ki.type = 1;
            ki.dwExtraInfo = 0;
            ki.dwFlags = 0;
            ki.time = 0;
            ki.wScan = 0;
            ki.wVk = key;
            SendInput( 1, ref ki, Marshal.SizeOf( ki ) );
        }

        internal static void UpKey(IntPtr win,  short key){
            KeyInput ki = new KeyInput();
            ki.type = 1;
            ki.dwExtraInfo = 0;
            ki.dwFlags = 0x0002;
            ki.time = 0;
            ki.wScan = 0;
            ki.wVk = key;
            SendInput( 1, ref ki, Marshal.SizeOf( ki ) );
        }

        internal static void PressKey(IntPtr win, short key) {
            DownKey(win, key);
            UpKey(win, key);
        }

        internal static void SendChar(IntPtr win,  char chr){
            SendMessage(win, WinMsg.WM_CHAR, chr, 0);
        }

        /// <summary>
        /// Close a window
        /// </summary>
        /// <param name="win">The handle to a window</param>
        internal static void CloseWindow(IntPtr win) {
            PostMessage(win, WinMsg.WM_CLOSE);
        }


        //
        // The following APIs helps to attach to a modal IE window.
        //

        internal const uint SMTO_ABORTIFHUNG = 0x0002;
        
        [DllImport("user32.dll")]
        internal static extern int RegisterWindowMessage(string msg);
        
        [DllImport("USER32.DLL")]
        internal static extern IntPtr SendMessageTimeout(IntPtr hwnd, int msg, int wParam, 
            int lParam, uint fFlags, uint uTimeout, ref int pResult);
        
        [DllImport("oleacc.dll", PreserveSig=false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        internal static extern object ObjectFromLresult(
            int lResult,[In] ref Guid refiid, int wParam, 
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppvObject);
    }
}
