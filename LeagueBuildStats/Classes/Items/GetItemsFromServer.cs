using Newtonsoft.Json;
using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace LeagueBuildStats.Classes.Items
{
	[Serializable]
	public class GetItemsFromServer
	{
		

		public List<KeyValuePair<int, int>> itemGold_Names = new List<KeyValuePair<int, int>>();
		public List<CreateItemDiv> itemsPrepared = new List<CreateItemDiv>();
		public string version = "";

		static int Compare2(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
		{
			return a.Value.CompareTo(b.Value);
		}

		public GetItemsFromServer() { }

		public bool DownloadListOfItems(string inputVersion = null)
		{
			bool success = false;
			try
			{
				// Setup RiotApi
				var staticApi = StaticRiotApi.GetInstance(Resources.App.ApiKey);
				ItemListStatic items;
				//Get all Items
				if (inputVersion == null)
				{
					items = staticApi.GetItems(RiotSharp.Region.na, ItemData.all);
				}
				else
				{
					items = staticApi.GetItems(RiotSharp.Region.na, inputVersion, ItemData.all);
				}

				StoreRiotItemData(items);

				SortRiotItemData(items);
				
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

		public bool StoreRiotItemData(ItemListStatic items)
		{
			bool success = false;
			try
			{
				string file = string.Format(@"{0}\Data\Items\getItems.{1}.bin", PublicStaticVariables.thisAppDataDir, items.Version);
				string dir = string.Format(@"{0}\Data\Items", PublicStaticVariables.thisAppDataDir);

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
					serializer.Serialize(jw, items);
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

		public void SortRiotItemData(ItemListStatic items)
		{
			itemGold_Names.Clear();
			//Sort items by price
			foreach (System.Collections.Generic.KeyValuePair<int, RiotSharp.StaticDataEndpoint.ItemStatic> i in items.Items)
			{
				//Removes rengar upgraded trinket from cluttering the Item list, the base rengar tricket desciption has all information needed
				if (!(i.Value.Gold.Purchasable == false && i.Value.RequiredChampion == "Rengar"))
				{
					//Removes the explorer ward from the item list, it might have been available in the past but it doesn't matter much. Same with golden transcendence
					if (i.Value.Name != "Explorer's Ward" && i.Value.Name != "Golden Transcendence")
					{
						//Remove the duplicate free Buiscuit that used to be obtained from mastery, either way this item doesn't matter much
						if (!(i.Value.Name == "Total Biscuit of Rejuvenation" && i.Value.Gold.TotalPrice == 0))
						{
							//Add Stealth Detection tag where i think it is missing
							if (i.Value.Description.Contains("Vision Ward"))
							{
								if (i.Value.Tags == null)
								{
									i.Value.Tags.Add("Stealth");
								}
								else if (!i.Value.Tags.Contains("Stealth"))
								{
									i.Value.Tags.Add("Stealth");
								}
							}
							itemGold_Names.Add(new KeyValuePair<int, int>(i.Value.Id, i.Value.Gold.TotalPrice));
						}
					}
				}
			}

			

			itemGold_Names.Sort(Compare2);

			itemsPrepared.Clear();
			//Loop through all sorted item names and store item data
			foreach (var item in itemGold_Names)
			{
				ItemStatic i = items.Items[item.Key];

				int enchantBaseId = 0;
				ItemStatic enchantBaseItem;
				CreateItemDiv newItem = new CreateItemDiv();
				if (i.Name.Contains("Enchantment:"))
				{
					enchantBaseId = Convert.ToInt16(i.From[0]);
					items.Items.TryGetValue(enchantBaseId, out enchantBaseItem);
					newItem.SetupItemInformation(i, items.Version, enchantBaseItem);
				}
				else
				{
					newItem.SetupItemInformation(i, items.Version);

				}
				itemsPrepared.Add(newItem);


			}

			ItemDataCorrections.RunCorrections(items);

			
			version = items.Version;
		}

		public bool LoadRiotItemData(string version)
		{
			bool success = false;
			try
			{
				string file;
				file = string.Format(@"{0}\Data\Items\getItems.{1}.bin", PublicStaticVariables.thisAppDataDir, version);


				string dir = string.Format(@"{0}\Data\Items", PublicStaticVariables.thisAppDataDir);

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

					ItemListStatic items = JsonConvert.DeserializeObject<ItemListStatic>(parsedData.ToString());

					SortRiotItemData(items);

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
				if (ex.Message.Contains("Could not find file") && ex.Message.Contains("getItems"))
				{
					//TODO: This exception usually means the data are not on the computer and will be downloaded
				}
				else
				{
					MessageBox.Show(ex.ToString());
				}			
				success = false;
			}
			return success;
		}

		public bool GetItemImages()
		{
			bool success = false;
			try
			{
				string version = itemsPrepared[0].thisVersion;
				string file = string.Format(@"{0}\Data\Items\Images\{1}\", PublicStaticVariables.thisAppDataDir, version);

				if (!Directory.Exists(file))
				{
					Directory.CreateDirectory(file);
				}


				List<string> spriteList = new List<string>();
				foreach (CreateItemDiv citem in itemsPrepared)
				{
					if (!spriteList.Contains(citem.aItem.Image.Sprite))
					{
						spriteList.Add(citem.aItem.Image.Sprite);
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
