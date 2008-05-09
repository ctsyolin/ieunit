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
using mshtml;
using System.Collections;


namespace QuickFocus {
    /// <summary>
    /// Summary description for TextField.
    /// </summary>
    public class TextArea : PageElement {
        private TextArea(IHTMLElement e) : base(e) {
        }

        public static TextArea AsTextArea(IHTMLElement e) {
            IHTMLTextAreaElement e2 = e as IHTMLTextAreaElement;
            if (e2 != null) {
                return new TextArea(e);
            }
            return null;
        }

        private int GetIndex() {
            int idx = -1;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("TEXTAREA") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                idx++;
                if ( this.e.Equals(it.Current) ) {
                    break;
                }
            }
            return idx;
        }

        private string Value {
            get { 
                string v = (e as IHTMLTextAreaElement).value; 
                return (v==null) ? "" : v;
            }
        }

        public override ArrayList ActionSnipList() {
            
            //Notice: an text input element can have both id and name, both can
            //can be access through the hash table document.all().

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.setTextArea(\"" + e.id + "\", \"" + EscapeString(this.Value) + "\");");
            }

            IHTMLTextAreaElement e2 = e as IHTMLTextAreaElement;
            if ( (e2.name != null) && IsUnique(e2.name) ) {
                AddSnip("_.setTextArea(\"" + e2.name + "\", \"" + EscapeString(this.Value) + "\");");
            }

            AddSnip("_.setTextArea(" + this.GetIndex() + ",\"" + EscapeString(this.Value) + "\");");

            return snipList;
        }


        public override ArrayList AssertSnipList() {
            AssertObjWithId();

            IHTMLTextAreaElement e2 = e as IHTMLTextAreaElement;
            if ( (e2.name != null) && IsUnique(e2.name) ) {
                AddSnip("_.assertEquals(\"" + EscapeString(e2.value) + "\", _.findTextArea(\"" + e2.name + "\").value);");
                AddSnip("_.assertNotNull(_.findTextArea(\"" + e2.name + "\"));");
            }

            return snipList;
        }    
    }
}
