#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;
using mshtml;


namespace QuickFocus.QA.Element
{
    [TestCase]
	public class FrameTest : QaTestCase
	{
        string successMsg = "Test finished successfully";
        private void TestPoint(int x, int y) {
            FormChecker.ClickMsgBoxAsync("Zooming into foreign frame/iframe", "&Yes");
            ClickPoint(x, y);
            AssertWaitStatusHas(successMsg);   
        }

        [Test] 
		public void SimpleFrame()
		{
            SetFlags(true,true,false,true,true,true);
            AttachSampleFile("Frames.html");
            TestPoint(120, 100);

            /*
            TestPoint(100, 230);
            TestPoint(100, 490);
            TestPoint(350, 300);
            TestPoint(400, 350);
             */
        }

        [Test] 
        public void NestedFrames() {
            SetFlags(true,true,false,true,true,true);
            AttachSampleFile("TopFrame.html");

            TestPoint(100, 100);
            TestPoint(300, 30);
            TestPoint(280, 150);
            TestPoint(300, 300);
            TestPoint(300, 400);
            TestPoint(400, 400);
            TestPoint(700, 400);
        }

        [Test]
        public void IFrameTest() {
            AttachSampleFile("Iframes.html");

            SetFlags(true,true,false,true,true,true);
            ClickPoint(100, 350);
            AssertWaitStatusHas(successMsg);   

            
            SetFlags(true,true,false,true,true,false);
            ClickPoint(100, 100);
            FormChecker.ClickMessageBox("Zooming into foreign frame/iframe", "&Yes");
            Wait(1500);
        }

        [Test]
        public void ForeignIframe() {
            AttachSampleFile("Iframes2.html"); 
            MoveToPoint(350, 350);
            AssertWaitStatusHas("IFRAME src=\"http://ieunit.sourceforge.net\"");
            Position pos = Root.mainForm.TheScreen.GetClientPosition(Root.mainForm.FocusedElement);
            AssertNotNull(Root.mainForm.FocusedElement.frame);
            AssertTrue(Math.Abs(164 - pos.x)<20);
            AssertTrue(Math.Abs(256 - pos.y)<20);
        }
	}
}

#endif