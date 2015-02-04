using Newtonsoft.Json.Linq;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes.Champions
{
	class ChampionDataCorrections
	{

		internal static string RunCorrections(string jsonString)
		{
			var jsonObject = JObject.Parse(jsonString);


			//List of updates to make
			//Todo: This list should be verified!!!!!!
			List<List<string>> myList = new List<List<string>>();
			// "Champion", "ability #", "coeff", "key", "link"       coeff can be a string if braced with \"
			myList.Add(new List<string> { "Thresh", "2", "\"(num of souls)\"", "f1", "" });
			myList.Add(new List<string> { "Thresh", "2", "\"(num of souls + ({{ e3 }})% of AD)\"", "f2", "" });
			myList.Add(new List<string> { "Aatrox", "1", "\"15 / 23.75 / 32.5 / 41.25 / 50 (+25% bonus AD)\"", "f4", "" });
			myList.Add(new List<string> { "Aatrox", "1", "0.75", "f5", "bonusattackdamage" });
			myList.Add(new List<string> { "Gragas", "2", "3", "f1", "" });
			myList.Add(new List<string> { "Cassiopeia", "2", "0.55", "f2", "spelldamage" });
			myList.Add(new List<string> { "Sion", "1", "0.10", "f2", "maxhealth" });
			myList.Add(new List<string> { "Ahri", "1", "\"{{ e3 }} (+64% of ability power)\"", "f1", "" });
			myList.Add(new List<string> { "Viktor", "0", "0.20", "f3", "spelldamage" });
			myList.Add(new List<string> { "Viktor", "0", "\"20 - 210 (Based on Viktor's level)\"", "f2", "" }); // TODO: verify this:   Damage = 11.2(level) + 8.8       For now I hard coded this as 20 - 210
			myList.Add(new List<string> { "Garen", "2", "\"Calculated on Stats Tab\"", "f1", "" }); //{{ e1 }} plus {{ e3 }}% of his attack
			myList.Add(new List<string> { "Evelynn", "0", "0.35, 0.40, 0.45, 0.50, 0.55", "f2", "@custom.percentOfAP" });
			myList.Add(new List<string> { "Zed", "1", "\"Calculated on Stats Tab\"", "f3", "" });
			myList.Add(new List<string> { "Sona", "0", "\"40 / 50 / 60 / 70 / 80 (+10 for each rank in Cresendo)\"", "f1", "" });
			myList.Add(new List<string> { "Sona", "1", "\"40 / 60 / 80 / 100 / 120 (+10 for each rank in Cresendo)\"", "f2", "" });
			myList.Add(new List<string> { "Sona", "1", "\"(2% of AP)\"", "f1", "" });
			myList.Add(new List<string> { "Sona", "2", "\"13 / 14 / 15 / 16 / 17% (+2% for each rank in Cresendo)\"", "f3", "" });
			myList.Add(new List<string> { "Sona", "2", "\" +0.075\"", "f2", "spelldamage" });
			myList.Add(new List<string> { "Sona", "2", "\"11 / 12 / 13 / 14 / 15% (+2% for each rank in Cresendo)\"", "f4", "" });
			myList.Add(new List<string> { "Sona", "2", "0.035", "f5", "spelldamage" });
			myList.Add(new List<string> { "Jayce", "3", "20.0, 60.0, 100.0, 140.0", "f1", "@custom.percent" });
			myList.Add(new List<string> { "Khazix", "0", "91, 123.5, 156, 188.5, 221", "f3", "" });
			myList.Add(new List<string> { "Khazix", "0", "1.56", "f2", "bonusattackdamage" });
			myList.Add(new List<string> { "Corki", "0", "0.50", "f1", "spelldamage" });
			myList.Add(new List<string> { "Azir", "1", "50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 110, 120, 130, 140, 150, 160, 170", "f2", "" });
			myList.Add(new List<string> { "Azir", "1", "2", "maxammo", "" });
			myList.Add(new List<string> { "Azir", "1", "12, 11, 10, 9, 8", "f1", "" });
			myList.Add(new List<string> { "Azir", "1", "\"(80 + 25 x level)\"", "f4", "" });
			myList.Add(new List<string> { "Azir", "2", "0.15", "f1", "bonushealth" });
			myList.Add(new List<string> { "Nidalee", "0", "4, 20, 50, 90", "f2", "" });
			myList.Add(new List<string> { "Nidalee", "0", "0.40", "f3", "spelldamage" });
			myList.Add(new List<string> { "Nidalee", "0", "10, 50, 125, 225", "f4", "" });
			myList.Add(new List<string> { "Nidalee", "0", "1.875", "f1", "attackdamage" });
			myList.Add(new List<string> { "Nidalee", "1", "50, 100, 150, 200", "f1", "" });
			myList.Add(new List<string> { "Nidalee", "1", "1.5", "f2", "" });
			myList.Add(new List<string> { "Nidalee", "2", "70, 130, 190, 250", "f1", "" });
			myList.Add(new List<string> { "Gnar", "1", "\"TODO\"", "f1", "" }); //need to find this out in game or wikia
			myList.Add(new List<string> { "Gnar", "2", "0.06", "f1", "maxhealth" });
			myList.Add(new List<string> { "Nunu", "0", "0.10", "f3", "" });
			myList.Add(new List<string> { "Nunu", "0", "0.15", "f4", "" });
			myList.Add(new List<string> { "Nunu", "0", "0.01", "f5", "" });
			myList.Add(new List<string> { "Nunu", "3", "\"TODO\"", "f2", "" });//need to find this out in game or wikia
			myList.Add(new List<string> { "Kalista", "2", "0.15, 0.18, 0.21, 0.24, 0.27", "f1", "@custom.percentOfAttack" });
			myList.Add(new List<string> { "RekSai", "1", "15, 20, 25", "f1", "" });
			myList.Add(new List<string> { "RekSai", "1", "80, 90, 100, 110, 120", "f1", "@custom.percentOfAttack" });
			myList.Add(new List<string> { "RekSai", "2", "\"TODO\"", "f2", "" });//need to find this out in game or wikia
			myList.Add(new List<string> { "Rengar", "0", "30, 60, 90, 120, 150", "f3", "" });
			myList.Add(new List<string> { "Rengar", "0", "\"TODO\"", "f2", "" });//need to find this out in game or wikia
			myList.Add(new List<string> { "Rengar", "0", "\"+50-101\"", "f4", "" });//Todo: generate a function for this value, which is based on champion level
			myList.Add(new List<string> { "Rengar", "1", "\"40-240\"", "f2", "" }); ;//Todo: generate a function for this value, which is based on champion level
			myList.Add(new List<string> { "Rengar", "1", "\"12 + (4 x level)\"", "f1", "" });
			myList.Add(new List<string> { "Rengar", "1", "\"(6.25% per 1% of Rengar's missing health)\"", "f3", "" });
			myList.Add(new List<string> { "Rengar", "2", "50, 100, 150, 200, 150", "f1", "" });
			myList.Add(new List<string> { "Xerath", "1", "60, 90, 120, 150, 180", "f1", "" });
			myList.Add(new List<string> { "Xerath", "1", "0.60", "f2", "spelldamage" });
			myList.Add(new List<string> { "Soraka", "1", "0.40", "f1", "spelldamage" });
			myList.Add(new List<string> { "MissFortune", "0", "0.85", "f1", "attackdamage" });
			myList.Add(new List<string> { "MissFortune", "0", "1.00", "f2", "attackdamage" });
			myList.Add(new List<string> { "MissFortune", "1", "0.06", "f1", "attackdamage" });
			myList.Add(new List<string> { "MissFortune", "1", "5", "f3", "" });
			myList.Add(new List<string> { "MissFortune", "1", "0.30", "f2", "attackdamage" });
			myList.Add(new List<string> { "Braum", "0", "0.025", "f1", "maxhealth" });
			myList.Add(new List<string> { "Twitch", "2", "0.25", "f1", "bonusattackdamage" });
			myList.Add(new List<string> { "MasterYi", "2", "0.10", "f1", "attackdamage" });
			myList.Add(new List<string> { "Taric", "0", "0.05", "f1", "bonushealth" });
			myList.Add(new List<string> { "Gangplank", "0", "0", "f2", "" }); //gold plundered will always be 0 for me
			myList.Add(new List<string> { "Kassadin", "3", "0.02", "f2", "@custom.percentOfMana" });
			myList.Add(new List<string> { "Kassadin", "3", "0.01", "f1", "@custom.percentOfMana" });
			myList.Add(new List<string> { "Zyra", "0", "70, 105, 140, 175, 210", "f3", "" });
			myList.Add(new List<string> { "Zyra", "2", "\"23 + (6.5 x level)\"", "f3", "" });
			myList.Add(new List<string> { "Nami", "1", "\"15% - (7.5% of AP)\"", "f1", "" });
			myList.Add(new List<string> { "Ezreal", "0", "1.10", "f1", "attackdamage" });






			//Execute the updates
			string newJson = "";
			foreach (List<string> subList in myList)
			{
				try
				{
					JObject jChampions = (JObject)jsonObject["data"];
					JObject jChampion = (JObject)jChampions[subList[0]];
					JArray jSpells = (JArray)jChampion["spells"];
					JObject jSpell = (JObject)jSpells[Convert.ToInt16(subList[1])]; //spells are numbered as follows: 0 for Q, 1 for W, 2 for E, 3 for R
					System.Diagnostics.Debug.WriteLine(subList[0] + " : " + subList[2]);
					string newVars = string.Format(@"{{
													""coeff"" : [
														{0}
													],
													""dyn"" : null,
													""key"" : ""{1}"",
													""link"" : ""{2}"",
													""ranksWith"" : null
											   }}", subList[2], subList[3], subList[4]);

					var jsonNewVars = JObject.Parse(newVars);

					//try first method of adding vars
					try
					{
						JArray jVars = (JArray)jSpell["vars"]; //This fails if vars = null
						jVars.Add(jsonNewVars);
					}
					//otherwise vars = null and must be replaced with JArray
					catch
					{
						jSpell.Remove("vars");
						jSpell.Add(new JProperty("vars", new JArray()));
						JArray jVars = (JArray)jSpell["vars"];
						jVars.Add(jsonNewVars);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
			newJson = jsonObject.ToString();
			return newJson;
		}


	}
}
