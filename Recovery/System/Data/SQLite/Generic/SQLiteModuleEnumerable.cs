using System;
using System.Collections.Generic;

namespace System.Data.SQLite.Generic
{
	// Token: 0x020001DA RID: 474
	public class SQLiteModuleEnumerable<T> : SQLiteModuleEnumerable
	{
		// Token: 0x060015B4 RID: 5556 RVA: 0x00062B4C File Offset: 0x00060D4C
		public SQLiteModuleEnumerable(string name, IEnumerable<T> enumerable)
			: base(name, enumerable)
		{
			this.enumerable = enumerable;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00062B60 File Offset: 0x00060D60
		public override SQLiteErrorCode Open(SQLiteVirtualTable table, ref SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			cursor = new SQLiteVirtualTableCursorEnumerator<T>(table, this.enumerable.GetEnumerator());
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00062B7C File Offset: 0x00060D7C
		public override SQLiteErrorCode Column(SQLiteVirtualTableCursor cursor, SQLiteContext context, int index)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator<T> sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator<T>;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator));
			}
			if (sqliteVirtualTableCursorEnumerator.EndOfEnumerator)
			{
				return this.CursorEndOfEnumeratorError(cursor);
			}
			T t = ((IEnumerator<T>)sqliteVirtualTableCursorEnumerator).Current;
			if (t != null)
			{
				context.SetString(this.GetStringFromObject(cursor, t));
			}
			else
			{
				context.SetNull();
			}
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00062BF8 File Offset: 0x00060DF8
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteModuleEnumerable<T>).Name);
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x00062C1C File Offset: 0x00060E1C
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

		// Token: 0x040008CB RID: 2251
		private IEnumerable<T> enumerable;

		// Token: 0x040008CC RID: 2252
		private bool disposed;
	}
}
