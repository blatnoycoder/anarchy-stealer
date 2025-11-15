using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001AA RID: 426
	internal sealed class SQLiteKeyReader : IDisposable
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x00057B60 File Offset: 0x00055D60
		internal SQLiteKeyReader(SQLiteConnection cnn, SQLiteDataReader reader, SQLiteStatement stmt)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
			List<SQLiteKeyReader.KeyInfo> list = new List<SQLiteKeyReader.KeyInfo>();
			List<SQLiteKeyReader.RowIdInfo> list2 = new List<SQLiteKeyReader.RowIdInfo>();
			this._stmt = stmt;
			using (DataTable schema = cnn.GetSchema("Catalogs"))
			{
				foreach (object obj in schema.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					dictionary.Add((string)dataRow["CATALOG_NAME"], Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture));
				}
			}
			using (DataTable schemaTable = reader.GetSchemaTable(false, false))
			{
				foreach (object obj2 in schemaTable.Rows)
				{
					DataRow dataRow2 = (DataRow)obj2;
					if (dataRow2[SchemaTableOptionalColumn.BaseCatalogName] != DBNull.Value)
					{
						string text = (string)dataRow2[SchemaTableOptionalColumn.BaseCatalogName];
						string text2 = (string)dataRow2[SchemaTableColumn.BaseTableName];
						List<string> list3;
						if (!dictionary2.ContainsKey(text))
						{
							list3 = new List<string>();
							dictionary2.Add(text, list3);
						}
						else
						{
							list3 = dictionary2[text];
						}
						if (!list3.Contains(text2))
						{
							list3.Add(text2);
						}
					}
				}
				foreach (KeyValuePair<string, List<string>> keyValuePair in dictionary2)
				{
					for (int i = 0; i < keyValuePair.Value.Count; i++)
					{
						string text3 = keyValuePair.Value[i];
						DataRow dataRow3 = null;
						using (DataTable schema2 = cnn.GetSchema("Indexes", new string[] { keyValuePair.Key, null, text3 }))
						{
							int num = 0;
							while (num < 2 && dataRow3 == null)
							{
								foreach (object obj3 in schema2.Rows)
								{
									DataRow dataRow4 = (DataRow)obj3;
									if (num == 0 && (bool)dataRow4["PRIMARY_KEY"])
									{
										dataRow3 = dataRow4;
										break;
									}
									if (num == 1 && (bool)dataRow4["UNIQUE"])
									{
										dataRow3 = dataRow4;
										break;
									}
								}
								num++;
							}
							if (dataRow3 == null)
							{
								keyValuePair.Value.RemoveAt(i);
								i--;
							}
							else
							{
								using (DataTable schema3 = cnn.GetSchema("Tables", new string[] { keyValuePair.Key, null, text3 }))
								{
									int num2 = dictionary[keyValuePair.Key];
									int num3 = Convert.ToInt32(schema3.Rows[0]["TABLE_ROOTPAGE"], CultureInfo.InvariantCulture);
									int cursorForTable = stmt._sql.GetCursorForTable(stmt, num2, num3);
									using (DataTable schema4 = cnn.GetSchema("IndexColumns", new string[]
									{
										keyValuePair.Key,
										null,
										text3,
										(string)dataRow3["INDEX_NAME"]
									}))
									{
										bool flag = (string)dataRow3["INDEX_NAME"] == "sqlite_master_PK_" + text3;
										SQLiteKeyReader.KeyQuery keyQuery = null;
										List<string> list4 = new List<string>();
										for (int j = 0; j < schema4.Rows.Count; j++)
										{
											string stringOrNull = SQLiteConvert.GetStringOrNull(schema4.Rows[j]["COLUMN_NAME"]);
											bool flag2 = true;
											foreach (object obj4 in schemaTable.Rows)
											{
												DataRow dataRow5 = (DataRow)obj4;
												if (!dataRow5.IsNull(SchemaTableColumn.BaseColumnName) && (string)dataRow5[SchemaTableColumn.BaseColumnName] == stringOrNull && (string)dataRow5[SchemaTableColumn.BaseTableName] == text3 && (string)dataRow5[SchemaTableOptionalColumn.BaseCatalogName] == keyValuePair.Key)
												{
													if (flag)
													{
														list2.Add(new SQLiteKeyReader.RowIdInfo
														{
															databaseName = keyValuePair.Key,
															tableName = text3,
															column = (int)dataRow5[SchemaTableColumn.ColumnOrdinal]
														});
													}
													schema4.Rows.RemoveAt(j);
													j--;
													flag2 = false;
													break;
												}
											}
											if (flag2)
											{
												list4.Add(stringOrNull);
											}
										}
										if (!flag && list4.Count > 0)
										{
											string[] array = new string[list4.Count];
											list4.CopyTo(array);
											keyQuery = new SQLiteKeyReader.KeyQuery(cnn, keyValuePair.Key, text3, array);
										}
										for (int k = 0; k < schema4.Rows.Count; k++)
										{
											string stringOrNull2 = SQLiteConvert.GetStringOrNull(schema4.Rows[k]["COLUMN_NAME"]);
											list.Add(new SQLiteKeyReader.KeyInfo
											{
												rootPage = num3,
												cursor = cursorForTable,
												database = num2,
												databaseName = keyValuePair.Key,
												tableName = text3,
												columnName = stringOrNull2,
												query = keyQuery,
												column = k
											});
										}
									}
								}
							}
						}
					}
				}
			}
			this._keyInfo = new SQLiteKeyReader.KeyInfo[list.Count];
			list.CopyTo(this._keyInfo);
			this._rowIdInfo = new SQLiteKeyReader.RowIdInfo[list2.Count];
			list2.CopyTo(this._rowIdInfo);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00058318 File Offset: 0x00056518
		internal int GetRowIdIndex(string databaseName, string tableName)
		{
			if (this._rowIdInfo != null && databaseName != null && tableName != null)
			{
				for (int i = 0; i < this._rowIdInfo.Length; i++)
				{
					if (this._rowIdInfo[i].databaseName == databaseName && this._rowIdInfo[i].tableName == tableName)
					{
						return this._rowIdInfo[i].column;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x000583A4 File Offset: 0x000565A4
		internal long? GetRowId(string databaseName, string tableName)
		{
			if (this._keyInfo != null && databaseName != null && tableName != null)
			{
				for (int i = 0; i < this._keyInfo.Length; i++)
				{
					if (this._keyInfo[i].databaseName == databaseName && this._keyInfo[i].tableName == tableName)
					{
						long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
						if (rowIdForCursor != 0L)
						{
							return new long?(rowIdForCursor);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0005845C File Offset: 0x0005665C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0005846C File Offset: 0x0005666C
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteKeyReader).Name);
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00058490 File Offset: 0x00056690
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this._stmt = null;
					if (this._keyInfo != null)
					{
						for (int i = 0; i < this._keyInfo.Length; i++)
						{
							if (this._keyInfo[i].query != null)
							{
								this._keyInfo[i].query.Dispose();
							}
						}
						this._keyInfo = null;
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00058514 File Offset: 0x00056714
		~SQLiteKeyReader()
		{
			this.Dispose(false);
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00058544 File Offset: 0x00056744
		internal int Count
		{
			get
			{
				if (this._keyInfo != null)
				{
					return this._keyInfo.Length;
				}
				return 0;
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0005855C File Offset: 0x0005675C
		private void Sync(int i)
		{
			this.Sync();
			if (this._keyInfo[i].cursor == -1)
			{
				throw new InvalidCastException();
			}
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00058584 File Offset: 0x00056784
		private void Sync()
		{
			if (this._isValid)
			{
				return;
			}
			SQLiteKeyReader.KeyQuery keyQuery = null;
			for (int i = 0; i < this._keyInfo.Length; i++)
			{
				if (this._keyInfo[i].query == null || this._keyInfo[i].query != keyQuery)
				{
					keyQuery = this._keyInfo[i].query;
					if (keyQuery != null)
					{
						keyQuery.Sync(this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor));
					}
				}
			}
			this._isValid = true;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00058638 File Offset: 0x00056838
		internal void Reset()
		{
			this._isValid = false;
			if (this._keyInfo == null)
			{
				return;
			}
			for (int i = 0; i < this._keyInfo.Length; i++)
			{
				if (this._keyInfo[i].query != null)
				{
					this._keyInfo[i].query.IsValid = false;
				}
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000586A0 File Offset: 0x000568A0
		internal string GetDataTypeName(int i)
		{
			this.Sync();
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetDataTypeName(this._keyInfo[i].column);
			}
			return "integer";
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00058700 File Offset: 0x00056900
		internal TypeAffinity GetFieldAffinity(int i)
		{
			this.Sync();
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetFieldAffinity(this._keyInfo[i].column);
			}
			return TypeAffinity.Uninitialized;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0005875C File Offset: 0x0005695C
		internal Type GetFieldType(int i)
		{
			this.Sync();
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetFieldType(this._keyInfo[i].column);
			}
			return typeof(long);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000587C0 File Offset: 0x000569C0
		internal string GetDatabaseName(int i)
		{
			return this._keyInfo[i].databaseName;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000587D4 File Offset: 0x000569D4
		internal string GetTableName(int i)
		{
			return this._keyInfo[i].tableName;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000587E8 File Offset: 0x000569E8
		internal string GetName(int i)
		{
			return this._keyInfo[i].columnName;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000587FC File Offset: 0x000569FC
		internal int GetOrdinal(string name)
		{
			for (int i = 0; i < this._keyInfo.Length; i++)
			{
				if (string.Compare(name, this._keyInfo[i].columnName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00058844 File Offset: 0x00056A44
		internal SQLiteBlob GetBlob(int i, bool readOnly)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetBlob(this._keyInfo[i].column, readOnly);
			}
			throw new InvalidCastException();
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000588A8 File Offset: 0x00056AA8
		internal bool GetBoolean(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetBoolean(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00058908 File Offset: 0x00056B08
		internal byte GetByte(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetByte(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00058968 File Offset: 0x00056B68
		internal long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetBytes(this._keyInfo[i].column, fieldOffset, buffer, bufferoffset, length);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x000589D0 File Offset: 0x00056BD0
		internal char GetChar(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetChar(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00058A30 File Offset: 0x00056C30
		internal long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetChars(this._keyInfo[i].column, fieldOffset, buffer, bufferoffset, length);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00058A98 File Offset: 0x00056C98
		internal DateTime GetDateTime(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetDateTime(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00058AF8 File Offset: 0x00056CF8
		internal decimal GetDecimal(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetDecimal(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00058B58 File Offset: 0x00056D58
		internal double GetDouble(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetDouble(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00058BB8 File Offset: 0x00056DB8
		internal float GetFloat(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetFloat(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00058C18 File Offset: 0x00056E18
		internal Guid GetGuid(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetGuid(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00058C78 File Offset: 0x00056E78
		internal short GetInt16(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetInt16(this._keyInfo[i].column);
			}
			long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
			if (rowIdForCursor == 0L)
			{
				throw new InvalidCastException();
			}
			return Convert.ToInt16(rowIdForCursor);
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00058D10 File Offset: 0x00056F10
		internal int GetInt32(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetInt32(this._keyInfo[i].column);
			}
			long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
			if (rowIdForCursor == 0L)
			{
				throw new InvalidCastException();
			}
			return Convert.ToInt32(rowIdForCursor);
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00058DA8 File Offset: 0x00056FA8
		internal long GetInt64(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetInt64(this._keyInfo[i].column);
			}
			long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
			if (rowIdForCursor == 0L)
			{
				throw new InvalidCastException();
			}
			return rowIdForCursor;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00058E3C File Offset: 0x0005703C
		internal string GetString(int i)
		{
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetString(this._keyInfo[i].column);
			}
			throw new InvalidCastException();
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00058E9C File Offset: 0x0005709C
		internal object GetValue(int i)
		{
			if (this._keyInfo[i].cursor == -1)
			{
				return DBNull.Value;
			}
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.GetValue(this._keyInfo[i].column);
			}
			if (this.IsDBNull(i))
			{
				return DBNull.Value;
			}
			return this.GetInt64(i);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00058F34 File Offset: 0x00057134
		internal bool IsDBNull(int i)
		{
			if (this._keyInfo[i].cursor == -1)
			{
				return true;
			}
			this.Sync(i);
			if (this._keyInfo[i].query != null)
			{
				return this._keyInfo[i].query._reader.IsDBNull(this._keyInfo[i].column);
			}
			return this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor) == 0L;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00058FD4 File Offset: 0x000571D4
		internal void AppendSchemaTable(DataTable tbl)
		{
			SQLiteKeyReader.KeyQuery keyQuery = null;
			for (int i = 0; i < this._keyInfo.Length; i++)
			{
				if (this._keyInfo[i].query == null || this._keyInfo[i].query != keyQuery)
				{
					keyQuery = this._keyInfo[i].query;
					if (keyQuery == null)
					{
						DataRow dataRow = tbl.NewRow();
						dataRow[SchemaTableColumn.ColumnName] = this._keyInfo[i].columnName;
						dataRow[SchemaTableColumn.ColumnOrdinal] = tbl.Rows.Count;
						dataRow[SchemaTableColumn.ColumnSize] = 8;
						dataRow[SchemaTableColumn.NumericPrecision] = 255;
						dataRow[SchemaTableColumn.NumericScale] = 255;
						dataRow[SchemaTableColumn.ProviderType] = DbType.Int64;
						dataRow[SchemaTableColumn.IsLong] = false;
						dataRow[SchemaTableColumn.AllowDBNull] = false;
						dataRow[SchemaTableOptionalColumn.IsReadOnly] = false;
						dataRow[SchemaTableOptionalColumn.IsRowVersion] = false;
						dataRow[SchemaTableColumn.IsUnique] = false;
						dataRow[SchemaTableColumn.IsKey] = true;
						dataRow[SchemaTableColumn.DataType] = typeof(long);
						dataRow[SchemaTableOptionalColumn.IsHidden] = true;
						dataRow[SchemaTableColumn.BaseColumnName] = this._keyInfo[i].columnName;
						dataRow[SchemaTableColumn.IsExpression] = false;
						dataRow[SchemaTableColumn.IsAliased] = false;
						dataRow[SchemaTableColumn.BaseTableName] = this._keyInfo[i].tableName;
						dataRow[SchemaTableOptionalColumn.BaseCatalogName] = this._keyInfo[i].databaseName;
						dataRow[SchemaTableOptionalColumn.IsAutoIncrement] = true;
						dataRow["DataTypeName"] = "integer";
						tbl.Rows.Add(dataRow);
					}
					else
					{
						keyQuery.Sync(0L);
						using (DataTable schemaTable = keyQuery._reader.GetSchemaTable())
						{
							foreach (object obj in schemaTable.Rows)
							{
								DataRow dataRow2 = (DataRow)obj;
								object[] itemArray = dataRow2.ItemArray;
								DataRow dataRow3 = tbl.Rows.Add(itemArray);
								dataRow3[SchemaTableOptionalColumn.IsHidden] = true;
								dataRow3[SchemaTableColumn.ColumnOrdinal] = tbl.Rows.Count - 1;
							}
						}
					}
				}
			}
		}

		// Token: 0x040007DE RID: 2014
		private SQLiteKeyReader.KeyInfo[] _keyInfo;

		// Token: 0x040007DF RID: 2015
		private SQLiteStatement _stmt;

		// Token: 0x040007E0 RID: 2016
		private bool _isValid;

		// Token: 0x040007E1 RID: 2017
		private SQLiteKeyReader.RowIdInfo[] _rowIdInfo;

		// Token: 0x040007E2 RID: 2018
		private bool disposed;

		// Token: 0x0200029A RID: 666
		private struct KeyInfo
		{
			// Token: 0x04000B40 RID: 2880
			internal string databaseName;

			// Token: 0x04000B41 RID: 2881
			internal string tableName;

			// Token: 0x04000B42 RID: 2882
			internal string columnName;

			// Token: 0x04000B43 RID: 2883
			internal int database;

			// Token: 0x04000B44 RID: 2884
			internal int rootPage;

			// Token: 0x04000B45 RID: 2885
			internal int cursor;

			// Token: 0x04000B46 RID: 2886
			internal SQLiteKeyReader.KeyQuery query;

			// Token: 0x04000B47 RID: 2887
			internal int column;
		}

		// Token: 0x0200029B RID: 667
		private struct RowIdInfo
		{
			// Token: 0x04000B48 RID: 2888
			internal string databaseName;

			// Token: 0x04000B49 RID: 2889
			internal string tableName;

			// Token: 0x04000B4A RID: 2890
			internal int column;
		}

		// Token: 0x0200029C RID: 668
		private sealed class KeyQuery : IDisposable
		{
			// Token: 0x06001880 RID: 6272 RVA: 0x00069E58 File Offset: 0x00068058
			internal KeyQuery(SQLiteConnection cnn, string database, string table, params string[] columns)
			{
				using (SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder())
				{
					this._command = cnn.CreateCommand();
					for (int i = 0; i < columns.Length; i++)
					{
						columns[i] = sqliteCommandBuilder.QuoteIdentifier(columns[i]);
					}
				}
				this._command.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT {0} FROM [{1}].[{2}] WHERE ROWID = ?", new object[]
				{
					string.Join(",", columns),
					database,
					table
				});
				this._command.Parameters.AddWithValue(null, 0L);
			}

			// Token: 0x170003F8 RID: 1016
			// (set) Token: 0x06001881 RID: 6273 RVA: 0x00069F1C File Offset: 0x0006811C
			internal bool IsValid
			{
				set
				{
					if (value)
					{
						throw new ArgumentException();
					}
					if (this._reader != null)
					{
						this._reader.Dispose();
						this._reader = null;
					}
				}
			}

			// Token: 0x06001882 RID: 6274 RVA: 0x00069F48 File Offset: 0x00068148
			internal void Sync(long rowid)
			{
				this.IsValid = false;
				this._command.Parameters[0].Value = rowid;
				this._reader = this._command.ExecuteReader();
				this._reader.Read();
			}

			// Token: 0x06001883 RID: 6275 RVA: 0x00069F9C File Offset: 0x0006819C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06001884 RID: 6276 RVA: 0x00069FAC File Offset: 0x000681AC
			private void CheckDisposed()
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(typeof(SQLiteKeyReader.KeyQuery).Name);
				}
			}

			// Token: 0x06001885 RID: 6277 RVA: 0x00069FD0 File Offset: 0x000681D0
			private void Dispose(bool disposing)
			{
				if (!this.disposed)
				{
					if (disposing)
					{
						this.IsValid = false;
						if (this._command != null)
						{
							this._command.Dispose();
						}
						this._command = null;
					}
					this.disposed = true;
				}
			}

			// Token: 0x06001886 RID: 6278 RVA: 0x0006A010 File Offset: 0x00068210
			~KeyQuery()
			{
				this.Dispose(false);
			}

			// Token: 0x04000B4B RID: 2891
			private SQLiteCommand _command;

			// Token: 0x04000B4C RID: 2892
			internal SQLiteDataReader _reader;

			// Token: 0x04000B4D RID: 2893
			private bool disposed;
		}
	}
}
