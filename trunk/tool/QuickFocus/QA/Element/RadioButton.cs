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
    public class RadioButton : QaTestCase {
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
        public void AssertRadioButtonOne() {
            AttachSampleFile("RadioButton.html");
            SetAssert();

            ClickElementById("idX");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }

        [Test]
        public void AssertRadioButtonAll() {
            AttachSampleFile("RadioButton.html");
            SetAssert();

            foreach(IHTMLElement2 e in FindElementsByTag("Input")) {
                ClickElement(e);
            }

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);           
        }

        [Test]
        public void ClickRadioButtonOne() {
            AttachSampleFile("RadioButton.html");
            SetAction();

            ClickElementById("idY");
            SnippetText +=  "\n"
                + "_.assertFalse(_.findRadioButton('idX').checked);\n"
                + "_.assertTrue(_.findRadioButton('idY').checked);\n"
                + "_.assertFalse(_.findRadioButton('idZ').checked);\n";

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }

        [Test]
        public void ClickRadioButtonAll() {
            AttachSampleFile("RadioButton.html");
            SetAction();

            SnippetText = "_.setTime(100,3000);\n";

            foreach(IHTMLElement2 e in FindElementsByTag("Input")) {
                ClickElement(e);
            }
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }        
    }
}

#endif