namespace LeagueBuildStats
{
	partial class LeagueBuildStatsForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeagueBuildStatsForm));
			this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
			this.Items = new DevExpress.XtraTab.XtraTabPage();
			this.Champions = new DevExpress.XtraTab.XtraTabPage();
			this.Runes = new DevExpress.XtraTab.XtraTabPage();
			this.Masteries = new DevExpress.XtraTab.XtraTabPage();
			this.Stats = new DevExpress.XtraTab.XtraTabPage();
			this.picBoxCursor2 = new System.Windows.Forms.PictureBox();
			this.panelCtlMainTopBar = new DevExpress.XtraEditors.PanelControl();
			this.popupMenuVersions = new DevExpress.XtraBars.PopupMenu(this.components);
			this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
			this.bar1 = new DevExpress.XtraBars.Bar();
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
			this.xtraTabControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBoxCursor2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelCtlMainTopBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.popupMenuVersions)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// xtraTabControl
			// 
			this.xtraTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.xtraTabControl.Location = new System.Drawing.Point(0, 108);
			this.xtraTabControl.Name = "xtraTabControl";
			this.xtraTabControl.SelectedTabPage = this.Items;
			this.xtraTabControl.Size = new System.Drawing.Size(1134, 543);
			this.xtraTabControl.TabIndex = 0;
			this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.Items,
            this.Champions,
            this.Runes,
            this.Masteries,
            this.Stats});
			// 
			// Items
			// 
			this.Items.Name = "Items";
			this.Items.Size = new System.Drawing.Size(1132, 516);
			this.Items.Text = "Items";
			// 
			// Champions
			// 
			this.Champions.Name = "Champions";
			this.Champions.Size = new System.Drawing.Size(1132, 516);
			this.Champions.Text = "Champions";
			// 
			// Runes
			// 
			this.Runes.Name = "Runes";
			this.Runes.Size = new System.Drawing.Size(1132, 516);
			this.Runes.Text = "Runes";
			// 
			// Masteries
			// 
			this.Masteries.Name = "Masteries";
			this.Masteries.Size = new System.Drawing.Size(1132, 516);
			this.Masteries.Text = "Masteries";
			// 
			// Stats
			// 
			this.Stats.Name = "Stats";
			this.Stats.Size = new System.Drawing.Size(1132, 516);
			this.Stats.Text = "Stats";
			// 
			// picBoxCursor2
			// 
			this.picBoxCursor2.Image = global::LeagueBuildStats.Properties.Resources.do_not_delete_this;
			this.picBoxCursor2.Location = new System.Drawing.Point(-32, -32);
			this.picBoxCursor2.Name = "picBoxCursor2";
			this.picBoxCursor2.Size = new System.Drawing.Size(48, 48);
			this.picBoxCursor2.TabIndex = 5;
			this.picBoxCursor2.TabStop = false;
			// 
			// panelCtlMainTopBar
			// 
			this.panelCtlMainTopBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelCtlMainTopBar.Location = new System.Drawing.Point(0, 0);
			this.panelCtlMainTopBar.Name = "panelCtlMainTopBar";
			this.panelCtlMainTopBar.Size = new System.Drawing.Size(1130, 102);
			this.panelCtlMainTopBar.TabIndex = 9;
			// 
			// popupMenuVersions
			// 
			this.popupMenuVersions.Manager = this.barManager1;
			this.popupMenuVersions.Name = "popupMenuVersions";
			// 
			// barManager1
			// 
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.MaxItemId = 3;
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(1130, 0);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 647);
			this.barDockControlBottom.Size = new System.Drawing.Size(1130, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 647);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(1130, 0);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 647);
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.Text = "layoutControlItem1";
			this.layoutControlItem1.TextSize = new System.Drawing.Size(50, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			// 
			// defaultLookAndFeel1
			// 
			this.defaultLookAndFeel1.LookAndFeel.SkinName = "Visual Studio 2013 Dark";
			this.defaultLookAndFeel1.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
			// 
			// bar1
			// 
			this.bar1.BarName = "Custom 1";
			this.bar1.DockCol = 0;
			this.bar1.DockRow = 0;
			this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar1.OptionsBar.AllowRename = true;
			this.bar1.Text = "Custom 1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
			this.ClientSize = new System.Drawing.Size(1130, 647);
			this.Controls.Add(this.picBoxCursor2);
			this.Controls.Add(this.panelCtlMainTopBar);
			this.Controls.Add(this.xtraTabControl);
			this.Controls.Add(this.barDockControlLeft);
			this.Controls.Add(this.barDockControlRight);
			this.Controls.Add(this.barDockControlBottom);
			this.Controls.Add(this.barDockControlTop);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(810, 685);
			this.Name = "Form1";
			this.Text = "League Build Stats - For League of Legends (LoL)";
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
			this.xtraTabControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picBoxCursor2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelCtlMainTopBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.popupMenuVersions)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraTab.XtraTabControl xtraTabControl;
		private DevExpress.XtraTab.XtraTabPage Items;
		private DevExpress.XtraTab.XtraTabPage Champions;
		private DevExpress.XtraTab.XtraTabPage Runes;
		private DevExpress.XtraTab.XtraTabPage Masteries;
		private DevExpress.XtraTab.XtraTabPage Stats;
		private DevExpress.XtraEditors.PanelControl panelCtlMainTopBar;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
		private DevExpress.XtraBars.PopupMenu popupMenuVersions;
		private DevExpress.XtraBars.BarManager barManager1;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.Bar bar1;
		private System.Windows.Forms.PictureBox picBoxCursor2;
	}
}

