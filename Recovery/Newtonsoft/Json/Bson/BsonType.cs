using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000145 RID: 325
	internal enum BsonType : sbyte
	{
		// Token: 0x040004F5 RID: 1269
		Number = 1,
		// Token: 0x040004F6 RID: 1270
		String,
		// Token: 0x040004F7 RID: 1271
		Object,
		// Token: 0x040004F8 RID: 1272
		Array,
		// Token: 0x040004F9 RID: 1273
		Binary,
		// Token: 0x040004FA RID: 1274
		Undefined,
		// Token: 0x040004FB RID: 1275
		Oid,
		// Token: 0x040004FC RID: 1276
		Boolean,
		// Token: 0x040004FD RID: 1277
		Date,
		// Token: 0x040004FE RID: 1278
		Null,
		// Token: 0x040004FF RID: 1279
		Regex,
		// Token: 0x04000500 RID: 1280
		Reference,
		// Token: 0x04000501 RID: 1281
		Code,
		// Token: 0x04000502 RID: 1282
		Symbol,
		// Token: 0x04000503 RID: 1283
		CodeWScope,
		// Token: 0x04000504 RID: 1284
		Integer,
		// Token: 0x04000505 RID: 1285
		TimeStamp,
		// Token: 0x04000506 RID: 1286
		Long,
		// Token: 0x04000507 RID: 1287
		MinKey = -1,
		// Token: 0x04000508 RID: 1288
		MaxKey = 127
	}
}
