using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using LeagueBuildStats.Forms;
using DevExpress.XtraRichEdit;
using LeagueBuildStats.Classes;
using RiotSharp.StaticDataEndpoint;
using Infragistics.Win.UltraWinToolTip;
using Infragistics.Win;

namespace LeagueBuildStats.UserControls.MainTopBar
{
	public partial class MainTopBar : UserControl
	{
		private Form1 form1;

		public UltraToolTipInfo tipInfoChamp = new UltraToolTipInfo();

		public UltraToolTipInfo tipInfoItem1 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem2 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem3 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem4 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem5 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem6 = new UltraToolTipInfo();

		public List<Panel> itemPanels = new List<Panel>();


		//TODO: maybe instead of looking directly are the tag is may be better to have public variables that are updated when the tag is updated. might not be necessary
		public KeyValuePair<string, ChampionStatic> champion1 { get { return (pnlChampion.Tag != null ? (KeyValuePair<string, ChampionStatic>)pnlChampion.Tag : new KeyValuePair<string, ChampionStatic>()); } }
		public CreateItemDiv item1 { get { return (pnlItem1.Tag != null ? (CreateItemDiv)pnlItem1.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item2 { get { return (pnlItem2.Tag != null ? (CreateItemDiv)pnlItem2.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item3 { get { return (pnlItem3.Tag != null ? (CreateItemDiv)pnlItem3.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item4 { get { return (pnlItem4.Tag != null ? (CreateItemDiv)pnlItem4.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item5 { get { return (pnlItem5.Tag != null ? (CreateItemDiv)pnlItem5.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item6 { get { return (pnlItem6.Tag != null ? (CreateItemDiv)pnlItem6.Tag : new CreateItemDiv()); } }

		public MainTopBar(Form1 form)
		{
			this.form1 = form;
			InitializeComponent();

			InitializeEvents();

			itemPanels.Add(pnlItem1);
			itemPanels.Add(pnlItem2);
			itemPanels.Add(pnlItem3);
			itemPanels.Add(pnlItem4);
			itemPanels.Add(pnlItem5);
			itemPanels.Add(pnlItem6);
			


			//Tooltips prep
			ultraToolTipManager1.AutoPopDelay = 0;
			ultraToolTipManager1.InitialDelay = 50;
			ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Office2007;
			ultraToolTipManager1.Appearance.BackColor = Color.Black;
			ultraToolTipManager1.Appearance.BackColor2 = Color.Black;
			ultraToolTipManager1.Appearance.BackColorAlpha = Alpha.Transparent;

			tipInfoChamp.ToolTipTextStyle = ToolTipTextStyle.Formatted;

			tipInfoItem1.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem2.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem3.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem4.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem5.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem6.ToolTipTextStyle = ToolTipTextStyle.Formatted;

		}


		private void InitializeEvents()
		{
			simpleBtnChkUpdates.Click += simpleBtnChkUpdates_Click;

			pnlItem1.DragEnter += pnlItem_DragEnter;
			pnlItem2.DragEnter += pnlItem_DragEnter;
			pnlItem3.DragEnter += pnlItem_DragEnter;
			pnlItem4.DragEnter += pnlItem_DragEnter;
			pnlItem5.DragEnter += pnlItem_DragEnter;
			pnlItem6.DragEnter += pnlItem_DragEnter;

			pnlChampion.DragEnter += pnlChampion_DragEnter;

			pnlItem1.DragDrop += pnlItem_DragDrop;
			pnlItem2.DragDrop += pnlItem_DragDrop;
			pnlItem3.DragDrop += pnlItem_DragDrop;
			pnlItem4.DragDrop += pnlItem_DragDrop;
			pnlItem5.DragDrop += pnlItem_DragDrop;
			pnlItem6.DragDrop += pnlItem_DragDrop;

			pnlChampion.DragDrop += pnlChampion_DragDrop;

			//pnlItem1.DragOver += pnlItem_DragOver;
			//pnlItem2.DragOver += pnlItem_DragOver;
			//pnlItem3.DragOver += pnlItem_DragOver;
			//pnlItem4.DragOver += pnlItem_DragOver;
			//pnlItem5.DragOver += pnlItem_DragOver;
			//pnlItem6.DragOver += pnlItem_DragOver;
		}

		

		

		void simpleBtnChkUpdates_Click(object sender, EventArgs e)
		{
			//TDOO
			//Realm tempRealm = getAllVersionAvailable.realm;
			//List<string> tempVersions = getAllVersionAvailable.versions;
			//bool getVersionsSuccess = getAllVersionAvailable.CollectVersionData();
			//if (getVersionsSuccess)
			//{
			//	if (tempRealm == getAllVersionAvailable.realm && tempVersions == getAllVersionAvailable.versions)
			//	{
			//		XtraMessageBox.Show(string.Format(@"List of League fo Legends versions is currently up to date."), "League Build Stats - Notice");
			//	}
			//	else
			//	{
			//		XtraMessageBox.Show(string.Format(@"Updates were found but we were unable to load the list. Please restart the application to load the updated list."), "League Build Stats - Notice");
			//	}
			//}
			//else
			//{
			//	XtraMessageBox.Show(string.Format(@"Failed to check for latest versions."), "League Build Stats - Notice");
			//}

		}



		internal void UpdateMainTopBar()
		{
			try
			{
				int id = 1;
				popupMenuVersions.LinksPersistInfo.Clear();
				barManager1.Items.Clear();
				foreach (string s in form1.getAllVersionAvailable.versions)
				{
					BarButtonItem barButtonItemNew = new BarButtonItem();
					popupMenuVersions.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItemNew));

					if (form1.getAllVersionAvailable.realm.V == s)
					{
						barButtonItemNew.Caption = s + " Current LoL";
						//TODO: This may not always be so but, on startup the Current LoL is always selected by default
						dropDownButtonRiotVersion.Text = barButtonItemNew.Caption;
					}
					else
					{
						barButtonItemNew.Caption = s;
					}
					barButtonItemNew.ItemClick += barButtonItemVersionList_ItemClick;
					barButtonItemNew.Id = id;
					barButtonItemNew.Name = s;

					barManager1.Items.Add(barButtonItemNew);

					id++;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		/// <summary>
		/// Executed when a version is selected in the dropdown. Loads the version selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void barButtonItemVersionList_ItemClick(object sender, ItemClickEventArgs e)
		{
			SplashForm.ShowSplashScreen();
			bool success = false;
			string selName = "";
			string selection = "";
			try
			{
				BarManager barManager = sender as BarManager;
				selection = barManager.PressedLink.Item.Caption;
				selName = barManager.PressedLink.Item.Name;
				success = true;
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.ToString());
			}
			if (success)
			{
				success = form1.LoadRiotDataFromFile(selName);
				if (!success)
				{
					//TODO: fix this
					//SplashForm.ChangeToDownloading();
					success = form1.DataCollection(selName);
				}
				//Todo: fix this
				//SplashForm.ChangeToLoading();
			}
			if (success)
			{
				form1.UpdateFormWithData(false); //Won't recreate the version drop down list
				//Clear item details section
				form1.itemsTab.clickedItemID = 0;
				form1.itemsTab.pnlItemDetailsTree.SuspendLayout();
				form1.itemsTab.pnlItemDetailInto.SuspendLayout();
				if (form1.itemsTab.pnlItemDetailsTree.Controls.Count > 0)
				{
					List<Control> ctrls = form1.itemsTab.pnlItemDetailsTree.Controls.Cast<Control>().ToList();
					form1.itemsTab.pnlItemDetailsTree.Controls.Clear();
					foreach (Control c in ctrls)
						c.Dispose();
				}

				if (form1.itemsTab.pnlItemDetailInto.Controls.Count > 0)
				{
					List<Control> ctrls = form1.itemsTab.pnlItemDetailInto.Controls.Cast<Control>().ToList();
					form1.itemsTab.pnlItemDetailInto.Controls.Clear();
					foreach (Control c in ctrls)
						c.Dispose();
				}
				form1.itemsTab.pnlItemDetailsTree.ResumeLayout();
				form1.itemsTab.pnlItemDetailInto.ResumeLayout();

				form1.itemsTab.picBoxItemDetail.Image = null;
				form1.itemsTab.lblDetailName.Text = "";
				form1.itemsTab.lblDetailPrice.Text = "";
				using (Stream s = form1.itemsTab.GenerateStreamFromString(""))
				{
					form1.itemsTab.richEditCtrDetails.LoadDocument(s, DocumentFormat.Html);
				}
				//Clear item filter selections 
				form1.itemsTab.createItemSortMenu.selectedTags.Clear();
				foreach (Control c in form1.itemsTab.xtraScrollableControlItemSort.Controls)
				{
					try
					{
						if (c.Name.Contains("checkItemTag"))
						{
							CheckBox cCheck = c as CheckBox;
							cCheck.Checked = false;
						}
					}
					catch (Exception ex)
					{

					}
				}
				//Update dropdown
				dropDownButtonRiotVersion.Text = selection;
			}
			SplashForm.CloseForm();
		}

		private void pnlItem_DragDrop(object sender, DragEventArgs e)
		{
			try
			{

				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if (dataString.Contains("ITEM") && form1.itemsTab.getItemsFromServer.itemsPrepared != null)
				{
					string itemID = dataString.Replace("ITEM", "");
					if (form1.itemsTab.getItemsFromServer.itemsPrepared.Count > 0)
					{
						//Generate PicBox
						CreateItemDiv itemPreped = form1.itemsTab.getItemsFromServer.itemsPrepared.Find(o => o.thisID.ToString() == itemID);
						PictureBox itemPicBox = new PictureBox();
						itemPicBox.Size = new Size(48, 48);
						itemPicBox.Location = new Point(0, 0);
						string file = string.Format(@"{0}\Data\Items\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, itemPreped.thisVersion, itemPreped.aItem.Value.Image.Sprite);
						Image imageItem = Image.FromFile(file);
						Image image = CommonMethods.cropImage(imageItem, new Rectangle(itemPreped.aItem.Value.Image.X, itemPreped.aItem.Value.Image.Y, itemPreped.aItem.Value.Image.Width, itemPreped.aItem.Value.Image.Height));
						itemPicBox.Image = image;
						itemPicBox.MouseClick += itemPicBox_MouseClick;
						itemPicBox.Cursor = Cursors.Hand;
						itemPicBox.Tag = itemPreped;

						Panel thisPnl = sender as Panel;
						thisPnl.Tag = itemPreped;
						if (thisPnl.Controls.Count > 0)
						{
							List<Control> ctrls = thisPnl.Controls.Cast<Control>().ToList();
							thisPnl.Controls.Clear();
							foreach (Control c in ctrls)
								c.Dispose();
						}
						thisPnl.Controls.Add(itemPicBox);



						//Generate tooltip for item on maintopbar
						string sTooltip = string.Format(@"
						<div style='max-width:300px;'>
						<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
							<span style='font-size:12pt;'>{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> <br/>
							<span style='color:Yellow;'>Cost: {1}</span> <br/><br/>
							{2}<br/>
						",//</p></div>"
									itemPreped.thisItemDisplayName, itemPreped.aItem.Value.Gold.TotalPrice, itemPreped.DivText);

						//todo: this is a test to show the stats of the items
						try
						{
							Type myType = typeof(StatsStatic);
							System.Reflection.PropertyInfo[] properties = myType.GetProperties();

							foreach (System.Reflection.PropertyInfo property in properties)
							{
								//Double statValue = (Double)itemPreped.aItem.Value.Stats.GetType().GetProperty(property.Name).GetValue(itemPreped.aItem.Value.Stats);
								//if (statValue != 0.0)
								//{
								//	sTooltip += "<br/>" + property.Name.Replace("Mod", "") + ": " + statValue;
								//}
								Double statValue = (Double)itemPreped.aItem.Value.Stats.GetType().GetProperty(property.Name).GetValue(itemPreped.aItem.Value.Stats);
								Double uniqueValue = 0.0;
								foreach (KeyValuePair<string, StatsStatic> uniqueStats in itemPreped.aItem.Value.UniqueStats)
								{
									uniqueValue += (Double)uniqueStats.Value.GetType().GetProperty(property.Name).GetValue(uniqueStats.Value);
								}

								Double newValue = statValue + uniqueValue;
								if (newValue != 0.0)
								{
									sTooltip += "<br/>" + property.Name.Replace("Mod", "") + ": " + newValue;
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.ToString());
						}

						sTooltip += "</p></div>";


						//tooltip
						UltraToolTipInfo temp = new UltraToolTipInfo();
						switch (thisPnl.Name)
						{
							case "pnlItem1":
								{
									temp = tipInfoItem1;
									break;
								}
							case "pnlItem2":
								{
									temp = tipInfoItem2;
									break;
								}
							case "pnlItem3":
								{
									temp = tipInfoItem3;
									break;
								}
							case "pnlItem4":
								{
									temp = tipInfoItem4;
									break;
								}
							case "pnlItem5":
								{
									temp = tipInfoItem5;
									break;
								}
							case "pnlItem6":
								{
									temp = tipInfoItem6;
									break;
								}

						}
						ultraToolTipManager1.SetUltraToolTip(itemPicBox, temp);
						temp.ToolTipTextFormatted = sTooltip;

					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void pnlItem_DragOver(object sender, DragEventArgs e)
		{
			try
			{
				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if (dataString.Contains("ITEM") && form1.itemsTab.getItemsFromServer.itemsPrepared != null)
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void pnlItem_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if (dataString.Contains("ITEM") && form1.itemsTab.getItemsFromServer.itemsPrepared != null)
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public void itemPicBox_MouseClick(object sender, MouseEventArgs e)
		{
			PictureBox c = sender as PictureBox;
			if (e.Button == MouseButtons.Left)
			{
				form1.itemsTab.GenerateItemDetails((CreateItemDiv)c.Tag);
			}
			else if (e.Button == MouseButtons.Right)
			{
				c.Parent.Tag = null;
				c.Dispose();
			}
		}


		private void pnlChampion_DragDrop(object sender, DragEventArgs e)
		{
			try
			{

				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if (dataString.Contains("CHAMPION") && form1.itemsTab.getItemsFromServer.itemsPrepared != null)
				{
					string championID = dataString.Replace("CHAMPION", "");
					if (form1.championsTab.getChampionsFromServer.championsOrder.Count > 0)
					{
						//Generate PicBox
						KeyValuePair<string, ChampionStatic> championPreped = form1.championsTab.getChampionsFromServer.championsOrder.Find(o => o.Value.Id.ToString() == championID);
						PictureBox championPicBox = new PictureBox();
						championPicBox.Size = new Size(48, 48);
						championPicBox.Location = new Point(0, 0);
						string file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, form1.championsTab.getChampionsFromServer.version, championPreped.Value.Image.Sprite);
						Image imageItem = Image.FromFile(file);
						Image image = CommonMethods.cropImage(imageItem, new Rectangle(championPreped.Value.Image.X, championPreped.Value.Image.Y, championPreped.Value.Image.Width, championPreped.Value.Image.Height));
						championPicBox.Image = image;
						championPicBox.MouseClick += championPicBox_MouseClick;
						championPicBox.Cursor = Cursors.Hand;
						championPicBox.Tag = championPreped;

						Panel thisPnl = sender as Panel;
						thisPnl.Tag = championPreped;
						if (thisPnl.Controls.Count > 0)
						{
							List<Control> ctrls = thisPnl.Controls.Cast<Control>().ToList();
							thisPnl.Controls.Clear();
							foreach (Control c in ctrls)
								c.Dispose();
						}
						thisPnl.Controls.Add(championPicBox);

						//Todo: this is temp tooltip used to see base stats for the champion
						string sTooltip = string.Format(@"
						<div style='max-width:300px;'>
						<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
							<span style='font-size:12pt;'>{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> <br/><br/>
							Armor: {1}<br/>
							ArmorPerLevel: {2}<br/>
							AttackDamage: {3}<br/>
							AttackDamagePerLevel: {4}<br/>
							AttackRange: {5}<br/>
							AttackSpeed: {6}<br/>
							AttackSpeedPerLevel: {7}<br/>
							Crit: {8}<br/>
							CitPerLevel: {9}<br/>
							Hp: {10}<br/>
							HpPerLevel: {11}<br/>
							HpRegen: {12}<br/>
							HpRegenPerLevel: {13}<br/>
							MoveSpeed: {14}<br/>
							Mp: {15}<br/>
							MpPerLevel: {16}<br/>
							MpRegen: {17}<br/>
							MpRegenPerLevel: {18}<br/>
							SpellBlock: {19}<br/>
							SpellBlackPerLevel: {20}
						</p></div>",
									championPreped.Value.Name,
									championPreped.Value.Stats.Armor,
									championPreped.Value.Stats.ArmorPerLevel,
									championPreped.Value.Stats.AttackDamage,
									championPreped.Value.Stats.AttackDamagePerLevel,
									championPreped.Value.Stats.AttackRange,
									Math.Round((0.625 / (1 + championPreped.Value.Stats.AttackSpeedOffset)), 3, MidpointRounding.AwayFromZero),
									championPreped.Value.Stats.AttackSpeedPerLevel,
									championPreped.Value.Stats.Crit,
									championPreped.Value.Stats.CritPerLevel,
									championPreped.Value.Stats.Hp,
									championPreped.Value.Stats.HpPerLevel,
									championPreped.Value.Stats.HpRegen,
									championPreped.Value.Stats.HpRegenPerLevel,
									championPreped.Value.Stats.MoveSpeed,
									championPreped.Value.Stats.Mp,
									championPreped.Value.Stats.MpPerLevel,
									championPreped.Value.Stats.MpRegen,
									championPreped.Value.Stats.MpRegenPerLevel,
									championPreped.Value.Stats.SpellBlock,
									championPreped.Value.Stats.SpellBlockPerLevel);

						//tooltip Todo: Comment ou the next two lines to disable this tooltip
						ultraToolTipManager1.SetUltraToolTip(championPicBox, tipInfoChamp);
						tipInfoChamp.ToolTipTextFormatted = sTooltip;
					}
					
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}



		private void pnlChampion_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if (dataString.Contains("CHAMPION") && form1.championsTab.getChampionsFromServer.championsOrder != null)
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public void championPicBox_MouseClick(object sender, MouseEventArgs e)
		{
			PictureBox c = sender as PictureBox;
			if (e.Button == MouseButtons.Left)
			{
				form1.championsTab.championControl_MouseClick(((KeyValuePair<string, ChampionStatic>)c.Tag).Value); //GenerateItemDetails((CreateItemDiv)c.Tag);
			}
			else if (e.Button == MouseButtons.Right)
			{
				c.Parent.Tag = null;
				c.Dispose();
			}
		}
	}
}
