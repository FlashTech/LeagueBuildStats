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
using LeagueBuildStats.Classes.Items;
using LeagueBuildStats.Classes;
using LeagueBuildStats.Classes.Champions;
using RiotSharp.StaticDataEndpoint;
using LeagueBuildStats.UserControls;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Infragistics.Win.UltraWinToolTip;
using Infragistics.Win;
using Infragistics.Win.FormattedLinkLabel;
using LeagueBuildStats.Classes.General_Classes;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using LeagueBuildStats.Classes.DragUtils;

namespace LeagueBuildStats.Forms
{
	public partial class ChampionsTab : UserControl
	{
		public GetChampionsFromServer getChampionsFromServer = new GetChampionsFromServer();

		private LeagueBuildStatsForm form;

		private UltraToolTipInfo tipInfoPassive = new UltraToolTipInfo();
		private UltraToolTipInfo tipInfoSpell0 = new UltraToolTipInfo();
		private UltraToolTipInfo tipInfoSpell1 = new UltraToolTipInfo();
		private UltraToolTipInfo tipInfoSpell2 = new UltraToolTipInfo();
		private UltraToolTipInfo tipInfoSpell3 = new UltraToolTipInfo();

		private UltraToolTipInfo tipStatsAttack = new UltraToolTipInfo();
		private UltraToolTipInfo tipStatsDefense = new UltraToolTipInfo();
		private UltraToolTipInfo tipStatsMagic = new UltraToolTipInfo();
		private UltraToolTipInfo tipStatsDifficulty = new UltraToolTipInfo();


		public Drag dragger = new Drag();

		private CreateChampionSortMenu createChampionSortMenu;

		private int statBarMaxWidth = 0;

		public ChampionsTab(LeagueBuildStatsForm form)
		{
			this.form = form;
			InitializeComponent();

			createChampionSortMenu = new CreateChampionSortMenu(pnlChampionSort, flowLayoutPanelItems2);

			//Tooltips prep
			ultraToolTipManager1.AutoPopDelay = 0;
			ultraToolTipManager1.InitialDelay = 50;
			ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Office2007;
			ultraToolTipManager1.Appearance.BackColor = Color.Black;
			ultraToolTipManager1.Appearance.BackColor2 = Color.Black;
			ultraToolTipManager1.Appearance.BackColorAlpha = Alpha.Transparent;

			tipInfoPassive.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoSpell0.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoSpell1.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoSpell2.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipInfoSpell3.ToolTipTextStyle = ToolTipTextStyle.Formatted;

			tipStatsAttack.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipStatsDefense.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipStatsMagic.ToolTipTextStyle = ToolTipTextStyle.Formatted;
			tipStatsDifficulty.ToolTipTextStyle = ToolTipTextStyle.Formatted;

			ultraToolTipManager1.SetUltraToolTip(picBoxInfoPassive, tipInfoPassive);
			ultraToolTipManager1.SetUltraToolTip(picBoxInfoAbil0, tipInfoSpell0);
			ultraToolTipManager1.SetUltraToolTip(picBoxInfoAbil1, tipInfoSpell1);
			ultraToolTipManager1.SetUltraToolTip(picBoxInfoAbil2, tipInfoSpell2);
			ultraToolTipManager1.SetUltraToolTip(picBoxInfoAbil3, tipInfoSpell3);

			ultraToolTipManager1.SetUltraToolTip(lblStatAttack, tipStatsAttack);
			ultraToolTipManager1.SetUltraToolTip(lblStatDefense, tipStatsDefense);
			ultraToolTipManager1.SetUltraToolTip(lblStatMagic, tipStatsMagic);
			ultraToolTipManager1.SetUltraToolTip(lblStatDifficulty, tipStatsDifficulty);

			//Prep Champion Info Section
			pnlCtrlChampionInfo.Location = new Point(pnlCtrlChampionInfo.Location.X, createChampionSortMenu.yPos);
			lblInfoName.Text = "";
			lblInfoDesc.Text = "";
			lblInfoPrimary.Text = "";
			lblInfoSecondary.Text = "";


			//Initialize Info Stats Bars
			statBarMaxWidth = lblStatAttack.Width;
			ResizeStatBars(1, 1, 1, 1);
		}

