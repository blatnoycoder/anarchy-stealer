using System;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001B4 RID: 436
	public sealed class SQLiteTransaction2 : SQLiteTransaction
	{
		// Token: 0x0600131A RID: 4890 RVA: 0x0005B368 File Offset: 0x00059568
		internal SQLiteTransaction2(SQLiteConnection connection, bool deferredLock)
			: base(connection, deferredLock)
		{
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0005B374 File Offset: 0x00059574
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteTransaction2).Name);
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0005B398 File Offset: 0x00059598
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

		// Token: 0x0600131D RID: 4893 RVA: 0x0005B3F0 File Offset: 0x000595F0
		public override void Commit()
		{
			this.CheckDisposed();
			base.IsValid(true);
			if (this._beginLevel == 0)
			{
				using (SQLiteCommand sqliteCommand = this._cnn.CreateCommand())
				{
					sqliteCommand.CommandText = "COMMIT;";
					sqliteCommand.ExecuteNonQuery();
				}
				this._cnn._transactionLevel = 0;
				this._cnn = null;
				return;
			}
			using (SQLiteCommand sqliteCommand2 = this._cnn.CreateCommand())
			{
				if (string.IsNullOrEmpty(this._savePointName))
				{
					throw new SQLiteException("Cannot commit, unknown SAVEPOINT");
				}
				sqliteCommand2.CommandText = string.Format("RELEASE {0};", this._savePointName);
				sqliteCommand2.ExecuteNonQuery();
			}
			this._cnn._transactionLevel--;
			this._cnn = null;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0005B4E8 File Offset: 0x000596E8
		protected override void Begin(bool deferredLock)
		{
			int num;
			if ((num = this._cnn._transactionLevel++) == 0)
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
						this._beginLevel = num;
					}
					return;
				}
				catch (SQLiteException)
				{
					this._cnn._transactionLevel--;
					this._cnn = null;
					throw;
				}
			}
			try
			{
				using (SQLiteCommand sqliteCommand2 = this._cnn.CreateCommand())
				{
					this._savePointName = this.GetSavePointName();
					sqliteCommand2.CommandText = string.Format("SAVEPOINT {0};", this._savePointName);
					sqliteCommand2.ExecuteNonQuery();
					this._beginLevel = num;
				}
			}
			catch (SQLiteException)
			{
				this._cnn._transactionLevel--;
				this._cnn = null;
				throw;
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0005B628 File Offset: 0x00059828
		protected override void IssueRollback(bool throwError)
		{
			SQLiteConnection sqliteConnection = Interlocked.Exchange<SQLiteConnection>(ref this._cnn, null);
			if (sqliteConnection != null)
			{
				if (this._beginLevel == 0)
				{
					try
					{
						using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
						{
							sqliteCommand.CommandText = "ROLLBACK;";
							sqliteCommand.ExecuteNonQuery();
						}
						sqliteConnection._transactionLevel = 0;
						return;
					}
					catch
					{
						if (throwError)
						{
							throw;
						}
						return;
					}
				}
				try
				{
					using (SQLiteCommand sqliteCommand2 = sqliteConnection.CreateCommand())
					{
						if (string.IsNullOrEmpty(this._savePointName))
						{
							throw new SQLiteException("Cannot rollback, unknown SAVEPOINT");
						}
						sqliteCommand2.CommandText = string.Format("ROLLBACK TO {0};", this._savePointName);
						sqliteCommand2.ExecuteNonQuery();
					}
					sqliteConnection._transactionLevel--;
				}
				catch
				{
					if (throwError)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0005B740 File Offset: 0x00059940
		private string GetSavePointName()
		{
			int num = ++this._cnn._transactionSequence;
			return string.Format("sqlite_dotnet_savepoint_{0}", num);
		}

		// Token: 0x04000819 RID: 2073
		private int _beginLevel;

		// Token: 0x0400081A RID: 2074
		private string _savePointName;

		// Token: 0x0400081B RID: 2075
		private bool disposed;
	}
}
