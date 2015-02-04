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

				RunListOfCorrections(temp);

				
			}
		}

		private static void RunListOfCorrections(ItemStatic temp)
		{
			MatchCollection matches;

			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Base Health Regen", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentHPRegenMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf("%") - 1);
				temp.Stats.PercentHPRegenMod = Convert.ToDouble(value) / 100;
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Base Mana Regen", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentHPRegenMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf("%") - 1);
				temp.Stats.PercentHPRegenMod = Convert.ToDouble(value) / 100;
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Health", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.FlatHPPoolMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.FlatHPPoolMod = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Cooldown Reduction", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.RPercentCooldownMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf("%") - 1);
				temp.Stats.RPercentCooldownMod = Convert.ToDouble(value) / 100;
			}
			matches = Regex.Matches(temp.Description, "Reduces the duration of stuns, slows, taunts, fears, silences, blinds, polymorphs, and immobilizes by [^%]{1,3}%", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentTenacityMod == 0.0)
			{
				string found = matches[0].ToString();
				int start = found.LastIndexOf(" ") + 1;
				int end = found.LastIndexOf("%");
				string value = found.Substring(start, end - start);
				temp.Stats.PercentTenacityMod = Convert.ToDouble(value) / 100;
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3} Ability Power", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.FlatMagicDamageMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.FlatMagicDamageMod = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3} Attack Damage", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.FlatPhysicalDamageMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.FlatPhysicalDamageMod = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3} Armor Penetration", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.RFlatArmorPenetrationMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.RFlatArmorPenetrationMod = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Attack Speed", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentAttackSpeedMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf("%") - 1);
				temp.Stats.PercentAttackSpeedMod = Convert.ToDouble(value) / 100;
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Movement Speed", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentMovementSpeedMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf("%") - 1);
				temp.Stats.PercentMovementSpeedMod = Convert.ToDouble(value) / 100;
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3} Gold per 10 seconds", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.RFlatGoldPer10Mod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.RFlatGoldPer10Mod = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3} Magic Penetration", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.RFlatMagicPenetrationMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.RFlatMagicPenetrationMod = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3} Ability Power per level", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.RFlatMagicDamageModPerLevel == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf(" ") - 1);
				temp.Stats.RFlatMagicDamageModPerLevel = Convert.ToDouble(value);
			}
			matches = Regex.Matches(temp.Description, "UNIQUE Passive[^\\+]*\\+[^%]{1,3}% Spell Vamp", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentSpellVampMod == 0.0)
			{
				string found = matches[0].ToString();
				int start = found.IndexOf("+") + 1;
				int end = found.LastIndexOf("%");
				string value = found.Substring(start, end - start);
				temp.Stats.PercentSpellVampMod = Convert.ToDouble(value) / 100 + 0.00000001;
			}
			matches = Regex.Matches(temp.Description, "\\+[^%]{1,3}% Spell Vamp", RegexOptions.IgnoreCase);
			if (matches.Count > 0 && temp.Stats.PercentSpellVampMod == 0.0)
			{
				string found = matches[0].ToString();
				string value = found.Substring(1, found.IndexOf("%") - 1);
				temp.Stats.PercentSpellVampMod = Convert.ToDouble(value) / 100;
			}
			
		}

	}
}
