using System;

namespace System.Data.SQLite
{
	// Token: 0x02000156 RID: 342
	[Flags]
	internal enum SQLiteTraceFlags
	{
		// Token: 0x040005E7 RID: 1511
		SQLITE_TRACE_NONE = 0,
		// Token: 0x040005E8 RID: 1512
		SQLITE_TRACE_STMT = 1,
		// Token: 0x040005E9 RID: 1513
		SQLITE_TRACE_PROFILE = 2,
		// Token: 0x040005EA RID: 1514
		SQLITE_TRACE_ROW = 4,
		// Token: 0x040005EB RID: 1515
		SQLITE_TRACE_CLOSE = 8
	}
}
