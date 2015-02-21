﻿using Newtonsoft.Json.Linq;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace LeagueBuildStats.Classes.Champions
{
	class ChampionDataCorrections
	{

		internal static string RunCorrections(string jsonString, string version)
		{
			var jsonObject = JObject.Parse(jsonString);

			//Load xml file with all Champion corrections
			XmlDocument xdcDocument = new XmlDocument();
			string result = string.Empty;
			using (Stream stream = typeof(ChampionDataCorrections).Assembly.GetManifestResourceStream("LeagueBuildStats.Classes.Champions" + ".ChampionCorrectionList.xml"))
			{
				using (StreamReader sr = new StreamReader(stream))
				{
					result = sr.ReadToEnd();
				}
			}
			xdcDocument.LoadXml(result);

			XmlElement xelRoot = xdcDocument.DocumentElement;
			XmlNodeList xnlNodes = xelRoot.SelectNodes("/XML/CorrectionList[@Version]");

			string newJson = "";
			//Loop through each Champion Correction List Node
			foreach (XmlNode xndNode in xnlNodes)
			{
				string xmlVersion = xndNode.Attributes["Version"].Value;
				//If the Version Attribute is Greater or Equal than use this Correction List Node
				if (CheckIfVersionIsGreaterEqual(version, xmlVersion))
				{
					//Loop through each Ability Correction Node
					foreach (XmlNode champNode in xndNode)
					{
						//Execute the updates
						try
						{ //<Ability champion="Thresh" button="e" buttonNum="2" key="f1" coeff="&quot;(num of souls)&quot;" link=""/>
							string champion = champNode.Attributes["champion"].Value;
							string button = champNode.Attributes["button"].Value;
							string buttonNum = champNode.Attributes["buttonNum"].Value;
							string key = champNode.Attributes["key"].Value;
							string coeff = champNode.Attributes["coeff"].Value;
							string link = champNode.Attributes["link"].Value;

							JObject jChampions = (JObject)jsonObject["data"];
							if ((JObject)jChampions[champion] != null) //If the champion exists in the data set
							{
								JObject jChampion = (JObject)jChampions[champion];
								JArray jSpells = (JArray)jChampion["spells"];
								JObject jSpell = (JObject)jSpells[Convert.ToInt16(buttonNum)]; //spells are numbered as follows: 0 for Q, 1 for W, 2 for E, 3 for R
								string newVars = string.Format(@"{{
												""coeff"" : [
													{0}
												],
												""dyn"" : null,
												""key"" : ""{1}"",
												""link"" : ""{2}"",
												""ranksWith"" : null
											}}", coeff, key, link);

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
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.ToString());
						}
					}
				}
			}
			newJson = jsonObject.ToString();
			return newJson;
		}


		private static bool CheckIfVersionIsGreaterEqual(string version, string p)
		{
			//Setup i to be the lowest counter of version delimiters like 2.21.1 over 3.20.4.1235 would be 3
			string[] splitVersion = version.Split('.');
			string[] splitP = p.Split('.');
			int splitVerCount = 0;
			int splitPCount = 0;
			foreach (string s in splitVersion)
			{
				splitVerCount += 1;
			}
			foreach (string s in splitP)
			{
				splitPCount += 1;
			}
			int count = splitPCount < splitVerCount ? splitPCount : splitVerCount;

			//Check if each delimiter is greater or equal
			bool isGreaterEqual = true;
			for (int i = 0; i < count; i++)
			{
				if (Convert.ToInt16(splitVersion[i]) > Convert.ToInt16(splitP[i]))
				{
					i = count; //eject, the version is greater
				}
				else if (Convert.ToInt16(splitVersion[i]) < Convert.ToInt16(splitP[i]))
				{
					isGreaterEqual = false;
					i = count; //eject, the version is not equal or greater
				}
				if (i == count - 1 && splitP[count] == null) //we made it to the end and eveything is equal, must check if p have another delimiter making 'version; not greater
				{
					isGreaterEqual = false;
				}
			}

			return isGreaterEqual;
		}


	}
}
