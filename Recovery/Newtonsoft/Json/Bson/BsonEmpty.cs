using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200013E RID: 318
	internal class BsonEmpty : BsonToken
	{
		// Token: 0x06000D6E RID: 3438 RVA: 0x0003F398 File Offset: 0x0003D598
		private BsonEmpty(BsonType type)
		{
			this.Type = type;
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0003F3A8 File Offset: 0x0003D5A8
		public override BsonType Type { get; }

		// Token: 0x040004E6 RID: 1254
		public static readonly BsonToken Null = new BsonEmpty(BsonType.Null);

		// Token: 0x040004E7 RID: 1255
		public static readonly BsonToken Undefined = new BsonEmpty(BsonType.Undefined);
	}
}
