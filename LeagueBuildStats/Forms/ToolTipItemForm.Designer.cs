namespace LeagueBuildStats.Forms
{
	partial class ToolTipItemForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.picBoxItem = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.richEditControl1 = new DevExpress.XtraRichEdit.RichEditControl();
			((System.ComponentModel.ISupportInitialize)(this.picBoxItem)).BeginInit();
			this.SuspendLayout();
			// 
			// picBoxItem
			// 
			this.picBoxItem.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picBoxItem.Location = new System.Drawing.Point(3, 3);
			this.picBoxItem.Name = "picBoxItem";
			this.picBoxItem.Size = new System.Drawing.Size(48, 48);
			this.picBoxItem.TabIndex = 1;
			this.picBoxItem.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(57, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 18);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.Gold;
			this.label2.Location = new System.Drawing.Point(57, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 18);
			this.label2.TabIndex = 3;
			this.label2.Text = "label2";
			// 
			// richEditControl1
			// 
			this.richEditControl1.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
			this.richEditControl1.Appearance.Text.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richEditControl1.Appearance.Text.ForeColor = System.Drawing.Color.White;
			this.richEditControl1.Appearance.Text.Options.UseFont = true;
			this.richEditControl1.Appearance.Text.Options.UseForeColor = true;
			this.richEditControl1.Appearance.Text.Options.UseTextOptions = true;
			this.richEditControl1.Appearance.Text.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.richEditControl1.AutoSizeMode = DevExpress.XtraRichEdit.AutoSizeMode.Vertical;
			this.richEditControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.richEditControl1.EnableToolTips = false;
			this.richEditControl1.Location = new System.Drawing.Point(0, 57);
			this.richEditControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
			this.richEditControl1.Name = "richEditControl1";
			this.richEditControl1.Options.Comments.ShowAllAuthors = true;
			this.richEditControl1.Options.Comments.Visibility = DevExpress.XtraRichEdit.Options.RichEditCommentVisibility.Auto;
			this.richEditControl1.Options.CopyPaste.MaintainDocumentSectionSettings = false;
			this.richEditControl1.Options.Fields.UseCurrentCultureDateTimeFormat = false;
			this.richEditControl1.Options.MailMerge.KeepLastParagraph = false;
			this.richEditControl1.Options.VerticalScrollbar.Visibility = DevExpress.XtraRichEdit.RichEditScrollbarVisibility.Hidden;
			this.richEditControl1.ReadOnly = true;
			this.richEditControl1.Size = new System.Drawing.Size(430, 25);
			this.richEditControl1.TabIndex = 4;
			this.richEditControl1.Text = "richEditControl1";
			// 
			// ToolTipForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(440, 144);
			this.Controls.Add(this.richEditControl1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.picBoxItem);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ToolTipForm";
			this.Text = "ToolTipForm";
			((System.ComponentModel.ISupportInitialize)(this.picBoxItem)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picBoxItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private DevExpress.XtraRichEdit.RichEditControl richEditControl1;

	}
}