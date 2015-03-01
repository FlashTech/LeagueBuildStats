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
using System.IO;
using System.ComponentModel.Design;
using LeagueBuildStats.Classes.Items;
using LeagueBuildStats.Classes.Champions;
using RiotSharp.StaticDataEndpoint;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using LeagueBuildStats.Classes.DragUtils;

namespace LeagueBuildStats.Forms
{
	public partial class ItemsTab : UserControl
	{
		public GetItemsFromServer getItemsFromServer = new GetItemsFromServer();
		public CreateItemSortMenu createItemSortMenu;
		public int clickedItemID = 0;

		private Form1 form1;

		public Drag dragger = new Drag();

		public ItemsTab(Form1 form)
		{
			this.form1 = form;
			InitializeComponent();
			richEditCtrDetails.ActiveView.BackColor = xtraScrollableControlItems.BackColor;
			richEditCtrDetails.PopupMenuShowing += richEditCtrDetails_PopupMenuShowing;
			richEditCtrDetails.Enabled = false; //Hides cursor and highlighting ability
			lblDetailName.Text = "";
			lblDetailPrice.Text = "";
			createItemSortMenu = new CreateItemSortMenu(xtraScrollableControlItemSort, flowLayoutPanelItems2);
		}

		/// <summary>
		/// Hides the right-click context menu for the richEditCtrDetails (RichEditControl)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void richEditCtrDetails_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			e.Menu.Items.Clear();
		}

		public bool CollectItemData(string inputVersion = null)
		{
			bool success;
			if (inputVersion == null)
			{
				success = getItemsFromServer.DownloadListOfItems();
			}
			else
			{
				success = getItemsFromServer.DownloadListOfItems(inputVersion);
			}
				
			if (success) 
			{
				//TODO: check if images actually wer downloaded
				success = getItemsFromServer.GetItemImages(); 
			}
			return success;
		}

		private bool StoreRiotItemData()
		{

			bool success = false;
			try
			{
				string file = string.Format(@"{0}\Data\Items\getItemsFromServer.{1}.bin", PublicStaticVariables.thisAppDataDir, getItemsFromServer.version);
				string dir = string.Format(@"{0}\Data\Items", PublicStaticVariables.thisAppDataDir);

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				using (FileStream fs = File.Open(file, FileMode.Create))
				using (StreamWriter sw = new StreamWriter(fs))
				using (JsonWriter jw = new JsonTextWriter(sw))
				{
					jw.Formatting = Formatting.Indented;

					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(jw, getItemsFromServer);
				}
				success = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				success = false;
			}
			return success;
		}

