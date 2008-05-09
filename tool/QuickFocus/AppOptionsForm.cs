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
using System.Globalization;

namespace QuickFocus
{
	/// <summary>
	/// Summary description for AppOptionsForm.
	/// </summary>
	public class AppOptionsForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMaxHistoryLength;
        private System.Windows.Forms.CheckBox cbAutoClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFocusFrameWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMaxKeyLength;
        private System.Windows.Forms.CheckBox cbCommentOutAlt;
        private System.Windows.Forms.CheckBox cbSiumlationMode;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AppOptionsForm()
		{
			InitializeComponent();
            AppConfig cfg = Root.appConfig;
            this.tbFocusFrameWidth.Text = cfg.focusFrameWidth.ToString(CultureInfo.CurrentCulture);
            this.tbMaxHistoryLength.Text = cfg.historyMaxLen.ToString(CultureInfo.CurrentCulture);
            this.tbMaxKeyLength.Text = cfg.maxKeyTextLength.ToString(CultureInfo.CurrentCulture);

            this.cbAutoClear.Checked = cfg.autoClear;
            this.cbCommentOutAlt.Checked = cfg.commentOutAlt;
            this.cbSiumlationMode.Checked = cfg.simulationMode;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMaxHistoryLength = new System.Windows.Forms.TextBox();
            this.cbAutoClear = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFocusFrameWidth = new System.Windows.Forms.TextBox();
            this.cbCommentOutAlt = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMaxKeyLength = new System.Windows.Forms.TextBox();
            this.cbSiumlationMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(112, 144);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 40);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 144);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 40);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Max History Length:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMaxHistoryLength
            // 
            this.tbMaxHistoryLength.Location = new System.Drawing.Point(120, 32);
            this.tbMaxHistoryLength.Name = "tbMaxHistoryLength";
            this.tbMaxHistoryLength.Size = new System.Drawing.Size(32, 20);
            this.tbMaxHistoryLength.TabIndex = 3;
            this.tbMaxHistoryLength.Text = "15";
            // 
            // cbAutoClear
            // 
            this.cbAutoClear.Location = new System.Drawing.Point(200, 32);
            this.cbAutoClear.Name = "cbAutoClear";
            this.cbAutoClear.Size = new System.Drawing.Size(232, 16);
            this.cbAutoClear.TabIndex = 4;
            this.cbAutoClear.Text = "Clear snippet window automatically";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Focus Frame Width:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFocusFrameWidth
            // 
            this.tbFocusFrameWidth.Location = new System.Drawing.Point(120, 64);
            this.tbFocusFrameWidth.Name = "tbFocusFrameWidth";
            this.tbFocusFrameWidth.Size = new System.Drawing.Size(32, 20);
            this.tbFocusFrameWidth.TabIndex = 6;
            this.tbFocusFrameWidth.Text = "2";
            // 
            // cbCommentOutAlt
            // 
            this.cbCommentOutAlt.Checked = true;
            this.cbCommentOutAlt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCommentOutAlt.Location = new System.Drawing.Point(200, 56);
            this.cbCommentOutAlt.Name = "cbCommentOutAlt";
            this.cbCommentOutAlt.Size = new System.Drawing.Size(208, 24);
            this.cbCommentOutAlt.TabIndex = 7;
            this.cbCommentOutAlt.Text = "Comment out alternative statements";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Max Key Text Length:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMaxKeyLength
            // 
            this.tbMaxKeyLength.Location = new System.Drawing.Point(120, 96);
            this.tbMaxKeyLength.Name = "tbMaxKeyLength";
            this.tbMaxKeyLength.Size = new System.Drawing.Size(32, 20);
            this.tbMaxKeyLength.TabIndex = 9;
            this.tbMaxKeyLength.Text = "10";
            // 
            // cbSiumlationMode
            // 
            this.cbSiumlationMode.Location = new System.Drawing.Point(200, 88);
            this.cbSiumlationMode.Name = "cbSiumlationMode";
            this.cbSiumlationMode.Size = new System.Drawing.Size(216, 16);
            this.cbSiumlationMode.TabIndex = 10;
            this.cbSiumlationMode.Text = "Run QuickFocus in simulation mode";
            // 
            // AppOptionsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(450, 208);
            this.Controls.Add(this.cbSiumlationMode);
            this.Controls.Add(this.tbMaxKeyLength);
            this.Controls.Add(this.tbFocusFrameWidth);
            this.Controls.Add(this.tbMaxHistoryLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbCommentOutAlt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbAutoClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AppOptionsForm";
            this.Text = "Application Configuration";
            this.ResumeLayout(false);

        }
		#endregion

        private void btnCancel_Click(object sender, System.EventArgs e) {

            this.Close();
        }

        private void btnOK_Click(object sender, System.EventArgs e) {
            AppConfig cfg = Root.appConfig;
            cfg.focusFrameWidth  = int.Parse(tbFocusFrameWidth.Text,CultureInfo.CurrentCulture);
            cfg.historyMaxLen    = int.Parse(tbMaxHistoryLength.Text,CultureInfo.CurrentCulture);
            cfg.maxKeyTextLength = int.Parse(tbMaxKeyLength.Text,CultureInfo.CurrentCulture);

            cfg.autoClear = cbAutoClear.Checked;
            cfg.commentOutAlt = cbCommentOutAlt.Checked;
            cfg.simulationMode = cbSiumlationMode.Checked;

            Root.mainForm.TheScreen.UpdateFrameWidth();

            this.Close();
        }
	}
}
