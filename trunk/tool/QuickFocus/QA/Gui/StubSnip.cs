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
    public class StubSnip : QaTestCase {

        public override void SetUp() {
            AttachSampleFile("HelloWorld.html");            
            SetFlags(true, true, true, true, true, false);
        }

        [Test]
        public void TestCaseStub() {
            AppChecker.ClickButtonByName("btnCaseStub");
            AssertSnipStartsWith("function SampleCase()");
        }

        [Test]
        public void SmartBookmarkStub() {
            AppChecker.ClickButtonByName("btnSbkStub");
            AssertSnipStartsWith("_.openWindow(\"");
        }
    }
}

#endif
