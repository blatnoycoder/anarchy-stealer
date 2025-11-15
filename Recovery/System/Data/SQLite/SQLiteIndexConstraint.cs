using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C7 RID: 455
	public sealed class SQLiteIndexConstraint
	{
		// Token: 0x06001487 RID: 5255 RVA: 0x0005EF28 File Offset: 0x0005D128
		internal SQLiteIndexConstraint(UnsafeNativeMethods.sqlite3_index_constraint constraint)
			: this(constraint.iColumn, constraint.op, constraint.usable, constraint.iTermOffset)
		{
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0005EF4C File Offset: 0x0005D14C
		private SQLiteIndexConstraint(int iColumn, SQLiteIndexConstraintOp op, byte usable, int iTermOffset)
		{
			this.iColumn = iColumn;
			this.op = op;
			this.usable = usable;
			this.iTermOffset = iTermOffset;
		}

		// Token: 0x0400088C RID: 2188
		public int iColumn;

		// Token: 0x0400088D RID: 2189
		public SQLiteIndexConstraintOp op;

		// Token: 0x0400088E RID: 2190
		public byte usable;

		// Token: 0x0400088F RID: 2191
		public int iTermOffset;
	}
}
