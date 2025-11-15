using System;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.SQLite
{
	// Token: 0x020001AF RID: 431
	public sealed class SQLiteParameter : DbParameter, ICloneable
	{
		// Token: 0x060012AE RID: 4782 RVA: 0x00059C30 File Offset: 0x00057E30
		internal SQLiteParameter(IDbCommand command)
			: this()
		{
			this._command = command;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00059C40 File Offset: 0x00057E40
		public SQLiteParameter()
			: this(null, (DbType)(-1), 0, null, DataRowVersion.Current)
		{
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00059C54 File Offset: 0x00057E54
		public SQLiteParameter(string parameterName)
			: this(parameterName, (DbType)(-1), 0, null, DataRowVersion.Current)
		{
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00059C68 File Offset: 0x00057E68
		public SQLiteParameter(string parameterName, object value)
			: this(parameterName, (DbType)(-1), 0, null, DataRowVersion.Current)
		{
			this.Value = value;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00059C80 File Offset: 0x00057E80
		public SQLiteParameter(string parameterName, DbType dbType)
			: this(parameterName, dbType, 0, null, DataRowVersion.Current)
		{
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00059C94 File Offset: 0x00057E94
		public SQLiteParameter(string parameterName, DbType dbType, string sourceColumn)
			: this(parameterName, dbType, 0, sourceColumn, DataRowVersion.Current)
		{
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00059CA8 File Offset: 0x00057EA8
		public SQLiteParameter(string parameterName, DbType dbType, string sourceColumn, DataRowVersion rowVersion)
			: this(parameterName, dbType, 0, sourceColumn, rowVersion)
		{
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00059CB8 File Offset: 0x00057EB8
		public SQLiteParameter(DbType dbType)
			: this(null, dbType, 0, null, DataRowVersion.Current)
		{
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00059CCC File Offset: 0x00057ECC
		public SQLiteParameter(DbType dbType, object value)
			: this(null, dbType, 0, null, DataRowVersion.Current)
		{
			this.Value = value;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00059CE4 File Offset: 0x00057EE4
		public SQLiteParameter(DbType dbType, string sourceColumn)
			: this(null, dbType, 0, sourceColumn, DataRowVersion.Current)
		{
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00059CF8 File Offset: 0x00057EF8
		public SQLiteParameter(DbType dbType, string sourceColumn, DataRowVersion rowVersion)
			: this(null, dbType, 0, sourceColumn, rowVersion)
		{
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00059D08 File Offset: 0x00057F08
		public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize)
			: this(parameterName, parameterType, parameterSize, null, DataRowVersion.Current)
		{
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00059D1C File Offset: 0x00057F1C
		public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize, string sourceColumn)
			: this(parameterName, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
		{
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00059D30 File Offset: 0x00057F30
		public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize, string sourceColumn, DataRowVersion rowVersion)
		{
			this._parameterName = parameterName;
			this._dbType = parameterType;
			this._sourceColumn = sourceColumn;
			this._rowVersion = rowVersion;
			this._dataSize = parameterSize;
			this._nullable = true;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00059D64 File Offset: 0x00057F64
		private SQLiteParameter(SQLiteParameter source)
			: this(source.ParameterName, source._dbType, 0, source.Direction, source.IsNullable, 0, 0, source.SourceColumn, source.SourceVersion, source.Value)
		{
			this._nullMapping = source._nullMapping;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00059DB4 File Offset: 0x00057FB4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion rowVersion, object value)
			: this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
		{
			this.Direction = direction;
			this.IsNullable = isNullable;
			this.Value = value;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00059DEC File Offset: 0x00057FEC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion rowVersion, bool sourceColumnNullMapping, object value)
			: this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
		{
			this.Direction = direction;
			this.SourceColumnNullMapping = sourceColumnNullMapping;
			this.Value = value;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00059E24 File Offset: 0x00058024
		public SQLiteParameter(DbType parameterType, int parameterSize)
			: this(null, parameterType, parameterSize, null, DataRowVersion.Current)
		{
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00059E38 File Offset: 0x00058038
		public SQLiteParameter(DbType parameterType, int parameterSize, string sourceColumn)
			: this(null, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
		{
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00059E4C File Offset: 0x0005804C
		public SQLiteParameter(DbType parameterType, int parameterSize, string sourceColumn, DataRowVersion rowVersion)
			: this(null, parameterType, parameterSize, sourceColumn, rowVersion)
		{
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x00059E5C File Offset: 0x0005805C
		// (set) Token: 0x060012C3 RID: 4803 RVA: 0x00059E64 File Offset: 0x00058064
		public IDbCommand Command
		{
			get
			{
				return this._command;
			}
			set
			{
				this._command = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00059E70 File Offset: 0x00058070
		// (set) Token: 0x060012C5 RID: 4805 RVA: 0x00059E78 File Offset: 0x00058078
		public override bool IsNullable
		{
			get
			{
				return this._nullable;
			}
			set
			{
				this._nullable = value;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x00059E84 File Offset: 0x00058084
		// (set) Token: 0x060012C7 RID: 4807 RVA: 0x00059ED8 File Offset: 0x000580D8
		[DbProviderSpecificTypeProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public override DbType DbType
		{
			get
			{
				if (this._dbType != (DbType)(-1))
				{
					return this._dbType;
				}
				if (this._objValue != null && this._objValue != DBNull.Value)
				{
					return SQLiteConvert.TypeToDbType(this._objValue.GetType());
				}
				return DbType.String;
			}
			set
			{
				this._dbType = value;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x00059EE4 File Offset: 0x000580E4
		// (set) Token: 0x060012C9 RID: 4809 RVA: 0x00059EE8 File Offset: 0x000580E8
		public override ParameterDirection Direction
		{
			get
			{
				return ParameterDirection.Input;
			}
			set
			{
				if (value != ParameterDirection.Input)
				{
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x00059EF8 File Offset: 0x000580F8
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x00059F00 File Offset: 0x00058100
		public override string ParameterName
		{
			get
			{
				return this._parameterName;
			}
			set
			{
				this._parameterName = value;
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00059F0C File Offset: 0x0005810C
		public override void ResetDbType()
		{
			this._dbType = (DbType)(-1);
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00059F18 File Offset: 0x00058118
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x00059F20 File Offset: 0x00058120
		[DefaultValue(0)]
		public override int Size
		{
			get
			{
				return this._dataSize;
			}
			set
			{
				this._dataSize = value;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x00059F2C File Offset: 0x0005812C
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x00059F34 File Offset: 0x00058134
		public override string SourceColumn
		{
			get
			{
				return this._sourceColumn;
			}
			set
			{
				this._sourceColumn = value;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x00059F40 File Offset: 0x00058140
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x00059F48 File Offset: 0x00058148
		public override bool SourceColumnNullMapping
		{
			get
			{
				return this._nullMapping;
			}
			set
			{
				this._nullMapping = value;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x00059F54 File Offset: 0x00058154
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x00059F5C File Offset: 0x0005815C
		public override DataRowVersion SourceVersion
		{
			get
			{
				return this._rowVersion;
			}
			set
			{
				this._rowVersion = value;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x00059F68 File Offset: 0x00058168
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x00059F70 File Offset: 0x00058170
		[TypeConverter(typeof(StringConverter))]
		[RefreshProperties(RefreshProperties.All)]
		public override object Value
		{
			get
			{
				return this._objValue;
			}
			set
			{
				this._objValue = value;
				if (this._dbType == (DbType)(-1) && this._objValue != null && this._objValue != DBNull.Value)
				{
					this._dbType = SQLiteConvert.TypeToDbType(this._objValue.GetType());
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x00059FC8 File Offset: 0x000581C8
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x00059FD0 File Offset: 0x000581D0
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
			set
			{
				this._typeName = value;
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00059FDC File Offset: 0x000581DC
		public object Clone()
		{
			return new SQLiteParameter(this);
		}

		// Token: 0x040007FC RID: 2044
		private const DbType UnknownDbType = (DbType)(-1);

		// Token: 0x040007FD RID: 2045
		private IDbCommand _command;

		// Token: 0x040007FE RID: 2046
		internal DbType _dbType;

		// Token: 0x040007FF RID: 2047
		private DataRowVersion _rowVersion;

		// Token: 0x04000800 RID: 2048
		private object _objValue;

		// Token: 0x04000801 RID: 2049
		private string _sourceColumn;

		// Token: 0x04000802 RID: 2050
		private string _parameterName;

		// Token: 0x04000803 RID: 2051
		private int _dataSize;

		// Token: 0x04000804 RID: 2052
		private bool _nullable;

		// Token: 0x04000805 RID: 2053
		private bool _nullMapping;

		// Token: 0x04000806 RID: 2054
		private string _typeName;
	}
}
