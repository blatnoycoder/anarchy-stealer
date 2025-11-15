using System;

namespace Stealer
{
	// Token: 0x02000011 RID: 17
	internal struct Site
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003E54 File Offset: 0x00002054
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003E5C File Offset: 0x0000205C
		public string sUrl { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003E68 File Offset: 0x00002068
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00003E70 File Offset: 0x00002070
		public string sTitle { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003E7C File Offset: 0x0000207C
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003E84 File Offset: 0x00002084
		public int iCount { get; set; }
	}
}
