#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;
using System.Diagnostics;


namespace QuickFocus.QA.Gui
{
	/// <summary>
	/// Test the context menu of the transparent screen panel.
	/// </summary>
	[TestCase]
    public class ContextMenuTest : QaTestCase
	{
        ContextMenu screenMenu;

        public override void SetUpCase() {
            screenMenu = (FindControl("screenPanel") as Panel).ContextMenu;
        }

        public override void SetUp() {
            AttachSampleFile("TextTable.html");
            Root.appConfig.focusLevel = 0;
        }

        [Test]
        public void ChangeFocusScope() {
            AssertNotNull( screenMenu );
            MenuItem miEnlarge = FormChecker.FindMenuItem(screenMenu, "Enlarge Focus Scope");
            MenuItem miShrink  = FormChecker.FindMenuItem(screenMenu, "Shrink Focus Scope");
            MenuItem miReset   = FormChecker.FindMenuItem(screenMenu, "Reset Focus Scope");

            AssertNotNull( miEnlarge );

            ClickElementById("TheContent");
            AssertStatusHas("<TD id=TheContent>The Content </TD>");
            AssertEquals(0, Root.appConfig.focusLevel);

            Invoke( delegate() {
                miEnlarge.PerformClick();
                WaitMin();
                AssertEquals(1, Root.appConfig.focusLevel);

                miEnlarge.PerformClick();
                miEnlarge.PerformClick();
                miEnlarge.PerformClick();
                WaitMin();
                AssertEquals(4, Root.appConfig.focusLevel);

                miShrink.PerformClick();
                WaitMin();
                // move the mouse to trigger update of the status text.
                AssertEquals(3, Root.appConfig.focusLevel);
                MoveMouseToElement( FindElementById("TheContent") );
                AssertStatusHas("3: \r\n<TABLE id=Table25 border=0>");

                miReset.PerformClick();
                WaitMin();
                AssertEquals(0, Root.appConfig.focusLevel);
            });
        }

        [Test]
        public void DetachPageMenu() {
            MenuItem miDetach  = FormChecker.FindMenuItem(screenMenu, "Detach Page");
            Invoke( delegate() {
                miDetach.PerformClick();
            });
            AssertEquals(AppState.StOpen, Root.mainForm.TheState);
        }

        // Validate the context menu of the snippet window.
        [Test]
        public void SnippetCtxMenu() {
            ContextMenu ctxMenu = FindControl("rtbSnipWin").ContextMenu;
            AssertNotNull(ctxMenu);

            Invoke(delegate() {
                AssertNotNull(FormChecker.FindMenuItem(ctxMenu, "Cu&t"));
                AssertNotNull(FormChecker.FindMenuItem(ctxMenu, "&Copy"));
                AssertNotNull(FormChecker.FindMenuItem(ctxMenu, "&Paste"));
                AssertNotNull(FormChecker.FindMenuItem(ctxMenu, "&Run Snippet"));
                AssertNotNull(FormChecker.FindMenuItem(ctxMenu, "&Debug Snippet..."));
                AssertNotNull(FormChecker.FindMenuItem(ctxMenu, "Clear Snippet"));


                FormChecker.FindMenuItem(ctxMenu, "Cu&t").PerformClick();
                FormChecker.FindMenuItem(ctxMenu, "&Copy").PerformClick();
                FormChecker.FindMenuItem(ctxMenu, "&Paste").PerformClick();
                //FormChecker.FindMenuItem(ctxMenu, "&Run Snippet").PerformClick();
                //FormChecker.FindMenuItem(ctxMenu, "&Debug Snippet...").PerformClick();
                FormChecker.FindMenuItem(ctxMenu, "Clear Snippet").PerformClick();
            });
        }

        [Test]
        public void ElementProperty() {
            MenuItem prop = FormChecker.FindMenuItem(screenMenu, "Element Properties...");
            ClickElementById("TheContent");
            Invoke(delegate() {
                prop.PerformClick();
            });
            ElementProperties propForm = Root.mainForm.TheScreen.PropertyForm;
            AssertNotNull( propForm );
            FormChecker u = new FormChecker(propForm);
            u.Invoke(delegate() {
                TextBox tbox = u.FindControl("tboxElementHtml") as TextBox;
                AssertNotNull(tbox);
                AssertTrue(tbox.Text.IndexOf("The Content") > 0);
                propForm.Close();
            });
        }
	}
}

#endif
