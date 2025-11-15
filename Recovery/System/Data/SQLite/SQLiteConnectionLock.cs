using System;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001E5 RID: 485
	internal abstract class SQLiteConnectionLock : IDisposable
	{
		// Token: 0x060015E2 RID: 5602 RVA: 0x00062C80 File Offset: 0x00060E80
		public SQLiteConnectionLock(SQLiteConnectionHandle handle, SQLiteConnectionFlags flags, bool autoLock)
		{
			this.handle = handle;
			this.flags = flags;
			if (autoLock)
			{
				this.Lock();
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00062CA4 File Offset: 0x00060EA4
		protected SQLiteConnectionHandle GetHandle()
		{
			return this.handle;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00062CAC File Offset: 0x00060EAC
		protected SQLiteConnectionFlags GetFlags()
		{
			return this.flags;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00062CB4 File Offset: 0x00060EB4
		protected IntPtr GetIntPtr()
		{
			if (this.handle == null)
			{
				throw new InvalidOperationException("Connection lock object has an invalid handle.");
			}
			IntPtr intPtr = this.handle;
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("Connection lock object has an invalid handle pointer.");
			}
			return intPtr;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00062D04 File Offset: 0x00060F04
		public void Lock()
		{
			this.CheckDisposed();
			if (this.statement != IntPtr.Zero)
			{
				return;
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = 0;
				intPtr = SQLiteString.Utf8IntPtrFromString("SELECT 1;", ref num);
				IntPtr zero = IntPtr.Zero;
				int num2 = 0;
				string text = "sqlite3_prepare_interop";
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_prepare_interop(this.GetIntPtr(), intPtr, num, ref this.statement, ref zero, ref num2);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, text);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00062DB0 File Offset: 0x00060FB0
		public void Unlock()
		{
			this.CheckDisposed();
			if (this.statement == IntPtr.Zero)
			{
				return;
			}
			string text = "sqlite3_finalize_interop";
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_finalize_interop(this.statement);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, text);
			}
			this.statement = IntPtr.Zero;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00062E08 File Offset: 0x00061008
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00062E18 File Offset: 0x00061018
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteConnectionLock).Name);
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00062E3C File Offset: 0x0006103C
		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && this.statement != IntPtr.Zero)
				{
					try
					{
						if (HelperMethods.LogPrepare(this.GetFlags()))
						{
							SQLiteLog.LogMessage(SQLiteErrorCode.Misuse, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Connection lock object was {0} with statement {1}", new object[]
							{
								disposing ? "disposed" : "finalized",
								this.statement
							}));
						}
					}
					catch
					{
					}
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00062EF4 File Offset: 0x000610F4
		~SQLiteConnectionLock()
		{
			this.Dispose(false);
		}

		// Token: 0x040008DA RID: 2266
		private const string LockNopSql = "SELECT 1;";

		// Token: 0x040008DB RID: 2267
		private const string StatementMessageFormat = "Connection lock object was {0} with statement {1}";

		// Token: 0x040008DC RID: 2268
		private SQLiteConnectionHandle handle;

		// Token: 0x040008DD RID: 2269
		private SQLiteConnectionFlags flags;

		// Token: 0x040008DE RID: 2270
		private IntPtr statement;

		// Token: 0x040008DF RID: 2271
		private bool disposed;
	}
}
