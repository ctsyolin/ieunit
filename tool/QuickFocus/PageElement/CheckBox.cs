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
    /// Summary description for RadioBox.
    /// </summary>
    public class CheckBox : PageElement {
        IHTMLInputElement checkBox;

        public CheckBox(IHTMLElement e, IHTMLInputElement checkBox) : base(e) {
            this.checkBox = checkBox;
        }

        public static CheckBox AsCheckBox(IHTMLElement e) {
            IHTMLInputElement e2 = e as IHTMLInputElement;
            if (e2 != null) {
                if ( e2.type =="checkbox" ) {
                    return new CheckBox(e, e2);
                }               
            }
            return null;
        }

        /// get index of this element
        private int GetIndex() {
            int idx = -1;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("INPUT") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLInputElement e2 = it.Current as IHTMLInputElement;
                if ( e2.type == "checkbox" ) { 
                    idx++;
                }
                if ( this.checkBox.Equals(e2) ) {
                    break;
                }
            }
            return idx;
        }

        public override ArrayList ActionSnipList() {
            int     idx = GetIndex();
            string  newValue;
            
            if ( InSubmitMode ) {
                //
                // If the user want to generate submit block we have
                // to use the current state.
                //
                newValue = checkBox.@checked ? "true" : "false";
            } else {
                newValue = checkBox.@checked ? "false" : "true";
            } 

            if ( (checkBox.name != null) && IsUnique(checkBox.name) ) {
                AddSnip("_.setCheckBox(\"" + EscapeString(checkBox.name) + "\"," + newValue + ");");
            }

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.setCheckBox(\"" + e.id + "\"," + newValue + ");");
            }

            if ( idx >= 0 ) {
                AddSnip( "_.setCheckBox(" + idx + "," + newValue + ");");
            }


            return snipList;
        }

        public override ArrayList AssertSnipList() {
            int     idx = GetIndex();
            bool    isChecked = checkBox.@checked;
            string  assertChecked = isChecked ? "_.assertTrue" : "_.assertFalse";

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.assertNotNull(_.findCheckBox(\"" + e.id + "\"));");
                AddSnip(assertChecked + "(_.findCheckBox(\"" + e.id + "\").checked);");
            }


            if ( checkBox.name != null ) {
                AddSnip("_.assertNotNull(_.findCheckBox(\"" + checkBox.name + "\"));");
                AddSnip(assertChecked + "(_.findCheckBox(\"" + checkBox.name + "\").checked);");
            }

            if ( idx >= 0 ) {
                AddSnip(assertChecked + "( _.findCheckBox(" + idx +").checked);");
                AddSnip("_.assertNotNull(_.findCheckBox(" + idx + "));");
            }

            return snipList;
        }
    
    }
}
