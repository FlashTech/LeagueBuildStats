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
using LeagueBuildStats.Classes.Masteries;
using Newtonsoft.Json;
using RiotSharp.StaticDataEndpoint;
using LeagueBuildStats.Classes;
using LeagueBuildStats.UserControls.Masteries;

using Infragistics.Win;
using Infragistics.Win.UltraWinToolTip;
using Infragistics.Win.FormattedLinkLabel;

namespace LeagueBuildStats.UserControls
{
	public partial class MasteriesTab : UserControl
	{
		public GetMasteriesFromServer getMasteriesFromServer = new GetMasteriesFromServer();

		private Form1 form;
		private Image masterSprite;

		public Dictionary<string, MasteryControl> listOfMasteryControls = new Dictionary<string, MasteryControl>();
		internal Dictionary<string, int> rowCountStorage = new Dictionary<string, int>();

		public Image GetGrayArrow() { return CommonMethods.cropImage(masterSprite, new Rectangle(62, 0, 21, 75)); }
		public Image GetGreenArrow() { return CommonMethods.cropImage(masterSprite, new Rectangle(83, 0, 21, 75)); }
		public Image GetRedArrow() { return CommonMethods.cropImage(masterSprite, new Rectangle(104, 0, 21, 75)); }
		public Image GetGrayBox() { return CommonMethods.cropImage(masterSprite, new Rectangle(127, 0, 52, 52)); }
		public Image GetGreenBox() { return CommonMethods.cropImage(masterSprite, new Rectangle(185, 0, 52, 52)); }
		public Image GetRedBox() { return CommonMethods.cropImage(masterSprite, new Rectangle(243, 0, 52, 52)); }

		public int mastOffenciveCount = 0;
		public int mastTotalCount = 0;
		public int mastDefenciveCount = 0;
		public int mastUtilityCount = 0;

		public MasteriesTab(Form1 form)
		{
			this.form = form;
			InitializeComponent();

			dropDownBtnReturnPoint.MouseClick += dropDownBtnReturnPoints_MouseClick;

			rowCountStorage.Add("offenceRow1Count", 0);
			rowCountStorage.Add("offenceRow2Count", 0);
			rowCountStorage.Add("offenceRow3Count", 0);
			rowCountStorage.Add("offenceRow4Count", 0);
			rowCountStorage.Add("offenceRow5Count", 0);
			rowCountStorage.Add("offenceRow6Count", 0);
			rowCountStorage.Add("defenceRow1Count", 0);
			rowCountStorage.Add("defenceRow2Count", 0);
			rowCountStorage.Add("defenceRow3Count", 0);
			rowCountStorage.Add("defenceRow4Count", 0);
			rowCountStorage.Add("defenceRow5Count", 0);
			rowCountStorage.Add("defenceRow6Count", 0);
			rowCountStorage.Add("utilityRow1Count", 0);
			rowCountStorage.Add("utilityRow2Count", 0);
			rowCountStorage.Add("utilityRow3Count", 0);
			rowCountStorage.Add("utilityRow4Count", 0);
			rowCountStorage.Add("utilityRow5Count", 0);
			rowCountStorage.Add("utilityRow6Count", 0);
		}

		void dropDownBtnReturnPoints_MouseClick(object sender, MouseEventArgs e)
		{
			ResetAllMasteries();
		}

		internal void UpdateTreeText()
		{
			lblOffenceCount.Text = "OFFENCE: " + mastOffenciveCount;
			lblDefenceCount.Text = "DEFENCE: " + mastDefenciveCount;
			lblUtilityCount.Text = "UTILITY: " + mastUtilityCount;
			lblPointsAvail.Text = "Points Available: " + mastTotalCount;
		}

		public bool CollectMasteryData(string inputVersion = null)
		{
			bool success;
			if (inputVersion == null)
			{
				success = getMasteriesFromServer.DownloadListOfMasteries();
			}
			else
			{
				success = getMasteriesFromServer.DownloadListOfMasteries(inputVersion);
			}

			if (success)
			{
				success = getMasteriesFromServer.GetMasteriesImages();
			}
			return success;
		}

