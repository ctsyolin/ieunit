/// <copyright from="2004" to="2006" company="VisuMap Technologies Inc.">
///   Copyright (C) VisuMap Technologies Inc.
/// 
///   Permission to use, copy, modify, distribute and sell this 
///   software and its documentation for any purpose is hereby 
///   granted without fee, provided that the above copyright notice 
///   appear in all copies and that both that copyright notice and 
///   this permission notice appear in supporting documentation. 
///   VisuMap Technologies Company makes no representations about the 
///   suitability of this software for any purpose. It is provided 
///   "as is" without explicit or implied warranty. 
/// </copyright>
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using mshtml;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Globalization;


namespace QuickFocus
{

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
        private QfBrowser browserMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbSnipWin;
        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.CheckBox cbAssertSnip;
        private System.Windows.Forms.CheckBox cbActionSnip;
        private System.Windows.Forms.ComboBox cmbbUrl;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolTip tooltipMain;

        private Bitmap bmAttachBtn;
        private Bitmap bmDetachBtn;        
        private Timer checkTimer = new Timer();
        private ScreenPanel screenPanel;

        private System.Windows.Forms.StatusBar statusBar;

        private FramedElement focusedElement;  
        private FramedElement selectedElement;
        private IHTMLDocument2 selectedDoc;
        private System.Windows.Forms.Panel panelInspection;
        private System.Windows.Forms.Panel panelInspRight;
        private System.Windows.Forms.Panel panelUpper;
        private System.Windows.Forms.Panel panelLow;
        private System.Windows.Forms.Panel panelNavBar;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btnTestSnip;
        private System.Windows.Forms.CheckBox cbShowAlternatives;
        private System.Windows.Forms.Button btnSubmitStub;
        private System.Windows.Forms.Button btnCaseStub;
        private System.Windows.Forms.Button btnSbkStub;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClearSnip;
        private System.Windows.Forms.Panel panelDragDrop;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem miOpenFile;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem miExitApp;
        private System.Windows.Forms.MenuItem miTools;
        private System.Windows.Forms.MenuItem miHelpTop;
        private System.Windows.Forms.MenuItem miHelp;
        private System.Windows.Forms.MenuItem miAbout;
        private System.Windows.Forms.MenuItem miOpenLogger;
        private System.Windows.Forms.MenuItem miAppOptions;
        private System.Windows.Forms.MenuItem miToggleMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuItem miFavorites;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem miLoadScript;
        private System.Windows.Forms.MenuItem miSaveScript;
        private System.Windows.Forms.MenuItem miSaveAs;


        AppState appState;
        private System.Windows.Forms.MenuItem miIeUnitHelp;
        private System.Windows.Forms.MenuItem miStartDebugger;
        private System.Windows.Forms.ContextMenu ctxmSnipWin;
        private System.Windows.Forms.MenuItem miCopySnippet;
        private System.Windows.Forms.MenuItem mPasteSnippet;
        private System.Windows.Forms.MenuItem miCutSnippet;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem mClearSnippet;
        private System.Windows.Forms.MenuItem miRunSnippet;
        private System.Windows.Forms.MenuItem miDebugSnippet;
        private string scriptName = null;

		public MainForm()
		{
            Root.mainForm = this;
            Root.appConfig = AppConfig.LoadConfiguration();

            InitializeComponent();

            LoadResources();  // Load the bitmaps

            this.screenPanel.Initialize();
            screenPanel.Visible = false;
            screenPanel.MouseUp   += new MouseEventHandler(screenPanel_MouseUp);
            screenPanel.MouseMove +=new MouseEventHandler(screenPanel_MouseMove);

            SetState(AppState.StInitialized);
            GetConfiguration();
            browserMain.DownloadBegin +=new EventHandler(browserMain_DownloadBegin);
            browserMain.CommandStateChange +=new AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEventHandler(browserMain_CommandStateChange);
            checkTimer.Tick +=new EventHandler(checkTimer_Tick);
            checkTimer.Interval = 500;  // check the document state every 500 milliseconds.

            browserMain_Resize(null, null);
            rtbSnipWin.SizeChanged +=new EventHandler(rtbSnipWin_SizeChanged);

            if ( !string.IsNullOrEmpty(this.cmbbUrl.Text) ) {
                this.OpenUrl(cmbbUrl.Text);
            }

            miFavorites.Select +=new EventHandler(miFavorites_Select);

            rtbSnipWin.DragEnter += new DragEventHandler(snipWin_DragEnter);
            rtbSnipWin.DragDrop +=new DragEventHandler(snipWin_DragDrop);
            rtbSnipWin.MouseEnter += new EventHandler(rtbSnipWin_MouseEnter);     
            Win32Interop.SendMessage(rtbSnipWin.Handle, 
                Win32Interop.EM_SETMARGINS, Win32Interop.EC_LEFTMARGIN|Win32Interop.EC_RIGHTMARGIN, 5);
        }

