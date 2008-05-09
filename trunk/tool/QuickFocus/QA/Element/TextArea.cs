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
    public class TextArea : QaTestCase {
        string successMsg = "Test finished successfully";

        public override void SetUp() {
            AttachSampleFile("TextAreaTest.html");
            SetFlags(true, true, true, false, false, false);
        }

        [Test]
        public void CheckAndTest() {
            ClearSnipWindow();

            this.ClickElementById("textArea1");
            this.AssertSnipHas("_.assertEquals(\"First TextArea\", _.findTextArea(\"textArea1\").value);");
            this.AssertSnipHas("_.assertNotNull(_.findTextArea(\"textArea1\"));");
            this.AssertSnipHas("_.setTextArea(\"textArea1\", \"First TextArea\");");

            this.ClickElementById("textArea2");

            this.ClickElementById("textArea3");
            this.AssertSnipHas("_.assertEquals(\"Third TextArea\", _.findTextArea(\"textArea3\").value);");
            this.AssertSnipHas("_.assertNotNull(_.findTextArea(\"textArea3\"));");
            this.AssertSnipHas("_.setTextArea(\"textArea3\", \"Third TextArea\");");

            AppChecker.ClickButtonByName("btnTestSnip");
            WaitForStatus(successMsg);
        }
    }
}

#endif
