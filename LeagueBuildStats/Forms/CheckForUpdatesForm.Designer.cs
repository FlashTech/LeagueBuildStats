namespace LeagueBuildStats.Forms
{
	partial class CheckForUpdatesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckForUpdatesForm));
			this.btnYes = new System.Windows.Forms.Button();
			this.lblText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnYes
			// 
			this.btnYes.Location = new System.Drawing.Point(72, 69);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(85, 24);
			this.btnYes.TabIndex = 0;
			this.btnYes.Text = "Yes";
			this.btnYes.UseVisualStyleBackColor = true;
			// 
			// lblText
			// 
			this.lblText.AutoSize = true;
			this.lblText.Location = new System.Drawing.Point(12, 28);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(136, 13);
			this.lblText.TabIndex = 1;
			this.lblText.Text = "Continue without updating?";
			// 
			// ContinueWithoutUpdating
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(169, 105);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.btnYes);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ContinueWithoutUpdating";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "League Build Stats";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Label lblText;
	}
}