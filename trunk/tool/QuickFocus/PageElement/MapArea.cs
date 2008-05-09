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
	/// Summary description for MapArea.
	/// </summary>
    public class MapArea : PageElement {
        private MapArea(IHTMLElement e) : base(e) {
        }

        public static MapArea AsMapArea(IHTMLElement e) {
            if ( e is HTMLAreaElementClass ) {
                return new MapArea(e);
            }
            return null;
        }

        private int GetIndex() {
            int idx = -1;
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("AREA") as IHTMLElementCollection;
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

            ClickObjWithId();

            AddSnip("_.clickObj(_.findByTag(\"AREA\", " + idx + "));");

            return snipList;
        }

        public override ArrayList AssertSnipList() {
            int idx = this.GetIndex();

            AssertObjWithId();

            AddSnip("_.assertNotNull(_.findByTag(\"AREA\", " + idx + "));");

            return snipList;
        }

    }
}
