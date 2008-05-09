#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;
using mshtml;


namespace QuickFocus.QA.Element
{
    /// <summary>
    /// Test the generation of submit snip and their test
    /// </summary>
    [TestCase]
    public class InputButton : QaTestCase {
        string successMsg = "Test finished successfully";

        public override void SetUp() {
        }
    
        private void SetAssert() {
            ClearSnipWindow();
            SetFlags(true,false,true,false,false,false);
        }

        [Test]
        public void AssertButtons1() {
            AttachSampleFile("InputText.html");
            SetAssert();

            ClickElementById("btnSubmit");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }


        [Test]
        public void AssertButtonsAll() {
            AttachSampleFile("Button.html");
            SetAssert();

            foreach(IHTMLElement2 e in FindElementsByTag("Button")) {
                ClickElement(e);
            }

            foreach(IHTMLElement2 e in FindElementsByTag("Input")) {
                ClickElement(e);
            }

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);           
        }

        private void SetAction() {
            ClearSnipWindow();
            SetFlags(false,true,true,true,false,false);
        }

        [Test]
        public void ClickButtons1() {
            AttachSampleFile("InputText.html");
            SetAction();

            ClickElementById("btnSubmit");
            // replace all alternative mark by this.win.history.back();
            SnippetText = 
                SnippetText.Replace("//", "_.win.history.back();\n");

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
            AssertIsHelloPage();
        }

        [Test]
        public void ClickButtonsAll() {
            AttachSampleFile("Button.html");
            SetAction();
            string backCmd = "_.win.history.back();_.checkSubmit();\n";
            // Speedup the test somewhat
            SnippetText = "_.setTime(150,3000);\n";

            
            foreach(IHTMLElement2 e in FindElementsByTag("Button")) {
                ClickElement(e);
                SnippetText += backCmd;
            }

            foreach(IHTMLElement2 e in FindElementsByTag("Input")) {
                ClickElement(e);
                SnippetText += backCmd;
            }
            
            // replace all alternative mark by this.win.history.back();
            SnippetText = 
                SnippetText.Replace("  //", backCmd);
        
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
            AssertPageTitle("Button Test Page");
        }

    }
}

#endif