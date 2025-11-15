using System;

namespace System.Data.SQLite
{
	// Token: 0x02000188 RID: 392
	public enum SQLiteConnectionEventType
	{
		// Token: 0x040006CA RID: 1738
		Invalid = -1,
		// Token: 0x040006CB RID: 1739
		Unknown,
		// Token: 0x040006CC RID: 1740
		Opening,
		// Token: 0x040006CD RID: 1741
		ConnectionString,
		// Token: 0x040006CE RID: 1742
		Opened,
		// Token: 0x040006CF RID: 1743
		ChangeDatabase,
		// Token: 0x040006D0 RID: 1744
		NewTransaction,
		// Token: 0x040006D1 RID: 1745
		EnlistTransaction,
		// Token: 0x040006D2 RID: 1746
		NewCommand,
		// Token: 0x040006D3 RID: 1747
		NewDataReader,
		// Token: 0x040006D4 RID: 1748
		NewCriticalHandle,
		// Token: 0x040006D5 RID: 1749
		Closing,
		// Token: 0x040006D6 RID: 1750
		Closed,
		// Token: 0x040006D7 RID: 1751
		DisposingCommand,
		// Token: 0x040006D8 RID: 1752
		DisposingDataReader,
		// Token: 0x040006D9 RID: 1753
		ClosingDataReader,
		// Token: 0x040006DA RID: 1754
		OpenedFromPool,
		// Token: 0x040006DB RID: 1755
		ClosedToPool
	}
}
