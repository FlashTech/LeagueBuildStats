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
using DevExpress.XtraNavBar;
using LeagueBuildStats.Classes.Runes;
using Newtonsoft.Json;
using RiotSharp.StaticDataEndpoint;
using LeagueBuildStats.Classes;
using LeagueBuildStats.Forms;
using Infragistics.Win.UltraWinToolTip;
using Infragistics.Win;
using LeagueBuildStats.UserControls.Runes;

namespace LeagueBuildStats.UserControls
{
	public partial class RunesTab : UserControl
	{
		public GetRunesFromServer getRunesFromServer = new GetRunesFromServer();

		private LeagueBuildStatsForm form;

		public PictureBox RuneBackgroundContainer
		{
			get { return picBoxRuneBackground; }
		}

		private UltraToolTipInfo tipInfo = new UltraToolTipInfo();

		RuneStatistics runeStatistics;

		public RunesTab(LeagueBuildStatsForm form)
		{
			this.form = form;
			InitializeComponent();

			runeStatistics = new RuneStatistics(this);
			runeStatistics.Dock = DockStyle.Fill;
			pnlCtrlStatisticsContainer.Controls.Add(runeStatistics);

			radioBntTier1.CheckedChanged += radioBntTier_CheckedChanged;
			radioBntTier2.CheckedChanged += radioBntTier_CheckedChanged;
			radioBntTier3.CheckedChanged += radioBntTier_CheckedChanged;

			btnClearAll.MouseClick += btnClearAll_MouseClick;

			foreach (Control p in picBoxRuneBackground.Controls)
			{
				if (p.Name.Contains("picBoxRune"))
				{
					p.MouseClick += p_MouseClick;
					p.MouseDoubleClick += p_MouseDoubleClick;
					p.MouseEnter +=p_MouseEnter;

					//All Runes have the same tooltip that is updated dynamically
					ultraToolTipManager1.SetUltraToolTip(p, tipInfo);
				}
			}

			//Tooltip Prepraration
			tipInfo.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Office2007;
		}

