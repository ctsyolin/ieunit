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

namespace QuickFocus {
    /// <summary>
    /// Summary description for Button.
    /// </summary>
    public class TableElement : PageElement {
        private TableElement(IHTMLElement e) : base(e) {
        }

        public static TableElement AsTableElement(IHTMLElement e) {
            if ( e.tagName == "TABLE" ) {
                return new TableElement(e);
            }
            return null;
        }
        /// <summary>
        /// Get the this button's index among all buttons with the same label.
        /// </summary>
        /// <returns>index of this button</returns>
        private int GetIndex() {
            int idx = -1;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("TABLE") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                idx++;
                if ( this.e.Equals(it.Current) ) {
                    break;
                }
            }
            return idx;
        }

        public override ArrayList ActionSnipList() {
            int idx = this.GetIndex();

            if ( (e.id != null) && IsUnique(e.id) ) {
                AddSnip("_.setFindScope(_.findObjById(\"" + e.id + "\"));");
            }

            AddSnip("_.setFindScope(_.findByTag(\"TABLE\", " + idx + "));");

            return snipList;
        }

        public override ArrayList AssertSnipList() {
            int idx = this.GetIndex();

            AssertObjWithId();
            AddSnip("_.assertNotNull(_.findByTag(\"TABLE\", " + idx + "));");

            return snipList;
        }
    }
}
