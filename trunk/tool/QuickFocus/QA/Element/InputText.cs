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
    public class InputText : QaTestCase {
        string successMsg = "Test finished successfully";

        public override void SetUp() {
        }

        private void SetAction() {
            ClearSnipWindow();
            SetFlags(false,true,true,false,false,false);
        }
    
        private void SetAssert() {
            ClearSnipWindow();
            SetFlags(true,false,true,false,false,false);
        }

        [Test]
        public void AssertInputField() {
            AttachSampleFile("InputText.html");
            SetAssert();

            ClickElementById("idC");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }

        [Test]
        public void AssertInputFieldAll() {
            AttachSampleFile("InputText.html");
            SetAssert();

            foreach( IHTMLElement e in FindElementsByTag("INPUT")) {
                if ( e.id != "btnSubmit" ) {
                    ClickElement(e as IHTMLElement2);
                }
            }

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }


        [Test]
        public void ActionInputFieldAll() {
            AttachSampleFile("InputText.html");
            SetAction();

            int i = 1;
            foreach( IHTMLElement e in FindElementsByTag("INPUT")) {
                if ( e.id != "btnSubmit" ) {
                    (e as IHTMLInputElement).value = "Value " + i++;
                    ClickElement(e as IHTMLElement2);
                }
            }
            AssertSnipHas("_.setField(\"nameB\", \"Value 2\");");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }
    }
}

#endif
