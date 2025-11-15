using System;

namespace System.Data.SQLite
{
	// Token: 0x02000176 RID: 374
	// (Invoke) Token: 0x060010EF RID: 4335
	public delegate bool SQLiteBackupCallback(SQLiteConnection source, string sourceName, SQLiteConnection destination, string destinationName, int pages, int remainingPages, int totalPages, bool retry);
}
