namespace LeagueBuildStats.UserControls.Runes
{
	partial class RuneStatistics
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
			this.xtraScrollCtrlStatistics = new DevExpress.XtraEditors.XtraScrollableControl();
			this.SuspendLayout();
			// 
			// xtraScrollCtrlStatistics
			// 
			this.xtraScrollCtrlStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xtraScrollCtrlStatistics.Location = new System.Drawing.Point(0, 0);
			this.xtraScrollCtrlStatistics.Name = "xtraScrollCtrlStatistics";
			this.xtraScrollCtrlStatistics.Size = new System.Drawing.Size(239, 563);
			this.xtraScrollCtrlStatistics.TabIndex = 0;
			// 
			// RuneStatistics
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.xtraScrollCtrlStatistics);
			this.Name = "RuneStatistics";
			this.Size = new System.Drawing.Size(239, 563);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.XtraScrollableControl xtraScrollCtrlStatistics;
	}
}
