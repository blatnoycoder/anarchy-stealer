using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001EF RID: 495
	internal sealed class SQLiteStreamChangeSet : SQLiteChangeSetBase, ISQLiteChangeSet, IEnumerable<ISQLiteChangeSetMetadataItem>, IEnumerable, IDisposable
	{
		// Token: 0x06001646 RID: 5702 RVA: 0x0006485C File Offset: 0x00062A5C
		internal SQLiteStreamChangeSet(Stream inputStream, Stream outputStream, SQLiteConnectionHandle handle, SQLiteConnectionFlags connectionFlags)
			: base(handle, connectionFlags)
		{
			this.inputStream = inputStream;
			this.outputStream = outputStream;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00064878 File Offset: 0x00062A78
		internal SQLiteStreamChangeSet(Stream inputStream, Stream outputStream, SQLiteConnectionHandle handle, SQLiteConnectionFlags connectionFlags, SQLiteChangeSetStartFlags startFlags)
			: base(handle, connectionFlags)
		{
			this.inputStream = inputStream;
			this.outputStream = outputStream;
			this.startFlags = startFlags;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0006489C File Offset: 0x00062A9C
		private void CheckInputStream()
		{
			if (this.inputStream == null)
			{
				throw new InvalidOperationException("input stream unavailable");
			}
			if (this.inputStreamAdapter == null)
			{
				this.inputStreamAdapter = new SQLiteStreamAdapter(this.inputStream, base.GetFlags());
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x000648D8 File Offset: 0x00062AD8
		private void CheckOutputStream()
		{
			if (this.outputStream == null)
			{
				throw new InvalidOperationException("output stream unavailable");
			}
			if (this.outputStreamAdapter == null)
			{
				this.outputStreamAdapter = new SQLiteStreamAdapter(this.outputStream, base.GetFlags());
			}
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00064914 File Offset: 0x00062B14
		public ISQLiteChangeSet Invert()
		{
			this.CheckDisposed();
			this.CheckInputStream();
			this.CheckOutputStream();
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_invert_strm(this.inputStreamAdapter.GetInputDelegate(), IntPtr.Zero, this.outputStreamAdapter.GetOutputDelegate(), IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_invert_strm");
			}
			return null;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00064970 File Offset: 0x00062B70
		public ISQLiteChangeSet CombineWith(ISQLiteChangeSet changeSet)
		{
			this.CheckDisposed();
			this.CheckInputStream();
			this.CheckOutputStream();
			SQLiteStreamChangeSet sqliteStreamChangeSet = changeSet as SQLiteStreamChangeSet;
			if (sqliteStreamChangeSet == null)
			{
				throw new ArgumentException("not a stream based change set", "changeSet");
			}
			sqliteStreamChangeSet.CheckInputStream();
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_concat_strm(this.inputStreamAdapter.GetInputDelegate(), IntPtr.Zero, sqliteStreamChangeSet.inputStreamAdapter.GetInputDelegate(), IntPtr.Zero, this.outputStreamAdapter.GetOutputDelegate(), IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_concat_strm");
			}
			return null;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00064A00 File Offset: 0x00062C00
		public void Apply(SessionConflictCallback conflictCallback, object clientData)
		{
			this.CheckDisposed();
			this.Apply(conflictCallback, null, clientData);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00064A14 File Offset: 0x00062C14
		public void Apply(SessionConflictCallback conflictCallback, SessionTableFilterCallback tableFilterCallback, object clientData)
		{
			this.CheckDisposed();
			this.CheckInputStream();
			if (conflictCallback == null)
			{
				throw new ArgumentNullException("conflictCallback");
			}
			UnsafeNativeMethods.xSessionFilter @delegate = base.GetDelegate(tableFilterCallback, clientData);
			UnsafeNativeMethods.xSessionConflict delegate2 = base.GetDelegate(conflictCallback, clientData);
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_apply_strm(base.GetIntPtr(), this.inputStreamAdapter.GetInputDelegate(), IntPtr.Zero, @delegate, delegate2, IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_apply_strm");
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00064A8C File Offset: 0x00062C8C
		public IEnumerator<ISQLiteChangeSetMetadataItem> GetEnumerator()
		{
			if (this.startFlags != SQLiteChangeSetStartFlags.None)
			{
				return new SQLiteStreamChangeSetEnumerator(this.inputStream, base.GetFlags(), this.startFlags);
			}
			return new SQLiteStreamChangeSetEnumerator(this.inputStream, base.GetFlags());
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00064AC4 File Offset: 0x00062CC4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00064ACC File Offset: 0x00062CCC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteStreamChangeSet).Name);
			}
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00064AF0 File Offset: 0x00062CF0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing)
				{
					if (this.outputStreamAdapter != null)
					{
						this.outputStreamAdapter.Dispose();
						this.outputStreamAdapter = null;
					}
					if (this.inputStreamAdapter != null)
					{
						this.inputStreamAdapter.Dispose();
						this.inputStreamAdapter = null;
					}
					if (this.outputStream != null)
					{
						this.outputStream = null;
					}
					if (this.inputStream != null)
					{
						this.inputStream = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040008FE RID: 2302
		private SQLiteStreamAdapter inputStreamAdapter;

		// Token: 0x040008FF RID: 2303
		private SQLiteStreamAdapter outputStreamAdapter;

		// Token: 0x04000900 RID: 2304
		private Stream inputStream;

		// Token: 0x04000901 RID: 2305
		private Stream outputStream;

		// Token: 0x04000902 RID: 2306
		private SQLiteChangeSetStartFlags startFlags;

		// Token: 0x04000903 RID: 2307
		private bool disposed;
	}
}
