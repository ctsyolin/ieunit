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
	/// Summary description for Frameset.
	/// </summary>
	public sealed class Frameset 
	{
		private Frameset() 
		{
		}

        private static string preFramePath;

        public static void Reset() {
            preFramePath = null;
        }

        public static bool IsForeignFrame(IHTMLWindow2 win) {

            try {
                if ( win.document.protocol == "File Protocol") {
                    return false;
                }

                if ( win.document.domain.Length == 0 ) {
                    return true;
                }
            } catch( UnauthorizedAccessException ) {
                return true;
            }

            return false;
        }

        public static ArrayList GetFramesetSnip(IHTMLDocument2 rootDoc, IHTMLDocument2 focusedDoc) {
            ArrayList snips = new ArrayList();
            if ( rootDoc.frames.length <= 0 ) {
                return snips;
            }
                    
            IHTMLWindow2 pwin = focusedDoc.parentWindow;

            if ( IsForeignFrame(pwin) ) {
                // We can't click or assert in a foreign frame.
                // So, no code here.
                return snips;
            }

            string framePath = "";
            while ( pwin != pwin.parent ) {
                if ( pwin.name != null ) {
                    framePath = pwin.name + "/" +  framePath;
                } else {
                    DispHTMLWindow2 win2 = pwin as DispHTMLWindow2;
                    HTMLFrameElementClass frm = win2.frameElement as HTMLFrameElementClass;
                    if ( (frm != null) && (frm.id != null) ) {
                        framePath = frm.id + "/" + framePath;
                    } else {
                        object x = 1;
                        int    i;
                        for(i=0; i<pwin.parent.length; i++) {
                            x = i;
                            object f = pwin.parent.item(ref x);
                            if ( f == pwin ) {
                                break;
                            }
                        }
                        if (i<pwin.parent.length) {
                            framePath = i + "/" + framePath;
                        } else {
                            // something is wrong if we got here
                            return snips;
                        }
                    }
                }
                pwin = pwin.parent;
            }

            if ( framePath.Length > 0 ) {
                // trim the last "/" character.
                framePath = framePath.Substring(0, framePath.Length-1);
            } else {
                return snips;
            }


            if ( framePath == preFramePath ) {
                // We are still in the same frame, we don't have to issue the setFrame()
                return snips;
            } else {
                preFramePath = framePath;
            }


            snips.Add("_.setFrame(\"" + preFramePath + "\");\n");

            return snips;
        }
	}
}
