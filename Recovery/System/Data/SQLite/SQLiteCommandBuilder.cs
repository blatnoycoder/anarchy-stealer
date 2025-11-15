using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x0200015A RID: 346
	public sealed class SQLiteCommandBuilder : DbCommandBuilder
	{
		// Token: 0x06000F90 RID: 3984 RVA: 0x00048830 File Offset: 0x00046A30
		public SQLiteCommandBuilder()
			: this(null)
		{
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0004883C File Offset: 0x00046A3C
		public SQLiteCommandBuilder(SQLiteDataAdapter adp)
		{
			this.QuotePrefix = "[";
			this.QuoteSuffix = "]";
			this.DataAdapter = adp;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00048870 File Offset: 0x00046A70
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteCommandBuilder).Name);
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00048894 File Offset: 0x00046A94
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

		// Token: 0x06000F94 RID: 3988 RVA: 0x000488CC File Offset: 0x00046ACC
		protected override void ApplyParameterInfo(DbParameter parameter, DataRow row, StatementType statementType, bool whereClause)
		{
			SQLiteParameter sqliteParameter = (SQLiteParameter)parameter;
			sqliteParameter.DbType = (DbType)row[SchemaTableColumn.ProviderType];
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000488FC File Offset: 0x00046AFC
		protected override string GetParameterName(string parameterName)
		{
			return HelperMethods.StringFormat(CultureInfo.InvariantCulture, "@{0}", new object[] { parameterName });
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00048928 File Offset: 0x00046B28
		protected override string GetParameterName(int parameterOrdinal)
		{
			return HelperMethods.StringFormat(CultureInfo.InvariantCulture, "@param{0}", new object[] { parameterOrdinal });
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004895C File Offset: 0x00046B5C
		protected override string GetParameterPlaceholder(int parameterOrdinal)
		{
			return this.GetParameterName(parameterOrdinal);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00048968 File Offset: 0x00046B68
		protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
		{
			if (adapter == base.DataAdapter)
			{
				((SQLiteDataAdapter)adapter).RowUpdating -= this.RowUpdatingEventHandler;
				return;
			}
			((SQLiteDataAdapter)adapter).RowUpdating += this.RowUpdatingEventHandler;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000489A8 File Offset: 0x00046BA8
		private void RowUpdatingEventHandler(object sender, RowUpdatingEventArgs e)
		{
			base.RowUpdatingHandler(e);
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x000489B4 File Offset: 0x00046BB4
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x000489C8 File Offset: 0x00046BC8
		public new SQLiteDataAdapter DataAdapter
		{
			get
			{
				this.CheckDisposed();
				return (SQLiteDataAdapter)base.DataAdapter;
			}
			set
			{
				this.CheckDisposed();
				base.DataAdapter = value;
			}
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x000489D8 File Offset: 0x00046BD8
		public new SQLiteCommand GetDeleteCommand()
		{
			this.CheckDisposed();
			return (SQLiteCommand)base.GetDeleteCommand();
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x000489EC File Offset: 0x00046BEC
		public new SQLiteCommand GetDeleteCommand(bool useColumnsForParameterNames)
		{
			this.CheckDisposed();
			return (SQLiteCommand)base.GetDeleteCommand(useColumnsForParameterNames);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00048A00 File Offset: 0x00046C00
		public new SQLiteCommand GetUpdateCommand()
		{
			this.CheckDisposed();
			return (SQLiteCommand)base.GetUpdateCommand();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00048A14 File Offset: 0x00046C14
		public new SQLiteCommand GetUpdateCommand(bool useColumnsForParameterNames)
		{
			this.CheckDisposed();
			return (SQLiteCommand)base.GetUpdateCommand(useColumnsForParameterNames);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00048A28 File Offset: 0x00046C28
		public new SQLiteCommand GetInsertCommand()
		{
			this.CheckDisposed();
			return (SQLiteCommand)base.GetInsertCommand();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00048A3C File Offset: 0x00046C3C
		public new SQLiteCommand GetInsertCommand(bool useColumnsForParameterNames)
		{
			this.CheckDisposed();
			return (SQLiteCommand)base.GetInsertCommand(useColumnsForParameterNames);
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00048A50 File Offset: 0x00046C50
		// (set) Token: 0x06000FA3 RID: 4003 RVA: 0x00048A60 File Offset: 0x00046C60
		[Browsable(false)]
		public override CatalogLocation CatalogLocation
		{
			get
			{
				this.CheckDisposed();
				return base.CatalogLocation;
			}
			set
			{
				this.CheckDisposed();
				base.CatalogLocation = value;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x00048A70 File Offset: 0x00046C70
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x00048A80 File Offset: 0x00046C80
		[Browsable(false)]
		public override string CatalogSeparator
		{
			get
			{
				this.CheckDisposed();
				return base.CatalogSeparator;
			}
			set
			{
				this.CheckDisposed();
				base.CatalogSeparator = value;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00048A90 File Offset: 0x00046C90
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x00048AA0 File Offset: 0x00046CA0
		[DefaultValue("[")]
		[Browsable(false)]
		public override string QuotePrefix
		{
			get
			{
				this.CheckDisposed();
				return base.QuotePrefix;
			}
			set
			{
				this.CheckDisposed();
				base.QuotePrefix = value;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00048AB0 File Offset: 0x00046CB0
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x00048AC0 File Offset: 0x00046CC0
		[Browsable(false)]
		public override string QuoteSuffix
		{
			get
			{
				this.CheckDisposed();
				return base.QuoteSuffix;
			}
			set
			{
				this.CheckDisposed();
				base.QuoteSuffix = value;
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00048AD0 File Offset: 0x00046CD0
		public override string QuoteIdentifier(string unquotedIdentifier)
		{
			this.CheckDisposed();
			if (string.IsNullOrEmpty(this.QuotePrefix) || string.IsNullOrEmpty(this.QuoteSuffix) || string.IsNullOrEmpty(unquotedIdentifier))
			{
				return unquotedIdentifier;
			}
			return this.QuotePrefix + unquotedIdentifier.Replace(this.QuoteSuffix, this.QuoteSuffix + this.QuoteSuffix) + this.QuoteSuffix;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00048B44 File Offset: 0x00046D44
		public override string UnquoteIdentifier(string quotedIdentifier)
		{
			this.CheckDisposed();
			if (string.IsNullOrEmpty(this.QuotePrefix) || string.IsNullOrEmpty(this.QuoteSuffix) || string.IsNullOrEmpty(quotedIdentifier))
			{
				return quotedIdentifier;
			}
			if (!quotedIdentifier.StartsWith(this.QuotePrefix, StringComparison.OrdinalIgnoreCase) || !quotedIdentifier.EndsWith(this.QuoteSuffix, StringComparison.OrdinalIgnoreCase))
			{
				return quotedIdentifier;
			}
			return quotedIdentifier.Substring(this.QuotePrefix.Length, quotedIdentifier.Length - (this.QuotePrefix.Length + this.QuoteSuffix.Length)).Replace(this.QuoteSuffix + this.QuoteSuffix, this.QuoteSuffix);
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00048BFC File Offset: 0x00046DFC
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x00048C0C File Offset: 0x00046E0C
		[Browsable(false)]
		public override string SchemaSeparator
		{
			get
			{
				this.CheckDisposed();
				return base.SchemaSeparator;
			}
			set
			{
				this.CheckDisposed();
				base.SchemaSeparator = value;
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00048C1C File Offset: 0x00046E1C
		protected override DataTable GetSchemaTable(DbCommand sourceCommand)
		{
			DataTable dataTable;
			using (IDataReader dataReader = sourceCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
			{
				DataTable schemaTable = dataReader.GetSchemaTable();
				if (this.HasSchemaPrimaryKey(schemaTable))
				{
					this.ResetIsUniqueSchemaColumn(schemaTable);
				}
				dataTable = schemaTable;
			}
			return dataTable;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00048C70 File Offset: 0x00046E70
		private bool HasSchemaPrimaryKey(DataTable schema)
		{
			DataColumn dataColumn = schema.Columns[SchemaTableColumn.IsKey];
			foreach (object obj in schema.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if ((bool)dataRow[dataColumn])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00048CFC File Offset: 0x00046EFC
		private void ResetIsUniqueSchemaColumn(DataTable schema)
		{
			DataColumn dataColumn = schema.Columns[SchemaTableColumn.IsUnique];
			DataColumn dataColumn2 = schema.Columns[SchemaTableColumn.IsKey];
			foreach (object obj in schema.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if (!(bool)dataRow[dataColumn2])
				{
					dataRow[dataColumn] = false;
				}
			}
			schema.AcceptChanges();
		}

		// Token: 0x0400060A RID: 1546
		private bool disposed;
	}
}
