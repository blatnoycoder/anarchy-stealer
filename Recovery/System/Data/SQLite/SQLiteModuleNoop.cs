using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x020001D5 RID: 469
	public class SQLiteModuleNoop : SQLiteModule
	{
		// Token: 0x0600156A RID: 5482 RVA: 0x00062070 File Offset: 0x00060270
		public SQLiteModuleNoop(string name)
			: base(name)
		{
			this.resultCodes = new Dictionary<string, SQLiteErrorCode>();
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00062084 File Offset: 0x00060284
		protected virtual SQLiteErrorCode GetDefaultResultCode()
		{
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00062088 File Offset: 0x00060288
		protected virtual bool ResultCodeToEofResult(SQLiteErrorCode resultCode)
		{
			return resultCode != SQLiteErrorCode.Ok;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00062094 File Offset: 0x00060294
		protected virtual bool ResultCodeToFindFunctionResult(SQLiteErrorCode resultCode)
		{
			return resultCode == SQLiteErrorCode.Ok;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x000620A0 File Offset: 0x000602A0
		protected virtual SQLiteErrorCode GetMethodResultCode(string methodName)
		{
			if (methodName == null || this.resultCodes == null)
			{
				return this.GetDefaultResultCode();
			}
			SQLiteErrorCode sqliteErrorCode;
			if (this.resultCodes != null && this.resultCodes.TryGetValue(methodName, out sqliteErrorCode))
			{
				return sqliteErrorCode;
			}
			return this.GetDefaultResultCode();
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000620F0 File Offset: 0x000602F0
		protected virtual bool SetMethodResultCode(string methodName, SQLiteErrorCode resultCode)
		{
			if (methodName == null || this.resultCodes == null)
			{
				return false;
			}
			this.resultCodes[methodName] = resultCode;
			return true;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00062114 File Offset: 0x00060314
		public override SQLiteErrorCode Create(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Create");
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00062128 File Offset: 0x00060328
		public override SQLiteErrorCode Connect(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Connect");
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0006213C File Offset: 0x0006033C
		public override SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("BestIndex");
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00062150 File Offset: 0x00060350
		public override SQLiteErrorCode Disconnect(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Disconnect");
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00062164 File Offset: 0x00060364
		public override SQLiteErrorCode Destroy(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Destroy");
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00062178 File Offset: 0x00060378
		public override SQLiteErrorCode Open(SQLiteVirtualTable table, ref SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Open");
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0006218C File Offset: 0x0006038C
		public override SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Close");
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x000621A0 File Offset: 0x000603A0
		public override SQLiteErrorCode Filter(SQLiteVirtualTableCursor cursor, int indexNumber, string indexString, SQLiteValue[] values)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Filter");
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x000621B4 File Offset: 0x000603B4
		public override SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Next");
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000621C8 File Offset: 0x000603C8
		public override bool Eof(SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			return this.ResultCodeToEofResult(this.GetMethodResultCode("Eof"));
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x000621E4 File Offset: 0x000603E4
		public override SQLiteErrorCode Column(SQLiteVirtualTableCursor cursor, SQLiteContext context, int index)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Column");
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x000621F8 File Offset: 0x000603F8
		public override SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("RowId");
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0006220C File Offset: 0x0006040C
		public override SQLiteErrorCode Update(SQLiteVirtualTable table, SQLiteValue[] values, ref long rowId)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Update");
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00062220 File Offset: 0x00060420
		public override SQLiteErrorCode Begin(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Begin");
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00062234 File Offset: 0x00060434
		public override SQLiteErrorCode Sync(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Sync");
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00062248 File Offset: 0x00060448
		public override SQLiteErrorCode Commit(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Commit");
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0006225C File Offset: 0x0006045C
		public override SQLiteErrorCode Rollback(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Rollback");
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00062270 File Offset: 0x00060470
		public override bool FindFunction(SQLiteVirtualTable table, int argumentCount, string name, ref SQLiteFunction function, ref IntPtr pClientData)
		{
			this.CheckDisposed();
			return this.ResultCodeToFindFunctionResult(this.GetMethodResultCode("FindFunction"));
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0006228C File Offset: 0x0006048C
		public override SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Rename");
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x000622A0 File Offset: 0x000604A0
		public override SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Savepoint");
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x000622B4 File Offset: 0x000604B4
		public override SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("Release");
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x000622C8 File Offset: 0x000604C8
		public override SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint)
		{
			this.CheckDisposed();
			return this.GetMethodResultCode("RollbackTo");
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x000622DC File Offset: 0x000604DC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteModuleNoop).Name);
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00062300 File Offset: 0x00060500
		protected override void Dispose(bool disposing)
		{
			try
			{
				bool flag = this.disposed;
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040008BE RID: 2238
		private Dictionary<string, SQLiteErrorCode> resultCodes;

		// Token: 0x040008BF RID: 2239
		private bool disposed;
	}
}
