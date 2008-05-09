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
using System.Windows.Forms;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for CommonUtil.
	/// </summary>
	public sealed class CommonUtil
	{
        private CommonUtil() {}
        public static string GetDataFilePath(IDataObject data) {
            string fileName = null;
            string[] fileNames = (string[]) data.GetData(DataFormats.FileDrop);
            if ( (fileNames != null) && (fileNames.Length > 0) ) {
                fileName = fileNames[0];
            } else {
                fileName = (string) data.GetData("UnicodeText");
            }

            return fileName;
        }
    }
}
