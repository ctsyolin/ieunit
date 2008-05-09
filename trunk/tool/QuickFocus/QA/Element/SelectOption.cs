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
    public class SelectOption : QaTestCase {
        string successMsg = "Test finished successfully";
    
        private void SetAssert() {
            ClearSnipWindow();
            SetFlags(true,false,true,false,false,false);
        }

        private void SetAction() {
            ClearSnipWindow();
            SetFlags(false,true,true,false,false,false);
        }

        [Test]
        public void AssertSelectTwo() {
            AttachSampleFile("SelectOption.html");
            SetAssert();

            ClickElementById("idA");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   

            ClearSnipWindow();

            ClickElementById("idB");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }

        [Test]
        public void ActionAll() {
            AttachSampleFile("SelectOption.html");
            SetAction();
            // Speedup the test somewhat.
            SnippetText = "_.setTime(50, 1000);\n";
            foreach(IHTMLElement2 e in FindElementsByTag("SELECT")) {
                ClickElement(e);
            }

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);           
        }
    }
}

#endif
