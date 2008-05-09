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
using mshtml;

namespace QuickFocus
{
	/// <summary>
	/// AppLogger is implemented a singleton object using static attribute.
	/// </summary>
	public class AppLogger : System.Windows.Forms.Form
	{
        private System.Windows.Forms.RichTextBox rtbLoggerWin;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btnClear;
        private static AppLogger appLogger;
        private static string logHistory = "";
        
		public AppLogger()
		{
			InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            this.SetBounds(
                Root.appConfig.infoWinLeft,   Root.appConfig.infoWinTop, 
                Root.appConfig.infoWinWidth,  Root.appConfig.infoWinHeight, 
                BoundsSpecified.All);
            this.rtbLoggerWin.Text = logHistory;
		}

        public static void OpenForm() {
            if ( (appLogger != null) && (appLogger.IsDisposed) ) {
                appLogger = null;
            }
            if ( appLogger == null ) {
                appLogger = new AppLogger();
                appLogger.Show();
            } else {
                appLogger.WindowState = FormWindowState.Normal;
            }
        }

        public static void CloseForm() {
            if( ( appLogger != null ) && (!appLogger.IsDisposed) ){
                appLogger.Close();
            }                                                                        
        }

        protected override void OnClosing(CancelEventArgs e) {
            Root.appConfig.infoWinTop = Top;
            Root.appConfig.infoWinLeft = Left;
            Root.appConfig.infoWinWidth = Width;
            Root.appConfig.infoWinHeight = Height;
            base.OnClosing(e);
        }
        
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        
        private static bool IsOpen {
            get {
                return  ( (appLogger == null) || ( appLogger.IsDisposed) ) ? false : true;
            }
        }

        public static void Write(string txt) {
            logHistory += txt;
            if ( IsOpen ) {
                appLogger.rtbLoggerWin.AppendText(txt);
            }
        }

        public static void WriteLine(string txt) {
            Write(txt + "\n");
        }

        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.rtbLoggerWin = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbLoggerWin
            // 
            this.rtbLoggerWin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLoggerWin.AutoSize = true;
            this.rtbLoggerWin.AutoWordSelection = true;
            this.rtbLoggerWin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLoggerWin.DetectUrls = false;
            this.rtbLoggerWin.Font = new System.Drawing.Font("Lucida Sans Typewriter", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.rtbLoggerWin.Location = new System.Drawing.Point(0, 38);
            this.rtbLoggerWin.Name = "rtbLoggerWin";
            this.rtbLoggerWin.ReadOnly = true;
            this.rtbLoggerWin.Size = new System.Drawing.Size(746, 200);
            this.rtbLoggerWin.TabIndex = 13;
            this.rtbLoggerWin.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClear.Location = new System.Drawing.Point(8, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(48, 24);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // AppLogger
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(744, 238);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rtbLoggerWin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AppLogger";
            this.Text = "QuickFocus Logger Window";
            this.ResumeLayout(false);

        }
		#endregion

        private void btnClear_Click(object sender, System.EventArgs e) {
            this.rtbLoggerWin.Clear();
            logHistory = "";
        }
	}
}
