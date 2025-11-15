using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000140 RID: 320
	internal class BsonBoolean : BsonValue
	{
		// Token: 0x06000D74 RID: 3444 RVA: 0x0003F3F4 File Offset: 0x0003D5F4
		private BsonBoolean(bool value)
			: base(value, BsonType.Boolean)
		{
		}

		// Token: 0x040004EB RID: 1259
		public static readonly BsonBoolean False = new BsonBoolean(false);

		// Token: 0x040004EC RID: 1260
		public static readonly BsonBoolean True = new BsonBoolean(true);
	}
}
