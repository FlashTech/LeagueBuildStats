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

namespace LeagueBuildStats.UserControls.Runes
{
	public partial class RuneStatistics : UserControl
	{
		public StatsStatic statsStatic = new StatsStatic();
		private RunesTab runesTab;

		private List<KeyValuePair<string, string>> runeNamePairs = new List<KeyValuePair<string, string>>()
			{
				//This list was generated from the function GetRunesFromServer.GenerateRuneDictionaryList()
				 new KeyValuePair<string, string>("FlatPhysicalDamageMod", "Attack Damage"), 
				 new KeyValuePair<string, string>("RFlatPhysicalDamageModPerLevel", "Attack Damage per Level"), 
				 new KeyValuePair<string, string>("%PercentAttackSpeedMod", "Attack Speed"), 
				 new KeyValuePair<string, string>("%FlatCritDamageMod", "Critical Damage"), 
				 new KeyValuePair<string, string>("%FlatCritChanceMod", "Critical Chance"), 
				 new KeyValuePair<string, string>("RFlatArmorPenetrationMod", "Armor Penetration"), 
				 new KeyValuePair<string, string>("FlatHPPoolMod", "Health"), 
				 new KeyValuePair<string, string>("RFlatHPModPerLevel", "Health per Level"), 
				 new KeyValuePair<string, string>("FlatArmorMod", "Armor"), 
				 new KeyValuePair<string, string>("FlatSpellBlockMod", "Magic Resist"), 
				 new KeyValuePair<string, string>("RFlatSpellBlockModPerLevel", "Magic Resist per Level"), 
				 new KeyValuePair<string, string>("%RPercentCooldownMod", "Cooldown Reduction"), 
				 new KeyValuePair<string, string>("FlatMagicDamageMod", "Ability Power"), 
				 new KeyValuePair<string, string>("RFlatMagicDamageModPerLevel", "Ability Power per Level"), 
				 new KeyValuePair<string, string>("FlatMPPoolMod", "Mana"), 
				 new KeyValuePair<string, string>("RFlatMPModPerLevel", "Mana per Level"), 
				 new KeyValuePair<string, string>("FlatMPRegenMod", "Mana Regeneration"), 
				 new KeyValuePair<string, string>("RFlatMagicPenetrationMod", "Magic Penetration"), 
				 new KeyValuePair<string, string>("FlatHPRegenMod", "Health Regeneration"), 
				 new KeyValuePair<string, string>("%RPercentCooldownModPerLevel", "Cooldown Reduction per Level"), 
				 new KeyValuePair<string, string>("RFlatMPRegenModPerLevel", "Mana Regeneration per Level"), 
				 new KeyValuePair<string, string>("RFlatArmorModPerLevel", "Armor per Level"), 
				 new KeyValuePair<string, string>("RFlatHPRegenModPerLevel", "Health Regeneration per Level"), 
				 new KeyValuePair<string, string>("%PercentMovementSpeedMod", "Movement Speed"), 
				 new KeyValuePair<string, string>("%RPercentTimeDeadMod", "Revival"), 
				 new KeyValuePair<string, string>("RFlatGoldPer10Mod", "Gold"), 
				 new KeyValuePair<string, string>("%PercentEXPBonus", "Experience"), 
				 new KeyValuePair<string, string>("FlatEnergyRegenMod", "Energy Regeneration"), 
				 new KeyValuePair<string, string>("RFlatEnergyRegenModPerLevel", "Energy Regeneration per Level"), 
				 new KeyValuePair<string, string>("FlatEnergyPoolMod", "Energy"), 
				 new KeyValuePair<string, string>("RFlatEnergyModPerLevel", "Energy per Level"), 
				 new KeyValuePair<string, string>("%PercentHPPoolMod", "Percent Health"), 
				 new KeyValuePair<string, string>("%PercentSpellVampMod", "Spell Vamp"), 
				 new KeyValuePair<string, string>("%PercentLifeStealMod", "Life Steal")
			};

		public RuneStatistics(RunesTab runesTab)
		{
			InitializeComponent();
			this.runesTab = runesTab;

		}


		public void AddOrRemoveStatistics(StatsStatic inputStatsStatic, bool Add = true)
		{
			try
			{
				Type myType = typeof(StatsStatic);
				System.Reflection.PropertyInfo[] properties = myType.GetProperties();

				foreach (System.Reflection.PropertyInfo property in properties)
				{
					Double newValue = (Double)inputStatsStatic.GetType().GetProperty(property.Name).GetValue(inputStatsStatic);
					Double oldValue = (Double)statsStatic.GetType().GetProperty(property.Name).GetValue(statsStatic);
					if (Add == true)
					{
						statsStatic.GetType().GetProperty(property.Name).SetValue(statsStatic, Math.Round(oldValue+newValue, 10, MidpointRounding.AwayFromZero));
					}
					else
					{
						statsStatic.GetType().GetProperty(property.Name).SetValue(statsStatic, Math.Round(oldValue - newValue, 10, MidpointRounding.AwayFromZero));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			GenerateStatisticsLabels();
		}

		private void GenerateStatisticsLabels()
		{
			List<Control> ctrls = xtraScrollCtrlStatistics.Controls.Cast<Control>().ToList();
			xtraScrollCtrlStatistics.Controls.Clear();
			foreach (Control c in ctrls)
				c.Dispose();

			Type myType = typeof(StatsStatic);
			System.Reflection.PropertyInfo[] properties = myType.GetProperties();

			int yPos = 10;
			int xPos = 3;
			foreach (System.Reflection.PropertyInfo property in properties)
			{
				Double statValue = (Double)statsStatic.GetType().GetProperty(property.Name).GetValue(statsStatic);
				if (statValue != 0.0)
				{
					Label newLblName = new Label();
					KeyValuePair<string, string> tempPair = runeNamePairs.FirstOrDefault(o => o.Key.Replace("%", "") == property.Name);
					newLblName.Text = tempPair.Value;
					newLblName.Location = new Point(xPos, yPos);
					newLblName.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
					newLblName.Size = new System.Drawing.Size(170, 20);
					xtraScrollCtrlStatistics.Controls.Add(newLblName);
					yPos += 20;
					Label newLblStat = new Label();
					if (tempPair.Key.Contains("%"))
					{
						newLblStat.Text = Math.Round(statValue * 100, 2, MidpointRounding.AwayFromZero).ToString() + "%";
					}
					else
					{
						newLblStat.Text = Math.Round(statValue, 2, MidpointRounding.AwayFromZero).ToString();
					}
					newLblStat.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
					newLblName.Size = new System.Drawing.Size(170, 20);
					newLblStat.Location = new Point(xPos, yPos);
					xtraScrollCtrlStatistics.Controls.Add(newLblStat);
					yPos += 20;

				}
			}

		}

	}
}
