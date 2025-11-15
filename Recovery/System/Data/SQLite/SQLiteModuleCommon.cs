using System;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001D6 RID: 470
	public class SQLiteModuleCommon : SQLiteModuleNoop
	{
		// Token: 0x06001588 RID: 5512 RVA: 0x00062338 File Offset: 0x00060538
		public SQLiteModuleCommon(string name)
			: this(name, false)
		{
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00062344 File Offset: 0x00060544
		public SQLiteModuleCommon(string name, bool objectIdentity)
			: base(name)
		{
			this.objectIdentity = objectIdentity;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00062354 File Offset: 0x00060554
		protected virtual string GetSqlForDeclareTable()
		{
			return SQLiteModuleCommon.declareSql;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0006235C File Offset: 0x0006055C
		protected virtual SQLiteErrorCode CursorTypeMismatchError(SQLiteVirtualTableCursor cursor, Type type)
		{
			if (type != null)
			{
				this.SetCursorError(cursor, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "not a \"{0}\" cursor", new object[] { type }));
			}
			else
			{
				this.SetCursorError(cursor, "cursor type mismatch");
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x000623B0 File Offset: 0x000605B0
		protected virtual string GetStringFromObject(SQLiteVirtualTableCursor cursor, object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is string)
			{
				return (string)value;
			}
			return value.ToString();
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000623D4 File Offset: 0x000605D4
		protected virtual long MakeRowId(int rowIndex, int hashCode)
		{
			long num = (long)rowIndex;
			num <<= 32;
			return num | (long)((ulong)hashCode);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000623F4 File Offset: 0x000605F4
		protected virtual long GetRowIdFromObject(SQLiteVirtualTableCursor cursor, object value)
		{
			int num = ((cursor != null) ? cursor.GetRowIndex() : 0);
			int hashCode = SQLiteMarshal.GetHashCode(value, this.objectIdentity);
			return this.MakeRowId(num, hashCode);
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00062430 File Offset: 0x00060630
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteModuleCommon).Name);
			}
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00062454 File Offset: 0x00060654
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

		// Token: 0x040008C0 RID: 2240
		private static readonly string declareSql = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "CREATE TABLE {0}(x);", new object[] { typeof(SQLiteModuleCommon).Name });

		// Token: 0x040008C1 RID: 2241
		private bool objectIdentity;

		// Token: 0x040008C2 RID: 2242
		private bool disposed;
	}
}
