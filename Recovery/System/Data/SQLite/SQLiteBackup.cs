using System;

namespace System.Data.SQLite
{
	// Token: 0x0200014F RID: 335
	internal sealed class SQLiteBackup : IDisposable
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x00047488 File Offset: 0x00045688
		internal SQLiteBackup(SQLiteBase sqlbase, SQLiteBackupHandle backup, IntPtr destDb, byte[] zDestName, IntPtr sourceDb, byte[] zSourceName)
		{
			this._sql = sqlbase;
			this._sqlite_backup = backup;
			this._destDb = destDb;
			this._zDestName = zDestName;
			this._sourceDb = sourceDb;
			this._zSourceName = zSourceName;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x000474C0 File Offset: 0x000456C0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000474D0 File Offset: 0x000456D0
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteBackup).Name);
			}
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x000474F4 File Offset: 0x000456F4
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this._sqlite_backup != null)
					{
						this._sqlite_backup.Dispose();
						this._sqlite_backup = null;
					}
					this._zSourceName = null;
					this._sourceDb = IntPtr.Zero;
					this._zDestName = null;
					this._destDb = IntPtr.Zero;
					this._sql = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00047568 File Offset: 0x00045768
		~SQLiteBackup()
		{
			this.Dispose(false);
		}

		// Token: 0x04000557 RID: 1367
		internal SQLiteBase _sql;

		// Token: 0x04000558 RID: 1368
		internal SQLiteBackupHandle _sqlite_backup;

		// Token: 0x04000559 RID: 1369
		internal IntPtr _destDb;

		// Token: 0x0400055A RID: 1370
		internal byte[] _zDestName;

		// Token: 0x0400055B RID: 1371
		internal IntPtr _sourceDb;

		// Token: 0x0400055C RID: 1372
		internal byte[] _zSourceName;

		// Token: 0x0400055D RID: 1373
		internal SQLiteErrorCode _stepResult;

		// Token: 0x0400055E RID: 1374
		private bool disposed;
	}
}