		public bool UpdateRuneControl()
		{
			bool success = false;
			picBoxMasteryMain.SuspendLayout();
			try
			{
				string file = string.Format(@"{0}\Data\Masteries\Images\{1}\masteryback.jpg", PublicStaticVariables.thisAppDataDir, getMasteriesFromServer.version);
				Image imageMasteryMain = Image.FromFile(file);
				picBoxMasteryMain.Image = imageMasteryMain;
				
				listOfMasteryControls.Clear();

				ResetAllMasteries();

				//Moves labels out of picBoxMain and then clears controls
				lblOffenceCount.Parent = xtraScrollableControl1;
				lblDefenceCount.Parent = xtraScrollableControl1;
				lblUtilityCount.Parent = xtraScrollableControl1;
				if (picBoxMasteryMain.Controls.Count > 0)
				{
					List<Control> ctrls = picBoxMasteryMain.Controls.Cast<Control>().ToList();
					picBoxMasteryMain.Controls.Clear();
					foreach (Control c in ctrls)
						c.Dispose();
				}
				//Forces these labels to be transparent on top of picBoxMain
				lblOffenceCount.Parent = picBoxMasteryMain;
				lblOffenceCount.BackColor = Color.Transparent;
				lblDefenceCount.Parent = picBoxMasteryMain;
				lblDefenceCount.BackColor = Color.Transparent;
				lblUtilityCount.Parent = picBoxMasteryMain;
				lblUtilityCount.BackColor = Color.Transparent;

				file = string.Format(@"{0}\Data\Masteries\Images\{1}\mastersprite.png", PublicStaticVariables.thisAppDataDir, getMasteriesFromServer.version);
				masterSprite = Image.FromFile(file);

				//Offence tree
				int xPos = 22;
				int yPos = 0;
				int row = 1;
				foreach (MasteryTreeListStatic masteryList in getMasteriesFromServer.masteries.Tree.Offense)
				{
					foreach (MasteryTreeItemStatic masteryTreeItem in masteryList.MasteryTreeItems)
					{
						if (masteryTreeItem != null)
						{
							GenerateMasteryItemControl(masteryTreeItem, xPos, yPos, row, "offence");
						}
						xPos += 62;
					}
					xPos = 22;
					yPos += 72;
					row++;
				}
				//Defence tree
				xPos = 295;
				yPos = 0;
				row = 1;
				foreach (MasteryTreeListStatic masteryList in getMasteriesFromServer.masteries.Tree.Defense)
				{
					foreach (MasteryTreeItemStatic masteryTreeItem in masteryList.MasteryTreeItems)
					{
						if (masteryTreeItem != null)
						{
							GenerateMasteryItemControl(masteryTreeItem, xPos, yPos, row, "defence");
						}
						xPos += 62;
					}
					xPos = 295;
					yPos += 72;
					row++;
				}
				//Utility tree
				xPos = 570;
				yPos = 0;
				row = 1;
				foreach (MasteryTreeListStatic masteryList in getMasteriesFromServer.masteries.Tree.Utility)
				{
					foreach (MasteryTreeItemStatic masteryTreeItem in masteryList.MasteryTreeItems)
					{
						if (masteryTreeItem != null)
						{
							GenerateMasteryItemControl(masteryTreeItem, xPos, yPos, row, "utility");
						}
						xPos += 62;
					}
					xPos = 570;
					yPos += 72;
					row++;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				picBoxMasteryMain.ResumeLayout();
			}

			return success;
		}

		private void GenerateMasteryItemControl(MasteryTreeItemStatic masteryTreeItem, int xPos, int yPos, int row, string treeType)
		{
			try
			{
				MasteryStatic masteryData = getMasteriesFromServer.masteries.Masteries.FirstOrDefault(o => o.Value.Id == masteryTreeItem.MasteryId).Value;
				string file = string.Format(@"{0}\Data\Masteries\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getMasteriesFromServer.version, masteryData.Image.Sprite);
				Image masteryImage = Image.FromFile(file);
				masteryImage = CommonMethods.cropImage(masteryImage, new Rectangle(masteryData.Image.X, masteryData.Image.Y, masteryData.Image.Width, masteryData.Image.Height));

				string grayFile = string.Format(@"{0}\Data\Masteries\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getMasteriesFromServer.version, "gray_" + masteryData.Image.Sprite);
				Image masteryGrayImage = Image.FromFile(grayFile);
				Image grayMasteryImage = CommonMethods.cropImage(masteryGrayImage, new Rectangle(masteryData.Image.X, masteryData.Image.Y, masteryData.Image.Width, masteryData.Image.Height));

				MasteryControl masteryControl = new MasteryControl();
				masteryControl.GenerateMasteryControl(masteryImage, grayMasteryImage, this, masteryData, row, treeType);
				masteryControl.Location = new Point(xPos, yPos);

				picBoxMasteryMain.Controls.Add(masteryControl);
				masteryControl.SendToBack();

				listOfMasteryControls.Add(masteryData.Id.ToString(), masteryControl);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}


		public bool CheckAllMasteries(int id, string addOrSub, string treeName, int rowChanging)
		{
			bool finalSuccess = true;
			foreach (KeyValuePair<string, MasteryControl> mControl in listOfMasteryControls)
			{
				if (mControl.Value.treeType == treeName)
				{
					bool success = mControl.Value.CheckIfActionAllowed(id, addOrSub, treeName, rowChanging);
					if (!success) { finalSuccess = false; }
				}
			}
			return finalSuccess;
		}

		public void UpdateAllMasteries(int id, string addOrSub, string treeName)
		{
			foreach (KeyValuePair<string, MasteryControl> mControl in listOfMasteryControls)
			{
				if (treeName == "ALL" && addOrSub == "add" && mControl.Value.currentRank == 0)
				{
					mControl.Value.SuspendLayout();
					mControl.Value.SetToDisabled();
					mControl.Value.ResumeLayout();
				}
				else if (treeName == "ALL" && addOrSub == "sub" && mControl.Value.currentRank == 0)
				{
					mControl.Value.SuspendLayout();
					mControl.Value.PerformUpdates(id, addOrSub, treeName);
					mControl.Value.ResumeLayout();
				}
				else if (mControl.Value.treeType == treeName)
				{
					mControl.Value.SuspendLayout();
					mControl.Value.PerformUpdates(id, addOrSub, treeName);
					mControl.Value.ResumeLayout();
				}
			}
		}

		private void ResetAllMasteries()
		{
			try
			{
				mastOffenciveCount = 0;
				mastTotalCount = 0;
				mastDefenciveCount = 0;
				mastUtilityCount = 0;
				UpdateTreeText();
				if (listOfMasteryControls != null)
				{
					foreach (KeyValuePair<string, MasteryControl> mControl in listOfMasteryControls)
					{
						mControl.Value.ResetMastery();
					}
				}
				var keys = new List<string>(rowCountStorage.Keys);
				foreach (string key in keys)
				{
					rowCountStorage[key] = 0;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

	}
}
