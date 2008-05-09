#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;


namespace QuickFocus.QA.Gui
{

    [TestCase]
    public class RunSnippet : QaTestCase {
        string successMsg = "Test finished successfully";
        public override void SetUp() {
            SetDefaultFlags();
            AttachSampleFile("InputText.html");
        }

        [Test]
        public void RunTestCase () {
            ClearSnipWindow();
            AppChecker.ClickButtonByName("btnCaseStub");
            AppChecker.ClickButtonByName("btnTestSnip");
            string msg = FormChecker.ClickMessageBox("QuickFocus - Script Runner Result", "OK");
            WaitForStatus(successMsg);
            AssertTrue(msg.IndexOf("Failures: 0")>=0);
            AssertStatusHas(successMsg);
        }

        [Test]
        public void RunSubmitSnippet () {
            // select on the Submit button.
            ClickElementById("btnSubmit");
            ClearSnipWindow();
            AppChecker.ClickButtonByName("btnSubmitStub");
            AppChecker.ClickButtonByName("btnTestSnip");
            WaitForStatus(successMsg);
            AssertStatusHas(successMsg);
            AssertIsHelloPage();
        }

        [Test]
        public void LoadAndRunScript() {
            Root.mainForm.Invoke(new MethodInvoker(delegate() {
                Root.mainForm.LoadScript(homeDir + @"samples\ApiTest\FindTag.jst");
            }));
            AppChecker.ClickButtonByName("btnTestSnip");
            string msg = FormChecker.ClickMessageBox("QuickFocus - Script Runner Result", "OK");
            WaitForStatus(successMsg);
            AssertTrue(msg.IndexOf("Failures: 0")>=0);
            AssertStatusHas(successMsg);
        }

        [Test]
        public void RunSbkSnippet () {
            // select on the Submit button.
            ClickElementById("btnSubmit");
            ClearSnipWindow();
            AppChecker.ClickButtonByName("btnSbkStub");
            AppendSnip("_.closeWindow();");
            AppChecker.ClickButtonByName("btnTestSnip");
            WaitForStatus(successMsg);
            AssertStatusHas(successMsg);
        }

        [Test]
        public void SkipComment() {
            // select on the Submit button.
            ClickElementById("btnSubmit");
            ClearSnipWindow();
            AppChecker.ClickButtonByName("btnSubmitStub");
            // Add some comments to the code snippet
            SnippetText = 
                "   // First comment \n"
                + "////////\n"
                + "// aaa   \n"
                + "//\n"
                + "\n\n"
                + "/*aaaaaaa*/\n"
                + "/**/\n"
                + "/*\nddddd\naaaaaa*/\n" 
                + "/*\n*/"
                + SnippetText;

            AppChecker.ClickButtonByName("btnTestSnip");
            WaitForStatus(successMsg);
            AssertStatusHas(successMsg);
            AssertIsHelloPage();
        }
    }
}

#endif 
