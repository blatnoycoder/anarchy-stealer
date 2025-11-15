using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200013C RID: 316
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x06000D64 RID: 3428 RVA: 0x0003F2DC File Offset: 0x0003D4DC
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0003F318 File Offset: 0x0003D518
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0003F31C File Offset: 0x0003D51C
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003F330 File Offset: 0x0003D530
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040004E4 RID: 1252
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
