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


namespace QuickFocus
{
	/// <summary>
	/// Summary description for TextField.
	/// </summary>
	public class InputText : PageElement
	{
        bool         isPassword;

		private InputText(IHTMLElement e, bool isPassword) : base(e)
		{
            this.isPassword = isPassword;
		}

        public static InputText AsInputText(IHTMLElement e) {
            IHTMLInputElement e2 = e as IHTMLInputElement;
            if (e2 != null ) {
                if ( e2.type=="password"  ) {
                    return new InputText(e, true);
                } else if (e2.type == "text" ) {
                    return new InputText(e, false);
                }
            }
            return null;
        }

        private int GetIndex() {
            int idx = -1;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("INPUT") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLInputElement e2 = it.Current as IHTMLInputElement;
                if ( (e2.type=="text") || (e2.type=="password") ) { 
                    idx++;
                }
                if ( this.e.Equals(e2) ) { // This works in deed.
                    break;
                }
            }
            return idx;
        }

        private string Value {
            get { 
                string v = (e as IHTMLInputElement).value; 
                return (v==null) ? "" : v;
            }
        }

        public override ArrayList ActionSnipList() {
            
            //Notice: an text input element can have both id and name, both can
            //can be access through the hash table document.all().

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.setField(\"" + e.id + "\", \"" + EscapeString(this.Value) + "\");");
            }

            IHTMLInputElement e2 = e as IHTMLInputElement;
            if ( (e2.name != null) && IsUnique(e2.name) ) {
                AddSnip("_.setField(\"" + e2.name + "\", \"" + EscapeString(this.Value) + "\");");
            }

            AddSnip("_.setField(" + this.GetIndex() + ",\"" + EscapeString(this.Value) + "\");");

            return snipList;
        }


        public override ArrayList AssertSnipList() {
            int idx = this.GetIndex();

            AssertObjWithId();

            IHTMLInputElement e2 = e as IHTMLInputElement;
            if ( (e2.name != null) && IsUnique(e2.name) ) {
                AddSnip("_.assertEquals(\"" + EscapeString(e2.value) + "\", _.findField(\"" + e2.name + "\").value);");
            }

            AddSnip("_.assertNotNull(_.findField(" + idx + "));");

            return snipList;
        }    
    }
}
