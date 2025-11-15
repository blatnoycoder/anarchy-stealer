using System;

namespace System.Data.SQLite
{
	// Token: 0x020001DF RID: 479
	// (Invoke) Token: 0x060015BE RID: 5566
	public delegate SQLiteChangeSetConflictResult SessionConflictCallback(object clientData, SQLiteChangeSetConflictType type, ISQLiteChangeSetMetadataItem item);
}
