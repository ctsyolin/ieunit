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
using System.Collections;
using mshtml;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for PageElement.
	/// </summary>
    public class PageElement {
        protected IHTMLElement  e;
        protected ArrayList     snipList = new ArrayList();
        private  bool           inSubmitMode; // indicate whether the user is want submit block

        protected PageElement(IHTMLElement e) {
            this.inSubmitMode = false;
            this.e = e;
        }

        public bool InSubmitMode  {
            set { inSubmitMode = value; }
            get { return inSubmitMode; }
        }

        public void ClearSnipList() {
            snipList.Clear();
        }

        public virtual ArrayList ActionSnipList() {
            return snipList;
        }

        public virtual ArrayList AssertSnipList() {
            return snipList;
        }

        protected void AddSnip(string snip) {
            snipList.Add(snip + "\n");
        }

        protected void InsertSnip(string snip) {
            snipList.Insert(0, snip + "\n");
        }

        protected void AssertObjWithId() {
            if ( (e.id != null) && IsUnique(e.id) ) {
                if ( e.innerText != null ) { 
                    AddSnip("_.assertTextContains(_.findObjById(\"" + e.id 
                        + "\"), \""  + EscapeString(MakeKeyString(e.innerText)) + "\");");
                } else {
                    AddSnip("_.assertNotNull(_.findObjById(\"" + e.id + "\"));");
                }
            }

            string name = ElementName;
            if (  (name != null) && IsUnique(name) ) {
                AddSnip("_.assertNotNull(_.findObjById(\"" + name + "\"));");
            }
        }

        protected void ClickObjWithId() {
            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.clickObjById(\"" + e.id + "\");");
            }

            string name = ElementName;
            if (  (name != null) && IsUnique(name) ) {
                AddSnip("_.clickObjById(\"" + name + "\");");
            }
        }

        //
        // DHTML modles doesn't grant that the name or id is uniqueu.
        // In many case we just issue id or name related statement 
        // if the id or name is unique since IeUnit doesn't support the no-unique
        // name or id.
        //
        protected bool IsUnique(string idOrName) {
            if ( idOrName == null ) {
                return false;
            }
            object obj = MainForm.SelectedDoc.all.item(idOrName,1);
            return (obj == null) ? true : false;
        }

        protected void AssertTagHasText(string keyText) {
            // If nothing else is available we just check the page as a whole.
            string tag = e.tagName;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags(tag) as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            int idx = -1;
            while ( it.MoveNext() ) {
                if ( it.Current != null ) {
                    idx++;
                }
                if ( this.e.Equals(it.Current) ) {
                    break;
                }
            }
            if (idx >= 0) {
                AddSnip("_.assertTagHasText(\"" + tag + "\"," + idx + ",\"" + keyText + "\");");
            } else {
                AddSnip("_.assertPageHasText(\"" + keyText + "\");");
            }
        }

        public static PageElement ToPageElement(IHTMLElement e) {
            PageElement pe;

            if ( e == null ) {
                return null;
            }

            pe = Button.AsButton(e);            if ( pe != null ) { return pe; }
            pe = InputText.AsInputText(e);      if ( pe != null ) { return pe;}
            pe = ImageElement.AsImageElement(e);if ( pe != null ) { return pe; }
            pe = AnchorLink.AsAnchorLink(e);    if ( pe != null ) { return pe; }
            pe = RadioButton.AsRadioButton(e);  if ( pe != null ) { return pe; }
            pe = CheckBox.AsCheckBox(e);        if ( pe != null ) { return pe; }
            pe = SelectOption.AsSelectOption(e);if ( pe != null ) { return pe; }
            pe = SelectOption.AsSelectOption(e);if ( pe != null ) { return pe; }
            pe = TableElement.AsTableElement(e);if ( pe != null ) { return pe; }
            pe = MapArea.AsMapArea(e);          if ( pe != null ) { return pe; }
            pe = TextArea.AsTextArea(e);        if ( pe != null ) { return pe; }
            pe = ForeignFrame.AsForeignFrame(e);if ( pe != null ) { return pe; }

            // TextBlock will catch all element with non-null innerText property.
            pe = TextBlock.AsTextBlock(e);      if ( pe != null ) { return pe; }
            
            // For any other element we just return a generic PageElement object
            return new PageElement(e);
        }

        /// <summary>
        /// Find an element in the parent chain with given tag name upto give level.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="tagName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        protected static IHTMLElement FindParentOfTag(IHTMLElement e, string tagName, int level) {
            if ( e == null ) { 
                return null;
            }

            for (int i=0; i<level; i++) {
                if ( e.tagName == tagName ) {
                    break;
                }
                e = e.parentElement;
                if ( e == null ){
                    break;
                }
            }

            if ( e==null) {
                return null;
            }

            if (e.tagName == tagName) {
                return e;
            } else {
                return null;
            }
        }

        
        /// <summary>
        /// Find the closest parent that has an id up
        /// to given level.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public IHTMLElement FindParentWithId(IHTMLElement e, int level) {
            if ( e == null ) {
                return null;
            }

            for(int i=0; i<level; i++) {
                if ( e.id != null ) {
                    return e;
                } 
                e = e.parentElement;
                if ( e == null ) {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Extract characteristics key string from a raw text string.
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        protected string MakeKeyString(string rawStr) {            
            if ( rawStr == null ) {
                return null;
            }

            rawStr = rawStr.Trim(' ', '\t', '\n','\r');
            return rawStr.Substring(0, 
                Math.Min(Root.appConfig.maxKeyTextLength, rawStr.Length));            
        }

        internal static string EscapeString(string str) {
            if ( str == null ) {
                return null;
            }
            string retStr = str.Replace("\"", "\\\"");
            retStr = retStr.Replace("\r", "\\r");
            retStr = retStr.Replace("\n", "\\n");
            return retStr;
        }

        protected string ElementName {
            get {
                if (e is HTMLInputElementClass ) {
                    return (e as HTMLInputElementClass).name;
                } else if ( e is HTMLSelectElementClass ) {
                    return (e as HTMLSelectElementClass).name;
                } else {
                    return null;
                }
            } 
        }
    }
}
