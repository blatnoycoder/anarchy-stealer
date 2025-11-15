using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000066 RID: 102
	public enum JsonToken
	{
		// Token: 0x040001BA RID: 442
		None,
		// Token: 0x040001BB RID: 443
		StartObject,
		// Token: 0x040001BC RID: 444
		StartArray,
		// Token: 0x040001BD RID: 445
		StartConstructor,
		// Token: 0x040001BE RID: 446
		PropertyName,
		// Token: 0x040001BF RID: 447
		Comment,
		// Token: 0x040001C0 RID: 448
		Raw,
		// Token: 0x040001C1 RID: 449
		Integer,
		// Token: 0x040001C2 RID: 450
		Float,
		// Token: 0x040001C3 RID: 451
		String,
		// Token: 0x040001C4 RID: 452
		Boolean,
		// Token: 0x040001C5 RID: 453
		Null,
		// Token: 0x040001C6 RID: 454
		Undefined,
		// Token: 0x040001C7 RID: 455
		EndObject,
		// Token: 0x040001C8 RID: 456
		EndArray,
		// Token: 0x040001C9 RID: 457
		EndConstructor,
		// Token: 0x040001CA RID: 458
		Date,
		// Token: 0x040001CB RID: 459
		Bytes
	}
}
