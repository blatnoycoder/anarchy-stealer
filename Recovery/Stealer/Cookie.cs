using System;

namespace Stealer
{
	// Token: 0x0200000E RID: 14
	internal struct Cookie
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003D78 File Offset: 0x00001F78
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003D80 File Offset: 0x00001F80
		public string expirationDate { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003D8C File Offset: 0x00001F8C
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00003D94 File Offset: 0x00001F94
		public string domain { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003DA0 File Offset: 0x00001FA0
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00003DA8 File Offset: 0x00001FA8
		public string name { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003DB4 File Offset: 0x00001FB4
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00003DBC File Offset: 0x00001FBC
		public string value { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003DC8 File Offset: 0x00001FC8
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00003DD0 File Offset: 0x00001FD0
		public string sPath { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003DDC File Offset: 0x00001FDC
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003DE4 File Offset: 0x00001FE4
		public string sKey { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003DF0 File Offset: 0x00001FF0
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public string secure { get; set; }
	}
}
