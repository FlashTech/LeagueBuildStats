using LeagueBuildStats.Classes.General_Classes;
using Newtonsoft.Json.Linq;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes.Items
{
	class ItemDataCorrections
	{
		internal static void RunCorrections(ItemListStatic item)
		{
			var keys = new List<int>(item.Items.Keys);
			foreach (int key in keys)
			{
				ItemStatic temp = item.Items[key];

				ItemCorrection(temp);

				
			}
		}

		private static void ItemCorrection(ItemStatic temp)
		{
			
			string descriptionMain;
			List<string> uniqueDescriptions = new List<string>();

			if (temp.SanitizedDescription.Contains("UNIQUE"))
			{
				descriptionMain = SubstringExtensions.Before(temp.SanitizedDescription, "UNIQUE");
				int i = 0;
				while ((i = temp.Description.IndexOf("UNIQUE", i)) != -1)
				{
					int j = temp.Description.IndexOf("<br>", i);
					if (j == -1) { j = temp.Description.Length - 1; }

					string uniqueDescription = temp.Description.Substring(i, j - i);

					if (!uniqueDescription.Contains("UNIQUE Active") //Todo: All of these should instead of being ignored should be filtered on the Stats Tab and Activatable Items or On/Off
						&& !uniqueDescription.Contains("UNIQUE Passive - Point Runner:")
						&& !uniqueDescription.Contains("UNIQUE Passive - Furor:")
						&& !uniqueDescription.Contains("UNIQUE Passive - Captain:"))
					{
						uniqueDescriptions.Add(uniqueDescription);
					}
					i++;
				}
			}
			else
			{
				descriptionMain = temp.SanitizedDescription;
			}


			StatCorrection(descriptionMain, temp.Stats);
			int ind = 0;
			temp.UniqueStats = new List<KeyValuePair<string, StatsStatic>>();
			foreach (string s in uniqueDescriptions)
			{
				if (s.Contains("UNIQUE Passive:") || s.Contains("UNIQUE Aura:"))
				{
					temp.UniqueStats.Add(new KeyValuePair<string, StatsStatic>(s, new StatsStatic()));
				}
				else if (s.Contains("UNIQUE Passive - Enhanced Movement"))
				{
					temp.UniqueStats.Add(new KeyValuePair<string, StatsStatic>(SubstringExtensions.Before(s, ":"), new StatsStatic()));
					temp.UniqueStats[ind].Value.FlatMovementSpeedMod = temp.Stats.FlatMovementSpeedMod;
					temp.Stats.FlatMovementSpeedMod = 0.0;
				}
				else
				{
					temp.UniqueStats.Add(new KeyValuePair<string, StatsStatic>(SubstringExtensions.Before(s, ":"), new StatsStatic()));
				}
				StatCorrection(s, temp.UniqueStats[ind].Value);
				ind++;
			}

		}


		private static void StatCorrection(string descriptionMain, StatsStatic statsStatic)
		{
			MatchCollection matches;


			matches = Regex.Matches(descriptionMain, "5 Attack Damage per stack", RegexOptions.IgnoreCase);
			for (int i = 0; i < matches.Count; i++)
			{
				statsStatic.FlatPhysicalDamageMod += 100;
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Base Health Regen", RegexOptions.IgnoreCase);
			if (statsStatic.PercentHPRegenMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentHPRegenMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Base Mana Regen", RegexOptions.IgnoreCase);
			if (statsStatic.PercentMPRegenMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentMPRegenMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Health", RegexOptions.IgnoreCase);
			if (statsStatic.FlatHPPoolMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.FlatHPPoolMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Health", RegexOptions.IgnoreCase);
			if (statsStatic.FlatHPPoolMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(0, found.IndexOf(" "));
					statsStatic.FlatHPPoolMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Cooldown Reduction", RegexOptions.IgnoreCase);
			if (statsStatic.RPercentCooldownMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.RPercentCooldownMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Reduces the duration of stuns, slows, taunts, fears, silences, blinds, polymorphs, and immobilizes by [^%]{1,4}%", RegexOptions.IgnoreCase);
			if (statsStatic.PercentTenacityMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					statsStatic.PercentTenacityMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Ability Power per level", RegexOptions.IgnoreCase);
			if (statsStatic.RFlatMagicDamageModPerLevel == 0.0)
			{
				for (int i = 0 ; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.RFlatMagicDamageModPerLevel += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Ability Power", RegexOptions.IgnoreCase);
			if (statsStatic.FlatMagicDamageMod == 0.0 && statsStatic.RFlatMagicDamageModPerLevel == 0.0) //Assume if per level then no flat damage mod
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.FlatMagicDamageMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Attack Damage", RegexOptions.IgnoreCase);
			if (statsStatic.FlatPhysicalDamageMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.FlatPhysicalDamageMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Armor Penetration", RegexOptions.IgnoreCase);
			if (statsStatic.RFlatArmorPenetrationMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.RFlatArmorPenetrationMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Attack Speed", RegexOptions.IgnoreCase);
			if (statsStatic.PercentAttackSpeedMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentAttackSpeedMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% bonus Attack Speed", RegexOptions.IgnoreCase);
			if (statsStatic.PercentAttackSpeedMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentAttackSpeedMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Movement Speed", RegexOptions.IgnoreCase);
			if (statsStatic.PercentMovementSpeedMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentMovementSpeedMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Movement Speed", RegexOptions.IgnoreCase);
			if (statsStatic.FlatMovementSpeedMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.FlatMovementSpeedMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Gold per 10 seconds", RegexOptions.IgnoreCase);
			if (statsStatic.RFlatGoldPer10Mod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.RFlatGoldPer10Mod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Magic Penetration", RegexOptions.IgnoreCase);
			if (statsStatic.RFlatMagicPenetrationMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					statsStatic.RFlatMagicPenetrationMod += Convert.ToDouble(value);
				}
			}
			matches = Regex.Matches(descriptionMain, "UNIQUE Passive[^\\+]*\\+[^%]{1,4}% Spell Vamp", RegexOptions.IgnoreCase);
			if (statsStatic.PercentSpellVampMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int start = found.IndexOf("+") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					statsStatic.PercentSpellVampMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Spell Vamp", RegexOptions.IgnoreCase);
			if (statsStatic.PercentSpellVampMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentSpellVampMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4} Magic Resist", RegexOptions.IgnoreCase);
			//Because of aura's that give you this stat this always needs to be checked
			double tempValue = 0;
			for (int i = 0; i < matches.Count; i++)
			{
				string found = matches[i].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				tempValue += Convert.ToDouble(value);
			}
			if (tempValue != 0)
			{
				statsStatic.FlatSpellBlockMod = tempValue;
			}
			matches = Regex.Matches(descriptionMain, "\\+[^%]{1,4}% Life Steal", RegexOptions.IgnoreCase);
			if (statsStatic.PercentLifeStealMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					statsStatic.PercentLifeStealMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "[^ ]{1,4} bonus Mana Regen per 5", RegexOptions.IgnoreCase);
			if (statsStatic.FlatMPRegenMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(0, found.IndexOf(" "));
					statsStatic.FlatMPRegenMod += Convert.ToDouble(value) / 5;
				}
			}
			matches = Regex.Matches(descriptionMain, "[^ ]{1,4}% increased Size, Slow Resistance, Tenacity", RegexOptions.IgnoreCase);
			if (statsStatic.PercentTenacityMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(0, found.IndexOf("%"));
					statsStatic.PercentTenacityMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "ignores [^%]{1,4}% of the target's Magic Resist", RegexOptions.IgnoreCase);
			if (statsStatic.RPercentMagicPenetrationMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(7, found.IndexOf("%") - 7);
					statsStatic.RPercentMagicPenetrationMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "ignores [^%]{1,4}% of the target's Armor", RegexOptions.IgnoreCase);
			if (statsStatic.RPercentArmorPenetrationMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(7, found.IndexOf("%") - 7);
					statsStatic.RPercentArmorPenetrationMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Increases self-healing, Health Regen, Lifesteal, and Spell Vamp effects by [^%]{1,4}%", RegexOptions.IgnoreCase);
			if (statsStatic.PercentHPRegenMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					statsStatic.PercentHPRegenMod += Convert.ToDouble(value) / 100;
					statsStatic.PercentLifeStealMod += Convert.ToDouble(value) / 100;
					statsStatic.PercentSpellVampMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Increases Ability Power by [^%]{1,4}%", RegexOptions.IgnoreCase);
			if (statsStatic.PercentMagicDamageMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					statsStatic.PercentMagicDamageMod += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Critical strikes deal [^%]{1,4}% damage instead of 200", RegexOptions.IgnoreCase);
			if (statsStatic.PercentCritDamageMod == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int end = found.IndexOf("%");
					string value = found.Substring(22, end - 22);
					statsStatic.PercentCritDamageMod += (Convert.ToDouble(value) / 100) - 2.00; //ex: 250% - 200% = +50%
				}
			}
			matches = Regex.Matches(descriptionMain, "Movement slowing effects are reduced by [^%]{1,4}%", RegexOptions.IgnoreCase);
			if (statsStatic.PercentSlowReistance == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					statsStatic.PercentSlowReistance += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "[^ ]{1,4}% increased Size, Slow Resistance", RegexOptions.IgnoreCase);
			if (statsStatic.PercentSlowReistance == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					string value = found.Substring(0, found.IndexOf("%"));
					statsStatic.PercentSlowReistance += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Grants bonus Attack Damage equal to [^%]{1,4}% of maximum Mana", RegexOptions.IgnoreCase);
			if (statsStatic.PercentManaAsBonusAttack == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int end = found.IndexOf("%");
					string value = found.Substring(36, end - 36);
					statsStatic.PercentManaAsBonusAttack += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Grants Ability Power equal to [^%]{1,4}% of maximum Mana", RegexOptions.IgnoreCase);
			if (statsStatic.PercentManaAsBonusAbility == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int end = found.IndexOf("%");
					string value = found.Substring(30, end - 30);
					statsStatic.PercentManaAsBonusAbility += Convert.ToDouble(value) / 100;
				}
			}
			matches = Regex.Matches(descriptionMain, "Health restore increases to [^%]{1,4}% of maximum Health if damage hasn't been taken", RegexOptions.IgnoreCase);
			if (statsStatic.PercentMaxHealthAsHealthRegen == 0.0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					string found = matches[i].ToString();
					int end = found.IndexOf("%");
					string value = found.Substring(28, end - 28);
					statsStatic.PercentMaxHealthAsHealthRegen += Convert.ToDouble(value) / 100;
				}
			}


		}
	}
}
