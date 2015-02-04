using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Forms
{
	public partial class ToolTipRuneForm : Form
	{
		private int desiredStartLocationX;
		private int desiredStartLocationY;
		private int oldX = -1;
		private int oldY = -1;

		public ToolTipRuneForm(int x, int y)
			   : this()
		{
			// here store the value for x & y into instance variables
			this.desiredStartLocationX = x+10;
			this.desiredStartLocationY = y+10;

			Load += new EventHandler(_Load);
		}

		private void _Load(object sender, EventArgs e)
		{
			this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);

			Timer t1 = new Timer();
			t1.Interval = 50;
			t1.Tick += new EventHandler(timer1_Tick);
			t1.Enabled = true;

			MouseWheel += ToolTipForm_MouseWheel;
			//this.Focus();
		}
		void ToolTipForm_MouseWheel(object sender, MouseEventArgs e)
		{
			this.Close();
		}

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		/// <summary>
		/// Check the cursor position and closes this ToolTipForm if the cursor was moved
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			int curX = Cursor.Position.X;
			int curY = Cursor.Position.Y;

			if (oldX != -1)
			{
				if (curX != oldX || curY != oldY)
				{
					this.Close();
				}
			}
			oldX = curX;
			oldY = curY;

		}

		public ToolTipRuneForm()
		{
			InitializeComponent();
			this.ShowInTaskbar = false;
		}

		public void updateRuneTip(string tier, string name, string description, string color)
		{
			lblTier.Text = tier;
			lblName.Text = name;
			txtBoxDescription.Text = description;
			switch (color)
			{
				case "red":
					lblName.ForeColor = Color.Red;
					return;
				case "blue":
					lblName.ForeColor = Color.LightBlue;
					return;
				case "yellow":
					lblName.ForeColor = Color.Yellow;
					return;
				case "black":
					lblName.ForeColor = Color.DarkViolet;
					return;
				default:
					lblName.ForeColor = Color.White;
					return;
			}
		}
	}
}
