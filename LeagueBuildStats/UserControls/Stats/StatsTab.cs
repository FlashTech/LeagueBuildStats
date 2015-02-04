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

namespace LeagueBuildStats.UserControls
{
	public partial class StatsTab : UserControl
	{
		private Form1 form1;

		public StatsTab()
		{
			InitializeComponent();
		}

		public StatsTab(Form1 form)
		{
			this.form1 = form;
			InitializeComponent();
		}

		public void UpdateStatsTab()
		{
			//todo: implement level and map selection
			int level = 1;
			try
			{
				//Get stats from items
				StatsStatic statsStatic = new StatsStatic();
				statsStatic = GetItemsStats(statsStatic);
				statsStatic = GetRunesStats(statsStatic);

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

				double dDamageBonus = 0;

				double dAverageAttackDamageWithCrit = (dAttackDamage * ((100 - dCritChance) / 100)) + ((dDamageIfCrit) * (dCritChance / 100));
				double dDPS = dAverageAttackDamageWithCrit * dAttackSpeed;


				Color HighlightColor = Color.Aqua;

				//Offence Text Boxes//
				lblCtrlAttackDamage.Text = Math.Round(dAttackDamage, MidpointRounding.AwayFromZero).ToString();
				//if (dAttackDamage != champ1.Stats.AttackDamage + ((level - 1) * champ1.Stats.AttackDamagePerLevel)) { lblCtrlAttackDamage.ForeColor = HighlightColor; } else { lblCtrlAttackDamage.ForeColor = Color.White; }
				lblCtrlAttackSpeed.Text = Math.Round(dAttackSpeed, 2, MidpointRounding.AwayFromZero).ToString();
				lblCtrlArmorPen1.Text = Math.Round(dArmorPen1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlArmorPen2.Text = Math.Round(dArmorPen2, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlCritChance.Text = Math.Round(dCritChance * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlCritDmg.Text = Math.Round(dCritDamage * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlDamageIfCrit.Text = Math.Round(dDamageIfCrit, MidpointRounding.AwayFromZero).ToString();

				lblCtrlAbilityPower.Text = Math.Round(dAbilityPower, MidpointRounding.AwayFromZero).ToString();
				lblCtrlMagicPen1.Text = Math.Round(dMagicPen1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlMagicPen2.Text = Math.Round(dMagicPen2, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlAveAttackDPS.Text = Math.Round(dDPS, MidpointRounding.AwayFromZero).ToString();


				//Defence Variables//////////////////////////////////////////////////
				double dHealth = champ1.Stats.Hp + (level - 1) * champ1.Stats.HpPerLevel + statsStatic.FlatHPPoolMod + (level - 1) * statsStatic.RFlatHPModPerLevel;
				double dHealthRegenBase = champ1.Stats.HpRegen + (level - 1) * champ1.Stats.HpRegenPerLevel ;
				double dHealthRegen1 = dHealthRegenBase + dHealthRegenBase * (statsStatic.PercentHPRegenMod) + (statsStatic.FlatHPRegenMod * 5) + (level - 1) * (statsStatic.RFlatHPRegenModPerLevel * 5);
				double dHealthRegen2 = dHealthRegen1 / dHealthRegenBase * 100;
				double dArmor = champ1.Stats.Armor + (level - 1) * champ1.Stats.ArmorPerLevel + statsStatic.FlatArmorMod + (level - 1) * statsStatic.RFlatArmorModPerLevel;
				double dMagicResist = statsStatic.FlatSpellBlockMod + (level - 1) * statsStatic.RFlatSpellBlockModPerLevel;
				double dTenacity = statsStatic.PercentTenacityMod * 100;//(1 - (1 - 0.35)*(1 - 0.25)*(1 - 0.15)) * 100; //todo there is no tenacity stat

				double dDamabeReduction = 0.0 + statsStatic.FlatBlockMod; //todo there is no damamge reduction stat
				double dAbsorbShield = 0.0; //todo there is no absorb shield stat

				double dEffPhysicalHealth = dHealth * (100 + dArmor) / (100 * (1 - (dDamabeReduction / 100)));
				double dEffMagicalHealth = dHealth * (100 + dMagicResist) / (100 * (1 - (dDamabeReduction / 100)));
				double dEffHealth = (dEffPhysicalHealth + dEffMagicalHealth) / 2;


				//Defence Text Boxes//
				lblCtrlHealth.Text = Math.Round(dHealth, MidpointRounding.AwayFromZero).ToString();
				lblCtrlHealthRegen1.Text = Math.Round(dHealthRegen1, MidpointRounding.AwayFromZero).ToString() + "/s";
				lblCtrlHealthRegen2.Text = Math.Round(dHealthRegen2, MidpointRounding.AwayFromZero).ToString() + "%/s";
				lblCtrlArmor.Text = Math.Round(dArmor, MidpointRounding.AwayFromZero).ToString();
				lblCtrlMagicResist.Text = Math.Round(dMagicResist, MidpointRounding.AwayFromZero).ToString();
				lblCtrlTenacity.Text = Math.Round(dTenacity, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlDmgReduction.Text = Math.Round(dDamabeReduction, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlAbsorbShield.Text = Math.Round(dAbsorbShield, MidpointRounding.AwayFromZero).ToString();

				lblCtrlEffPhysHealth.Text = Math.Round(dEffPhysicalHealth, MidpointRounding.AwayFromZero).ToString();
				lblCtrlEffMagHealth.Text = Math.Round(dEffMagicalHealth, MidpointRounding.AwayFromZero).ToString();
				lblCtrlEffHealth.Text = Math.Round(dEffHealth, MidpointRounding.AwayFromZero).ToString();


				//Utility Variables//////////////////////////////////////////////////
				double dCooldownReduction = statsStatic.RPercentCooldownMod + (level - 1) * statsStatic.RPercentCooldownModPerLevel;
				double dMovementBase = champ1.Stats.MoveSpeed;

				double dMovementSpeed = (dMovementBase + statsStatic.FlatMovementSpeedMod + (level - 1) * statsStatic.RFlatMovementSpeedModPerLevel) * (1 + statsStatic.PercentMovementSpeedMod);
				//(Base Movement Speed + Flat Movement Bonuses) × (1 + Additional Percentage Movement Bonuses) × (1 + First Multiplicative Percentage Movement Bonus) × .... 
				//× (1 + Last Multiplicative Percentage Movement Bonus) × (1 - Slow Ratio × Slow Resist Ratio)

				double dLifeSteal = statsStatic.PercentLifeStealMod;
				double dSpellVamp = statsStatic.PercentSpellVampMod;

				double dAttackRange = champ1.Stats.AttackRange;
				double dGoldPer10 = 16.0 + statsStatic.RFlatGoldPer10Mod;
				double dExperienceGain = (1 + statsStatic.PercentEXPBonus) * 100;
				double dRevivalSpeed = (1 + statsStatic.RPercentTimeDeadMod) * 100;

				double dManaenergy = champ1.Stats.Mp + (level - 1) * champ1.Stats.MpPerLevel;
				double dBaseManaRegen = champ1.Stats.MpRegen + (level - 1) * champ1.Stats.MpRegenPerLevel;
				double dManaEnergyRegen1 = dBaseManaRegen + (statsStatic.FlatMPRegenMod * 5) + (level - 1) * (statsStatic.RFlatMPRegenModPerLevel * 5);
				double dManaEnergyRegen2 = (dManaEnergyRegen1 / dBaseManaRegen) * 100;

				//Utility Text Boxes//
				lblCtrlCooldownReduction.Text = Math.Round(dCooldownReduction * 100, 1, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlMovement.Text = Math.Round(dMovementSpeed, MidpointRounding.AwayFromZero).ToString();
				lblCtrlLifeSteal.Text = Math.Round(dLifeSteal, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlSpellVamp.Text = Math.Round(dSpellVamp, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlAutoAttackRange.Text = Math.Round(dAttackRange, MidpointRounding.AwayFromZero).ToString();
				lblCtrlGoldPer10.Text = Math.Round(dGoldPer10, 2, MidpointRounding.AwayFromZero).ToString();
				lblCtrlExperienceGain.Text = Math.Round(dExperienceGain, MidpointRounding.AwayFromZero).ToString() + "%";
				lblCtrlRevivalSpeed.Text = Math.Round(dRevivalSpeed, MidpointRounding.AwayFromZero).ToString() + "%";

				lblCtrlManaEnergy.Text = Math.Round(dManaenergy, MidpointRounding.AwayFromZero).ToString();
				lblCtrlManaEnergyRegen1.Text = Math.Round(dManaEnergyRegen1, 1, MidpointRounding.AwayFromZero).ToString();
				lblCtrlManaEnergyRegen2.Text = Math.Round(dManaEnergyRegen2, MidpointRounding.AwayFromZero).ToString() + "%/s";

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		

		private StatsStatic GetItemsStats(StatsStatic statsStatic)
		{

			foreach (Panel p in form1.mainTopBar.itemPanels)
			{
				if (p.Tag != null)
				{
					//TODO: it is possible to loop through these like on the runes tab
					statsStatic.PercentTenacityMod = (1 - (1 - ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentTenacityMod)* (1 - statsStatic.PercentTenacityMod));
					statsStatic.FlatArmorMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatArmorMod;
					statsStatic.FlatAttackSpeedMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatAttackSpeedMod;
					statsStatic.FlatBlockMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatBlockMod;
					statsStatic.FlatCritChanceMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatCritChanceMod;
					statsStatic.FlatCritDamageMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatCritDamageMod;
					statsStatic.FlatEnergyPoolMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatEnergyPoolMod;
					statsStatic.FlatEnergyRegenMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatEnergyRegenMod;
					statsStatic.FlatEXPBonus += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatEXPBonus;
					statsStatic.FlatHPPoolMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatHPPoolMod;
					statsStatic.FlatHPRegenMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatHPRegenMod;
					statsStatic.FlatMagicDamageMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatMagicDamageMod;
					statsStatic.FlatMovementSpeedMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatMovementSpeedMod;
					statsStatic.FlatMPPoolMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatMPPoolMod;
					statsStatic.FlatMPRegenMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatMPRegenMod;
					statsStatic.FlatPhysicalDamageMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatPhysicalDamageMod;
					statsStatic.FlatSpellBlockMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.FlatSpellBlockMod;
					statsStatic.PercentArmorMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentArmorMod;
					statsStatic.PercentAttackSpeedMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentAttackSpeedMod;
					statsStatic.PercentBlockMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentBlockMod;
					statsStatic.PercentCritChanceMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentCritChanceMod;
					statsStatic.PercentCritDamageMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentCritDamageMod;
					statsStatic.PercentDodgeMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentDodgeMod;
					statsStatic.PercentEXPBonus += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentEXPBonus;
					statsStatic.PercentHPPoolMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentHPPoolMod;
					statsStatic.PercentHPRegenMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentHPRegenMod;
					statsStatic.PercentLifeStealMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentLifeStealMod;
					statsStatic.PercentMagicDamageMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentMagicDamageMod;
					statsStatic.PercentMovementSpeedMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentMovementSpeedMod;
					statsStatic.PercentMPPoolMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentMPPoolMod;
					statsStatic.PercentMPRegenMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentMPRegenMod;
					statsStatic.PercentPhysicalDamageMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentPhysicalDamageMod;
					statsStatic.PercentSpellBlockMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentSpellBlockMod;
					statsStatic.PercentSpellVampMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentSpellVampMod;
					statsStatic.RFlatArmorModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatArmorModPerLevel;
					statsStatic.RFlatArmorPenetrationMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatArmorPenetrationMod;
					statsStatic.RFlatArmorPenetrationModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatArmorPenetrationModPerLevel;
					statsStatic.RFlatCritChanceModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatCritChanceModPerLevel;
					statsStatic.RFlatCritDamageModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatCritDamageModPerLevel;
					statsStatic.RFlatDodgeMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatDodgeMod;
					statsStatic.RFlatDodgeModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatDodgeModPerLevel;
					statsStatic.RFlatEnergyModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatEnergyModPerLevel;
					statsStatic.RFlatEnergyRegenModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatEnergyRegenModPerLevel;
					statsStatic.RFlatGoldPer10Mod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatGoldPer10Mod;
					statsStatic.RFlatHPModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatHPModPerLevel;
					statsStatic.RFlatHPRegenModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatHPRegenModPerLevel;
					statsStatic.RFlatMagicDamageModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatMagicDamageModPerLevel;
					statsStatic.RFlatMagicPenetrationMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatMagicPenetrationMod;
					statsStatic.RFlatMagicPenetrationModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatMagicPenetrationModPerLevel;
					statsStatic.RFlatMovementSpeedModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatMovementSpeedModPerLevel;
					statsStatic.RFlatMPModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatMPModPerLevel;
					statsStatic.RFlatMPRegenModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatMPRegenModPerLevel;
					statsStatic.RFlatPhysicalDamageModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatPhysicalDamageModPerLevel;
					statsStatic.RFlatSpellBlockModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatSpellBlockModPerLevel;
					statsStatic.RFlatTimeDeadMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.PercentLifeStealMod;
					statsStatic.RFlatTimeDeadModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RFlatTimeDeadModPerLevel;
					statsStatic.RPercentArmorPenetrationMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentArmorPenetrationMod;
					statsStatic.RPercentArmorPenetrationModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentArmorPenetrationModPerLevel;
					statsStatic.RPercentAttackSpeedModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentAttackSpeedModPerLevel;
					statsStatic.RPercentCooldownMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentCooldownMod;
					statsStatic.RPercentCooldownModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentCooldownModPerLevel;
					statsStatic.RPercentMagicPenetrationMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentMagicPenetrationMod;
					statsStatic.RPercentMagicPenetrationModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentMagicPenetrationModPerLevel;
					statsStatic.RPercentMovementSpeedModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentMovementSpeedModPerLevel;
					statsStatic.RPercentTimeDeadMod += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentTimeDeadMod;
					statsStatic.RPercentTimeDeadModPerLevel += ((CreateItemDiv)p.Tag).aItem.Value.Stats.RPercentTimeDeadModPerLevel;
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
							statsStatic.FlatArmorMod += ((RuneStatic)p.Tag).Stats.FlatArmorMod;
							statsStatic.FlatAttackSpeedMod += ((RuneStatic)p.Tag).Stats.FlatAttackSpeedMod;
							statsStatic.FlatBlockMod += ((RuneStatic)p.Tag).Stats.FlatBlockMod;
							statsStatic.FlatCritChanceMod += ((RuneStatic)p.Tag).Stats.FlatCritChanceMod;
							statsStatic.FlatCritDamageMod += ((RuneStatic)p.Tag).Stats.FlatCritDamageMod;
							statsStatic.FlatEnergyPoolMod += ((RuneStatic)p.Tag).Stats.FlatEnergyPoolMod;
							statsStatic.FlatEnergyRegenMod += ((RuneStatic)p.Tag).Stats.FlatEnergyRegenMod;
							statsStatic.FlatEXPBonus += ((RuneStatic)p.Tag).Stats.FlatEXPBonus;
							statsStatic.FlatHPPoolMod += ((RuneStatic)p.Tag).Stats.FlatHPPoolMod;
							statsStatic.FlatHPRegenMod += ((RuneStatic)p.Tag).Stats.FlatHPRegenMod;
							statsStatic.FlatMagicDamageMod += ((RuneStatic)p.Tag).Stats.FlatMagicDamageMod;
							statsStatic.FlatMovementSpeedMod += ((RuneStatic)p.Tag).Stats.FlatMovementSpeedMod;
							statsStatic.FlatMPPoolMod += ((RuneStatic)p.Tag).Stats.FlatMPPoolMod;
							statsStatic.FlatMPRegenMod += ((RuneStatic)p.Tag).Stats.FlatMPRegenMod;
							statsStatic.FlatPhysicalDamageMod += ((RuneStatic)p.Tag).Stats.FlatPhysicalDamageMod;
							statsStatic.FlatSpellBlockMod += ((RuneStatic)p.Tag).Stats.FlatSpellBlockMod;
							statsStatic.PercentArmorMod += ((RuneStatic)p.Tag).Stats.PercentArmorMod;
							statsStatic.PercentAttackSpeedMod += ((RuneStatic)p.Tag).Stats.PercentAttackSpeedMod;
							statsStatic.PercentBlockMod += ((RuneStatic)p.Tag).Stats.PercentBlockMod;
							statsStatic.PercentCritChanceMod += ((RuneStatic)p.Tag).Stats.PercentCritChanceMod;
							statsStatic.PercentCritDamageMod += ((RuneStatic)p.Tag).Stats.PercentCritDamageMod;
							statsStatic.PercentDodgeMod += ((RuneStatic)p.Tag).Stats.PercentDodgeMod;
							statsStatic.PercentEXPBonus += ((RuneStatic)p.Tag).Stats.PercentEXPBonus;
							statsStatic.PercentHPPoolMod += ((RuneStatic)p.Tag).Stats.PercentHPPoolMod;
							statsStatic.PercentHPRegenMod += ((RuneStatic)p.Tag).Stats.PercentHPRegenMod;
							statsStatic.PercentLifeStealMod += ((RuneStatic)p.Tag).Stats.PercentLifeStealMod;
							statsStatic.PercentMagicDamageMod += ((RuneStatic)p.Tag).Stats.PercentMagicDamageMod;
							statsStatic.PercentMovementSpeedMod += ((RuneStatic)p.Tag).Stats.PercentMovementSpeedMod;
							statsStatic.PercentMPPoolMod += ((RuneStatic)p.Tag).Stats.PercentMPPoolMod;
							statsStatic.PercentMPRegenMod += ((RuneStatic)p.Tag).Stats.PercentMPRegenMod;
							statsStatic.PercentPhysicalDamageMod += ((RuneStatic)p.Tag).Stats.PercentPhysicalDamageMod;
							statsStatic.PercentSpellBlockMod += ((RuneStatic)p.Tag).Stats.PercentSpellBlockMod;
							statsStatic.PercentSpellVampMod += ((RuneStatic)p.Tag).Stats.PercentSpellVampMod;
							statsStatic.RFlatArmorModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatArmorModPerLevel;
							statsStatic.RFlatArmorPenetrationMod += ((RuneStatic)p.Tag).Stats.RFlatArmorPenetrationMod;
							statsStatic.RFlatArmorPenetrationModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatArmorPenetrationModPerLevel;
							statsStatic.RFlatCritChanceModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatCritChanceModPerLevel;
							statsStatic.RFlatCritDamageModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatCritDamageModPerLevel;
							statsStatic.RFlatDodgeMod += ((RuneStatic)p.Tag).Stats.RFlatDodgeMod;
							statsStatic.RFlatDodgeModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatDodgeModPerLevel;
							statsStatic.RFlatEnergyModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatEnergyModPerLevel;
							statsStatic.RFlatEnergyRegenModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatEnergyRegenModPerLevel;
							statsStatic.RFlatGoldPer10Mod += ((RuneStatic)p.Tag).Stats.RFlatGoldPer10Mod;
							statsStatic.RFlatHPModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatHPModPerLevel;
							statsStatic.RFlatHPRegenModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatHPRegenModPerLevel;
							statsStatic.RFlatMagicDamageModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatMagicDamageModPerLevel;
							statsStatic.RFlatMagicPenetrationMod += ((RuneStatic)p.Tag).Stats.RFlatMagicPenetrationMod;
							statsStatic.RFlatMagicPenetrationModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatMagicPenetrationModPerLevel;
							statsStatic.RFlatMovementSpeedModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatMovementSpeedModPerLevel;
							statsStatic.RFlatMPModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatMPModPerLevel;
							statsStatic.RFlatMPRegenModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatMPRegenModPerLevel;
							statsStatic.RFlatPhysicalDamageModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatPhysicalDamageModPerLevel;
							statsStatic.RFlatSpellBlockModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatSpellBlockModPerLevel;
							statsStatic.RFlatTimeDeadMod += ((RuneStatic)p.Tag).Stats.PercentLifeStealMod;
							statsStatic.RFlatTimeDeadModPerLevel += ((RuneStatic)p.Tag).Stats.RFlatTimeDeadModPerLevel;
							statsStatic.RPercentArmorPenetrationMod += ((RuneStatic)p.Tag).Stats.RPercentArmorPenetrationMod;
							statsStatic.RPercentArmorPenetrationModPerLevel += ((RuneStatic)p.Tag).Stats.RPercentArmorPenetrationModPerLevel;
							statsStatic.RPercentAttackSpeedModPerLevel += ((RuneStatic)p.Tag).Stats.RPercentAttackSpeedModPerLevel;
							statsStatic.RPercentCooldownMod += ((RuneStatic)p.Tag).Stats.RPercentCooldownMod;
							statsStatic.RPercentCooldownModPerLevel += ((RuneStatic)p.Tag).Stats.RPercentCooldownModPerLevel;
							statsStatic.RPercentMagicPenetrationMod += ((RuneStatic)p.Tag).Stats.RPercentMagicPenetrationMod;
							statsStatic.RPercentMagicPenetrationModPerLevel += ((RuneStatic)p.Tag).Stats.RPercentMagicPenetrationModPerLevel;
							statsStatic.RPercentMovementSpeedModPerLevel += ((RuneStatic)p.Tag).Stats.RPercentMovementSpeedModPerLevel;
							statsStatic.RPercentTimeDeadMod += ((RuneStatic)p.Tag).Stats.RPercentTimeDeadMod;
							statsStatic.RPercentTimeDeadModPerLevel += ((RuneStatic)p.Tag).Stats.RPercentTimeDeadModPerLevel;
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
