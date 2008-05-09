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
using System.IO;
using System.Reflection;
using mshtml;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for StubGenerator.
	/// </summary>
	public sealed class StubGenerator
	{
        private StubGenerator(){}
        private static int stubIndex;
        private static string CurrentUrl {
            get {
                string url = Root.mainForm.CurrentUrl;
                if ( url.StartsWith("file:///") ) {
                    url = url.Substring(8);
                }
                return url;
            }
        }

        public static string GenerateCaseStub() {
            string stub = 
                "function SampleCase() {\n"
                +"    assimilate(this, new IeUnit());\n"
                +"    this.setUp = function() {\n"
                +"        _.openWindow(\"" + CurrentUrl + "\");\n"
                +"    };\n"
                +"    this.tearDown = function() {\n"
                +"        _.closeWindow();\n"
                +"    };\n"
                +"\n"
                +"    this.testCaseOne = function(){\n"
                +"        _.assertTrue(true);\n"
                +"    };\n"
                +"}\n"
                ;
            return stub;
        }

        public static string GenerateSbkStub(bool issueOpen) {
            IHTMLElement    submitElement;
            PageElement     pe = null;
            StringBuilder   sb = new StringBuilder();

            if ( Root.mainForm.SelectedElement != null ) {
                submitElement = Root.mainForm.SelectedElement.e;
            } else {
                submitElement = null;
            }            

            sb.Append( (issueOpen) ? "_.openWindow(\"" + CurrentUrl + "\");\n" : "" );

            if ( MainForm.SelectedDoc == null ) {
                return sb.ToString();
            }

            // Fill the input fields.
            IHTMLElementCollection eList = MainForm.SelectedDoc.all.tags("INPUT") as IHTMLElementCollection;
            IEnumerator it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                // We issue the submit statement at the end of the snip.
                if ( submitElement == it.Current ) {
                    continue;
                }

                pe = PageElement.ToPageElement(it.Current as IHTMLElement);
                if ( pe != null ) {
                    pe.InSubmitMode = true;

                    if ( !( pe is Button) ) {
                        ArrayList snips = pe.ActionSnipList();
                        if ( snips.Count > 0 ) {
                            sb.Append((string) snips[0]);
                        }
                    }
                }
            }

            eList = MainForm.SelectedDoc.all.tags("SELECT") as IHTMLElementCollection;
            it = eList.GetEnumerator();
            while ( it.MoveNext() ) {
                // We issue the submit statement at the end of the snip.
                if ( submitElement.Equals( it.Current ) ) {
                    continue;
                }

                pe = PageElement.ToPageElement(it.Current as IHTMLElement);
                if ( pe != null ) {
                    ArrayList snips = pe.ActionSnipList();
                    if ( snips.Count > 0 ) {
                        sb.Append((string) snips[0]);
                    }
                }
            }


            pe = PageElement.ToPageElement(submitElement);
            if ( pe != null ) {
                ArrayList snips = pe.ActionSnipList();
                if ( snips.Count > 0 ) {
                    sb.Append( (string) snips[0] );
                }
            }

            return sb.ToString();
        }

        private static string preFilePath = null;

        public static void CleanTemporaryFile() {
            if ( ( preFilePath != null ) &&  File.Exists(preFilePath) ) {
                File.Delete(preFilePath);
            }
        }

        public static bool IsAscii(string code) {
            for(int i=0; i<code.Length; i++) {
                if ( code[i] > 127 ) {
                    return false;
                }
            }
            return true;
        }

        public static DataObject MakeFileDropObject() {
            string code = Root.mainForm.CurrentCode;
            if ( code.Length == 0 ) {
                return null;
            }

            CleanTemporaryFile();

            string fileName = "QuickFocus" + stubIndex;
            string filePath = Root.appConfig.appDirectory + "\\" + fileName + ".";
           
            
            if ( code.StartsWith("function ") ) {
                filePath += "jst";
            } else {
                filePath += "sbk";
            }
            stubIndex ++;
            
            StreamWriter file;
            //Notice: JavaScript can only interpret ASCII and UTF16. 
            // C# by default will use UTF8, so we have to explicitly use
            // UTF16 or ASCII to avoid UTF8.
            if ( IsAscii(code) ) {
                file = new System.IO.StreamWriter(filePath);
            } else {
                file = new System.IO.StreamWriter(filePath,false, System.Text.Encoding.Unicode);
            }

            file.Write(code.Replace("\n", "\r\n"));
            file.Close();

            preFilePath = filePath;

            string[] fileNames = new string[] {filePath};
            DataObject dataObj = new DataObject();
            dataObj.SetData(DataFormats.FileDrop, fileNames);
            return dataObj;
        }
	}
}
