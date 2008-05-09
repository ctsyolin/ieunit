#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;


namespace QuickFocus.QA.Gui
{
	/// <summary>
	/// Check the functionality of the checkbox which controls
	/// the generation of snips triggered by mouse clicks. We don't
	/// check the correctness of the snips here.
	/// </summary>
	[TestCase]
    public class ClickSnip : QaTestCase
	{
        public override void SetUp() {
            AttachSampleFile("AnchorLink.html");
        }

        [Test]
        public void FocusToElement() {
            SetDefaultFlags();
            MoveMouseToElement( FindElementById("idHello") );
            AssertStatusHas("<A id=idHello href=\"HelloWorld.html\">");
        }

        [Test]
        public void GenerateAssertSnip() {
            SetFlags(true,false,false,false,false,false);
            ClickElementById("idHello");
            AssertSnipHas("_.assertTextContains(_.findObjById(\"idHello\"), \"Link with \");");
        }

        [Test]
        public void GenerateActionSnip() {
            SetFlags(false,true,false,false,false,false);
            ClickElementById("idHello");
            AssertSnipHas("_.clickObjById(\"idHello\")");
        }

        [Test]
        public void AlternativeSnip() {
            SetFlags(true,true,true,true,false,false);
            ClickElementById("idHello");

            AssertSnipHas("_.assertTextContains(_.findObjById(\"idHello\"), \"Link with \");");
            AssertSnipHas("_.clickObjById(\"idHello\")");
            AssertSnipHas("  //");
        }
    }
}

#endif

