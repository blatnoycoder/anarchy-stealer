using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000137 RID: 311
	internal enum BsonBinaryType : byte
	{
		// Token: 0x040004C7 RID: 1223
		Binary,
		// Token: 0x040004C8 RID: 1224
		Function,
		// Token: 0x040004C9 RID: 1225
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld,
		// Token: 0x040004CA RID: 1226
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld,
		// Token: 0x040004CB RID: 1227
		Uuid,
		// Token: 0x040004CC RID: 1228
		Md5,
		// Token: 0x040004CD RID: 1229
		UserDefined = 128
	}
}
