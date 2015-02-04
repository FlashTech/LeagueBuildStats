namespace LeagueBuildStats.UserControls
{
	partial class ChampionControl
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
			this.picBoxChampion = new System.Windows.Forms.PictureBox();
			this.lblChamp = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picBoxChampion)).BeginInit();
			this.SuspendLayout();
			// 
			// picBoxItem
			// 
			this.picBoxChampion.BackColor = System.Drawing.Color.Transparent;
			this.picBoxChampion.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picBoxChampion.Location = new System.Drawing.Point(0, 0);
			this.picBoxChampion.Name = "picBoxItem";
			this.picBoxChampion.Size = new System.Drawing.Size(48, 48);
			this.picBoxChampion.TabIndex = 2;
			this.picBoxChampion.TabStop = false;
			// 
			// lblChamp
			// 
			this.lblChamp.BackColor = System.Drawing.Color.Transparent;
			this.lblChamp.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblChamp.ForeColor = System.Drawing.Color.White;
			this.lblChamp.Location = new System.Drawing.Point(-6, 48);
			this.lblChamp.Margin = new System.Windows.Forms.Padding(0);
			this.lblChamp.Name = "lblChamp";
			this.lblChamp.Size = new System.Drawing.Size(60, 12);
			this.lblChamp.TabIndex = 3;
			this.lblChamp.Text = "Heimerdinger";
			this.lblChamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ChampionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.picBoxChampion);
			this.Controls.Add(this.lblChamp);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ChampionControl";
			this.Size = new System.Drawing.Size(50, 63);
			((System.ComponentModel.ISupportInitialize)(this.picBoxChampion)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picBoxChampion;
		private System.Windows.Forms.Label lblChamp;

	}
}
