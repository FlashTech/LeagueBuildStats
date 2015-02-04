using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBuildStats
{
	class PublicStaticVariables
	{
		public static string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		public static string appFolderName = "LeagueBuildStats";

		public static string thisAppDataDir = string.Format(@"{0}\{1}", PublicStaticVariables.appDataFolder, PublicStaticVariables.appFolderName);

		public List<KeyValuePair<string, string>> ListForChampionTableSelections = new List<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("_Primary/Secondary Role", "mainAll"),

				new KeyValuePair<string, string>("Assassin", "Assassin"),
				new KeyValuePair<string, string>("Fighter", "Fighter"),
				new KeyValuePair<string, string>("Mage", "Mage"),
				new KeyValuePair<string, string>("Marksman", "Marksman"),
				new KeyValuePair<string, string>("Support", "Support"),
				new KeyValuePair<string, string>("Tank", "Tank"),

				new KeyValuePair<string, string>("_Resource Bar", "mainAll"),

				new KeyValuePair<string, string>("Mana", "Mana"),
				new KeyValuePair<string, string>("Other", "Other")

			};

		public List<KeyValuePair<string, string>> ListForItemTableSelections = new List<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("_All Items", "mainAll"),

				new KeyValuePair<string, string>("_Start", "Consumable GoldPer Vision Trinket"),
				new KeyValuePair<string, string>("Lane", "Lane"),
				new KeyValuePair<string, string>("Jungle", "Jungle"),

				new KeyValuePair<string, string>("_Tools", "Consumable GoldPer Vision Trinket"),
				new KeyValuePair<string, string>("Consumable", "Consumable"),
				new KeyValuePair<string, string>("Gold Income", "GoldPer"),
				new KeyValuePair<string, string>("Trinkets", "Trinket"),
				new KeyValuePair<string, string>("Vision", "Vision"),

				new KeyValuePair<string, string>("_Defence", "Health Armor SpellBlock HealthRegen Tenacity"),
				new KeyValuePair<string, string>("Health", "Health"),
				new KeyValuePair<string, string>("Armor", "Armor"),
				new KeyValuePair<string, string>("Magic Resist", "SpellBlock"),
				new KeyValuePair<string, string>("Health Regen", "HealthRegen"),
				new KeyValuePair<string, string>("Tenacity", "Tenacity"),

				new KeyValuePair<string, string>("_Attack", "Damage ArmorPenetration CriticalStrike AttackSpeed LifeSteal"),
				new KeyValuePair<string, string>("Damage", "Damage"),
				new KeyValuePair<string, string>("Armor Penetration", "ArmorPenetration"),
				new KeyValuePair<string, string>("Critical Strike", "CriticalStrike"),
				new KeyValuePair<string, string>("Attack Speed", "AttackSpeed"),
				new KeyValuePair<string, string>("Life Steal", "LifeSteal"),

				new KeyValuePair<string, string>("_Special Effects", "OnHit Aura Slow Active Stealth"),
				new KeyValuePair<string, string>("On hit", "OnHit"),
				new KeyValuePair<string, string>("Aura", "Aura"),
				new KeyValuePair<string, string>("Slow", "Slow"),
				new KeyValuePair<string, string>("Active", "Active"),
				new KeyValuePair<string, string>("Stealth Detection", "Stealth"),

				new KeyValuePair<string, string>("_Magic", "SpellDamage MagicPenetration CooldownReduction SpellVamp Mana ManaRegen"),
				new KeyValuePair<string, string>("Ability Power", "SpellDamage"),
				new KeyValuePair<string, string>("Magic Penetration", "MagicPenetration"),
				new KeyValuePair<string, string>("Cooldown Reduction", "CooldownReduction"),
				new KeyValuePair<string, string>("Spell Vamp", "SpellVamp"),
				new KeyValuePair<string, string>("Mana", "Mana"),
				new KeyValuePair<string, string>("Mana Regen", "ManaRegen"),

				new KeyValuePair<string, string>("_Movement", "Boots NonbootsMovement"),
				new KeyValuePair<string, string>("Boots", "Boots"),
				new KeyValuePair<string, string>("Other Movement", "NonbootsMovement")
			};

		public List<string> AllTags = new List<string>() {
				"noTag", "SpellDamage", "CriticalStrike", "AttackSpeed", "OnHit", "NonbootsMovement", "Consumable", "Health", "HealthRegen", 
				"Armor", "Slow", "ManaRegen", "GoldPer", "Active", "CooldownReduction", "Vision", "Aura", "SpellBlock", "Damage", 
				"LifeSteal", "Mana", "Boots", "Tenacity", "Trinket", "SpellVamp", "MagicPenetration", "ArmorPenetration", "Stealth", "Lane", "Jungle" };

		public static List<KeyValuePair<string, string>> StaticMapIDNames = new List<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("1", "Summoner's Rift"),
				new KeyValuePair<string, string>("2", "Summoner's Rift"),
				new KeyValuePair<string, string>("3", "The Proving Grounds"),
				new KeyValuePair<string, string>("4", "Twisted Treeline"),
				new KeyValuePair<string, string>("8", "The Crystal Scar"),
				new KeyValuePair<string, string>("10", "Twisted Treeline"),
				new KeyValuePair<string, string>("12", "Howling Abyss")
			};

	}
}
