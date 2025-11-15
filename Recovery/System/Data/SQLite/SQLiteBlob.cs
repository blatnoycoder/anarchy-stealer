using System;

namespace System.Data.SQLite
{
	// Token: 0x02000158 RID: 344
	public sealed class SQLiteBlob : IDisposable
	{
		// Token: 0x06000F4A RID: 3914 RVA: 0x00047598 File Offset: 0x00045798
		private SQLiteBlob(SQLiteBase sqlbase, SQLiteBlobHandle blob)
		{
			this._sql = sqlbase;
			this._sqlite_blob = blob;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000475B0 File Offset: 0x000457B0
		public static SQLiteBlob Create(SQLiteDataReader dataReader, int i, bool readOnly)
		{
			if (dataReader == null)
			{
				throw new ArgumentNullException("dataReader");
			}
			long? rowId = dataReader.GetRowId(i);
			if (rowId == null)
			{
				throw new InvalidOperationException("No RowId is available");
			}
			return SQLiteBlob.Create(SQLiteDataReader.GetConnection(dataReader), dataReader.GetDatabaseName(i), dataReader.GetTableName(i), dataReader.GetName(i), rowId.Value, readOnly);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0004761C File Offset: 0x0004581C
		public static SQLiteBlob Create(SQLiteConnection connection, string databaseName, string tableName, string columnName, long rowId, bool readOnly)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			SQLite3 sqlite = connection._sql as SQLite3;
			if (sqlite == null)
			{
				throw new InvalidOperationException("Connection has no wrapper");
			}
			SQLiteConnectionHandle sql = sqlite._sql;
			if (sql == null)
			{
				throw new InvalidOperationException("Connection has an invalid handle.");
			}
			SQLiteBlobHandle sqliteBlobHandle = null;
			try
			{
			}
			finally
			{
				IntPtr zero = IntPtr.Zero;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_blob_open(sql, SQLiteConvert.ToUTF8(databaseName), SQLiteConvert.ToUTF8(tableName), SQLiteConvert.ToUTF8(columnName), rowId, readOnly ? 0 : 1, ref zero);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, null);
				}
				sqliteBlobHandle = new SQLiteBlobHandle(sql, zero);
			}
			SQLiteConnection.OnChanged(connection, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, sqliteBlobHandle, null, new object[]
			{
				typeof(SQLiteBlob),
				databaseName,
				tableName,
				columnName,
				rowId,
				readOnly
			}));
			return new SQLiteBlob(sqlite, sqliteBlobHandle);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00047730 File Offset: 0x00045930
		private void CheckOpen()
		{
			if (this._sqlite_blob == IntPtr.Zero)
			{
				throw new InvalidOperationException("Blob is not open");
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00047758 File Offset: 0x00045958
		private void VerifyParameters(byte[] buffer, int count, int offset)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentException("Negative offset not allowed.");
			}
			if (count < 0)
			{
				throw new ArgumentException("Negative count not allowed.");
			}
			if (count > buffer.Length)
			{
				throw new ArgumentException("Buffer is too small.");
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000477B4 File Offset: 0x000459B4
		public void Reopen(long rowId)
		{
			this.CheckDisposed();
			this.CheckOpen();
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_blob_reopen(this._sqlite_blob, rowId);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				this.Dispose();
				throw new SQLiteException(sqliteErrorCode, null);
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x000477F8 File Offset: 0x000459F8
		public int GetCount()
		{
			this.CheckDisposed();
			this.CheckOpen();
			return UnsafeNativeMethods.sqlite3_blob_bytes(this._sqlite_blob);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00047818 File Offset: 0x00045A18
		public void Read(byte[] buffer, int count, int offset)
		{
			this.CheckDisposed();
			this.CheckOpen();
			this.VerifyParameters(buffer, count, offset);
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_blob_read(this._sqlite_blob, buffer, count, offset);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, null);
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00047860 File Offset: 0x00045A60
		public void Write(byte[] buffer, int count, int offset)
		{
			this.CheckDisposed();
			this.CheckOpen();
			this.VerifyParameters(buffer, count, offset);
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_blob_write(this._sqlite_blob, buffer, count, offset);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, null);
			}
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x000478A8 File Offset: 0x00045AA8
		public void Close()
		{
			this.Dispose();
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x000478B0 File Offset: 0x00045AB0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x000478C0 File Offset: 0x00045AC0
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteBlob).Name);
			}
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x000478E4 File Offset: 0x00045AE4
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this._sqlite_blob != null)
					{
						this._sqlite_blob.Dispose();
						this._sqlite_blob = null;
					}
					this._sql = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00047924 File Offset: 0x00045B24
		~SQLiteBlob()
		{
			this.Dispose(false);
		}

		// Token: 0x040005FA RID: 1530
		internal SQLiteBase _sql;

		// Token: 0x040005FB RID: 1531
		internal SQLiteBlobHandle _sqlite_blob;

		// Token: 0x040005FC RID: 1532
		private bool disposed;
	}
}
