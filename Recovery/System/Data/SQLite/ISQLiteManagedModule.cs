using System;

namespace System.Data.SQLite
{
	// Token: 0x020001CF RID: 463
	public interface ISQLiteManagedModule
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060014CD RID: 5325
		bool Declared { get; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060014CE RID: 5326
		string Name { get; }

		// Token: 0x060014CF RID: 5327
		SQLiteErrorCode Create(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error);

		// Token: 0x060014D0 RID: 5328
		SQLiteErrorCode Connect(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error);

		// Token: 0x060014D1 RID: 5329
		SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index);

		// Token: 0x060014D2 RID: 5330
		SQLiteErrorCode Disconnect(SQLiteVirtualTable table);

		// Token: 0x060014D3 RID: 5331
		SQLiteErrorCode Destroy(SQLiteVirtualTable table);

		// Token: 0x060014D4 RID: 5332
		SQLiteErrorCode Open(SQLiteVirtualTable table, ref SQLiteVirtualTableCursor cursor);

		// Token: 0x060014D5 RID: 5333
		SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor);

		// Token: 0x060014D6 RID: 5334
		SQLiteErrorCode Filter(SQLiteVirtualTableCursor cursor, int indexNumber, string indexString, SQLiteValue[] values);

		// Token: 0x060014D7 RID: 5335
		SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor);

		// Token: 0x060014D8 RID: 5336
		bool Eof(SQLiteVirtualTableCursor cursor);

		// Token: 0x060014D9 RID: 5337
		SQLiteErrorCode Column(SQLiteVirtualTableCursor cursor, SQLiteContext context, int index);

		// Token: 0x060014DA RID: 5338
		SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId);

		// Token: 0x060014DB RID: 5339
		SQLiteErrorCode Update(SQLiteVirtualTable table, SQLiteValue[] values, ref long rowId);

		// Token: 0x060014DC RID: 5340
		SQLiteErrorCode Begin(SQLiteVirtualTable table);

		// Token: 0x060014DD RID: 5341
		SQLiteErrorCode Sync(SQLiteVirtualTable table);

		// Token: 0x060014DE RID: 5342
		SQLiteErrorCode Commit(SQLiteVirtualTable table);

		// Token: 0x060014DF RID: 5343
		SQLiteErrorCode Rollback(SQLiteVirtualTable table);

		// Token: 0x060014E0 RID: 5344
		bool FindFunction(SQLiteVirtualTable table, int argumentCount, string name, ref SQLiteFunction function, ref IntPtr pClientData);

		// Token: 0x060014E1 RID: 5345
		SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName);

		// Token: 0x060014E2 RID: 5346
		SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint);

		// Token: 0x060014E3 RID: 5347
		SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint);

		// Token: 0x060014E4 RID: 5348
		SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint);
	}
}
