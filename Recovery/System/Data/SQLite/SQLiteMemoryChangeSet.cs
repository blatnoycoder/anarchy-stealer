using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x020001EE RID: 494
	internal sealed class SQLiteMemoryChangeSet : SQLiteChangeSetBase, ISQLiteChangeSet, IEnumerable<ISQLiteChangeSetMetadataItem>, IEnumerable, IDisposable
	{
		// Token: 0x0600163C RID: 5692 RVA: 0x000644B8 File Offset: 0x000626B8
		internal SQLiteMemoryChangeSet(byte[] rawData, SQLiteConnectionHandle handle, SQLiteConnectionFlags connectionFlags)
			: base(handle, connectionFlags)
		{
			this.rawData = rawData;
			this.startFlags = SQLiteChangeSetStartFlags.None;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000644D0 File Offset: 0x000626D0
		internal SQLiteMemoryChangeSet(byte[] rawData, SQLiteConnectionHandle handle, SQLiteConnectionFlags connectionFlags, SQLiteChangeSetStartFlags startFlags)
			: base(handle, connectionFlags)
		{
			this.rawData = rawData;
			this.startFlags = startFlags;
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000644EC File Offset: 0x000626EC
		public ISQLiteChangeSet Invert()
		{
			this.CheckDisposed();
			SQLiteSessionHelpers.CheckRawData(this.rawData);
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			ISQLiteChangeSet isqliteChangeSet;
			try
			{
				int num = 0;
				intPtr = SQLiteBytes.ToIntPtr(this.rawData, ref num);
				int num2 = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_invert(num, intPtr, ref num2, ref intPtr2);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_invert");
				}
				byte[] array = SQLiteBytes.FromIntPtr(intPtr2, num2);
				isqliteChangeSet = new SQLiteMemoryChangeSet(array, base.GetHandle(), base.GetFlags());
			}
			finally
			{
				if (intPtr2 != IntPtr.Zero)
				{
					SQLiteMemory.FreeUntracked(intPtr2);
					intPtr2 = IntPtr.Zero;
				}
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return isqliteChangeSet;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000645B8 File Offset: 0x000627B8
		public ISQLiteChangeSet CombineWith(ISQLiteChangeSet changeSet)
		{
			this.CheckDisposed();
			SQLiteSessionHelpers.CheckRawData(this.rawData);
			SQLiteMemoryChangeSet sqliteMemoryChangeSet = changeSet as SQLiteMemoryChangeSet;
			if (sqliteMemoryChangeSet == null)
			{
				throw new ArgumentException("not a memory based change set", "changeSet");
			}
			SQLiteSessionHelpers.CheckRawData(sqliteMemoryChangeSet.rawData);
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			ISQLiteChangeSet isqliteChangeSet;
			try
			{
				int num = 0;
				intPtr = SQLiteBytes.ToIntPtr(this.rawData, ref num);
				int num2 = 0;
				intPtr2 = SQLiteBytes.ToIntPtr(sqliteMemoryChangeSet.rawData, ref num2);
				int num3 = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_concat(num, intPtr, num2, intPtr2, ref num3, ref intPtr3);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_concat");
				}
				byte[] array = SQLiteBytes.FromIntPtr(intPtr3, num3);
				isqliteChangeSet = new SQLiteMemoryChangeSet(array, base.GetHandle(), base.GetFlags());
			}
			finally
			{
				if (intPtr3 != IntPtr.Zero)
				{
					SQLiteMemory.FreeUntracked(intPtr3);
					intPtr3 = IntPtr.Zero;
				}
				if (intPtr2 != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr2);
					intPtr2 = IntPtr.Zero;
				}
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return isqliteChangeSet;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000646E8 File Offset: 0x000628E8
		public void Apply(SessionConflictCallback conflictCallback, object clientData)
		{
			this.CheckDisposed();
			this.Apply(conflictCallback, null, clientData);
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000646FC File Offset: 0x000628FC
		public void Apply(SessionConflictCallback conflictCallback, SessionTableFilterCallback tableFilterCallback, object clientData)
		{
			this.CheckDisposed();
			SQLiteSessionHelpers.CheckRawData(this.rawData);
			if (conflictCallback == null)
			{
				throw new ArgumentNullException("conflictCallback");
			}
			UnsafeNativeMethods.xSessionFilter @delegate = base.GetDelegate(tableFilterCallback, clientData);
			UnsafeNativeMethods.xSessionConflict delegate2 = base.GetDelegate(conflictCallback, clientData);
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = 0;
				intPtr = SQLiteBytes.ToIntPtr(this.rawData, ref num);
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_apply(base.GetIntPtr(), num, intPtr, @delegate, delegate2, IntPtr.Zero);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_apply");
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

		// Token: 0x06001642 RID: 5698 RVA: 0x000647B0 File Offset: 0x000629B0
		public IEnumerator<ISQLiteChangeSetMetadataItem> GetEnumerator()
		{
			if (this.startFlags != SQLiteChangeSetStartFlags.None)
			{
				return new SQLiteMemoryChangeSetEnumerator(this.rawData, this.startFlags);
			}
			return new SQLiteMemoryChangeSetEnumerator(this.rawData);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x000647DC File Offset: 0x000629DC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x000647E4 File Offset: 0x000629E4
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteMemoryChangeSet).Name);
			}
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00064808 File Offset: 0x00062A08
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing && this.rawData != null)
				{
					this.rawData = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040008FB RID: 2299
		private byte[] rawData;

		// Token: 0x040008FC RID: 2300
		private SQLiteChangeSetStartFlags startFlags;

		// Token: 0x040008FD RID: 2301
		private bool disposed;
	}
}
