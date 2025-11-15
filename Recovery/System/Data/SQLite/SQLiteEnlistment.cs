using System;
using System.Globalization;
using System.Transactions;

namespace System.Data.SQLite
{
	// Token: 0x020001C0 RID: 448
	internal sealed class SQLiteEnlistment : IDisposable, IEnlistmentNotification
	{
		// Token: 0x06001447 RID: 5191 RVA: 0x0005E56C File Offset: 0x0005C76C
		internal SQLiteEnlistment(SQLiteConnection cnn, Transaction scope, IsolationLevel defaultIsolationLevel, bool throwOnUnavailable, bool throwOnUnsupported)
		{
			this._transaction = cnn.BeginTransaction(this.GetSystemDataIsolationLevel(cnn, scope, defaultIsolationLevel, throwOnUnavailable, throwOnUnsupported));
			this._scope = scope;
			this._scope.EnlistVolatile(this, EnlistmentOptions.None);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005E5B4 File Offset: 0x0005C7B4
		private IsolationLevel GetSystemDataIsolationLevel(SQLiteConnection connection, Transaction transaction, IsolationLevel defaultIsolationLevel, bool throwOnUnavailable, bool throwOnUnsupported)
		{
			if (transaction == null)
			{
				if (connection != null)
				{
					return connection.GetDefaultIsolationLevel();
				}
				if (throwOnUnavailable)
				{
					throw new InvalidOperationException("isolation level is unavailable");
				}
				return defaultIsolationLevel;
			}
			else
			{
				IsolationLevel isolationLevel = transaction.IsolationLevel;
				switch (isolationLevel)
				{
				case IsolationLevel.Serializable:
					return IsolationLevel.Serializable;
				case IsolationLevel.RepeatableRead:
					return IsolationLevel.RepeatableRead;
				case IsolationLevel.ReadCommitted:
					return IsolationLevel.ReadCommitted;
				case IsolationLevel.ReadUncommitted:
					return IsolationLevel.ReadUncommitted;
				case IsolationLevel.Snapshot:
					return IsolationLevel.Snapshot;
				case IsolationLevel.Chaos:
					return IsolationLevel.Chaos;
				case IsolationLevel.Unspecified:
					return IsolationLevel.Unspecified;
				default:
					if (throwOnUnsupported)
					{
						throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "unsupported isolation level {0}", new object[] { isolationLevel }));
					}
					return defaultIsolationLevel;
				}
			}
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0005E674 File Offset: 0x0005C874
		private void Cleanup(SQLiteConnection cnn)
		{
			if (this._disposeConnection && cnn != null)
			{
				cnn.Dispose();
			}
			this._transaction = null;
			this._scope = null;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0005E69C File Offset: 0x0005C89C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0005E6AC File Offset: 0x0005C8AC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteEnlistment).Name);
			}
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0005E6D0 File Offset: 0x0005C8D0
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this._transaction != null)
					{
						this._transaction.Dispose();
						this._transaction = null;
					}
					if (this._scope != null)
					{
						this._scope = null;
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0005E730 File Offset: 0x0005C930
		~SQLiteEnlistment()
		{
			this.Dispose(false);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005E760 File Offset: 0x0005C960
		public void Commit(Enlistment enlistment)
		{
			this.CheckDisposed();
			SQLiteConnection sqliteConnection = null;
			try
			{
				for (;;)
				{
					sqliteConnection = this._transaction.Connection;
					if (sqliteConnection != null)
					{
						lock (sqliteConnection._enlistmentSyncRoot)
						{
							if (!object.ReferenceEquals(sqliteConnection, this._transaction.Connection))
							{
								continue;
							}
							sqliteConnection._enlistment = null;
							this._transaction.IsValid(true);
							sqliteConnection._transactionLevel = 1;
							this._transaction.Commit();
						}
						break;
					}
					break;
				}
				enlistment.Done();
			}
			finally
			{
				this.Cleanup(sqliteConnection);
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0005E818 File Offset: 0x0005CA18
		public void InDoubt(Enlistment enlistment)
		{
			this.CheckDisposed();
			enlistment.Done();
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0005E828 File Offset: 0x0005CA28
		public void Prepare(PreparingEnlistment preparingEnlistment)
		{
			this.CheckDisposed();
			if (!this._transaction.IsValid(false))
			{
				preparingEnlistment.ForceRollback();
				return;
			}
			preparingEnlistment.Prepared();
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0005E850 File Offset: 0x0005CA50
		public void Rollback(Enlistment enlistment)
		{
			this.CheckDisposed();
			SQLiteConnection sqliteConnection = null;
			try
			{
				for (;;)
				{
					sqliteConnection = this._transaction.Connection;
					if (sqliteConnection != null)
					{
						lock (sqliteConnection._enlistmentSyncRoot)
						{
							if (!object.ReferenceEquals(sqliteConnection, this._transaction.Connection))
							{
								continue;
							}
							sqliteConnection._enlistment = null;
							this._transaction.Rollback();
						}
						break;
					}
					break;
				}
				enlistment.Done();
			}
			finally
			{
				this.Cleanup(sqliteConnection);
			}
		}

		// Token: 0x04000872 RID: 2162
		internal SQLiteTransaction _transaction;

		// Token: 0x04000873 RID: 2163
		internal Transaction _scope;

		// Token: 0x04000874 RID: 2164
		internal bool _disposeConnection;

		// Token: 0x04000875 RID: 2165
		private bool disposed;
	}
}
