using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBuildStats.Classes.General_Classes
{
	public static class StringExtensions
	{
		/// <summary>
		/// Counts how many words in a string
		/// </summary>
		/// <param name="input">String with words to be counted</param>
		/// <param name="count">Max words that will be returned</param>
		/// <param name="ignoreConsecutiveDelimiter">Remove extra delimeters (like spaces).</param>
		/// <param name="wordDelimiter"></param>
		/// <returns>Returns a list of words</returns>
		public static string[] GetWords(
			this string input,
			int count,
			bool ignoreConsecutiveDelimiter = true,
			char[] wordDelimiter = null)
		{
			if (string.IsNullOrEmpty(input)) return new string[] { };
			StringSplitOptions options = ignoreConsecutiveDelimiter ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
			string[] words = input.Split(wordDelimiter, count, options);
			if (words.Length < count)
				return words;   // not so many words found

			// "repair" last word since that contains the rest of the string
			// if there were more words, we want only the first word of the rest 
			int lastIndex = words.Length - 1;
			words[lastIndex] = words[lastIndex].Split(wordDelimiter, 2)[0];
			return words;
		}

		/// <summary>
		/// Takes a string a adds <br/> when the string is too long. Aka: artificial word-wrap
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static string BrWrapper(string p)
		{
			p = p.Replace(">", "> ");
			p = p.Replace("<", " <");
			p = p.Replace("/", " / ");
			p = p.Replace("< / ", "</");
			p = p.Replace(" / >", "/>");

			string newP = "";
			//Create newlines when linelength is long
			if (p.Length >= 50)
			{
				string[] words = p.GetWords(5000);
				int lineLength = 50;
				int lineLengthTemp = 0;
				bool waitForClose = false;
				for (int i = 0; i < words.Count(); i++ )
				{
					string s = words[i];

					newP += s + " ";
					if (s.Contains("<br"))
					{
						lineLengthTemp = newP.Length;
						lineLength = 50;
					}
					if (s.Contains("<"))
					{
						waitForClose = true;
					}
					if (waitForClose)
					{
						lineLength += s.Length - 1; //this extends the line length to compensate for html that doesn't effect line length
					}
					if (s.Contains(">"))
					{
						waitForClose = false;
					}

					//If we are not at the end of the line
					if (i + 1 != words.Count())
					{
						//If next word does not contain br
						if (!words[i + 1].Contains("<br"))
						{
							//Then check if conditions meet to add <br/>
							if (waitForClose == false && (newP.Length - lineLengthTemp) >= lineLength)
							{
								newP += "<br/>";
								lineLengthTemp = newP.Length;
								lineLength = 50; //Resets expected line length
							}
						}
					}
				}
			}
			else
			{
				newP = p;
			}
			return newP;
		}
	}
}
