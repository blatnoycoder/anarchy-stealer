using System;

namespace System.Data.SQLite
{
	// Token: 0x0200018A RID: 394
	public enum SQLiteJournalModeEnum
	{
		// Token: 0x040006E5 RID: 1765
		Default = -1,
		// Token: 0x040006E6 RID: 1766
		Delete,
		// Token: 0x040006E7 RID: 1767
		Persist,
		// Token: 0x040006E8 RID: 1768
		Off,
		// Token: 0x040006E9 RID: 1769
		Truncate,
		// Token: 0x040006EA RID: 1770
		Memory,
		// Token: 0x040006EB RID: 1771
		Wal
	}
}
