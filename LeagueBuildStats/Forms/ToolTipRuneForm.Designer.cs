namespace LeagueBuildStats.Forms
{
	partial class ToolTipRuneForm
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
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.lblTier = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.txtBoxDescription = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.txtBoxDescription);
			this.panelControl1.Controls.Add(this.lblName);
			this.panelControl1.Controls.Add(this.lblTier);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(315, 80);
			this.panelControl1.TabIndex = 0;
			// 
			// lblTier
			// 
			this.lblTier.AutoSize = true;
			this.lblTier.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTier.Location = new System.Drawing.Point(6, 4);
			this.lblTier.Name = "lblTier";
			this.lblTier.Size = new System.Drawing.Size(38, 13);
			this.lblTier.TabIndex = 0;
			this.lblTier.Text = "Tier: 3";
			// 
			// lblName
			// 
			this.lblName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.ForeColor = System.Drawing.Color.DarkViolet;
			this.lblName.Location = new System.Drawing.Point(12, 24);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(291, 19);
			this.lblName.TabIndex = 1;
			this.lblName.Text = "Greater Mark of Attack Damage";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtBoxDescription
			// 
			this.txtBoxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
			this.txtBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtBoxDescription.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBoxDescription.ForeColor = System.Drawing.Color.White;
			this.txtBoxDescription.Location = new System.Drawing.Point(0, 41);
			this.txtBoxDescription.Multiline = true;
			this.txtBoxDescription.Name = "txtBoxDescription";
			this.txtBoxDescription.Size = new System.Drawing.Size(310, 30);
			this.txtBoxDescription.TabIndex = 2;
			this.txtBoxDescription.Text = "This is a long description to test if it word wraps something something something" +
    "";
			this.txtBoxDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// ToolTipRuneForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
			this.ClientSize = new System.Drawing.Size(315, 80);
			this.Controls.Add(this.panelControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ToolTipRuneForm";
			this.Text = "ToolTipRuneForm";
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.panelControl1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblTier;
		private System.Windows.Forms.TextBox txtBoxDescription;
	}
}