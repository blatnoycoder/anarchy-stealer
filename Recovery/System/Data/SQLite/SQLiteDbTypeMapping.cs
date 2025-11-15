using System;

namespace System.Data.SQLite
{
	// Token: 0x02000193 RID: 403
	internal sealed class SQLiteDbTypeMapping
	{
		// Token: 0x06001198 RID: 4504 RVA: 0x00052E88 File Offset: 0x00051088
		internal SQLiteDbTypeMapping(string newTypeName, DbType newDataType, bool newPrimary)
		{
			this.typeName = newTypeName;
			this.dataType = newDataType;
			this.primary = newPrimary;
		}

		// Token: 0x04000728 RID: 1832
		internal string typeName;

		// Token: 0x04000729 RID: 1833
		internal DbType dataType;

		// Token: 0x0400072A RID: 1834
		internal bool primary;
	}
}
