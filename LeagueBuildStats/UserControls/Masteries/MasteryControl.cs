using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RiotSharp.StaticDataEndpoint;
using LeagueBuildStats.Classes;
using Infragistics.Win.UltraWinToolTip;
using Infragistics.Win;
using LeagueBuildStats.Classes.General_Classes;

namespace LeagueBuildStats.UserControls.Masteries
{
	public partial class MasteryControl : UserControl
	{
		private Image imageColor;
		private Image imageGray;
		private MasteriesTab masteriesTab;
		public MasteryStatic masteryData; 
		public int ranks;
		public int currentRank = 0;
		public int row = 0;
		public string treeType;
		public string state = "disabled";

		private UltraToolTipInfo tipInfo = new UltraToolTipInfo();

		public void GenerateMasteryControl(Image image, Image imageGray, MasteriesTab masteriesTab, MasteryStatic masteryData, int row, string treeType)
		{
			this.masteryData = masteryData;
			this.masteriesTab = masteriesTab;
			this.imageColor = image;
			this.imageGray = imageGray;
			this.row = row;
			this.treeType = treeType;
			ranks = masteryData.Rank;

			picBoxImage.Image = imageGray;
			picBoxBorder.Image = masteriesTab.GetGrayBox();
			txtBoxCount.Text = currentRank + "/" + ranks;

			if (row == 1)
			{
				SetToEnabled();
			}

			//Requirement Arrow
			if (masteryData.Prerequisite != null)
			{
				if (masteryData.Prerequisite != "0")
				{
					picBoxArrow.Visible = true;
					picBoxArrow.Image = masteriesTab.GetGrayArrow();
				}
			}


			//Tooltip Prepraration
			tipInfo.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			UpdateToolTip();
			ultraToolTipManager1.SetUltraToolTip(picBoxBorder, tipInfo);
			ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Office2007;
			ultraToolTipManager1.Appearance.BackColor = Color.Black;
			ultraToolTipManager1.Appearance.BackColor2 = Color.Black;
			ultraToolTipManager1.Appearance.BackColorAlpha = Alpha.Transparent;
		}

