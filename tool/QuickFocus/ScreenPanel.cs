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
using System.Windows.Forms;
using System.Drawing;
using mshtml;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Security;
using System.Security.Permissions;

namespace QuickFocus {
    public class ScreenPanel : System.Windows.Forms.Panel {
        private FramedElement selectItem;
        private FramedElement flushItem;

        private Pen selectPen;
        private Pen flushPen;
        private ElementProperties eProperty;

        private float frameWidth; // short-cut to AppConfig.focusBoxWidth
        
        public ScreenPanel() {
        }

        //
        // We can't put the following code into the contructor ScreenPanel()
        // because the VS C# will try to instanciate a ScreenPanel object
        // when open the MainForm form. That will fail and result into some
        // unpredicatable behavior of VS C#.
        //
        public void Initialize() {

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Paint +=new PaintEventHandler(ScreenPanel_Paint);
            this.DragEnter += new DragEventHandler(ScreenPanel_DragEnter);
            this.DragDrop +=new DragEventHandler(ScreenPanel_DragDrop);

            UpdateFrameWidth();
            InitContextMenu();
        }

        public void UpdateFrameWidth() {
            frameWidth = (float) Root.appConfig.focusFrameWidth;
            flushPen = new Pen(Color.FromArgb(0, 255,  0), frameWidth);
            selectPen = new Pen(Color.FromArgb(255, 0, 0), frameWidth);
        }

        private void InitContextMenu() {
            this.ContextMenu = new ContextMenu();
            ContextMenu.MenuItems.Add(new MenuItem("Enlarge Focus Scope", new EventHandler(EnlargeFocusScope)));
            ContextMenu.MenuItems.Add(new MenuItem("Shrink Focus Scope", new EventHandler(ReduceFocusScope)));
            ContextMenu.MenuItems.Add(new MenuItem("Reset Focus Scope", new EventHandler(ResetFocusScope)));
            ContextMenu.MenuItems.Add(new MenuItem("-"));
            ContextMenu.MenuItems.Add(new MenuItem("Detach Page", new EventHandler(DetachDocument)));
            ContextMenu.MenuItems.Add(new MenuItem("Click Focused Element", new EventHandler(ClickFocusedElement)));
            ContextMenu.MenuItems.Add(new MenuItem("-"));

            MenuItem mi = new MenuItem("Select Ancestor Element");
            mi.Select += new EventHandler(miSelectAcenstor_Select);
            mi.MenuItems.Add("<None>");
            ContextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Select Sibling Element");
            mi.Select +=new EventHandler(miSelectSibling_Select);
            mi.MenuItems.Add("<None>");
            ContextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Select Child Element");
            mi.Select +=new EventHandler(miSelectChildren_Select);
            mi.MenuItems.Add("<None>");
            ContextMenu.MenuItems.Add(mi);

            ContextMenu.MenuItems.Add(new MenuItem("-"));
            ContextMenu.MenuItems.Add(new MenuItem("Element Properties...", new EventHandler(ShowElementProperties)));

        }

        protected override CreateParams CreateParams {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;  // WS_EX_TRANSPARENT as defined in winuser.h
                return cp;
            }
        } 

        private void DetachDocument(object sender, EventArgs e) {
            Root.mainForm.DetachPage();
        }

        private void ClickFocusedElement(object sender, EventArgs e) {
            FramedElement fe = Root.mainForm.FocusedElement;
            if ( (fe != null) && (fe.e!=null) ) {
                IHTMLElement element = fe.e as IHTMLElement;
                if ( element != null ) {
                    try {
                        element.click();
                    } catch(Exception){}
                }
            }
        }

        private void ShowElementProperties(object sender, EventArgs e) {
            FramedElement fe = Root.mainForm.FocusedElement;
            eProperty = new ElementProperties(fe);
            eProperty.Show();
        }

        internal ElementProperties PropertyForm {
            get { return eProperty; }
        }

        private void ResetFocusScope(object sender, EventArgs e) {
            Root.appConfig.focusLevel = 0;
            Root.mainForm.SetStatusFocusedItem();
        }

        private void ReduceFocusScope(object sender, EventArgs e) {
            Root.appConfig.focusLevel = Math.Max(0, Root.appConfig.focusLevel - 1);
            Root.mainForm.SetStatusFocusedItem();
        }

