#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using mshtml;
using System.IO;

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections;
using FormCheckBox = System.Windows.Forms.CheckBox;

namespace QuickFocus.QA
{
	/// <summary>
	/// Base TestCase to hold commonly used methods.
	/// </summary>
	public class QaTestCase : TestCase
	{
        protected string homeDir = @"C:\work\IeUnit\";
        private FormChecker appChecker;

        public QaTestCase() {
            appChecker = new FormChecker(Root.mainForm);
        }

        #region Find Methods
        protected Control FindControl(string ctrlName) {
            return AppChecker.FindControl(ctrlName);
        }

        protected Control FindControlByTag(string tag) {
            return AppChecker.FindControlByTag(tag);
        }

        protected IHTMLElement2 FindElementById(string id) {
            return CurrentDoc.all.item(id, 0) as IHTMLElement2;
        }

        protected ArrayList FindElementsByTag(string tagName) {
            ArrayList retList = new ArrayList();
            IHTMLElementCollection elements = CurrentDoc.all.tags(tagName) as IHTMLElementCollection;
            foreach(object obj in elements) {
                retList.Add(obj);
            }
            return retList;
        }

        protected string AppStatus {
            get {
                return (FindControl("statusBar") as StatusBar).Text;
            }
        }

        #endregion


        #region Assert methods

        protected void AssertIsHelloPage() {
            AxSHDocVw.AxWebBrowser browser = FindControlByTag("browserMain") as AxSHDocVw.AxWebBrowser;
            IHTMLDocument2 doc = browser.Document as IHTMLDocument2;
            AssertEquals( "Hello World!", doc.body.innerText);
            AssertEquals("Hello World", doc.title);
        }

        protected void AssertPageTitle(string title) {
            AxSHDocVw.AxWebBrowser browser = FindControlByTag("browserMain") as AxSHDocVw.AxWebBrowser;
            IHTMLDocument2 doc = browser.Document as IHTMLDocument2;
            AssertEquals(title, doc.title);
        }

        protected void AssertPageHasText(string txt) {
            string pageText = CurrentDoc.body.innerText;
            if ( pageText.IndexOf ( txt ) < 0 ) {
                AssertFail("The page doesn't contain " + txt );
            }
        }

        protected void AssertSnipHas(string txt){
            string snip = SnippetText;
            if ( snip.IndexOf(txt) < 0) {
                AssertFail("The snip code \"" + snip + "\" doesn't contain " + txt );
            }
        }

        protected void AssertSnipStartsWith(string txt){
            string snip = SnippetText;
            if (! snip.StartsWith(txt) ) {
                AssertFail("The snip code \"" + snip + "\" doesn't start with \"" + txt + "\"");
            }
        }

        protected void AssertStatusHas(string txt) {
            string status = AppStatus;
            if ( status.IndexOf(txt) < 0) {
                AssertFail("The status text \"" + status + "\" doesn't contain " + txt );
            }
        }

        protected void AssertWaitStatusHas(string txt) {
            WaitForStatus(txt);
            AssertStatusHas(txt);
        }

        #endregion

        protected void SetFlags(bool assertFlag, bool actionFlag, bool altFlag,
            bool commentAltFlag, bool autoClearFlag, bool simulationFlag) 
        {
            Invoke( delegate() {
                (FindControl("cbAssertSnip") as FormCheckBox).Checked = assertFlag;
                (FindControl("cbActionSnip") as FormCheckBox).Checked = actionFlag;
                (FindControl("cbShowAlternatives") as FormCheckBox).Checked = altFlag;
                Root.appConfig.autoClear = autoClearFlag;
                Root.appConfig.simulationMode = simulationFlag;
                Root.appConfig.commentOutAlt = commentAltFlag;
            });
        }

        protected void SetDefaultFlags() {
            SetFlags(true, true, true, true, false, false);
        }
        protected FormChecker AppChecker {
            get { return appChecker; }
        }

        protected void WaitForStatus(string msg) {
            for(int i=0; i<150; i++) {
                if (AppStatus.IndexOf(msg) >= 0 ) {
                    break;
                }
                Wait(200);
            }
        }

        protected void ClearSnipWindow() {
            Invoke(delegate() {
                Root.mainForm.ClearSnipWin();
                WaitMin();
            });
        }

        protected void OpenUrl(string url) {
            ComboBox cBox = FindControl("cmbbUrl") as ComboBox;
            AppChecker.SetText(cBox, url);
            AppChecker.ClickButtonByName("btnGo");
            Application.DoEvents();
            WaitMin();
        }

