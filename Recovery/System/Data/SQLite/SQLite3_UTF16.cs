using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x0200014E RID: 334
	internal sealed class SQLite3_UTF16 : SQLite3
	{
		// Token: 0x06000F32 RID: 3890 RVA: 0x00046D98 File Offset: 0x00044F98
		internal SQLite3_UTF16(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString, IntPtr db, string fileName, bool ownHandle)
			: base(fmt, kind, fmtString, db, fileName, ownHandle)
		{
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00046DAC File Offset: 0x00044FAC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLite3_UTF16).Name);
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00046DD0 File Offset: 0x00044FD0
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

		// Token: 0x06000F35 RID: 3893 RVA: 0x00046E08 File Offset: 0x00045008
		public override string ToString(IntPtr b, int nbytelen)
		{
			this.CheckDisposed();
			return SQLite3_UTF16.UTF16ToString(b, nbytelen);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00046E18 File Offset: 0x00045018
		public static string UTF16ToString(IntPtr b, int nbytelen)
		{
			if (nbytelen == 0 || b == IntPtr.Zero)
			{
				return string.Empty;
			}
			if (nbytelen == -1)
			{
				return Marshal.PtrToStringUni(b);
			}
			return Marshal.PtrToStringUni(b, nbytelen / 2);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00046E50 File Offset: 0x00045050
		internal override void Open(string strFilename, string vfsName, SQLiteConnectionFlags connectionFlags, SQLiteOpenFlagsEnum openFlags, int maxPoolSize, bool usePool)
		{
			SQLiteBase.BumpOpenCount();
			if (this._sql != null)
			{
				this.Close(false);
			}
			if (this._sql != null)
			{
				throw new SQLiteException("connection handle is still active");
			}
			this._maxPoolSize = maxPoolSize;
			this._usePool = usePool;
			if (this._usePool && !SQLite3.IsAllowedToUsePool(openFlags))
			{
				this._usePool = false;
			}
			this._fileName = strFilename;
			this._returnToFileName = strFilename;
			this._flags = connectionFlags;
			if (usePool)
			{
				this._sql = SQLiteConnectionPool.Remove(strFilename, maxPoolSize, out this._poolVersion);
				SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.OpenedFromPool, null, null, null, null, this._sql, strFilename, new object[]
				{
					typeof(SQLite3_UTF16),
					strFilename,
					vfsName,
					connectionFlags,
					openFlags,
					maxPoolSize,
					usePool,
					this._poolVersion
				}));
			}
			if (this._sql == null)
			{
				try
				{
				}
				finally
				{
					IntPtr intPtr = IntPtr.Zero;
					uint id = (uint)Process.GetCurrentProcess().Id;
					if (IntPtr.Size == 8)
					{
						intPtr = new IntPtr((long)(15153171595752648096UL | (ulong)id));
					}
					else
					{
						intPtr = new IntPtr((int)(157298080U | id));
					}
					int num = 0;
					if (!HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoExtensionFunctions))
					{
						num |= 1;
					}
					if (HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoCoreFunctions))
					{
						num |= 2;
					}
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_open16_interop(SQLiteConvert.ToUTF8(strFilename), SQLiteConvert.ToUTF8(vfsName), openFlags, num, ref intPtr);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, null);
					}
					this._sql = new SQLiteConnectionHandle(intPtr, true);
					SQLiteBase.BumpCreateCount();
				}
				lock (this._sql)
				{
				}
				SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, this._sql, strFilename, new object[]
				{
					typeof(SQLite3_UTF16),
					strFilename,
					vfsName,
					connectionFlags,
					openFlags,
					maxPoolSize,
					usePool
				}));
			}
			if (!HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoBindFunctions))
			{
				if (this._functions == null)
				{
					this._functions = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
				}
				foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> keyValuePair in SQLiteFunction.BindFunctions(this, connectionFlags))
				{
					this._functions[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			this.SetTimeout(0);
			GC.KeepAlive(this._sql);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00047150 File Offset: 0x00045350
		internal override void Bind_DateTime(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, DateTime dt)
		{
			switch (this._datetimeFormat)
			{
			case SQLiteDateFormats.Ticks:
			case SQLiteDateFormats.JulianDay:
			case SQLiteDateFormats.UnixEpoch:
				base.Bind_DateTime(stmt, flags, index, dt);
				return;
			}
			if (HelperMethods.LogBind(flags))
			{
				SQLiteStatementHandle sqliteStatementHandle = ((stmt != null) ? stmt._sqlite_stmt : null);
				SQLite3.LogBind(sqliteStatementHandle, index, dt);
			}
			this.Bind_Text(stmt, flags, index, base.ToString(dt));
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x000471C8 File Offset: 0x000453C8
		internal override void Bind_Text(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, string value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_text16(sqlite_stmt, index, value, value.Length * 2, (IntPtr)(-1));
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00047228 File Offset: 0x00045428
		internal override DateTime GetDateTime(SQLiteStatement stmt, int index)
		{
			if (this._datetimeFormat == SQLiteDateFormats.Ticks)
			{
				return SQLiteConvert.TicksToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
			}
			if (this._datetimeFormat == SQLiteDateFormats.JulianDay)
			{
				return SQLiteConvert.ToDateTime(this.GetDouble(stmt, index), this._datetimeKind);
			}
			if (this._datetimeFormat == SQLiteDateFormats.UnixEpoch)
			{
				return SQLiteConvert.UnixEpochToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
			}
			return base.ToDateTime(this.GetText(stmt, index));
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000472A8 File Offset: 0x000454A8
		internal override string ColumnName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			IntPtr intPtr = UnsafeNativeMethods.sqlite3_column_name16_interop(stmt._sqlite_stmt, index, ref num);
			if (intPtr == IntPtr.Zero)
			{
				throw new SQLiteException(SQLiteErrorCode.NoMem, this.GetLastError());
			}
			return SQLite3_UTF16.UTF16ToString(intPtr, num);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x000472F4 File Offset: 0x000454F4
		internal override string ColumnType(SQLiteStatement stmt, int index, ref TypeAffinity nAffinity)
		{
			int num = 0;
			IntPtr intPtr = UnsafeNativeMethods.sqlite3_column_decltype16_interop(stmt._sqlite_stmt, index, ref num);
			nAffinity = this.ColumnAffinity(stmt, index);
			if (intPtr != IntPtr.Zero && (num > 0 || num == -1))
			{
				string text = SQLite3_UTF16.UTF16ToString(intPtr, num);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			string[] typeDefinitions = stmt.TypeDefinitions;
			if (typeDefinitions != null && index < typeDefinitions.Length && typeDefinitions[index] != null)
			{
				return typeDefinitions[index];
			}
			return string.Empty;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00047388 File Offset: 0x00045588
		internal override string GetText(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_text16_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x000473B4 File Offset: 0x000455B4
		internal override string ColumnOriginalName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_origin_name16_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000473E0 File Offset: 0x000455E0
		internal override string ColumnDatabaseName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_database_name16_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0004740C File Offset: 0x0004560C
		internal override string ColumnTableName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_table_name16_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00047438 File Offset: 0x00045638
		internal override string GetParamValueText(IntPtr ptr)
		{
			int num = 0;
			return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_value_text16_interop(ptr, ref num), num);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0004745C File Offset: 0x0004565C
		internal override void ReturnError(IntPtr context, string value)
		{
			UnsafeNativeMethods.sqlite3_result_error16(context, value, value.Length * 2);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00047470 File Offset: 0x00045670
		internal override void ReturnText(IntPtr context, string value)
		{
			UnsafeNativeMethods.sqlite3_result_text16(context, value, value.Length * 2, (IntPtr)(-1));
		}

		// Token: 0x04000556 RID: 1366
		private bool disposed;
	}
}
