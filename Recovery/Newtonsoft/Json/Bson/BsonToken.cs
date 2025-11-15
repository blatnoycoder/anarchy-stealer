using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200013B RID: 315
	internal abstract class BsonToken
	{
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000D5E RID: 3422
		public abstract BsonType Type { get; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x0003F2AC File Offset: 0x0003D4AC
		// (set) Token: 0x06000D60 RID: 3424 RVA: 0x0003F2B4 File Offset: 0x0003D4B4
		public BsonToken Parent { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x0003F2C0 File Offset: 0x0003D4C0
		// (set) Token: 0x06000D62 RID: 3426 RVA: 0x0003F2C8 File Offset: 0x0003D4C8
		public int CalculatedSize { get; set; }
	}
}
