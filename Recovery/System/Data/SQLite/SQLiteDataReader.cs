using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x02000196 RID: 406
	public sealed class SQLiteDataReader : DbDataReader
	{
		// Token: 0x060011B4 RID: 4532 RVA: 0x000532AC File Offset: 0x000514AC
		internal SQLiteDataReader(SQLiteCommand cmd, CommandBehavior behave)
		{
			this._throwOnDisposed = true;
			this._command = cmd;
			this._version = this._command.Connection._version;
			this._baseSchemaName = this._command.Connection._baseSchemaName;
			this._commandBehavior = behave;
			this._activeStatementIndex = -1;
			this._rowsAffected = -1;
			this.RefreshFlags();
			SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.NewDataReader, null, null, this._command, this, null, null, new object[] { behave }));
			if (this._command != null)
			{
				this.NextResult();
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00053358 File Offset: 0x00051558
		private void CheckDisposed()
		{
			if (this.disposed && this._throwOnDisposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteDataReader).Name);
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00053388 File Offset: 0x00051588
		protected override void Dispose(bool disposing)
		{
			SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.DisposingDataReader, null, null, this._command, this, null, null, new object[] { disposing, this.disposed, this._commandBehavior, this._readingState, this._rowsAffected, this._stepCount, this._fieldCount, this._disposeCommand, this._throwOnDisposed }));
			try
			{
				if (!this.disposed)
				{
					this._throwOnDisposed = false;
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0005346C File Offset: 0x0005166C
		internal void Cancel()
		{
			this._version = 0;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00053478 File Offset: 0x00051678
		public override void Close()
		{
			this.CheckDisposed();
			SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.ClosingDataReader, null, null, this._command, this, null, null, new object[] { this._commandBehavior, this._readingState, this._rowsAffected, this._stepCount, this._fieldCount, this._disposeCommand, this._throwOnDisposed }));
			try
			{
				if (this._command != null)
				{
					try
					{
						try
						{
							if (this._version != 0)
							{
								try
								{
									while (this.NextResult())
									{
									}
								}
								catch (SQLiteException)
								{
								}
							}
							this._command.ResetDataReader();
						}
						finally
						{
							if ((this._commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.Default && this._command.Connection != null)
							{
								this._command.Connection.Close();
							}
						}
					}
					finally
					{
						if (this._disposeCommand)
						{
							this._command.Dispose();
						}
					}
				}
				this._command = null;
				this._activeStatement = null;
				this._fieldIndexes = null;
				this._fieldTypeArray = null;
			}
			finally
			{
				if (this._keyInfo != null)
				{
					this._keyInfo.Dispose();
					this._keyInfo = null;
				}
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0005360C File Offset: 0x0005180C
		private void CheckClosed()
		{
			if (!this._throwOnDisposed)
			{
				return;
			}
			if (this._command == null)
			{
				throw new InvalidOperationException("DataReader has been closed");
			}
			if (this._version == 0)
			{
				throw new SQLiteException("Execution was aborted by the user");
			}
			SQLiteConnection connection = this._command.Connection;
			if (connection._version != this._version || connection.State != ConnectionState.Open)
			{
				throw new InvalidOperationException("Connection was closed, statement was terminated");
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0005368C File Offset: 0x0005188C
		private void CheckValidRow()
		{
			if (this._readingState != 0)
			{
				throw new InvalidOperationException("No current row");
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x000536A4 File Offset: 0x000518A4
		public override IEnumerator GetEnumerator()
		{
			this.CheckDisposed();
			return new DbEnumerator(this, (this._commandBehavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x000536C0 File Offset: 0x000518C0
		public override int Depth
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				return 0;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x000536D0 File Offset: 0x000518D0
		public override int FieldCount
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				if (this._keyInfo == null)
				{
					return this._fieldCount;
				}
				return this._fieldCount + this._keyInfo.Count;
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00053704 File Offset: 0x00051904
		public void RefreshFlags()
		{
			this.CheckDisposed();
			this._flags = SQLiteCommand.GetFlags(this._command);
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x00053720 File Offset: 0x00051920
		public int StepCount
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				return this._stepCount;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00053734 File Offset: 0x00051934
		private int PrivateVisibleFieldCount
		{
			get
			{
				return this._fieldCount;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x0005373C File Offset: 0x0005193C
		public override int VisibleFieldCount
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				return this.PrivateVisibleFieldCount;
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00053750 File Offset: 0x00051950
		private void VerifyForGet()
		{
			this.CheckClosed();
			this.CheckValidRow();
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00053760 File Offset: 0x00051960
		private TypeAffinity VerifyType(int i, DbType typ)
		{
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoVerifyTypeAffinity))
			{
				return TypeAffinity.None;
			}
			TypeAffinity affinity = this.GetSQLiteType(this._flags, i).Affinity;
			switch (affinity)
			{
			case TypeAffinity.Int64:
				if (typ == DbType.Int64)
				{
					return affinity;
				}
				if (typ == DbType.Int32)
				{
					return affinity;
				}
				if (typ == DbType.Int16)
				{
					return affinity;
				}
				if (typ == DbType.Byte)
				{
					return affinity;
				}
				if (typ == DbType.SByte)
				{
					return affinity;
				}
				if (typ == DbType.Boolean)
				{
					return affinity;
				}
				if (typ == DbType.DateTime)
				{
					return affinity;
				}
				if (typ == DbType.Double)
				{
					return affinity;
				}
				if (typ == DbType.Single)
				{
					return affinity;
				}
				if (typ == DbType.Decimal)
				{
					return affinity;
				}
				break;
			case TypeAffinity.Double:
				if (typ == DbType.Double)
				{
					return affinity;
				}
				if (typ == DbType.Single)
				{
					return affinity;
				}
				if (typ == DbType.Decimal)
				{
					return affinity;
				}
				if (typ == DbType.DateTime)
				{
					return affinity;
				}
				break;
			case TypeAffinity.Text:
				if (typ == DbType.String)
				{
					return affinity;
				}
				if (typ == DbType.Guid)
				{
					return affinity;
				}
				if (typ == DbType.DateTime)
				{
					return affinity;
				}
				if (typ == DbType.Decimal)
				{
					return affinity;
				}
				break;
			case TypeAffinity.Blob:
				if (typ == DbType.Guid)
				{
					return affinity;
				}
				if (typ == DbType.Binary)
				{
					return affinity;
				}
				if (typ == DbType.String)
				{
					return affinity;
				}
				break;
			}
			throw new InvalidCastException();
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0005388C File Offset: 0x00051A8C
		private void InvokeReadValueCallback(int index, SQLiteReadEventArgs eventArgs, out bool complete)
		{
			complete = false;
			SQLiteConnectionFlags flags = this._flags;
			this._flags &= ~SQLiteConnectionFlags.UseConnectionReadValueCallbacks;
			try
			{
				string dataTypeName = this.GetDataTypeName(index);
				if (dataTypeName != null)
				{
					SQLiteConnection connection = SQLiteDataReader.GetConnection(this);
					if (connection != null)
					{
						SQLiteTypeCallbacks sqliteTypeCallbacks;
						if (connection.TryGetTypeCallbacks(dataTypeName, out sqliteTypeCallbacks) && sqliteTypeCallbacks != null)
						{
							SQLiteReadValueCallback readValueCallback = sqliteTypeCallbacks.ReadValueCallback;
							if (readValueCallback != null)
							{
								object readValueUserData = sqliteTypeCallbacks.ReadValueUserData;
								readValueCallback(this._activeStatement._sql, this, flags, eventArgs, dataTypeName, index, readValueUserData, out complete);
							}
						}
					}
				}
			}
			finally
			{
				this._flags |= SQLiteConnectionFlags.UseConnectionReadValueCallbacks;
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0005395C File Offset: 0x00051B5C
		internal long? GetRowId(int i)
		{
			this.VerifyForGet();
			if (this._keyInfo == null)
			{
				return null;
			}
			string databaseName = this.GetDatabaseName(i);
			string tableName = this.GetTableName(i);
			int rowIdIndex = this._keyInfo.GetRowIdIndex(databaseName, tableName);
			if (rowIdIndex != -1)
			{
				return new long?(this.GetInt64(rowIdIndex));
			}
			return this._keyInfo.GetRowId(databaseName, tableName);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000539C8 File Offset: 0x00051BC8
		public SQLiteBlob GetBlob(int i, bool readOnly)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetBlob", new SQLiteReadBlobEventArgs(readOnly), sqliteDataReaderValue), out flag);
				if (flag)
				{
					return sqliteDataReaderValue.BlobValue;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetBlob(i - this.PrivateVisibleFieldCount, readOnly);
			}
			return SQLiteBlob.Create(this, i, readOnly);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00053A60 File Offset: 0x00051C60
		public override bool GetBoolean(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetBoolean", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.BooleanValue == null)
					{
						throw new SQLiteException("missing boolean return value");
					}
					return sqliteDataReaderValue.BooleanValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetBoolean(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Boolean);
			return Convert.ToBoolean(this.GetValue(i), CultureInfo.CurrentCulture);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00053B24 File Offset: 0x00051D24
		public override byte GetByte(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetByte", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					byte? byteValue = sqliteDataReaderValue.ByteValue;
					int? num = ((byteValue != null) ? new int?((int)byteValue.GetValueOrDefault()) : null);
					if (num == null)
					{
						throw new SQLiteException("missing byte return value");
					}
					return sqliteDataReaderValue.ByteValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetByte(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Byte);
			return this._activeStatement._sql.GetByte(this._activeStatement, i);
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00053C18 File Offset: 0x00051E18
		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteReadArrayEventArgs sqliteReadArrayEventArgs = new SQLiteReadArrayEventArgs(fieldOffset, buffer, bufferoffset, length);
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetBytes", sqliteReadArrayEventArgs, sqliteDataReaderValue), out flag);
				if (flag)
				{
					byte[] bytesValue = sqliteDataReaderValue.BytesValue;
					if (bytesValue != null)
					{
						Array.Copy(bytesValue, sqliteReadArrayEventArgs.DataOffset, sqliteReadArrayEventArgs.ByteBuffer, (long)sqliteReadArrayEventArgs.BufferOffset, (long)sqliteReadArrayEventArgs.Length);
						return (long)sqliteReadArrayEventArgs.Length;
					}
					return -1L;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetBytes(i - this.PrivateVisibleFieldCount, fieldOffset, buffer, bufferoffset, length);
			}
			this.VerifyType(i, DbType.Binary);
			return this._activeStatement._sql.GetBytes(this._activeStatement, i, (int)fieldOffset, buffer, bufferoffset, length);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00053D0C File Offset: 0x00051F0C
		public override char GetChar(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetChar", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					char? charValue = sqliteDataReaderValue.CharValue;
					int? num = ((charValue != null) ? new int?((int)charValue.GetValueOrDefault()) : null);
					if (num == null)
					{
						throw new SQLiteException("missing character return value");
					}
					return sqliteDataReaderValue.CharValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetChar(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.SByte);
			return this._activeStatement._sql.GetChar(this._activeStatement, i);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00053E04 File Offset: 0x00052004
		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteReadArrayEventArgs sqliteReadArrayEventArgs = new SQLiteReadArrayEventArgs(fieldoffset, buffer, bufferoffset, length);
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetChars", sqliteReadArrayEventArgs, sqliteDataReaderValue), out flag);
				if (flag)
				{
					char[] charsValue = sqliteDataReaderValue.CharsValue;
					if (charsValue != null)
					{
						Array.Copy(charsValue, sqliteReadArrayEventArgs.DataOffset, sqliteReadArrayEventArgs.CharBuffer, (long)sqliteReadArrayEventArgs.BufferOffset, (long)sqliteReadArrayEventArgs.Length);
						return (long)sqliteReadArrayEventArgs.Length;
					}
					return -1L;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetChars(i - this.PrivateVisibleFieldCount, fieldoffset, buffer, bufferoffset, length);
			}
			if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoVerifyTextAffinity))
			{
				this.VerifyType(i, DbType.String);
			}
			return this._activeStatement._sql.GetChars(this._activeStatement, i, (int)fieldoffset, buffer, bufferoffset, length);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00053F14 File Offset: 0x00052114
		public override string GetDataTypeName(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetDataTypeName(i - this.PrivateVisibleFieldCount);
			}
			TypeAffinity typeAffinity = TypeAffinity.Uninitialized;
			return this._activeStatement._sql.ColumnType(this._activeStatement, i, ref typeAffinity);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00053F74 File Offset: 0x00052174
		public override DateTime GetDateTime(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetDateTime", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.DateTimeValue == null)
					{
						throw new SQLiteException("missing date/time return value");
					}
					return sqliteDataReaderValue.DateTimeValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetDateTime(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.DateTime);
			return this._activeStatement._sql.GetDateTime(this._activeStatement, i);
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00054040 File Offset: 0x00052240
		public override decimal GetDecimal(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetDecimal", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.DecimalValue == null)
					{
						throw new SQLiteException("missing decimal return value");
					}
					return sqliteDataReaderValue.DecimalValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetDecimal(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Decimal);
			CultureInfo cultureInfo = CultureInfo.CurrentCulture;
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.GetInvariantDecimal))
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}
			return decimal.Parse(this._activeStatement._sql.GetText(this._activeStatement, i), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, cultureInfo);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0005413C File Offset: 0x0005233C
		public override double GetDouble(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetDouble", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.DoubleValue == null)
					{
						throw new SQLiteException("missing double return value");
					}
					return sqliteDataReaderValue.DoubleValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetDouble(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Double);
			return this._activeStatement._sql.GetDouble(this._activeStatement, i);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00054208 File Offset: 0x00052408
		public TypeAffinity GetFieldAffinity(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetFieldAffinity(i - this.PrivateVisibleFieldCount);
			}
			return this.GetSQLiteType(this._flags, i).Affinity;
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0005425C File Offset: 0x0005245C
		public override Type GetFieldType(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetFieldType(i - this.PrivateVisibleFieldCount);
			}
			return SQLiteConvert.SQLiteTypeToType(this.GetSQLiteType(this._flags, i));
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000542B0 File Offset: 0x000524B0
		public override float GetFloat(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetFloat", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.FloatValue == null)
					{
						throw new SQLiteException("missing float return value");
					}
					return sqliteDataReaderValue.FloatValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetFloat(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Single);
			return Convert.ToSingle(this._activeStatement._sql.GetDouble(this._activeStatement, i));
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00054380 File Offset: 0x00052580
		public override Guid GetGuid(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetGuid", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.GuidValue == null)
					{
						throw new SQLiteException("missing guid return value");
					}
					return sqliteDataReaderValue.GuidValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetGuid(i - this.PrivateVisibleFieldCount);
			}
			TypeAffinity typeAffinity = this.VerifyType(i, DbType.Guid);
			if (typeAffinity == TypeAffinity.Blob)
			{
				byte[] array = new byte[16];
				this._activeStatement._sql.GetBytes(this._activeStatement, i, 0, array, 0, 16);
				return new Guid(array);
			}
			return new Guid(this._activeStatement._sql.GetText(this._activeStatement, i));
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00054484 File Offset: 0x00052684
		public override short GetInt16(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetInt16", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					short? int16Value = sqliteDataReaderValue.Int16Value;
					int? num = ((int16Value != null) ? new int?((int)int16Value.GetValueOrDefault()) : null);
					if (num == null)
					{
						throw new SQLiteException("missing int16 return value");
					}
					return sqliteDataReaderValue.Int16Value.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetInt16(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Int16);
			return this._activeStatement._sql.GetInt16(this._activeStatement, i);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0005457C File Offset: 0x0005277C
		public override int GetInt32(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetInt32", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.Int32Value == null)
					{
						throw new SQLiteException("missing int32 return value");
					}
					return sqliteDataReaderValue.Int32Value.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetInt32(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Int32);
			return this._activeStatement._sql.GetInt32(this._activeStatement, i);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00054648 File Offset: 0x00052848
		public override long GetInt64(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetInt64", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					if (sqliteDataReaderValue.Int64Value == null)
					{
						throw new SQLiteException("missing int64 return value");
					}
					return sqliteDataReaderValue.Int64Value.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetInt64(i - this.PrivateVisibleFieldCount);
			}
			this.VerifyType(i, DbType.Int64);
			return this._activeStatement._sql.GetInt64(this._activeStatement, i);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00054714 File Offset: 0x00052914
		public override string GetName(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetName(i - this.PrivateVisibleFieldCount);
			}
			return this._activeStatement._sql.ColumnName(this._activeStatement, i);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00054770 File Offset: 0x00052970
		public string GetDatabaseName(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetDatabaseName(i - this.PrivateVisibleFieldCount);
			}
			return this._activeStatement._sql.ColumnDatabaseName(this._activeStatement, i);
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000547CC File Offset: 0x000529CC
		public string GetTableName(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetTableName(i - this.PrivateVisibleFieldCount);
			}
			return this._activeStatement._sql.ColumnTableName(this._activeStatement, i);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00054828 File Offset: 0x00052A28
		public string GetOriginalName(int i)
		{
			this.CheckDisposed();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetName(i - this.PrivateVisibleFieldCount);
			}
			return this._activeStatement._sql.ColumnOriginalName(this._activeStatement, i);
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00054884 File Offset: 0x00052A84
		public override int GetOrdinal(string name)
		{
			this.CheckDisposed();
			bool throwOnDisposed = this._throwOnDisposed;
			if (this._fieldIndexes == null)
			{
				this._fieldIndexes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			}
			int num;
			if (!this._fieldIndexes.TryGetValue(name, out num))
			{
				num = this._activeStatement._sql.ColumnIndex(this._activeStatement, name);
				if (num == -1 && this._keyInfo != null)
				{
					num = this._keyInfo.GetOrdinal(name);
					if (num > -1)
					{
						num += this.PrivateVisibleFieldCount;
					}
				}
				this._fieldIndexes.Add(name, num);
			}
			if (num == -1 && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StrictConformance))
			{
				throw new IndexOutOfRangeException();
			}
			return num;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0005494C File Offset: 0x00052B4C
		public override DataTable GetSchemaTable()
		{
			this.CheckDisposed();
			return this.GetSchemaTable(true, false);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0005495C File Offset: 0x00052B5C
		private static void GetStatementColumnParents(SQLiteBase sql, SQLiteStatement stmt, int fieldCount, ref Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns, ref Dictionary<int, SQLiteDataReader.ColumnParent> columnToParent)
		{
			if (parentToColumns == null)
			{
				parentToColumns = new Dictionary<SQLiteDataReader.ColumnParent, List<int>>(new SQLiteDataReader.ColumnParent());
			}
			if (columnToParent == null)
			{
				columnToParent = new Dictionary<int, SQLiteDataReader.ColumnParent>();
			}
			for (int i = 0; i < fieldCount; i++)
			{
				string text = sql.ColumnDatabaseName(stmt, i);
				string text2 = sql.ColumnTableName(stmt, i);
				string text3 = sql.ColumnOriginalName(stmt, i);
				SQLiteDataReader.ColumnParent columnParent = new SQLiteDataReader.ColumnParent(text, text2, null);
				SQLiteDataReader.ColumnParent columnParent2 = new SQLiteDataReader.ColumnParent(text, text2, text3);
				List<int> list;
				if (!parentToColumns.TryGetValue(columnParent, out list))
				{
					parentToColumns.Add(columnParent, new List<int>(new int[] { i }));
				}
				else if (list != null)
				{
					list.Add(i);
				}
				else
				{
					parentToColumns[columnParent] = new List<int>(new int[] { i });
				}
				columnToParent.Add(i, columnParent2);
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00054A40 File Offset: 0x00052C40
		private static int CountParents(Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns)
		{
			int num = 0;
			if (parentToColumns != null)
			{
				foreach (SQLiteDataReader.ColumnParent columnParent in parentToColumns.Keys)
				{
					if (columnParent != null)
					{
						string tableName = columnParent.TableName;
						if (!string.IsNullOrEmpty(tableName))
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00054AB8 File Offset: 0x00052CB8
		internal DataTable GetSchemaTable(bool wantUniqueInfo, bool wantDefaultValue)
		{
			this.CheckClosed();
			bool throwOnDisposed = this._throwOnDisposed;
			Dictionary<SQLiteDataReader.ColumnParent, List<int>> dictionary = null;
			Dictionary<int, SQLiteDataReader.ColumnParent> dictionary2 = null;
			SQLiteBase sql = this._command.Connection._sql;
			SQLiteDataReader.GetStatementColumnParents(sql, this._activeStatement, this._fieldCount, ref dictionary, ref dictionary2);
			DataTable dataTable = new DataTable("SchemaTable");
			DataTable dataTable2 = null;
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add(SchemaTableColumn.ColumnName, typeof(string));
			dataTable.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof(int));
			dataTable.Columns.Add(SchemaTableColumn.ColumnSize, typeof(int));
			dataTable.Columns.Add(SchemaTableColumn.NumericPrecision, typeof(int));
			dataTable.Columns.Add(SchemaTableColumn.NumericScale, typeof(int));
			dataTable.Columns.Add(SchemaTableColumn.IsUnique, typeof(bool));
			dataTable.Columns.Add(SchemaTableColumn.IsKey, typeof(bool));
			dataTable.Columns.Add(SchemaTableOptionalColumn.BaseServerName, typeof(string));
			dataTable.Columns.Add(SchemaTableOptionalColumn.BaseCatalogName, typeof(string));
			dataTable.Columns.Add(SchemaTableColumn.BaseColumnName, typeof(string));
			dataTable.Columns.Add(SchemaTableColumn.BaseSchemaName, typeof(string));
			dataTable.Columns.Add(SchemaTableColumn.BaseTableName, typeof(string));
			dataTable.Columns.Add(SchemaTableColumn.DataType, typeof(Type));
			dataTable.Columns.Add(SchemaTableColumn.AllowDBNull, typeof(bool));
			dataTable.Columns.Add(SchemaTableColumn.ProviderType, typeof(int));
			dataTable.Columns.Add(SchemaTableColumn.IsAliased, typeof(bool));
			dataTable.Columns.Add(SchemaTableColumn.IsExpression, typeof(bool));
			dataTable.Columns.Add(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool));
			dataTable.Columns.Add(SchemaTableOptionalColumn.IsRowVersion, typeof(bool));
			dataTable.Columns.Add(SchemaTableOptionalColumn.IsHidden, typeof(bool));
			dataTable.Columns.Add(SchemaTableColumn.IsLong, typeof(bool));
			dataTable.Columns.Add(SchemaTableOptionalColumn.IsReadOnly, typeof(bool));
			dataTable.Columns.Add(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type));
			dataTable.Columns.Add(SchemaTableOptionalColumn.DefaultValue, typeof(object));
			dataTable.Columns.Add("DataTypeName", typeof(string));
			dataTable.Columns.Add("CollationType", typeof(string));
			dataTable.BeginLoadData();
			for (int i = 0; i < this._fieldCount; i++)
			{
				SQLiteType sqliteType = this.GetSQLiteType(this._flags, i);
				DataRow dataRow = dataTable.NewRow();
				DbType type = sqliteType.Type;
				dataRow[SchemaTableColumn.ColumnName] = this.GetName(i);
				dataRow[SchemaTableColumn.ColumnOrdinal] = i;
				dataRow[SchemaTableColumn.ColumnSize] = SQLiteConvert.DbTypeToColumnSize(type);
				dataRow[SchemaTableColumn.NumericPrecision] = SQLiteConvert.DbTypeToNumericPrecision(type);
				dataRow[SchemaTableColumn.NumericScale] = SQLiteConvert.DbTypeToNumericScale(type);
				dataRow[SchemaTableColumn.ProviderType] = sqliteType.Type;
				dataRow[SchemaTableColumn.IsLong] = false;
				dataRow[SchemaTableColumn.AllowDBNull] = true;
				dataRow[SchemaTableOptionalColumn.IsReadOnly] = false;
				dataRow[SchemaTableOptionalColumn.IsRowVersion] = false;
				dataRow[SchemaTableColumn.IsUnique] = false;
				dataRow[SchemaTableColumn.IsKey] = false;
				dataRow[SchemaTableOptionalColumn.IsAutoIncrement] = false;
				dataRow[SchemaTableColumn.DataType] = this.GetFieldType(i);
				dataRow[SchemaTableOptionalColumn.IsHidden] = false;
				dataRow[SchemaTableColumn.BaseSchemaName] = this._baseSchemaName;
				text3 = dictionary2[i].ColumnName;
				if (!string.IsNullOrEmpty(text3))
				{
					dataRow[SchemaTableColumn.BaseColumnName] = text3;
				}
				dataRow[SchemaTableColumn.IsExpression] = string.IsNullOrEmpty(text3);
				dataRow[SchemaTableColumn.IsAliased] = string.Compare(this.GetName(i), text3, StringComparison.OrdinalIgnoreCase) != 0;
				string text4 = dictionary2[i].TableName;
				if (!string.IsNullOrEmpty(text4))
				{
					dataRow[SchemaTableColumn.BaseTableName] = text4;
				}
				text4 = dictionary2[i].DatabaseName;
				if (!string.IsNullOrEmpty(text4))
				{
					dataRow[SchemaTableOptionalColumn.BaseCatalogName] = text4;
				}
				string text5 = null;
				if (!string.IsNullOrEmpty(text3))
				{
					string text6 = string.Empty;
					if (dataRow[SchemaTableOptionalColumn.BaseCatalogName] != DBNull.Value)
					{
						text6 = (string)dataRow[SchemaTableOptionalColumn.BaseCatalogName];
					}
					string text7 = string.Empty;
					if (dataRow[SchemaTableColumn.BaseTableName] != DBNull.Value)
					{
						text7 = (string)dataRow[SchemaTableColumn.BaseTableName];
					}
					if (sql.DoesTableExist(text6, text7))
					{
						string text8 = string.Empty;
						if (dataRow[SchemaTableColumn.BaseColumnName] != DBNull.Value)
						{
							text8 = (string)dataRow[SchemaTableColumn.BaseColumnName];
						}
						string text9 = null;
						bool flag = false;
						bool flag2 = false;
						bool flag3 = false;
						this._command.Connection._sql.ColumnMetaData(text6, text7, text3, true, ref text5, ref text9, ref flag, ref flag2, ref flag3);
						if (flag || flag2)
						{
							dataRow[SchemaTableColumn.AllowDBNull] = false;
						}
						bool flag4 = (bool)dataRow[SchemaTableColumn.AllowDBNull];
						dataRow[SchemaTableColumn.IsKey] = flag2 && SQLiteDataReader.CountParents(dictionary) <= 1;
						dataRow[SchemaTableOptionalColumn.IsAutoIncrement] = flag3;
						dataRow["CollationType"] = text9;
						string[] array = text5.Split(new char[] { '(' });
						if (array.Length > 1)
						{
							text5 = array[0];
							array = array[1].Split(new char[] { ')' });
							if (array.Length > 1)
							{
								array = array[0].Split(new char[] { ',', '.' });
								if (sqliteType.Type == DbType.Binary || SQLiteConvert.IsStringDbType(sqliteType.Type))
								{
									dataRow[SchemaTableColumn.ColumnSize] = Convert.ToInt32(array[0], CultureInfo.InvariantCulture);
								}
								else
								{
									dataRow[SchemaTableColumn.NumericPrecision] = Convert.ToInt32(array[0], CultureInfo.InvariantCulture);
									if (array.Length > 1)
									{
										dataRow[SchemaTableColumn.NumericScale] = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
									}
								}
							}
						}
						if (wantDefaultValue)
						{
							using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].TABLE_INFO([{1}])", new object[] { text6, text7 }), this._command.Connection))
							{
								using (DbDataReader dbDataReader = sqliteCommand.ExecuteReader())
								{
									while (dbDataReader.Read())
									{
										if (string.Compare(text8, dbDataReader.GetString(1), StringComparison.OrdinalIgnoreCase) == 0)
										{
											if (!dbDataReader.IsDBNull(4))
											{
												dataRow[SchemaTableOptionalColumn.DefaultValue] = dbDataReader[4];
												break;
											}
											break;
										}
									}
								}
							}
						}
						if (wantUniqueInfo)
						{
							if (text6 != text || text7 != text2)
							{
								text = text6;
								text2 = text7;
								DbConnection connection = this._command.Connection;
								string text10 = "Indexes";
								string[] array2 = new string[4];
								array2[0] = text6;
								array2[2] = text7;
								dataTable2 = connection.GetSchema(text10, array2);
							}
							foreach (object obj in dataTable2.Rows)
							{
								DataRow dataRow2 = (DataRow)obj;
								DbConnection connection2 = this._command.Connection;
								string text11 = "IndexColumns";
								string[] array3 = new string[5];
								array3[0] = text6;
								array3[2] = text7;
								array3[3] = (string)dataRow2["INDEX_NAME"];
								DataTable schema = connection2.GetSchema(text11, array3);
								foreach (object obj2 in schema.Rows)
								{
									DataRow dataRow3 = (DataRow)obj2;
									if (string.Compare(SQLiteConvert.GetStringOrNull(dataRow3["COLUMN_NAME"]), text3, StringComparison.OrdinalIgnoreCase) == 0)
									{
										if (dictionary.Count == 1 && schema.Rows.Count == 1 && !flag4)
										{
											dataRow[SchemaTableColumn.IsUnique] = dataRow2["UNIQUE"];
											break;
										}
										break;
									}
								}
							}
						}
					}
					if (string.IsNullOrEmpty(text5))
					{
						TypeAffinity typeAffinity = TypeAffinity.Uninitialized;
						text5 = this._activeStatement._sql.ColumnType(this._activeStatement, i, ref typeAffinity);
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dataRow["DataTypeName"] = text5;
					}
				}
				dataTable.Rows.Add(dataRow);
			}
			if (this._keyInfo != null)
			{
				this._keyInfo.AppendSchemaTable(dataTable);
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000555B8 File Offset: 0x000537B8
		public override string GetString(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetString", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					return sqliteDataReaderValue.StringValue;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetString(i - this.PrivateVisibleFieldCount);
			}
			if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoVerifyTextAffinity))
			{
				this.VerifyType(i, DbType.String);
			}
			return this._activeStatement._sql.GetText(this._activeStatement, i);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0005567C File Offset: 0x0005387C
		public override object GetValue(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
			{
				SQLiteDataReaderValue sqliteDataReaderValue = new SQLiteDataReaderValue();
				bool flag;
				this.InvokeReadValueCallback(i, new SQLiteReadValueEventArgs("GetValue", null, sqliteDataReaderValue), out flag);
				if (flag)
				{
					return sqliteDataReaderValue.Value;
				}
			}
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.GetValue(i - this.PrivateVisibleFieldCount);
			}
			SQLiteType sqliteType = this.GetSQLiteType(this._flags, i);
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.DetectTextAffinity) && (sqliteType == null || sqliteType.Affinity == TypeAffinity.Text))
			{
				sqliteType = this.GetSQLiteType(sqliteType, this._activeStatement._sql.GetText(this._activeStatement, i));
			}
			else if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.DetectStringType) && (sqliteType == null || SQLiteConvert.IsStringDbType(sqliteType.Type)))
			{
				sqliteType = this.GetSQLiteType(sqliteType, this._activeStatement._sql.GetText(this._activeStatement, i));
			}
			return this._activeStatement._sql.GetValue(this._activeStatement, this._flags, i, sqliteType);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000557C8 File Offset: 0x000539C8
		public override int GetValues(object[] values)
		{
			this.CheckDisposed();
			int num = this.FieldCount;
			if (values.Length < num)
			{
				num = values.Length;
			}
			for (int i = 0; i < num; i++)
			{
				values[i] = this.GetValue(i);
			}
			return num;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00055810 File Offset: 0x00053A10
		public NameValueCollection GetValues()
		{
			this.CheckDisposed();
			if (this._activeStatement == null || this._activeStatement._sql == null)
			{
				throw new InvalidOperationException();
			}
			int privateVisibleFieldCount = this.PrivateVisibleFieldCount;
			NameValueCollection nameValueCollection = new NameValueCollection(privateVisibleFieldCount);
			for (int i = 0; i < privateVisibleFieldCount; i++)
			{
				string text = this._activeStatement._sql.ColumnName(this._activeStatement, i);
				string text2 = this._activeStatement._sql.GetText(this._activeStatement, i);
				nameValueCollection.Add(text, text2);
			}
			return nameValueCollection;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000558A0 File Offset: 0x00053AA0
		public override bool HasRows
		{
			get
			{
				this.CheckDisposed();
				this.CheckClosed();
				if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StickyHasRows))
				{
					return this._readingState != 1 || this._stepCount > 0;
				}
				return this._readingState != 1;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x000558F8 File Offset: 0x00053AF8
		public override bool IsClosed
		{
			get
			{
				this.CheckDisposed();
				return this._command == null;
			}
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0005590C File Offset: 0x00053B0C
		public override bool IsDBNull(int i)
		{
			this.CheckDisposed();
			this.VerifyForGet();
			if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
			{
				return this._keyInfo.IsDBNull(i - this.PrivateVisibleFieldCount);
			}
			return this._activeStatement._sql.IsNull(this._activeStatement, i);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0005596C File Offset: 0x00053B6C
		public override bool NextResult()
		{
			this.CheckDisposed();
			this.CheckClosed();
			bool throwOnDisposed = this._throwOnDisposed;
			SQLiteStatement sqliteStatement = null;
			bool flag = (this._commandBehavior & CommandBehavior.SchemaOnly) != CommandBehavior.Default;
			int num;
			for (;;)
			{
				if (sqliteStatement == null && this._activeStatement != null && this._activeStatement._sql != null && this._activeStatement._sql.IsOpen())
				{
					if (!flag)
					{
						this._activeStatement._sql.Reset(this._activeStatement);
					}
					if ((this._commandBehavior & CommandBehavior.SingleResult) != CommandBehavior.Default)
					{
						break;
					}
				}
				sqliteStatement = this._command.GetStatement(this._activeStatementIndex + 1);
				if (sqliteStatement == null)
				{
					return false;
				}
				if (this._readingState < 1)
				{
					this._readingState = 1;
				}
				this._activeStatementIndex++;
				num = sqliteStatement._sql.ColumnCount(sqliteStatement);
				if (flag && num != 0)
				{
					goto IL_0231;
				}
				if (!flag && sqliteStatement._sql.Step(sqliteStatement))
				{
					goto Block_18;
				}
				if (num != 0)
				{
					goto IL_022A;
				}
				int num2 = 0;
				bool flag2 = false;
				if (!sqliteStatement.TryGetChanges(ref num2, ref flag2))
				{
					return false;
				}
				if (!flag2)
				{
					if (this._rowsAffected == -1)
					{
						this._rowsAffected = 0;
					}
					this._rowsAffected += num2;
				}
				if (!flag)
				{
					sqliteStatement._sql.Reset(sqliteStatement);
				}
			}
			for (;;)
			{
				sqliteStatement = this._command.GetStatement(this._activeStatementIndex + 1);
				if (sqliteStatement == null)
				{
					return false;
				}
				this._activeStatementIndex++;
				if (!flag && sqliteStatement._sql.Step(sqliteStatement))
				{
					this._stepCount++;
				}
				if (sqliteStatement._sql.ColumnCount(sqliteStatement) == 0)
				{
					int num3 = 0;
					bool flag3 = false;
					if (!sqliteStatement.TryGetChanges(ref num3, ref flag3))
					{
						break;
					}
					if (!flag3)
					{
						if (this._rowsAffected == -1)
						{
							this._rowsAffected = 0;
						}
						this._rowsAffected += num3;
					}
				}
				if (!flag)
				{
					sqliteStatement._sql.Reset(sqliteStatement);
				}
			}
			return false;
			Block_18:
			this._stepCount++;
			this._readingState = -1;
			goto IL_0231;
			IL_022A:
			this._readingState = 1;
			IL_0231:
			this._activeStatement = sqliteStatement;
			this._fieldCount = num;
			this._fieldIndexes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			this._fieldTypeArray = new SQLiteType[this.PrivateVisibleFieldCount];
			if ((this._commandBehavior & CommandBehavior.KeyInfo) != CommandBehavior.Default)
			{
				this.LoadKeyInfo();
			}
			return true;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00055BF4 File Offset: 0x00053DF4
		internal static SQLiteConnection GetConnection(SQLiteDataReader dataReader)
		{
			try
			{
				if (dataReader != null)
				{
					SQLiteCommand command = dataReader._command;
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

		// Token: 0x060011E9 RID: 4585 RVA: 0x00055C48 File Offset: 0x00053E48
		private SQLiteType GetSQLiteType(SQLiteType oldType, string text)
		{
			if (SQLiteConvert.LooksLikeNull(text))
			{
				return new SQLiteType(TypeAffinity.Null, DbType.Object);
			}
			if (SQLiteConvert.LooksLikeInt64(text))
			{
				return new SQLiteType(TypeAffinity.Int64, DbType.Int64);
			}
			if (SQLiteConvert.LooksLikeDouble(text))
			{
				return new SQLiteType(TypeAffinity.Double, DbType.Double);
			}
			if (this._activeStatement != null && SQLiteConvert.LooksLikeDateTime(this._activeStatement._sql, text))
			{
				return new SQLiteType(TypeAffinity.DateTime, DbType.DateTime);
			}
			return oldType;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00055CC0 File Offset: 0x00053EC0
		private SQLiteType GetSQLiteType(SQLiteConnectionFlags flags, int i)
		{
			SQLiteType sqliteType = this._fieldTypeArray[i];
			if (sqliteType == null)
			{
				sqliteType = (this._fieldTypeArray[i] = new SQLiteType());
			}
			if (sqliteType.Affinity == TypeAffinity.Uninitialized)
			{
				sqliteType.Type = SQLiteConvert.TypeNameToDbType(SQLiteDataReader.GetConnection(this), this._activeStatement._sql.ColumnType(this._activeStatement, i, ref sqliteType.Affinity), flags);
			}
			else
			{
				sqliteType.Affinity = this._activeStatement._sql.ColumnAffinity(this._activeStatement, i);
			}
			return sqliteType;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00055D54 File Offset: 0x00053F54
		public override bool Read()
		{
			this.CheckDisposed();
			this.CheckClosed();
			bool throwOnDisposed = this._throwOnDisposed;
			if ((this._commandBehavior & CommandBehavior.SchemaOnly) != CommandBehavior.Default)
			{
				return false;
			}
			if (this._readingState == -1)
			{
				this._readingState = 0;
				return true;
			}
			if (this._readingState == 0)
			{
				if ((this._commandBehavior & CommandBehavior.SingleRow) == CommandBehavior.Default && this._activeStatement._sql.Step(this._activeStatement))
				{
					this._stepCount++;
					if (this._keyInfo != null)
					{
						this._keyInfo.Reset();
					}
					return true;
				}
				this._readingState = 1;
			}
			return false;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00055E00 File Offset: 0x00054000
		public override int RecordsAffected
		{
			get
			{
				this.CheckDisposed();
				return this._rowsAffected;
			}
		}

		// Token: 0x17000344 RID: 836
		public override object this[string name]
		{
			get
			{
				this.CheckDisposed();
				return this.GetValue(this.GetOrdinal(name));
			}
		}

		// Token: 0x17000345 RID: 837
		public override object this[int i]
		{
			get
			{
				this.CheckDisposed();
				return this.GetValue(i);
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00055E38 File Offset: 0x00054038
		private void LoadKeyInfo()
		{
			if (this._keyInfo != null)
			{
				this._keyInfo.Dispose();
				this._keyInfo = null;
			}
			this._keyInfo = new SQLiteKeyReader(this._command.Connection, this, this._activeStatement);
		}

		// Token: 0x0400072F RID: 1839
		private SQLiteCommand _command;

		// Token: 0x04000730 RID: 1840
		private SQLiteConnectionFlags _flags;

		// Token: 0x04000731 RID: 1841
		private int _activeStatementIndex;

		// Token: 0x04000732 RID: 1842
		private SQLiteStatement _activeStatement;

		// Token: 0x04000733 RID: 1843
		private int _readingState;

		// Token: 0x04000734 RID: 1844
		private int _rowsAffected;

		// Token: 0x04000735 RID: 1845
		private int _fieldCount;

		// Token: 0x04000736 RID: 1846
		private int _stepCount;

		// Token: 0x04000737 RID: 1847
		private Dictionary<string, int> _fieldIndexes;

		// Token: 0x04000738 RID: 1848
		private SQLiteType[] _fieldTypeArray;

		// Token: 0x04000739 RID: 1849
		private CommandBehavior _commandBehavior;

		// Token: 0x0400073A RID: 1850
		internal bool _disposeCommand;

		// Token: 0x0400073B RID: 1851
		internal bool _throwOnDisposed;

		// Token: 0x0400073C RID: 1852
		private SQLiteKeyReader _keyInfo;

		// Token: 0x0400073D RID: 1853
		internal int _version;

		// Token: 0x0400073E RID: 1854
		private string _baseSchemaName;

		// Token: 0x0400073F RID: 1855
		private bool disposed;

		// Token: 0x02000298 RID: 664
		private sealed class ColumnParent : IEqualityComparer<SQLiteDataReader.ColumnParent>
		{
			// Token: 0x0600187B RID: 6267 RVA: 0x00069D30 File Offset: 0x00067F30
			public ColumnParent()
			{
			}

			// Token: 0x0600187C RID: 6268 RVA: 0x00069D38 File Offset: 0x00067F38
			public ColumnParent(string databaseName, string tableName, string columnName)
				: this()
			{
				this.DatabaseName = databaseName;
				this.TableName = tableName;
				this.ColumnName = columnName;
			}

			// Token: 0x0600187D RID: 6269 RVA: 0x00069D58 File Offset: 0x00067F58
			public bool Equals(SQLiteDataReader.ColumnParent x, SQLiteDataReader.ColumnParent y)
			{
				return (x == null && y == null) || (x != null && y != null && string.Equals(x.DatabaseName, y.DatabaseName, StringComparison.OrdinalIgnoreCase) && string.Equals(x.TableName, y.TableName, StringComparison.OrdinalIgnoreCase) && string.Equals(x.ColumnName, y.ColumnName, StringComparison.OrdinalIgnoreCase));
			}

			// Token: 0x0600187E RID: 6270 RVA: 0x00069DD4 File Offset: 0x00067FD4
			public int GetHashCode(SQLiteDataReader.ColumnParent obj)
			{
				int num = 0;
				if (obj != null && obj.DatabaseName != null)
				{
					num ^= obj.DatabaseName.GetHashCode();
				}
				if (obj != null && obj.TableName != null)
				{
					num ^= obj.TableName.GetHashCode();
				}
				if (obj != null && obj.ColumnName != null)
				{
					num ^= obj.ColumnName.GetHashCode();
				}
				return num;
			}

			// Token: 0x04000B3B RID: 2875
			public string DatabaseName;

			// Token: 0x04000B3C RID: 2876
			public string TableName;

			// Token: 0x04000B3D RID: 2877
			public string ColumnName;
		}
	}
}
