#if QA_TEST
using System;
using VisuMap.AppTest;
using System.Windows.Forms;
using mshtml;


namespace QuickFocus.QA.Misc
{
    [TestCase]
	public class MultipleNames : QaTestCase {
        [Test]
        public void CheckNonUniqueNames() {
            string successMsg = "Test finished successfully";

            SetFlags(true,true,true,false,true,true);
            AttachSampleFile("MultipleNames.html");
            
            foreach( IHTMLElement e in FindElementsByTag("INPUT")) {
                ClickElement(e as IHTMLElement2);                
                AssertWaitStatusHas(successMsg);   
            }

            foreach( IHTMLElement e in FindElementsByTag("B")) {
                ClickElement(e as IHTMLElement2);                
                AssertWaitStatusHas(successMsg);   
            }

        }
	}
}

#endif