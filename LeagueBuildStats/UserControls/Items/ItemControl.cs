using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeagueBuildStats.Classes;
using LeagueBuildStats.Forms;
using System.IO;
using System.Threading;
using LeagueBuildStats.Classes.DragUtils;
using Infragistics.Win.UltraWinToolTip;
using RiotSharp.StaticDataEndpoint;
using DevExpress.XtraRichEdit;

namespace LeagueBuildStats
{
	public partial class ItemControl : UserControl
	{
		public CreateItemDiv item;
		Form1 frm1;
		public ItemsTab itemsTab;

		public string ItemCostLabel
		{
			get { return lblItem.Text; }
			set { lblItem.Text = value; }
		}

		public Image ItemImage
		{
			set { picBoxItem.Image = value; }
		}


		public ItemControl(Form1 frm1, ItemsTab itemsTab)
			   : this()
		{
			this.frm1 = frm1;
			this.itemsTab = itemsTab;
		}


		public ItemControl()
		{
			InitializeComponent();

			picBoxItem.MouseDown += picBoxItem_MouseDown;
			picBoxItem.MouseMove += picBoxItem_MouseMove;
			picBoxItem.MouseUp += picBoxItem_MouseUp;
			picBoxItem.MouseDoubleClick += picBoxItem_MouseDoubleClick;
			picBoxItem.MouseClick += picBoxItem_MouseClick;

			picBoxItem.GiveFeedback+=picBoxItem_GiveFeedback;

		}


		private void picBoxItem_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			//Drag.UpdateCursor(sender, e);
			e.UseDefaultCursors = true;
		}

		void picBoxItem_MouseClick(object sender, MouseEventArgs e)
		{
			if (itemsTab.clickedItemID != item.thisID)
			{
				itemsTab.GenerateItemDetails(item);
				itemsTab.clickedItemID = item.thisID;
			}
		}

		void picBoxItem_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//Generate PicBox
			CreateItemDiv itemPreped = item;


