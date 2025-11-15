using System;

namespace System.Data.SQLite
{
	// Token: 0x020001A1 RID: 417
	public class SQLiteFunctionEx : SQLiteFunction
	{
		// Token: 0x0600124B RID: 4683 RVA: 0x000579E0 File Offset: 0x00055BE0
		protected CollationSequence GetCollationSequence()
		{
			return this._base.GetCollationSequence(this, this._context);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x000579F4 File Offset: 0x00055BF4
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteFunctionEx).Name);
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00057A18 File Offset: 0x00055C18
		protected override void Dispose(bool disposing)
		{
			try
			{
				bool flag = this.disposed;
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040007C6 RID: 1990
		private bool disposed;
	}
}
