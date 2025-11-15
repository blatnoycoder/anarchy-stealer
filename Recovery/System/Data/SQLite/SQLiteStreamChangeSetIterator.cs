using System;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001E8 RID: 488
	internal sealed class SQLiteStreamChangeSetIterator : SQLiteChangeSetIterator
	{
		// Token: 0x060015FA RID: 5626 RVA: 0x000632D0 File Offset: 0x000614D0
		private SQLiteStreamChangeSetIterator(SQLiteStreamAdapter streamAdapter, IntPtr iterator, bool ownHandle)
			: base(iterator, ownHandle)
		{
			this.streamAdapter = streamAdapter;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x000632E4 File Offset: 0x000614E4
		public static SQLiteStreamChangeSetIterator Create(Stream stream, SQLiteConnectionFlags connectionFlags)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			SQLiteStreamAdapter sqliteStreamAdapter = null;
			SQLiteStreamChangeSetIterator sqliteStreamChangeSetIterator = null;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				sqliteStreamAdapter = new SQLiteStreamAdapter(stream, connectionFlags);
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_start_strm(ref intPtr, sqliteStreamAdapter.GetInputDelegate(), IntPtr.Zero);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_start_strm");
				}
				sqliteStreamChangeSetIterator = new SQLiteStreamChangeSetIterator(sqliteStreamAdapter, intPtr, true);
			}
			finally
			{
				if (sqliteStreamChangeSetIterator == null)
				{
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3changeset_finalize(intPtr);
						intPtr = IntPtr.Zero;
					}
					if (sqliteStreamAdapter != null)
					{
						sqliteStreamAdapter.Dispose();
						sqliteStreamAdapter = null;
					}
				}
			}
			return sqliteStreamChangeSetIterator;
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0006338C File Offset: 0x0006158C
		public static SQLiteStreamChangeSetIterator Create(Stream stream, SQLiteConnectionFlags connectionFlags, SQLiteChangeSetStartFlags startFlags)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			SQLiteStreamAdapter sqliteStreamAdapter = null;
			SQLiteStreamChangeSetIterator sqliteStreamChangeSetIterator = null;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				sqliteStreamAdapter = new SQLiteStreamAdapter(stream, connectionFlags);
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_start_v2_strm(ref intPtr, sqliteStreamAdapter.GetInputDelegate(), IntPtr.Zero, startFlags);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_start_v2_strm");
				}
				sqliteStreamChangeSetIterator = new SQLiteStreamChangeSetIterator(sqliteStreamAdapter, intPtr, true);
			}
			finally
			{
				if (sqliteStreamChangeSetIterator == null)
				{
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3changeset_finalize(intPtr);
						intPtr = IntPtr.Zero;
					}
					if (sqliteStreamAdapter != null)
					{
						sqliteStreamAdapter.Dispose();
						sqliteStreamAdapter = null;
					}
				}
			}
			return sqliteStreamChangeSetIterator;
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00063434 File Offset: 0x00061634
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteStreamChangeSetIterator).Name);
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00063458 File Offset: 0x00061658
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

		// Token: 0x040008E5 RID: 2277
		private SQLiteStreamAdapter streamAdapter;

		// Token: 0x040008E6 RID: 2278
		private bool disposed;
	}
}
