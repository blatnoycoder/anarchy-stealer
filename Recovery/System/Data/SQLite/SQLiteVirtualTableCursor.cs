using System;

namespace System.Data.SQLite
{
	// Token: 0x020001CE RID: 462
	public class SQLiteVirtualTableCursor : ISQLiteNativeHandle, IDisposable
	{
		// Token: 0x060014BC RID: 5308 RVA: 0x0005FCFC File Offset: 0x0005DEFC
		public SQLiteVirtualTableCursor(SQLiteVirtualTable table)
			: this()
		{
			this.table = table;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0005FD0C File Offset: 0x0005DF0C
		private SQLiteVirtualTableCursor()
		{
			this.rowIndex = SQLiteVirtualTableCursor.InvalidRowIndex;
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x0005FD20 File Offset: 0x0005DF20
		public virtual SQLiteVirtualTable Table
		{
			get
			{
				this.CheckDisposed();
				return this.table;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0005FD30 File Offset: 0x0005DF30
		public virtual int IndexNumber
		{
			get
			{
				this.CheckDisposed();
				return this.indexNumber;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0005FD40 File Offset: 0x0005DF40
		public virtual string IndexString
		{
			get
			{
				this.CheckDisposed();
				return this.indexString;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0005FD50 File Offset: 0x0005DF50
		public virtual SQLiteValue[] Values
		{
			get
			{
				this.CheckDisposed();
				return this.values;
			}
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0005FD60 File Offset: 0x0005DF60
		protected virtual int TryPersistValues(SQLiteValue[] values)
		{
			int num = 0;
			if (values != null)
			{
				foreach (SQLiteValue sqliteValue in values)
				{
					if (sqliteValue != null && sqliteValue.Persist())
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0005FDAC File Offset: 0x0005DFAC
		public virtual void Filter(int indexNumber, string indexString, SQLiteValue[] values)
		{
			this.CheckDisposed();
			if (values != null && this.TryPersistValues(values) != values.Length)
			{
				throw new SQLiteException("failed to persist one or more values");
			}
			this.indexNumber = indexNumber;
			this.indexString = indexString;
			this.values = values;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0005FDEC File Offset: 0x0005DFEC
		public virtual int GetRowIndex()
		{
			return this.rowIndex;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0005FDF4 File Offset: 0x0005DFF4
		public virtual void NextRowIndex()
		{
			this.rowIndex++;
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0005FE04 File Offset: 0x0005E004
		// (set) Token: 0x060014C7 RID: 5319 RVA: 0x0005FE14 File Offset: 0x0005E014
		public virtual IntPtr NativeHandle
		{
			get
			{
				this.CheckDisposed();
				return this.nativeHandle;
			}
			internal set
			{
				this.nativeHandle = value;
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0005FE20 File Offset: 0x0005E020
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0005FE30 File Offset: 0x0005E030
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteVirtualTableCursor).Name);
			}
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0005FE54 File Offset: 0x0005E054
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0005FE68 File Offset: 0x0005E068
		~SQLiteVirtualTableCursor()
		{
			this.Dispose(false);
		}

		// Token: 0x040008A8 RID: 2216
		protected static readonly int InvalidRowIndex;

		// Token: 0x040008A9 RID: 2217
		private int rowIndex;

		// Token: 0x040008AA RID: 2218
		private SQLiteVirtualTable table;

		// Token: 0x040008AB RID: 2219
		private int indexNumber;

		// Token: 0x040008AC RID: 2220
		private string indexString;

		// Token: 0x040008AD RID: 2221
		private SQLiteValue[] values;

		// Token: 0x040008AE RID: 2222
		private IntPtr nativeHandle;

		// Token: 0x040008AF RID: 2223
		private bool disposed;
	}
}