		private void ResizeStatBars(int attack, int defense, int magic, int difficulty)
		{
			lblStatAttack.Width = statBarMaxWidth / 10 * attack;
			lblStatDefense.Width = statBarMaxWidth / 10 * defense;
			lblStatMagic.Width = statBarMaxWidth / 10 * magic;
			lblStatDifficulty.Width = statBarMaxWidth / 10 * difficulty;

			string fontStyle = "<font style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias;'>";

			tipStatsAttack.ToolTipTextFormatted = fontStyle + "Attack: " + attack.ToString() + "</font>";
			tipStatsDefense.ToolTipTextFormatted = fontStyle + "Defense: " + defense.ToString() + "</font>";
			tipStatsMagic.ToolTipTextFormatted = fontStyle + "Magic: " + magic.ToString() + "</font>";
			tipStatsDifficulty.ToolTipTextFormatted = fontStyle + "Difficulty: " + difficulty.ToString() + "</font>";
		}


		internal void ClearChampionInfoSection()
		{
			lblInfoName.Text = "";
			lblInfoDesc.Text = "";
			lblInfoPrimary.Text = "";
			lblInfoSecondary.Text = "";
			picboxInfoChamp.Image = null;
			picBoxInfoPassive.Image = null;
			picBoxInfoAbil0.Image = null;
			picBoxInfoAbil1.Image = null;
			picBoxInfoAbil2.Image = null;
			picBoxInfoAbil3.Image = null;
			tipInfoPassive.ToolTipTextFormatted = "";
			tipInfoSpell0.ToolTipTextFormatted = "";
			tipInfoSpell1.ToolTipTextFormatted = "";
			tipInfoSpell2.ToolTipTextFormatted = "";
			tipInfoSpell3.ToolTipTextFormatted = "";
		}

		internal void ClearSortingSelections()
		{
			createChampionSortMenu.ClearSelections();
		}

		public void championControl_MouseClick(ChampionStatic thischampCtrl)
		{
			//Update Labels
			lblInfoName.Text = thischampCtrl.Name;
			lblInfoDesc.Text = thischampCtrl.Title;
			lblInfoPrimary.Text = thischampCtrl.Tags.Count > 0 ? thischampCtrl.Tags[0].ToString() : "";
			lblInfoSecondary.Text = thischampCtrl.Tags.Count > 1 ? "Secondary: " + thischampCtrl.Tags[1].ToString() : "";

			//Champion Image
			string file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getChampionsFromServer.version, thischampCtrl.Image.Sprite);
			string dir = string.Format(@"{0}\Data\Champions\Images", PublicStaticVariables.thisAppDataDir);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			Image tempImage = Image.FromFile(file);
			picboxInfoChamp.Image = CommonMethods.cropImage(tempImage, new Rectangle(thischampCtrl.Image.X, thischampCtrl.Image.Y, thischampCtrl.Image.Width, thischampCtrl.Image.Height));

			//Passive Image
			file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getChampionsFromServer.version, thischampCtrl.Passive.Image.Sprite);
			tempImage = Image.FromFile(file);
			picBoxInfoPassive.Image = CommonMethods.cropImage(tempImage, new Rectangle(thischampCtrl.Passive.Image.X, thischampCtrl.Passive.Image.Y, thischampCtrl.Passive.Image.Width, thischampCtrl.Passive.Image.Height));
			GenerateTooltip(thischampCtrl.Passive, tipInfoPassive);

