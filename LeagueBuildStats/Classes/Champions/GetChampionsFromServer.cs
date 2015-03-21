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
using RiotSharp;
using DevExpress.XtraEditors;
using System.Configuration;

namespace LeagueBuildStats.Classes.Champions
{
	[Serializable]
	public class GetChampionsFromServer
	{
		//todo: this doesn't need to be public
		public ChampionListStatic champions;
		public List<KeyValuePair<string, ChampionStatic>> championsOrder = new List<KeyValuePair<string, ChampionStatic>>();
		public string version = "";
		public static int Compare1(KeyValuePair<string, ChampionStatic> a, KeyValuePair<string, ChampionStatic> b)
		{
			return a.Key.CompareTo(b.Key);
		}

		public GetChampionsFromServer() { }

		public bool DownloadListOfChampions(string inputVersion = null)
		{
			bool success = false;
			try
			{
				// Setup RiotApi
				var staticApi = StaticRiotApi.GetInstance(Resources.App.ApiKey);

				//Get all Items
				if (inputVersion == null)
				{
					champions = staticApi.GetChampions(RiotSharp.Region.na, ChampionData.all);
				}
				else
				{
					champions = staticApi.GetChampions(RiotSharp.Region.na, inputVersion, ChampionData.all);
				}

				StoreRiotChampionData(champions);

				//This action happens almost isntantly
				//SortRiotChampionData(champions); //Todo: this is disabled because LoadRiotChampionData() calls this.


				//TODO: for now i am loading the data in order to cause ChampionDataCorrections
				LoadRiotChampionData(champions.Version);

				
				success = true;
			}
			catch (Exception ex)
			{
				//TODO: correctly handle errors rather than this
				XtraMessageBox.Show(
string.Format(@"pvp.net/api/lol {1}
Note: This error may happen when selecting versions below 3.7.1", ex.Message), "League Build Stats - Notice");
				success = false;
			}


			return success;
		}



		public bool StoreRiotChampionData(ChampionListStatic champions)
		{
			bool success = false;
			try
			{
				string file = string.Format(@"{0}\Data\Champions\getChampions.{1}.bin", PublicStaticVariables.thisAppDataDir, champions.Version);
				string dir = string.Format(@"{0}\Data\Champions", PublicStaticVariables.thisAppDataDir);

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				using (FileStream fs = File.Open(file, FileMode.Create))
				using (StreamWriter sw = new StreamWriter(fs))
				using (JsonWriter jw = new JsonTextWriter(sw))
				{
					jw.Formatting = Formatting.Indented;

					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(jw, champions);
				}
				success = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				success = false;
			}
			return success;
		}

		public void SortRiotChampionData(ChampionListStatic champions)
		{
			//Sort Champions by name
			championsOrder = new List<KeyValuePair<string, ChampionStatic>>();
			foreach (KeyValuePair<string, ChampionStatic> i in champions.Champions)
			{
				championsOrder.Add(new KeyValuePair<string, ChampionStatic>(i.Key, i.Value));
			}


			championsOrder.Sort((firstPair, nextPair) =>
			{
				return firstPair.Key.CompareTo(nextPair.Key);
			}
			);
			version = champions.Version;
		}

		public bool LoadRiotChampionData(string version)
		{
			bool success = false;
			try
			{
				string file;
				file = string.Format(@"{0}\Data\Champions\getChampions.{1}.bin", PublicStaticVariables.thisAppDataDir, version);


				string dir = string.Format(@"{0}\Data\Champions", PublicStaticVariables.thisAppDataDir);

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				StreamReader re = new StreamReader(file);
				try
				{
					JsonTextReader reader = new JsonTextReader(re);

					JsonSerializer se = new JsonSerializer();
					object parsedData = se.Deserialize(reader);

					//Todo: this action should be timed
					string jsonData = ChampionDataCorrections.RunCorrections(parsedData.ToString(), version);

					champions = JsonConvert.DeserializeObject<ChampionListStatic>(jsonData);

					champions = ChampionDataCorrections.RunStatCorrections(champions, version);


					SortRiotChampionData(champions);

					//TODO: temp test
					//FindMissingParamsInSpells(champions);
					//FindMissingBaseAttackSpeeds(champions);

					success = true;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					success = false;
				}
				finally
				{
					re.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				success = false;
			}
			return success;
		}

		public void FindMissingBaseAttackSpeeds(ChampionListStatic champions)
		{
			List<string> tempChampsMissingBaseAS = new List<string>();
			foreach (ChampionStatic thisChamp in champions.Champions.Values)
			{
				if (thisChamp.Stats.AttackSpeedOffset == 0.0)
				{
					tempChampsMissingBaseAS.Add(thisChamp.Name);
				}
			}
			tempChampsMissingBaseAS.Add("done");
			string all = "";
			foreach (string s in tempChampsMissingBaseAS)
			{
				all += s + ",";
			}
			all += "done";
		}


		/// <summary>
		/// TEMP TESTER FUNCTION
		/// </summary>
		/// <param name="champions"></param>
		public void FindMissingParamsInSpells(ChampionListStatic champions)
		{
			List<string> tempMissingResource = new List<string>();
			List<string> links = new System.Collections.Generic.List<string>();
			foreach (ChampionStatic thisChamp in champions.Champions.Values)
			{
				foreach (ChampionSpellStatic champSpell in thisChamp.Spells)
				{
					//Store unique Links
					if (champSpell.Vars != null)
					{
						foreach (var coeff in champSpell.Vars)
						{
							if (coeff.Link != null)
							{
								if (!links.Contains(coeff.Link))
								{
									links.Add(coeff.Link);
								}
							}
						}
					}



					//Prepare Desc//
					string sDescPrep = champSpell.Tooltip;


					//Replace Ceoff markers
					if (champSpell.Vars != null)
					{
						foreach (SpellVarsStatic spellVars in champSpell.Vars)
						{
							string sTarget = "{{ " + spellVars.Key + " }}";
							string sReplacement = "";
							if (spellVars.Coeff != null)
							{
								var varsCoeff = (Newtonsoft.Json.Linq.JArray)spellVars.Coeff;
								int index = 0;
								int icount = varsCoeff.Count;
								foreach (string sVar in varsCoeff)
								{
									sReplacement += sVar;
									if ((index + 1) != icount)
									{
										sReplacement += "/";
									}
									index++;
								}
							}
							sDescPrep = sDescPrep.Replace(sTarget, sReplacement);
						}
					}

					//Replace effect markers
					if (champSpell.EffectBurns != null)
					{
						int i = 0;
						foreach (string sReplacement in champSpell.EffectBurns)
						{
							if (sReplacement != "")
							{
								string sTarget = "{{ e" + i + " }}";
								sDescPrep = sDescPrep.Replace(sTarget, sReplacement);
							}
							i++;
						}
					}


					//TODO: Check for missing params Uncomment lines below to reactivate
					List<string> tempMissingParams = new List<string>();
					if (sDescPrep.Contains("{{"))
					{
						MatchCollection matchList = Regex.Matches(sDescPrep, "{{ [^}}]+ }}");
						var list = matchList.Cast<Match>().Select(match => match.Value).ToList();

						foreach (string sMatch in list)
						{
							if (!tempMissingParams.Contains(sMatch))
							{
								tempMissingParams.Add(sMatch);
							}
						}


						//MessageBox.Show("Champion is missing params!");
						//using (System.IO.StreamWriter fileWrite = new System.IO.StreamWriter(string.Format(@"{0}\WriteLines3.txt", "E:\\VisualStudioProjects2013-Latest\\RiotSharp-LoLStats-Web\\RiotSharp-LoLStats-WinForm\\bin\\Debug"), true))
						//{
						//	fileWrite.WriteLine(string.Format(@" * Champion Name: {0} (ID: {1})", thisChamp.Name, thisChamp.Id));
						//	string missingParams = "";
						//	foreach (string sMissing in tempMissingParams)
						//	{
						//		missingParams += sMissing + " ";
						//	}
						//	fileWrite.WriteLine(string.Format(@"  * Ability Name: {0}", champSpell.Name));
						//	fileWrite.WriteLine(string.Format(@"  * Ability Key: {0}", champSpell.Key));
						//	fileWrite.WriteLine(string.Format(@"  * Params Missing from Tooltip: {0}", missingParams));
						//	fileWrite.WriteLine(string.Format(@""));
						//	fileWrite.WriteLine(string.Format(@""));
						//}
					}


				}
			}

			//using (System.IO.StreamWriter fileWrite = new System.IO.StreamWriter(string.Format(@"{0}\WriteLines3.txt", "E:\\VisualStudioProjects2013-Latest\\RiotSharp-LoLStats-Web\\RiotSharp-LoLStats-WinForm\\bin\\Debug"), true))
			//{
			//	foreach (string st in links)
			//	{
			//		fileWrite.WriteLine(st);
			//	}
			//}
			int d = 2;
			d++;
		}

		public bool GetChampionImages()
		{
			bool success = false;
			try
			{
				string version = champions.Version;
				string file = string.Format(@"{0}\Data\Champions\Images\{1}\", PublicStaticVariables.thisAppDataDir, version);

				if (!Directory.Exists(file))
				{
					Directory.CreateDirectory(file);
				}

				List<string> spriteList = new List<string>();
				foreach (KeyValuePair<string, ChampionStatic> champ in champions.Champions)
				{
					if (!spriteList.Contains(champ.Value.Image.Sprite))
					{
						spriteList.Add(champ.Value.Image.Sprite);
					}
					if (!spriteList.Contains(champ.Value.Passive.Image.Sprite))
					{
						spriteList.Add(champ.Value.Passive.Image.Sprite);
					}
					if (!spriteList.Contains(champ.Value.Spells[0].Image.Sprite))
					{
						spriteList.Add(champ.Value.Spells[0].Image.Sprite);
					}
					if (!spriteList.Contains(champ.Value.Spells[1].Image.Sprite))
					{
						spriteList.Add(champ.Value.Spells[1].Image.Sprite);
					}
					if (!spriteList.Contains(champ.Value.Spells[2].Image.Sprite))
					{
						spriteList.Add(champ.Value.Spells[2].Image.Sprite);
					}
					if (!spriteList.Contains(champ.Value.Spells[3].Image.Sprite))
					{
						spriteList.Add(champ.Value.Spells[3].Image.Sprite);
					}
				}
				foreach (string sprite in spriteList)
				{
					string imageURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/sprite/" + sprite;
					CommonMethods.DownloadRemoteImageFile(imageURL, file + sprite);
				}


				success = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				success = false;
			}
			return success;
		}


	}
}
