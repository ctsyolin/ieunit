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
    public class CheckBox : QaTestCase {
        string successMsg = "Test finished successfully";

        private void SetAssert() {
            ClearSnipWindow();
            SetFlags(true,false,true,false,false, false);
        }

        private void SetAction() {
            ClearSnipWindow();
            SetFlags(false,true,true,false,false, false);
        }

        [Test]
        public void AssertCheckBox() {
            AttachSampleFile("CheckBox.html");
            SetAssert();

            ClickElementById("idA");
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);   
        }

        [Test]
        public void AssertCheckAll() {
            AttachSampleFile("CheckBox.html");
            SetAssert();

            foreach(IHTMLElement2 e in FindElementsByTag("Input")) {
                ClickElement(e);
            }

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);           
        }

        [Test]
        public void ClickCheckBoxOne() {
            AttachSampleFile("CheckBox.html");
            SetAction();
            string checkCmd = ";\n_.assertFalse(_.findCheckBox('idA').checked);\n"
                +"_.setCheckBox('idA',true);\n";

            ClickElementById("idA");
            SnippetText = 
                SnippetText.Replace(";", checkCmd);       
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }

        [Test]
        public void ClickButtonsAll() {
            AttachSampleFile("CheckBox.html");
            SetAction();


            foreach(IHTMLElement2 e in FindElementsByTag("Input")) {
                ClickElement(e);
            }

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }

    }
}

#endif