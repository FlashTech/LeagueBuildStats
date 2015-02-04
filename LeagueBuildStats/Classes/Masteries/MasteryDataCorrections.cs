using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeagueBuildStats.Classes.Masteries
{
	class MasteryDataCorrections
	{
		internal static void RunCorrections(MasteryListStatic masteries)
		{
			

			var keys = new List<int>(masteries.Masteries.Keys);
			foreach (int key in keys)
			{
				MasteryStatic temp = masteries.Masteries[key];

				RunListOfCorrections(temp);

			}
		}

		private static void RunListOfCorrections(MasteryStatic temp)
		{
			temp.StatsList = new List<StatsStatic>();
			
			MatchCollection matches;

			foreach (string sDesc in temp.Description)
			{
				int pos = temp.Description.FindIndex(o => o == sDesc);

				bool bContinue = true;

				temp.StatsList.Add(new StatsStatic());

				//Offencive Tree
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% Attack Speed", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].PercentAttackSpeedMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].PercentAttackSpeedMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% Cooldown Reduction", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].RPercentCooldownMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].RPercentCooldownMod = Convert.ToDouble(value) / 100;//This will also apply for the Utility mastery called Intelligence
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Attack Damage per level", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].RFlatPhysicalDamageModPerLevel == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].RFlatPhysicalDamageModPerLevel = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Ability Power per level", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].RFlatMagicDamageModPerLevel == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].RFlatMagicDamageModPerLevel = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Attack Damage", RegexOptions.IgnoreCase);
				if (bContinue && matches.Count > 0 && temp.StatsList[pos].FlatPhysicalDamageMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatPhysicalDamageMod = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Ability Power", RegexOptions.IgnoreCase);
				if (bContinue && matches.Count > 0 && temp.StatsList[pos].FlatMagicDamageMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatMagicDamageMod = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "Increases bonus Attack Damage by [^%]{1,4}%", RegexOptions.IgnoreCase);
				if (bContinue && matches.Count > 0 && temp.StatsList[pos].PercentBonusPhysicalDamageMod == 0.0)
				{
					string found = matches[0].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					temp.StatsList[pos].PercentBonusPhysicalDamageMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "Increases Ability Power by [^%]{1,4}%", RegexOptions.IgnoreCase);
				if (bContinue && matches.Count > 0 && temp.StatsList[pos].PercentMagicDamageMod == 0.0)
				{
					string found = matches[0].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					temp.StatsList[pos].PercentMagicDamageMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% Armor and Magic Penetration", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].RPercentArmorPenetrationMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].RPercentArmorPenetrationMod = Convert.ToDouble(value) / 100; //armor pen
					temp.StatsList[pos].RPercentMagicPenetrationMod = Convert.ToDouble(value) / 100; //magic pen
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% increased damage", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].PercentIncreasedDamageMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].PercentIncreasedDamageMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}

				//Defencive Tree

				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Health per 5 seconds", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].FlatHPRegenMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatHPRegenMod = Convert.ToDouble(value) / 5; //is stored as per 1 second
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "Increases bonus Armor and Magic Resist by [^%]{1,4}%", RegexOptions.IgnoreCase);
				if (bContinue && matches.Count > 0 && temp.StatsList[pos].PercentBonusMagicResistMod == 0.0)
				{
					string found = matches[0].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					temp.StatsList[pos].PercentBonusMagicResistMod = Convert.ToDouble(value) / 100;
					temp.StatsList[pos].PercentBonusArmorMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Health", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") &&matches.Count > 0 && temp.StatsList[pos].FlatHPPoolMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatHPPoolMod = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% Maximum Health", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].PercentHPPoolMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].PercentHPPoolMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Armor", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].FlatArmorMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatArmorMod = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Magic Resist", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].FlatSpellBlockMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatSpellBlockMod = Convert.ToDouble(value);
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "Reduces the duration of crowd control effects by [^%]{1,4}%", RegexOptions.IgnoreCase);
				if (bContinue && matches.Count > 0 && temp.StatsList[pos].PercentTenacityMod == 0.0)
				{
					string found = matches[0].ToString();
					int start = found.LastIndexOf(" ") + 1;
					int end = found.LastIndexOf("%");
					string value = found.Substring(start, end - start);
					temp.StatsList[pos].PercentTenacityMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}

				//Utility Tree


				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% Movement Speed", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].PercentMovementSpeedMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].PercentMovementSpeedMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Mana Regen per 5 seconds", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].FlatHPRegenMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].FlatHPRegenMod = Convert.ToDouble(value) / 5; //is stored as per 1 second
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4} Gold every 10 seconds", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].RFlatGoldPer10Mod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf(" ") - 1);
					temp.StatsList[pos].RFlatGoldPer10Mod = Convert.ToDouble(value); //is stored as per 1 second
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% Lifesteal and Spellvamp", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].PercentLifeStealMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].PercentLifeStealMod = Convert.ToDouble(value) / 100;
					temp.StatsList[pos].PercentSpellVampMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}
				matches = Regex.Matches(temp.Description[pos], "\\+[^%]{1,4}% increased maximum Mana", RegexOptions.IgnoreCase);
				if (bContinue && temp.Description[pos].StartsWith("+") && matches.Count > 0 && temp.StatsList[pos].PercentMPPoolMod == 0.0)
				{
					string found = matches[0].ToString();
					string value = found.Substring(1, found.IndexOf("%") - 1);
					temp.StatsList[pos].PercentMPPoolMod = Convert.ToDouble(value) / 100;
					bContinue = false;
				}




			}
		}

	}
}
