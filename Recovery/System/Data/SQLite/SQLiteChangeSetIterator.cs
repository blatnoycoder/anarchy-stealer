using System;

namespace System.Data.SQLite
{
	// Token: 0x020001E6 RID: 486
	internal class SQLiteChangeSetIterator : IDisposable
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x00062F24 File Offset: 0x00061124
		protected SQLiteChangeSetIterator(IntPtr iterator, bool ownHandle)
		{
			this.iterator = iterator;
			this.ownHandle = ownHandle;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00062F3C File Offset: 0x0006113C
		internal void CheckHandle()
		{
			if (this.iterator == IntPtr.Zero)
			{
				throw new InvalidOperationException("iterator is not open");
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00062F60 File Offset: 0x00061160
		internal IntPtr GetIntPtr()
		{
			return this.iterator;
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00062F68 File Offset: 0x00061168
		public bool Next()
		{
			this.CheckDisposed();
			this.CheckHandle();
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_next(this.iterator);
			SQLiteErrorCode sqliteErrorCode2 = sqliteErrorCode;
			if (sqliteErrorCode2 == SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(SQLiteErrorCode.Ok, "sqlite3changeset_next: unexpected result Ok");
			}
			switch (sqliteErrorCode2)
			{
			case SQLiteErrorCode.Row:
				return true;
			case SQLiteErrorCode.Done:
				return false;
			default:
				throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_next");
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00062FCC File Offset: 0x000611CC
		public static SQLiteChangeSetIterator Attach(IntPtr iterator)
		{
			return new SQLiteChangeSetIterator(iterator, false);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00062FD8 File Offset: 0x000611D8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00062FE8 File Offset: 0x000611E8
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteChangeSetIterator).Name);
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0006300C File Offset: 0x0006120C
		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && this.iterator != IntPtr.Zero)
				{
					if (this.ownHandle)
					{
						UnsafeNativeMethods.sqlite3changeset_finalize(this.iterator);
					}
					this.iterator = IntPtr.Zero;
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00063078 File Offset: 0x00061278
		~SQLiteChangeSetIterator()
		{
			this.Dispose(false);
		}

		// Token: 0x040008E0 RID: 2272
		private IntPtr iterator;

		// Token: 0x040008E1 RID: 2273
		private bool ownHandle;

		// Token: 0x040008E2 RID: 2274
		private bool disposed;
	}
}
