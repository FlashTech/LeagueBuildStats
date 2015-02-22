using Newtonsoft.Json;
using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes.Masteries
{
	[Serializable]
	public class GetMasteriesFromServer
	{
		public MasteryListStatic masteries;
		public string version;
		public GetMasteriesFromServer() { }

		public bool DownloadListOfMasteries(string inputVersion = null)
		{
			
			bool success = false;
			// Get all Items for NA
			try
			{
				// Setup RiotApi
				var staticApi = StaticRiotApi.GetInstance(Resources.App.ApiKey);

				//Get all Items
				if (inputVersion == null)
				{
					masteries = staticApi.GetMasteries(RiotSharp.Region.na, MasteryData.all);
				}
				else
				{
					masteries = staticApi.GetMasteries(RiotSharp.Region.na, inputVersion, MasteryData.all);
				}

				version = masteries.Version;

				StoreRiotMasteryData(masteries);

				MasteryDataCorrections.RunCorrections(masteries);

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

		private bool StoreRiotMasteryData(MasteryListStatic masteries)
		{
			bool success = false;
			try
			{
				string file = string.Format(@"{0}\Data\Masteries\getMasteries.{1}.bin", PublicStaticVariables.thisAppDataDir, masteries.Version);
				string dir = string.Format(@"{0}\Data\Masteries", PublicStaticVariables.thisAppDataDir);

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
					serializer.Serialize(jw, masteries);
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

		internal bool LoadRiotMasteryData(string version)
		{
			bool success = false;
			try
			{
				string file;
				file = string.Format(@"{0}\Data\Masteries\getMasteries.{1}.bin", PublicStaticVariables.thisAppDataDir, version);


				string dir = string.Format(@"{0}\Data\Masteries", PublicStaticVariables.thisAppDataDir);

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

					masteries = JsonConvert.DeserializeObject<MasteryListStatic>(parsedData.ToString());

					this.version = masteries.Version;

					MasteryDataCorrections.RunCorrections(masteries);

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

		public bool GetMasteriesImages()
		{
			bool success = false;
			try
			{
				string version = masteries.Version;
				string file = string.Format(@"{0}\Data\Masteries\Images\{1}\", PublicStaticVariables.thisAppDataDir, version);

				if (!Directory.Exists(file))
				{
					Directory.CreateDirectory(file);
				}

				List<string> spriteList = new List<string>();
				List<string> quintFullImageList = new List<string>();
				foreach (KeyValuePair<int, MasteryStatic> mastery in masteries.Masteries)
				{
					if (!spriteList.Contains(mastery.Value.Image.Sprite))
					{
						spriteList.Add(mastery.Value.Image.Sprite);
					}
				}
				foreach (string sprite in spriteList)
				{
					string imageURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/sprite/" + sprite;
					CommonMethods.DownloadRemoteImageFile(imageURL, file + sprite);

					imageURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/sprite/" + "gray_" + sprite;
					CommonMethods.DownloadRemoteImageFile(imageURL, file + "gray_" + sprite);
				}

				//background image
				file = string.Format(@"{0}\Data\Masteries\Images\{1}\masteryback.jpg", PublicStaticVariables.thisAppDataDir, version);
				CommonMethods.DownloadRemoteImageFile("http://ddragon.leagueoflegends.com/cdn/img/mastery/masteryback.jpg", file);

				//Common images
				file = string.Format(@"{0}\Data\Masteries\Images\{1}\mastersprite.png", PublicStaticVariables.thisAppDataDir, version);
				CommonMethods.DownloadRemoteImageFile("http://ddragon.leagueoflegends.com/cdn/img/global/mastersprite.png", file);

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
