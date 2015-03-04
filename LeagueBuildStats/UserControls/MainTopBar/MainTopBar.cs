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
using System.Reflection;
using LeagueBuildStats.Classes.General_Classes;

namespace LeagueBuildStats.UserControls.MainTopBar
{
	public partial class MainTopBar : UserControl
	{
		private LeagueBuildStatsForm form1;

		public UltraToolTipInfo tipInfoChamp = new UltraToolTipInfo();

		public UltraToolTipInfo tipInfoItem1 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem2 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem3 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem4 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem5 = new UltraToolTipInfo();
		public UltraToolTipInfo tipInfoItem6 = new UltraToolTipInfo();

		public UltraToolTipInfo tipInfoItemElixir = new UltraToolTipInfo();

		public List<Panel> itemPanels = new List<Panel>();

		public ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

		//TODO: maybe instead of looking directly are the tag is may be better to have public variables that are updated when the tag is updated. might not be necessary
		public KeyValuePair<string, ChampionStatic> champion1 { get { return (pnlChampion.Tag != null ? (KeyValuePair<string, ChampionStatic>)pnlChampion.Tag : new KeyValuePair<string, ChampionStatic>()); } }
		public CreateItemDiv item1 { get { return (pnlItem1.Tag != null ? (CreateItemDiv)pnlItem1.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item2 { get { return (pnlItem2.Tag != null ? (CreateItemDiv)pnlItem2.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item3 { get { return (pnlItem3.Tag != null ? (CreateItemDiv)pnlItem3.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item4 { get { return (pnlItem4.Tag != null ? (CreateItemDiv)pnlItem4.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item5 { get { return (pnlItem5.Tag != null ? (CreateItemDiv)pnlItem5.Tag : new CreateItemDiv()); } }
		public CreateItemDiv item6 { get { return (pnlItem6.Tag != null ? (CreateItemDiv)pnlItem6.Tag : new CreateItemDiv()); } }
		public CreateItemDiv itemElixir { get { return (pnlElixir.Tag != null ? (CreateItemDiv)pnlElixir.Tag : new CreateItemDiv()); } }

		public MainTopBar(LeagueBuildStatsForm form)
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
			itemPanels.Add(pnlElixir);
			


			//Tooltips prep
			ultraToolTipManagerGearIcon.AutoPopDelay = 0;
			ultraToolTipManagerGearIcon.InitialDelay = 50;
			ultraToolTipManagerGearIcon.DisplayStyle = ToolTipDisplayStyle.Office2007;
			ultraToolTipManagerGearIcon.Appearance.BackColor = Color.Black;
			ultraToolTipManagerGearIcon.Appearance.BackColor2 = Color.Black;
			ultraToolTipManagerGearIcon.Appearance.BackColorAlpha = Alpha.Transparent;

			tipInfoChamp.ToolTipTextStyle = ToolTipTextStyle.Formatted;

			tipInfoItem1.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem2.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem3.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem4.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem5.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoItem6.ToolTipTextStyle = ToolTipTextStyle.Formatted;

			tipInfoItemElixir.ToolTipTextStyle = ToolTipTextStyle.Formatted;

		}

		public void InitializeGoldImage()
		{
			//Gold Image
			string file = string.Format(@"{0}\Data\Masteries\Images\{1}\mastersprite.png", PublicStaticVariables.thisAppDataDir, form1.getAllVersionAvailable.realm.V);
			Image masterSprite = Image.FromFile(file);

			picBoxGold.Image = CommonMethods.cropImage(masterSprite, new Rectangle(301, 0, 23, 17));
		}

		public void UpdateTotalGoldCost()
		{
			int totalCost = 0;
			totalCost += GetCost(pnlItem1);
			totalCost += GetCost(pnlItem2);
			totalCost += GetCost(pnlItem3);
			totalCost += GetCost(pnlItem4);
			totalCost += GetCost(pnlItem5);
			totalCost += GetCost(pnlItem6);
			totalCost += GetCost(pnlElixir);

			lblPriceOfItems.Text = string.Format("{0:n0}", totalCost);
		}

		private int GetCost(Panel pnlItem)
		{
			int cost = 0;
			if (pnlItem.Tag != null)
			{
				cost = ((CreateItemDiv)pnlItem.Tag).aItem.Gold.TotalPrice;
			}
			return cost;
		}


		private void InitializeEvents()
		{
			simpleBtnChkUpdates.Click += simpleBtnChkUpdates_Click;

			pnlChampion.DragEnter += pnlChampion_DragEnter;

			pnlItem1.DragEnter += pnlItem_DragEnter;
			pnlItem2.DragEnter += pnlItem_DragEnter;
			pnlItem3.DragEnter += pnlItem_DragEnter;
			pnlItem4.DragEnter += pnlItem_DragEnter;
			pnlItem5.DragEnter += pnlItem_DragEnter;
			pnlItem6.DragEnter += pnlItem_DragEnter;

			pnlElixir.DragEnter += pnlElixir_DragEnter;


			pnlChampion.DragDrop += pnlChampion_DragDrop;

			pnlItem1.DragDrop += pnlItem_DragDrop;
			pnlItem2.DragDrop += pnlItem_DragDrop;
			pnlItem3.DragDrop += pnlItem_DragDrop;
			pnlItem4.DragDrop += pnlItem_DragDrop;
			pnlItem5.DragDrop += pnlItem_DragDrop;
			pnlItem6.DragDrop += pnlItem_DragDrop;

			pnlElixir.DragDrop += pnlItem_DragDrop;

			picBoxInfoButton.Image = Bitmap.FromHicon(SystemIcons.Information.Handle);

			picBoxInfoButton.MouseClick += picBoxInfoButton_MouseClick;
			picBoxSettings.MouseClick += picBoxSettings_MouseClick;
			btnCheckUpdates.MouseClick += btnCheckUpdates_MouseClick;

			contextMenuStrip1.Items.Add("Show detected stats in tooltips");
			((ToolStripMenuItem)contextMenuStrip1.Items[0]).CheckOnClick = true;
			((ToolStripMenuItem)contextMenuStrip1.Items[0]).CheckedChanged+=Item1_CheckedChanged; //Todo: rename this
		}

		
		void pnlElixir_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if (dataString.Contains("ELIXIR") && form1.itemsTab.getItemsFromServer.itemsPrepared != null)
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}


		private void Item1_CheckedChanged(object sender, EventArgs e)
		{
			//The following are already dynamic based on if checked = true
			if (pnlChampion.Tag != null)
			{
				string sTooltip = CreateChampPicBoxTooltip((KeyValuePair<string, ChampionStatic>)pnlChampion.Tag);
				tipInfoChamp.ToolTipTextFormatted = sTooltip;
			}
			if (pnlItem1.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlItem1.Tag);
				tipInfoItem1.ToolTipTextFormatted = sTooltip;
			}
			if (pnlItem2.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlItem2.Tag);
				tipInfoItem2.ToolTipTextFormatted = sTooltip;
			}
			if (pnlItem3.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlItem3.Tag);
				tipInfoItem3.ToolTipTextFormatted = sTooltip;
			}
			if (pnlItem4.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlItem4.Tag);
				tipInfoItem4.ToolTipTextFormatted = sTooltip;
			}
			if (pnlItem5.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlItem5.Tag);
				tipInfoItem5.ToolTipTextFormatted = sTooltip;
			}
			if (pnlItem6.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlItem6.Tag);
				tipInfoItem6.ToolTipTextFormatted = sTooltip;
			}
			if (pnlElixir.Tag != null)
			{
				string sTooltip = CreateItemPicBoxTooltip((CreateItemDiv)pnlElixir.Tag);
				tipInfoItemElixir.ToolTipTextFormatted = sTooltip;
			}
		}


		void picBoxSettings_MouseClick(object sender, MouseEventArgs e)
		{
			PictureBox btnSender = (PictureBox)sender;
			Point ptLowerLeft = new Point(0, btnSender.Height);
			ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
			contextMenuStrip1.Show(ptLowerLeft);
		}

		void btnCheckUpdates_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				string folderPath = Assembly.GetExecutingAssembly().Location.Replace(Assembly.GetExecutingAssembly().GetName().Name + ".exe", "");
				System.Diagnostics.Process.Start(folderPath + "LeagueBuildStatsUpdater.exe");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		void picBoxInfoButton_MouseClick(object sender, MouseEventArgs e)
		{
			string message = string.Format(@"LeagueBuildStats isn’t endorsed by Riot Games and doesn’t reflect the views or opinions of Riot Games or anyone 
officially involved in producing or managing League of Legends. League of Legends and Riot Games are trademarks or
registered trademarks of Riot Games, Inc. League of Legends © Riot Games, Inc.");

			string caption = "League Build Stats - Information";
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			DialogResult result;

			result = XtraMessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Information);

			if (result == DialogResult.OK)
			{
				//do nothing
			}
		}
		

		void simpleBtnChkUpdates_Click(object sender, EventArgs e)
		{
			Realm tempRealm = form1.getAllVersionAvailable.realm;
			List<string> tempVersions = form1.getAllVersionAvailable.versions;
			bool getVersionsSuccess = form1.getAllVersionAvailable.CollectVersionData();
			if (getVersionsSuccess)
			{
				if (tempRealm.V == form1.getAllVersionAvailable.realm.V && tempVersions.Count == form1.getAllVersionAvailable.versions.Count)
				{
					XtraMessageBox.Show(string.Format(@"League of Legends versions are currently up to date."), "League Build Stats - Notice");
				}
				else
				{
					UpdateMainTopBarVersions();
					XtraMessageBox.Show(string.Format(@"League of Legends Versions have been updated!"), "League Build Stats - Notice");
				}
			}
		}


		/// <summary>
		/// Clears and re-creates the dropdown selection of League of Legends versions
		/// </summary>
		internal void UpdateMainTopBarVersions()
		{
			try
			{
				int id = 1;

				//Get current selected verions
				string currentVersion = "";
				if (dropDownButtonRiotVersion.Text.Contains(" "))
				{
					currentVersion = SubstringExtensions.Before(dropDownButtonRiotVersion.Text, " ");
				}
				else
				{
					currentVersion = dropDownButtonRiotVersion.Text;
				}

				//If the selected version is still current then add back in the " Current LoL"
				if (currentVersion == form1.getAllVersionAvailable.realm.V)
				{
					currentVersion += " Current LoL";
				}


				popupMenuVersions.LinksPersistInfo.Clear();
				//barManager1.Items.Clear();
				foreach (string s in form1.getAllVersionAvailable.versions)
				{
					BarButtonItem barButtonItemNew = new BarButtonItem();
					popupMenuVersions.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItemNew));

					if (form1.getAllVersionAvailable.realm.V == s)
					{
						barButtonItemNew.Caption = s + " Current LoL";
						if (currentVersion.Length < 2)
						{
							dropDownButtonRiotVersion.Text = barButtonItemNew.Caption;
						}
						else
						{
							dropDownButtonRiotVersion.Text = currentVersion;
						}
					}
					else
					{
						barButtonItemNew.Caption = s;
					}
					barButtonItemNew.ItemClick += barButtonItemVersionList_ItemClick;
					barButtonItemNew.Id = id;
					barButtonItemNew.Name = s;

					//barManager1.Items.Add(barButtonItemNew);

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

				success = form1.LoadRiotDataFromFile(selName);
				if (!success)
				{
					success = form1.DataCollection(selName);
				}
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.ToString());
			}

			if (success)
			{
				//Performs the update to all tabes
				form1.UpdateFormWithData(false); //Won't recreate the version drop down list

				ClearSelectionsFromOldVersion();

				
				//Update dropdown
				dropDownButtonRiotVersion.Text = selection;
			}
			SplashForm.CloseForm();
		}

		private void ClearSelectionsFromOldVersion()
		{
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
				catch
				{
					//Do nothing
				}
			}
			form1.itemsTab.createItemSortMenu.txtEditSearchBar.Text = "";

			//Clear Champion and Items selected in mainTopBar
			List<Control> ctrlsToClear = new List<Control>() { pnlChampion, pnlItem1, pnlItem2, pnlItem3, pnlItem4, pnlItem5, pnlItem6, pnlElixir };
			foreach (Control cTop in ctrlsToClear)
			{
				if (cTop.Controls.Count > 0)
				{
					List<Control> ctrls = cTop.Controls.Cast<Control>().ToList();
					cTop.Controls.Clear();
					foreach (Control c in ctrls)
					{
						c.Dispose();
					}
				}
				cTop.Tag = null;
			}
			UpdateTotalGoldCost();

			//Clear Champion selection and spells on Champion Tab
			form1.championsTab.ClearChampionInfoSection();
			form1.championsTab.ClearSortingSelections();
		}

		private void pnlItem_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				string dataString = e.Data.GetData(DataFormats.Text) as string;
				if ( (dataString.Contains("ITEM") || dataString.Contains("ELIXIR") ) && form1.itemsTab.getItemsFromServer.itemsPrepared != null)
				{
					string itemID = dataString.Replace("ITEM", "");
					itemID = itemID.Replace("ELIXIR", "");
					if (form1.itemsTab.getItemsFromServer.itemsPrepared.Count > 0)
					{
						//Generate PicBox
						CreateItemDiv itemPreped = form1.itemsTab.getItemsFromServer.itemsPrepared.Find(o => o.thisID.ToString() == itemID);
						
						
						PictureBox itemPicBox = new PictureBox();
						itemPicBox.Size = new Size(48, 48);
						itemPicBox.Location = new Point(0, 0);
						string file = string.Format(@"{0}\Data\Items\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, itemPreped.thisVersion, itemPreped.aItem.Image.Sprite);
						Image imageItem = Image.FromFile(file);
						Image image = CommonMethods.cropImage(imageItem, new Rectangle(itemPreped.aItem.Image.X, itemPreped.aItem.Image.Y, itemPreped.aItem.Image.Width, itemPreped.aItem.Image.Height));
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
							{
								c.Dispose();
							}

						}
						thisPnl.Controls.Add(itemPicBox);

						string sTooltip = CreateItemPicBoxTooltip(itemPreped);

						


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
							case "pnlElixir":
								{
									temp = tipInfoItemElixir;
									break;
								}

						}
						ultraToolTipManagerGearIcon.SetUltraToolTip(itemPicBox, temp);
						temp.ToolTipTextFormatted = sTooltip;

					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			UpdateTotalGoldCost();
		}

		public string CreateItemPicBoxTooltip(CreateItemDiv itemPreped)
		{
			//Generate tooltip for item on maintopbar
			string sTooltip = string.Format(@"
						<div style='max-width:300px;'>
						<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
							<span style='font-size:12pt;'>{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> <br/>
							<span style='color:Yellow;'>Cost: {1}</span> <br/><br/>
							{2}<br/>
						",//</p></div>"
						itemPreped.thisItemDisplayName, itemPreped.aItem.Gold.TotalPrice, itemPreped.DivText);

			//todo: this is a test to show the stats of the items
			if (((ToolStripMenuItem)contextMenuStrip1.Items[0]).Checked)
			{
				try
				{
					Type myType = typeof(StatsStatic);
					System.Reflection.PropertyInfo[] properties = myType.GetProperties();

					foreach (System.Reflection.PropertyInfo property in properties)
					{
						Double statValue = (Double)itemPreped.aItem.Stats.GetType().GetProperty(property.Name).GetValue(itemPreped.aItem.Stats);
						Double uniqueValue = 0.0;
						foreach (KeyValuePair<string, StatsStatic> uniqueStats in itemPreped.aItem.UniqueStats)
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
			}

			sTooltip += "</p></div>";

			return sTooltip;
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
				UpdateTotalGoldCost();
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

						string sTooltip = CreateChampPicBoxTooltip(championPreped);

						

						//tooltip Todo: Comment ou the next two lines to disable this tooltip
						ultraToolTipManagerGearIcon.SetUltraToolTip(championPicBox, tipInfoChamp);
						tipInfoChamp.ToolTipTextFormatted = sTooltip;
					}
					
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public string CreateChampPicBoxTooltip(KeyValuePair<string, ChampionStatic> championPreped)
		{
			string sTooltip = "";
			if (((ToolStripMenuItem)contextMenuStrip1.Items[0]).Checked)
			{
				sTooltip = string.Format(@"
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
			}
//			else
//			{
//				sTooltip = string.Format(@"
//						<div style='max-width:300px;'>
//						<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
//							<span style='font-size:12pt;'>{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> <br/><br/>
//						</p></div>",
//						championPreped.Value.Name);
//			}



			return sTooltip;
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
