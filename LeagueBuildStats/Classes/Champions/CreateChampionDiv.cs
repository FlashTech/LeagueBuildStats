using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSharp_LoLStats_WinForm.Classes.Champions
{
	[Serializable]
	public class CreateChampionDiv
	{
		private System.Collections.Generic.KeyValuePair<int, RiotSharp.StaticDataEndpoint.ItemStatic> thisItem;
		public System.Collections.Generic.KeyValuePair<int, RiotSharp.StaticDataEndpoint.ItemStatic> aItem
		{
			get
			{
				return this.thisItem;
			}
			set
			{
				this.thisItem = value;
			}
		}
		public string NewDesc;
		public string DivText;
		public string thisTags;
		public string thisVersion;
		public int thisID;
		public bool filteredOut;
		public string tooltipOfItem;
		public string htmlToolTipOfItem;

		public void SetupItemInformation(System.Collections.Generic.KeyValuePair<int, RiotSharp.StaticDataEndpoint.ItemStatic> inputItem, string version)
		{
			thisItem = inputItem;
			thisVersion = version;
			thisID = thisItem.Value.Id;

			//TODO: Note: this is a tester used to break the look and observe an items information
			if (thisItem.Value.Name.Contains("Alacrity"))
			{
				string temp = thisItem.Value.Description.ToString();
			}

			//Filter Out Items
			if (thisItem.Value.Name.Contains("Enchantment:"))
			{
				filteredOut = true;
			}
			if (thisItem.Value.Gold.Purchasable == false)
			{
				filteredOut = true;
			}

			if (thisItem.Value.Name.Contains("Bonetooth Necklace"))
			{
				filteredOut = true;
			}


			// store tags for this item
			if (thisItem.Value.Tags != null)
			{
				foreach (string tag in thisItem.Value.Tags)
				{
					thisTags += " " + tag;
				}
				if (!thisItem.Value.Tags.Contains("stealth") && (thisItem.Value.Name.Contains("Greater Vision Totem") || thisItem.Value.Name.Contains("Oracle's Lens")))
				{
					thisTags += " " + "Stealth";
				}
				if (!thisItem.Value.Tags.Contains("stealth") && (thisItem.Value.Description.Contains("stealth-detecting")))
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
			switch (thisItem.Value.Image.Sprite)
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

			//Update description to include if requires a champion or if requires a map
			string ItemDesc = thisItem.Value.Description.ToString();
			if (thisItem.Value.RequiredChampion != null)
			{
				ItemDesc += string.Format(@"<br/><br/> <font color=""#780000""> This item is only available on {0} </font>", thisItem.Value.RequiredChampion);
			}
			if (thisItem.Value.Maps != null)
			{
				string tempDescMaps = "";
				foreach (KeyValuePair<string, bool> map in thisItem.Value.Maps)
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
			string ItemID = "Item" + thisItem.Value.Id;



			//This section is the item image. A border around the image can be added by adding a border style to the div below.
			string divOfItem = string.Format(@"
				<span class=""tooltipWrapper{0}"">
					<a href=""#"" class=""tooltip"">
						<div ID=""{2}"" onmouseover=""OnHoverDiv('{1}')"" class=""itemImage"" style=""background-image: url('{3}'); background-position: -{4}px -{5}px;"">
						</div>"
				, thisTags					//0
				, thisItem.Value.Id			//1
				, ItemID					//2
				, imageUrl					//3
				, thisItem.Value.Image.X 	//4
				, thisItem.Value.Image.Y	//5
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
				, thisItem.Value.Image.X					//1
				, thisItem.Value.Image.Y					//2
				, thisItem.Value.Name						//3
				, thisItem.Value.Gold.TotalPrice.ToString()	//4
				, ItemDesc									//5
			);





			string tempDescription = NewDesc;

			tempDescription = tempDescription.Replace("<consumable>", "<font color='#CC3300'>"); //dark-ish red
			tempDescription = tempDescription.Replace("</consumable>", "</font>");

			tempDescription = tempDescription.Replace("<groupLimit>", "<font color='#FFFFFF'>"); //white
			tempDescription = tempDescription.Replace("</groupLimit>", "</font>");

			tempDescription = tempDescription.Replace("<stats>", "<font color='#66FF99'>"); //Light green
			tempDescription = tempDescription.Replace("</stats>", "</font>");

			tempDescription = tempDescription.Replace("<unique>", "<font color='#E6E600'>"); //dull yellow
			tempDescription = tempDescription.Replace("</unique>", "</font>");

			tempDescription = tempDescription.Replace("<active>", "<font color='#E6E600'>"); //dull yellow
			tempDescription = tempDescription.Replace("</active>", "</font>");

			tempDescription = tempDescription.Replace("<passive>", "<font color='#E6E600'>"); //dull yellow
			tempDescription = tempDescription.Replace("</passive>", "</font>");

			tempDescription = tempDescription.Replace("<aura>", "<font color='#E6E600'>"); //dull yellow
			tempDescription = tempDescription.Replace("</aura>", "</font>");


			//This had to be done on one line or else it causes problems
			//string styles = "<style>consumable{    color:#CC3300;/*dark-ish red*/}groupLimit{    color:#FFFFFF;/*white*/}stats{    color:#66FF99;/*Light green*/}unique{    color:#E6E600;/*dull yellow*/}active{    color:#E6E600;/*dull yellow*/}passive{    color:#E6E600;/*dull yellow*/}aura{    color:#E6E600;/*dull yellow*/}</style>";
			//htmlToolTipOfItem = string.Format(@"<html><head>{0}</head><body><font color='#FFFFFF'>{1}</font></body></html>", styles, ItemDesc);
			htmlToolTipOfItem = string.Format(@"<html><head></head><body>{0}</body></html>", tempDescription);


			DivText = divOfItem + "</a></span>";
		}
	}
}
