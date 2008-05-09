#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;


namespace QuickFocus.QA.Gui
{
    /// <summary>
    /// Test the generation of submit snip and their test
    /// </summary>
    [TestCase]
    public class SubmitSnip : QaTestCase {
        public override void SetUp() {
            SetDefaultFlags();
            AttachSampleFile("InputText.html");
        }

        [Test]
        public void GenerateSubmitSnip() {
            ClickElementById("btnSubmit");
            AppChecker.ClickButtonByName("btnSubmitStub");
            AssertSnipHas("_.setField(\"nameA\", \"aaaa\");");
            AssertSnipHas("_.clickObjById(\"btnSubmit\");");
        }

        [Test]
        public void CheckTestSnip() {
            ClickElementById("btnSubmit");
            ClearSnipWindow();
            AppChecker.ClickButtonByName("btnSubmitStub");
            AppChecker.ClickButtonByName("btnTestSnip");
            WaitForStatus("Test finished successfully");
            AssertStatusHas("Test finished successfully");
            AssertIsHelloPage();
        }
    }
}

#endif

