using DevExpress.XtraEditors;
using Newtonsoft.Json;
using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes
{
	[Serializable]
	public class GetAllVersionAvailable 
	{
		public List<string> versions = new List<string>();
		public Realm realm = null;

		public GetAllVersionAvailable() { }


		public static List<string> Convert(StringCollection collection)
		{
			List<string> list = new List<string>();
			foreach (string item in collection)
			{
				list.Add(item);
			}
			return list;
		}

		public bool CollectVersionData()
		{
			bool success = false;
			List<string> checkVersions = new List<string>();
			// Setup RiotApi
			var staticApi = StaticRiotApi.GetInstance(Resources.App.ApiKey);

			try
			{
				//Get all Version list and Realm
				versions = staticApi.GetVersions(RiotSharp.Region.na);
				realm = staticApi.GetRealm(RiotSharp.Region.na);
				success = true;
			}
			catch (Exception ex)
			{
				
				//TODO: correctly handle errors rather than this
				XtraMessageBox.Show(@"There was a problem downloading Versions from League of Legends.
								" + ex.Message + @"
								  " + ex.ToString(), "League Build Stats - Notice");
				//System.NullReferenceException means no interenet connection or something
			}
			if (success)
			{
				success = StoreRiotVersionData();
			}

			return success;
		}

		private bool StoreRiotVersionData()
		{

			bool success = false;
			try
			{
				string file = string.Format(@"{0}\Data\versions.bin", PublicStaticVariables.thisAppDataDir);
				string dir = string.Format(@"{0}\Data\", PublicStaticVariables.thisAppDataDir);

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
					serializer.Serialize(jw, versions);
				}

				file = string.Format(@"{0}\Data\realm.bin", PublicStaticVariables.thisAppDataDir);
				using (FileStream fs = File.Open(file, FileMode.Create))
				using (StreamWriter sw = new StreamWriter(fs))
				using (JsonWriter jw = new JsonTextWriter(sw))
				{
					jw.Formatting = Formatting.Indented;

					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(jw, realm);
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

		public bool LoadRiotVersionData()
		{
			bool success = false;
			try
			{
				//Versions ////////////////////////////////////////////////
				string file = string.Format(@"{0}/Data/versions.bin", PublicStaticVariables.thisAppDataDir);
				StreamReader re = new StreamReader(file);
				try
				{
					JsonTextReader reader = new JsonTextReader(re);

					JsonSerializer se = new JsonSerializer();
					object parsedData = se.Deserialize(reader);

					versions = JsonConvert.DeserializeObject<List<string>>(parsedData.ToString());

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

				//Realm //////////////////////////////////////////////////
				file = string.Format(@"{0}/Data/realm.bin", PublicStaticVariables.thisAppDataDir);
				re = new StreamReader(file);
				try
				{
					JsonTextReader reader = new JsonTextReader(re);

					JsonSerializer se = new JsonSerializer();
					object parsedData = se.Deserialize(reader);

					realm = JsonConvert.DeserializeObject<Realm>(parsedData.ToString());

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

	}
}
