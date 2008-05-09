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
using System.Windows.Forms;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using mshtml;
using System.Text.RegularExpressions;

namespace Win32Dom
{
    public class Desktoop {
        public Desktoop() {
        }

        /// <summary>
        /// Find the first top window whose title contains given string.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Window findTopWindow(string title) {
            /*
            IntPtr winHandle = Win32Api.FindWindow(null, title);
            if ( winHandle != IntPtr.Zero ) {
                Window win = new Window( winHandle, null );
                return win;
            } else {
                return null;
            }
            */

            FindWindowByRegExp proc = new FindWindowByRegExp(".*" + title + ".*");

            Win32Api.EnumChildWindows(Win32Api.GetDesktopWindow(),
                new Win32Api.WindowEnumProc(proc.OnEnumWindow), IntPtr.Zero);

            if (proc.winPtr != IntPtr.Zero) {
                return new Window(proc.winPtr, null);
            } else {
                return null;
            }
        }


        public ArrayList findAllTopWindows() {
            ArrayList winList = new ArrayList();
            IntPtr firstWin = Win32Api.FindWindow(null, null);
            if ( firstWin != IntPtr.Zero ) {
                winList.Add( new Window( firstWin, null ) );;
            }
            IntPtr nextWin = firstWin;
            while (true) {
                nextWin = Win32Api.GetWindow(nextWin, GwType.GW_HWNDNEXT);
                if ( nextWin == IntPtr.Zero ) {
                    break;
                } else {
                    winList.Add( new Window( nextWin, null ) );                    
                }
            }
            return winList;
        }
    }

    /// <summary>
    /// Help class to traverse desktop windows
    /// </summary>
    class FindWindowByRegExp {
        private Regex regExp;
        public IntPtr winPtr;
        public FindWindowByRegExp(string pattern) {
            regExp = new Regex(pattern);
            winPtr = IntPtr.Zero;
        }
        public int OnEnumWindow(IntPtr hwnd, IntPtr lParam) {
            string title = Win32Api.GetWindowText(hwnd, 256);
            if ((!string.IsNullOrEmpty(title)) && (regExp.IsMatch(title))) {
                winPtr = hwnd;
                return 0;
            }
            return 1;
        }
    }


	public class Window
	{
        public IntPtr        winHandle;
        public XmlElement    xmlElement;

        /// <summary>
        /// Construct a window object
        /// </summary>
        /// <param name="winHandle"></param>
        /// <param name="xmlElement"></param>
        public Window(IntPtr winHandle, XmlElement xmlElement) {
            this.winHandle = winHandle;
            this.xmlElement = xmlElement;
            if ( xmlElement == null ) {
                syncronizeDomTree();                                                                             
            }
        }

        #region Navigation methods
        /// <summary>
        /// Synchronize the window object's DOM tree with the current status.
        /// </summary>
        public void syncronizeDomTree() {
            XmlDocument   xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\"?><Win32Dom/>");
            LoadDomTree(xmlDoc.DocumentElement, winHandle);
            xmlElement = (XmlElement) xmlDoc.DocumentElement.FirstChild;
        }

        private void LoadDomTree(XmlElement e, IntPtr win) {
            XmlElement parent = e.OwnerDocument.CreateElement("Win");
            parent.SetAttribute("Hnd", win.ToString());
            parent.SetAttribute("Cls", Win32Api.GetClassName(win));
            parent.SetAttribute("Cap", Win32Api.GetWindowText(win)); 
            parent.SetAttribute("Text",Win32Api.GetText(win)); 
            parent.SetAttribute("Sty", Win32Api.GetWindowStyle(win).ToString("X")); 
            
            RECT rect = Win32Api.GetWindowRect(win);
            parent.SetAttribute("x", rect.left.ToString());
            parent.SetAttribute("y", rect.top.ToString());
            parent.SetAttribute("w", (rect.right-rect.left).ToString());
            parent.SetAttribute("h", (rect.bottom-rect.top).ToString());

            e.AppendChild( parent );

            // append all child elements
            for (
                    IntPtr childWin = Win32Api.GetWindow(win, GwType.GW_CHILD);
                    childWin != IntPtr.Zero;
                    childWin = Win32Api.GetWindow(childWin, GwType.GW_HWNDNEXT) ) {
                LoadDomTree(parent, childWin);
            }
        }

        /// <summary>
        /// Return the window's DOM tree as an string
        /// </summary>
        /// <returns></returns>
        public string getDomTree() {
            StringWriter strWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(strWriter);
            xmlWriter.Formatting = Formatting.Indented;
            xmlElement.WriteTo( xmlWriter );
            return strWriter.ToString();
        }

        /// <summary>
        /// Find the first window satifying given xpath condition.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public Window findWindow(string xpath) {
            XmlElement e = (XmlElement) xmlElement.SelectSingleNode(xpath); 
            if ( e == null ) {
                return null;
            }
            int hnd = Int32.Parse ( e.GetAttribute("Hnd") );
            return new Window( new IntPtr( hnd ), e );
        }

        /// <summary>
        /// Find all windows  satifying given xpath condition.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public ArrayList findWindowList(string xpath) {
            ArrayList wins = new ArrayList();
            XmlNodeList nodes = xmlElement.SelectNodes(xpath); 

            for(int idx=0; idx<nodes.Count; idx++) {
                XmlElement  e   = (XmlElement)nodes[idx];
                int         hnd = Int32.Parse ( e.GetAttribute("Hnd") );
                wins.Add( new Window( new IntPtr( hnd ), e) );
            }

            return wins;
        }

        /// <summary>
        /// Close the window.
        /// </summary>
        public void close() {
            Win32Api.CloseWindow( winHandle );
        }

        #endregion


        #region Keyboard methods
        /// <summary>
        /// Send a key to the window.
        /// </summary>
        /// <param name="key">the key value</param>
        public void pressKey(short key) {
            Win32Api.PressKey(winHandle, key);
            Application.DoEvents();
        }

        public void downKey(short key) {
            Win32Api.DownKey(winHandle, key);
            Application.DoEvents();
        }

        public void upKey(short key) {
            Win32Api.UpKey(winHandle, key);
            Application.DoEvents();
        }

        /// <summary>
        /// Send text to the window
        /// </summary>
        /// <param name="text"></param>
        public void sendText(string text) {
            for(int i=0; i<text.Length; i++) {
                Win32Api.SendChar(winHandle, text[i]);
            }
        }
        #endregion

        
        #region basic window's properties.
        /// <summary>
        /// Get or set the window's text (or caption).
        /// </summary>
        public string caption {
            get { return Win32Api.GetWindowText(winHandle); }
            set { Win32Api.SendTextMessage(winHandle, WinMsg.WM_SETTEXT, value);  }
        }

        public string text {
            get {  
                return  Win32Api.GetText(winHandle);
            }
            set { Win32Api.SendTextMessage(winHandle, WinMsg.WM_SETTEXT, value);  }
        }

        /// <summary>
        /// Return the window's class name.
        /// </summary>
        public string className {
            get { return Win32Api.GetClassName( winHandle ); }
        }

        /// <summary>
        /// Return window's style.
        /// </summary>
        public int winStyle {
            get { return (int) Win32Api.GetWindowStyle( winHandle ); }
            set { Win32Api.SetWindowStyle( winHandle, value ); }
        }

        /// <summary>
        /// return the window's top coordinate
        /// </summary>
        public int top {
            get {
                RECT rect = Win32Api.GetWindowRect(winHandle);
                return rect.top;
            }
        }

        /// <summary>
        /// return the window's left coordinate
        /// </summary>
        public int left {
            get {
                RECT rect = Win32Api.GetWindowRect(winHandle);
                return rect.left;
            }
        }

        /// <summary>
        /// return the window's width
        /// </summary>
        public int width {
            get {
                RECT rect = Win32Api.GetWindowRect(winHandle);
                return rect.right - rect.left;
            }
        }

        /// <summary>
        /// return the window's height
        /// </summary>
        public int height {
            get {
                RECT rect = Win32Api.GetWindowRect(winHandle);
                return rect.bottom - rect.top;
            }
        }
        #endregion

        #region Mouse controls

        /// <summary>
        /// Move the mouse button to a point within a window object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool MouseMoveTo(int x, int y) {
            Win32Dom.POINT p = new POINT(x, y);
            
            if ( ! Win32Api.ClientToScreen(winHandle, ref p) ) {
                return false;
            }

            Win32Api.SetCursorPos(p.x, p.y);
            Application.DoEvents();
            return true;
        }


        /// <summary>
        /// Trigger a pressing mouse button event.
        /// </summary>
        /// <param name="button">Button type: "left", "middle", "right"</param>
        /// <returns>true for success</returns>
        public bool MouseDown(string button) {
            MouseInput mi = new MouseInput( 0 );
            switch( button ) {
                case "left":
                    mi.dwFlags |= MouseInput.MOUSEEVENTF_LEFTDOWN;
                    break;

                case "right":
                    mi.dwFlags |= MouseInput.MOUSEEVENTF_RIGHTDOWN;
                    break;

                case "middle":
                    mi.dwFlags |= MouseInput.MOUSEEVENTF_MIDDLEDOWN;
                    break;

                default:
                    return false;
            }

            if (mi.dwFlags != 0) {
                if (0 == Win32Api.SendMouseInput( 1, ref mi, Marshal.SizeOf( mi ) )) {
                    return false;
                }
                Application.DoEvents();
            }
            return true;
        }
        
        /// <summary>
        /// Trigger a releasing mouse button events.
        /// </summary>
        /// <param name="button">button type: "left", "middle", "right"</param>
        /// <returns>true for success</returns>
        public bool MouseUp(string button) {
            MouseInput mi = new MouseInput( 0 );
            switch( button ) {
                case "left":
                    mi.dwFlags |= MouseInput.MOUSEEVENTF_LEFTUP;
                    break;

                case "right":
                    mi.dwFlags |= MouseInput.MOUSEEVENTF_RIGHTUP;
                    break;

                case "middle":
                    mi.dwFlags |= MouseInput.MOUSEEVENTF_MIDDLEUP;
                    break;

                default:
                    return false;
            }

            if (mi.dwFlags != 0) {
                if (0 == Win32Api.SendMouseInput( 1, ref mi, Marshal.SizeOf( mi ) )) {
                    return false;
                }
                Application.DoEvents();
            }
            return true;
        }

        public bool MouseClick(string button, int x, int y) {
            if (! MouseMoveTo(x, y) ) {
                return false;
            }

            if ( ! MouseDown(button) ) {
                return false;
            }

            return MouseUp(button);
        }

        #endregion


        #region convert window to specifical window controls.
        /// <summary>
        /// Convert the window object to a button object.
        /// </summary>
        /// <returns></returns>
        public Button toButton() {
            return new Button(winHandle, xmlElement);
        }

        /// <summary>
        /// Convert the window object to a text box object.
        /// </summary>
        /// <returns></returns>
        public TextBox toTextBox() {
            return new TextBox(winHandle, xmlElement);
        }

        /// <summary>
        /// Convert the window object to a label object.
        /// </summary>
        /// <returns></returns>
        public Label toLabel() {
            return new Label(winHandle, xmlElement);
        }


        /// <summary>
        /// Convert the current window to DHTML object if the window hosts a DHTML object
        /// </summary>
        /// <returns>The HTMLDocument object</returns>
        public HTMLDocument toDhtml() {
            Guid            IID_IHTMLDocument = typeof(HTMLDocument).GUID;
            object          docObj = null;
            int             lRes = 0;
            int             nMsg = Win32Api.RegisterWindowMessage("WM_HTML_GETOBJECT");

            IntPtr ret = Win32Api.SendMessageTimeout(winHandle, nMsg, 0, 0, 
                Win32Api.SMTO_ABORTIFHUNG, 1000, ref lRes);

            if ( ret.ToInt32() == 0 ) {
                return null;
            }

            Win32Api.ObjectFromLresult(lRes, ref IID_IHTMLDocument, 0, out docObj);

            return (HTMLDocument) docObj;
        }
        #endregion

    }

    /// <summary>
    /// Represent a text box control in a window.
    /// </summary>
    public class TextBox : Window
    {
        internal TextBox( IntPtr winHandle, XmlElement xmlElement) : base(winHandle, xmlElement) {
        }
    }

    /// <summary>
    /// Represent a text label control in a window.
    /// </summary>
    public class Label : Window {
        internal Label(IntPtr winHandle, XmlElement xmlElement) : base(winHandle, xmlElement) {
        }
    }

    /// <summary>
    /// Represent a button control in a window.
    /// </summary>
    public class Button : Window
    {
        internal Button(IntPtr winHandle, XmlElement xmlElement) : base(winHandle, xmlElement) {}
        
        /// <summary>
        /// Click the button.
        /// </summary>
        public void click() {
            Win32Api.PostMessage(winHandle, WinMsg.BM_CLICK);
            Application.DoEvents();
        }
    }
}