        private void EnlargeFocusScope(object sender, EventArgs e) {
            Root.appConfig.focusLevel = Root.appConfig.focusLevel + 1;
            Root.mainForm.SetStatusFocusedItem();
        }

        static IHTMLElement preItem;
        public void ShowElementAt(int x, int y) {
            FramedElement item = FindElement(x, y);
            if ( (item == null) || (item.e==null) || (item.e==preItem) ) {
                return;
            }
            
            preItem = item.e;
            ShowFlushMarker(item);
            Root.mainForm.FocusedElement = item;

            if ( item.e != null ) {                
                Root.mainForm.SetStatusFocusedItem();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {
        }

        public IHTMLElement FocusedElement(IHTMLElement e) {
            int focusLevel = Root.appConfig.focusLevel;
            if ( focusLevel == 0 ) {
                return e;
            }

            for (int i=1; i<=focusLevel; i++) {
                if ( e.parentElement == null ) {
                    return e;
                }
                e = e.parentElement;
            }
            return e;
        }

        public FramedElement FindElement(int x, int y) {
            IHTMLDocument2 doc = MainForm.Browser.Document as IHTMLDocument2;
            if ( doc == null ) {
                return null;
            }
            IHTMLElement item = doc.elementFromPoint(x, y);
            if ( item == null ) {
                return null;
            }

            //
            // If there is a frame at the given point we have to drill into the
            // frame and find the target object within the frame.
            //
            if ( (item is HTMLFrameElementClass)  || (item is HTMLIFrameClass) ) {

                int relX = x;
                int relY = y;
                IHTMLElement pFrame   = item;    // the parent frame of the target element
                IHTMLElement ppFrame  = null;    // the parent of ppFrame;
                IHTMLElement tElement = null;    // the target element that covers (x,y).

                IHTMLWindow2 pWin;
                while ( true ) {
                    if ( pFrame is HTMLFrameElementClass ) {
                        pWin = (pFrame as HTMLFrameElementClass).contentWindow;
                    } else {
                        pWin = (pFrame as HTMLIFrameClass).contentWindow;
                    }
                    
                    // convert parent coordinator to relative coord w.r.t the frame.
                    IHTMLRect frameRect = (pFrame as IHTMLElement2).getBoundingClientRect();
                    relX -= frameRect.left;
                    relY -= frameRect.top;


                    try {
                        tElement = (pWin.document as IHTMLDocument2).elementFromPoint(relX, relY);
                    } catch (UnauthorizedAccessException) {
                        //
                        // the elementFromPoint() call will raise this exception if the parent frame is
                        // a foreign frame. In this case we use the foreign frame as target element.
                        //
                        tElement = pFrame;
                        pFrame   = ppFrame;
                        relX += frameRect.left;
                        relY += frameRect.top;
                        break;
                    }

                    if ( (tElement is HTMLFrameElementClass) || ( tElement is HTMLIFrameClass ) ) {
                        //
                        // the target element found is still a frame. 
                        // We have to drill into it further.
                        //
                        ppFrame = pFrame;
                        pFrame = tElement;
                    } else {                            
                        break;
                    }
                }

                if ( tElement == null ) {
                    //
                    // something is not correct. Maybe the page has not loaded yet.
                    //
                    return null;
                }                    

                if ( pFrame != null ) {
                    //
                    // Notice: (x-relX, y-relY) is the clicked element's top-left corn's coordinator
                    // relative the window's orignal. (deltaX, deltaY) is the coordinator of
                    // e relative selectedE;
                    //
                    IHTMLElement selectedE = FocusedElement(tElement);
                    int deltaX = (tElement as IHTMLElement2).clientLeft - (selectedE as IHTMLElement2).clientLeft;
                    int deltaY = (tElement as IHTMLElement2).clientTop - (selectedE as IHTMLElement2).clientTop;
                    return new FramedElement(selectedE, pFrame, x-relX-deltaX, y-relY-deltaY);
                } else {
                    // the found element is a foreign frame as direct child of the top level window.
                    return new FramedElement(FocusedElement(tElement) , null);
                }
            } else {
                //
                // The found element is not a frame, but a normal element.
                //
                return new FramedElement(FocusedElement(item) , null);
            }
        }

        private void ScreenPanel_Paint(object sender, PaintEventArgs e) {
            if ( (flushItem != null) && (flushItem.e!=null) ) {
                if ( Root.appConfig.focusFrameWidth > 0 ) {
                    Position pos = GetClientPosition(flushItem);
                    e.Graphics.DrawRectangle(flushPen, pos.x, pos.y, pos.width, pos.height);
                }
            }

            if ( (selectItem != null) && (selectItem.e!=null) ) {
                if ( Root.appConfig.focusFrameWidth > 0 ) {
                    Position pos = GetClientPosition(selectItem);
                    e.Graphics.DrawRectangle(selectPen, pos.x, pos.y, pos.width, pos.height);
                }
            }
        }

        public Position GetClientPosition(FramedElement fe) {
            IHTMLElement2 e2 = fe.e as IHTMLElement2;
            IHTMLRect     rect;
            if ( e2 != null ) {
                rect = e2.getBoundingClientRect();
            } else {
                return null;
            }

            int fw = Root.appConfig.focusFrameWidth;
            float width = rect.right-rect.left+fw;
            float height = rect.bottom-rect.top+fw;
            rect.left -= fw+1;
            rect.top  -= fw+1;
            rect.left += (int)(fw/2.0);  // REVISIT: some fine adjustments. Check this
            rect.top  += (int)(fw/2.0);  // with the TableText.html page and different frame width

            if ( fe.IsInFrame ) {
                rect.left += fe.clientLeft;
                rect.top  += fe.clientTop;
            }

            if ( e2 is HTMLAreaElementClass ) {
                //
                // the bounding box for area element is relative to 
                // the map image's original, we have add the offset here
                //
                HTMLAreaElementClass e3 = e2 as HTMLAreaElementClass;
                HTMLMapElementClass map = e3.parentElement as HTMLMapElementClass;
                if ( (map != null) && (map.name!=null) ) {
                    IHTMLElementCollection eList = (e3.ownerDocument as HTMLDocument).all.tags("IMG") as IHTMLElementCollection;
                    IEnumerator it = eList.GetEnumerator();
                    string nameRef = "#" + map.name;
                    while ( it.MoveNext() ) {
                        HTMLImgClass img = it.Current as HTMLImgClass;
                        if ( img.useMap == nameRef ) {
                            IHTMLRect rectImg = img.getBoundingClientRect();
                            rect.top += rectImg.top + img.clientLeft;
                            rect.left += rectImg.left + img.clientTop;
                        }
                    }                    
                }
            }

            return new Position(rect.left, rect.top, width, height);
        }

        public void ShowSelectMarker(FramedElement item) {
            selectItem = item;
            MainForm.Browser.Parent.Refresh();
        }

        public void ShowFlushMarker(FramedElement item) {
            flushItem = item;
            MainForm.Browser.Parent.Refresh();
        }

        public void Reset() {
            selectItem = null;
            flushItem = null;
            try {
                MainForm.Browser.Parent.Refresh();
            } catch (Exception) {
            }
        }


        private void ScreenPanel_DragEnter(object sender, DragEventArgs e) {
            string fileName = CommonUtil.GetDataFilePath(e.Data);
            e.Effect = DragDropEffects.None;
            if (fileName != null) {
                if ( fileName.EndsWith(".lnk") ) {
                    // We don't know yet how to interpret the *.lnk file
                } else {
                    e.Effect = DragDropEffects.Copy|DragDropEffects.Link|DragDropEffects.Move;
                }
            }
        }

        public static string ExtractUrlFromFile(string fileName) {
            try {
                //extract the URL from *.url file.
                using (StreamReader sr = new StreamReader(fileName)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        if ( line.StartsWith("URL=") ) {
                            return line.Substring(4);
                        }
                    }
                }
            } catch(FileNotFoundException) {
                MsgBox.Alert("Can't find file " + fileName);
            }
            return null;
        }

        private void ScreenPanel_DragDrop(object sender, DragEventArgs e) {
            string fileName = CommonUtil.GetDataFilePath(e.Data);
            if ( fileName == null ) {
                return;
            }

            if ( fileName.EndsWith(".url") ) {
                fileName = ExtractUrlFromFile(fileName);
                if ( fileName != null ) {
                    Root.mainForm.OpenUrl(fileName);
                }
            } else if ( fileName.EndsWith(".lnk") ) {
                // Intepreter the content of link file.
            } else {
                Root.mainForm.OpenUrl(fileName);
            }
        }
        #region Select related elements
        private class ElementMenuItem : MenuItem {
            IHTMLElement element;
            public ElementMenuItem(IHTMLElement e, string caption, EventHandler evt) 
                : base(caption, evt) {
                element = e;
            }
            public IHTMLElement Element { 
                get { return element;}
            }
        }

        private string SelMenuItemLabel(IHTMLElement e) {
            string cap = e.outerHTML;
            if ( cap.Length >= 60 ) {
                return cap.Substring(0, 60) + "...";
            } else { 
                return cap;
            }
        }

        private void miSelectAcenstor_Select(object sender, EventArgs e) {
            MenuItem mi = sender as MenuItem;
            mi.MenuItems.Clear();
            FramedElement fe = Root.mainForm.SelectedElement;
            if ( (fe == null) || (fe.e == null) || (fe.e.parentElement == null) ) {
                mi.MenuItems.Add("<None>");
                return;
            }

            for(IHTMLElement parent = fe.e.parentElement; parent != null; parent = parent.parentElement) {
                mi.MenuItems.Add(
                    new ElementMenuItem(parent, 
                    SelMenuItemLabel(parent), new EventHandler(ClickElementMenu)));
            }
        }

        private void ClickElementMenu(object sender, EventArgs e) {
            ElementMenuItem mi = sender as ElementMenuItem;
            if ( Root.mainForm.FocusedElement == null ) {
                return;
            }
            FramedElement newElement = Root.mainForm.FocusedElement.Clone();
            newElement.e = mi.Element;
            Root.mainForm.SelectedElement = newElement;
        }

        private void miSelectSibling_Select(object sender, EventArgs e) {
            MenuItem mi = sender as MenuItem;
            mi.MenuItems.Clear();
            FramedElement fe = Root.mainForm.SelectedElement;
            if ( (fe == null) || (fe.e == null) ) {
                mi.MenuItems.Add("<None>");
                return;
            }

            IHTMLElement parent = fe.e.parentElement;
            if ( parent == null ) {
                mi.MenuItems.Add("<None>");
                return;
            }

            IHTMLElementCollection eList = parent.children as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLElement silbling = it.Current as IHTMLElement;                
                mi.MenuItems.Add(
                    new ElementMenuItem(silbling, SelMenuItemLabel(silbling), 
                    new EventHandler(ClickElementMenu) ) );
            }
        }

