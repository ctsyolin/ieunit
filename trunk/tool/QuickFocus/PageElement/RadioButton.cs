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
	/// Summary description for RadioBox.
	/// </summary>
	public class RadioButton : PageElement
	{
        IHTMLInputElement radio;

        public RadioButton(IHTMLElement e, IHTMLInputElement radio) : base(e) {
            this.radio = radio;
        }

        public static RadioButton AsRadioButton(IHTMLElement e) {
            IHTMLInputElement e2 = e as IHTMLInputElement;
            if (e2 != null ) {
                if ( e2.type == "radio" ) {
                    return new RadioButton(e, e2);
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
                if ( e2.type=="radio" ) { 
                    idx++;
                }
                if ( this.radio.Equals(e2) ) {
                    break;
                }
            }
            return idx;
        }

        public override ArrayList ActionSnipList() {
            if ( InSubmitMode ) {
                //
                // If the user want a submit block we only issue code for
                // those radio buttons which are in checked state.
                //
                if (! radio.@checked ) {
                    return snipList;
                }
            }

            int idx = GetIndex();

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.checkRadioButton(\"" + e.id + "\");");
            }

            if ( idx >= 0 ) {
                AddSnip("_.checkRadioButton(" + idx + ");");
            }

            return snipList;
        }

        public override ArrayList AssertSnipList() {
            int     idx = GetIndex();
            
            bool    isChecked = radio.@checked;
            string  assertChecked = isChecked ? "_.assertTrue" : "_.assertFalse";

            AssertObjWithId();

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip(assertChecked + "(_.findRadioButton(\"" + e.id + "\").checked);");
            }

            if ( idx >= 0 ) {
                AddSnip("_.assertNotNull(_.findRadioButton(" + idx + "));");
                AddSnip(assertChecked + "( _.findRadioButton(" + idx +").checked);");
            }

            return snipList;
        }
    
	}
}
