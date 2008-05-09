#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;


namespace QuickFocus.QA.Misc {

    [TestCase]
    public class UnicodeTest : QaTestCase {
        string successMsg = "Test finished successfully";
        public override void SetUp() {
            SetDefaultFlags();
        }

        [Test]
        public void RunTestCase () {
            AttachSampleFile("UnicodePage2.html");
            ClearSnipWindow();
            ClickElementById("id0");
            AppChecker.ClickButtonByName("btnTestSnip");
            WaitForStatus(successMsg);
        }
    }
}

#endif 
