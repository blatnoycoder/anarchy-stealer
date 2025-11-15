using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000056 RID: 86
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000AB20 File Offset: 0x00008D20
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000AB28 File Offset: 0x00008D28
		public bool WriteData { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000AB34 File Offset: 0x00008D34
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000AB3C File Offset: 0x00008D3C
		public bool ReadData { get; set; }

		// Token: 0x060001BF RID: 447 RVA: 0x0000AB48 File Offset: 0x00008D48
		public JsonExtensionDataAttribute()
		{
			this.WriteData = true;
			this.ReadData = true;
		}
	}
}
