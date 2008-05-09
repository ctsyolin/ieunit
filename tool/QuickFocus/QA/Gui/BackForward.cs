#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;


namespace QuickFocus.QA.Gui
{
	/// <summary>
	/// Summary description for BackForward.
	/// </summary>
	[TestCase] 
    public class BackForward : QaTestCase
	{
        public override void SetUp() {
            SetDefaultFlags();
            AttachSampleFile("HelloWorld.html");
            WaitMin();
        }

        [Test] 
        public void BackAndForward() {
            AttachSampleFile("HelloWorld.html");
            AttachSampleFile("AnchorLink.html");
            AttachSampleFile("Button.html");
            AppChecker.ClickButtonByName("btnBack");
            AppChecker.ClickButtonByName("btnBack");
            AssertIsHelloPage();
            AppChecker.ClickButtonByName("btnForward");
            AppChecker.ClickButtonByName("btnForward");
            AssertPageTitle("Button Test Page");

        }

        [Test] 
        public void ReloadUrl() {
            AttachSampleFile("HelloWorld.html");
            AttachSampleFile("AnchorLink.html");
            AttachSampleFile("Button.html");
            int counter = Root.appConfig.historyList.Count;
            AttachSampleFile("HelloWorld.html");
            AssertEquals(counter, Root.appConfig.historyList.Count);
        }
	}
}

#endif