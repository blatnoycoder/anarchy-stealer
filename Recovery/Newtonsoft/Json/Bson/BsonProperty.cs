using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000144 RID: 324
	internal class BsonProperty
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0003F4C4 File Offset: 0x0003D6C4
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x0003F4CC File Offset: 0x0003D6CC
		public BsonString Name { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0003F4D8 File Offset: 0x0003D6D8
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0003F4E0 File Offset: 0x0003D6E0
		public BsonToken Value { get; set; }
	}
}
