using System;

namespace System.Data.SQLite
{
	// Token: 0x020001F1 RID: 497
	internal sealed class SQLiteMemoryChangeSetEnumerator : SQLiteChangeSetEnumerator
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x00064CF8 File Offset: 0x00062EF8
		public SQLiteMemoryChangeSetEnumerator(byte[] rawData)
			: base(SQLiteMemoryChangeSetIterator.Create(rawData))
		{
			this.rawData = rawData;
			this.flags = SQLiteChangeSetStartFlags.None;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00064D14 File Offset: 0x00062F14
		public SQLiteMemoryChangeSetEnumerator(byte[] rawData, SQLiteChangeSetStartFlags flags)
			: base(SQLiteMemoryChangeSetIterator.Create(rawData, flags))
		{
			this.rawData = rawData;
			this.flags = flags;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00064D34 File Offset: 0x00062F34
		public override void Reset()
		{
			this.CheckDisposed();
			SQLiteMemoryChangeSetIterator sqliteMemoryChangeSetIterator;
			if (this.flags != SQLiteChangeSetStartFlags.None)
			{
				sqliteMemoryChangeSetIterator = SQLiteMemoryChangeSetIterator.Create(this.rawData, this.flags);
			}
			else
			{
				sqliteMemoryChangeSetIterator = SQLiteMemoryChangeSetIterator.Create(this.rawData);
			}
			base.ResetIterator(sqliteMemoryChangeSetIterator);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00064D80 File Offset: 0x00062F80
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteMemoryChangeSetEnumerator).Name);
			}
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00064DA4 File Offset: 0x00062FA4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed)
				{
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x04000906 RID: 2310
		private byte[] rawData;

		// Token: 0x04000907 RID: 2311
		private SQLiteChangeSetStartFlags flags;

		// Token: 0x04000908 RID: 2312
		private bool disposed;
	}
}
