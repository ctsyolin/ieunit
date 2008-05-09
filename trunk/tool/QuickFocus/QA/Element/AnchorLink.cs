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
    public class AnchorLink : QaTestCase {
        string successMsg = "Test finished successfully";

        public override void SetUp() {
        }
    
        private void SetAssert() {
            SetFlags(true,false,true,true,false,false);
            ClearSnipWindow();
        }

        private void SetAction() {
            SetFlags(false,true,true,true,false,false);
            ClearSnipWindow();
        }

        [Test]
        public void AssertLink1() {
            AttachSampleFile("AnchorLink.html");
            SetAssert();

            ClickElementById("idHello");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }

        [Test]
        public void AssertLinkAll() {
            AttachSampleFile("AnchorLink.html");
            SetAssert();

            foreach(IHTMLElement2 e in FindElementsByTag("A")) {
                ClickElement(e);
            }
            
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }


        [Test]
        public void ClickLink1() {
            AttachSampleFile("AnchorLink.html");
            SetAction();

            ClickElementById("idHello");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
            this.AssertIsHelloPage();
        }

        [Test]
        public void ClickLinkAll() {
            AttachSampleFile("AnchorLink.html");
            SetAction();
            string backCmd = "_.win.history.back();_.checkSubmit();\n";
            // Speedup the test somewhat
            SnippetText = "_.setTime(150, 5000);\n";

            foreach(IHTMLElement2 e in FindElementsByTag("A")) {
                ClickElement(e);
                SnippetText += backCmd;
            }

            // replace all alternative mark by this.win.history.back();
            SnippetText = 
                SnippetText.Replace("  //", backCmd);

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }

        [Test]
        public void ClickLinkDuplicateLabel() {
            AttachSampleFile("AnchorLink.html");
            SetAction();
            SnippetText = "_.setTime(320, 3000);\n";

            ClickElement( (IHTMLElement2)FindElementsByTag("A")[0] );
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
            this.AssertIsHelloPage();

            ClearSnipWindow();

            // The second link has the same text but points to a different page.
            AppChecker.ClickButtonByName("btnBack");
            ClickElement( (IHTMLElement2)FindElementsByTag("A")[1] );
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
            this.AssertPageTitle("Button Test Page");
        }

        [Test]
        public void MultiLineAnchor() {
            AttachSampleFile("AnchorLink.html");
            SetAssert();

            ClickElementById("idML");
            AssertSnipHas("findLink(\"3\\r\\nLines\\r\\n");
        }
    }
}

#endif
