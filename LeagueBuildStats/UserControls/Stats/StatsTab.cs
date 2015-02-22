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
using RiotSharp.StaticDataEndpoint;
using LeagueBuildStats.UserControls.Masteries;
using LeagueBuildStats.Classes;

namespace LeagueBuildStats.UserControls
{
	public partial class StatsTab : UserControl
	{
		private Form1 form1;

		private ContextMenuStrip contextMenuStripLevel = new ContextMenuStrip();

		public string lblMasteryOff { set { lblMasteryOffence.Text = value; } }
		public string lblMasteryDef { set { lblMasteryDefence.Text = value; } }
		public string lblMasteryUtil { set { lblMasteryUtility.Text = value; } }

		public StatsTab()
		{
			InitializeComponent();
		}

		public StatsTab(Form1 form)
		{
			this.form1 = form;
			InitializeComponent();

			PrepareLevelMenu();

			
		}

		private EventHandler onClick;
		private Image nullImage = null;

		private void PrepareLevelMenu()
		{
			contextMenuStripLevel.Items.Add("18", null, (s, e) => MenuLevel_MouseClick("18"));
			contextMenuStripLevel.Items.Add("17", null, (s, e) => MenuLevel_MouseClick("17"));
			contextMenuStripLevel.Items.Add("16", null, (s, e) => MenuLevel_MouseClick("16"));
			contextMenuStripLevel.Items.Add("15", null, (s, e) => MenuLevel_MouseClick("15"));
			contextMenuStripLevel.Items.Add("14", null, (s, e) => MenuLevel_MouseClick("14"));
			contextMenuStripLevel.Items.Add("13", null, (s, e) => MenuLevel_MouseClick("13"));
			contextMenuStripLevel.Items.Add("12", null, (s, e) => MenuLevel_MouseClick("12"));
			contextMenuStripLevel.Items.Add("11", null, (s, e) => MenuLevel_MouseClick("11"));
			contextMenuStripLevel.Items.Add("10", null, (s, e) => MenuLevel_MouseClick("10"));
			contextMenuStripLevel.Items.Add("9", null, (s, e) => MenuLevel_MouseClick("9"));
			contextMenuStripLevel.Items.Add("8", null, (s, e) => MenuLevel_MouseClick("8"));
			contextMenuStripLevel.Items.Add("7", null, (s, e) => MenuLevel_MouseClick("7"));
			contextMenuStripLevel.Items.Add("6", null, (s, e) => MenuLevel_MouseClick("6"));
			contextMenuStripLevel.Items.Add("5", null, (s, e) => MenuLevel_MouseClick("5"));
			contextMenuStripLevel.Items.Add("4", null, (s, e) => MenuLevel_MouseClick("4"));
			contextMenuStripLevel.Items.Add("3", null, (s, e) => MenuLevel_MouseClick("3"));
			contextMenuStripLevel.Items.Add("2", null, (s, e) => MenuLevel_MouseClick("2"));
			contextMenuStripLevel.Items.Add("1", null, (s, e) => MenuLevel_MouseClick("1"));


			btnChampLevel.MouseEnter+=btnChampLevel_MouseEnter;
			btnChampLevel.MouseLeave += btnChampLevel_MouseLeave;
			btnChampLevel.MouseClick += btnChampLevel_MouseClick;

			btnChampLevel.UseVisualStyleBackColor = false;
			btnChampLevel.BackColor = Color.FromArgb(45, 45, 48);
		}

		public void InitializeMasteryIamge()
		{
			string file = string.Format(@"{0}\Data\Masteries\Images\{1}\mastersprite.png", PublicStaticVariables.thisAppDataDir, form1.getAllVersionAvailable.realm.V);
			Image masterSprite = Image.FromFile(file);

			picBoxMasteryImage.Image = CommonMethods.cropImage(masterSprite, new Rectangle(123, 75, 155, 44));
		}

		void btnChampLevel_MouseLeave(object sender, EventArgs e)
		{
			btnChampLevel.UseVisualStyleBackColor = false;
			btnChampLevel.BackColor = Color.FromArgb(45, 45, 48);
		}

		private void btnChampLevel_MouseEnter(object sender, EventArgs e)
		{
			btnChampLevel.UseVisualStyleBackColor = false;
			btnChampLevel.BackColor = Color.FromArgb(54, 54, 57);
		}

		void btnChampLevel_MouseClick(object sender, MouseEventArgs e)
		{
			Button btnSender = (Button)sender;
			Point ptLowerLeft = new Point(0, btnSender.Height);
			ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
			contextMenuStripLevel.Show(ptLowerLeft);
		}

