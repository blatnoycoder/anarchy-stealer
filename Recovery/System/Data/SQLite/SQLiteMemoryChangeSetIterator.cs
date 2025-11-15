using System;

namespace System.Data.SQLite
{
	// Token: 0x020001E7 RID: 487
	internal sealed class SQLiteMemoryChangeSetIterator : SQLiteChangeSetIterator
	{
		// Token: 0x060015F5 RID: 5621 RVA: 0x000630A8 File Offset: 0x000612A8
		private SQLiteMemoryChangeSetIterator(IntPtr pData, IntPtr iterator, bool ownHandle)
			: base(iterator, ownHandle)
		{
			this.pData = pData;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x000630BC File Offset: 0x000612BC
		public static SQLiteMemoryChangeSetIterator Create(byte[] rawData)
		{
			SQLiteSessionHelpers.CheckRawData(rawData);
			SQLiteMemoryChangeSetIterator sqliteMemoryChangeSetIterator = null;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int num = 0;
				intPtr = SQLiteBytes.ToIntPtr(rawData, ref num);
				if (intPtr == IntPtr.Zero)
				{
					throw new SQLiteException(SQLiteErrorCode.NoMem, null);
				}
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_start(ref intPtr2, num, intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_start");
				}
				sqliteMemoryChangeSetIterator = new SQLiteMemoryChangeSetIterator(intPtr, intPtr2, true);
			}
			finally
			{
				if (sqliteMemoryChangeSetIterator == null)
				{
					if (intPtr2 != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3changeset_finalize(intPtr2);
						intPtr2 = IntPtr.Zero;
					}
					if (intPtr != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr);
						intPtr = IntPtr.Zero;
					}
				}
			}
			return sqliteMemoryChangeSetIterator;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00063180 File Offset: 0x00061380
		public static SQLiteMemoryChangeSetIterator Create(byte[] rawData, SQLiteChangeSetStartFlags flags)
		{
			SQLiteSessionHelpers.CheckRawData(rawData);
			SQLiteMemoryChangeSetIterator sqliteMemoryChangeSetIterator = null;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int num = 0;
				intPtr = SQLiteBytes.ToIntPtr(rawData, ref num);
				if (intPtr == IntPtr.Zero)
				{
					throw new SQLiteException(SQLiteErrorCode.NoMem, null);
				}
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_start_v2(ref intPtr2, num, intPtr, flags);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_start_v2");
				}
				sqliteMemoryChangeSetIterator = new SQLiteMemoryChangeSetIterator(intPtr, intPtr2, true);
			}
			finally
			{
				if (sqliteMemoryChangeSetIterator == null)
				{
					if (intPtr2 != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3changeset_finalize(intPtr2);
						intPtr2 = IntPtr.Zero;
					}
					if (intPtr != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr);
						intPtr = IntPtr.Zero;
					}
				}
			}
			return sqliteMemoryChangeSetIterator;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00063244 File Offset: 0x00061444
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteMemoryChangeSetIterator).Name);
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00063268 File Offset: 0x00061468
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			try
			{
				if (!this.disposed && this.pData != IntPtr.Zero)
				{
					SQLiteMemory.Free(this.pData);
					this.pData = IntPtr.Zero;
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x040008E3 RID: 2275
		private IntPtr pData;

		// Token: 0x040008E4 RID: 2276
		private bool disposed;
	}
}
