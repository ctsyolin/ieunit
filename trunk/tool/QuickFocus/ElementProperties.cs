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
	/// Summary description for ElementProperties.
	/// </summary>
	public class ElementProperties : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.PropertyGrid grid;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox tboxElementHtml;

		public ElementProperties(FramedElement fe)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            tboxElementHtml.Text = fe.e.outerHTML;

            grid.SelectedObject = new ElementProperty(fe);
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
            this.grid = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tboxElementHtml = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.BackColor = System.Drawing.SystemColors.Control;
            this.grid.CommandsVisibleIfAvailable = true;
            this.grid.Dock = System.Windows.Forms.DockStyle.Right;
            this.grid.HelpVisible = false;
            this.grid.LargeButtons = false;
            this.grid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.grid.Location = new System.Drawing.Point(376, 0);
            this.grid.Name = "grid";
            this.grid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.grid.Size = new System.Drawing.Size(232, 310);
            this.grid.TabIndex = 0;
            this.grid.Text = "propertyGrid1";
            this.grid.ViewBackColor = System.Drawing.Color.White;
            this.grid.ViewForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(373, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 310);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // tboxElementHtml
            // 
            this.tboxElementHtml.BackColor = System.Drawing.Color.White;
            this.tboxElementHtml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tboxElementHtml.Location = new System.Drawing.Point(0, 0);
            this.tboxElementHtml.Multiline = true;
            this.tboxElementHtml.Name = "tboxElementHtml";
            this.tboxElementHtml.ReadOnly = true;
            this.tboxElementHtml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tboxElementHtml.Size = new System.Drawing.Size(373, 310);
            this.tboxElementHtml.TabIndex = 2;
            this.tboxElementHtml.Text = "";
            // 
            // ElementProperties
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(608, 310);
            this.Controls.Add(this.tboxElementHtml);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ElementProperties";
            this.Text = "Element Properties";
            this.Resize += new System.EventHandler(this.ElementProperties_Resize);
            this.ResumeLayout(false);

        }
		#endregion

        private void ElementProperties_Resize(object sender, System.EventArgs e) {
            this.splitter1.Left = 200;
        }

	}

    public class ElementProperty {
        FramedElement   fe;
        IHTMLRect       bboxRect;

        public ElementProperty(FramedElement fe) { 
            bboxRect = (fe.e as IHTMLElement2).getBoundingClientRect();
            this.fe = fe; 
        }

        [ Category("HTML") ]
        public string Tag {
            get { return fe.e.tagName; }
        }

        [ Category("HTML") ]
        public string Id {
            get { return fe.e.id; }
        }

        [ Category("HTML") ]
        public string Name {
            get { 
                if ( fe.e is HTMLInputElementClass ) {
                    return (fe.e as HTMLInputElementClass ).name;
                } else {
                    return "";
                }                    
            }
        }

        [ Category("HTML") ]
        public string Type {
            get { 
                if ( fe.e is HTMLInputElementClass ) {
                    return ( fe.e as HTMLInputElementClass ).type; 
                } else {
                    return "";
                }                    
            }
        }

        [ Category("HTML") ]
        public string Value {
            get { 
                if ( fe.e is HTMLInputElementClass ) {
                    return ( fe.e as HTMLInputElementClass ).value; 
                } else {
                    return "";
                }                    
            }
        }

        [ Category("HTML") ]
        public string Href {
            get { 
                if ( fe.e is HTMLAnchorElementClass ) {
                    return ( fe.e as HTMLAnchorElementClass ).href; 
                } else {
                    return "";
                }                    
            }
        }

        [ Category("HTML") ]
        public int SourceIndex {
            get { return fe.e.sourceIndex;}
        }

        [ Category("Geometry") ]
        public int Top {
            get { return bboxRect.top; }
        }

        [ Category("Geometry") ]
        public int Left {
            get { return bboxRect.left; }
        }

        [ Category("Geometry") ]
        public int Width {
            get { return bboxRect.right - bboxRect.left; }
        }

        [ Category("Geometry") ]
        public int Height {
            get { return bboxRect.bottom - bboxRect.top; }
        }

        [ Category("Context") ]
        public bool IsInFrame {
            get {  return fe.IsInFrame; }
        }

        [ Category("Context") ]
        public int ParentTop {
            get { return fe.clientTop; }
        }

        [ Category("Context") ]
        public int ParentLeft {
            get { return fe.clientLeft; }
        }
    }
}
