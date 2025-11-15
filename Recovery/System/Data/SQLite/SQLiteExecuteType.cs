using System;

namespace System.Data.SQLite
{
	// Token: 0x0200018C RID: 396
	public enum SQLiteExecuteType
	{
		// Token: 0x040006F2 RID: 1778
		None,
		// Token: 0x040006F3 RID: 1779
		NonQuery,
		// Token: 0x040006F4 RID: 1780
		Scalar,
		// Token: 0x040006F5 RID: 1781
		Reader,
		// Token: 0x040006F6 RID: 1782
		Default = 1
	}
}
