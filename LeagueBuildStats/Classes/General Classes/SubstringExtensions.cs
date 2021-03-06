﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBuildStats.Classes.General_Classes
{
	static class SubstringExtensions
	{
		/// <summary>
		/// Get string value between [first] a and [last] b.
		/// </summary>
		public static string Between(this string value, string a, string b)
		{
			int posA = value.IndexOf(a);
			int posB = value.LastIndexOf(b);
			if (posA == -1)
			{
				return "";
			}
			if (posB == -1)
			{
				return "";
			}
			int adjustedPosA = posA + a.Length;
			if (adjustedPosA >= posB)
			{
				return "";
			}
			return value.Substring(adjustedPosA, posB - adjustedPosA);
		}

		/// <summary>
		/// Get string value after [first] a.
		/// </summary>
		public static string Before(this string value, string a)
		{
			int posA = value.IndexOf(a);
			if (posA == -1)
			{
				return "";
			}
			return value.Substring(0, posA);
		}

		/// <summary>
		/// Get string value after [last] a.
		/// </summary>
		public static string After(this string value, string a)
		{
			int posA = value.LastIndexOf(a);
			if (posA == -1)
			{
				return "";
			}
			int adjustedPosA = posA + a.Length;
			if (adjustedPosA >= value.Length)
			{
				return "";
			}
			return value.Substring(adjustedPosA);
		}


		public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
		{
			int Place = Source.LastIndexOf(Find);
			string result = Source;
			if (Place != -1)
			{
				result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
			}
			return result;
		}
	}
}