		void btnClearAll_MouseClick(object sender, MouseEventArgs e)
		{
			foreach (Control c in picBoxRuneBackground.Controls)
			{
				try
				{
					PictureBox p = c as PictureBox;
					if (p.Tag != null)
					{
						//remove from statistics
						runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, false);
					}
					p.Image = null;
					p.Tag = null;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		private void p_MouseEnter(object sender, EventArgs e)
		{
			Control c = sender as Control;
			if (c.Tag != null)
			{
				try
				{
					UpdateToolTip((RuneStatic)c.Tag);
				}
				catch (Exception ex)
				{
					tipInfo.ToolTipTextFormatted = "";
					MessageBox.Show(ex.ToString());
				}
			}
			else
			{
				tipInfo.ToolTipTextFormatted = "";
			}
		}
		//RuneStatic thisRune = getRunesFromServer.runes.Runes.FirstOrDefault(o => o.Value.Id.ToString() == hoveredID).Value;
		//toolTipForm.updateRuneTip("Tier " + thisRune.Metadata.Tier, thisRune.Name, thisRune.Description, thisRune.Metadata.Type);
		private void UpdateToolTip(RuneStatic thisRune)
		{


			//Prepare color of title
			string titleColor = "Black";
			switch (thisRune.Metadata.Type)
			{
				case "red":
					titleColor = "Red";
					break;
				case "yellow":
					titleColor = "#FBB117";
					break;
				case "blue":
					titleColor = "Blue";
					break;
				case "black":
					titleColor = "Purple";
					break;
			}


			//Compile Description
			string tipDesc = string.Format(@"
				<div style='max-width:300px;'>
				<p style='color:Black; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
					Tier: {0}<br/>
					<span style='color:{1}; font-weight:bold; font-size:11pt;'>{2}</span> <br/>
					{3}<br/>
				</p></div>", thisRune.Metadata.Tier, titleColor, thisRune.Name, thisRune.Description);
			tipInfo.ToolTipTextFormatted = tipDesc.Replace("<br>", "<br/>");
		}

		void p_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			PictureBox p = sender as PictureBox;
			if (p.Tag != null)
			{
				//remove from statistics
				runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, false);
			}
			p.Image = null;
			p.Tag = null;
		}

		void p_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				PictureBox p = sender as PictureBox;
				ultraToolTipManager1.ShowToolTip(p);
			}
			else if (e.Button == MouseButtons.Right)
			{
				PictureBox p = sender as PictureBox;
				if (p.Tag != null)
				{
					//remove from statistics
					runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, false);
				}
				p.Image = null;
				p.Tag = null;
			}
		}

		private void radioBntTier_CheckedChanged(object sender, EventArgs e)
		{
			navBarControl1.BeginUpdate();
			try
			{
				RadioButton radiobtn = sender as RadioButton;
				string tierNumber = radiobtn.Text.Replace("Tier ", "");
				foreach (NavBarItem navItem in navBarControl1.Items)
				{
					if (navItem.Tag.ToString() == tierNumber)
					{
						if (radiobtn.Checked == true)
						{
							navItem.Visible = true;
						}
						else
						{
							navItem.Visible = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			navBarControl1.EndUpdate();
		}

		public bool CollectRuneData(string inputVersion = null)
		{
			bool success;
			if (inputVersion == null)
			{
				success = getRunesFromServer.DownloadListOfRunes();
			}
			else
			{
				success = getRunesFromServer.DownloadListOfRunes(inputVersion);
			}

			if (success)
			{
				success = getRunesFromServer.GetRunesImages();
			}
			return success;
		}

		public bool UpdateRuneControl()
		{
			bool success = false;
			navBarControl1.BeginUpdate();
			try
			{
				navBarControl1.Groups.Clear();
				navBarControl1.Items.Clear();
				foreach (Control c in picBoxRuneBackground.Controls)
				{
					if (c.Name.Contains("picBoxRuneSlot"))
					{
						try
						{
							PictureBox p = c as PictureBox;
							if (p.Tag != null)
							{
								//remove from statistics
								runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, false);
							}
							p.Image = null;
							p.Tag = null;
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.ToString());
						}
					}
				}

				NavBarGroup navBarGroupMarks = new NavBarGroup();
				navBarGroupMarks.Caption = "Marks";
				navBarGroupMarks.Expanded = false;
				navBarGroupMarks.Name = "Marks";
				NavBarGroup navBarGroupSeals = new NavBarGroup();
				navBarGroupSeals.Caption = "Seals";
				navBarGroupSeals.Expanded = false;
				navBarGroupSeals.Name = "Seals";
				NavBarGroup navBarGroupGlyphs = new NavBarGroup();
				navBarGroupGlyphs.Caption = "Glyphs";
				navBarGroupGlyphs.Expanded = false;
				navBarGroupGlyphs.Name = "Glyphs";
				NavBarGroup navBarGroupQuints = new NavBarGroup();
				navBarGroupQuints.Caption = "Quintessences";
				navBarGroupQuints.Expanded = false;
				navBarGroupQuints.Name = "Quintessences";


				foreach (KeyValuePair<int, RuneStatic> rune in getRunesFromServer.runeSorted)
				{
					NavBarItem navBarItemNew = new NavBarItem();
					navBarItemNew.Caption = rune.Value.Description;
					navBarItemNew.Name = rune.Value.Id.ToString();
					navBarItemNew.Tag = rune.Value.Metadata.Tier;
					navBarItemNew.LinkClicked += navBarItemNew_LinkClicked;
					switch (rune.Value.Metadata.Type)
					{
						case "red":
							navBarGroupMarks.ItemLinks.Add(navBarItemNew);
							break;
						case "yellow":
							navBarGroupSeals.ItemLinks.Add(navBarItemNew);
							break;
						case "blue":
							navBarGroupGlyphs.ItemLinks.Add(navBarItemNew);
							break;
						case "black":
							navBarGroupQuints.ItemLinks.Add(navBarItemNew);
							break;
					}
					navBarControl1.Items.Add(navBarItemNew);
				}

				navBarControl1.Groups.Add(navBarGroupMarks);
				navBarControl1.Groups.Add(navBarGroupSeals);
				navBarControl1.Groups.Add(navBarGroupGlyphs);
				navBarControl1.Groups.Add(navBarGroupQuints);

				//Default only Teir 3 is visible
				foreach (NavBarItem navItem in navBarControl1.Items)
				{
					if (navItem.Tag.ToString() != "3")
					{
						navItem.Visible = false;
					}
				}
				success = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			navBarControl1.EndUpdate();

			return success;
		}

		private void navBarItemNew_LinkClicked(object sender, NavBarLinkEventArgs e)
		{
			try
			{
				string group = e.Link.Group.Caption;
				string runeID = e.Link.ItemName;
				RuneStatic thisRune = getRunesFromServer.runes.Runes.FirstOrDefault(o => o.Value.Id.ToString() == runeID).Value;
				
				//Black (Quints) utilize the Full image rather than the sprite
				string file;
				Image imageRune;
				if (thisRune.Metadata.Type == "black")
				{
					file = string.Format(@"{0}\Data\Runes\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getRunesFromServer.version, thisRune.Image.Full);
					imageRune = Image.FromFile(file);
					
				}
				else
				{
					file = string.Format(@"{0}\Data\Runes\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getRunesFromServer.version, thisRune.Image.Sprite);
					imageRune = Image.FromFile(file);
					imageRune = CommonMethods.cropImage(imageRune, new Rectangle(thisRune.Image.X, thisRune.Image.Y, thisRune.Image.Width, thisRune.Image.Height));
				}

				

				if (thisRune.Metadata.Type == "red")
				{
					for (int i = 1; i <= 9; i++)
					{
						PictureBox p = picBoxRuneBackground.Controls.Find("picBoxRuneSlot" + i, true)[0] as PictureBox;
						if (p.Image == null)
						{
							p.Image = imageRune;
							p.Tag = thisRune;
							//add to statistics
							runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, true);
							i = 100;
						}
					}
				}
				else if (thisRune.Metadata.Type == "yellow")
				{
					for (int i = 10; i <= 18; i++)
					{
						PictureBox p = picBoxRuneBackground.Controls.Find("picBoxRuneSlot" + i, true)[0] as PictureBox;
						if (p.Image == null)
						{
							p.Image = imageRune;
							p.Tag = thisRune;
							//add to statistics
							runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, true);
							i = 100;
						}
					}
				}
				else if (thisRune.Metadata.Type == "blue")
				{
					for (int i = 19; i <= 27; i++)
					{
						PictureBox p = picBoxRuneBackground.Controls.Find("picBoxRuneSlot" + i, true)[0] as PictureBox;
						if (p.Image == null)
						{
							p.Image = imageRune;
							p.Tag = thisRune;
							//add to statistics
							runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, true);
							i = 100;
						}
					}
				}
				else if (thisRune.Metadata.Type == "black")
				{
					for (int i = 28; i <= 30; i++)
					{
						PictureBox p = picBoxRuneBackground.Controls.Find("picBoxRuneSlot" + i, true)[0] as PictureBox;
						if (p.Image == null)
						{
							p.Image = imageRune;
							p.Tag = thisRune;
							//add to statistics
							runeStatistics.AddOrRemoveStatistics(((RuneStatic)p.Tag).Stats, true);
							i = 100;
						}
					}
				}
				

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

				
		}
	}
}
