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
			this.picBoxImage = new System.Windows.Forms.PictureBox();
			this.picBoxBorder = new System.Windows.Forms.PictureBox();
			this.picBoxArrow = new System.Windows.Forms.PictureBox();
			this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
			this.pnlNumBorder = new System.Windows.Forms.Panel();
			this.lblBoxCount = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picBoxImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxBorder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxArrow)).BeginInit();
			this.pnlNumBorder.SuspendLayout();
			this.SuspendLayout();
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
			// pnlNumBorder
			// 
			this.pnlNumBorder.BackColor = System.Drawing.Color.Black;
			this.pnlNumBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlNumBorder.Controls.Add(this.lblBoxCount);
			this.pnlNumBorder.Location = new System.Drawing.Point(17, 57);
			this.pnlNumBorder.Name = "pnlNumBorder";
			this.pnlNumBorder.Size = new System.Drawing.Size(28, 18);
			this.pnlNumBorder.TabIndex = 4;
			// 
			// lblBoxCount
			// 
			this.lblBoxCount.AutoSize = true;
			this.lblBoxCount.BackColor = System.Drawing.Color.Black;
			this.lblBoxCount.ForeColor = System.Drawing.Color.Gold;
			this.lblBoxCount.Location = new System.Drawing.Point(1, 1);
			this.lblBoxCount.Name = "lblBoxCount";
			this.lblBoxCount.Size = new System.Drawing.Size(24, 13);
			this.lblBoxCount.TabIndex = 0;
			this.lblBoxCount.Text = "1/3";
			// 
			// MasteryControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.pnlNumBorder);
			this.Controls.Add(this.picBoxImage);
			this.Controls.Add(this.picBoxBorder);
			this.Controls.Add(this.picBoxArrow);
			this.Name = "MasteryControl";
			this.Size = new System.Drawing.Size(52, 75);
			((System.ComponentModel.ISupportInitialize)(this.picBoxImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxBorder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBoxArrow)).EndInit();
			this.pnlNumBorder.ResumeLayout(false);
			this.pnlNumBorder.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picBoxImage;
		private System.Windows.Forms.PictureBox picBoxBorder;
		private System.Windows.Forms.PictureBox picBoxArrow;
		private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
		private System.Windows.Forms.Panel pnlNumBorder;
		private System.Windows.Forms.Label lblBoxCount;
	}
}
