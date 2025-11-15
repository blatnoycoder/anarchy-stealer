using System;

namespace Stealer
{
	// Token: 0x0200000D RID: 13
	public struct Password
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003D3C File Offset: 0x00001F3C
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00003D44 File Offset: 0x00001F44
		public string sUrl { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003D50 File Offset: 0x00001F50
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00003D58 File Offset: 0x00001F58
		public string sUsername { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003D64 File Offset: 0x00001F64
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00003D6C File Offset: 0x00001F6C
		public string sPassword { get; set; }
	}
}
