using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200013F RID: 319
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000D71 RID: 3441 RVA: 0x0003F3CC File Offset: 0x0003D5CC
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0003F3E4 File Offset: 0x0003D5E4
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0003F3EC File Offset: 0x0003D5EC
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x040004E9 RID: 1257
		private readonly object _value;

		// Token: 0x040004EA RID: 1258
		private readonly BsonType _type;
	}
}
