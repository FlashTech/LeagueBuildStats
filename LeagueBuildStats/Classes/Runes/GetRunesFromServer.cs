using Newtonsoft.Json;
using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeagueBuildStats.Classes.General_Classes;

namespace LeagueBuildStats.Classes.Runes
{
	[Serializable]
	public class GetRunesFromServer
	{
		public RuneListStatic runes;
		public List<KeyValuePair<int,RuneStatic>> runeSorted;
		public string version;
		public GetRunesFromServer() { }

		public bool DownloadListOfRunes(string inputVersion = null)
		{
			
			bool success = false;
			// Get all Items for NA
			try
			{
				// Setup RiotApi
				var staticApi = StaticRiotApi.GetInstance("b649d183-d319-4bda-95fb-1faadfa1966d");

				//Get all Items
				if (inputVersion == null)
				{
					runes = staticApi.GetRunes(RiotSharp.Region.na, RuneData.all);
				}
				else
				{
					runes = staticApi.GetRunes(RiotSharp.Region.na, inputVersion, RuneData.all);
				}

				StoreRiotRuneData(runes);

				SortRiotRuneData(runes);

				success = true;
			}
			catch (Exception ex)
			{
				//TODO: correctly handle errors rather than this
				MessageBox.Show(ex.ToString());
				success = false;
			}
			return success;
		}

		private bool StoreRiotRuneData(RuneListStatic runes)
		{
			bool success = false;
			try
			{
				string file = string.Format(@"{0}\Data\Runes\getRunes.{1}.bin", PublicStaticVariables.thisAppDataDir, runes.Version);
				string dir = string.Format(@"{0}\Data\Runes", PublicStaticVariables.thisAppDataDir);

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
					serializer.Serialize(jw, runes);
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

		private void SortRiotRuneData(RuneListStatic runes)
		{
			//This was the only way I could seem to filter out the random rare runes
			RuneListStatic runesThinned = new RuneListStatic();
			runesThinned.Version = runes.Version;
			runesThinned.BasicData = runes.BasicData;
			runesThinned.Type = runes.Type;
			runesThinned.Runes = new Dictionary<int, RuneStatic>();
			foreach (KeyValuePair<int, RuneStatic> iRune in runes.Runes)
			{
				string iName = iRune.Value.Image.Full;
				if (iName.StartsWith("r") || iName.StartsWith("y") || iName.StartsWith("b"))
				{
					runesThinned.Runes.Add(iRune.Key, iRune.Value);
				}
			}
			this.runes = runesThinned;

			//Sort
			runeSorted = new List<KeyValuePair<int, RuneStatic>>();
			foreach (RuneStatic runeS in this.runes.Runes.Values)
			{
				runeSorted.Add(new KeyValuePair<int, RuneStatic>(runeS.Id, runeS));
			}
			runeSorted.Sort((firstPair, nextPair) =>
			{
				return firstPair.Key.CompareTo(nextPair.Key);
			}
			);

			//Temp function to generate list keyvalue pair used in runeStatic
			//GenerateRuneDictionaryList(runeSorted);
					
			this.version = runes.Version;
		}

		private void GenerateRuneDictionaryList(List<KeyValuePair<int, RuneStatic>> runeSorted)
		{
			//Generates keyvaluepair used in runeStatic
			Dictionary<string, int> listOfNames = new Dictionary<string, int>();
			//TEMP
			string textAll = "";
			foreach (KeyValuePair<int, RuneStatic> a in runeSorted)
			{
				Type myType = typeof(StatsStatic);
				System.Reflection.PropertyInfo[] properties = myType.GetProperties();
				string thisStatName = "";
				foreach (System.Reflection.PropertyInfo property in properties)
				{
					Double statValue = (Double)a.Value.Stats.GetType().GetProperty(property.Name).GetValue(a.Value.Stats);
					if (statValue != 0.0)
					{
						thisStatName = property.Name;
					}
				}

				int test;
				if (!listOfNames.TryGetValue(thisStatName, out test))
				{

					listOfNames.Add(thisStatName, 1);

					string valueName = a.Value.Name.After(" of ");
					if (valueName.Contains("Scaling"))
					{
						valueName = valueName.After("Scaling ");
						valueName += " per Level";
					}
					if (a.Value.Description.Contains("%"))
					{
						thisStatName = "%" + thisStatName;
					}

					textAll += string.Format(@"new KeyValuePair<string, string>('{0}'', '{1}'), replace ", thisStatName, valueName);
				}
			}
		}


		internal bool LoadRiotRuneData(string version)
		{
			bool success = false;
			try
			{
				string file;
				file = string.Format(@"{0}\Data\Runes\getRunes.{1}.bin", PublicStaticVariables.thisAppDataDir, version);


				string dir = string.Format(@"{0}\Data\Runes", PublicStaticVariables.thisAppDataDir);

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

					RuneListStatic runes = JsonConvert.DeserializeObject<RuneListStatic>(parsedData.ToString());

					SortRiotRuneData(runes);

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

		public bool GetRunesImages()
		{
			bool success = false;
			try
			{
				string version = runes.Version;
				string file = string.Format(@"{0}\Data\Runes\Images\{1}\", PublicStaticVariables.thisAppDataDir, version);

				if (!Directory.Exists(file))
				{
					Directory.CreateDirectory(file);
				}

				List<string> spriteList = new List<string>();
				List<string> quintFullImageList = new List<string>();
				foreach (KeyValuePair<int, RuneStatic> rune in runes.Runes)
				{
					if (!spriteList.Contains(rune.Value.Image.Sprite))
					{
						spriteList.Add(rune.Value.Image.Sprite);
					}
					if (rune.Value.Metadata.Type == "black")
					{
						quintFullImageList.Add(rune.Value.Image.Full);
					}
				}
				foreach (string sprite in spriteList)
				{
					string imageURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/sprite/" + sprite;
					CommonMethods.DownloadRemoteImageFile(imageURL, file + sprite);
				}
				foreach (string fullImage in quintFullImageList)
				{
					string imageURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/rune/" + fullImage;
					CommonMethods.DownloadRemoteImageFile(imageURL, file + fullImage);
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
