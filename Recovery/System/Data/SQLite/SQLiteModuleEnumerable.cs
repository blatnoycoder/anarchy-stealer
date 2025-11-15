using System;
using System.Collections;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001D8 RID: 472
	public class SQLiteModuleEnumerable : SQLiteModuleCommon
	{
		// Token: 0x0600159C RID: 5532 RVA: 0x00062650 File Offset: 0x00060850
		public SQLiteModuleEnumerable(string name, IEnumerable enumerable)
			: this(name, enumerable, false)
		{
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0006265C File Offset: 0x0006085C
		public SQLiteModuleEnumerable(string name, IEnumerable enumerable, bool objectIdentity)
			: base(name)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable");
			}
			this.enumerable = enumerable;
			this.objectIdentity = objectIdentity;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00062684 File Offset: 0x00060884
		protected virtual SQLiteErrorCode CursorEndOfEnumeratorError(SQLiteVirtualTableCursor cursor)
		{
			this.SetCursorError(cursor, "already hit end of enumerator");
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00062694 File Offset: 0x00060894
		public override SQLiteErrorCode Create(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error)
		{
			this.CheckDisposed();
			if (this.DeclareTable(connection, this.GetSqlForDeclareTable(), ref error) == SQLiteErrorCode.Ok)
			{
				table = new SQLiteVirtualTable(arguments);
				return SQLiteErrorCode.Ok;
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000626CC File Offset: 0x000608CC
		public override SQLiteErrorCode Connect(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error)
		{
			this.CheckDisposed();
			if (this.DeclareTable(connection, this.GetSqlForDeclareTable(), ref error) == SQLiteErrorCode.Ok)
			{
				table = new SQLiteVirtualTable(arguments);
				return SQLiteErrorCode.Ok;
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00062704 File Offset: 0x00060904
		public override SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index)
		{
			this.CheckDisposed();
			if (!table.BestIndex(index))
			{
				this.SetTableError(table, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "failed to select best index for virtual table \"{0}\"", new object[] { table.TableName }));
				return SQLiteErrorCode.Error;
			}
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00062754 File Offset: 0x00060954
		public override SQLiteErrorCode Disconnect(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			table.Dispose();
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00062764 File Offset: 0x00060964
		public override SQLiteErrorCode Destroy(SQLiteVirtualTable table)
		{
			this.CheckDisposed();
			table.Dispose();
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00062774 File Offset: 0x00060974
		public override SQLiteErrorCode Open(SQLiteVirtualTable table, ref SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			cursor = new SQLiteVirtualTableCursorEnumerator(table, this.enumerable.GetEnumerator());
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00062790 File Offset: 0x00060990
		public override SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator));
			}
			sqliteVirtualTableCursorEnumerator.Close();
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x000627D0 File Offset: 0x000609D0
		public override SQLiteErrorCode Filter(SQLiteVirtualTableCursor cursor, int indexNumber, string indexString, SQLiteValue[] values)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator));
			}
			sqliteVirtualTableCursorEnumerator.Filter(indexNumber, indexString, values);
			sqliteVirtualTableCursorEnumerator.Reset();
			sqliteVirtualTableCursorEnumerator.MoveNext();
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x00062820 File Offset: 0x00060A20
		public override SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator));
			}
			if (sqliteVirtualTableCursorEnumerator.EndOfEnumerator)
			{
				return this.CursorEndOfEnumeratorError(cursor);
			}
			sqliteVirtualTableCursorEnumerator.MoveNext();
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x00062874 File Offset: 0x00060A74
		public override bool Eof(SQLiteVirtualTableCursor cursor)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.ResultCodeToEofResult(this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator)));
			}
			return sqliteVirtualTableCursorEnumerator.EndOfEnumerator;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x000628B8 File Offset: 0x00060AB8
		public override SQLiteErrorCode Column(SQLiteVirtualTableCursor cursor, SQLiteContext context, int index)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator));
			}
			if (sqliteVirtualTableCursorEnumerator.EndOfEnumerator)
			{
				return this.CursorEndOfEnumeratorError(cursor);
			}
			object obj = sqliteVirtualTableCursorEnumerator.Current;
			if (obj != null)
			{
				context.SetString(this.GetStringFromObject(cursor, obj));
			}
			else
			{
				context.SetNull();
			}
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x00062928 File Offset: 0x00060B28
		public override SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId)
		{
			this.CheckDisposed();
			SQLiteVirtualTableCursorEnumerator sqliteVirtualTableCursorEnumerator = cursor as SQLiteVirtualTableCursorEnumerator;
			if (sqliteVirtualTableCursorEnumerator == null)
			{
				return this.CursorTypeMismatchError(cursor, typeof(SQLiteVirtualTableCursorEnumerator));
			}
			if (sqliteVirtualTableCursorEnumerator.EndOfEnumerator)
			{
				return this.CursorEndOfEnumeratorError(cursor);
			}
			object obj = sqliteVirtualTableCursorEnumerator.Current;
			rowId = this.GetRowIdFromObject(cursor, obj);
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00062984 File Offset: 0x00060B84
		public override SQLiteErrorCode Update(SQLiteVirtualTable table, SQLiteValue[] values, ref long rowId)
		{
			this.CheckDisposed();
			this.SetTableError(table, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "virtual table \"{0}\" is read-only", new object[] { table.TableName }));
			return SQLiteErrorCode.Error;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x000629C4 File Offset: 0x00060BC4
		public override SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName)
		{
			this.CheckDisposed();
			if (!table.Rename(newName))
			{
				this.SetTableError(table, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "failed to rename virtual table from \"{0}\" to \"{1}\"", new object[] { table.TableName, newName }));
				return SQLiteErrorCode.Error;
			}
			return SQLiteErrorCode.Ok;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00062A18 File Offset: 0x00060C18
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteModuleEnumerable).Name);
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00062A3C File Offset: 0x00060C3C
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

		// Token: 0x040008C6 RID: 2246
		private IEnumerable enumerable;

		// Token: 0x040008C7 RID: 2247
		private bool objectIdentity;

		// Token: 0x040008C8 RID: 2248
		private bool disposed;
	}
}
