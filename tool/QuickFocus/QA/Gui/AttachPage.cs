#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;

namespace QuickFocus.QA.Gui
{
	/// <summary>
	/// Summary description for AttachPage.
	/// </summary>
	[TestCase]
    public class AttachPage : QaTestCase 
	{
        public override void SetUp() {
            SetDefaultFlags();
            AttachSampleFile("HelloWorld.html");            
        }


        [Test]
        public void OpenHelloPage() {
            AssertIsHelloPage();
        }

        [Test]
        public void AttachDetachPage() {
            AssertTrue( FindControl("screenPanel").Visible );
            DetachPage();
            AssertFalse( FindControl("screenPanel").Visible );
            AttachPage();
            AssertTrue( FindControl("screenPanel").Visible );
        }
    }
}
#endif
