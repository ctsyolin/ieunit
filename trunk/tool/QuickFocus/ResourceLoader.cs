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
using System.Reflection;
using System.Drawing;
using System.IO;

namespace QuickFocus {
    public sealed class ResourceLoader {
        private static Assembly asm = Assembly.GetExecutingAssembly();

        private ResourceLoader() {}

        /// <summary>
        /// Get a bitmap object form embedded in the current assembly.
        /// </summary>
        /// <param name="bitmapPath">The icon's path in the assembly</param>
        /// <returns>The bitmap object found</returns>
        public static Bitmap GetBitmap(string bitmapPath) {
            Stream bitmapStream = GetEmbeddedSource(bitmapPath);
            return new Bitmap(bitmapStream);
        }

        /// <summary>
        /// Get the bitmap object form a icon object embedded in the current assembly.
        /// </summary>
        /// <param name="iconPath">The icon's path in the assembly</param>
        /// <returns>THe bitmap of the icon</returns>
        public static Bitmap GetIconBitmap(string iconPath) {
            Icon icon = (new Icon( GetEmbeddedSource(iconPath) ));
            return icon.ToBitmap();
        }

        public static Icon GetIcon(string iconPath) {
            return (new Icon( GetEmbeddedSource(iconPath) ));
        }

        private static Stream GetEmbeddedSource(string rcsPath) {
            Stream stream = asm.GetManifestResourceStream(rcsPath);
            return stream;
        }
    }
}