		private void UpdateToolTip()
		{
			//Prepare color of title
			string titleColor = "Black";
			switch (treeType)
			{
				case "offence":
					titleColor = "Red";
					break;
				case "defence":
					titleColor = "Blue";
					break;
				case "utility":
					titleColor = "Green";
					break;
			}

			//Preprare description
			string sDescription = "";
			if (currentRank > 0 && currentRank < ranks && masteryData.Description.Count > 1)
			{
				sDescription = string.Format(@"
					{0}<br/>
					<br/>
					Next Rank:<br/>
					{1}",
						StringExtensions.BrWrapper(masteryData.Description[currentRank - 1]), StringExtensions.BrWrapper(masteryData.Description[currentRank]));
			}
			else if (currentRank == ranks)
			{
				sDescription = StringExtensions.BrWrapper(masteryData.Description[ranks - 1]);
			}
			else
			{
				sDescription = StringExtensions.BrWrapper(masteryData.Description[0]);
			}
			
			//Compile Description
			string tipDesc = string.Format(@"
				<div style='max-width:300px;'>
				<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
					<span style='color:{0}; font-weight:bold; font-size:11pt;'>{1}</span> <br/>
					Rank: {2}/{3} <br/>
					<br/>
					{4}
				</p></div>", titleColor, masteryData.Name, currentRank, ranks, sDescription);

			//Todo: temp description
			tipDesc = string.Format(@"
				<div style='max-width:300px;'>
				<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
					<span style='color:{0}; font-weight:bold; font-size:11pt;'>{1}</span> <br/>
					Rank: {2}/{3} <br/>
					<br/>
					{4}<br/>"//</p></div>"
						, titleColor, masteryData.Name, currentRank, ranks, sDescription);

			//todo: this is a test to show the stats of the items
			try
			{
				Type myType = typeof(StatsStatic);
				System.Reflection.PropertyInfo[] properties = myType.GetProperties();

				foreach (System.Reflection.PropertyInfo property in properties)
				{
					int i = (currentRank - 1 > 0) ? (currentRank - 1) : 0;
					Double statValue = (Double)masteryData.StatsList[i].GetType().GetProperty(property.Name).GetValue(masteryData.StatsList[i]);
					if (statValue != 0.0)
					{
						tipDesc += "<br/>" + property.Name.Replace("Mod", "") + ": " + statValue;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			tipDesc += "</p></div>";



			tipInfo.ToolTipTextFormatted = tipDesc.Replace("<br>","<br/>");
		}

		
		

		public MasteryControl()
		{
			InitializeComponent();
			picBoxBorder.Parent = picBoxImage; //allows transparency to the image
			picBoxBorder.Location = new Point(0, 0);
			picBoxArrow.Visible = false;

			picBoxBorder.MouseClick += picBoxBorder_MouseClick;			
		}


		void picBoxBorder_MouseClick(object sender, MouseEventArgs e)
		{
			//Updates Mastery
			masteriesTab.SuspendLayout();
			if (e.Button == MouseButtons.Left)
			{
				if (currentRank < ranks && 
					state == "enabled" &&
					masteriesTab.mastTotalCount < 30)
				{
					currentRank++;
					txtBoxCount.Text = currentRank + "/" + ranks;
					switch (treeType)
					{
						case "offence":
							masteriesTab.mastOffenciveCount++;
							string iNum = masteriesTab.rowCountStorage.Keys.First(o => o.Contains(row.ToString()) && o.Contains("offence"));
							int newCount = masteriesTab.rowCountStorage[iNum] + 1;
							masteriesTab.rowCountStorage[iNum] = newCount;
							break;
						case "defence":
							masteriesTab.mastDefenciveCount++;
							iNum = masteriesTab.rowCountStorage.Keys.First(o => o.Contains(row.ToString()) && o.Contains("defence"));
							newCount = masteriesTab.rowCountStorage[iNum] + 1;
							masteriesTab.rowCountStorage[iNum] = newCount;
							break;
						case "utility":
							masteriesTab.mastUtilityCount++;
							iNum = masteriesTab.rowCountStorage.Keys.First(o => o.Contains(row.ToString()) && o.Contains("utility"));
							newCount = masteriesTab.rowCountStorage[iNum] + 1;
							masteriesTab.rowCountStorage[iNum] = newCount;
							break;
					}
					masteriesTab.mastTotalCount++;
					if (currentRank == ranks)
					{
						SetToMaxed();
					}
					if (masteriesTab.mastTotalCount == 30)
					{
						masteriesTab.UpdateAllMasteries(masteryData.Id, "add", "ALL");
					}
					else
					{
						masteriesTab.UpdateAllMasteries(masteryData.Id, "add", treeType);
					}
					//Update Tooltip
					UpdateToolTip();
				}
			}
			if (e.Button == MouseButtons.Right)
			{
				if (currentRank > 0)
				{
					if (masteriesTab.CheckAllMasteries(masteryData.Id, "sub", treeType, row))
					{
						currentRank--;
						txtBoxCount.Text = currentRank + "/" + ranks;
						masteriesTab.mastTotalCount--;
						SetToEnabled();

						switch (treeType)
						{
							case "offence":
								masteriesTab.mastOffenciveCount--;
								string iNum = masteriesTab.rowCountStorage.Keys.First(o => o.Contains(row.ToString()) && o.Contains("offence"));
								int newCount = masteriesTab.rowCountStorage[iNum] - 1;
							masteriesTab.rowCountStorage[iNum] = newCount;
								break;
							case "defence":
								masteriesTab.mastDefenciveCount--;
								iNum = masteriesTab.rowCountStorage.Keys.First(o => o.Contains(row.ToString()) && o.Contains("defence"));
								newCount = masteriesTab.rowCountStorage[iNum] - 1;
							masteriesTab.rowCountStorage[iNum] = newCount;
								break;
							case "utility":
								masteriesTab.mastUtilityCount--;
								iNum = masteriesTab.rowCountStorage.Keys.First(o => o.Contains(row.ToString()) && o.Contains("utility"));
								newCount = masteriesTab.rowCountStorage[iNum] - 1;
							masteriesTab.rowCountStorage[iNum] = newCount;
								break;
						}
						if (masteriesTab.mastTotalCount == 29)
						{
							masteriesTab.UpdateAllMasteries(masteryData.Id, "sub", "ALL");
						}
						else
						{
							masteriesTab.UpdateAllMasteries(masteryData.Id, "sub", treeType);
						}
						//Update Tooltip
						UpdateToolTip();
					}
				}
			}
			ultraToolTipManager1.ShowToolTip(picBoxBorder);

			masteriesTab.UpdateTreeText();
			masteriesTab.ResumeLayout();
		}

		public int FindTreeCount()
		{
			int treeCount = -1;
			switch(treeType)
			{
				case "offence":
					treeCount = masteriesTab.mastOffenciveCount;
					break;
				case "defence":
					treeCount = masteriesTab.mastDefenciveCount;
					break;
				case "utility":
					treeCount = masteriesTab.mastUtilityCount;
					break;
			}
			return treeCount;
		}

		public bool CheckIfActionAllowed(int id, string addOrSub, string treeName, int rowChanging)
		{
			bool success = true;
			if (masteryData.Id == 4123)
			{
				success = true;
			}
			
			if (addOrSub == "add")
			{
				success = true;
			}
			else if (addOrSub == "sub")
			{
				if (masteryData.Prerequisite == id.ToString() && currentRank == ranks)
				{
					success = false;
				}
				else if (treeName == this.treeType && currentRank > 0 && rowChanging < row)//&& FindTreeCount() <= (row - 1) * 4
				{
					int prevRowTotal = 0;
					int counter = 1;
					foreach (KeyValuePair<string,int> rowCount in masteriesTab.rowCountStorage.Where(o => o.Key.Contains(treeType)))
					{
						if (counter < row)
						{
							prevRowTotal += rowCount.Value;
						}
						counter++;
					}
					if (prevRowTotal > (row - 1) * 4)
					{
						success = true;
					}
					else
					{
						success = false;
					}
				}
				else
				{
					success = true;
				}
			}
			else
			{
				success = false;
			}
			return success;
		}

		public void PerformUpdates(int id, string addOrSub, string treeName)
		{
			if (addOrSub == "add")
			{
				if (masteriesTab.mastTotalCount == 30 && currentRank == 0)
				{
					SetToDisabled();
				}
				else if (FindTreeCount() >= (row - 1) * 4)
				{
					if (masteryData.Prerequisite == "0")
					{
						if (currentRank == ranks)
						{
							SetToMaxed();
						}
						else
						{
							SetToEnabled();
						}
					}
					else
					{
						MasteryControl cPrereq = masteriesTab.listOfMasteryControls[masteryData.Prerequisite];
						if (cPrereq.currentRank == cPrereq.ranks)
						{
							if (currentRank == ranks)
							{
								SetToMaxed();
							}
							else
							{
								SetToEnabled();
							}
						}
					}
				}
			}
			else if (addOrSub == "sub")
			{
				if (masteriesTab.mastTotalCount < 30)
				{
					if (FindTreeCount() >= (row - 1) * 4)
					{
						if (masteryData.Prerequisite == "0")
						{
							if (currentRank == ranks)
							{
								SetToMaxed();
							}
							else
							{
								SetToEnabled();
							}
						}
						else
						{
							MasteryControl cPrereq = masteriesTab.listOfMasteryControls[masteryData.Prerequisite];
							if (cPrereq.currentRank < cPrereq.ranks)
							{
								SetToDisabled();
							}
						}
					}
					else if (FindTreeCount() < (row - 1) * 4)
					{
						SetToDisabled();
					}
				}
			}

		}

		public void SetToEnabled()
		{
			picBoxImage.Image = imageColor;
			picBoxBorder.Image = masteriesTab.GetGreenBox();
			picBoxArrow.Image = masteriesTab.GetGreenArrow();
			state = "enabled";
		}

		public void SetToMaxed()
		{
			picBoxImage.Image = imageColor;
			picBoxBorder.Image = masteriesTab.GetRedBox();
			picBoxArrow.Image = masteriesTab.GetRedArrow();
			state = "maxed";
		}

		public void SetToDisabled()
		{
			picBoxImage.Image = imageGray;
			picBoxBorder.Image = masteriesTab.GetGrayBox();
			picBoxArrow.Image = masteriesTab.GetGrayArrow();
			state = "disabled";
		}



		internal void ResetMastery()
		{
			currentRank = 0;
			txtBoxCount.Text = currentRank + "/" + ranks;
			if (row == 1)
			{
				SetToEnabled();
			}
			else
			{
				SetToDisabled();
			}
		}

	}
}
