using System;

namespace System.Data.SQLite
{
	// Token: 0x02000191 RID: 401
	internal sealed class SQLiteType
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x00052CD8 File Offset: 0x00050ED8
		public SQLiteType()
		{
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00052CE0 File Offset: 0x00050EE0
		public SQLiteType(TypeAffinity affinity, DbType type)
			: this()
		{
			this.Affinity = affinity;
			this.Type = type;
		}

		// Token: 0x04000725 RID: 1829
		internal DbType Type;

		// Token: 0x04000726 RID: 1830
		internal TypeAffinity Affinity;
	}
}
