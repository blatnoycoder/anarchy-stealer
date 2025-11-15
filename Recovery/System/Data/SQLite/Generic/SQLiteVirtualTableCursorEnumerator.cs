using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite.Generic
{
	// Token: 0x020001D9 RID: 473
	public class SQLiteVirtualTableCursorEnumerator<T> : SQLiteVirtualTableCursorEnumerator, IEnumerator<T>, IDisposable, IEnumerator
	{
		// Token: 0x060015AF RID: 5551 RVA: 0x00062A74 File Offset: 0x00060C74
		public SQLiteVirtualTableCursorEnumerator(SQLiteVirtualTable table, IEnumerator<T> enumerator)
			: base(table, enumerator)
		{
			this.enumerator = enumerator;
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00062A88 File Offset: 0x00060C88
		T IEnumerator<T>.Current
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				if (this.enumerator == null)
				{
					return default(T);
				}
				return this.enumerator.Current;
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00062AC8 File Offset: 0x00060CC8
		public override void Close()
		{
			if (this.enumerator != null)
			{
				this.enumerator = null;
			}
			base.Close();
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00062AE4 File Offset: 0x00060CE4
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteVirtualTableCursorEnumerator<T>).Name);
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00062B08 File Offset: 0x00060D08
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed)
				{
					this.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040008C9 RID: 2249
		private IEnumerator<T> enumerator;

		// Token: 0x040008CA RID: 2250
		private bool disposed;
	}
}
