using System;
using System.Globalization;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001EC RID: 492
	internal sealed class SQLiteSession : SQLiteConnectionLock, ISQLiteSession, IDisposable
	{
		// Token: 0x0600161E RID: 5662 RVA: 0x00063CC8 File Offset: 0x00061EC8
		public SQLiteSession(SQLiteConnectionHandle handle, SQLiteConnectionFlags flags, string databaseName)
			: base(handle, flags, true)
		{
			this.databaseName = databaseName;
			this.InitializeHandle();
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00063CE0 File Offset: 0x00061EE0
		private void CheckHandle()
		{
			if (this.session == IntPtr.Zero)
			{
				throw new InvalidOperationException("session is not open");
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00063D04 File Offset: 0x00061F04
		private void InitializeHandle()
		{
			if (this.session != IntPtr.Zero)
			{
				return;
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_create(base.GetIntPtr(), SQLiteString.GetUtf8BytesFromString(this.databaseName), ref this.session);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3session_create");
			}
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00063D5C File Offset: 0x00061F5C
		private UnsafeNativeMethods.xSessionFilter ApplyTableFilter(SessionTableFilterCallback callback, object clientData)
		{
			this.tableFilterCallback = callback;
			this.tableFilterClientData = clientData;
			if (callback == null)
			{
				if (this.xFilter != null)
				{
					this.xFilter = null;
				}
				return null;
			}
			if (this.xFilter == null)
			{
				this.xFilter = new UnsafeNativeMethods.xSessionFilter(this.Filter);
			}
			return this.xFilter;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00063DB8 File Offset: 0x00061FB8
		private void InitializeStreamManager()
		{
			if (this.streamManager != null)
			{
				return;
			}
			this.streamManager = new SQLiteSessionStreamManager(base.GetFlags());
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00063DD8 File Offset: 0x00061FD8
		private SQLiteStreamAdapter GetStreamAdapter(Stream stream)
		{
			this.InitializeStreamManager();
			return this.streamManager.GetAdapter(stream);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00063DEC File Offset: 0x00061FEC
		private int Filter(IntPtr context, IntPtr pTblName)
		{
			try
			{
				return this.tableFilterCallback(this.tableFilterClientData, SQLiteString.StringFromUtf8IntPtr(pTblName)) ? 1 : 0;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(base.GetFlags()))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "xSessionFilter", ex }));
					}
				}
				catch
				{
				}
			}
			return 0;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00063E90 File Offset: 0x00062090
		public bool IsEnabled()
		{
			this.CheckDisposed();
			this.CheckHandle();
			return UnsafeNativeMethods.sqlite3session_enable(this.session, -1) != 0;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00063EB0 File Offset: 0x000620B0
		public void SetToEnabled()
		{
			this.CheckDisposed();
			this.CheckHandle();
			UnsafeNativeMethods.sqlite3session_enable(this.session, 1);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00063ECC File Offset: 0x000620CC
		public void SetToDisabled()
		{
			this.CheckDisposed();
			this.CheckHandle();
			UnsafeNativeMethods.sqlite3session_enable(this.session, 0);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00063EE8 File Offset: 0x000620E8
		public bool IsIndirect()
		{
			this.CheckDisposed();
			this.CheckHandle();
			return UnsafeNativeMethods.sqlite3session_indirect(this.session, -1) != 0;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00063F08 File Offset: 0x00062108
		public void SetToIndirect()
		{
			this.CheckDisposed();
			this.CheckHandle();
			UnsafeNativeMethods.sqlite3session_indirect(this.session, 1);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00063F24 File Offset: 0x00062124
		public void SetToDirect()
		{
			this.CheckDisposed();
			this.CheckHandle();
			UnsafeNativeMethods.sqlite3session_indirect(this.session, 0);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00063F40 File Offset: 0x00062140
		public bool IsEmpty()
		{
			this.CheckDisposed();
			this.CheckHandle();
			return UnsafeNativeMethods.sqlite3session_isempty(this.session) != 0;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00063F60 File Offset: 0x00062160
		public long GetMemoryBytesInUse()
		{
			this.CheckDisposed();
			this.CheckHandle();
			return UnsafeNativeMethods.sqlite3session_memory_used(this.session);
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00063F7C File Offset: 0x0006217C
		public void AttachTable(string name)
		{
			this.CheckDisposed();
			this.CheckHandle();
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_attach(this.session, SQLiteString.GetUtf8BytesFromString(name));
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3session_attach");
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00063FC0 File Offset: 0x000621C0
		public void SetTableFilter(SessionTableFilterCallback callback, object clientData)
		{
			this.CheckDisposed();
			this.CheckHandle();
			UnsafeNativeMethods.sqlite3session_table_filter(this.session, this.ApplyTableFilter(callback, clientData), IntPtr.Zero);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00063FF8 File Offset: 0x000621F8
		public void CreateChangeSet(ref byte[] rawData)
		{
			this.CheckDisposed();
			this.CheckHandle();
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_changeset(this.session, ref num, ref intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3session_changeset");
				}
				rawData = SQLiteBytes.FromIntPtr(intPtr, num);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.FreeUntracked(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00064078 File Offset: 0x00062278
		public void CreateChangeSet(Stream stream)
		{
			this.CheckDisposed();
			this.CheckHandle();
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			SQLiteStreamAdapter streamAdapter = this.GetStreamAdapter(stream);
			if (streamAdapter == null)
			{
				throw new SQLiteException("could not get or create adapter for output stream");
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_changeset_strm(this.session, streamAdapter.GetOutputDelegate(), IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3session_changeset_strm");
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000640E8 File Offset: 0x000622E8
		public void CreatePatchSet(ref byte[] rawData)
		{
			this.CheckDisposed();
			this.CheckHandle();
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_patchset(this.session, ref num, ref intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3session_patchset");
				}
				rawData = SQLiteBytes.FromIntPtr(intPtr, num);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.FreeUntracked(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00064168 File Offset: 0x00062368
		public void CreatePatchSet(Stream stream)
		{
			this.CheckDisposed();
			this.CheckHandle();
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			SQLiteStreamAdapter streamAdapter = this.GetStreamAdapter(stream);
			if (streamAdapter == null)
			{
				throw new SQLiteException("could not get or create adapter for output stream");
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_patchset_strm(this.session, streamAdapter.GetOutputDelegate(), IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3session_patchset_strm");
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x000641D8 File Offset: 0x000623D8
		public void LoadDifferencesFromTable(string fromDatabaseName, string tableName)
		{
			this.CheckDisposed();
			this.CheckHandle();
			if (fromDatabaseName == null)
			{
				throw new ArgumentNullException("fromDatabaseName");
			}
			if (tableName == null)
			{
				throw new ArgumentNullException("tableName");
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3session_diff(this.session, SQLiteString.GetUtf8BytesFromString(fromDatabaseName), SQLiteString.GetUtf8BytesFromString(tableName), ref intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					string text = null;
					if (intPtr != IntPtr.Zero)
					{
						text = SQLiteString.StringFromUtf8IntPtr(intPtr);
						if (!string.IsNullOrEmpty(text))
						{
							text = HelperMethods.StringFormat(CultureInfo.CurrentCulture, ": {0}", new object[] { text });
						}
					}
					throw new SQLiteException(sqliteErrorCode, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "{0}{1}", new object[] { "sqlite3session_diff", text }));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.FreeUntracked(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x000642DC File Offset: 0x000624DC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteSession).Name);
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00064300 File Offset: 0x00062500
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed)
				{
					if (disposing)
					{
						if (this.xFilter != null)
						{
							this.xFilter = null;
						}
						if (this.streamManager != null)
						{
							this.streamManager.Dispose();
							this.streamManager = null;
						}
					}
					if (this.session != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3session_delete(this.session);
						this.session = IntPtr.Zero;
					}
					base.Unlock();
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040008F3 RID: 2291
		private SQLiteSessionStreamManager streamManager;

		// Token: 0x040008F4 RID: 2292
		private string databaseName;

		// Token: 0x040008F5 RID: 2293
		private IntPtr session;

		// Token: 0x040008F6 RID: 2294
		private UnsafeNativeMethods.xSessionFilter xFilter;

		// Token: 0x040008F7 RID: 2295
		private SessionTableFilterCallback tableFilterCallback;

		// Token: 0x040008F8 RID: 2296
		private object tableFilterClientData;

		// Token: 0x040008F9 RID: 2297
		private bool disposed;
	}
}
