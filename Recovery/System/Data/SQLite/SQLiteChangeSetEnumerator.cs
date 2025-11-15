using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x020001F0 RID: 496
	internal abstract class SQLiteChangeSetEnumerator : IEnumerator<ISQLiteChangeSetMetadataItem>, IDisposable, IEnumerator
	{
		// Token: 0x06001652 RID: 5714 RVA: 0x00064B90 File Offset: 0x00062D90
		public SQLiteChangeSetEnumerator(SQLiteChangeSetIterator iterator)
		{
			this.SetIterator(iterator);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00064BA0 File Offset: 0x00062DA0
		private void CheckIterator()
		{
			if (this.iterator == null)
			{
				throw new InvalidOperationException("iterator unavailable");
			}
			this.iterator.CheckHandle();
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00064BC4 File Offset: 0x00062DC4
		private void SetIterator(SQLiteChangeSetIterator iterator)
		{
			this.iterator = iterator;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00064BD0 File Offset: 0x00062DD0
		private void CloseIterator()
		{
			if (this.iterator != null)
			{
				this.iterator.Dispose();
				this.iterator = null;
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00064BF0 File Offset: 0x00062DF0
		protected void ResetIterator(SQLiteChangeSetIterator iterator)
		{
			this.CloseIterator();
			this.SetIterator(iterator);
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x00064C00 File Offset: 0x00062E00
		public ISQLiteChangeSetMetadataItem Current
		{
			get
			{
				this.CheckDisposed();
				return new SQLiteChangeSetMetadataItem(this.iterator);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x00064C14 File Offset: 0x00062E14
		object IEnumerator.Current
		{
			get
			{
				this.CheckDisposed();
				return this.Current;
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00064C24 File Offset: 0x00062E24
		public bool MoveNext()
		{
			this.CheckDisposed();
			this.CheckIterator();
			return this.iterator.Next();
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00064C40 File Offset: 0x00062E40
		public virtual void Reset()
		{
			this.CheckDisposed();
			throw new NotImplementedException();
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00064C50 File Offset: 0x00062E50
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00064C60 File Offset: 0x00062E60
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteChangeSetEnumerator).Name);
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00064C84 File Offset: 0x00062E84
		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing)
				{
					this.CloseIterator();
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00064CC8 File Offset: 0x00062EC8
		~SQLiteChangeSetEnumerator()
		{
			this.Dispose(false);
		}

		// Token: 0x04000904 RID: 2308
		private SQLiteChangeSetIterator iterator;

		// Token: 0x04000905 RID: 2309
		private bool disposed;
	}
}
