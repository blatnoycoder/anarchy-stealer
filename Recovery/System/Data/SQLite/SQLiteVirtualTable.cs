using System;

namespace System.Data.SQLite
{
	// Token: 0x020001CD RID: 461
	public class SQLiteVirtualTable : ISQLiteNativeHandle, IDisposable
	{
		// Token: 0x060014AE RID: 5294 RVA: 0x0005FB4C File Offset: 0x0005DD4C
		public SQLiteVirtualTable(string[] arguments)
		{
			this.arguments = arguments;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0005FB5C File Offset: 0x0005DD5C
		public virtual string[] Arguments
		{
			get
			{
				this.CheckDisposed();
				return this.arguments;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0005FB6C File Offset: 0x0005DD6C
		public virtual string ModuleName
		{
			get
			{
				this.CheckDisposed();
				string[] array = this.Arguments;
				if (array != null && array.Length > 0)
				{
					return array[0];
				}
				return null;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0005FBA4 File Offset: 0x0005DDA4
		public virtual string DatabaseName
		{
			get
			{
				this.CheckDisposed();
				string[] array = this.Arguments;
				if (array != null && array.Length > 1)
				{
					return array[1];
				}
				return null;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0005FBDC File Offset: 0x0005DDDC
		public virtual string TableName
		{
			get
			{
				this.CheckDisposed();
				string[] array = this.Arguments;
				if (array != null && array.Length > 2)
				{
					return array[2];
				}
				return null;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0005FC14 File Offset: 0x0005DE14
		public virtual SQLiteIndex Index
		{
			get
			{
				this.CheckDisposed();
				return this.index;
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0005FC24 File Offset: 0x0005DE24
		public virtual bool BestIndex(SQLiteIndex index)
		{
			this.CheckDisposed();
			this.index = index;
			return true;
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0005FC34 File Offset: 0x0005DE34
		public virtual bool Rename(string name)
		{
			this.CheckDisposed();
			if (this.arguments != null && this.arguments.Length > 2)
			{
				this.arguments[2] = name;
				return true;
			}
			return false;
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0005FC68 File Offset: 0x0005DE68
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x0005FC78 File Offset: 0x0005DE78
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

		// Token: 0x060014B8 RID: 5304 RVA: 0x0005FC84 File Offset: 0x0005DE84
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005FC94 File Offset: 0x0005DE94
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteVirtualTable).Name);
			}
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0005FCB8 File Offset: 0x0005DEB8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0005FCCC File Offset: 0x0005DECC
		~SQLiteVirtualTable()
		{
			this.Dispose(false);
		}

		// Token: 0x040008A1 RID: 2209
		private const int ModuleNameIndex = 0;

		// Token: 0x040008A2 RID: 2210
		private const int DatabaseNameIndex = 1;

		// Token: 0x040008A3 RID: 2211
		private const int TableNameIndex = 2;

		// Token: 0x040008A4 RID: 2212
		private string[] arguments;

		// Token: 0x040008A5 RID: 2213
		private SQLiteIndex index;

		// Token: 0x040008A6 RID: 2214
		private IntPtr nativeHandle;

		// Token: 0x040008A7 RID: 2215
		private bool disposed;
	}
}
