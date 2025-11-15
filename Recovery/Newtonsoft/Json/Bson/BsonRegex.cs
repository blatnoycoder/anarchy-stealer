using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000143 RID: 323
	internal class BsonRegex : BsonToken
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0003F474 File Offset: 0x0003D674
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x0003F47C File Offset: 0x0003D67C
		public BsonString Pattern { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0003F488 File Offset: 0x0003D688
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0003F490 File Offset: 0x0003D690
		public BsonString Options { get; set; }

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003F49C File Offset: 0x0003D69C
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0003F4C0 File Offset: 0x0003D6C0
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
