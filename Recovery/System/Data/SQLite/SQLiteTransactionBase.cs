using System;
using System.Data.Common;

namespace System.Data.SQLite
{
	// Token: 0x020001B2 RID: 434
	public abstract class SQLiteTransactionBase : DbTransaction
	{
		// Token: 0x0600130A RID: 4874 RVA: 0x0005AF54 File Offset: 0x00059154
		internal SQLiteTransactionBase(SQLiteConnection connection, bool deferredLock)
		{
			this._cnn = connection;
			this._version = this._cnn._version;
			this._level = (deferredLock ? IsolationLevel.ReadCommitted : IsolationLevel.Serializable);
			this.Begin(deferredLock);
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x0005AFA8 File Offset: 0x000591A8
		public override IsolationLevel IsolationLevel
		{
			get
			{
				this.CheckDisposed();
				return this._level;
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0005AFB8 File Offset: 0x000591B8
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteTransactionBase).Name);
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0005AFDC File Offset: 0x000591DC
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing && this.IsValid(false))
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

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x0005B034 File Offset: 0x00059234
		public new SQLiteConnection Connection
		{
			get
			{
				this.CheckDisposed();
				return this._cnn;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x0005B044 File Offset: 0x00059244
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0005B04C File Offset: 0x0005924C
		public override void Rollback()
		{
			this.CheckDisposed();
			this.IsValid(true);
			this.IssueRollback(true);
		}

		// Token: 0x06001311 RID: 4881
		protected abstract void Begin(bool deferredLock);

		// Token: 0x06001312 RID: 4882
		protected abstract void IssueRollback(bool throwError);

		// Token: 0x06001313 RID: 4883 RVA: 0x0005B064 File Offset: 0x00059264
		internal bool IsValid(bool throwError)
		{
			if (this._cnn == null)
			{
				if (throwError)
				{
					throw new ArgumentNullException("No connection associated with this transaction");
				}
				return false;
			}
			else if (this._cnn._version != this._version)
			{
				if (throwError)
				{
					throw new SQLiteException("The connection was closed and re-opened, changes were already rolled back");
				}
				return false;
			}
			else if (this._cnn.State != ConnectionState.Open)
			{
				if (throwError)
				{
					throw new SQLiteException("Connection was closed");
				}
				return false;
			}
			else
			{
				if (this._cnn._transactionLevel != 0 && !this._cnn._sql.AutoCommit)
				{
					return true;
				}
				this._cnn._transactionLevel = 0;
				if (throwError)
				{
					throw new SQLiteException("No transaction is active on this connection");
				}
				return false;
			}
		}

		// Token: 0x04000814 RID: 2068
		internal SQLiteConnection _cnn;

		// Token: 0x04000815 RID: 2069
		internal int _version;

		// Token: 0x04000816 RID: 2070
		private IsolationLevel _level;

		// Token: 0x04000817 RID: 2071
		private bool disposed;
	}
}
