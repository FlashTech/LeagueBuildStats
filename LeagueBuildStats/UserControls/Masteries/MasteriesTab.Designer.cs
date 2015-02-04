namespace LeagueBuildStats.UserControls
{
	partial class MasteriesTab
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
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.dropDownBtnReturnPoint = new DevExpress.XtraEditors.DropDownButton();
			this.lblPointsAvail = new System.Windows.Forms.Label();
			this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
			this.lblUtilityCount = new System.Windows.Forms.Label();
			this.lblDefenceCount = new System.Windows.Forms.Label();
			this.lblOffenceCount = new System.Windows.Forms.Label();
			this.picBoxMasteryMain = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.xtraScrollableControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBoxMasteryMain)).BeginInit();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.dropDownBtnReturnPoint);
			this.panelControl1.Controls.Add(this.lblPointsAvail);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(127, 671);
			this.panelControl1.TabIndex = 0;
			// 
			// dropDownBtnReturnPoint
			// 
			this.dropDownBtnReturnPoint.DropDownArrowStyle = DevExpress.XtraEditors.DropDownArrowStyle.Hide;
			this.dropDownBtnReturnPoint.Location = new System.Drawing.Point(8, 36);
			this.dropDownBtnReturnPoint.Name = "dropDownBtnReturnPoint";
			this.dropDownBtnReturnPoint.Size = new System.Drawing.Size(113, 23);
			this.dropDownBtnReturnPoint.TabIndex = 2;
			this.dropDownBtnReturnPoint.Text = "Return Points";
			// 
			// lblPointsAvail
			// 
			this.lblPointsAvail.AutoSize = true;
			this.lblPointsAvail.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPointsAvail.Location = new System.Drawing.Point(5, 15);
			this.lblPointsAvail.Name = "lblPointsAvail";
			this.lblPointsAvail.Size = new System.Drawing.Size(120, 16);
			this.lblPointsAvail.TabIndex = 0;
			this.lblPointsAvail.Text = "Points Available: 30";
			// 
			// xtraScrollableControl1
			// 
			this.xtraScrollableControl1.Controls.Add(this.lblUtilityCount);
			this.xtraScrollableControl1.Controls.Add(this.lblDefenceCount);
			this.xtraScrollableControl1.Controls.Add(this.lblOffenceCount);
			this.xtraScrollableControl1.Controls.Add(this.picBoxMasteryMain);
			this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xtraScrollableControl1.Location = new System.Drawing.Point(127, 0);
			this.xtraScrollableControl1.Name = "xtraScrollableControl1";
			this.xtraScrollableControl1.Size = new System.Drawing.Size(844, 671);
			this.xtraScrollableControl1.TabIndex = 1;
			// 
			// lblUtilityCount
			// 
			this.lblUtilityCount.AutoSize = true;
			this.lblUtilityCount.BackColor = System.Drawing.Color.Transparent;
			this.lblUtilityCount.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUtilityCount.Location = new System.Drawing.Point(573, 444);
			this.lblUtilityCount.Name = "lblUtilityCount";
			this.lblUtilityCount.Size = new System.Drawing.Size(113, 25);
			this.lblUtilityCount.TabIndex = 3;
			this.lblUtilityCount.Text = "UTILITY: 0";
			// 
			// lblDefenceCount
			// 
			this.lblDefenceCount.AutoSize = true;
			this.lblDefenceCount.BackColor = System.Drawing.Color.Transparent;
			this.lblDefenceCount.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDefenceCount.Location = new System.Drawing.Point(299, 444);
			this.lblDefenceCount.Name = "lblDefenceCount";
			this.lblDefenceCount.Size = new System.Drawing.Size(125, 25);
			this.lblDefenceCount.TabIndex = 2;
			this.lblDefenceCount.Text = "DEFENCE: 0";
			// 
			// lblOffenceCount
			// 
			this.lblOffenceCount.AutoSize = true;
			this.lblOffenceCount.BackColor = System.Drawing.Color.Transparent;
			this.lblOffenceCount.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOffenceCount.Location = new System.Drawing.Point(26, 444);
			this.lblOffenceCount.Name = "lblOffenceCount";
			this.lblOffenceCount.Size = new System.Drawing.Size(125, 25);
			this.lblOffenceCount.TabIndex = 1;
			this.lblOffenceCount.Text = "OFFENCE: 0";
			// 
			// picBoxMasteryMain
			// 
			this.picBoxMasteryMain.Location = new System.Drawing.Point(0, 0);
			this.picBoxMasteryMain.Name = "picBoxMasteryMain";
			this.picBoxMasteryMain.Size = new System.Drawing.Size(827, 479);
			this.picBoxMasteryMain.TabIndex = 0;
			this.picBoxMasteryMain.TabStop = false;
			// 
			// MasteriesTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.xtraScrollableControl1);
			this.Controls.Add(this.panelControl1);
			this.Name = "MasteriesTab";
			this.Size = new System.Drawing.Size(971, 671);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.panelControl1.PerformLayout();
			this.xtraScrollableControl1.ResumeLayout(false);
			this.xtraScrollableControl1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBoxMasteryMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picBoxMasteryMain;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private System.Windows.Forms.Label lblPointsAvail;
		private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
		private System.Windows.Forms.Label lblUtilityCount;
		private System.Windows.Forms.Label lblDefenceCount;
		private System.Windows.Forms.Label lblOffenceCount;
		private DevExpress.XtraEditors.DropDownButton dropDownBtnReturnPoint;

	}
}
