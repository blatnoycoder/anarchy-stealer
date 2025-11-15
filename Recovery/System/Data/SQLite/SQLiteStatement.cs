using System;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001B1 RID: 433
	internal sealed class SQLiteStatement : IDisposable
	{
		// Token: 0x060012FD RID: 4861 RVA: 0x0005A520 File Offset: 0x00058720
		internal SQLiteStatement(SQLiteBase sqlbase, SQLiteConnectionFlags flags, SQLiteStatementHandle stmt, string strCommand, SQLiteStatement previous)
		{
			this._sql = sqlbase;
			this._sqlite_stmt = stmt;
			this._sqlStatement = strCommand;
			this._flags = flags;
			int num = 0;
			int num2 = this._sql.Bind_ParamCount(this, this._flags);
			if (num2 > 0)
			{
				if (previous != null)
				{
					num = previous._unnamedParameters;
				}
				this._paramNames = new string[num2];
				this._paramValues = new SQLiteParameter[num2];
				for (int i = 0; i < num2; i++)
				{
					string text = this._sql.Bind_ParamName(this, this._flags, i + 1);
					if (string.IsNullOrEmpty(text))
					{
						text = HelperMethods.StringFormat(CultureInfo.InvariantCulture, ";{0}", new object[] { num });
						num++;
						this._unnamedParameters++;
					}
					this._paramNames[i] = text;
					this._paramValues[i] = null;
				}
			}
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0005A618 File Offset: 0x00058818
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0005A628 File Offset: 0x00058828
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteStatement).Name);
			}
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0005A64C File Offset: 0x0005884C
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this._sqlite_stmt != null)
					{
						this._sqlite_stmt.Dispose();
						this._sqlite_stmt = null;
					}
					this._paramNames = null;
					this._paramValues = null;
					this._sql = null;
					this._sqlStatement = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0005A6B0 File Offset: 0x000588B0
		~SQLiteStatement()
		{
			this.Dispose(false);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0005A6E0 File Offset: 0x000588E0
		internal bool TryGetChanges(ref int changes, ref bool readOnly)
		{
			if (this._sql != null && this._sql.IsOpen())
			{
				changes = this._sql.Changes;
				readOnly = this._sql.IsReadOnly(this);
				return true;
			}
			return false;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0005A71C File Offset: 0x0005891C
		internal bool MapParameter(string s, SQLiteParameter p)
		{
			if (this._paramNames == null)
			{
				return false;
			}
			int num = 0;
			if (s.Length > 0 && ":$@;".IndexOf(s[0]) == -1)
			{
				num = 1;
			}
			int num2 = this._paramNames.Length;
			for (int i = 0; i < num2; i++)
			{
				if (string.Compare(this._paramNames[i], num, s, 0, Math.Max(this._paramNames[i].Length - num, s.Length), StringComparison.OrdinalIgnoreCase) == 0)
				{
					this._paramValues[i] = p;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0005A7C4 File Offset: 0x000589C4
		internal void BindParameters()
		{
			if (this._paramNames == null)
			{
				return;
			}
			int num = this._paramNames.Length;
			for (int i = 0; i < num; i++)
			{
				this.BindParameter(i + 1, this._paramValues[i]);
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0005A810 File Offset: 0x00058A10
		private static SQLiteConnection GetConnection(SQLiteStatement statement)
		{
			try
			{
				if (statement != null)
				{
					SQLiteCommand command = statement._command;
					if (command != null)
					{
						SQLiteConnection connection = command.Connection;
						if (connection != null)
						{
							return connection;
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
			return null;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0005A864 File Offset: 0x00058A64
		private void InvokeBindValueCallback(int index, SQLiteParameter parameter, out bool complete)
		{
			complete = false;
			SQLiteConnectionFlags flags = this._flags;
			this._flags &= ~SQLiteConnectionFlags.UseConnectionBindValueCallbacks;
			try
			{
				if (parameter != null)
				{
					SQLiteConnection connection = SQLiteStatement.GetConnection(this);
					if (connection != null)
					{
						string text = parameter.TypeName;
						if (text == null && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseParameterNameForTypeName))
						{
							text = parameter.ParameterName;
						}
						if (text == null && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseParameterDbTypeForTypeName))
						{
							text = SQLiteConvert.DbTypeToTypeName(connection, parameter.DbType, this._flags);
						}
						if (text != null)
						{
							SQLiteTypeCallbacks sqliteTypeCallbacks;
							if (connection.TryGetTypeCallbacks(text, out sqliteTypeCallbacks) && sqliteTypeCallbacks != null)
							{
								SQLiteBindValueCallback bindValueCallback = sqliteTypeCallbacks.BindValueCallback;
								if (bindValueCallback != null)
								{
									object bindValueUserData = sqliteTypeCallbacks.BindValueUserData;
									bindValueCallback(this._sql, this._command, flags, parameter, text, index, bindValueUserData, out complete);
								}
							}
						}
					}
				}
			}
			finally
			{
				this._flags |= SQLiteConnectionFlags.UseConnectionBindValueCallbacks;
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0005A994 File Offset: 0x00058B94
		private void BindParameter(int index, SQLiteParameter param)
		{
			if (param == null)
			{
				throw new SQLiteException("Insufficient parameters supplied to the command");
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionBindValueCallbacks))
			{
				bool flag;
				this.InvokeBindValueCallback(index, param, out flag);
				if (flag)
				{
					return;
				}
			}
			object value = param.Value;
			DbType dbType = param.DbType;
			if (value != null && dbType == DbType.Object)
			{
				dbType = SQLiteConvert.TypeToDbType(value.GetType());
			}
			if (this._sql.ForceLogPrepare || HelperMethods.LogPreBind(this._flags))
			{
				IntPtr intPtr = this._sqlite_stmt;
				SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} with database type {2} and raw value {{{3}}}...", new object[] { intPtr, index, dbType, value }));
			}
			if (value == null || Convert.IsDBNull(value))
			{
				this._sql.Bind_Null(this, this._flags, index);
				return;
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			bool flag2 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindInvariantText);
			CultureInfo cultureInfo = CultureInfo.CurrentCulture;
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.ConvertInvariantText))
			{
				cultureInfo = invariantCulture;
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindAllAsText))
			{
				if (value is DateTime)
				{
					this._sql.Bind_DateTime(this, this._flags, index, (DateTime)value);
					return;
				}
				this._sql.Bind_Text(this, this._flags, index, flag2 ? SQLiteConvert.ToStringWithProvider(value, invariantCulture) : SQLiteConvert.ToStringWithProvider(value, cultureInfo));
				return;
			}
			else
			{
				bool flag3 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindInvariantDecimal);
				if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindDecimalAsText) && value is decimal)
				{
					this._sql.Bind_Text(this, this._flags, index, (flag2 || flag3) ? SQLiteConvert.ToStringWithProvider(value, invariantCulture) : SQLiteConvert.ToStringWithProvider(value, cultureInfo));
					return;
				}
				switch (dbType)
				{
				case DbType.Binary:
					this._sql.Bind_Blob(this, this._flags, index, (byte[])value);
					return;
				case DbType.Byte:
					this._sql.Bind_UInt32(this, this._flags, index, (uint)Convert.ToByte(value, cultureInfo));
					return;
				case DbType.Boolean:
					this._sql.Bind_Boolean(this, this._flags, index, SQLiteConvert.ToBoolean(value, cultureInfo, true));
					return;
				case DbType.Currency:
				case DbType.Double:
				case DbType.Single:
					this._sql.Bind_Double(this, this._flags, index, Convert.ToDouble(value, cultureInfo));
					return;
				case DbType.Date:
				case DbType.DateTime:
				case DbType.Time:
					this._sql.Bind_DateTime(this, this._flags, index, (value is string) ? this._sql.ToDateTime((string)value) : Convert.ToDateTime(value, cultureInfo));
					return;
				case DbType.Decimal:
					this._sql.Bind_Text(this, this._flags, index, (flag2 || flag3) ? SQLiteConvert.ToStringWithProvider(Convert.ToDecimal(value, cultureInfo), invariantCulture) : SQLiteConvert.ToStringWithProvider(Convert.ToDecimal(value, cultureInfo), cultureInfo));
					return;
				case DbType.Guid:
					if (this._command.Connection._binaryGuid)
					{
						this._sql.Bind_Blob(this, this._flags, index, ((Guid)value).ToByteArray());
						return;
					}
					this._sql.Bind_Text(this, this._flags, index, flag2 ? SQLiteConvert.ToStringWithProvider(value, invariantCulture) : SQLiteConvert.ToStringWithProvider(value, cultureInfo));
					return;
				case DbType.Int16:
					this._sql.Bind_Int32(this, this._flags, index, (int)Convert.ToInt16(value, cultureInfo));
					return;
				case DbType.Int32:
					this._sql.Bind_Int32(this, this._flags, index, Convert.ToInt32(value, cultureInfo));
					return;
				case DbType.Int64:
					this._sql.Bind_Int64(this, this._flags, index, Convert.ToInt64(value, cultureInfo));
					return;
				case DbType.SByte:
					this._sql.Bind_Int32(this, this._flags, index, (int)Convert.ToSByte(value, cultureInfo));
					return;
				case DbType.UInt16:
					this._sql.Bind_UInt32(this, this._flags, index, (uint)Convert.ToUInt16(value, cultureInfo));
					return;
				case DbType.UInt32:
					this._sql.Bind_UInt32(this, this._flags, index, Convert.ToUInt32(value, cultureInfo));
					return;
				case DbType.UInt64:
					this._sql.Bind_UInt64(this, this._flags, index, Convert.ToUInt64(value, cultureInfo));
					return;
				}
				this._sql.Bind_Text(this, this._flags, index, flag2 ? SQLiteConvert.ToStringWithProvider(value, invariantCulture) : SQLiteConvert.ToStringWithProvider(value, cultureInfo));
				return;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x0005AE68 File Offset: 0x00059068
		internal string[] TypeDefinitions
		{
			get
			{
				return this._types;
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0005AE70 File Offset: 0x00059070
		internal void SetTypes(string typedefs)
		{
			int num = typedefs.IndexOf("TYPES", 0, StringComparison.OrdinalIgnoreCase);
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException();
			}
			string[] array = typedefs.Substring(num + 6).Replace(" ", string.Empty).Replace(";", string.Empty)
				.Replace("\"", string.Empty)
				.Replace("[", string.Empty)
				.Replace("]", string.Empty)
				.Replace("`", string.Empty)
				.Split(new char[] { ',', '\r', '\n', '\t' });
			for (int i = 0; i < array.Length; i++)
			{
				if (string.IsNullOrEmpty(array[i]))
				{
					array[i] = null;
				}
			}
			this._types = array;
		}

		// Token: 0x0400080A RID: 2058
		internal SQLiteBase _sql;

		// Token: 0x0400080B RID: 2059
		internal string _sqlStatement;

		// Token: 0x0400080C RID: 2060
		internal SQLiteStatementHandle _sqlite_stmt;

		// Token: 0x0400080D RID: 2061
		internal int _unnamedParameters;

		// Token: 0x0400080E RID: 2062
		internal string[] _paramNames;

		// Token: 0x0400080F RID: 2063
		internal SQLiteParameter[] _paramValues;

		// Token: 0x04000810 RID: 2064
		internal SQLiteCommand _command;

		// Token: 0x04000811 RID: 2065
		private SQLiteConnectionFlags _flags;

		// Token: 0x04000812 RID: 2066
		private string[] _types;

		// Token: 0x04000813 RID: 2067
		private bool disposed;
	}
}
