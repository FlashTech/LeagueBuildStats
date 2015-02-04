namespace LeagueBuildStats.UserControls.Masteries
{
	partial class MasteryControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.txtBoxCount = new System.Windows.Forms.TextBox();
			this.picBoxImage = new System.Windows.Forms.PictureBox();
			this.picBoxBorder = new System.Windows.Forms.PictureBox();
			this.picBoxArrow = new System.Windows.Forms.PictureBox();
			this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
			((System.ComponentModel.ISupportInitialize)(this.picBoxImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxBorder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxArrow)).BeginInit();
			this.SuspendLayout();
			// 
			// txtBoxCount
			// 
			this.txtBoxCount.BackColor = System.Drawing.Color.Black;
			this.txtBoxCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtBoxCount.ForeColor = System.Drawing.Color.Gold;
			this.txtBoxCount.Location = new System.Drawing.Point(17, 57);
			this.txtBoxCount.Multiline = true;
			this.txtBoxCount.Name = "txtBoxCount";
			this.txtBoxCount.Size = new System.Drawing.Size(28, 18);
			this.txtBoxCount.TabIndex = 1;
			this.txtBoxCount.Text = "1/3";
			this.txtBoxCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtBoxCount.WordWrap = false;
			// 
			// picBoxImage
			// 
			this.picBoxImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picBoxImage.Location = new System.Drawing.Point(0, 18);
			this.picBoxImage.Name = "picBoxImage";
			this.picBoxImage.Size = new System.Drawing.Size(52, 52);
			this.picBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picBoxImage.TabIndex = 0;
			this.picBoxImage.TabStop = false;
			// 
			// picBoxBorder
			// 
			this.picBoxBorder.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picBoxBorder.Location = new System.Drawing.Point(0, 18);
			this.picBoxBorder.Name = "picBoxBorder";
			this.picBoxBorder.Size = new System.Drawing.Size(52, 52);
			this.picBoxBorder.TabIndex = 2;
			this.picBoxBorder.TabStop = false;
			// 
			// picBoxArrow
			// 
			this.picBoxArrow.Location = new System.Drawing.Point(16, -50);
			this.picBoxArrow.Name = "picBoxArrow";
			this.picBoxArrow.Size = new System.Drawing.Size(21, 75);
			this.picBoxArrow.TabIndex = 3;
			this.picBoxArrow.TabStop = false;
			// 
			// ultraToolTipManager1
			// 
			this.ultraToolTipManager1.AutoPopDelay = 0;
			this.ultraToolTipManager1.ContainingControl = this;
			this.ultraToolTipManager1.InitialDelay = 50;
			this.ultraToolTipManager1.ToolTipTextStyle = Infragistics.Win.ToolTipTextStyle.Formatted;
			// 
			// MasteryControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.txtBoxCount);
			this.Controls.Add(this.picBoxImage);
			this.Controls.Add(this.picBoxBorder);
			this.Controls.Add(this.picBoxArrow);
			this.Name = "MasteryControl";
			this.Size = new System.Drawing.Size(52, 75);
			((System.ComponentModel.ISupportInitialize)(this.picBoxImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxBorder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxArrow)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picBoxImage;
		private System.Windows.Forms.TextBox txtBoxCount;
		private System.Windows.Forms.PictureBox picBoxBorder;
		private System.Windows.Forms.PictureBox picBoxArrow;
		private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
	}
}
