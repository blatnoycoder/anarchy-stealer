using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000141 RID: 321
	internal class BsonString : BsonValue
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0003F41C File Offset: 0x0003D61C
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x0003F424 File Offset: 0x0003D624
		public int ByteCount { get; set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0003F430 File Offset: 0x0003D630
		public bool IncludeLength { get; }

		// Token: 0x06000D79 RID: 3449 RVA: 0x0003F438 File Offset: 0x0003D638
		public BsonString(object value, bool includeLength)
			: base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
