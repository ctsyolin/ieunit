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
	/// Summary description for SelectOption.
	/// </summary>
	public class SelectOption : PageElement
	{
		private SelectOption(IHTMLElement e) : base (e) {}

        public static SelectOption AsSelectOption(IHTMLElement e) {
            return ( e.tagName == "SELECT" ) ? new SelectOption(e) : null;
        }

        private int GetIndex() {
            int     idx     = -1;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("SELECT") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                IHTMLElement e2 = (IHTMLElement) it.Current;
                idx++;
                if ( this.e.Equals(e2) ) {
                    break;
                }
            }
            return idx;
        }

        private void AddNewSnip(HTMLOptionElementClass op, string snippet) {
            if ( op.selected ) {
                InsertSnip(snippet);
            } else {
                AddSnip(snippet);
            }
        }

        public override ArrayList ActionSnipList() {
            IHTMLSelectElement e2 = e as IHTMLSelectElement;
            IHTMLSelectElement opts = (IHTMLSelectElement) e2.options;   
            int     idx     = GetIndex();
            string  name    = ElementName;

            for(int i=0; i<opts.length; i++) {
                HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                AddNewSnip(op, "_.setSelectOption(" + idx + ", \"" + op.innerText + "\");");
            }


            for(int i=0; i<opts.length; i++) {
                HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                AddNewSnip(op, "_.setSelectOption(" + idx + ", " + i + ");");
                if (name!=null) {
                    AddNewSnip(op, "_.setSelectOption(\"" + name + "\", " + i + ");");
                }
            }
            AddSnip("");

            if ( (e.id != null) && IsUnique(e.id) ) {
                for(int i=0; i<opts.length; i++) {
                    HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                    AddNewSnip(op,"_.setSelectOption(\"" + e.id + "\", \"" + op.innerText + "\");");
                }
            }
            AddSnip("");

            if ( name != null ) {
                for(int i=0; i<opts.length; i++) {
                    HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                    AddNewSnip(op, "_.setSelectOption(\"" + name + "\", \"" + op.innerText + "\");");
                }
            }
            AddSnip("");

            return snipList;
        }

        public override ArrayList AssertSnipList() {
            IHTMLSelectElement e2 = e as IHTMLSelectElement;
            IHTMLSelectElement opts = (IHTMLSelectElement) e2.options;   
            int idx = GetIndex();

            AddSnip("_.assertNotNull(_.findSelect(" + idx + "));");

            for(int i=0; i<opts.length; i++) {
                HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                AddNewSnip(op, "_.assertSelectHasOption(" + idx + ", \"" + op.innerText + "\");");
            }
            AddSnip("");

            AssertObjWithId();

            string name = ElementName;
            if ( (name != null) && IsUnique(name) ) {
                for(int i=0; i<opts.length; i++) {
                    HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                    AddNewSnip(op, "_.assertSelectHasOption(\"" + name + "\", \"" + op.innerText + "\");");
                }
                AddSnip("");
            }

            if ( (e.id != null) && IsUnique(e.id) ) {
                for(int i=0; i<opts.length; i++) {
                    HTMLOptionElementClass op = (HTMLOptionElementClass) opts.item(i, 0);
                    AddNewSnip(op, "_.assertSelectHasOption(\"" + e.id + "\", \"" + op.innerText + "\");");
                }
                AddSnip("");
            }

            return snipList;
        }



	}
}
