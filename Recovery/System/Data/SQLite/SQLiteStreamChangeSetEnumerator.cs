using System;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001F2 RID: 498
	internal sealed class SQLiteStreamChangeSetEnumerator : SQLiteChangeSetEnumerator
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00064DE4 File Offset: 0x00062FE4
		public SQLiteStreamChangeSetEnumerator(Stream stream, SQLiteConnectionFlags connectionFlags)
			: base(SQLiteStreamChangeSetIterator.Create(stream, connectionFlags))
		{
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00064DF4 File Offset: 0x00062FF4
		public SQLiteStreamChangeSetEnumerator(Stream stream, SQLiteConnectionFlags connectionFlags, SQLiteChangeSetStartFlags startFlags)
			: base(SQLiteStreamChangeSetIterator.Create(stream, connectionFlags, startFlags))
		{
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00064E04 File Offset: 0x00063004
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteStreamChangeSetEnumerator).Name);
			}
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00064E28 File Offset: 0x00063028
		protected override void Dispose(bool disposing)
		{
			try
			{
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x04000909 RID: 2313
		private bool disposed;
	}
}
