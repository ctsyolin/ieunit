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


namespace QuickFocus
{
	/// <summary>
	/// This element encasulate all element which have an innerText and is not respresented
	/// by other PageElement. This class is used in ToPageElement() to catch all 
	/// the most HTML elements.
	/// </summary>
	public class TextBlock : PageElement
	{
        private string txt;

        private TextBlock(IHTMLElement e, string txt) : base(e) {
            this.txt = txt;
        }

        public static TextBlock AsTextBlock(IHTMLElement e) {
            string txt = e.innerText;
            if ( (txt == null) || (txt.Length==0)) {
                return null;
            }
            return new TextBlock(e, txt);
        }


        /// <summary>
        /// Find the index of given table within the document DOM tree.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private int FindTableIndex(HTMLTableClass table) {
            int idx = -1;
            HTMLDocumentClass doc = table.ownerDocument as HTMLDocumentClass;
            if( doc == null) {
                return -1;
            }

            IHTMLElementCollection eList = doc.all.tags("TABLE") as IHTMLElementCollection;
            if ( eList == null ) {
                return -1;
            }

            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                HTMLTableClass e2 = it.Current as HTMLTableClass;
                if ( e2 != null ) {
                    idx++;
                    if ( e2.Equals(table) ) { // This works in deed.
                        return idx;
                    }
                }
            }
            return -1;
        }

        public override ArrayList AssertSnipList() {
            string keyText = EscapeString(MakeKeyString(txt));

            // If we can locate the object or one of its close parent
            // by id we check the innerText part of the object with id.
            IHTMLElement parent = FindParentWithId(e, 3);
            if ( (parent != null) && (parent.tagName != "TABLE") ) {
                if ( IsUnique( parent.id ) ) {
                    AddSnip("_.assertTextContains(_.findObjById(\"" 
                        + parent.id + "\"), \"" + keyText + "\");");
                }
            }

            if ( parent != e ) {
                AssertObjWithId();
            }

            // If the clicked object is embeded in a table we try
            // locate it throgh the table.
            HTMLTableCellClass td = FindParentOfTag(e, "TD", 3) as HTMLTableCellClass;
            if (td == null) {
                td = FindParentOfTag(e, "TH", 3) as HTMLTableCellClass;
            }

            do {
                if ( td == null ) {
                    break;
                }

                int columnIdx = td.cellIndex;
                HTMLTableRowClass row = FindParentOfTag(td.parentElement, "TR", 3) as HTMLTableRowClass;
                if ( row == null ) {
                    break;
                }

                int rowIdx = row.rowIndex;
                HTMLTableClass table = td.offsetParent as HTMLTableClass;
                if (table == null ) {
                    break;
                }

                string snip = "";
                if ( (table.id != null) && IsUnique(table.id) ) {
                    snip += "_.assertTextContains(_.findTableCell(\"" + table.id + "\",";
                } else {
                    int tableIdx = FindTableIndex(table);
                    Debug.Assert( tableIdx >= 0 );
                    if ( tableIdx < 0 ) {
                        break;
                    }
                    snip += "_.assertTextContains(_.findTableCell(" + tableIdx + ",";
                }
                snip += rowIdx + "," + columnIdx + "), \"" + keyText + "\");";
                AddSnip(snip);
            } while( false ); 

            AssertTagHasText(keyText);

            return snipList;
        }

        public override ArrayList ActionSnipList() {
            IHTMLElement anchor = FindParentOfTag(e, "A", 3);
            if ( anchor != null ) {
                ClickObjWithId();
                AnchorLink link = PageElement.ToPageElement(anchor) as AnchorLink;
                if ( link != null ) {
                    ArrayList snips = link.ActionSnipList();
                    foreach(string snip in snips) {
                        AddSnip(snip);
                    }
                }
            }

            return snipList;
        }
	}
}