		public bool LoadRiotItemData(string version = null)
		{
			bool success = false;
			try
			{
				string file;
				if (version == null)
				{
					file = string.Format(@"{0}\Data\Items\getItemsFromServer.{1}.bin", PublicStaticVariables.thisAppDataDir, form1.getAllVersionAvailable.realm.V);
				}
				else
				{
					file = string.Format(@"{0}\Data\Items\getItemsFromServer.{1}.bin", PublicStaticVariables.thisAppDataDir, version);
				}

				string dir = string.Format(@"{0}\Data\Items", PublicStaticVariables.thisAppDataDir);

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				StreamReader re = new StreamReader(file);
				try
				{
					JsonTextReader reader = new JsonTextReader(re);

					JsonSerializer se = new JsonSerializer();
					object parsedData = se.Deserialize(reader);

					getItemsFromServer = JsonConvert.DeserializeObject<GetItemsFromServer>(parsedData.ToString());

					success = true;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					success = false;
				}
				finally
				{
					re.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				success = false;
			}
			return success;
		}

		public bool UpdateItemControl()
		{
			bool success = false;
			flowLayoutPanelItems2.SuspendLayout();
			if (flowLayoutPanelItems2.Controls.Count > 0)
			{
				List<Control> ctrls = flowLayoutPanelItems2.Controls.Cast<Control>().ToList();
				flowLayoutPanelItems2.Controls.Clear();
				foreach (Control c in ctrls)
					c.Dispose();
			}
			try
			{
				foreach (CreateItemDiv item in getItemsFromServer.itemsPrepared)
				{
					string version = getItemsFromServer.itemsPrepared[0].thisVersion;
					string file = string.Format(@"{0}\Data\Items\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, version, item.aItem.Image.Sprite);
					string dir = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, version);

					if (!Directory.Exists(dir)) 
					{
						Directory.CreateDirectory(dir);
					}

					Image imageItem = Image.FromFile(file);

					//Creating this control is the slowest part of this code
					ItemControl itemControl = new ItemControl(form1, this);

					itemControl.ItemCostLabel = item.aItem.Gold.TotalPrice.ToString();
					Image image = CommonMethods.cropImage(imageItem, new Rectangle(item.aItem.Image.X, item.aItem.Image.Y, item.aItem.Image.Width, item.aItem.Image.Height));
					itemControl.ItemImage = image;

					itemControl.Name = "ItemControl " + item.aItem.Name;
					itemControl.item = item;

					List<string> temp = new List<string>();

					if (item.aItem.Tags == null)
					{
						temp.Add("noTag");
						itemControl.Tag = temp;
					}
					else
					{
						temp = item.aItem.Tags;
					}

					ItemTags itemTags = new ItemTags();
					itemTags.ItemName = item.aItem.Name;
					itemTags.Tags = temp;

					itemControl.Tag = itemTags;

					flowLayoutPanelItems2.Controls.Add(itemControl);
				}

				success = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				success = false;
			}
			flowLayoutPanelItems2.ResumeLayout();
			return success;
		}




		internal void GenerateItemDetails(CreateItemDiv item)
		{
			try
			{
				//Go to Items Tab
				//XtraTabControl xtraTabControl = form1.Controls.Find("xtraTabControl", true)[0] as XtraTabControl;
				//xtraTabControl.SelectedTabPageIndex = 0;


				//clear all detail sections
				pnlItemDetailsTree.SuspendLayout();
				pnlItemDetailInto.SuspendLayout();

				if (pnlItemDetailsTree.Controls.Count > 0)
				{
					List<Control> ctrls = pnlItemDetailsTree.Controls.Cast<Control>().ToList();
					pnlItemDetailsTree.Controls.Clear();
					foreach (Control c in ctrls)
						c.Dispose();
				}

				if (pnlItemDetailInto.Controls.Count > 0)
				{
					List<Control> ctrls = pnlItemDetailInto.Controls.Cast<Control>().ToList();
					pnlItemDetailInto.Controls.Clear();
					foreach (Control c in ctrls)
						c.Dispose();
				}
				picBoxItemDetail.Image = null;
				lblDetailName.Text = "";
				lblDetailPrice.Text = "";
				using (Stream s = GenerateStreamFromString(""))
				{
					richEditCtrDetails.LoadDocument(s, DocumentFormat.Html);
				}

				//Prepare Item "Into" details
				if (item.aItem.Into != null)
				{
					List<int> itemsInto = item.aItem.Into;
					int total = itemsInto.Count();
					decimal rows = Math.Floor(((decimal)total / 8)) + 1;
					int ind = 0;
					int indStart = 0;
					for (int i = 0; i < rows; i++)
					{
						List<int> itemRowList = new List<int>();
						int indEnd;
						if (total - ind > 8)
						{
							indEnd = ind + 8;
						}
						else
						{
							indEnd = total;
						}
						for (ind = indStart; ind < indEnd; ind++)
						{
							itemRowList.Add(item.aItem.Into[ind]);
						}
						CreateRowOfItemDetailsInto(itemRowList, i + 1);
						indStart = ind;
					}
				}

				//Prepare Item tree details
				RecursiveItemChildren(item, pnlItemDetailsTree, 1, 1, true);

				pnlItemDetailsTree.ResumeLayout();
				pnlItemDetailInto.ResumeLayout();


				//Prepare Item Details section
				PrepareItemDetailSection(item);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			
		}

		private void PrepareItemDetailSection(CreateItemDiv item)
		{
			string file = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, item.thisVersion);
			string dir = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, item.thisVersion);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			Image tempImageSprite = Image.FromFile(file + @"\" + item.aItem.Image.Sprite);

			Image image = CommonMethods.cropImage(tempImageSprite, new Rectangle(item.aItem.Image.X, item.aItem.Image.Y, item.aItem.Image.Width, item.aItem.Image.Height));

			//Update the Item image
			picBoxItemDetail.Image = image;

			//Update the Title and price
			lblDetailName.Text = item.thisItemDisplayName;
			lblDetailPrice.Text = "Cost: " + item.aItem.Gold.TotalPrice;

			using (Stream s = GenerateStreamFromString(item.htmlToolTipOfItem))
			{
				richEditCtrDetails.LoadDocument(s, DocumentFormat.Html);
			}

			ParagraphProperties parProperties = richEditCtrDetails.Document.BeginUpdateParagraphs(richEditCtrDetails.Document.Selection);
			parProperties.LineSpacingType = ParagraphLineSpacing.Multiple;
			parProperties.LineSpacingMultiplier = 0.7f;
			richEditCtrDetails.Document.EndUpdateParagraphs(parProperties);

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


		private void CreateRowOfItemDetailsInto(List<int> itemRowList, int row)
		{
			try
			{
				int center = (pnlItemDetailInto.Width / 2);
				int yPos = (row - 1) * 70;
				int xPos = center - (itemRowList.Count() * (50 / 2));
				for (int i = 0; i < itemRowList.Count(); i++)
				{
					CreateItemDiv item = new CreateItemDiv();

					item = getItemsFromServer.itemsPrepared.FirstOrDefault(o => o.thisID == itemRowList[i]);

					if (item.thisID != 0)
					{
						ItemControl itemControl = new ItemControl(form1, this);
						itemControl.ItemCostLabel = item.aItem.Gold.TotalPrice.ToString();
						string file = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, item.thisVersion);
						string dir = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, item.thisVersion);

						if (!Directory.Exists(dir))
						{
							Directory.CreateDirectory(dir);
						}

						Image temp = Image.FromFile(string.Format(@"{0}\{1}", file, item.aItem.Image.Sprite));
						itemControl.ItemImage = CommonMethods.cropImage(temp, new Rectangle(item.aItem.Image.X, item.aItem.Image.Y, item.aItem.Image.Width, item.aItem.Image.Height));

						itemControl.Name = "ItemControlInto " + item.aItem.Name;

						itemControl.item = item;

						itemControl.Location = new Point(xPos, yPos);
						itemControl.Click += itemControl_Click;

						pnlItemDetailInto.Controls.Add(itemControl);
						xPos += 50;
					}
					else
					{
						XtraMessageBox.Show(string.Format(@"This is awkward.. So, there was a reference to an item ID that does NOT exist.
Either Rito forgot to remove this reference when they deleted an item 
or Rito for some reason has an incorrect item ID listed
or Rito is planning to add a new item but didnt finish the job (Im hoping for this one)
or something went horribly wrong.
Please continue : )"), "League Build Stats - Notice");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		void itemControl_Click(object sender, EventArgs e)
		{
			ItemControl itemControl = sender as ItemControl;
			GenerateItemDetails(itemControl.item);
		}

		private void RecursiveItemChildren(CreateItemDiv item, Panel pnl, int LayerCount, int LayerPosition, bool start)
		{
			Panel pnlNew = new Panel();
			pnlNew.Width = pnl.Width / LayerCount;
			pnlNew.Height = 20;
			pnlNew.AutoSize = true;
			pnlNew.BorderStyle = System.Windows.Forms.BorderStyle.None;
			pnlNew.Name = "TreeItem" + item.aItem.Name;
			if (LayerPosition > 1)
			{
				pnlNew.Location = new Point((pnl.Width / LayerCount) * (LayerPosition-1), 74);
			}
			else if (start)
			{
				pnlNew.Location = new Point(0, 0);
			}
			else
			{
				pnlNew.Location = new Point(0, 74);
			}

			int centerPos = ((pnlNew.Width - 50) / (2)) ;
			int yPosition = 0;

			ItemControl itemControl = new ItemControl(form1, this);
			itemControl.ItemCostLabel = item.aItem.Gold.TotalPrice.ToString();
			itemControl.Margin = new System.Windows.Forms.Padding(0);

			string file = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, item.thisVersion);
			string dir = string.Format(@"{0}\Data\Items\Images\{1}", PublicStaticVariables.thisAppDataDir, item.thisVersion);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			Image temp = Image.FromFile(string.Format(@"{0}\{1}", file, item.aItem.Image.Sprite));
			itemControl.ItemImage = CommonMethods.cropImage(temp, new Rectangle(item.aItem.Image.X, item.aItem.Image.Y, item.aItem.Image.Width, item.aItem.Image.Height));

			itemControl.Name = "ItemControlTree " + item.aItem.Name;

			itemControl.item = item;

			itemControl.Location = new Point(centerPos, yPosition);
			itemControl.Click += itemControl_Click;

			pnlNew.Controls.Add(itemControl);
			pnl.Controls.Add(pnlNew);


			int position = 0;
			if (item.aItem.From != null)
			{
				//Create Connecting Lines to children
				Color color = Color.DimGray;

				Panel pnlLine1 = new Panel();
				pnlLine1.BackColor = color;
				pnlLine1.Margin = new Padding(0, 0, 0, 0);
				int line1x1 = itemControl.Location.X + 50/2;
				int line1y1 = itemControl.Location.Y + 63;
				pnlLine1.Location = new Point(line1x1, line1y1);
				int line1x2 = 1;
				int line1y2 = 5;
				pnlLine1.Size = new Size(line1x2, line1y2);
				pnlNew.Controls.Add(pnlLine1);

				Panel pnlLine2 = new Panel();
				pnlLine2.BackColor = color;
				pnlLine2.Margin = new Padding(0, 0, 0, 0);
				int icount = item.aItem.From.Count();
				int line2x1 = pnlNew.Width / (icount * 2);
				int line2y1 = line1y1 + line1y2;
				pnlLine2.Location = new Point(line2x1, line2y1);
				int line2x2 = (pnlNew.Width / (icount * 2)) * (icount*2 - 2);
				int line2y2 = 1;
				pnlLine2.Size = new Size(line2x2, line2y2);
				if (icount > 1) 
				{ 
					pnlNew.Controls.Add(pnlLine2); 
				}

				for (int i = 0; i < icount; i++)
				{
					Panel pnlLine3 = new Panel();
					pnlLine3.BackColor = color;
					pnlLine3.Margin = new Padding(0, 0, 0, 0);
					int line3x1 = pnlNew.Width / (icount * 2) * (i*2 + 1);
					int line3y1 = line1y1 + line1y2;
					pnlLine3.Location = new Point(line3x1, line3y1);
					int line3x2 = 1;
					int line3y2 = 6;
					pnlLine3.Size = new Size(line3x2, line3y2);

					pnlNew.Controls.Add(pnlLine3);
				}

					
				
				//Create children tree
				foreach (string sItem in item.aItem.From)
				{
					position += 1;

					CreateItemDiv cItem = getItemsFromServer.itemsPrepared.FirstOrDefault(o => o.thisID.ToString() == sItem);
					RecursiveItemChildren(cItem, pnlNew, item.aItem.From.Count, position, false);
					pnl.AutoSize = true;
					pnlNew.AutoSize = true;
				}
			}
			pnl.AutoSize = true;
			pnlNew.AutoSize = true;
		}
	}
}