		private void MenuLevel_MouseClick(string p)
		{
			btnChampLevel.Text = "Level: " + p;
			btnChampLevel.Tag = p;
		}


		public void UpdateStatsTab()
		{
			//todo: implement level and map selection
			int level = Convert.ToInt16((string)btnChampLevel.Tag);
			try
			{
				//Get stats from items
				StatsStatic statsStatic = new StatsStatic();
				statsStatic = GetItemsStats(statsStatic);
				statsStatic = GetRunesStats(statsStatic);
				statsStatic = GetMasteryStats(statsStatic);

				ChampionStatic champ1 = new ChampionStatic();
				ChampionStatsStatic sStatic = new ChampionStatsStatic();
				if (form1.mainTopBar.champion1.Value != null)
				{
					champ1 = form1.mainTopBar.champion1.Value;
				}
				else
				{
					champ1.Stats = sStatic;
				}

				//Offence Variables//////////////////////////////////////////////////
				double dAttackDamage = champ1.Stats.AttackDamage + ((level - 1) * champ1.Stats.AttackDamagePerLevel) + statsStatic.FlatPhysicalDamageMod;
				double dAttackSpeedBase = 0.625 / (1 + champ1.Stats.AttackSpeedOffset) + ((level - 1) * champ1.Stats.AttackSpeedPerLevel / 100);
				double dAttackSpeed = dAttackSpeedBase * (1 + (statsStatic.PercentAttackSpeedMod));
				if (dAttackSpeed >= 2.5) { dAttackSpeed = 2.5; }
				double dArmorPen1 = statsStatic.RFlatArmorPenetrationMod + (level - 1) * statsStatic.RFlatArmorPenetrationModPerLevel;
				double dArmorPen2 = statsStatic.RPercentArmorPenetrationMod + (level - 1) * statsStatic.RPercentArmorPenetrationModPerLevel;
				double dCritChance = champ1.Stats.Crit + (level - 1) * champ1.Stats.CritPerLevel + statsStatic.FlatCritChanceMod + (level - 1) * statsStatic.RFlatCritChanceModPerLevel;
				double dCritDamage = 2 + statsStatic.FlatCritDamageMod + (level - 1) * statsStatic.RFlatCritDamageModPerLevel;
				double dDamageIfCrit = dAttackDamage * (dCritDamage);

				double dAbilityPower = (statsStatic.FlatMagicDamageMod + (level - 1) * statsStatic.RFlatMagicDamageModPerLevel);
				double dMagicPen1 = statsStatic.RFlatMagicPenetrationMod + (level - 1) * statsStatic.RFlatMagicPenetrationModPerLevel;
				double dMagicPen2 = statsStatic.RPercentMagicPenetrationMod + (level - 1) * statsStatic.RPercentMagicPenetrationModPerLevel;

				double dDamageBonus1 = statsStatic.FlatMagicDamageOnHit + statsStatic.FlatMagicDamageFromPercentAbility * dAbilityPower;
				double dDamageBonus2 = statsStatic.PercentIncreasedDamageMod;
				if (champ1.Stats.AttackRange < 300)
				{
					dDamageBonus2 += statsStatic.PercentIncreasedDamageModMelee;
				}
				else
				{
					dDamageBonus2 += statsStatic.PercentIncreasedDamageModRanged;
				}

				double dAverageAttackDamageWithCrit = (dAttackDamage * ((100 - dCritChance) / 100)) + ((dDamageIfCrit) * (dCritChance / 100));
				double dDPS = (dAverageAttackDamageWithCrit + dDamageBonus1) * dAttackSpeed;


				Color HighlightColor = Color.Aqua;



				//Defence Variables//////////////////////////////////////////////////
				double dHealth = champ1.Stats.Hp + (level - 1) * champ1.Stats.HpPerLevel + statsStatic.FlatHPPoolMod + (level - 1) * statsStatic.RFlatHPModPerLevel;
				double dHealthRegenBase = (champ1.Stats.HpRegen + (level - 1) * champ1.Stats.HpRegenPerLevel);
				double dHealthRegen1 = dHealthRegenBase + dHealthRegenBase * (statsStatic.PercentHPRegenMod) + (statsStatic.FlatHPRegenMod) + (level - 1) * (statsStatic.RFlatHPRegenModPerLevel);
				double dHealthRegen2 = dHealthRegen1 == 0.0 ? 0 : (dHealthRegen1 / dHealthRegenBase);
				double dArmor = champ1.Stats.Armor + (level - 1) * champ1.Stats.ArmorPerLevel + statsStatic.FlatArmorMod + (level - 1) * statsStatic.RFlatArmorModPerLevel;
				double dMagicResist = statsStatic.FlatSpellBlockMod + (level - 1) * statsStatic.RFlatSpellBlockModPerLevel;
				double dTenacity = statsStatic.PercentTenacityMod;//(1 - (1 - 0.35)*(1 - 0.25)*(1 - 0.15)) * 100; //todo there is no tenacity stat

				double dDamabeReduction = 0.0 + statsStatic.FlatBlockMod; //todo there is no damamge reduction stat
				double dSlowResist = statsStatic.PercentSlowReistance;

				double dEffPhysicalHealth = dHealth * (100 + dArmor) / (100 * (1 - (dDamabeReduction / 100)));
				double dEffMagicalHealth = dHealth * (100 + dMagicResist) / (100 * (1 - (dDamabeReduction / 100)));
				double dEffHealth = (dEffPhysicalHealth + dEffMagicalHealth) / 2;




				//Utility Variables//////////////////////////////////////////////////
				double dCooldownReduction = statsStatic.RPercentCooldownMod + (level - 1) * statsStatic.RPercentCooldownModPerLevel;
				if (dCooldownReduction >= 0.40) { dCooldownReduction = 0.40; }
				double dMovementBase = champ1.Stats.MoveSpeed;

				double dMovementSpeed = (dMovementBase + statsStatic.FlatMovementSpeedMod + (level - 1) * statsStatic.RFlatMovementSpeedModPerLevel) * (1 + statsStatic.PercentMovementSpeedMod);
				//(Base Movement Speed + Flat Movement Bonuses) × (1 + Additional Percentage Movement Bonuses) × (1 + First Multiplicative Percentage Movement Bonus) × .... 
				//× (1 + Last Multiplicative Percentage Movement Bonus) × (1 - Slow Ratio × Slow Resist Ratio)

				double dLifeSteal = statsStatic.PercentLifeStealMod;
				double dSpellVamp = statsStatic.PercentSpellVampMod;

				double dAttackRange = champ1.Stats.AttackRange;
				double dGoldPer10 = 16.0 + statsStatic.RFlatGoldPer10Mod;
				double dExperienceGain = (1 + statsStatic.PercentEXPBonus);
				double dRevivalSpeed = (1 + statsStatic.RPercentTimeDeadMod);

				double dManaenergy = champ1.Stats.Mp + (level - 1) * champ1.Stats.MpPerLevel + statsStatic.FlatMPPoolMod;
				
				double dBaseManaRegen = (champ1.Stats.MpRegen + (level - 1) * champ1.Stats.MpRegenPerLevel);
				double dManaEnergyRegen1 = 0.0;
				double dManaEnergyRegen2 = 0;

				if (champ1.Partype == ParTypeStatic.Mana)
				{
					lblTextManaEnergy.Text = "Mana:";
					lblTextManaEnergyRegen.Text = "Mana Regen per 5:";
					dManaenergy += statsStatic.FlatEnergyPoolMod;
					dManaEnergyRegen1 = dBaseManaRegen + dBaseManaRegen * (statsStatic.PercentMPRegenMod) + (statsStatic.FlatMPRegenMod) + (level - 1) * (statsStatic.RFlatMPRegenModPerLevel);
					dManaEnergyRegen2 = dManaEnergyRegen1 == 0.0 ? 0 : ((dManaEnergyRegen1 / dBaseManaRegen));
				}
				else if (champ1.Partype == ParTypeStatic.Energy)
				{
					lblTextManaEnergy.Text = "Energy:";
					lblTextManaEnergyRegen.Text = "Energy Regen per 5:";
					dManaenergy += statsStatic.FlatEnergyPoolMod;
					dManaEnergyRegen1 = dBaseManaRegen + (statsStatic.FlatEnergyRegenMod) + (level - 1) * (statsStatic.RFlatEnergyRegenModPerLevel);
					dManaEnergyRegen2 = dManaEnergyRegen1 == 0.0 ? 0 : ((dManaEnergyRegen1 / dBaseManaRegen));
				}
				else
				{
					lblTextManaEnergy.Text = "Mana:";
					lblTextManaEnergyRegen.Text = "Mana Regen per 5:";
					dManaenergy = 0.0;
					dManaEnergyRegen1 = 0.0;
					dManaEnergyRegen2 = 0.0;
				}
				dAttackDamage += statsStatic.PercentManaAsBonusAttack * dManaenergy;
				dAbilityPower += statsStatic.PercentManaAsBonusAbility * dManaenergy;



				////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


				//Offence Text Boxes//
				lblCtrlAttackDamage.Text = Math.Round(dAttackDamage, MidpointRounding.AwayFromZero).ToString();
				//if (dAttackDamage != champ1.Stats.AttackDamage + ((level - 1) * champ1.Stats.AttackDamagePerLevel)) { lblCtrlAttackDamage.ForeColor = HighlightColor; } else { lblCtrlAttackDamage.ForeColor = Color.White; }
				lblCtrlAttackSpeed.Text = Math.Round(dAttackSpeed, 2, MidpointRounding.AwayFromZero).ToString();
				lblCtrlArmorPen1.Text = Math.Round(dArmorPen1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlArmorPen2.Text = Math.Round(dArmorPen2 * 100, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlCritChance.Text = Math.Round(dCritChance * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlCritDmg.Text = Math.Round(dCritDamage * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlDamageIfCrit.Text = Math.Round(dDamageIfCrit, MidpointRounding.AwayFromZero).ToString();

				lblCtrlAbilityPower.Text = Math.Round(dAbilityPower, MidpointRounding.AwayFromZero).ToString();
				lblCtrlMagicPen1.Text = Math.Round(dMagicPen1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlMagicPen2.Text = Math.Round(dMagicPen2 * 100, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlDmgBonus1.Text = Math.Round(dDamageBonus1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlDmgBonus2.Text = Math.Round(dDamageBonus2 * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlAveAttackDPS.Text = Math.Round(dDPS, MidpointRounding.AwayFromZero).ToString();


				


				//Defence Text Boxes//
				lblCtrlHealth.Text = Math.Round(dHealth, MidpointRounding.AwayFromZero).ToString();
				lblCtrlHealthRegen1.Text = Math.Round(dHealthRegen1, 1, MidpointRounding.AwayFromZero).ToString() + "";
				lblCtrlHealthRegen2.Text = Math.Round(dHealthRegen2 * 100, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlArmor.Text = Math.Round(dArmor, MidpointRounding.AwayFromZero).ToString();
				lblCtrlMagicResist.Text = Math.Round(dMagicResist, MidpointRounding.AwayFromZero).ToString();
				lblCtrlTenacity.Text = Math.Round(dTenacity * 100, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlDmgReduction.Text = Math.Round(dDamabeReduction, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlSlowResist.Text = Math.Round(dSlowResist * 100, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlEffPhysHealth.Text = Math.Round(dEffPhysicalHealth, MidpointRounding.AwayFromZero).ToString();
				lblCtrlEffMagHealth.Text = Math.Round(dEffMagicalHealth, MidpointRounding.AwayFromZero).ToString();
				lblCtrlEffHealth.Text = Math.Round(dEffHealth, MidpointRounding.AwayFromZero).ToString();


				

				//Utility Text Boxes//
				lblCtrlCooldownReduction.Text = Math.Round(dCooldownReduction * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlMovement.Text = Math.Round(dMovementSpeed, MidpointRounding.AwayFromZero).ToString();
				lblCtrlLifeSteal.Text = Math.Round(dLifeSteal * 100, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlSpellVamp.Text = Math.Round(dSpellVamp * 100, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlAutoAttackRange.Text = Math.Round(dAttackRange, MidpointRounding.AwayFromZero).ToString();
				lblCtrlGoldPer10.Text = Math.Round(dGoldPer10, 2, MidpointRounding.AwayFromZero).ToString();
				lblCtrlExperienceGain.Text = Math.Round(dExperienceGain * 100, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlRevivalSpeed.Text = Math.Round(dRevivalSpeed * 100, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlManaEnergy.Text = Math.Round(dManaenergy, MidpointRounding.AwayFromZero).ToString();
				lblCtrlManaEnergyRegen1.Text = Math.Round(dManaEnergyRegen1, 1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlManaEnergyRegen2.Text = Math.Round(dManaEnergyRegen2 * 100, MidpointRounding.AwayFromZero).ToString() + "%";

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private StatsStatic GetMasteryStats(StatsStatic statsStatic)
		{
			foreach (Control c in form1.masteriesTab.picBoxMasteryMain.Controls)
			{
				if (c.GetType() == typeof(MasteryControl))
				{
					try
					{
						MasteryControl p = c as MasteryControl;
						if (p.currentRank != 0)
						{
							try
							{
								StatsStatic statsPreped = (StatsStatic)p.masteryData.StatsList[p.currentRank - 1];

								Type myType = typeof(StatsStatic);
								System.Reflection.PropertyInfo[] properties = myType.GetProperties();

								foreach (System.Reflection.PropertyInfo property in properties)
								{
									Double statValue = (Double)statsPreped.GetType().GetProperty(property.Name).GetValue(statsPreped);


									if (statValue != 0.0)
									{
										Double newValue = (Double)statsStatic.GetType().GetProperty(property.Name).GetValue(statsStatic);
										if (property.Name == "PercentSlowReistance"
											|| property.Name == "PercentTenacityMod"
											|| property.Name == "RPercentArmorPenetrationMod"
											|| property.Name == "RFlatMagicPenetrationMod")
										{
											newValue = (1 - (1 - statValue) * (1 - newValue));
										}
										else
										{
											newValue += statValue;
										}

										statsStatic.GetType().GetProperty(property.Name).SetValue(statsStatic, newValue);
									}
								}
							}
							catch (Exception ex)
							{
								MessageBox.Show(ex.ToString());
							}

						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
				}
			}
			return statsStatic;
		}

		

		private StatsStatic GetItemsStats(StatsStatic statsStatic)
		{
			List<string> uniqueStatsList = new List<string>();

			foreach (Panel p in form1.mainTopBar.itemPanels)
			{
				if (p.Tag != null)
				{
					try
					{
						CreateItemDiv itemPreped = (CreateItemDiv)p.Tag;

						Type myType = typeof(StatsStatic);
						System.Reflection.PropertyInfo[] properties = myType.GetProperties();

						List<string> tempUniqueStatsList = new List<string>();

						foreach (System.Reflection.PropertyInfo property in properties)
						{
							Double statValue = (Double)itemPreped.aItem.Value.Stats.GetType().GetProperty(property.Name).GetValue(itemPreped.aItem.Value.Stats);
							Double uniqueValue = 0.0;
							foreach (KeyValuePair<string, StatsStatic> uniqueStats in itemPreped.aItem.Value.UniqueStats)
							{
								if (!uniqueStatsList.Contains(uniqueStats.Key))
								{
									uniqueValue += (Double)uniqueStats.Value.GetType().GetProperty(property.Name).GetValue(uniqueStats.Value);
									if (uniqueValue != 0.0)
									{
										tempUniqueStatsList.Add(uniqueStats.Key);
									}
								}
							}


							if (statValue != 0.0 || uniqueValue != 0.0)
							{
								Double newValue = (Double)statsStatic.GetType().GetProperty(property.Name).GetValue(statsStatic);
								if (property.Name == "PercentSlowReistance" 
									|| property.Name == "PercentTenacityMod" 
									|| property.Name == "RPercentArmorPenetrationMod"
									|| property.Name == "RFlatMagicPenetrationMod")
								{
									newValue = (1 - (1 - statValue) * (1 - newValue));
									newValue = (1 - (1 - uniqueValue) * (1 - newValue));
								}
								else
								{
									newValue += statValue + uniqueValue;
								}

								statsStatic.GetType().GetProperty(property.Name).SetValue(statsStatic, newValue);
							}
						}
						foreach (string s in tempUniqueStatsList)
						{
							uniqueStatsList.Add(s);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}

				}
			}
			return statsStatic;
		}

		private StatsStatic GetRunesStats(StatsStatic statsStatic)
		{
			foreach (Control c in form1.runesTab.RuneBackgroundContainer.Controls)
			{
				if (c.Name.Contains("picBoxRuneSlot"))
				{
					try
					{
						PictureBox p = c as PictureBox;
						if (p.Tag != null)
						{
							try
							{
								RuneStatic itemPreped = (RuneStatic)p.Tag;

								Type myType = typeof(StatsStatic);
								System.Reflection.PropertyInfo[] properties = myType.GetProperties();

								foreach (System.Reflection.PropertyInfo property in properties)
								{
									Double statValue = (Double)itemPreped.Stats.GetType().GetProperty(property.Name).GetValue(itemPreped.Stats);
									

									if (statValue != 0.0)
									{
										Double newValue = (Double)statsStatic.GetType().GetProperty(property.Name).GetValue(statsStatic);
										if (property.Name == "PercentSlowReistance"
											|| property.Name == "PercentTenacityMod"
											|| property.Name == "RPercentArmorPenetrationMod"
											|| property.Name == "RFlatMagicPenetrationMod")
										{
											newValue = (1 - (1 - statValue) * (1 - newValue));
										}
										else
										{
											newValue += statValue;
										}

										statsStatic.GetType().GetProperty(property.Name).SetValue(statsStatic, newValue);
									}
								}
							}
							catch (Exception ex)
							{
								MessageBox.Show(ex.ToString());
							}

						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
				}
			}

			return statsStatic;
		}

	}
}
