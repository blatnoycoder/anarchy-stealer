using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C8 RID: 456
	public sealed class SQLiteIndexOrderBy
	{
		// Token: 0x06001489 RID: 5257 RVA: 0x0005EF74 File Offset: 0x0005D174
		internal SQLiteIndexOrderBy(UnsafeNativeMethods.sqlite3_index_orderby orderBy)
			: this(orderBy.iColumn, orderBy.desc)
		{
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0005EF8C File Offset: 0x0005D18C
		private SQLiteIndexOrderBy(int iColumn, byte desc)
		{
			this.iColumn = iColumn;
			this.desc = desc;
		}

		// Token: 0x04000890 RID: 2192
		public int iColumn;

		// Token: 0x04000891 RID: 2193
		public byte desc;
	}
}
