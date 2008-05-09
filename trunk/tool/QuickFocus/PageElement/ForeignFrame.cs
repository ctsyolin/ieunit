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
using System.Windows.Forms;
using System.Collections;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for ForeignFrame.
	/// </summary>
	public class ForeignFrame : PageElement
	{
        bool zoomedPage = false;

		private ForeignFrame(IHTMLElement e) : base(e) 
		{
		}

        public static ForeignFrame AsForeignFrame(IHTMLElement e) {
            if ( (e is IHTMLIFrameElement) || (e is IHTMLFrameElement) ) {
                // Notice: Only Foreign frame element should be passed to this method.
                return new ForeignFrame(e);
            } 
            return null;
        }

        private void ZoomingIntoFrame() {
            if ( zoomedPage ) {
                // we already switched to foreign frame.
                return; 
            }

            string msg = 
                "The selected object is a foreign frame/iframe object.\n"
                +"DHTML API doesn't allow in-place inspection.\n"
                +"Do you want to zoom into the frame/iframe  object?";
            string caption = "Zooming into foreign frame/iframe";

            DialogResult answer = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo);

            if ( answer.Equals(DialogResult.Yes) ) {
                if ( e is HTMLIFrameClass ) {
                    HTMLIFrameClass frame = e as HTMLIFrameClass;
                    Root.mainForm.OpenUrl( frame.src );
                } else if ( e is HTMLFrameElementClass ) {
                    HTMLFrameElementClass frame = e as HTMLFrameElementClass;
                    Root.mainForm.OpenUrl( frame.src );
                }
            }
            zoomedPage = true;
        }

        public override ArrayList ActionSnipList() {
            ZoomingIntoFrame();
            return snipList;
        }

        public override ArrayList AssertSnipList() {
            AssertObjWithId();
            ZoomingIntoFrame();
            return snipList;
        }
	}
}