			if (item.thisItemDisplayName.Contains("Elixir"))
			{
				Control cTemp = frm1.mainTopBar.Controls.Find("pnlElixir", true)[0];

				if (cTemp.Controls.Count > 0)
				{
					List<Control> ctrls = cTemp.Controls.Cast<Control>().ToList();
					cTemp.Controls.Clear();
					foreach (Control c in ctrls)
						c.Dispose();
				}

				PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
				string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
				cTemp.Tag = itemPreped;
				cTemp.Controls.Add(itemPicBox);
				//tooltip
				frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItemElixir);
				frm1.mainTopBar.tipInfoItemElixir.ToolTipTextFormatted = sTooltip;
					
			}
			else
			{
				//Todo: frm1.mainTopBar.Controls.Find("pnlItem1", true)[0]; should be changed to a public object to more easily reference faster
				Control cTemp = frm1.mainTopBar.Controls.Find("pnlItem1", true)[0];
				if (cTemp.Tag == null)
				{
					PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
					string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
					cTemp.Tag = itemPreped;
					cTemp.Controls.Add(itemPicBox);
					//tooltip
					frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItem1);
					frm1.mainTopBar.tipInfoItem1.ToolTipTextFormatted = sTooltip;
					return;
				}
				cTemp = frm1.mainTopBar.Controls.Find("pnlItem2", true)[0];
				if (cTemp.Tag == null)
				{
					PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
					string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
					cTemp.Tag = itemPreped;
					cTemp.Controls.Add(itemPicBox);
					//tooltip
					frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItem2);
					frm1.mainTopBar.tipInfoItem2.ToolTipTextFormatted = sTooltip;
					return;
				}
				cTemp = frm1.mainTopBar.Controls.Find("pnlItem3", true)[0];
				if (cTemp.Tag == null)
				{
					PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
					string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
					cTemp.Tag = itemPreped;
					cTemp.Controls.Add(itemPicBox);
					//tooltip
					frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItem3);
					frm1.mainTopBar.tipInfoItem3.ToolTipTextFormatted = sTooltip;
					return;
				}
				cTemp = frm1.mainTopBar.Controls.Find("pnlItem4", true)[0];
				if (cTemp.Tag == null)
				{
					PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
					string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
					cTemp.Tag = itemPreped;
					cTemp.Controls.Add(itemPicBox);
					//tooltip
					frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItem4);
					frm1.mainTopBar.tipInfoItem4.ToolTipTextFormatted = sTooltip;
					return;
				}
				cTemp = frm1.mainTopBar.Controls.Find("pnlItem5", true)[0];
				if (cTemp.Tag == null)
				{
					PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
					string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
					cTemp.Tag = itemPreped;
					cTemp.Controls.Add(itemPicBox);
					//tooltip
					frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItem5);
					frm1.mainTopBar.tipInfoItem5.ToolTipTextFormatted = sTooltip;
					return;
				}
				cTemp = frm1.mainTopBar.Controls.Find("pnlItem6", true)[0];
				if (cTemp.Tag == null)
				{
					PictureBox itemPicBox = GenerateItemPicBox(itemPreped);
					string sTooltip = frm1.mainTopBar.CreateItemPicBoxTooltip(itemPreped);
					cTemp.Tag = itemPreped;
					cTemp.Controls.Add(itemPicBox);
					//tooltip
					frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, frm1.mainTopBar.tipInfoItem6);
					frm1.mainTopBar.tipInfoItem6.ToolTipTextFormatted = sTooltip;
					return;
				}
			}
		}

		private PictureBox GenerateItemPicBox(CreateItemDiv itemPreped)
		{
			PictureBox itemPicBox = new PictureBox();
			itemPicBox.Size = new Size(48, 48);
			itemPicBox.Location = new Point(0, 0);
			string file = string.Format(@"{0}\Data\Items\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, itemPreped.thisVersion, itemPreped.aItem.Value.Image.Sprite);
			Image imageItem = Image.FromFile(file);
			Image image = CommonMethods.cropImage(imageItem, new Rectangle(itemPreped.aItem.Value.Image.X, itemPreped.aItem.Value.Image.Y, itemPreped.aItem.Value.Image.Width, itemPreped.aItem.Value.Image.Height));
			itemPicBox.Image = image;
			itemPicBox.MouseClick += frm1.mainTopBar.itemPicBox_MouseClick;
			itemPicBox.Cursor = Cursors.Hand;
			itemPicBox.Tag = itemPreped;

			return itemPicBox;
		}



		public Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		

		private bool pressed = false;
		private Point newMouseDelta = Point.Empty;
		private Point oldMouseDelta = Point.Empty;
		private float diffX;
		private float diffY;

		void picBoxItem_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				pressed = true;
				oldMouseDelta = e.Location;
			}
		}

		void picBoxItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (pressed)
			{
				newMouseDelta = e.Location;

				diffX = (oldMouseDelta.X - newMouseDelta.X);
				diffY = (newMouseDelta.Y - oldMouseDelta.Y);
				
				if (Math.Abs(diffY) > 10 || Math.Abs(diffX) > 10)
				{
					oldMouseDelta = Point.Empty;
					pressed = false;
					Control control = sender as Control;



					itemsTab.dragger.StartDragging(picBoxItem);
					if (item.thisItemDisplayName.Contains("Elixir"))
					{
						picBoxItem.DoDragDrop("ELIXIR" + item.thisID.ToString(), DragDropEffects.Copy | DragDropEffects.Move);
					}
					else
					{
						picBoxItem.DoDragDrop("ITEM" + item.thisID.ToString(), DragDropEffects.Copy | DragDropEffects.Move);
					}
					itemsTab.dragger.StopDragging();
				}
			}
		}

		void picBoxItem_MouseUp(object sender, MouseEventArgs e)
		{
			pressed = false;
		}

	}
}
