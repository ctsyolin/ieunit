/// <copyright from="2004" to="2006" company="VisuMap Technologies Inc.">
///   Copyright (C) VisuMap Technologies Inc.
/// 
///   Permission to use, copy, modify, distribute and sell this 
///   software and its documentation for any purpose is hereby 
///   granted without fee, provided that the above copyright notice 
///   appear in all copies and that both that copyright notice and 
///   this permission notice appear in supporting documentation. 
///   VisuMap Technologies Company makes no representations about the 
///   suitability of this software for any purpose. It is provided 
///   "as is" without explicit or implied warranty. 
/// </copyright>
using System;
using System.Runtime.InteropServices;

namespace QuickFocus {
    public sealed class Win32Interop {
        public const uint EM_SETMARGINS  = 0x00D3;
        public const uint EC_LEFTMARGIN  = 0x0001;
        public const uint EC_RIGHTMARGIN = 0x0002;

        private Win32Interop() {}
        [DllImport( "user32.dll" )]
        public static extern IntPtr SendMessage( IntPtr handleToWindow, uint message, uint wParam, int lParam );
    }
}
