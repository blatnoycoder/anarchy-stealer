using System;
using System.Text.RegularExpressions;

namespace Stealer.Chromium
{
	// Token: 0x02000021 RID: 33
	internal sealed class Parser
	{
		// Token: 0x06000099 RID: 153 RVA: 0x000069B0 File Offset: 0x00004BB0
		public static string RemoveLatest(string data)
		{
			return Regex.Split(Regex.Split(data, "\",")[0], "\"")[0];
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000069EC File Offset: 0x00004BEC
		public static bool DetectTitle(string data)
		{
			return data.Contains("\"name");
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006A10 File Offset: 0x00004C10
		public static string Get(string data, int index)
		{
			string text;
			try
			{
				text = Parser.RemoveLatest(Regex.Split(data, Parser.separator)[index]);
			}
			catch (IndexOutOfRangeException)
			{
				text = "Failed to parse url";
			}
			return text;
		}

		// Token: 0x04000077 RID: 119
		public static string separator = "\": \"";
	}
}
