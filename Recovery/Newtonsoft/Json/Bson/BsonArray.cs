using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200013D RID: 317
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x0003F34C File Offset: 0x0003D54C
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0003F364 File Offset: 0x0003D564
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0003F368 File Offset: 0x0003D568
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0003F37C File Offset: 0x0003D57C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040004E5 RID: 1253
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
