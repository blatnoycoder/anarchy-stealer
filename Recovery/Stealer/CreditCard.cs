using System;

namespace Stealer
{
	// Token: 0x0200000F RID: 15
	internal struct CreditCard
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003E04 File Offset: 0x00002004
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003E0C File Offset: 0x0000200C
		public string sNumber { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003E18 File Offset: 0x00002018
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003E20 File Offset: 0x00002020
		public string sExpYear { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003E2C File Offset: 0x0000202C
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003E34 File Offset: 0x00002034
		public string sExpMonth { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003E40 File Offset: 0x00002040
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003E48 File Offset: 0x00002048
		public string sName { get; set; }
	}
}
