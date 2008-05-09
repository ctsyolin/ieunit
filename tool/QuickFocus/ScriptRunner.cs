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
using mshtml;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for RunScript.
	/// </summary>
	public sealed class ScriptRunner
	{
        private ScriptRunner(){}
        private static string scriptPath;
        private static bool startWithDebugger;
        private static string orgScriptSrc;

        public static string OrgScriptSrc {
            get { return orgScriptSrc; }
            set { orgScriptSrc = value; }
        }

        /// <summary>
        /// Run a test script snip with the smart bookmark runner.
        /// </summary>
        /// <param name="script"></param>
        public static void Run(string script, bool startWithDebugger) {
            script = script.Trim();
            if ( script.Length == 0 ) {
                MsgBox.Alert("No script present in the snip window");
                return;
            }

            ScriptRunner.startWithDebugger = startWithDebugger;

            // Skip the command head
            bool seenOneSlash = false;
            int  idx;
            for(idx=0; idx<script.Length; idx++) {
                char c = script[idx];
                if ( Char.IsWhiteSpace(c) ) {
                    seenOneSlash = false;
                    continue;
                }
                if ( seenOneSlash ) {
                    if ( c == '/' ) {
                        int idx2 = script.IndexOf("\n", idx+1);
                        if ( idx2 < 0 ) {
                            MsgBox.Alert("Script contains only comments");
                            return;
                        }
                        idx = idx2;
                        seenOneSlash = false;
                    } else if ( c == '*' ) {
                        int idx2 = script.IndexOf("*/", idx+1);
                        if ( idx2 < 0 ) {
                            MsgBox.Alert("Script contains only comments");
                            return;
                        }
                        idx = idx2+1;
                        seenOneSlash = false;
                    } else {
                        break;
                    }
                } else {
                    if ( c == '/' ) {
                        seenOneSlash = true;
                    } else {
                        break;
                    }
                }
            }
            script = script.Substring(idx);
            if ( script.Length == 0 ) {
                MsgBox.Alert("No statements in the code snippet");
                return;
            }

            bool isTestCase = script.StartsWith("function ");

            //Check the first statement
            if ( isTestCase ) {
                //
                // The selected snippet is a complete test case, we run it as stand-alone test case.
                //
                scriptPath = Root.appConfig.appDirectory + "\\SnipTest.jst";
            } else {
                //
                // Wrap the script code into a try-catch block.
                //
                if ( script .StartsWith("_.openWindow") ) {
                    script = 
                        "try {\n\n"
                        + script + "\n\n"
                        + "} catch (ex) {\n  this.log(\"Exception: \" + ex.description); \n}\n";
                } else {
                    IHTMLDocument2 doc = MainForm.Browser.Document as IHTMLDocument2;
                    if ( doc == null ) {
                        MsgBox.Alert("No document present in the current page");
                        return;
                    }

                    script = 
                        "try {\n\n"
                        +"_.seekAndSetWindow(\"" + PageElement.EscapeString(doc.title) + "\");\n"
                        + script + "\n\n"
                        + "} catch (ex) {\n  this.log(\"Exception: \" + ex.description);\n}\n";
                }
                scriptPath = Root.appConfig.appDirectory + "\\SnipTest.sbk";
            }

            if( startWithDebugger && (! isTestCase ) ) {
                script = "debugger;\n" + script;
            }
            SaveScript(scriptPath, script);

            Thread runnerThread = new Thread( new ThreadStart(RunnerProc) );
            runnerThread.Start();            
            // Global.mainForm.Invoke(new MethodInvoker(RunnerProc));
        }

        public static void SaveScript(string scriptPath, string script) {
            StreamWriter writer;            
            if ( StubGenerator.IsAscii(script) ) {
                writer = new System.IO.StreamWriter(scriptPath, false, System.Text.Encoding.Default);
            } else {
                // Use unicode the encode the script.
                writer = new System.IO.StreamWriter(scriptPath, false, System.Text.Encoding.Unicode);
            }


            writer.Write(script.Replace("\n", "\r\n"));
            writer.Close();
        }

        /// <summary>
        /// The thread body to execute the test snip
        /// </summary>
        public static void RunnerProc() {
            bool isTestCase = scriptPath.EndsWith(".jst");
            string strSuccess = "Test finished successfully.";

            ProcessStartInfo startInfo = new ProcessStartInfo("cscript.EXE");
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            string cmdOptions = "/NoLogo ";
            if ( startWithDebugger ) {
                cmdOptions += "/X ";
            }
            
            string appOptions = "";
            if ( OrgScriptSrc != null ) {
                appOptions += "-orgsrc \"" + OrgScriptSrc + "\" ";
                int idx = OrgScriptSrc.LastIndexOf("\\");
                if ( idx > 0 ) {
                    appOptions += "-I " + OrgScriptSrc.Substring(0, idx) + " ";
                }
            }

            if ( isTestCase ) {
                string runner = System.Environment.GetEnvironmentVariable("IEUNIT_HOME") + "lib\\IeTextRunner.wsf";
                startInfo.Arguments = cmdOptions +  "\"" + runner + "\" -runfiles \"" + scriptPath + "\" " + appOptions;
            } else {
                string runner = System.Environment.GetEnvironmentVariable("IEUNIT_HOME") + "lib\\SmartBookmark.wsf";
                startInfo.Arguments = cmdOptions +  "\"" + runner + "\" \"" + scriptPath + "\" " + appOptions;
            }
            
            Root.mainForm.SetStatusText("Running snippet...");
            System.Diagnostics.Process proc = Process.Start(startInfo);
            string result = proc.StandardOutput.ReadToEnd();
            result += proc.StandardError.ReadToEnd();
            if ( result.Length > 0 ) {
                MessageBox.Show(result, "QuickFocus - Script Runner Result");
                if ( isTestCase ) {
                    if ( result.IndexOf("Failures: 0") < 0 ) {
                        AppLogger.WriteLine("Test failed: " + result);
                        Root.mainForm.SetStatusText("Test failed.");
                    } else {
                        AppLogger.WriteLine(strSuccess);
                        Root.mainForm.SetStatusText(strSuccess);
                    }
                } else {
                    AppLogger.WriteLine("Test failed: " + result);
                    Root.mainForm.SetStatusText("Test failed: " + result);
                }
            } else {
                AppLogger.WriteLine(strSuccess);
                Root.mainForm.SetStatusText(strSuccess);
            }
        }

    }
}
