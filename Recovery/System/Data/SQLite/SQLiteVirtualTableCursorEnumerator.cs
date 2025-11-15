using System;
using System.Collections;

namespace System.Data.SQLite
{
	// Token: 0x020001D7 RID: 471
	public class SQLiteVirtualTableCursorEnumerator : SQLiteVirtualTableCursor, IEnumerator
	{
		// Token: 0x06001592 RID: 5522 RVA: 0x000624CC File Offset: 0x000606CC
		public SQLiteVirtualTableCursorEnumerator(SQLiteVirtualTable table, IEnumerator enumerator)
			: base(table)
		{
			this.enumerator = enumerator;
			this.endOfEnumerator = true;
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000624E4 File Offset: 0x000606E4
		public virtual bool MoveNext()
		{
			this.CheckDisposed();
			this.CheckClosed();
			if (this.enumerator == null)
			{
				return false;
			}
			this.endOfEnumerator = !this.enumerator.MoveNext();
			if (!this.endOfEnumerator)
			{
				this.NextRowIndex();
			}
			return !this.endOfEnumerator;
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x0006253C File Offset: 0x0006073C
		public virtual object Current
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				if (this.enumerator == null)
				{
					return null;
				}
				return this.enumerator.Current;
			}
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00062564 File Offset: 0x00060764
		public virtual void Reset()
		{
			this.CheckDisposed();
			this.CheckClosed();
			if (this.enumerator == null)
			{
				return;
			}
			this.enumerator.Reset();
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x0006258C File Offset: 0x0006078C
		public virtual bool EndOfEnumerator
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				return this.endOfEnumerator;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001597 RID: 5527 RVA: 0x000625A0 File Offset: 0x000607A0
		public virtual bool IsOpen
		{
			get
			{
				this.CheckDisposed();
				return this.enumerator != null;
			}
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x000625B4 File Offset: 0x000607B4
		public virtual void Close()
		{
			if (this.enumerator != null)
			{
				this.enumerator = null;
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x000625C8 File Offset: 0x000607C8
		public virtual void CheckClosed()
		{
			this.CheckDisposed();
			if (!this.IsOpen)
			{
				throw new InvalidOperationException("virtual table cursor is closed");
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000625E8 File Offset: 0x000607E8
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteVirtualTableCursorEnumerator).Name);
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0006260C File Offset: 0x0006080C
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

		// Token: 0x040008C3 RID: 2243
		private IEnumerator enumerator;

		// Token: 0x040008C4 RID: 2244
		private bool endOfEnumerator;

		// Token: 0x040008C5 RID: 2245
		private bool disposed;
	}
}