        protected void OpenSampleFile(string fileName) {
            string filePath = homeDir + @"tool\QuickFocus\samples\" + fileName;
            if ( File.Exists(filePath) ) {
                OpenUrl(filePath);
            } else {
                filePath = homeDir + @"samples\ApiTest\" + fileName;
                if ( File.Exists(filePath) ) {
                    OpenUrl(filePath);
                } else {
                    MsgBox.Alert("Sample file " + fileName + " not found");
                    return;
                }
            }

            // Wait till the page has loaded or attached.
            for(int i=0; i<300; i++) {
                if ( (Root.mainForm.TheState == AppState.StOpen) 
                    || (Root.mainForm.TheState == AppState.StAttached) ){
                    break;
                }
                Wait(100);
            }
        }

        protected void AttachSampleFile(string fileName) {
            OpenSampleFile(fileName);
            AttachPage();
        }


        protected void AttachPage() {
            for(int i=0; i<100; i++) {
                if ( Root.mainForm.TheState == AppState.StAttached ) {
                    return;
                }
                if ( Root.mainForm.TheState == AppState.StOpen ) {
                    AppChecker.ClickButtonByName("btnAttach");
                }
                WaitMin();
            }
        }

        protected void DetachPage() {
            for(int i=0; i<100; i++) {
                if ( Root.mainForm.TheState == AppState.StOpen ) {
                    return;
                }
                if ( Root.mainForm.TheState == AppState.StAttached ) {
                    AppChecker.ClickButtonByName("btnAttach");
                }
                Wait(100);
            }
        }

        public void Wait(int milliseconds) {
            Thread.Sleep(milliseconds);
        }

        public void WaitMin() {
            Thread.Sleep(FormChecker.MinWaitTimeMS);
        }

        private IHTMLDocument2 CurrentDoc {
            get { return MainForm.Browser.Document as IHTMLDocument2; }
        }

        protected void MoveMouseToElement(IHTMLElement2 e2) {
            IHTMLRect       rect    = e2.getBoundingClientRect();
            MoveToPoint( (rect.left + rect.right)/2, (rect.top + rect.bottom)/2 );
            WaitMin();
        }

        protected void ClickElement(IHTMLElement2 e) {
            MoveMouseToElement( e );
            UnsafeNativeMethods.ClickMouse(MouseButtons.Left);
            Application.DoEvents();
            WaitMin();
        }

        protected void MoveToPoint(int x, int y) {                
            Invoke( delegate() {
                Cursor.Position = MainForm.Browser.PointToScreen(new Point(x, y));                
            });
            Application.DoEvents();
        }

        protected void ClickPoint(int x, int y) {
            MoveToPoint(x, y);
            WaitMin();
            UnsafeNativeMethods.ClickMouse(MouseButtons.Left);
            Application.DoEvents();
            WaitMin();
        }

        protected void ClickAreaElement(IHTMLElement2 img, IHTMLElement2 area) {
            IHTMLRect rectImg = img.getBoundingClientRect();
            IHTMLRect rectArea = area.getBoundingClientRect();

            MoveToPoint(
                (rectArea.left + rectArea.right)/2 + img.clientLeft + rectImg.left,
                (rectArea.top + rectArea.bottom)/2 + img.clientTop + rectImg.top );
            WaitMin();
            UnsafeNativeMethods.ClickMouse(MouseButtons.Left);
            Application.DoEvents();
            WaitMin();
        }

        protected void ClickElementById(string id) {
            IHTMLElement2 e = FindElementById(id);
            ClickElement( e );
        }

        public void AppendSnip(string snip) {
            RichTextBox rtb = AppChecker.FindControl("rtbSnipWin") as RichTextBox;
            Invoke( delegate() { rtb.Text = rtb.Text + "\n" + snip; });
        }

        public string SnippetText {
            get {
                RichTextBox rtb = AppChecker.FindControl("rtbSnipWin") as RichTextBox;
                string retText = null;
                Invoke(delegate() { retText = rtb.Text; });
                return retText;
            }
            set {
                RichTextBox rtb = AppChecker.FindControl("rtbSnipWin") as RichTextBox;
                Invoke( delegate() {
                    rtb.Text = value;
                    rtb.SelectionStart = rtb.Text.Length;
                    rtb.SelectionLength = 0;
                });
            }
        }

        /// <summary>
        /// Shortcut to simplify Form.Invoke() call.
        /// </summary>
        public void Invoke(MethodInvokerVoid method) {
            AppChecker.Invoke(method);
        }
    }
}

#endif