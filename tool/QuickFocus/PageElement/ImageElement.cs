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
using System.Collections;
using mshtml;

namespace QuickFocus
{
	/// <summary>
	/// Encapuslate the handling of image element.
	/// </summary>
    public class ImageElement : PageElement {
        private HTMLImgClass img;

        private ImageElement(IHTMLElement e) : base(e) {
            img = e as HTMLImgClass;
        }

        public static ImageElement AsImageElement(IHTMLElement e) {
            if ( e is  HTMLImgClass ) {
                return new ImageElement(e);
            } else {
                return null;
            }
        }

        private int GetImageIndex(string srcKey) {
            int     idx     = -1;
        
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("IMG") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                HTMLImgClass e2 = (HTMLImgClass) it.Current;
                if ( (e2.src != null) && (e2.src.IndexOf(srcKey) >= 0 ) ) {
                    idx++;
                }
                if ( this.e.Equals(e2) ) {
                    return idx;
                }
            }
            return -1;
        }

        private string LocateImg(string srcKey) {
            string snip = "";
            int idx = GetImageIndex(srcKey);
            if ( idx < 0 ) {
                return null;
            } else if ( idx == 0 ) {
                snip += "_.findImage(\"" + srcKey + "\")";
            } else {
                snip += "_.findImage(\"" + srcKey + "\"," + idx + ")";
            }
            return snip;
        }

        private string FileName(string filePath) {
            int idx = filePath.LastIndexOf("/");
            return ( idx >= 0 ) ?
                filePath.Substring(idx+1) : filePath;
        }

        public override ArrayList AssertSnipList() {            
            if ( e.id != null ) {
                AddSnip("_.assertNotNull(_.findObjById(\"" + e.id + "\"));");
            }

            string locStr;

            locStr = LocateImg( FileName(img.src) );
            if ( locStr != null ) {
                AddSnip("_.assertNotNull(" + locStr + ");");
            }

            locStr = LocateImg(img.src);
            if ( locStr != null ) {
                AddSnip("_.assertNotNull(" + locStr + ");");
            }

            return snipList;
        }

        public override ArrayList ActionSnipList() {
            if ( (FindParentOfTag(e, "A", 3) != null) || (e.onclick != System.DBNull.Value) ) {
                if ( e.id != null ) {
                    AddSnip("_.clickObjById(\"" + e.id + "\");");
                }

                string keyStr = FileName(img.src);
                int    idx = GetImageIndex(keyStr);
                if ( idx == 0 ) {
                    AddSnip("_.clickImage(\"" + keyStr + "\");");
                } else if (idx > 0 ) {
                    AddSnip("_.clickImage(\"" + keyStr + "\", " + idx + ");");
                }
            
                keyStr = img.src;
                idx    = GetImageIndex(keyStr);
                if ( idx == 0 ) {
                    AddSnip("_.clickImage(\"" + keyStr + "\");");
                } else if (idx > 0 ) {
                    AddSnip("_.clickImage(\"" + keyStr + "\", " + idx + ");");
                }                
            }
            return snipList;
        }
    }

}
