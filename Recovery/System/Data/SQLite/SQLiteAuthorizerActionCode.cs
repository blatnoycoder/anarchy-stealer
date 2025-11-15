using System;

namespace System.Data.SQLite
{
	// Token: 0x0200018D RID: 397
	public enum SQLiteAuthorizerActionCode
	{
		// Token: 0x040006F8 RID: 1784
		None = -1,
		// Token: 0x040006F9 RID: 1785
		Copy,
		// Token: 0x040006FA RID: 1786
		CreateIndex,
		// Token: 0x040006FB RID: 1787
		CreateTable,
		// Token: 0x040006FC RID: 1788
		CreateTempIndex,
		// Token: 0x040006FD RID: 1789
		CreateTempTable,
		// Token: 0x040006FE RID: 1790
		CreateTempTrigger,
		// Token: 0x040006FF RID: 1791
		CreateTempView,
		// Token: 0x04000700 RID: 1792
		CreateTrigger,
		// Token: 0x04000701 RID: 1793
		CreateView,
		// Token: 0x04000702 RID: 1794
		Delete,
		// Token: 0x04000703 RID: 1795
		DropIndex,
		// Token: 0x04000704 RID: 1796
		DropTable,
		// Token: 0x04000705 RID: 1797
		DropTempIndex,
		// Token: 0x04000706 RID: 1798
		DropTempTable,
		// Token: 0x04000707 RID: 1799
		DropTempTrigger,
		// Token: 0x04000708 RID: 1800
		DropTempView,
		// Token: 0x04000709 RID: 1801
		DropTrigger,
		// Token: 0x0400070A RID: 1802
		DropView,
		// Token: 0x0400070B RID: 1803
		Insert,
		// Token: 0x0400070C RID: 1804
		Pragma,
		// Token: 0x0400070D RID: 1805
		Read,
		// Token: 0x0400070E RID: 1806
		Select,
		// Token: 0x0400070F RID: 1807
		Transaction,
		// Token: 0x04000710 RID: 1808
		Update,
		// Token: 0x04000711 RID: 1809
		Attach,
		// Token: 0x04000712 RID: 1810
		Detach,
		// Token: 0x04000713 RID: 1811
		AlterTable,
		// Token: 0x04000714 RID: 1812
		Reindex,
		// Token: 0x04000715 RID: 1813
		Analyze,
		// Token: 0x04000716 RID: 1814
		CreateVtable,
		// Token: 0x04000717 RID: 1815
		DropVtable,
		// Token: 0x04000718 RID: 1816
		Function,
		// Token: 0x04000719 RID: 1817
		Savepoint,
		// Token: 0x0400071A RID: 1818
		Recursive
	}
}
