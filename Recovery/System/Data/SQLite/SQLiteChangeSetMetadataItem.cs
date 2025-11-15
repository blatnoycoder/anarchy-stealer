using System;

namespace System.Data.SQLite
{
	// Token: 0x020001F3 RID: 499
	internal sealed class SQLiteChangeSetMetadataItem : ISQLiteChangeSetMetadataItem, IDisposable
	{
		// Token: 0x06001668 RID: 5736 RVA: 0x00064E5C File Offset: 0x0006305C
		public SQLiteChangeSetMetadataItem(SQLiteChangeSetIterator iterator)
		{
			this.iterator = iterator;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00064E6C File Offset: 0x0006306C
		private void CheckIterator()
		{
			if (this.iterator == null)
			{
				throw new InvalidOperationException("iterator unavailable");
			}
			this.iterator.CheckHandle();
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00064E90 File Offset: 0x00063090
		private void PopulateOperationMetadata()
		{
			if (this.tableName == null || this.numberOfColumns == null || this.operationCode == null || this.indirect == null)
			{
				this.CheckIterator();
				IntPtr zero = IntPtr.Zero;
				SQLiteAuthorizerActionCode sqliteAuthorizerActionCode = SQLiteAuthorizerActionCode.None;
				int num = 0;
				int num2 = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_op(this.iterator.GetIntPtr(), ref zero, ref num2, ref sqliteAuthorizerActionCode, ref num);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_op");
				}
				this.tableName = SQLiteString.StringFromUtf8IntPtr(zero);
				this.numberOfColumns = new int?(num2);
				this.operationCode = new SQLiteAuthorizerActionCode?(sqliteAuthorizerActionCode);
				this.indirect = new bool?(num != 0);
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00064F54 File Offset: 0x00063154
		private void PopulatePrimaryKeyColumns()
		{
			if (this.primaryKeyColumns == null)
			{
				this.CheckIterator();
				IntPtr zero = IntPtr.Zero;
				int num = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_pk(this.iterator.GetIntPtr(), ref zero, ref num);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_pk");
				}
				byte[] array = SQLiteBytes.FromIntPtr(zero, num);
				if (array != null)
				{
					this.primaryKeyColumns = new bool[num];
					for (int i = 0; i < array.Length; i++)
					{
						this.primaryKeyColumns[i] = array[i] != 0;
					}
				}
			}
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00064FE8 File Offset: 0x000631E8
		private void PopulateNumberOfForeignKeyConflicts()
		{
			if (this.numberOfForeignKeyConflicts == null)
			{
				this.CheckIterator();
				int num = 0;
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3changeset_fk_conflicts(this.iterator.GetIntPtr(), ref num);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, "sqlite3changeset_fk_conflicts");
				}
				this.numberOfForeignKeyConflicts = new int?(num);
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x00065044 File Offset: 0x00063244
		public string TableName
		{
			get
			{
				this.CheckDisposed();
				this.PopulateOperationMetadata();
				return this.tableName;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00065058 File Offset: 0x00063258
		public int NumberOfColumns
		{
			get
			{
				this.CheckDisposed();
				this.PopulateOperationMetadata();
				return this.numberOfColumns.Value;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x00065074 File Offset: 0x00063274
		public SQLiteAuthorizerActionCode OperationCode
		{
			get
			{
				this.CheckDisposed();
				this.PopulateOperationMetadata();
				return this.operationCode.Value;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00065090 File Offset: 0x00063290
		public bool Indirect
		{
			get
			{
				this.CheckDisposed();
				this.PopulateOperationMetadata();
				return this.indirect.Value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x000650AC File Offset: 0x000632AC
		public bool[] PrimaryKeyColumns
		{
			get
			{
				this.CheckDisposed();
				this.PopulatePrimaryKeyColumns();
				return this.primaryKeyColumns;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x000650C0 File Offset: 0x000632C0
		public int NumberOfForeignKeyConflicts
		{
			get
			{
				this.CheckDisposed();
				this.PopulateNumberOfForeignKeyConflicts();
				return this.numberOfForeignKeyConflicts.Value;
			}
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000650DC File Offset: 0x000632DC
		public SQLiteValue GetOldValue(int columnIndex)
		{
			this.CheckDisposed();
			this.CheckIterator();
			IntPtr zero = IntPtr.Zero;
			UnsafeNativeMethods.sqlite3changeset_old(this.iterator.GetIntPtr(), columnIndex, ref zero);
			return SQLiteValue.FromIntPtr(zero);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0006511C File Offset: 0x0006331C
		public SQLiteValue GetNewValue(int columnIndex)
		{
			this.CheckDisposed();
			this.CheckIterator();
			IntPtr zero = IntPtr.Zero;
			UnsafeNativeMethods.sqlite3changeset_new(this.iterator.GetIntPtr(), columnIndex, ref zero);
			return SQLiteValue.FromIntPtr(zero);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0006515C File Offset: 0x0006335C
		public SQLiteValue GetConflictValue(int columnIndex)
		{
			this.CheckDisposed();
			this.CheckIterator();
			IntPtr zero = IntPtr.Zero;
			UnsafeNativeMethods.sqlite3changeset_conflict(this.iterator.GetIntPtr(), columnIndex, ref zero);
			return SQLiteValue.FromIntPtr(zero);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0006519C File Offset: 0x0006339C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000651AC File Offset: 0x000633AC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteChangeSetMetadataItem).Name);
			}
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000651D0 File Offset: 0x000633D0
		private void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing && this.iterator != null)
				{
					this.iterator = null;
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00065220 File Offset: 0x00063420
		~SQLiteChangeSetMetadataItem()
		{
			this.Dispose(false);
		}

		// Token: 0x0400090A RID: 2314
		private SQLiteChangeSetIterator iterator;

		// Token: 0x0400090B RID: 2315
		private string tableName;

		// Token: 0x0400090C RID: 2316
		private int? numberOfColumns;

		// Token: 0x0400090D RID: 2317
		private SQLiteAuthorizerActionCode? operationCode;

		// Token: 0x0400090E RID: 2318
		private bool? indirect;

		// Token: 0x0400090F RID: 2319
		private bool[] primaryKeyColumns;

		// Token: 0x04000910 RID: 2320
		private int? numberOfForeignKeyConflicts;

		// Token: 0x04000911 RID: 2321
		private bool disposed;
	}
}
