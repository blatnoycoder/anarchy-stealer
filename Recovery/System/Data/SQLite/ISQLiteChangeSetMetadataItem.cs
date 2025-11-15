using System;

namespace System.Data.SQLite
{
	// Token: 0x020001E2 RID: 482
	public interface ISQLiteChangeSetMetadataItem : IDisposable
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060015C9 RID: 5577
		string TableName { get; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060015CA RID: 5578
		int NumberOfColumns { get; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060015CB RID: 5579
		SQLiteAuthorizerActionCode OperationCode { get; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060015CC RID: 5580
		bool Indirect { get; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060015CD RID: 5581
		bool[] PrimaryKeyColumns { get; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060015CE RID: 5582
		int NumberOfForeignKeyConflicts { get; }

		// Token: 0x060015CF RID: 5583
		SQLiteValue GetOldValue(int columnIndex);

		// Token: 0x060015D0 RID: 5584
		SQLiteValue GetNewValue(int columnIndex);

		// Token: 0x060015D1 RID: 5585
		SQLiteValue GetConflictValue(int columnIndex);
	}
}
