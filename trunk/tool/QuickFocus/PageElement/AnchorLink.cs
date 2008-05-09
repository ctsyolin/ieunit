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
	/// Summary description for AnchorLink.
	/// </summary>
	public class AnchorLink : PageElement
	{
        string keyText;
        string keyTextEsc;

        private AnchorLink(IHTMLElement e) : base(e) {
            keyText = MakeKeyString(e.innerText);
            keyTextEsc = EscapeString(keyText);
        }

        public static AnchorLink AsAnchorLink(IHTMLElement e) {
            if ( e is IHTMLAnchorElement2 ) {
                return new AnchorLink( e );
            } else {
                return null;
            }
        }

        private int GetLinkIndex() {
            int     idx     = -1;
            if ( keyText == null ) {
                // REVISIT: some anchor link has a image element in it
                // we have to handle it here more appropriately.
                return -1;
            }

            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("A") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();

            while ( it.MoveNext() ) {
                IHTMLElement e2 = (IHTMLElement) it.Current;
                string txt = e2.innerText;
                if ( ( txt != null) && (txt.IndexOf(keyText) >= 0 ) ) {
                    idx++;
                }
                if ( this.e.Equals(e2) ) {
                    return idx;
                }
            }
            return -1;
        }

        public override ArrayList ActionSnipList() {
            int idx = this.GetLinkIndex();

            ClickObjWithId();            

            if ( idx == 0 ) {
                AddSnip("_.clickLink(\"" +  keyTextEsc + "\");");
            } else if ( idx > 0 ) {
                AddSnip("_.clickLink(\"" + keyTextEsc + "\", " + idx + ");");
            } else {                
                HTMLAnchorElementClass anchor = e as HTMLAnchorElementClass;
                ImageElement img = PageElement.ToPageElement(anchor.firstChild as IHTMLElement) as ImageElement;
                if ( img != null ) {
                    ArrayList snips = img.ActionSnipList();
                    foreach(string snip in snips) {
                        AddSnip(snip);
                    }
                }
            }

            return snipList;
        }


        public override ArrayList AssertSnipList() {
            int idx = this.GetLinkIndex();

            AssertObjWithId();

            if ( idx == 0 ) {
                AddSnip("_.assertNotNull(_.findLink(\"" + keyTextEsc + "\"));");
            } else if ( idx > 0 ) {
                AddSnip("_.assertNotNull(_.findLink(\"" + keyTextEsc + "\", " + idx + "));" );
            } else {
                HTMLAnchorElementClass anchor = e as HTMLAnchorElementClass;
                ImageElement img = PageElement.ToPageElement(anchor.firstChild as IHTMLElement) as ImageElement;
                if ( img != null ) {
                    ArrayList snips = img.AssertSnipList();
                    foreach(string snip in snips) {
                        AddSnip(snip);
                    }
                }
            }

            return snipList;
        }
    }
}
