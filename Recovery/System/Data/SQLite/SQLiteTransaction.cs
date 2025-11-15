using System;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001B3 RID: 435
	public class SQLiteTransaction : SQLiteTransactionBase
	{
		// Token: 0x06001314 RID: 4884 RVA: 0x0005B128 File Offset: 0x00059328
		internal SQLiteTransaction(SQLiteConnection connection, bool deferredLock)
			: base(connection, deferredLock)
		{
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0005B134 File Offset: 0x00059334
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteTransaction).Name);
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0005B158 File Offset: 0x00059358
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing && base.IsValid(false))
				{
					this.IssueRollback(false);
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0005B1B0 File Offset: 0x000593B0
		public override void Commit()
		{
			this.CheckDisposed();
			base.IsValid(true);
			if (this._cnn._transactionLevel - 1 == 0)
			{
				using (SQLiteCommand sqliteCommand = this._cnn.CreateCommand())
				{
					sqliteCommand.CommandText = "COMMIT;";
					sqliteCommand.ExecuteNonQuery();
				}
			}
			this._cnn._transactionLevel--;
			this._cnn = null;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0005B238 File Offset: 0x00059438
		protected override void Begin(bool deferredLock)
		{
			if (this._cnn._transactionLevel++ == 0)
			{
				try
				{
					using (SQLiteCommand sqliteCommand = this._cnn.CreateCommand())
					{
						if (!deferredLock)
						{
							sqliteCommand.CommandText = "BEGIN IMMEDIATE;";
						}
						else
						{
							sqliteCommand.CommandText = "BEGIN;";
						}
						sqliteCommand.ExecuteNonQuery();
					}
				}
				catch (SQLiteException)
				{
					this._cnn._transactionLevel--;
					this._cnn = null;
					throw;
				}
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0005B2E4 File Offset: 0x000594E4
		protected override void IssueRollback(bool throwError)
		{
			SQLiteConnection sqliteConnection = Interlocked.Exchange<SQLiteConnection>(ref this._cnn, null);
			if (sqliteConnection != null)
			{
				try
				{
					using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
					{
						sqliteCommand.CommandText = "ROLLBACK;";
						sqliteCommand.ExecuteNonQuery();
					}
				}
				catch
				{
					if (throwError)
					{
						throw;
					}
				}
				sqliteConnection._transactionLevel = 0;
			}
		}

		// Token: 0x04000818 RID: 2072
		private bool disposed;
	}
}
