#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;
using mshtml;


namespace QuickFocus.QA.Element
{
	/// <summary>
	/// Summary description for MapArea.
	/// </summary>
	[TestCase]
	public class MapArea : QaTestCase
	{
        string successMsg = "Test finished successfully";
    
        public override void SetUp() {
            AttachSampleFile("MapArea.html");
            ClearSnipWindow();
            SnippetText = "_.setTime(100,3000);\n";
        }

        [Test]
        public void MapAreaAssert() {
            SetFlags(true,false,true,false,false,false);
            ClickAreaElement(FindElementById("mapImage"), FindElementById("areaA"));
            ClickAreaElement(FindElementById("mapImage"), FindElementById("areaB"));

            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
        }

        [Test]
        public void MapAreaClick() {
            SetFlags(false,true,true,false,false,false);
            ClickAreaElement(FindElementById("mapImage"), FindElementById("areaB"));
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
            AssertPageHasText("Circle");

            ClearSnipWindow();
            SnippetText = "_.setTime(100,3000);\n";

            ClickAreaElement(FindElementById("mapImage"), FindElementById("areaA"));
            AppChecker.ClickButtonByName("btnTestSnip");
            AssertWaitStatusHas(successMsg);
            AssertPageHasText("Rectangle");
        }
	}
}

#endif
