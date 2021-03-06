﻿using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueBuildStats.Classes.General_Classes;
using LeagueBuildStats.Classes;
using System.Text.RegularExpressions;

namespace LeagueBuildStats
{
	[Serializable]
	public class CreateItemDiv
	{
		public ItemStatic aItem
		{
			get;
			set;
		}
		public string NewDesc;
		public string DivText;
		public string thisTags;
		public string thisVersion;
		public int thisID;
		public bool filteredOut;
		public string tooltipOfItem;
		public string htmlToolTipOfItem;
		public string thisItemDisplayName;


		public void SetupItemInformation(ItemStatic inputItem, string version, ItemStatic enchantBaseItem = null)
		{
			thisVersion = version;
			thisID = inputItem.Id;


			//Filter Out Items
			if (inputItem.Name.Contains("Enchantment:"))
			{
				filteredOut = true;
			}
			if (inputItem.Gold.Purchasable == false)
			{
				filteredOut = true;
			}

			if (inputItem.Name.Contains("Bonetooth Necklace"))
			{
				filteredOut = true;
			}


			// store tags for this item
			if (inputItem.Tags != null)
			{
				foreach (string tag in inputItem.Tags)
				{
					thisTags += " " + tag;
				}
				if (!inputItem.Tags.Contains("stealth") && (inputItem.Name.Contains("Greater Vision Totem") || inputItem.Name.Contains("Oracle's Lens")))
				{
					thisTags += " " + "Stealth";
				}
				if (!inputItem.Tags.Contains("stealth") && (inputItem.Description.Contains("stealth-detecting")))
				{
					thisTags += " " + "Stealth";
				}
			}

			if (thisTags == null)
			{
				//If there are no tags then store this information
				thisTags += " " + "noTag";
			}

			//Select the appropriate image for the item
			string imageUrl = string.Format(@"http://ddragon.leagueoflegends.com/cdn/{0}/img/sprite/item0.png", thisVersion); ;
			switch (inputItem.Image.Sprite)
			{
				case "item0.png":
					imageUrl = string.Format(@"http://ddragon.leagueoflegends.com/cdn/{0}/img/sprite/item0.png", thisVersion);
					break;
				case "item1.png":
					imageUrl = string.Format(@"http://ddragon.leagueoflegends.com/cdn/{0}/img/sprite/item1.png", thisVersion); ;
					break;
				case "item2.png":
					imageUrl = string.Format(@"http://ddragon.leagueoflegends.com/cdn/{0}/img/sprite/item2.png", thisVersion); ;
					break;
			}

			//Fix enchantment description and name
			thisItemDisplayName = inputItem.Name;
			string ItemDesc = inputItem.Description.ToString();
			if (enchantBaseItem != null)
			{
				if (enchantBaseItem.Tags.Contains("Boots"))
				{
					ItemDesc = enchantBaseItem.Description + "<br/><br/>" + ItemDesc;
				}
				else
				{
					ItemDesc += "<br/><br/>" + enchantBaseItem.Description;
				}
				//Todo, 
				inputItem.Description = ItemDesc;
				inputItem.Name = enchantBaseItem.Name + " - " + thisItemDisplayName.After("Enchantment: ");
				thisItemDisplayName = enchantBaseItem.Name + " - " + thisItemDisplayName.After("Enchantment: ");
			}



			//Update description to include if requires a champion or if requires a map
			if (inputItem.RequiredChampion != null)
			{
				ItemDesc += string.Format(@"<br/><br/> <font color=""#780000""> This item is only available on {0} </font>", inputItem.RequiredChampion);
			}
			if (inputItem.Maps != null)
			{
				string tempDescMaps = "";
				foreach (KeyValuePair<string, bool> map in inputItem.Maps)
				{
					if (map.Value == false)
					{
						string mapName = "";
						foreach (KeyValuePair<string, string> mapID in PublicStaticVariables.StaticMapIDNames)
						{
							if (mapID.Key == map.Key) mapName = mapID.Value;
						}
						tempDescMaps += string.Format(@"&nbsp;{0} <br/>", mapName);
					}
				}
				if (tempDescMaps != "")
				{
					tempDescMaps = tempDescMaps.Substring(0, tempDescMaps.Length - 6);
					ItemDesc += "<br/><br/> <font color=\"#B69B30\"> This item is not available on the following maps: <br/>" + tempDescMaps + "</font>";
				}
			}


			//Stores the new Item Description
			while (ItemDesc.EndsWith("<br/>") || ItemDesc.EndsWith("<br>"))
			{
				if (ItemDesc.EndsWith("<br/>"))
				{
					ItemDesc = ItemDesc.Substring(0, ItemDesc.Length - 5);
				}
				else
				{
					ItemDesc = ItemDesc.Substring(0, ItemDesc.Length - 4);
				}
			}
			NewDesc = ItemDesc + "<br/>";

			//Only actually render the item if it has not been filtered out
			string ItemID = "Item" + inputItem.Id;



			//This section is the item image. A border around the image can be added by adding a border style to the div below.
			string divOfItem = string.Format(@"
				<span class=""tooltipWrapper{0}"">
					<a href=""#"" class=""tooltip"">
						<div ID=""{2}"" onmouseover=""OnHoverDiv('{1}')"" class=""itemImage"" style=""background-image: url('{3}'); background-position: -{4}px -{5}px;"">
						</div>"
				, thisTags					//0
				, inputItem.Id			//1
				, ItemID					//2
				, imageUrl					//3
				, inputItem.Image.X 	//4
				, inputItem.Image.Y	//5
			);

			//This section is the tooltip image and information.
			tooltipOfItem = string.Format(@"
						<span class=""itemToolTip"">
							<div class=""itemImage"" style=""background-image: url('{0}'); background-position: -{1}px -{2}px;"">
							</div>
							<div class=""itemToolTipSpacer"">&nbsp;</div>
							<span class=""itemToolTipTitle"">{3}</span> <br/>
							<br />
							Cost: <span class=""itemToolTipGold"">{4}</span> <br />
							<br />
							{5} </i></i></i>
						</span>"
				//Note: The </i></i></i> above is a percaution because I found unclosed <i> in Descriptions that broke everything if not closed.
				, imageUrl									//0
				, inputItem.Image.X					//1
				, inputItem.Image.Y					//2
				, thisItemDisplayName								//3
				, inputItem.Gold.TotalPrice.ToString()	//4
				, ItemDesc									//5
			);





			string tempDescription = NewDesc;

			tempDescription = tempDescription.Replace("<consumable>", "<span style='color:#CC3300;'>");//"<font color='#CC3300'>"); //dark-ish red
			tempDescription = tempDescription.Replace("</consumable>", "</span>");//"</font>");

			tempDescription = tempDescription.Replace("<groupLimit>", "<span style='color:#FFFFFF;'>");//"<font color='#FFFFFF'>"); //white
			tempDescription = tempDescription.Replace("</groupLimit>", "</span>");//"</font>");

			tempDescription = tempDescription.Replace("<stats>", "<span style='color:#66FF99;'>");//"<font color='#66FF99'>"); //Light green
			tempDescription = tempDescription.Replace("</stats>", "</span>");//"</font>");

			tempDescription = tempDescription.Replace("<unique>", "<span style='color:#E6E600;'>"); //dull yellow
			tempDescription = tempDescription.Replace("</unique>", "</span>");

			tempDescription = tempDescription.Replace("<active>", "<span style='color:#E6E600;'>"); //dull yellow
			tempDescription = tempDescription.Replace("</active>", "</span>");

			tempDescription = tempDescription.Replace("<passive>", "<span style='color:#E6E600;'>"); //dull yellow
			tempDescription = tempDescription.Replace("</passive>", "</span>");

			tempDescription = tempDescription.Replace("<aura>", "<span style='color:#E6E600;'>"); //dull yellow
			tempDescription = tempDescription.Replace("</aura>", "</span>");

			tempDescription = tempDescription.Replace("<mana>", "");
			tempDescription = tempDescription.Replace("</mana>", "");

			if (Regex.Matches(tempDescription, "<span>").Count != Regex.Matches(tempDescription, "</span>").Count)
			{
				tempDescription = SpanFixer(tempDescription);
			}

			//Fix all of Riot's ill-formaed html
			tempDescription = tempDescription.Replace("<br>", "<br/>");
			int iCount = Regex.Matches(tempDescription, "<i>").Count;
			while (Regex.Matches(tempDescription, "</i>").Count < iCount)
			{
				tempDescription += "</i>";
			}
			if (Regex.Matches(tempDescription, "</i>").Count > iCount)
			{
				tempDescription = SubstringExtensions.ReplaceLastOccurrence(tempDescription, "</i>", "");
			}

			//This had to be done on one line or else it causes problems
			htmlToolTipOfItem = string.Format(@"<html><head></head><body>{0}</body></html>", tempDescription);



			DivText = StringExtensions.BrWrapper(tempDescription);

			aItem = inputItem;
		}


		private string SpanFixer(string tempDescription)
		{
			MatchCollection spansFound = Regex.Matches(tempDescription, "</span>|<span");
			int ind = 0;
			foreach (Match match in spansFound)
			{
				if (ind % 2 == 0)
				{
					if (match.Value == "</span>")
					{
						//problem
						string begin = tempDescription.Substring(0, match.Index);
						string end = tempDescription.Substring(match.Index + match.Length, tempDescription.Length - (match.Index + match.Length));
						tempDescription = begin + end;
						SpanFixer(tempDescription);
						return tempDescription;
					}
				}
				else
				{
					//Todo: not sure i should check for an unexpected open <span>
				}
				ind++;
			}
			return tempDescription;
		}



	}
}
