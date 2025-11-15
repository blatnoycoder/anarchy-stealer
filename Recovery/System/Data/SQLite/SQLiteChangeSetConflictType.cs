using System;

namespace System.Data.SQLite
{
	// Token: 0x020001DB RID: 475
	public enum SQLiteChangeSetConflictType
	{
		// Token: 0x040008CE RID: 2254
		Data = 1,
		// Token: 0x040008CF RID: 2255
		NotFound,
		// Token: 0x040008D0 RID: 2256
		Conflict,
		// Token: 0x040008D1 RID: 2257
		Constraint,
		// Token: 0x040008D2 RID: 2258
		ForeignKey
	}
}
