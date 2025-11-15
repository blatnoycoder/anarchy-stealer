using System;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001EB RID: 491
	internal sealed class SQLiteChangeGroup : ISQLiteChangeGroup, IDisposable
	{
		// Token: 0x06001611 RID: 5649 RVA: 0x00063944 File Offset: 0x00061B44
		public SQLiteChangeGroup(SQLiteConnectionFlags flags)
		{
			this.flags = flags;
			this.InitializeHandle();
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0006395C File Offset: 0x00061B5C
		private void CheckHandle()
		{
			if (this.changeGroup == IntPtr.Zero)
			{
				throw new InvalidOperationException("change group not open");
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00063980 File Offset: 0x00061B80
		private void InitializeHandle()
		{
			if (this.changeGroup != IntPtr.Zero)
			{
				return;
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changegroup_new(ref this.changeGroup);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3changegroup_new");
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000639C8 File Offset: 0x00061BC8
		private void InitializeStreamManager()
		{
			if (this.streamManager != null)
			{
				return;
			}
			this.streamManager = new SQLiteSessionStreamManager(this.flags);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x000639E8 File Offset: 0x00061BE8
		private SQLiteStreamAdapter GetStreamAdapter(Stream stream)
		{
			this.InitializeStreamManager();
			return this.streamManager.GetAdapter(stream);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x000639FC File Offset: 0x00061BFC
		public void AddChangeSet(byte[] rawData)
		{
			this.CheckDisposed();
			this.CheckHandle();
			SQLiteSessionHelpers.CheckRawData(rawData);
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = 0;
				intPtr = SQLiteBytes.ToIntPtr(rawData, ref num);
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changegroup_add(this.changeGroup, num, intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changegroup_add");
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

		// Token: 0x06001617 RID: 5655 RVA: 0x00063A80 File Offset: 0x00061C80
		public void AddChangeSet(Stream stream)
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
				throw new SQLiteException("could not get or create adapter for input stream");
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changegroup_add_strm(this.changeGroup, streamAdapter.GetInputDelegate(), IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3changegroup_add_strm");
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00063AF0 File Offset: 0x00061CF0
		public void CreateChangeSet(ref byte[] rawData)
		{
			this.CheckDisposed();
			this.CheckHandle();
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changegroup_output(this.changeGroup, ref num, ref intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changegroup_output");
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

		// Token: 0x06001619 RID: 5657 RVA: 0x00063B70 File Offset: 0x00061D70
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
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changegroup_output_strm(this.changeGroup, streamAdapter.GetOutputDelegate(), IntPtr.Zero);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, "sqlite3changegroup_output_strm");
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00063BE0 File Offset: 0x00061DE0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00063BF0 File Offset: 0x00061DF0
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteChangeGroup).Name);
			}
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00063C14 File Offset: 0x00061E14
		private void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed)
				{
					if (disposing && this.streamManager != null)
					{
						this.streamManager.Dispose();
						this.streamManager = null;
					}
					if (this.changeGroup != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3changegroup_delete(this.changeGroup);
						this.changeGroup = IntPtr.Zero;
					}
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00063C98 File Offset: 0x00061E98
		~SQLiteChangeGroup()
		{
			this.Dispose(false);
		}

		// Token: 0x040008EF RID: 2287
		private SQLiteSessionStreamManager streamManager;

		// Token: 0x040008F0 RID: 2288
		private SQLiteConnectionFlags flags;

		// Token: 0x040008F1 RID: 2289
		private IntPtr changeGroup;

		// Token: 0x040008F2 RID: 2290
		private bool disposed;
	}
}
