using DevExpress.Utils.Commands;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Forms
{
	public partial class ToolTipItemForm : Form
	{
		// add this code after the class' default constructor

		private int desiredStartLocationX;
		private int desiredStartLocationY;
		private int oldX = -1;
		private int oldY = -1;
		private ItemControl itemcontrol = new ItemControl();
		private CreateItemDiv item = new CreateItemDiv();

		public ToolTipItemForm(int x, int y, ItemControl itemcontrol, CreateItemDiv item)
			   : this()
		{
			// here store the value for x & y into instance variables
			this.desiredStartLocationX = x-10;
			this.desiredStartLocationY = y-10;

			Load += new EventHandler(Form2_Load);
			this.itemcontrol = itemcontrol;
			this.item = item;
		}

		private void Form2_Load(object sender, System.EventArgs e)
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


		//protected override bool ShowWithoutActivation
		//{
		//	get { return true; }
		//}

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





		public ToolTipItemForm()
		{
			InitializeComponent();

			richEditControl1.ActiveView.BackColor = this.BackColor;
			richEditControl1.DocumentLoaded += richEditControl1_DocumentLoaded;
			richEditControl1.Enabled = false;

			picBoxItem.Click += picBoxItem_Click;


			this.Focus();
			// Do not allow form to be displayed in taskbar.
			this.ShowInTaskbar = false;
			//this.TopMost = true;
		}

		void picBoxItem_Click(object sender, EventArgs e)
		{
			itemcontrol.itemsTab.GenerateItemDetails(item);
			this.Close();
		}




		void richEditControl1_DocumentLoaded(object sender, EventArgs e)
		{
			this.Height = richEditControl1.Height + 58;
		}





		public void UpdateToolTipForm(Image image, string itemName, int itemId, string price, string htmlString, string version)
		{
			//Update the Item image
			picBoxItem.Image = image;

			//Update the Title and price
			label1.Text = itemName;
			label2.Text = "Cost: " + price;


			//Update the html description section
			//Removes invalid characters
			//string tempName = itemName;
			//tempName = tempName.Replace("\\", "");
			//tempName = tempName.Replace("/", "");
			//tempName = tempName.Replace(":", "");
			//tempName = tempName.Replace("?", "");
			//tempName = tempName.Replace("\"", "");
			//tempName = tempName.Replace("<", "");
			//tempName = tempName.Replace(">", "");
			//tempName = tempName.Replace("|", "");
			string itemFile = PublicStaticVariables.thisAppDataDir + string.Format(@"\Data\Items\ItemInfo{0}.{1}.htm", itemId, version);
			//If the file doesnt exist already then we will creat one
			if (!File.Exists(itemFile))
			{
				//TODO must create firectory if doesnt exists
				System.IO.File.WriteAllText(itemFile, htmlString);

			}

			richEditControl1.LoadDocument(itemFile);


			ParagraphProperties parProperties = richEditControl1.Document.BeginUpdateParagraphs(richEditControl1.Document.Selection);
			// Specifies triple spacing
			parProperties.LineSpacingType = ParagraphLineSpacing.Multiple;
			parProperties.LineSpacingMultiplier = 0.7f;
			richEditControl1.Document.EndUpdateParagraphs(parProperties);

		}
	}


}