			//Ability0 Image
			file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getChampionsFromServer.version, thischampCtrl.Spells[0].Image.Sprite);
			tempImage = Image.FromFile(file);
			picBoxInfoAbil0.Image = CommonMethods.cropImage(tempImage, new Rectangle(thischampCtrl.Spells[0].Image.X, thischampCtrl.Spells[0].Image.Y, thischampCtrl.Spells[0].Image.Width, thischampCtrl.Spells[0].Image.Height));
			GenerateTooltip(thischampCtrl.Spells[0], tipInfoSpell0);

			//Ability1 Image
			file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getChampionsFromServer.version, thischampCtrl.Spells[1].Image.Sprite);
			tempImage = Image.FromFile(file);
			picBoxInfoAbil1.Image = CommonMethods.cropImage(tempImage, new Rectangle(thischampCtrl.Spells[1].Image.X, thischampCtrl.Spells[1].Image.Y, thischampCtrl.Spells[1].Image.Width, thischampCtrl.Spells[1].Image.Height));
			GenerateTooltip(thischampCtrl.Spells[1], tipInfoSpell1);

			//Ability2 Image
			file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getChampionsFromServer.version, thischampCtrl.Spells[2].Image.Sprite);
			tempImage = Image.FromFile(file);
			picBoxInfoAbil2.Image = CommonMethods.cropImage(tempImage, new Rectangle(thischampCtrl.Spells[2].Image.X, thischampCtrl.Spells[2].Image.Y, thischampCtrl.Spells[2].Image.Width, thischampCtrl.Spells[2].Image.Height));
			GenerateTooltip(thischampCtrl.Spells[2], tipInfoSpell2);

			//Ability3 Image
			file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, getChampionsFromServer.version, thischampCtrl.Spells[3].Image.Sprite);
			tempImage = Image.FromFile(file);
			picBoxInfoAbil3.Image = CommonMethods.cropImage(tempImage, new Rectangle(thischampCtrl.Spells[3].Image.X, thischampCtrl.Spells[3].Image.Y, thischampCtrl.Spells[3].Image.Width, thischampCtrl.Spells[3].Image.Height));
			GenerateTooltip(thischampCtrl.Spells[3], tipInfoSpell3);

			//Update Stat Bars
			ResizeStatBars(thischampCtrl.Info.Attack, thischampCtrl.Info.Defense, thischampCtrl.Info.Magic, thischampCtrl.Info.Difficulty);
		}

		

		private void GenerateTooltip(PassiveStatic passiveStatic, UltraToolTipInfo thisToolTipInfo)
		{

			try
			{
				string sDescPrep = passiveStatic.Description;

				//Fix color tags
				sDescPrep = sDescPrep.Replace("class=\"color", "style=\"color:#");


				//Artificial Word-wrap
				sDescPrep = StringExtensions.BrWrapper(sDescPrep);



				//Prep Image for tooltip
				string version = getChampionsFromServer.version;
				string file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, version, passiveStatic.Image.Sprite);
				string dir = string.Format(@"{0}\Data\Champions\Images", PublicStaticVariables.thisAppDataDir);

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				Image imageTemp = Image.FromFile(file);
				imageTemp = CommonMethods.cropImage(imageTemp, new Rectangle(passiveStatic.Image.X, passiveStatic.Image.Y, passiveStatic.Image.Width, passiveStatic.Image.Height));
				string image = FormattedLinkEditor.EncodeImage(imageTemp);


				//Compile Description
				string tipDesc = string.Format(@"
				<div style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; border-style:solid; border-width:2px; width:400px'>
				  <img style='padding: 4 4 0 4;' data='{2}' width='48' height='48'/>
				  <div>
					<br/>
					<b>{0}</b>
				  </div>
				<hr style='border-style:solid; margin: 0 0 0 0;'/>
				{1}
				</div>", passiveStatic.Name,
						   sDescPrep, 
						   image);

				tipDesc = tipDesc.Replace("<br>", "<br/>");
				tipDesc = tipDesc.Replace("\"", "'");
				//tipDesc = tipDesc.Replace("\r", "");
				//tipDesc = tipDesc.Replace("\n", "");
				//tipDesc = tipDesc.Replace("\t", "");



				

				//TODO: add \r for troubleshooting
				tipDesc = tipDesc.Replace("<br/>", "<br/>\r\n");


				//These tags needed to be closed for UltraToolTipInfo to accept it as well-formed html because by default HtmlAgilityPack removes the /> at the end
				HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
				HtmlNode.ElementsFlags["hr"] = HtmlElementFlag.Closed;
				HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
				//Fixes Riots malformed html
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(tipDesc);
				MemoryStream memoryStream = new MemoryStream();
				doc.Save(memoryStream);
				memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(memoryStream);
				tipDesc = streamReader.ReadToEnd();


				thisToolTipInfo.ToolTipTextFormatted = tipDesc;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void GenerateTooltip(ChampionSpellStatic champSpell, UltraToolTipInfo thisToolTipInfo)
		{
			try
			{
				//Prepare Desc//
				string sDescPrep = champSpell.Tooltip;


				//Replace Var.Ceoff markers in tooltip
				if (champSpell.Vars != null)
				{
					foreach (SpellVarsStatic spellVars in champSpell.Vars)
					{
						string sTarget = "{{ " + spellVars.Key + " }}";
						string sReplacement = "";
						string ofAdAp = "";
						string dynText = spellVars.Dyn != null ? spellVars.Dyn : "";
						if (spellVars.Coeff != null)
						{
							var varsCoeff = (Newtonsoft.Json.Linq.JArray)spellVars.Coeff;
							int index = 0;
							int icount = varsCoeff.Count;
							bool isPercent = false;
							foreach (string sVar in varsCoeff)
							{
								string sVarNew = sVar;
								try 
								{
									double dVar = Convert.ToDouble(sVarNew);
									if (dVar < 3)
									{
										sVarNew = (dVar * 100).ToString();
										isPercent = true;
									}
								}
								catch
								{
									//do nothing
								}

								sReplacement += sVarNew;
								if ((index + 1) != icount)
								{
									sReplacement += "/";
								}
								index++;
							}
							if (isPercent)
							{
								sReplacement += "%";
							}
						}

						if (spellVars.Link.Contains("bonusarmor"))
						{
							ofAdAp = " of bonus armor";
						}
						else if (spellVars.Link.Contains("bonusattackdamage"))
						{
							ofAdAp = " of bonus attack damage";
						}
						else if (spellVars.Link.Contains("bonushealth"))
						{
							ofAdAp = " of bonus health";
						}
						else if (spellVars.Link.Contains("maxhealth")) //This one was made up by for ChampionDataCorrections
						{
							ofAdAp = " of max health";
						}
						else if (spellVars.Link.Contains("bonusspellblock"))
						{
							ofAdAp = " of bonus magic resist";
						}

						else if (spellVars.Link.Contains("armor"))
						{
							ofAdAp = " of total armor";
						}
						else if (spellVars.Link.Contains("attackdamage"))
						{
							ofAdAp = " of attack damage";
						}
						else if (spellVars.Link.Contains("health"))
						{
							ofAdAp = " of health";
						}
						else if (spellVars.Link.Contains("spelldamage"))
						{
							ofAdAp = " of ability power";
						}
						else if (spellVars.Link.Contains("mana"))
						{
							ofAdAp = " of mana";
						}

						else if (spellVars.Link.Contains("@text"))
						{

						}
						else if (spellVars.Link.Contains("@cooldownchampion"))
						{

						}
						else if (spellVars.Link.Contains("@dynamic.abilitypower"))
						{
							ofAdAp = " of ability power";
						}
						else if (spellVars.Link.Contains("@dynamic.attackdamage") && champSpell.Name == "Savagery") //Rengar
						{
							sReplacement = "";
						}
						else if (spellVars.Link.Contains("@dynamic.attackdamage"))
						{
							ofAdAp = " per attack damage";
						}
						else if (spellVars.Link.Contains("@stacks"))
						{
							ofAdAp = " of stacks";
						}
						else if (spellVars.Link.Contains("@special.BraumWArmor"))
						{
							//TODO
						}
						else if (spellVars.Link.Contains("@special.BraumWMR"))
						{
							//TODO
						}
						else if (spellVars.Link.Contains("@special.dariusr3"))
						{
							//TODO
						}
						else if (spellVars.Link.Contains("@special.jaycew"))
						{
							//TODO
						}
						else if (spellVars.Link.Contains("@special.jaxrarmor"))
						{
							sReplacement = "bonus";
						}
						else if (spellVars.Link.Contains("@special.jaxrmr"))
						{
							sReplacement = "bonus";
						}
						else if (spellVars.Link.Contains("@special.nautilusq"))
						{
							//TODO
						}
						else if (spellVars.Link.Contains("@special.viw"))
						{
							dynText = "1% per ";
							ofAdAp = " bonus AD";
						}
						else if (spellVars.Link.Contains("@custom.MagicDamage")) //This one was made up by for ChampionDataCorrections
						{
							ofAdAp = "";
						}
						else if (spellVars.Link.Contains("@custom.percentOfAttack")) //This one was made up by for ChampionDataCorrections
						{
							ofAdAp = "% of attack damage";
						}
						else if (spellVars.Link.Contains("@custom.percentOfMana")) //This one was made up by for ChampionDataCorrections
						{
							ofAdAp = "% of mana";
						}
						else if (spellVars.Link.Contains("@custom.percentOfAP")) //This one was made up by for ChampionDataCorrections
						{
							ofAdAp = "% of AP";
						}
						else if (spellVars.Link.Contains("@custom.percent")) //This one was made up by for ChampionDataCorrections
						{
							ofAdAp = "%";
						}
						


						sDescPrep = sDescPrep.Replace(sTarget, dynText + sReplacement + ofAdAp);
					}
				}

				//Replace effect markers in tooltip
				if (champSpell.EffectBurns != null)
				{
					int i = 0;
					foreach (string sReplacement in champSpell.EffectBurns)
					{
						if (sReplacement != "")
						{
							string newReplace = sReplacement;
							string sTarget = "{{ e" + i + " }}";
							sDescPrep = sDescPrep.Replace(sTarget, newReplace);
						}
						i++;
					}
				}


				//Replace cost marker in tooltip
				if (sDescPrep.Contains("{{ cost }}"))
				{
					string sTarget = "{{ cost }}";
					sDescPrep = sDescPrep.Replace(sTarget, champSpell.CostBurn);
				}


				//replace tags in Resource
				string resourceCost = champSpell.Resource != null ? champSpell.Resource.Replace("{{ cost }}", champSpell.CostBurn) : "Passive";
				if (champSpell.EffectBurns != null)
				{
					int i = 0;
					foreach (string sReplacement in champSpell.EffectBurns)
					{
						if (sReplacement != "")
						{
							string newReplace = sReplacement;
							string sTarget = "{{ e" + i + " }}";
							resourceCost = resourceCost.Replace(sTarget, newReplace);
						}
						i++;
					}
				}





				//Fix color tags
				sDescPrep = sDescPrep.Replace("class=\"color", "style=\"color:#");


				//Artificial Word-wrap
				sDescPrep = StringExtensions.BrWrapper(sDescPrep);



				//Prep Image for tooltip
				string version = getChampionsFromServer.version;
				string file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, version, champSpell.Image.Sprite);
				string dir = string.Format(@"{0}\Data\Champions\Images", PublicStaticVariables.thisAppDataDir);

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				Image imageTemp = Image.FromFile(file);
				imageTemp = CommonMethods.cropImage(imageTemp, new Rectangle(champSpell.Image.X, champSpell.Image.Y, champSpell.Image.Width, champSpell.Image.Height));
				string image = FormattedLinkEditor.EncodeImage(imageTemp);


				//Compile Description
				string tipDesc = string.Format(@"
				<div style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; border-style:solid; border-width:2px; width:400px'>
				  <img style='padding: 4 4 0 4;' data='{5}' width='48' height='48'/>
				  <div>
					<br/>
					<b>{0}</b><br/>
					<span style='font-size:8pt'>
					  {1}<br/>
					  {2}<br/>
					  {3}
					</span>
				  </div>
				<hr style='border-style:solid; margin: 0 0 0 0;'/>
				{4}
				</div>", champSpell.Name,
					   "Cooldown: " + (champSpell.Resource != null ? champSpell.CooldownBurn : "Passive"),
						   "Cost: " + resourceCost,
						   "Range: " + champSpell.RangeBurn,
						   sDescPrep, 
						   image);

				tipDesc = tipDesc.Replace("<br>", "<br/>");
				tipDesc = tipDesc.Replace("\"", "'");
				//tipDesc = tipDesc.Replace("\r", "");
				//tipDesc = tipDesc.Replace("\n", "");
				//tipDesc = tipDesc.Replace("\t", "");



				

				//TODO: add \r for troubleshooting
				tipDesc = tipDesc.Replace("<br/>", "<br/>\r\n");


				//These tags needed to be closed for UltraToolTipInfo to accept it as well-formed html because by default HtmlAgilityPack removes the /> at the end
				HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
				HtmlNode.ElementsFlags["hr"] = HtmlElementFlag.Closed;
				HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
				//Fixes Riots malformed html
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(tipDesc);
				MemoryStream memoryStream = new MemoryStream();
				doc.Save(memoryStream);
				memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(memoryStream);
				tipDesc = streamReader.ReadToEnd();


				thisToolTipInfo.ToolTipTextFormatted = tipDesc;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public bool CollectChampionData(string inputVersion = null)
		{
			bool success; 
			if (inputVersion == null)
			{
				success = getChampionsFromServer.DownloadListOfChampions();
			}
			else
			{
				success = getChampionsFromServer.DownloadListOfChampions(inputVersion);
			}

			if (success) 
			{
				success = getChampionsFromServer.GetChampionImages();
			}
			return success;
		}

		public bool UpdateChampionControl()
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
				foreach (KeyValuePair<string, ChampionStatic> champ in getChampionsFromServer.championsOrder)
				{
					string version = getChampionsFromServer.version;
					string file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, version, champ.Value.Image.Sprite);
					string dir = string.Format(@"{0}\Data\Champions\Images", PublicStaticVariables.thisAppDataDir);

					if (!Directory.Exists(dir))
					{
						Directory.CreateDirectory(dir);
					}
					Image imageItem = Image.FromFile(file);

					//Creating this control is actually pretty quick
					ChampionControl championControl = new ChampionControl(form, champ);

					Image image = CommonMethods.cropImage(imageItem, new Rectangle(champ.Value.Image.X, champ.Value.Image.Y, champ.Value.Image.Width, champ.Value.Image.Height));
					championControl.ChampImage = image;

					championControl.Name = "ItemControl " + champ.Value.Name;
					championControl.ChampLabel = champ.Value.Name;


					//championControl.item = champ;
					ChampionTags champTags = new ChampionTags();

					

					List<string> temp = new List<string>();
					if (champ.Value.Partype == ParTypeStatic.Mana)
					{
						temp.Add("Mana");
					}
					else
					{
						temp.Add("Other");
					}

					if (champ.Value.Tags == null)
					{
						
						temp.Add("noTag");
						//championControl.Tag = temp;
					}
					else
					{
						foreach (TagStatic t in champ.Value.Tags)
						{
							temp.Add(t.ToString());
						}
						//championControl.Tag = temp;
						
					}

					champTags.Tags = temp;
					champTags.ChampionName = champ.Value.Name;
					championControl.Tag = champTags;

					flowLayoutPanelItems2.Controls.Add(championControl);
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



		
	}
}