        private void miSelectChildren_Select(object sender, EventArgs e) {
            MenuItem mi = sender as MenuItem;
            mi.MenuItems.Clear();
            FramedElement fe = Root.mainForm.SelectedElement;
            if ( (fe == null) || (fe.e == null) ) {
                mi.MenuItems.Add("<None>");
                return;
            }

            IHTMLElementCollection eList = fe.e.children as IHTMLElementCollection;
            if ( (eList == null) || (eList.length==0) ) {
                mi.MenuItems.Add("<None>");
                return;                
            }

            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLElement child = it.Current as IHTMLElement;                
                mi.MenuItems.Add(
                    new ElementMenuItem(child, SelMenuItemLabel(child), 
                    new EventHandler(ClickElementMenu) ) );
            }
        }

        #endregion
    }

    public class FramedElement {
        public FramedElement(IHTMLElement e, IHTMLElement frame) {
            this.e = e;
            this.frame = frame;
            this.clientTop = 0;
            this.clientLeft = 0;
        }

        public FramedElement(IHTMLElement e, IHTMLElement frame, int clientLeft, int clientTop) {
            this.e = e;
            this.frame = frame;
            this.clientTop = clientTop;
            this.clientLeft = clientLeft;
        }

        public FramedElement Clone() {
            return new FramedElement(e, frame, clientLeft, clientTop);
        }

        public HTMLWindow2Class ContentWindow {
            get {
                if ( frame is HTMLFrameElementClass ) {
                    return (frame as HTMLFrameElementClass).contentWindow as HTMLWindow2Class;
                } else if ( frame is HTMLIFrameClass ) {
                    return (frame as HTMLIFrameClass).contentWindow as HTMLWindow2Class;
                } else {
                    return null;
                }
            }
        }
        
        public bool IsInFrame {
            get { return (frame != null); }
        }

        public IHTMLElement    e;
        public int             clientTop;  // element top-left coordinator relative to the top level window
        public int             clientLeft;
        public IHTMLElement    frame;      // can be either HTMLFrameElementClass or HTMLIFrameClass. 
    }

    public class Position {
        public Position(int x, int y, float width, float height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int x;
        public int y;
        public float width;
        public float height;
    }
}
