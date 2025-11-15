using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C9 RID: 457
	public sealed class SQLiteIndexConstraintUsage
	{
		// Token: 0x0600148B RID: 5259 RVA: 0x0005EFA4 File Offset: 0x0005D1A4
		internal SQLiteIndexConstraintUsage()
		{
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0005EFAC File Offset: 0x0005D1AC
		internal SQLiteIndexConstraintUsage(UnsafeNativeMethods.sqlite3_index_constraint_usage constraintUsage)
			: this(constraintUsage.argvIndex, constraintUsage.omit)
		{
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0005EFC4 File Offset: 0x0005D1C4
		private SQLiteIndexConstraintUsage(int argvIndex, byte omit)
		{
			this.argvIndex = argvIndex;
			this.omit = omit;
		}

		// Token: 0x04000892 RID: 2194
		public int argvIndex;

		// Token: 0x04000893 RID: 2195
		public byte omit;
	}
}
