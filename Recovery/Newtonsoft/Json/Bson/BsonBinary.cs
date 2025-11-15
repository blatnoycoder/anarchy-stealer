using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000142 RID: 322
	internal class BsonBinary : BsonValue
	{
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0003F44C File Offset: 0x0003D64C
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x0003F454 File Offset: 0x0003D654
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003F460 File Offset: 0x0003D660
		public BsonBinary(byte[] value, BsonBinaryType binaryType)
			: base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
