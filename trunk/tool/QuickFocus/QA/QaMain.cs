#if QA_TEST
using System;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using VisuMap.AppTest;
using System.IO;

namespace QuickFocus.QA
{
	/// <summary>
	/// Summary description for TestMain.
	/// </summary>
	public class QaMain : UnitTest
	{
        internal TestSuite   suite;

        private QaMain(){}

        public static void StartByArguments(string[] appArgs) {
            if ((appArgs.Length > 0) && (appArgs[0] == "-t")) {
                Start();
            } 
        }

        public static void Start() {
            QaMain qaMain = new QaMain();
            qaMain.suite = new TestSuite();
            qaMain.suite.AddCases( Assembly.GetExecutingAssembly() );
            qaMain.StartTest(true);            
        }

        public static void InitConfig() {
            AppConfig cfg = Root.appConfig;
            cfg.focusLevel = 0;
            cfg.showAlternatives = true;
            cfg.commentOutAlt = true;
            cfg.outActionSnip = true;
            cfg.outAssertSnip = true;
            cfg.pageAttached = false;
            cfg.autoClear = false;
            cfg.simulationMode = false;
            cfg.maxKeyTextLength = 10;

            SetWinGeometry(50, 50, 850, 850, 200);
        }

        private static void SetWinGeometry(int left, int top, int width, int height, int lowPanelHeight) {
            Panel lowPanel = (new FormChecker(Root.mainForm)).FindControl("panelLow") as Panel;
            Root.mainForm.Invoke( new MethodInvoker( delegate(){
                Root.mainForm.SetBounds(left, top, width, height, BoundsSpecified.All);
                lowPanel.Height = lowPanelHeight;
            }) );
        }

        public override void TestMain() {
            InitConfig();
            TestRunnerPanel runnerPanel = new TestRunnerPanel("QuickFocus.QA");
            runnerPanel.CfgFile = "QuickFocusQa.xml";
            runnerPanel.BeforeStart += new TestRunnerPanel.CallbackHandler(runnerPanel_BeforeStart);
            runnerPanel.Run(suite);

            //We need BeginInvoke() because TestMain() is running under separate thread.
            if (! Root.mainForm.IsDisposed) {
                Root.mainForm.BeginInvoke(new MethodInvoker(Root.mainForm.Close));
            }
        }

        void runnerPanel_BeforeStart() {
            // Some tests requires the browser react to mouse movement that requires
            // the browser to be the active window.
            Root.mainForm.Invoke(new MethodInvoker(Root.mainForm.Activate));
        }
	}
}
#endif
