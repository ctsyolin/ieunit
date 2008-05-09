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
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace QuickFocus {
    /// <summary>
    /// Summary description for AppConfig.
    /// </summary>
    public class AppConfig {
        public AppConfig() {}

        private static string        appCfgFile;
        
        public int  focusLevel      = 0;     // default focus level
        public bool outActionSnip   = true;
        public bool outAssertSnip   = true;
        public int  historyMaxLen   = 32;
        public bool autoClear       = false;
        public int  infoWinTop      = 250;
        public int  infoWinLeft     = 250;
        public int  infoWinWidth    = 900;
        public int  infoWinHeight   = 300;
        public bool pageAttached    = true;
        public int  focusFrameWidth = 2;
        public int  mainWinTop      = -1;
        public int  mainWinLeft     = -1;
        public int  mainWinWidth    = -1;
        public int  mainWinHeight   = -1;

        public string   appDirectory= null;   // the directory of the application's executable
        public string   appHome     = null;        // Home directory of IeUnit
        public int      browserPanelHeight  = -1;
        public bool     showAlternatives    = false;
        public bool     commentOutAlt       = true;
        public int      maxKeyTextLength    = 16;
        public string   currentFileDir      = ".";
        public string   currentScriptDir    = ".";
        public bool     simulationMode       = false;

        public ArrayList historyList = new ArrayList();

        public static AppConfig LoadConfiguration() {
            string exeLocation    = Assembly.GetExecutingAssembly().Location;
            string appDir = exeLocation.Substring(0, exeLocation.LastIndexOf('\\'));
            appCfgFile = appDir + "\\QuickFocus.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(AppConfig));            
            FileStream fs = null;
            AppConfig  appCfg;
            try {
                fs = new FileStream(appCfgFile, FileMode.Open);
                appCfg = (AppConfig) serializer.Deserialize(fs);
                fs.Close();
            } catch(FileNotFoundException) {
                if ( fs != null ) {
                    fs.Close();
                }
                appCfg = new AppConfig();
            }
            
            appCfg.appDirectory = appDir;
            int idx = appDir.LastIndexOf("\\IeUnit\\");
            if ( idx >= 0) {
                appCfg.appHome = appDir.Substring(0, idx + 7);
            } else {               
                idx = appDir.LastIndexOf("\\QuickFocus\\");
                if ( idx >= 0 ) {
                    // QuickFocus is installed in separat directory.
                    appCfg.appHome = appDir.Substring(0, idx + 11);
                } else {
                    MsgBox.Alert("QuickFocus can only be installed in IeUnit or QuickFocus directory");
                    Application.Exit();
                }
            }
            Root.appConfig = appCfg;
            return appCfg;
        }

        public void SaveConfiguration() {
            if ( historyList.Count > historyMaxLen ) {
                historyList.RemoveRange(historyMaxLen, historyList.Count-historyMaxLen);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(AppConfig));
            StreamWriter writer = new StreamWriter(appCfgFile);
            serializer.Serialize(writer, this);
        }
    }
}
