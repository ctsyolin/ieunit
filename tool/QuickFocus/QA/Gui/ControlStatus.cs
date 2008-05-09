#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;


namespace QuickFocus.QA.Gui
{
	/// <summary>
	/// Summary description for ControlStatus.
	/// </summary>
	/// 
    [TestCase]
	public class ControlStatus : QaTestCase
	{
        public override void SetUp() {
            AttachSampleFile("HelloWorld.html");            
        }

        private void AssertHasButton(string tag) {
            System.Windows.Forms.Button btn = FindControl(tag) as System.Windows.Forms.Button;
            AssertNotNull( btn );
        }

        [Test]
        public void CheckAllControls()
        {
            ComboBox cBox = FindControl("cmbbUrl") as ComboBox;
            AssertNotNull( cBox );
            AssertHasButton ( "btnBack" );
            AssertHasButton ( "btnForward" );
            AssertHasButton ( "btnGo" );
            AssertHasButton ( "btnStop" );
            AssertHasButton ( "btnAttach" );
            AssertHasButton ( "btnSubmitStub" );
            AssertHasButton ( "btnTestSnip" );
            AssertHasButton ( "btnCaseStub" );
            AssertHasButton ( "btnSbkStub" );
            AssertHasButton ( "btnClearSnip" );

            //Notice: WebBrowser object has no Name property, but a special tag
            AssertNotNull ( FindControlByTag("browserMain") as AxSHDocVw.AxWebBrowser );
            AssertNotNull ( FindControl("screenPanel")    as Panel );
            AssertNotNull ( FindControl("rtbSnipWin")     as RichTextBox );
            AssertNotNull ( FindControl("statusBar")      as StatusBar );
            AssertNotNull ( FindControl("cbAssertSnip")   as System.Windows.Forms.CheckBox );
            AssertNotNull ( FindControl("cbActionSnip")   as System.Windows.Forms.CheckBox );
            AssertNotNull ( FindControl("cbShowAlternatives")   as System.Windows.Forms.CheckBox );
        }

        private void AssertHasMenu(params string[] menuPath) {
            AssertNotNull ( AppChecker.FindMenuItem(menuPath) );
        }
        [Test]
        public void CheckMainMenu() {
            AssertHasMenu("&File", "&Open File...");
            AssertHasMenu("&File", "E&xit");
            AssertHasMenu("&File", "&Load Script...");
            AssertHasMenu("&File", "&Save Script...");
            AssertHasMenu("&File", "Save Script As...");

            AssertHasMenu("&Tools", "&Logger Window...");
            AssertHasMenu("&Tools", "&Toggle Mode");
            AssertHasMenu("&Tools", "&Debug Snippet...");
            AssertHasMenu("&Tools", "&Toggle Mode");

            AssertHasMenu("&Help", "Online &Help...");
            AssertHasMenu("&Help", "&IeUnit API Help...");
            AssertHasMenu("&Help", "&About QuickFocus...");
        }

        [Test]
        public void ToggleModeByMenu() {
            AssertTrue( FindControl("screenPanel").Visible );
            Invoke(delegate() {
                AppChecker.FindMenuItem("&Tools", "&Toggle Mode").PerformClick();
            });
            WaitMin();
            AssertFalse( FindControl("screenPanel").Visible );
        }
	}
}

#endif