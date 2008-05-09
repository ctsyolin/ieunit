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
using System.Diagnostics;
using System.Text;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for Button.
	/// </summary>
	public class Button : PageElement 
	{
        bool isInputElement;

		private Button(IHTMLElement e, bool isInputElement) : base(e)
		{
            this.isInputElement = isInputElement;
		}

        public static Button AsButton(IHTMLElement e) {
            if ( e.tagName == "BUTTON" ) {
                return new Button(e, false);
            }

            IHTMLInputElement e2 = e as IHTMLInputElement;
            if (e2 != null ) {
                string type= e2.type;
                if ( (type=="submit") || (type=="button") || (type=="image") ) {
                    return new Button(e, true);
                }               
            }
            return null;
        }

        private string Label {
            get {
                if ( isInputElement ) {
                    IHTMLInputElement e2 = e as IHTMLInputElement;
                    if ( (e2.type=="button") || (e2.type=="submit") ) {
                        return e2.value;
                    } else if ( e2.type == "image" ) {
                        int index = e2.src.LastIndexOf("/");
                        if ( index <= 0 ) {
                            index = 0;
                        }
                        return e2.src.Substring(index);
                    } else {
                        return "???";
                    }
                } else {
                    return e.innerText;
                }
            }
        }

        /// <summary>
        /// Get the this button's index among all buttons with the same label.
        /// </summary>
        /// <returns>index of this button</returns>
        private int GetIndex() {
            int idx = -1;

            string myLabel = this.Label;

            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("INPUT") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLInputElement e2 = it.Current as IHTMLInputElement;
                if ( (e2.type=="submit") || (e2.type=="button") ) { 
                    if ( myLabel.Equals(e2.value) ) {
                        idx++;
                    }
                } else if (e2.type == "image") {
                    if ( e2.src.IndexOf(myLabel) >= 0 ) {
                        idx++;
                    }
                }
                if ( this.e.Equals(e2) ) { // This works in deed.
                    return idx;
                }
            }

            eList = MainForm.SelectedDoc.all.tags("Button") as IHTMLElementCollection;
            it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLElement e2 = (IHTMLElement) it.Current;
                if ( e2.innerText == myLabel ) {
                    idx++;
                }

                if ( this.e.Equals(e2) ) {
                    return idx;
                }
            }
            return -1;
        }

        public override ArrayList ActionSnipList() {
            int idx = this.GetIndex();

            ClickObjWithId();

            if ( idx == 0 ) {
                AddSnip("_.clickButton(\"" + EscapeString(this.Label) + "\");");
            } else {
                AddSnip("_.clickButton(\"" + EscapeString(this.Label) + "\", " + idx + ");");
            }

            return snipList;
        }

        public override ArrayList AssertSnipList() {
            int idx = this.GetIndex();

            AssertObjWithId();

            if ( idx == 0 ) {
                AddSnip("_.assertNotNull(_.findButton(\"" + EscapeString(Label) + "\"));");
            } else {
                AddSnip("_.assertNotNull(_.findButton(\"" + EscapeString(Label) + "\", " + idx + "));");
            }

            return snipList;
        }
	}
}