        public ScreenPanel TheScreen {
            get { return this.screenPanel; }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnGo = new System.Windows.Forms.Button();
            this.rtbSnipWin = new System.Windows.Forms.RichTextBox();
            this.ctxmSnipWin = new System.Windows.Forms.ContextMenu();
            this.miCutSnippet = new System.Windows.Forms.MenuItem();
            this.miCopySnippet = new System.Windows.Forms.MenuItem();
            this.mPasteSnippet = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.miRunSnippet = new System.Windows.Forms.MenuItem();
            this.miDebugSnippet = new System.Windows.Forms.MenuItem();
            this.mClearSnippet = new System.Windows.Forms.MenuItem();
            this.cbActionSnip = new System.Windows.Forms.CheckBox();
            this.cbAssertSnip = new System.Windows.Forms.CheckBox();
            this.btnAttach = new System.Windows.Forms.Button();
            this.panelUpper = new System.Windows.Forms.Panel();
            this.panelNavBar = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbbUrl = new System.Windows.Forms.ComboBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.tooltipMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnTestSnip = new System.Windows.Forms.Button();
            this.cbShowAlternatives = new System.Windows.Forms.CheckBox();
            this.btnSubmitStub = new System.Windows.Forms.Button();
            this.btnCaseStub = new System.Windows.Forms.Button();
            this.btnSbkStub = new System.Windows.Forms.Button();
            this.panelDragDrop = new System.Windows.Forms.Panel();
            this.btnClearSnip = new System.Windows.Forms.Button();
            this.panelInspection = new System.Windows.Forms.Panel();
            this.panelInspRight = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelLow = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.miOpenFile = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.miLoadScript = new System.Windows.Forms.MenuItem();
            this.miSaveScript = new System.Windows.Forms.MenuItem();
            this.miSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.miExitApp = new System.Windows.Forms.MenuItem();
            this.miFavorites = new System.Windows.Forms.MenuItem();
            this.miTools = new System.Windows.Forms.MenuItem();
            this.miOpenLogger = new System.Windows.Forms.MenuItem();
            this.miToggleMode = new System.Windows.Forms.MenuItem();
            this.miStartDebugger = new System.Windows.Forms.MenuItem();
            this.miAppOptions = new System.Windows.Forms.MenuItem();
            this.miHelpTop = new System.Windows.Forms.MenuItem();
            this.miHelp = new System.Windows.Forms.MenuItem();
            this.miIeUnitHelp = new System.Windows.Forms.MenuItem();
            this.miAbout = new System.Windows.Forms.MenuItem();
            this.screenPanel = new QuickFocus.ScreenPanel();
            this.browserMain = new QuickFocus.QfBrowser();
            this.panelUpper.SuspendLayout();
            this.panelNavBar.SuspendLayout();
            this.panelInspection.SuspendLayout();
            this.panelInspRight.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelLow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.browserMain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnGo.BackColor = System.Drawing.SystemColors.Control;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGo.Location = new System.Drawing.Point(632, 0);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(20, 20);
            this.btnGo.TabIndex = 2;
            this.tooltipMain.SetToolTip(this.btnGo, "Navigate to the specified URL.");
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // rtbSnipWin
            // 
            this.rtbSnipWin.AcceptsTab = true;
            this.rtbSnipWin.AllowDrop = true;
            this.rtbSnipWin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSnipWin.AutoSize = true;
            this.rtbSnipWin.AutoWordSelection = true;
            this.rtbSnipWin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbSnipWin.BulletIndent = 4;
            this.rtbSnipWin.ContextMenu = this.ctxmSnipWin;
            this.rtbSnipWin.DetectUrls = false;
            this.rtbSnipWin.Font = new System.Drawing.Font("Lucida Sans Typewriter", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSnipWin.Location = new System.Drawing.Point(3, 0);
            this.rtbSnipWin.Name = "rtbSnipWin";
            this.rtbSnipWin.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtbSnipWin.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbSnipWin.Size = new System.Drawing.Size(437, 146);
            this.rtbSnipWin.TabIndex = 5;
            this.rtbSnipWin.Text = "";
            // 
            // ctxmSnipWin
            // 
            this.ctxmSnipWin.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miCutSnippet,
            this.miCopySnippet,
            this.mPasteSnippet,
            this.menuItem9,
            this.miRunSnippet,
            this.miDebugSnippet,
            this.mClearSnippet});
            // 
            // miCutSnippet
            // 
            this.miCutSnippet.Index = 0;
            this.miCutSnippet.ShowShortcut = false;
            this.miCutSnippet.Text = "Cu&t";
            this.miCutSnippet.Click += new System.EventHandler(this.miCutSnippet_Click);
            // 
            // miCopySnippet
            // 
            this.miCopySnippet.Index = 1;
            this.miCopySnippet.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.miCopySnippet.ShowShortcut = false;
            this.miCopySnippet.Text = "&Copy";
            this.miCopySnippet.Click += new System.EventHandler(this.miCopySnippet_Click);
            // 
            // mPasteSnippet
            // 
            this.mPasteSnippet.Index = 2;
            this.mPasteSnippet.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mPasteSnippet.ShowShortcut = false;
            this.mPasteSnippet.Text = "&Paste";
            this.mPasteSnippet.Click += new System.EventHandler(this.mPasteSnippet_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.Text = "-";
            // 
            // miRunSnippet
            // 
            this.miRunSnippet.Index = 4;
            this.miRunSnippet.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.miRunSnippet.Text = "&Run Snippet";
            this.miRunSnippet.Click += new System.EventHandler(this.miRunSnippet_Click);
            // 
            // miDebugSnippet
            // 
            this.miDebugSnippet.Index = 5;
            this.miDebugSnippet.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.miDebugSnippet.Text = "&Debug Snippet...";
            this.miDebugSnippet.Click += new System.EventHandler(this.miStartDebugger_Click);
            // 
            // mClearSnippet
            // 
            this.mClearSnippet.Index = 6;
            this.mClearSnippet.Text = "Clear Snippet";
            this.mClearSnippet.Click += new System.EventHandler(this.mClearSnippet_Click);
            // 
            // cbActionSnip
            // 
            this.cbActionSnip.Checked = true;
            this.cbActionSnip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActionSnip.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbActionSnip.Location = new System.Drawing.Point(88, 8);
            this.cbActionSnip.Name = "cbActionSnip";
            this.cbActionSnip.Size = new System.Drawing.Size(56, 16);
            this.cbActionSnip.TabIndex = 10;
            this.cbActionSnip.Text = "Action";
            this.tooltipMain.SetToolTip(this.cbActionSnip, "Generate action snips.");
            this.cbActionSnip.CheckedChanged += new System.EventHandler(this.cbActionSnip_CheckedChanged);
            // 
            // cbAssertSnip
            // 
            this.cbAssertSnip.Checked = true;
            this.cbAssertSnip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAssertSnip.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbAssertSnip.Location = new System.Drawing.Point(8, 8);
            this.cbAssertSnip.Name = "cbAssertSnip";
            this.cbAssertSnip.Size = new System.Drawing.Size(72, 16);
            this.cbAssertSnip.TabIndex = 9;
            this.cbAssertSnip.Text = "Assertion";
            this.tooltipMain.SetToolTip(this.cbAssertSnip, "Generate assertion snips.");
            this.cbAssertSnip.CheckedChanged += new System.EventHandler(this.cbAssertSnip_CheckedChanged);
            // 
            // btnAttach
            // 
            this.btnAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAttach.BackColor = System.Drawing.SystemColors.Control;
            this.btnAttach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttach.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttach.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAttach.Location = new System.Drawing.Point(680, 0);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(20, 20);
            this.btnAttach.TabIndex = 4;
            this.tooltipMain.SetToolTip(this.btnAttach, "Attach/Detach to current page");
            this.btnAttach.UseVisualStyleBackColor = false;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // panelUpper
            // 
            this.panelUpper.Controls.Add(this.screenPanel);
            this.panelUpper.Controls.Add(this.browserMain);
            this.panelUpper.Controls.Add(this.panelNavBar);
            this.panelUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUpper.Location = new System.Drawing.Point(0, 0);
            this.panelUpper.Name = "panelUpper";
            this.panelUpper.Size = new System.Drawing.Size(704, 99);
            this.panelUpper.TabIndex = 6;
            // 
            // panelNavBar
            // 
            this.panelNavBar.Controls.Add(this.cmbbUrl);
            this.panelNavBar.Controls.Add(this.btnStop);
            this.panelNavBar.Controls.Add(this.btnForward);
            this.panelNavBar.Controls.Add(this.btnBack);
            this.panelNavBar.Controls.Add(this.label1);
            this.panelNavBar.Controls.Add(this.btnGo);
            this.panelNavBar.Controls.Add(this.btnAttach);
            this.panelNavBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNavBar.Location = new System.Drawing.Point(0, 0);
            this.panelNavBar.Name = "panelNavBar";
            this.panelNavBar.Size = new System.Drawing.Size(704, 24);
            this.panelNavBar.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnStop.BackColor = System.Drawing.SystemColors.Control;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.SystemColors.Control;
            this.btnStop.Location = new System.Drawing.Point(656, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(20, 20);
            this.btnStop.TabIndex = 7;
            this.tooltipMain.SetToolTip(this.btnStop, "Stop downloading current page");
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnForward
            // 
            this.btnForward.BackColor = System.Drawing.SystemColors.Control;
            this.btnForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForward.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForward.ForeColor = System.Drawing.SystemColors.Control;
            this.btnForward.Location = new System.Drawing.Point(24, 0);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(20, 20);
            this.btnForward.TabIndex = 6;
            this.tooltipMain.SetToolTip(this.btnForward, "Forward");
            this.btnForward.UseVisualStyleBackColor = false;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.SystemColors.Control;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBack.Location = new System.Drawing.Point(2, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(20, 20);
            this.btnBack.TabIndex = 5;
            this.tooltipMain.SetToolTip(this.btnBack, "Back");
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "URL:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbbUrl
            // 
            this.cmbbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbbUrl.Location = new System.Drawing.Point(78, 0);
            this.cmbbUrl.MaxDropDownItems = 25;
            this.cmbbUrl.Name = "cmbbUrl";
            this.cmbbUrl.Size = new System.Drawing.Size(555, 21);
            this.cmbbUrl.TabIndex = 4;
            this.cmbbUrl.SelectionChangeCommitted += new System.EventHandler(this.cmbbUrl_SelectionChangeCommitted);
            this.cmbbUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbbUrl_KeyUp);
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBar.Location = new System.Drawing.Point(0, 152);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(704, 16);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusbar";
            // 
            // btnTestSnip
            // 
            this.btnTestSnip.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTestSnip.Location = new System.Drawing.Point(136, 40);
            this.btnTestSnip.Name = "btnTestSnip";
            this.btnTestSnip.Size = new System.Drawing.Size(88, 24);
            this.btnTestSnip.TabIndex = 15;
            this.btnTestSnip.Text = "&Run Snippet";
            this.tooltipMain.SetToolTip(this.btnTestSnip, "Execute selected snippet      Alt+R");
            this.btnTestSnip.Click += new System.EventHandler(this.btnTestSnip_Click);
            // 
            // cbShowAlternatives
            // 
            this.cbShowAlternatives.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbShowAlternatives.Location = new System.Drawing.Point(152, 8);
            this.cbShowAlternatives.Name = "cbShowAlternatives";
            this.cbShowAlternatives.Size = new System.Drawing.Size(80, 16);
            this.cbShowAlternatives.TabIndex = 19;
            this.cbShowAlternatives.Text = "Alternatives";
            this.tooltipMain.SetToolTip(this.cbShowAlternatives, "Generate alternative code");
            this.cbShowAlternatives.CheckedChanged += new System.EventHandler(this.cbShowAlternatives_CheckedChanged);
            // 
            // btnSubmitStub
            // 
            this.btnSubmitStub.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubmitStub.Location = new System.Drawing.Point(8, 48);
            this.btnSubmitStub.Name = "btnSubmitStub";
            this.btnSubmitStub.Size = new System.Drawing.Size(88, 24);
            this.btnSubmitStub.TabIndex = 21;
            this.btnSubmitStub.Text = "&Submit";
            this.tooltipMain.SetToolTip(this.btnSubmitStub, "Generate submit snippet      Alt+S");
            this.btnSubmitStub.Click += new System.EventHandler(this.btnSubmitStub_Click);
            // 
            // btnCaseStub
            // 
            this.btnCaseStub.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCaseStub.Location = new System.Drawing.Point(8, 80);
            this.btnCaseStub.Name = "btnCaseStub";
            this.btnCaseStub.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnCaseStub.Size = new System.Drawing.Size(88, 24);
            this.btnCaseStub.TabIndex = 6;
            this.btnCaseStub.Text = "Test &Case";
            this.tooltipMain.SetToolTip(this.btnCaseStub, "Generate initial test case stub      Alt+C");
            this.btnCaseStub.Click += new System.EventHandler(this.btnCaseStub_Click);
            // 
            // btnSbkStub
            // 
            this.btnSbkStub.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSbkStub.Location = new System.Drawing.Point(8, 16);
            this.btnSbkStub.Name = "btnSbkStub";
            this.btnSbkStub.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSbkStub.Size = new System.Drawing.Size(88, 24);
            this.btnSbkStub.TabIndex = 16;
            this.btnSbkStub.Text = "S&BK Submit";
            this.tooltipMain.SetToolTip(this.btnSbkStub, "Generate smart bookmark snippet      Alt+B");
            this.btnSbkStub.Click += new System.EventHandler(this.btnSbkStub_Click);
            // 
            // panelDragDrop
            // 
            this.panelDragDrop.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelDragDrop.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelDragDrop.Location = new System.Drawing.Point(192, 104);
            this.panelDragDrop.Name = "panelDragDrop";
            this.panelDragDrop.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelDragDrop.Size = new System.Drawing.Size(32, 32);
            this.panelDragDrop.TabIndex = 18;
            this.tooltipMain.SetToolTip(this.panelDragDrop, "Drag this to desktop to copy current snip code to external script file");
            this.panelDragDrop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelDragDrop_MouseDown);
            // 
            // btnClearSnip
            // 
            this.btnClearSnip.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearSnip.Location = new System.Drawing.Point(136, 72);
            this.btnClearSnip.Name = "btnClearSnip";
            this.btnClearSnip.Size = new System.Drawing.Size(88, 24);
            this.btnClearSnip.TabIndex = 23;
            this.btnClearSnip.Text = "C&lear Snippet";
            this.tooltipMain.SetToolTip(this.btnClearSnip, "Clear the snippet window      Alt+L");
            this.btnClearSnip.Click += new System.EventHandler(this.btnClearSnip_Click);
            // 
            // panelInspection
            // 
            this.panelInspection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInspection.Controls.Add(this.panelInspRight);
            this.panelInspection.Controls.Add(this.rtbSnipWin);
            this.panelInspection.Location = new System.Drawing.Point(0, 0);
            this.panelInspection.Name = "panelInspection";
            this.panelInspection.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelInspection.Size = new System.Drawing.Size(704, 146);
            this.panelInspection.TabIndex = 7;
            // 
            // panelInspRight
            // 
            this.panelInspRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInspRight.AutoScroll = true;
            this.panelInspRight.Controls.Add(this.groupBox1);
            this.panelInspRight.Controls.Add(this.btnClearSnip);
            this.panelInspRight.Controls.Add(this.btnTestSnip);
            this.panelInspRight.Controls.Add(this.panelDragDrop);
            this.panelInspRight.Controls.Add(this.cbActionSnip);
            this.panelInspRight.Controls.Add(this.cbShowAlternatives);
            this.panelInspRight.Controls.Add(this.cbAssertSnip);
            this.panelInspRight.Location = new System.Drawing.Point(440, 0);
            this.panelInspRight.Name = "panelInspRight";
            this.panelInspRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelInspRight.Size = new System.Drawing.Size(264, 146);
            this.panelInspRight.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSubmitStub);
            this.groupBox1.Controls.Add(this.btnSbkStub);
            this.groupBox1.Controls.Add(this.btnCaseStub);
            this.groupBox1.Location = new System.Drawing.Point(8, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 112);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create Snippet";
            // 
            // panelLow
            // 
            this.panelLow.Controls.Add(this.statusBar);
            this.panelLow.Controls.Add(this.panelInspection);
            this.panelLow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLow.Location = new System.Drawing.Point(0, 105);
            this.panelLow.Name = "panelLow";
            this.panelLow.Size = new System.Drawing.Size(704, 168);
            this.panelLow.TabIndex = 8;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 99);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(704, 6);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.miFavorites,
            this.miTools,
            this.miHelpTop});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOpenFile,
            this.menuItem3,
            this.miLoadScript,
            this.miSaveScript,
            this.miSaveAs,
            this.menuItem2,
            this.miExitApp});
            this.menuItem1.Text = "&File";
            // 
            // miOpenFile
            // 
            this.miOpenFile.Index = 0;
            this.miOpenFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.miOpenFile.Text = "&Open File...";
            this.miOpenFile.Click += new System.EventHandler(this.miOpenFile_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // miLoadScript
            // 
            this.miLoadScript.Index = 2;
            this.miLoadScript.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.miLoadScript.Text = "&Load Script...";
            this.miLoadScript.Click += new System.EventHandler(this.miLoadScript_Click);
            // 
            // miSaveScript
            // 
            this.miSaveScript.Index = 3;
            this.miSaveScript.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.miSaveScript.Text = "&Save Script...";
            this.miSaveScript.Click += new System.EventHandler(this.miSaveScript_Click);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Index = 4;
            this.miSaveAs.Text = "Save Script As...";
            this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 5;
            this.menuItem2.Text = "-";
            // 
            // miExitApp
            // 
            this.miExitApp.Index = 6;
            this.miExitApp.Text = "E&xit";
            this.miExitApp.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miFavorites
            // 
            this.miFavorites.Index = 1;
            this.miFavorites.Text = "F&avorites";
            // 
            // miTools
            // 
            this.miTools.Index = 2;
            this.miTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOpenLogger,
            this.miToggleMode,
            this.miStartDebugger,
            this.miAppOptions});
            this.miTools.Text = "&Tools";
            // 
            // miOpenLogger
            // 
            this.miOpenLogger.Index = 0;
            this.miOpenLogger.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.miOpenLogger.Text = "&Logger Window...";
            this.miOpenLogger.Click += new System.EventHandler(this.miOpenLogger_Click);
            // 
            // miToggleMode
            // 
            this.miToggleMode.Index = 1;
            this.miToggleMode.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.miToggleMode.Text = "&Toggle Mode";
            this.miToggleMode.Click += new System.EventHandler(this.miToggleMode_Click);
            // 
            // miStartDebugger
            // 
            this.miStartDebugger.Index = 2;
            this.miStartDebugger.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.miStartDebugger.Text = "&Debug Snippet...";
            this.miStartDebugger.Click += new System.EventHandler(this.miStartDebugger_Click);
            // 
            // miAppOptions
            // 
            this.miAppOptions.Index = 3;
            this.miAppOptions.Text = "&Options...";
            this.miAppOptions.Click += new System.EventHandler(this.miAppOptions_Click);
            // 
            // miHelpTop
            // 
            this.miHelpTop.Index = 3;
            this.miHelpTop.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miHelp,
            this.miIeUnitHelp,
            this.miAbout});
            this.miHelpTop.Text = "&Help";
            // 
            // miHelp
            // 
            this.miHelp.Index = 0;
            this.miHelp.Shortcut = System.Windows.Forms.Shortcut.CtrlF1;
            this.miHelp.Text = "Online &Help...";
            this.miHelp.Click += new System.EventHandler(this.miHelp_Click);
            // 
            // miIeUnitHelp
            // 
            this.miIeUnitHelp.Index = 1;
            this.miIeUnitHelp.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.miIeUnitHelp.Text = "&IeUnit API Help...";
            this.miIeUnitHelp.Click += new System.EventHandler(this.miIeUnitHelp_Click);
            // 
            // miAbout
            // 
            this.miAbout.Index = 2;
            this.miAbout.Text = "&About QuickFocus...";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // screenPanel
            // 
            this.screenPanel.AllowDrop = true;
            this.screenPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.screenPanel.BackColor = System.Drawing.Color.Transparent;
            this.screenPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.screenPanel.Location = new System.Drawing.Point(40, 5);
            this.screenPanel.Name = "screenPanel";
            this.screenPanel.Size = new System.Drawing.Size(207, 91);
            this.screenPanel.TabIndex = 1;
            // 
            // browserMain
            // 
            this.browserMain.AllowDrop = true;
            this.browserMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserMain.Enabled = true;
            this.browserMain.Location = new System.Drawing.Point(0, 24);
            this.browserMain.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("browserMain.OcxState")));
            this.browserMain.Size = new System.Drawing.Size(704, 75);
            this.browserMain.TabIndex = 0;
            this.browserMain.Tag = "browserMain";
            this.browserMain.Resize += new System.EventHandler(this.browserMain_Resize);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 273);
            this.Controls.Add(this.panelUpper);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelLow);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "QuickFocus";
            this.panelUpper.ResumeLayout(false);
            this.panelNavBar.ResumeLayout(false);
            this.panelInspection.ResumeLayout(false);
            this.panelInspection.PerformLayout();
            this.panelInspRight.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panelLow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.browserMain)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void LoadResources() {
            this.bmAttachBtn = ResourceLoader.GetBitmap("QuickFocus.Images.AttachButton.bmp");
            this.bmDetachBtn = ResourceLoader.GetBitmap("QuickFocus.Images.DetachButton.bmp");
            bmAttachBtn.MakeTransparent(Color.White);
            bmDetachBtn.MakeTransparent(Color.White);

            this.btnBack.Image = ResourceLoader.GetBitmap("QuickFocus.Images.GoBack.bmp");
            ( this.btnBack.Image as Bitmap ).MakeTransparent(Color.White);
            this.btnForward.Image = ResourceLoader.GetBitmap("QuickFocus.Images.GoForward.bmp");
            ( this.btnForward.Image as Bitmap ).MakeTransparent(Color.White);
            this.btnAttach.Image = bmAttachBtn;           
            this.Icon = ResourceLoader.GetIcon("QuickFocus.QuickFocusApp.ico");
            this.btnGo.Image = ResourceLoader.GetBitmap("QuickFocus.Images.GoToURL.bmp");
            ( this.btnGo.Image as Bitmap ).MakeTransparent(Color.White);
            this.btnStop.Image = ResourceLoader.GetBitmap("QuickFocus.Images.StopDownload.bmp");
            ( this.btnStop.Image as Bitmap ).MakeTransparent(Color.White);
            this.panelDragDrop.BackgroundImage = ResourceLoader.GetBitmap("QuickFocus.Images.CopySnippet.bmp");
            
        }

        /// <summary>
        /// Get the configuration information the AppConfig object.
        /// </summary>
        private void GetConfiguration() {
            this.SuspendLayout();
            AppConfig cfg = Root.appConfig;

            cbActionSnip.Checked = cfg.outActionSnip;
            cbAssertSnip.Checked = cfg.outAssertSnip;
            cbShowAlternatives.Checked = cfg.showAlternatives;

            cmbbUrl.Items.AddRange(GetHistoryList());
            if (cmbbUrl.Items.Count > 0) {
                cmbbUrl.SelectedIndex = 0;
            }
            if ( cfg.mainWinWidth > 0 ) {
                this.SetBounds(
                    cfg.mainWinLeft,   cfg.mainWinTop, 
                    cfg.mainWinWidth,  cfg.mainWinHeight, 
                    BoundsSpecified.All);
                this.panelLow.Height = cfg.browserPanelHeight;
            }
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Put configuration information to the AppConfig object.
        /// </summary>
        private void PutConfiguration() {
            AppConfig cfg = Root.appConfig;
      
            cfg.outActionSnip = this.cbActionSnip.Checked;
            cfg.outAssertSnip = this.cbAssertSnip.Checked;
            cfg.mainWinLeft = this.Left;
            cfg.mainWinTop = this.Top;
            cfg.mainWinWidth = this.Width;
            cfg.mainWinHeight = this.Height;
            cfg.browserPanelHeight = this.panelLow.Height;
            cfg.showAlternatives = this.cbShowAlternatives.Checked;
        }

        protected override void OnClosing(CancelEventArgs e) {
            AppLogger.CloseForm();
            PutConfiguration();
            Root.appConfig.SaveConfiguration();
            StubGenerator.CleanTemporaryFile();
            base.OnClosing(e);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		static string[] appArgs;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
            try {
                appArgs = args;
                Application.Idle += new EventHandler(Application_Idle);
                Application.Run(new MainForm());
            } catch (System.IO.FileNotFoundException ex) {
                MsgBox.Alert(ex.Message + ":" + ex.FileName);
            } catch (Exception ex) {
                MessageBox.Show("Failed to start application: " + ex.ToString());
            }
		}

        public void OpenUrl(string url) {
            object arg1 = 0;
            object arg2 = "";
            object arg3 = "";
            object arg4 = "";
            SetState(AppState.StOpening); 
            browserMain.Navigate(url, ref arg1, ref arg2, ref arg3, ref arg4);
        }


        private void AddToHistory(string url) {
            AppConfig cfg = Root.appConfig;

            int idx = cfg.historyList.IndexOf(url);
            if ( idx == 0 ) {
                return;
            }
            if ( idx > 0 ){
                cfg.historyList.RemoveAt(idx);
            }

            cfg.historyList.Insert(0,url);
            if (cfg.historyList.Count > cfg.historyMaxLen) {
                cfg.historyList.RemoveAt(cfg.historyList.Count-1);
            }
        }

        private string[] GetHistoryList() {
            return (string[]) Root.appConfig.historyList.ToArray(typeof(string));
        }

        private bool IsDocReady(IHTMLDocument2 doc) {
            if ( (doc.readyState == "complete") || (doc.readyState == "interactive") ) {
                return true;
            } else {
                return false;
            }
        }

        private void checkTimer_Tick(object sender, EventArgs e) {
            IHTMLDocument2 doc = browserMain.Document as IHTMLDocument2;

            Application.DoEvents();
            if ( doc == null ) {
                return;
            }

            SetStatusText(doc.readyState);

            //REVISIT: sometime the page stays in 'interactive' state for ever.
            // I have encountered the case once, and Mark Focas reported this too.
            // But, I wasn't able to create repeatable case for this problem.
            // We probably have to accept 'interactive' here!.
            //

            if ( ! IsDocReady(doc) ) {
                return;
            }
            
            //
            // Set the top document object as the selected object. If the page has
            // frames, we will change selectedDoc to the document of the selected
            // frame when the user select an object in a frame.
            //
            // We need to set this to enable GenerateSbkStub() to generate 
            // submit-less stub.
            //
            selectedDoc = doc;

            // If the page has frames we have to wait till all frames are in
            // complete state.
            int frameCount = doc.frames.length;
            if ( frameCount > 0 ) {
                for(int i=0; i<frameCount; i++) {
                    object x = i;
                    object frameObj = doc.frames.item(ref x);
                    DispHTMLWindow2 frameWin = frameObj as DispHTMLWindow2;
                    try {
                        if ( ! IsDocReady( frameWin.document ) ) {
                            return;
                        }
                    } catch ( System.UnauthorizedAccessException ) { }
                }
            }

            checkTimer.Stop();

            AddToHistory(browserMain.LocationURL);
            cmbbUrl.Items.Clear();
            cmbbUrl.Items.AddRange(GetHistoryList());
            
            SetWindowTitle();
            SetState(AppState.StOpen);
            cmbbUrl.Text = browserMain.LocationURL;
            if ( Root.appConfig.pageAttached  ) {
                AttachPage();
            }            
        }

        private void ClearSelection() {
            selectedElement = null;
            focusedElement = null;
        }

        private void SetState(AppState state) {
            appState = state;

            this.SuspendLayout();

            if ( state == AppState.StAttached ) {
                this.btnAttach.Image = bmDetachBtn;
                this.screenPanel.Visible = true;
                browserMain_Resize(null, null);
                this.screenPanel.Visible = true;
            } else {
                this.btnAttach.Image = bmAttachBtn;
                this.screenPanel.Visible = false;
                screenPanel.Reset();
                this.screenPanel.Visible = false;
            }

            switch ( state ) {
                case AppState.StAttached:
                    Cursor.Current = Cursors.Default;
                    SetStatusText("Attached");
                    break;

                case AppState.StAttaching:
                    Cursor.Current = Cursors.WaitCursor;
                    SetStatusText("Attaching");
                    break;

                case AppState.StInitialized:
                    Cursor.Current = Cursors.Default;
                    SetStatusText("Initialized");
                    break;

                case AppState.StOpen:
                    Cursor.Current = Cursors.Default;
                    btnAttach.Enabled = true;
                    SetStatusText("Loaded");
                    break;

                case AppState.StOpening:
                    Cursor.Current = Cursors.WaitCursor;
                    ClearSelection();
                    SetStatusText("Loading");
                    break;
            }

            this.ResumeLayout(false);
        }

        public  AppState TheState {
            get {
                return appState;
            }
        }


        public void AttachPage() {
            SetState(AppState.StAttached);
            Root.appConfig.pageAttached = true;
        }

        public void DetachPage() {
            SetState(AppState.StOpen);
            Root.appConfig.pageAttached = false;
        }

        private void btnAttach_Click(object sender, System.EventArgs e) {
            TogglePageMode();
        }

        public void TogglePageMode() {
            if ( appState == AppState.StOpen ) {
                AttachPage();
            } else if ( appState == AppState.StAttached ){
                DetachPage();
            }
        }

        public void ClearSnipWin() {
            this.rtbSnipWin.Clear();
            Frameset.Reset();
            ScriptRunner.OrgScriptSrc = null;
            scriptName = null;
        }

        private void btnCaseStub_Click(object sender, System.EventArgs e) {
            ClearSnipConditional();
            InsertSnip( StubGenerator.GenerateCaseStub() );
        }

        private void cmbbUrl_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
            if ( e.KeyValue == 13 ) {
                OpenUrl( cmbbUrl.Text );                
            }
        }

        private void btnGo_Click(object sender, System.EventArgs e) {
            OpenUrl( cmbbUrl.Text );
        }

        private void btnBack_Click(object sender, System.EventArgs e) {
            try {
                this.browserMain.GoBack();
            } catch(COMException) { // raised when there is no back destination.
            }
        }

        private void btnForward_Click(object sender, System.EventArgs e) {
            try {
                this.browserMain.GoForward();
            } catch(COMException) { // raised when there is no forward destination
            }
        }

        private void cmbbUrl_SelectionChangeCommitted(object sender, System.EventArgs e) {
            OpenUrl( cmbbUrl.SelectedItem.ToString() );
        }

        /// <summary>
        /// Clear the output window if auto-clear flag is set.
        /// </summary>
        private void ClearSnipConditional() {
            if ( Root.appConfig.autoClear ) {
                ClearSnipWin();
            }
        }


        public static IHTMLDocument2 SelectedDoc {
            get { return Root.mainForm.selectedDoc; }
        }

        public static AxSHDocVw.AxWebBrowser Browser {
            get { return Root.mainForm.browserMain; }
        }

        private void browserMain_DownloadBegin(object sender, EventArgs e) {
            checkTimer.Start();
            SetState(AppState.StOpening);
        }

        private void InsertSnip(string snip) {
            if ( ( snip == null ) || (snip.Length==0) ) {
                return;
            }
            if ( rtbSnipWin.SelectionLength == 0 ) {
                int codePos = rtbSnipWin.SelectionStart;
                rtbSnipWin.Text = rtbSnipWin.Text.Insert(codePos, snip);
                rtbSnipWin.SelectionStart = codePos + snip.Length;
                rtbSnipWin.SelectionLength = 0;
                rtbSnipWin.Focus();
                rtbSnipWin.ScrollToCaret();
            } else {
                int codePos = rtbSnipWin.SelectionStart;
                string newText = rtbSnipWin.Text.Remove(codePos, rtbSnipWin.SelectionLength);
                rtbSnipWin.Text = newText.Insert(codePos, snip);
                rtbSnipWin.SelectionStart = codePos;
                rtbSnipWin.SelectionLength = snip.Length;
                rtbSnipWin.Focus();
                rtbSnipWin.ScrollToCaret();
            }
        }

        private StringBuilder InsertSnipList(StringBuilder strBuilder, ArrayList snips) {
            if ( strBuilder == null ) {
                strBuilder = new StringBuilder();
            }

            if ( snips.Count > 0 ) {
                strBuilder.Append( (string) snips[0] );
            }
            if ( Root.appConfig.showAlternatives ) {
                for(int i=1; i<snips.Count; i++) {
                    if ( Root.appConfig.commentOutAlt ) {
                        strBuilder.Append( "  //" + snips[i]);
                    } else {
                        strBuilder.Append( "  " + snips[i] );
                    }
                }
            }
            return strBuilder;
        }
        private void screenPanel_MouseUp(object sender, MouseEventArgs e) {
            if ( (focusedElement == null) || (focusedElement.e == null) ) {
                return;
            }

            if ( e.Button.Equals(MouseButtons.Right) ) {
                return;
            }


            SetSelectedElement(focusedElement.Clone());
        }

        private void SetSelectedElement(FramedElement fe) {
            selectedElement = fe;
            PageElement pe = PageElement.ToPageElement(fe.e);
            if ( pe == null ) {
                return;
            }

            if ( fe.IsInFrame ) {
                selectedDoc = fe.ContentWindow.document;
            } else {
                selectedDoc = browserMain.Document as IHTMLDocument2;
            }

            screenPanel.ShowSelectMarker(fe);

            ClearSnipConditional();
            
            StringBuilder strBuilder = new StringBuilder();

            if ( cbAssertSnip.Checked || cbActionSnip.Checked ) {
                IHTMLDocument2 doc2 = browserMain.Document as IHTMLDocument2;
                strBuilder = InsertSnipList(strBuilder, Frameset.GetFramesetSnip(doc2, SelectedDoc) );
            }
            
            if ( cbAssertSnip.Checked ) {
                strBuilder = InsertSnipList( strBuilder, pe.AssertSnipList() );
            }

            if ( cbActionSnip.Checked ) {
                pe.ClearSnipList();
                strBuilder = InsertSnipList(strBuilder, pe.ActionSnipList());
            }

            if ( strBuilder.Length > 0 ) {
                string newSnip = strBuilder.ToString();
                InsertSnip( newSnip );
                AppLogger.Write( newSnip );
            }

            if ( Root.appConfig.simulationMode ) {
                if ( pe is InputText ) {
                    btnTestSnip_Click(null, null);
                    this.browserMain.Focus();
                    (fe.e as IHTMLElement2).focus();
                } else if ( pe is SelectOption) {
                    ;
                } else {
                    btnTestSnip_Click(null, null);
                } 
            }
        }

        public void SetStatusFocusedItem() {
            if ( focusedElement != null ) {
                SetStatusText(focusedElement.e.outerHTML);
            }
        }

        private void SetWindowTitle() {
            IHTMLDocument2 doc = browserMain.Document as IHTMLDocument2;
            if ( doc == null ) {
                return;
            }
            if ( (doc.title == null) || (doc.title.Length == 0) ) {
                try {
                    doc.title = "Temporary Title";
                } catch(Exception) { }
            }
            SetWindowTitle(doc.title);
        }

        private void SetWindowTitle(string title) {
            this.Text = "QuickFocus - " + title;
        }

        public void SetStatusText(string txt) {
            this.Invoke( new MethodInvoker( delegate() {
                SetStatusText0(Root.appConfig.focusLevel + ": " + txt);
            }));
        }

        private void SetStatusText0(string txt) {
            this.statusBar.Text = txt;
        }

        static IHTMLElement preItem = null;
        private void screenPanel_MouseMove(object sender, MouseEventArgs e) {
            FramedElement item = screenPanel.FindElement(e.X, e.Y);

            if ( (item == null) || (item.e==null) || (item.e==preItem) ) {
                return;
            }


            preItem = item.e;
            screenPanel.ShowFlushMarker(item);
            focusedElement = item;


            if ( item.e != null ) {                
                SetStatusFocusedItem();
            }

            if ( (item.e != null) && (item.e.tagName == "INPUT") ) {
                IHTMLInputElement e2 = item.e as IHTMLInputElement;
                if ( e2 != null ) {
                    if ( (e2.type=="text") || (e2.type=="password") ) {
                        this.browserMain.Focus();
                        (e2 as IHTMLElement2).focus();
                    }
                }
            }
        }

        private void browserMain_Resize(object sender, System.EventArgs e) { 
            if ( (this.screenPanel == null) || (browserMain.Document==null) ) {
                return;
            }

            this.screenPanel.Top = browserMain.Top;
            this.screenPanel.Left = browserMain.Left;
            this.screenPanel.Width = browserMain.Width - 20; // we always have vertical scrollbar of 20 pixels.
    

            HTMLBodyClass body = (browserMain.Document as IHTMLDocument2).body as HTMLBodyClass;
            if ( (body!=null) && (body.clientWidth != body.scrollWidth) ) {
                // the horizental scrollbar must be active.
                this.screenPanel.Height = browserMain.Height - 20;
            } else {
                this.screenPanel.Height = browserMain.Height;
            }
        }

        private void btnTestSnip_Click(object sender, System.EventArgs e) {
            RunCodeSnippet(false);
        }

        private void RunCodeSnippet(bool withDebugger) {
            string script = null;
            if ( ( rtbSnipWin.SelectedText == null) || (rtbSnipWin.SelectedText.Length==0) ) {
                script = rtbSnipWin.Text;
            } else {
                script = rtbSnipWin.SelectedText;
            }

            if ( (script != null) && (script.Length > 0) ) {
                ScriptRunner.Run(script,withDebugger);
                rtbSnipWin.Focus();
            }
        }

        public string CurrentUrl {
            get { return this.cmbbUrl.Text; }
        }

        public string CurrentCode {
            get { return this.rtbSnipWin.Text; }
        }

        public FramedElement FocusedElement {
            get { return focusedElement; }
            set { focusedElement = value; }
        }

        public FramedElement SelectedElement {
            get { return selectedElement; }
            set {
                SetSelectedElement(value);
            }
        }

        private bool CheckSelectedElement() {
            if ( this.SelectedElement == null ) {
                if ( Root.appConfig.pageAttached ) {
                    MsgBox.Alert("Please select a submit element in the page for\n"
                        + "generating smart bookmark stub.");
                } else {
                    MsgBox.Alert("Please press Ctrl-X then select a submit element in the page for\n"
                        + "generating smart bookmark stub.");
                }
                return false;
            }
            return true;
        }
        
        private void btnSbkStub_Click(object sender, System.EventArgs e) {
            /*
             * Commented out this block so that it works more like the "test case stub": the
             * use can intialize a new SBK stub any time. 
            if ( ! CheckSelectedElement() ) {
                return;
            }
            */
            ClearSnipConditional();
            InsertSnip( StubGenerator.GenerateSbkStub(true) );
        }

        private void panelDragDrop_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            DataObject dataObj = StubGenerator.MakeFileDropObject();
            if (dataObj != null ) {
                DoDragDrop(dataObj, DragDropEffects.Link|DragDropEffects.Link|DragDropEffects.Copy);
            }
            return;                                
        }

        private void btnSubmitStub_Click(object sender, System.EventArgs e) {
            ClearSnipConditional();
            InsertSnip( StubGenerator.GenerateSbkStub(false) );
        }

        private void btnStop_Click(object sender, System.EventArgs e) {
            browserMain.Stop();
        }

        private void rtbSnipWin_SizeChanged(object sender, EventArgs e) {
            // REVISIT: If don't call this, the scroll won't show up when
            // we shrink the window.
            rtbSnipWin.Refresh(); 
        }

        static bool firstTime = true;
        private static void Application_Idle(object sender, EventArgs e) {
            // Make sure we just handle the very first call.
            if ( ! firstTime ) {
                return;
            } else {
                firstTime = false;
            }

#if QA_TEST
            // Pass the argument list and start the test panel if configured.
            QA.QaMain.StartByArguments(appArgs);
#endif
        }

        private void btnClearSnip_Click(object sender, System.EventArgs e) {
            ClearSnipWin();
        }

        private void miExit_Click(object sender, System.EventArgs e) {
            this.Close();
        }

        private void miHelp_Click(object sender, System.EventArgs e) {
            String helpFile = Root.appConfig.appHome + "\\doc\\QuickFocus.chm";
            System.Windows.Forms.Help.ShowHelp(this, helpFile);
        }

        private void miAbout_Click(object sender, System.EventArgs e) {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void miOpenLogger_Click(object sender, System.EventArgs e) {
            AppLogger.OpenForm();      
        }

        private void miOpenFile_Click(object sender, System.EventArgs e) {
            OpenNewHtmlFile();
        }

        public void OpenNewHtmlFile() {
            string fileName = null;
            OpenFileDialog  openFileDialog  = new OpenFileDialog();
            openFileDialog.InitialDirectory = Root.appConfig.currentFileDir;
            openFileDialog.Filter = "All Files|*|HTML files|*.html;*.htm|URL shortcuts|*.url";
            openFileDialog.FilterIndex = 1 ;
            openFileDialog.RestoreDirectory = true ;

            if(openFileDialog.ShowDialog() != DialogResult.OK) {
                openFileDialog.Dispose();
                return;
            }

            fileName = openFileDialog.FileName;
            openFileDialog.Dispose();
            Root.appConfig.currentFileDir = fileName.Substring(0, fileName.LastIndexOf("\\"));

            if ( fileName.EndsWith (".url") ) {
                // fetch the real URL from the *.url file.
                fileName = ScreenPanel.ExtractUrlFromFile(fileName);
            }
            if ( fileName != null ) {
                this.OpenUrl(fileName);
            }
        }

        private void miToggleMode_Click(object sender, System.EventArgs e) {
            TogglePageMode();
        }

        private void miAppOptions_Click(object sender, System.EventArgs e) {
            AppOptionsForm optionsForm = new AppOptionsForm();
            optionsForm.ShowDialog();
        }

        private void browserMain_CommandStateChange(object sender, AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEvent e) {
            const int CSC_NAVIGATEFORWARD = 1;
            const int CSC_NAVIGATEBACK = 2;
            if ( e.command.Equals(CSC_NAVIGATEFORWARD) ) {
                this.btnForward.Enabled = e.enable;
            } else if ( e.command.Equals(CSC_NAVIGATEBACK) ) {
                this.btnBack.Enabled = e.enable;
            }
        }

        private string MakeSafeFileName(string fileName){
            char[] newName = fileName.ToCharArray();
            for(int i=0; i<newName.Length; i++) {
                if ( Char.IsLetterOrDigit(newName[i]) || Char.IsWhiteSpace(newName[i]) ) {
                } else {
                    newName[i] = ' ';
                }
            }
            return new string(newName);
        }

        private DataObject MakeUrlDataObject() {
            string path = System.IO.Path.GetTempPath();
            string fileName;
            string filePath;
            try {
                IHTMLDocument2 doc = browserMain.Document as IHTMLDocument2;
                if ( (doc != null) && (doc.title!=null) && (doc.title.Length>0) ) {
                    fileName = MakeSafeFileName(doc.title) + ".url";
                } else {
                    fileName = "Temp QuickFocus.url";
                }
                filePath = System.IO.Path.Combine(path, fileName); 

                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine("[DEFAULT]");
                sw.WriteLine("BASEURL=" + cmbbUrl.Text);
                sw.WriteLine("[InternetShortcut]");
                sw.WriteLine("URL=" + cmbbUrl.Text);
                sw.Close();
                DataObject dObj = new DataObject();
                dObj.SetData(DataFormats.FileDrop, new string[] {filePath});
                return dObj;
            } catch (Exception ex) {
                MsgBox.Alert(ex.GetType().ToString() + ":" + ex.Message);
                throw;
            }
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
        protected override void WndProc(ref Message m) {
            // Listen for operating system messages.
            const int WM_NCLBUTTONDOWN = 0x00A1;

            switch (m.Msg) {
                case WM_NCLBUTTONDOWN: {
                    int x = ((int)m.LParam) & 0xffff;
                    int y = ((int)m.LParam >>16) & 0xffff;
                    Point pos = PointToClient(new Point(x, y));
                    if ( (1<pos.X) && ( pos.X<20) && (-42<pos.Y) && (pos.Y<-20) ) {
                        // Start drag-and-drop operation.

                        DataObject dataObj = MakeUrlDataObject();

                        DoDragDrop(dataObj, DragDropEffects.All);
                        return;
                    }
                }
                break;

            }
            base.WndProc(ref m);
        }

        #region Favorites Menu

        private string GetFavoritePath(MenuItem mi) {
            string path;

            if ( mi == this.miFavorites ) {
                path = Environment.GetFolderPath( Environment.SpecialFolder.Favorites);
            } else {
                path = mi.Text;
                while( mi.Parent != this.miFavorites ) {
                    path = (mi.Parent as MenuItem).Text + "\\" + path;
                    mi = (MenuItem) mi.Parent;                    
                }
                path = Environment.GetFolderPath( Environment.SpecialFolder.Favorites) + "\\" + path;
            }

            return path;
        }

        private void FillFavoritesMenu(MenuItem mi) {

            if ( (! mi.IsParent) && (mi!=miFavorites) ) {
                return;
            }

            if (mi.MenuItems.Count > 1) {
                // we alread filled this menu
                return;
            }
            mi.MenuItems.Clear();

            string path = GetFavoritePath( mi );
            
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] subdirs = dir.GetDirectories();
            foreach(DirectoryInfo sdir in subdirs) {
                MenuItem subItem = new MenuItem(sdir.Name);
                subItem.Select += new EventHandler(miFavorites_Select);
                subItem.MenuItems.Add("?");
                mi.MenuItems.Add(subItem);               
            }

            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo f in files) {
                if ( f.Name.EndsWith(".url") ) {               
                    MenuItem subItem = new MenuItem(f.Name.Substring(0, f.Name.Length-4));
                    subItem.Select += new EventHandler(miFavorites_Select);
                    subItem.Click +=new EventHandler(favoritesSubItem_Click);
                    mi.MenuItems.Add(subItem);
                }
            }
        }

        private void miFavorites_Select(object sender, EventArgs e) {
            this.SuspendLayout();
            FillFavoritesMenu((MenuItem) sender);
            this.ResumeLayout(false);
        }

        private void favoritesSubItem_Click(object sender, EventArgs e) {
            MenuItem mi = (MenuItem) sender;
            string path = GetFavoritePath(mi);
            OpenUrl( ScreenPanel.ExtractUrlFromFile(path + ".url") );
        }

        #endregion

        public void LoadScript(string scriptPath) {
            try {
                using (StreamReader sr = new StreamReader(scriptPath)) {
                    ScriptRunner.OrgScriptSrc = scriptPath;
                    this.rtbSnipWin.Text = sr.ReadToEnd();
                    scriptName = scriptPath;
                    rtbSnipWin_MouseEnter(null, null);
                }
            } catch(FileNotFoundException) {
                MsgBox.Alert("Can't read script file " + scriptPath);
            }
        }

        private void miLoadScript_Click(object sender, System.EventArgs e) {
            OpenFileDialog  openFileDialog  = new OpenFileDialog();
            openFileDialog.InitialDirectory = Root.appConfig.currentScriptDir;
            openFileDialog.Filter = "IeUnit Test Case(*.jst)|*.jst|Smart Bookmark(*.sbk)|*.sbk|All Files|*";
            openFileDialog.FilterIndex = 1 ;
            openFileDialog.RestoreDirectory = true ;

            if(openFileDialog.ShowDialog() != DialogResult.OK) {
                openFileDialog.Dispose();
                return;
            }

            string fileName = openFileDialog.FileName;
            openFileDialog.Dispose();
            Root.appConfig.currentScriptDir = fileName.Substring(0, fileName.LastIndexOf("\\"));
            LoadScript(fileName);
        }

        private void SaveScriptFile(string fileName) {
            try {
                ScriptRunner.SaveScript(fileName, this.rtbSnipWin.Text);
            } catch(FileNotFoundException) {
                MsgBox.Alert("Can't write to script file " + fileName);
            }        
        }

        private void miSaveScript_Click(object sender, System.EventArgs e) {
            if ( scriptName == null ) {
                miSaveAs_Click(null,null);
            } else {
                SaveScriptFile(scriptName);
            }
        }

        private void miSaveAs_Click(object sender, System.EventArgs e) {
            SaveFileDialog  saveFileDialog  = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Root.appConfig.currentScriptDir;
            saveFileDialog.Filter = "IeUnit Test Case(*.jst)|*.jst|Smart Bookmark(*.sbk)|*.sbk|All Files|*";
            saveFileDialog.FilterIndex = 1 ;
            saveFileDialog.RestoreDirectory = true ;

            if(saveFileDialog.ShowDialog() != DialogResult.OK) {
                saveFileDialog.Dispose();
                return;
            }

            scriptName = saveFileDialog.FileName;
            saveFileDialog.Dispose();
            Root.appConfig.currentScriptDir = scriptName.Substring(0, scriptName.LastIndexOf("\\"));

            SaveScriptFile(scriptName);
        }

        private void miIeUnitHelp_Click(object sender, System.EventArgs e) {
            String helpFile = System.Environment.GetEnvironmentVariable("IEUNIT_HOME") + "doc\\ApiDoc\\files.html";
            System.Windows.Forms.Help.ShowHelp(this, helpFile);        
        }

        private void cbShowAlternatives_CheckedChanged(object sender, System.EventArgs e) {
            Root.appConfig.showAlternatives = ((System.Windows.Forms.CheckBox)sender).Checked;
        }

        private void cbActionSnip_CheckedChanged(object sender, System.EventArgs e) {
            Root.appConfig.outActionSnip = ((System.Windows.Forms.CheckBox)sender).Checked;
        }
       
        private void cbAssertSnip_CheckedChanged(object sender, System.EventArgs e) {
            Root.appConfig.outAssertSnip = ((System.Windows.Forms.CheckBox)sender).Checked;        
        }

        private void miStartDebugger_Click(object sender, System.EventArgs e) {
            RunCodeSnippet(true);
        }

        private void snipWin_DragEnter(object sender, DragEventArgs e) {
            string fileName = CommonUtil.GetDataFilePath(e.Data);
            if ( fileName != null ) {
                string fn = fileName.ToLower(CultureInfo.CurrentCulture);
                if ( fn.EndsWith(".jst") || fn.EndsWith(".sbk") ) {
                    e.Effect = DragDropEffects.Copy;
                }
            } else {
                e.Effect = DragDropEffects.None;
            }
        }


        private void snipWin_DragDrop(object sender, DragEventArgs e) {
            string fileName = CommonUtil.GetDataFilePath(e.Data);
            if ( fileName == null ) {
                return;
            }
            LoadScript(fileName);
        }

        /// <summary>
        /// Allow redirect windows message to the main form.
        /// </summary>
        /// <param name="msg"></param>
        internal void SendWndMsg(ref Message msg) {
            this.PreProcessMessage(ref msg);
        }

        private void rtbSnipWin_MouseEnter(object sender, EventArgs e) {
            if ( scriptName == null ) {
                SetStatusText0("Script: [None]");
            } else {
                SetStatusText0("Script: " + scriptName);
            }
        }

        private void miCutSnippet_Click(object sender, System.EventArgs e) {
            rtbSnipWin.Cut();
        }

        private void miCopySnippet_Click(object sender, System.EventArgs e) {
            rtbSnipWin.Copy();
        }

        private void mPasteSnippet_Click(object sender, System.EventArgs e) {
            rtbSnipWin.Paste(DataFormats.GetFormat(DataFormats.Text));
        }

        private void mClearSnippet_Click(object sender, System.EventArgs e) {
            rtbSnipWin.Clear();
        }

        private void miRunSnippet_Click(object sender, System.EventArgs e) {
            RunCodeSnippet(false);
        }
    }

    public class QfBrowser : AxSHDocVw.AxWebBrowser {
        private bool isControlDown = false;

        public override bool PreProcessMessage(ref Message msg) {
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP   = 0x101;

            int     ch = msg.WParam.ToInt32();
            bool    msgProcessed = false;
            switch (msg.Msg) {
                case WM_KEYDOWN:
                if ( ch == 17 ) { // Control Key
                    isControlDown = true;
                } else {
                    if ( isControlDown ) {
                        char theChar = Convert.ToChar(ch);
                        if ("OASLXD".IndexOf(theChar) >= 0) {
                            Root.mainForm.SendWndMsg(ref msg);
                            msgProcessed = true;
                            //
                            // most ctrl key handles will open popup windows which consume
                            // the next WM_KEYUP(Ctrl) message. We switch isControlDown=false
                            // explicitely here.
                            //
                            if ( theChar != 'X' ) { // Ctrl+X won't open a new window.
                                isControlDown = false;
                            }
                        }
                    } 
                    
                    //
                    // F1 key has been pressed. Regardless of Ctrl key status
                    // we have forward the message the the main form.
                    //
                    if ( ch == 112 ) { 
                        Root.mainForm.SendWndMsg(ref msg);
                        msgProcessed = true;
                        isControlDown = false;
                    }
                }
                break;

                case WM_KEYUP:
                if ( ch == 17 ) { // Control Key
                    isControlDown = false;
                }
                break;
            }

            if ( ! msgProcessed ) {
                return base.PreProcessMessage(ref msg);
            } else {
                return true;
            }
        }
    }

    public enum AppState {
        StInitialized,
        StOpening,
        StOpen,
        StAttaching,
        StAttached
    };
}
