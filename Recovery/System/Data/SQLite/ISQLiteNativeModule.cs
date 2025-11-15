using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C1 RID: 449
	public interface ISQLiteNativeModule
	{
		// Token: 0x06001452 RID: 5202
		SQLiteErrorCode xCreate(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError);

		// Token: 0x06001453 RID: 5203
		SQLiteErrorCode xConnect(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError);

		// Token: 0x06001454 RID: 5204
		SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex);

		// Token: 0x06001455 RID: 5205
		SQLiteErrorCode xDisconnect(IntPtr pVtab);

		// Token: 0x06001456 RID: 5206
		SQLiteErrorCode xDestroy(IntPtr pVtab);

		// Token: 0x06001457 RID: 5207
		SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor);

		// Token: 0x06001458 RID: 5208
		SQLiteErrorCode xClose(IntPtr pCursor);

		// Token: 0x06001459 RID: 5209
		SQLiteErrorCode xFilter(IntPtr pCursor, int idxNum, IntPtr idxStr, int argc, IntPtr argv);

		// Token: 0x0600145A RID: 5210
		SQLiteErrorCode xNext(IntPtr pCursor);

		// Token: 0x0600145B RID: 5211
		int xEof(IntPtr pCursor);

		// Token: 0x0600145C RID: 5212
		SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index);

		// Token: 0x0600145D RID: 5213
		SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId);

		// Token: 0x0600145E RID: 5214
		SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId);

		// Token: 0x0600145F RID: 5215
		SQLiteErrorCode xBegin(IntPtr pVtab);

		// Token: 0x06001460 RID: 5216
		SQLiteErrorCode xSync(IntPtr pVtab);

		// Token: 0x06001461 RID: 5217
		SQLiteErrorCode xCommit(IntPtr pVtab);

		// Token: 0x06001462 RID: 5218
		SQLiteErrorCode xRollback(IntPtr pVtab);

		// Token: 0x06001463 RID: 5219
		int xFindFunction(IntPtr pVtab, int nArg, IntPtr zName, ref SQLiteCallback callback, ref IntPtr pClientData);

		// Token: 0x06001464 RID: 5220
		SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew);

		// Token: 0x06001465 RID: 5221
		SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint);

		// Token: 0x06001466 RID: 5222
		SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint);

		// Token: 0x06001467 RID: 5223
		SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint);
	}
}
