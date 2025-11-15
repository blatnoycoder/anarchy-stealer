using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Transactions;

namespace System.Data.SQLite
{
	// Token: 0x02000166 RID: 358
	public sealed class SQLiteConnection : DbConnection, ICloneable, IDisposable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000FDA RID: 4058 RVA: 0x00048F74 File Offset: 0x00047174
		// (remove) Token: 0x06000FDB RID: 4059 RVA: 0x00048FAC File Offset: 0x000471AC
		private static event SQLiteConnectionEventHandler _handlers;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000FDC RID: 4060 RVA: 0x00048FE4 File Offset: 0x000471E4
		// (remove) Token: 0x06000FDD RID: 4061 RVA: 0x00049020 File Offset: 0x00047220
		private event SQLiteBusyEventHandler _busyHandler;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000FDE RID: 4062 RVA: 0x0004905C File Offset: 0x0004725C
		// (remove) Token: 0x06000FDF RID: 4063 RVA: 0x00049098 File Offset: 0x00047298
		private event SQLiteProgressEventHandler _progressHandler;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000FE0 RID: 4064 RVA: 0x000490D4 File Offset: 0x000472D4
		// (remove) Token: 0x06000FE1 RID: 4065 RVA: 0x00049110 File Offset: 0x00047310
		private event SQLiteAuthorizerEventHandler _authorizerHandler;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000FE2 RID: 4066 RVA: 0x0004914C File Offset: 0x0004734C
		// (remove) Token: 0x06000FE3 RID: 4067 RVA: 0x00049188 File Offset: 0x00047388
		private event SQLiteUpdateEventHandler _updateHandler;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000FE4 RID: 4068 RVA: 0x000491C4 File Offset: 0x000473C4
		// (remove) Token: 0x06000FE5 RID: 4069 RVA: 0x00049200 File Offset: 0x00047400
		private event SQLiteCommitHandler _commitHandler;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000FE6 RID: 4070 RVA: 0x0004923C File Offset: 0x0004743C
		// (remove) Token: 0x06000FE7 RID: 4071 RVA: 0x00049278 File Offset: 0x00047478
		private event SQLiteTraceEventHandler _traceHandler;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000FE8 RID: 4072 RVA: 0x000492B4 File Offset: 0x000474B4
		// (remove) Token: 0x06000FE9 RID: 4073 RVA: 0x000492F0 File Offset: 0x000474F0
		private event EventHandler _rollbackHandler;

		// Token: 0x06000FEA RID: 4074 RVA: 0x0004932C File Offset: 0x0004752C
		private static string GetDefaultCatalogName()
		{
			return SQLiteConnection._defaultCatalogName;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00049334 File Offset: 0x00047534
		private static bool IsDefaultCatalogName(string catalogName)
		{
			return string.Compare(catalogName, SQLiteConnection.GetDefaultCatalogName(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00049348 File Offset: 0x00047548
		private static string GetTemporaryCatalogName()
		{
			return SQLiteConnection._temporaryCatalogName;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00049350 File Offset: 0x00047550
		private static bool IsTemporaryCatalogName(string catalogName)
		{
			return string.Compare(catalogName, SQLiteConnection.GetTemporaryCatalogName(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00049364 File Offset: 0x00047564
		private static string GetMasterTableName(bool temporary)
		{
			if (!temporary)
			{
				return SQLiteConnection._defaultMasterTableName;
			}
			return SQLiteConnection._temporaryMasterTableName;
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000FEF RID: 4079 RVA: 0x00049378 File Offset: 0x00047578
		// (remove) Token: 0x06000FF0 RID: 4080 RVA: 0x000493B4 File Offset: 0x000475B4
		public override event StateChangeEventHandler StateChange;

		// Token: 0x06000FF1 RID: 4081 RVA: 0x000493F0 File Offset: 0x000475F0
		public SQLiteConnection()
			: this(null)
		{
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x000493FC File Offset: 0x000475FC
		public SQLiteConnection(string connectionString)
			: this(connectionString, false)
		{
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00049408 File Offset: 0x00047608
		internal SQLiteConnection(IntPtr db, string fileName, bool ownHandle)
			: this()
		{
			this._sql = new SQLite3(SQLiteDateFormats.ISO8601, DateTimeKind.Unspecified, null, db, fileName, ownHandle);
			this._flags = SQLiteConnectionFlags.None;
			this._connectionState = ((db != IntPtr.Zero) ? ConnectionState.Open : ConnectionState.Closed);
			this._connectionString = null;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0004945C File Offset: 0x0004765C
		private void InitializeDefaults()
		{
			this._defaultDbType = SQLiteConnection._DefaultDbType;
			this._defaultTypeName = null;
			this._vfsName = null;
			this._defaultTimeout = 30;
			this._busyTimeout = 0;
			this._waitTimeout = 30000;
			this._prepareRetries = 3;
			this._progressOps = 0;
			this._defaultIsolation = IsolationLevel.Serializable;
			this._baseSchemaName = "sqlite_default_schema";
			this._binaryGuid = true;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x000494CC File Offset: 0x000476CC
		public SQLiteConnection(string connectionString, bool parseViaFramework)
		{
			this._noDispose = false;
			UnsafeNativeMethods.Initialize();
			SQLiteLog.Initialize(typeof(SQLiteConnection).Name);
			this._cachedSettings = new Dictionary<string, object>(new TypeNameStringComparer());
			this._typeNames = new SQLiteDbTypeMap();
			this._typeCallbacks = new SQLiteTypeCallbacksMap();
			this._parseViaFramework = parseViaFramework;
			this._flags = SQLiteConnectionFlags.None;
			this.InitializeDefaults();
			this._connectionState = ConnectionState.Closed;
			this._connectionString = null;
			if (connectionString != null)
			{
				this.ConnectionString = connectionString;
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00049564 File Offset: 0x00047764
		public SQLiteConnection(SQLiteConnection connection)
			: this(connection.ConnectionString, connection.ParseViaFramework)
		{
			if (connection.State == ConnectionState.Open)
			{
				this.Open();
				using (DataTable schema = connection.GetSchema("Catalogs"))
				{
					foreach (object obj in schema.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						string text = dataRow[0].ToString();
						if (!SQLiteConnection.IsDefaultCatalogName(text) && !SQLiteConnection.IsTemporaryCatalogName(text))
						{
							using (SQLiteCommand sqliteCommand = this.CreateCommand())
							{
								sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "ATTACH DATABASE '{0}' AS [{1}]", new object[]
								{
									dataRow[1],
									dataRow[0]
								});
								sqliteCommand.ExecuteNonQuery();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004969C File Offset: 0x0004789C
		private static SQLiteConnectionHandle GetCriticalHandle(SQLiteConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			SQLite3 sqlite = connection._sql as SQLite3;
			if (sqlite == null)
			{
				throw new InvalidOperationException("Connection has no wrapper");
			}
			SQLiteConnectionHandle sql = sqlite._sql;
			if (sql == null)
			{
				throw new InvalidOperationException("Connection has an invalid handle.");
			}
			IntPtr intPtr = sql;
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("Connection has an invalid handle pointer.");
			}
			return sql;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00049718 File Offset: 0x00047918
		public object GetCriticalHandle()
		{
			this.CheckDisposed();
			return SQLiteConnection.GetCriticalHandle(this);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00049728 File Offset: 0x00047928
		public static ISQLiteConnectionPool CreatePool(string typeName, object argument)
		{
			if (typeName == null)
			{
				return null;
			}
			if (typeName != null)
			{
				if (typeName == "weak")
				{
					return new WeakConnectionPool();
				}
				if (typeName == "strong")
				{
					return new StrongConnectionPool();
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00049780 File Offset: 0x00047980
		internal static void OnChanged(SQLiteConnection connection, ConnectionEventArgs e)
		{
			if (connection != null && !connection.CanRaiseEvents)
			{
				return;
			}
			SQLiteConnectionEventHandler sqliteConnectionEventHandler;
			lock (SQLiteConnection._syncRoot)
			{
				if (SQLiteConnection._handlers != null)
				{
					sqliteConnectionEventHandler = SQLiteConnection._handlers.Clone() as SQLiteConnectionEventHandler;
				}
				else
				{
					sqliteConnectionEventHandler = null;
				}
			}
			if (sqliteConnectionEventHandler != null)
			{
				sqliteConnectionEventHandler(connection, e);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000FFB RID: 4091 RVA: 0x00049800 File Offset: 0x00047A00
		// (remove) Token: 0x06000FFC RID: 4092 RVA: 0x0004984C File Offset: 0x00047A4C
		public static event SQLiteConnectionEventHandler Changed
		{
			add
			{
				lock (SQLiteConnection._syncRoot)
				{
					SQLiteConnection._handlers -= value;
					SQLiteConnection._handlers += value;
				}
			}
			remove
			{
				lock (SQLiteConnection._syncRoot)
				{
					SQLiteConnection._handlers -= value;
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00049894 File Offset: 0x00047A94
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x0004989C File Offset: 0x00047A9C
		public static ISQLiteConnectionPool ConnectionPool
		{
			get
			{
				return SQLiteConnectionPool.GetConnectionPool();
			}
			set
			{
				SQLiteConnectionPool.SetConnectionPool(value);
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000498A4 File Offset: 0x00047AA4
		public static object CreateHandle(IntPtr nativeHandle)
		{
			SQLiteConnectionHandle sqliteConnectionHandle;
			try
			{
			}
			finally
			{
				sqliteConnectionHandle = ((nativeHandle != IntPtr.Zero) ? new SQLiteConnectionHandle(nativeHandle, true) : null);
			}
			if (sqliteConnectionHandle != null)
			{
				SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, sqliteConnectionHandle, null, new object[]
				{
					typeof(SQLiteConnection),
					nativeHandle
				}));
			}
			return sqliteConnectionHandle;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00049920 File Offset: 0x00047B20
		public void BackupDatabase(SQLiteConnection destination, string destinationName, string sourceName, int pages, SQLiteBackupCallback callback, int retryMilliseconds)
		{
			this.CheckDisposed();
			if (this._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException("Source database is not open.");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (destination._connectionState != ConnectionState.Open)
			{
				throw new ArgumentException("Destination database is not open.", "destination");
			}
			if (destinationName == null)
			{
				throw new ArgumentNullException("destinationName");
			}
			if (sourceName == null)
			{
				throw new ArgumentNullException("sourceName");
			}
			SQLiteBase sql = this._sql;
			if (sql == null)
			{
				throw new InvalidOperationException("Connection object has an invalid handle.");
			}
			SQLiteBackup sqliteBackup = null;
			try
			{
				sqliteBackup = sql.InitializeBackup(destination, destinationName, sourceName);
				bool flag = false;
				while (sql.StepBackup(sqliteBackup, pages, ref flag) && (callback == null || callback(this, sourceName, destination, destinationName, pages, sql.RemainingBackup(sqliteBackup), sql.PageCountBackup(sqliteBackup), flag)))
				{
					if (flag && retryMilliseconds >= 0)
					{
						Thread.Sleep(retryMilliseconds);
					}
					if (pages == 0)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (HelperMethods.LogBackup(this._flags))
				{
					SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception while backing up database: {0}", new object[] { ex }));
				}
				throw;
			}
			finally
			{
				if (sqliteBackup != null)
				{
					sql.FinishBackup(sqliteBackup);
				}
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00049A80 File Offset: 0x00047C80
		public int ClearCachedSettings()
		{
			this.CheckDisposed();
			int num = -1;
			if (this._cachedSettings != null)
			{
				num = this._cachedSettings.Count;
				this._cachedSettings.Clear();
			}
			return num;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00049ABC File Offset: 0x00047CBC
		internal bool TryGetCachedSetting(string name, object @default, out object value)
		{
			if (name == null || this._cachedSettings == null)
			{
				value = @default;
				return false;
			}
			return this._cachedSettings.TryGetValue(name, out value);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00049AE4 File Offset: 0x00047CE4
		internal void SetCachedSetting(string name, object value)
		{
			if (name == null || this._cachedSettings == null)
			{
				return;
			}
			this._cachedSettings[name] = value;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00049B08 File Offset: 0x00047D08
		public int ClearTypeMappings()
		{
			this.CheckDisposed();
			int num = -1;
			if (this._typeNames != null)
			{
				num = this._typeNames.Clear();
			}
			return num;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00049B3C File Offset: 0x00047D3C
		public Dictionary<string, object> GetTypeMappings()
		{
			this.CheckDisposed();
			Dictionary<string, object> dictionary = null;
			if (this._typeNames != null)
			{
				dictionary = new Dictionary<string, object>(this._typeNames.Count, this._typeNames.Comparer);
				foreach (KeyValuePair<string, SQLiteDbTypeMapping> keyValuePair in this._typeNames)
				{
					SQLiteDbTypeMapping value = keyValuePair.Value;
					object obj = null;
					object obj2 = null;
					object obj3 = null;
					if (value != null)
					{
						obj = value.typeName;
						obj2 = value.dataType;
						obj3 = value.primary;
					}
					dictionary.Add(keyValuePair.Key, new object[] { obj, obj2, obj3 });
				}
			}
			return dictionary;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00049C20 File Offset: 0x00047E20
		public int AddTypeMapping(string typeName, DbType dataType, bool primary)
		{
			this.CheckDisposed();
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			int num = -1;
			if (this._typeNames != null)
			{
				num = 0;
				if (primary && this._typeNames.ContainsKey(dataType))
				{
					num += (this._typeNames.Remove(dataType) ? 1 : 0);
				}
				if (this._typeNames.ContainsKey(typeName))
				{
					num += (this._typeNames.Remove(typeName) ? 1 : 0);
				}
				this._typeNames.Add(new SQLiteDbTypeMapping(typeName, dataType, primary));
			}
			return num;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00049CCC File Offset: 0x00047ECC
		public int ClearTypeCallbacks()
		{
			this.CheckDisposed();
			int num = -1;
			if (this._typeCallbacks != null)
			{
				num = this._typeCallbacks.Count;
				this._typeCallbacks.Clear();
			}
			return num;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00049D08 File Offset: 0x00047F08
		public bool TryGetTypeCallbacks(string typeName, out SQLiteTypeCallbacks callbacks)
		{
			this.CheckDisposed();
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (this._typeCallbacks == null)
			{
				callbacks = null;
				return false;
			}
			return this._typeCallbacks.TryGetValue(typeName, out callbacks);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00049D40 File Offset: 0x00047F40
		public bool SetTypeCallbacks(string typeName, SQLiteTypeCallbacks callbacks)
		{
			this.CheckDisposed();
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (this._typeCallbacks == null)
			{
				return false;
			}
			if (callbacks == null)
			{
				return this._typeCallbacks.Remove(typeName);
			}
			callbacks.TypeName = typeName;
			this._typeCallbacks[typeName] = callbacks;
			return true;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00049DA0 File Offset: 0x00047FA0
		public void BindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteFunction function)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for binding functions.");
			}
			this._sql.BindFunction(functionAttribute, function, this._flags);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00049DD4 File Offset: 0x00047FD4
		public void BindFunction(SQLiteFunctionAttribute functionAttribute, Delegate callback1, Delegate callback2)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for binding functions.");
			}
			this._sql.BindFunction(functionAttribute, new SQLiteDelegateFunction(callback1, callback2), this._flags);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00049E0C File Offset: 0x0004800C
		public bool UnbindFunction(SQLiteFunctionAttribute functionAttribute)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for unbinding functions.");
			}
			return this._sql.UnbindFunction(functionAttribute, this._flags);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00049E3C File Offset: 0x0004803C
		public bool UnbindAllFunctions(bool registered)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for unbinding functions.");
			}
			return SQLiteFunction.UnbindAllFunctions(this._sql, this._flags, registered);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00049E6C File Offset: 0x0004806C
		[Conditional("CHECK_STATE")]
		internal static void Check(SQLiteConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			connection.CheckDisposed();
			if (connection._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException("The connection is not open.");
			}
			SQLite3 sqlite = connection._sql as SQLite3;
			if (sqlite == null)
			{
				throw new InvalidOperationException("The connection handle wrapper is null.");
			}
			SQLiteConnectionHandle sql = sqlite._sql;
			if (sql == null)
			{
				throw new InvalidOperationException("The connection handle is null.");
			}
			if (sql.IsInvalid)
			{
				throw new InvalidOperationException("The connection handle is invalid.");
			}
			if (sql.IsClosed)
			{
				throw new InvalidOperationException("The connection handle is closed.");
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00049F0C File Offset: 0x0004810C
		internal static SortedList<string, string> ParseConnectionString(string connectionString, bool parseViaFramework, bool allowNameOnly)
		{
			return SQLiteConnection.ParseConnectionString(null, connectionString, parseViaFramework, allowNameOnly);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00049F18 File Offset: 0x00048118
		private static SortedList<string, string> ParseConnectionString(SQLiteConnection connection, string connectionString, bool parseViaFramework, bool allowNameOnly)
		{
			if (!parseViaFramework)
			{
				return SQLiteConnection.ParseConnectionString(connection, connectionString, allowNameOnly);
			}
			return SQLiteConnection.ParseConnectionStringViaFramework(connection, connectionString, false);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00049F34 File Offset: 0x00048134
		private static string EscapeForConnectionString(string value, bool allowEquals)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			if (value.IndexOfAny(SQLiteConvert.SpecialChars) == -1)
			{
				return value;
			}
			int length = value.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			int i = 0;
			while (i < length)
			{
				char c = value[i];
				char c2 = c;
				if (c2 <= '\'')
				{
					if (c2 != '"' && c2 != '\'')
					{
						goto IL_00B3;
					}
					goto IL_007F;
				}
				else
				{
					switch (c2)
					{
					case ';':
						goto IL_007F;
					case '<':
						goto IL_00B3;
					case '=':
						if (!allowEquals)
						{
							throw new ArgumentException("equals sign character is not allowed here");
						}
						stringBuilder.Append(c);
						break;
					default:
						if (c2 == '\\')
						{
							goto IL_007F;
						}
						goto IL_00B3;
					}
				}
				IL_00BB:
				i++;
				continue;
				IL_007F:
				stringBuilder.Append('\\');
				stringBuilder.Append(c);
				goto IL_00BB;
				IL_00B3:
				stringBuilder.Append(c);
				goto IL_00BB;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0004A014 File Offset: 0x00048214
		private static string BuildConnectionString(SortedList<string, string> opts)
		{
			if (opts == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> keyValuePair in opts)
			{
				stringBuilder.AppendFormat("{0}{1}{2}{3}", new object[]
				{
					SQLiteConnection.EscapeForConnectionString(keyValuePair.Key, false),
					'=',
					SQLiteConnection.EscapeForConnectionString(keyValuePair.Value, true),
					';'
				});
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0004A0BC File Offset: 0x000482BC
		private void SetupSQLiteBase(SortedList<string, string> opts)
		{
			object obj = SQLiteConnection.TryParseEnum(typeof(SQLiteDateFormats), SQLiteConnection.FindKey(opts, "DateTimeFormat", SQLiteDateFormats.ISO8601.ToString()), true);
			SQLiteDateFormats sqliteDateFormats = ((obj is SQLiteDateFormats) ? ((SQLiteDateFormats)obj) : SQLiteDateFormats.ISO8601);
			obj = SQLiteConnection.TryParseEnum(typeof(DateTimeKind), SQLiteConnection.FindKey(opts, "DateTimeKind", DateTimeKind.Unspecified.ToString()), true);
			DateTimeKind dateTimeKind = ((obj is DateTimeKind) ? ((DateTimeKind)obj) : DateTimeKind.Unspecified);
			string text = SQLiteConnection.FindKey(opts, "DateTimeFormatString", null);
			if (SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(opts, "UseUTF16Encoding", false.ToString())))
			{
				this._sql = new SQLite3_UTF16(sqliteDateFormats, dateTimeKind, text, IntPtr.Zero, null, false);
				return;
			}
			this._sql = new SQLite3(sqliteDateFormats, dateTimeKind, text, IntPtr.Zero, null, false);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0004A1A4 File Offset: 0x000483A4
		public new void Dispose()
		{
			if (this._noDispose)
			{
				return;
			}
			base.Dispose();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0004A1B8 File Offset: 0x000483B8
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteConnection).Name);
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0004A1DC File Offset: 0x000483DC
		protected override void Dispose(bool disposing)
		{
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.TraceWarning) && this._noDispose)
			{
				global::System.Diagnostics.Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "WARNING: Disposing of connection \"{0}\" with the no-dispose flag set.", new object[] { this._connectionString }));
			}
			this._disposing = true;
			try
			{
				if (!this.disposed)
				{
					this.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0004A26C File Offset: 0x0004846C
		public object Clone()
		{
			this.CheckDisposed();
			return new SQLiteConnection(this);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0004A27C File Offset: 0x0004847C
		public static void CreateFile(string databaseFileName)
		{
			FileStream fileStream = File.Create(databaseFileName);
			fileStream.Close();
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0004A29C File Offset: 0x0004849C
		internal void OnStateChange(ConnectionState newState, ref StateChangeEventArgs eventArgs)
		{
			ConnectionState connectionState = this._connectionState;
			this._connectionState = newState;
			if (this.StateChange != null && newState != connectionState)
			{
				StateChangeEventArgs stateChangeEventArgs = new StateChangeEventArgs(connectionState, newState);
				this.StateChange(this, stateChangeEventArgs);
				eventArgs = stateChangeEventArgs;
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0004A2E8 File Offset: 0x000484E8
		private static IsolationLevel GetFallbackDefaultIsolationLevel()
		{
			return IsolationLevel.Serializable;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0004A2F0 File Offset: 0x000484F0
		internal IsolationLevel GetDefaultIsolationLevel()
		{
			return this._defaultIsolation;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0004A2F8 File Offset: 0x000484F8
		[Obsolete("Use one of the standard BeginTransaction methods, this one will be removed soon")]
		public SQLiteTransaction BeginTransaction(IsolationLevel isolationLevel, bool deferredLock)
		{
			this.CheckDisposed();
			return (SQLiteTransaction)this.BeginDbTransaction((!deferredLock) ? IsolationLevel.Serializable : IsolationLevel.ReadCommitted);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0004A320 File Offset: 0x00048520
		[Obsolete("Use one of the standard BeginTransaction methods, this one will be removed soon")]
		public SQLiteTransaction BeginTransaction(bool deferredLock)
		{
			this.CheckDisposed();
			return (SQLiteTransaction)this.BeginDbTransaction((!deferredLock) ? IsolationLevel.Serializable : IsolationLevel.ReadCommitted);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0004A348 File Offset: 0x00048548
		public new SQLiteTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			this.CheckDisposed();
			return (SQLiteTransaction)this.BeginDbTransaction(isolationLevel);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0004A35C File Offset: 0x0004855C
		public new SQLiteTransaction BeginTransaction()
		{
			this.CheckDisposed();
			return (SQLiteTransaction)this.BeginDbTransaction(this._defaultIsolation);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0004A378 File Offset: 0x00048578
		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			if (this._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException();
			}
			if (isolationLevel == IsolationLevel.Unspecified)
			{
				isolationLevel = this._defaultIsolation;
			}
			isolationLevel = this.GetEffectiveIsolationLevel(isolationLevel);
			if (isolationLevel != IsolationLevel.Serializable && isolationLevel != IsolationLevel.ReadCommitted)
			{
				throw new ArgumentException("isolationLevel");
			}
			SQLiteTransaction sqliteTransaction;
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.AllowNestedTransactions))
			{
				sqliteTransaction = new SQLiteTransaction2(this, isolationLevel != IsolationLevel.Serializable);
			}
			else
			{
				sqliteTransaction = new SQLiteTransaction(this, isolationLevel != IsolationLevel.Serializable);
			}
			SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.NewTransaction, null, sqliteTransaction, null, null, null, null, null));
			return sqliteTransaction;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004A42C File Offset: 0x0004862C
		public override void ChangeDatabase(string databaseName)
		{
			this.CheckDisposed();
			SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.ChangeDatabase, null, null, null, null, null, databaseName, null));
			throw new NotImplementedException();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0004A45C File Offset: 0x0004865C
		public override void Close()
		{
			this.CheckDisposed();
			SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Closing, null, null, null, null, null, null, null));
			if (this._sql != null)
			{
				lock (this._enlistmentSyncRoot)
				{
					SQLiteEnlistment enlistment = this._enlistment;
					this._enlistment = null;
					if (enlistment != null)
					{
						SQLiteConnection sqliteConnection = new SQLiteConnection();
						sqliteConnection._sql = this._sql;
						sqliteConnection._transactionLevel = this._transactionLevel;
						sqliteConnection._transactionSequence = this._transactionSequence;
						sqliteConnection._enlistment = enlistment;
						sqliteConnection._connectionState = this._connectionState;
						sqliteConnection._version = this._version;
						SQLiteTransaction transaction = enlistment._transaction;
						if (transaction != null)
						{
							transaction._cnn = sqliteConnection;
						}
						enlistment._disposeConnection = true;
						this._sql = null;
					}
				}
				if (this._sql != null)
				{
					this._sql.Close(this._disposing);
					this._sql = null;
				}
				this._transactionLevel = 0;
				this._transactionSequence = 0;
			}
			StateChangeEventArgs stateChangeEventArgs = null;
			this.OnStateChange(ConnectionState.Closed, ref stateChangeEventArgs);
			SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Closed, stateChangeEventArgs, null, null, null, null, null, null));
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0004A598 File Offset: 0x00048798
		public int PoolCount
		{
			get
			{
				if (this._sql == null)
				{
					return 0;
				}
				return this._sql.CountPool();
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0004A5B4 File Offset: 0x000487B4
		public static long CreateCount
		{
			get
			{
				return SQLiteBase.CreateCount;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0004A5BC File Offset: 0x000487BC
		public static long OpenCount
		{
			get
			{
				return SQLiteBase.OpenCount;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0004A5C4 File Offset: 0x000487C4
		public static long CloseCount
		{
			get
			{
				return SQLiteBase.CloseCount;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0004A5CC File Offset: 0x000487CC
		public static long DisposeCount
		{
			get
			{
				return SQLiteBase.DisposeCount;
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0004A5D4 File Offset: 0x000487D4
		public static void ClearPool(SQLiteConnection connection)
		{
			if (connection._sql == null)
			{
				return;
			}
			connection._sql.ClearPool();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0004A5F0 File Offset: 0x000487F0
		public static void ClearAllPools()
		{
			SQLiteConnectionPool.ClearAllPools();
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0004A5F8 File Offset: 0x000487F8
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x0004A608 File Offset: 0x00048808
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
		[Editor("SQLite.Designer.SQLiteConnectionStringEditor, SQLite.Designer, Version=1.0.116.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public override string ConnectionString
		{
			get
			{
				this.CheckDisposed();
				return this._connectionString;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (this._connectionState != ConnectionState.Closed)
				{
					throw new InvalidOperationException();
				}
				this._connectionString = value;
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0004A634 File Offset: 0x00048834
		public new SQLiteCommand CreateCommand()
		{
			this.CheckDisposed();
			return new SQLiteCommand(this);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0004A644 File Offset: 0x00048844
		protected override DbCommand CreateDbCommand()
		{
			return this.CreateCommand();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0004A64C File Offset: 0x0004884C
		public ISQLiteSession CreateSession(string databaseName)
		{
			this.CheckDisposed();
			return new SQLiteSession(SQLiteConnection.GetCriticalHandle(this), this._flags, databaseName);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0004A668 File Offset: 0x00048868
		public ISQLiteChangeSet CreateChangeSet(byte[] rawData)
		{
			this.CheckDisposed();
			return new SQLiteMemoryChangeSet(rawData, SQLiteConnection.GetCriticalHandle(this), this._flags);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0004A684 File Offset: 0x00048884
		public ISQLiteChangeSet CreateChangeSet(byte[] rawData, SQLiteChangeSetStartFlags flags)
		{
			this.CheckDisposed();
			return new SQLiteMemoryChangeSet(rawData, SQLiteConnection.GetCriticalHandle(this), this._flags, flags);
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0004A6A0 File Offset: 0x000488A0
		public ISQLiteChangeSet CreateChangeSet(Stream inputStream, Stream outputStream)
		{
			this.CheckDisposed();
			return new SQLiteStreamChangeSet(inputStream, outputStream, SQLiteConnection.GetCriticalHandle(this), this._flags);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0004A6BC File Offset: 0x000488BC
		public ISQLiteChangeSet CreateChangeSet(Stream inputStream, Stream outputStream, SQLiteChangeSetStartFlags flags)
		{
			this.CheckDisposed();
			return new SQLiteStreamChangeSet(inputStream, outputStream, SQLiteConnection.GetCriticalHandle(this), this._flags, flags);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0004A6D8 File Offset: 0x000488D8
		public ISQLiteChangeGroup CreateChangeGroup()
		{
			this.CheckDisposed();
			return new SQLiteChangeGroup(this._flags);
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0004A6EC File Offset: 0x000488EC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string DataSource
		{
			get
			{
				this.CheckDisposed();
				return this._dataSource;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0004A6FC File Offset: 0x000488FC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string FileName
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for getting file name.");
				}
				return this._sql.GetFileName(SQLiteConnection.GetDefaultCatalogName());
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0004A72C File Offset: 0x0004892C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Database
		{
			get
			{
				this.CheckDisposed();
				return SQLiteConnection.GetDefaultCatalogName();
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0004A73C File Offset: 0x0004893C
		internal static string MapUriPath(string path)
		{
			if (path.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
			{
				return path.Substring(7);
			}
			if (path.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
			{
				return path.Substring(5);
			}
			if (path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
			{
				return path;
			}
			throw new InvalidOperationException("Invalid connection string: invalid URI");
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0004A79C File Offset: 0x0004899C
		private static bool ShouldUseLegacyConnectionStringParser(SQLiteConnection connection)
		{
			string text = "No_SQLiteConnectionNewParser";
			object settingValue;
			if (connection != null && connection.TryGetCachedSetting(text, null, out settingValue))
			{
				return settingValue != null;
			}
			if (connection == null && SQLiteConnection.TryGetLastCachedSetting(text, null, out settingValue))
			{
				return settingValue != null;
			}
			settingValue = UnsafeNativeMethods.GetSettingValue(text, null);
			if (connection != null)
			{
				connection.SetCachedSetting(text, settingValue);
			}
			else
			{
				SQLiteConnection.SetLastCachedSetting(text, settingValue);
			}
			return settingValue != null;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0004A818 File Offset: 0x00048A18
		private static SortedList<string, string> ParseConnectionString(string connectionString, bool allowNameOnly)
		{
			return SQLiteConnection.ParseConnectionString(null, connectionString, allowNameOnly);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0004A824 File Offset: 0x00048A24
		private static SortedList<string, string> ParseConnectionString(SQLiteConnection connection, string connectionString, bool allowNameOnly)
		{
			SortedList<string, string> sortedList = new SortedList<string, string>(StringComparer.OrdinalIgnoreCase);
			string text = null;
			string[] array;
			if (SQLiteConnection.ShouldUseLegacyConnectionStringParser(connection))
			{
				array = SQLiteConvert.Split(connectionString, ';');
			}
			else
			{
				array = SQLiteConvert.NewSplit(connectionString, ';', true, ref text);
			}
			if (array == null)
			{
				throw new ArgumentException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Invalid ConnectionString format, cannot parse: {0}", new object[] { (text != null) ? text : "could not split connection string into properties" }));
			}
			int num = ((array != null) ? array.Length : 0);
			for (int i = 0; i < num; i++)
			{
				if (array[i] != null)
				{
					array[i] = array[i].Trim();
					if (array[i].Length != 0)
					{
						int num2 = array[i].IndexOf('=');
						if (num2 != -1)
						{
							sortedList.Add(SQLiteConnection.UnwrapString(array[i].Substring(0, num2).Trim()), SQLiteConnection.UnwrapString(array[i].Substring(num2 + 1).Trim()));
						}
						else
						{
							if (!allowNameOnly)
							{
								throw new ArgumentException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Invalid ConnectionString format for part \"{0}\", no equal sign found", new object[] { array[i] }));
							}
							sortedList.Add(SQLiteConnection.UnwrapString(array[i].Trim()), string.Empty);
						}
					}
				}
			}
			return sortedList;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0004A9AC File Offset: 0x00048BAC
		private static SortedList<string, string> ParseConnectionStringViaFramework(SQLiteConnection connection, string connectionString, bool strict)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
			dbConnectionStringBuilder.ConnectionString = connectionString;
			SortedList<string, string> sortedList = new SortedList<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in dbConnectionStringBuilder.Keys)
			{
				string text = (string)obj;
				object obj2 = dbConnectionStringBuilder[text];
				string text2 = null;
				if (obj2 is string)
				{
					text2 = (string)obj2;
				}
				else
				{
					if (strict)
					{
						throw new ArgumentException("connection property value is not a string", text);
					}
					if (obj2 != null)
					{
						text2 = obj2.ToString();
					}
				}
				sortedList.Add(text, text2);
			}
			return sortedList;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0004AA74 File Offset: 0x00048C74
		public override void EnlistTransaction(Transaction transaction)
		{
			this.CheckDisposed();
			bool flag2;
			int waitTimeout;
			lock (this._enlistmentSyncRoot)
			{
				flag2 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.WaitForEnlistmentReset);
				waitTimeout = this._waitTimeout;
			}
			if (flag2)
			{
				this.WaitForEnlistmentReset(waitTimeout, null);
			}
			lock (this._enlistmentSyncRoot)
			{
				if (this._enlistment == null || !(transaction == this._enlistment._scope))
				{
					if (this._enlistment != null)
					{
						throw new ArgumentException("Already enlisted in a transaction");
					}
					if (this._transactionLevel > 0 && transaction != null)
					{
						throw new ArgumentException("Unable to enlist in transaction, a local transaction already exists");
					}
					if (transaction == null)
					{
						throw new ArgumentNullException("Unable to enlist in transaction, it is null");
					}
					bool flag4 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StrictEnlistment);
					this._enlistment = new SQLiteEnlistment(this, transaction, SQLiteConnection.GetFallbackDefaultIsolationLevel(), flag4, flag4);
					SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.EnlistTransaction, null, null, null, null, null, null, new object[] { this._enlistment }));
				}
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0004ABE8 File Offset: 0x00048DE8
		public bool WaitForEnlistmentReset(int timeoutMilliseconds, bool? returnOnDisposed)
		{
			if (returnOnDisposed == null)
			{
				this.CheckDisposed();
			}
			else if (this.disposed)
			{
				return returnOnDisposed.Value;
			}
			if (timeoutMilliseconds < 0)
			{
				throw new ArgumentException("timeout cannot be negative");
			}
			int num;
			if (timeoutMilliseconds == 0)
			{
				num = 0;
			}
			else
			{
				num = Math.Min(timeoutMilliseconds / 10, 100);
				if (num == 0)
				{
					num = 100;
				}
			}
			DateTime utcNow = DateTime.UtcNow;
			for (;;)
			{
				bool flag = Monitor.TryEnter(this._enlistmentSyncRoot);
				try
				{
					if (flag && this._enlistment == null)
					{
						return true;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this._enlistmentSyncRoot);
						flag = false;
					}
				}
				if (num == 0)
				{
					break;
				}
				double totalMilliseconds = DateTime.UtcNow.Subtract(utcNow).TotalMilliseconds;
				if (totalMilliseconds < 0.0 || totalMilliseconds >= (double)timeoutMilliseconds)
				{
					return false;
				}
				Thread.Sleep(num);
			}
			return false;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0004ACF0 File Offset: 0x00048EF0
		internal static string FindKey(SortedList<string, string> items, string key, string defValue)
		{
			if (string.IsNullOrEmpty(key))
			{
				return defValue;
			}
			string text;
			if (items.TryGetValue(key, out text))
			{
				return text;
			}
			if (items.TryGetValue(key.Replace(" ", string.Empty), out text))
			{
				return text;
			}
			if (items.TryGetValue(key.Replace(" ", "_"), out text))
			{
				return text;
			}
			return defValue;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0004AD60 File Offset: 0x00048F60
		internal static object TryParseEnum(Type type, string value, bool ignoreCase)
		{
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					return Enum.Parse(type, value, ignoreCase);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0004ADA0 File Offset: 0x00048FA0
		private static bool TryParseByte(string value, NumberStyles style, out byte result)
		{
			return byte.TryParse(value, style, null, out result);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0004ADAC File Offset: 0x00048FAC
		public int SetLimitOption(SQLiteLimitOpsEnum option, int value)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for changing a limit option.");
			}
			return this._sql.SetLimitOption(option, value);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0004ADD8 File Offset: 0x00048FD8
		public void SetConfigurationOption(SQLiteConfigDbOpsEnum option, object value)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for changing a configuration option.");
			}
			if (option == SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoLoadExtension))
			{
				throw new SQLiteException("Loading extensions is disabled for this database connection.");
			}
			SQLiteErrorCode sqliteErrorCode = this._sql.SetConfigurationOption(option, value);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, null);
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0004AE50 File Offset: 0x00049050
		public void EnableExtensions(bool enable)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Database connection not valid for {0} extensions.", new object[] { enable ? "enabling" : "disabling" }));
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoLoadExtension))
			{
				throw new SQLiteException("Loading extensions is disabled for this database connection.");
			}
			this._sql.SetLoadExtension(enable);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0004AED4 File Offset: 0x000490D4
		public void LoadExtension(string fileName)
		{
			this.CheckDisposed();
			this.LoadExtension(fileName, null);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0004AEE4 File Offset: 0x000490E4
		public void LoadExtension(string fileName, string procName)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for loading extensions.");
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoLoadExtension))
			{
				throw new SQLiteException("Loading extensions is disabled for this database connection.");
			}
			this._sql.LoadExtension(fileName, procName);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0004AF40 File Offset: 0x00049140
		public void CreateModule(SQLiteModule module)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for creating modules.");
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoCreateModule))
			{
				throw new SQLiteException("Creating modules is disabled for this database connection.");
			}
			this._sql.CreateModule(module, this._flags);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0004AFA0 File Offset: 0x000491A0
		internal static byte[] FromHexString(string text)
		{
			string text2 = null;
			return SQLiteConnection.FromHexString(text, ref text2);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0004AFBC File Offset: 0x000491BC
		internal static string ToHexString(byte[] array)
		{
			if (array == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				stringBuilder.AppendFormat("{0:x2}", array[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0004B008 File Offset: 0x00049208
		private static byte[] FromHexString(string text, ref string error)
		{
			if (text == null)
			{
				error = "string is null";
				return null;
			}
			if (text.Length % 2 != 0)
			{
				error = "string contains an odd number of characters";
				return null;
			}
			byte[] array = new byte[text.Length / 2];
			for (int i = 0; i < text.Length; i += 2)
			{
				string text2 = text.Substring(i, 2);
				if (!SQLiteConnection.TryParseByte(text2, NumberStyles.HexNumber, out array[i / 2]))
				{
					error = HelperMethods.StringFormat(CultureInfo.CurrentCulture, "string contains \"{0}\", which cannot be converted to a byte value", new object[] { text2 });
					return null;
				}
			}
			return array;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0004B0A4 File Offset: 0x000492A4
		private bool GetDefaultPooling()
		{
			bool flag = false;
			if (flag)
			{
				if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoConnectionPool))
				{
					flag = false;
				}
				if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionPool))
				{
					flag = true;
				}
			}
			else
			{
				if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionPool))
				{
					flag = true;
				}
				if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoConnectionPool))
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0004B124 File Offset: 0x00049324
		private IsolationLevel GetEffectiveIsolationLevel(IsolationLevel isolationLevel)
		{
			if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.MapIsolationLevels))
			{
				return isolationLevel;
			}
			if (isolationLevel > IsolationLevel.ReadUncommitted)
			{
				if (isolationLevel <= IsolationLevel.RepeatableRead)
				{
					if (isolationLevel == IsolationLevel.ReadCommitted)
					{
						return IsolationLevel.ReadCommitted;
					}
					if (isolationLevel != IsolationLevel.RepeatableRead)
					{
						goto IL_0091;
					}
				}
				else if (isolationLevel != IsolationLevel.Serializable && isolationLevel != IsolationLevel.Snapshot)
				{
					goto IL_0091;
				}
				return IsolationLevel.Serializable;
			}
			if (isolationLevel != IsolationLevel.Unspecified && isolationLevel != IsolationLevel.Chaos && isolationLevel != IsolationLevel.ReadUncommitted)
			{
				goto IL_0091;
			}
			return IsolationLevel.ReadCommitted;
			IL_0091:
			return SQLiteConnection.GetFallbackDefaultIsolationLevel();
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0004B1CC File Offset: 0x000493CC
		public override void Open()
		{
			this.CheckDisposed();
			SQLiteConnection._lastConnectionInOpen = this;
			SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Opening, null, null, null, null, null, null, null));
			if (this._connectionState != ConnectionState.Closed)
			{
				throw new InvalidOperationException();
			}
			this.Close();
			SortedList<string, string> sortedList = SQLiteConnection.ParseConnectionString(this, this._connectionString, this._parseViaFramework, false);
			string text = SQLiteConnection.FindKey(sortedList, "Flags", null);
			object obj;
			if (text != null)
			{
				obj = SQLiteConnection.TryParseEnum(typeof(SQLiteConnectionFlags), text, true);
			}
			else
			{
				obj = null;
			}
			bool flag = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "NoDefaultFlags", false.ToString()));
			if (obj is SQLiteConnectionFlags)
			{
				this._flags |= (SQLiteConnectionFlags)obj;
			}
			else if (!flag)
			{
				this._flags |= SQLiteConnection.DefaultFlags;
			}
			if (!SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "NoSharedFlags", false.ToString())))
			{
				lock (SQLiteConnection._syncRoot)
				{
					this._flags |= SQLiteConnection._sharedFlags;
				}
			}
			bool flag3 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword);
			SortedList<string, string> sortedList2 = sortedList;
			string text2 = this._connectionString;
			if (flag3)
			{
				sortedList2 = new SortedList<string, string>(StringComparer.OrdinalIgnoreCase);
				foreach (KeyValuePair<string, string> keyValuePair in sortedList)
				{
					if (!string.Equals(keyValuePair.Key, "Password", StringComparison.OrdinalIgnoreCase) && !string.Equals(keyValuePair.Key, "HexPassword", StringComparison.OrdinalIgnoreCase) && !string.Equals(keyValuePair.Key, "TextPassword", StringComparison.OrdinalIgnoreCase))
					{
						sortedList2.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
				text2 = SQLiteConnection.BuildConnectionString(sortedList2);
			}
			SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.ConnectionString, null, null, null, null, null, text2, new object[] { sortedList2 }));
			text = SQLiteConnection.FindKey(sortedList, "DefaultDbType", null);
			if (text != null)
			{
				obj = SQLiteConnection.TryParseEnum(typeof(DbType), text, true);
				this._defaultDbType = ((obj is DbType) ? new DbType?((DbType)obj) : null);
			}
			if (this._defaultDbType != null && this._defaultDbType.Value == (DbType)(-1))
			{
				this._defaultDbType = null;
			}
			text = SQLiteConnection.FindKey(sortedList, "DefaultTypeName", null);
			if (text != null)
			{
				this._defaultTypeName = text;
			}
			text = SQLiteConnection.FindKey(sortedList, "VfsName", null);
			if (text != null)
			{
				this._vfsName = text;
			}
			bool flag4 = false;
			bool flag5 = false;
			if (Convert.ToInt32(SQLiteConnection.FindKey(sortedList, "Version", SQLiteConvert.ToString(3)), CultureInfo.InvariantCulture) != 3)
			{
				throw new NotSupportedException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Only SQLite Version {0} is supported at this time", new object[] { 3 }));
			}
			string text3 = SQLiteConnection.FindKey(sortedList, "Data Source", null);
			if (string.IsNullOrEmpty(text3))
			{
				text3 = SQLiteConnection.FindKey(sortedList, "Uri", null);
				if (string.IsNullOrEmpty(text3))
				{
					text3 = SQLiteConnection.FindKey(sortedList, "FullUri", null);
					if (string.IsNullOrEmpty(text3))
					{
						throw new ArgumentException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Data Source cannot be empty.  Use {0} to open an in-memory database", new object[] { ":memory:" }));
					}
					flag5 = true;
				}
				else
				{
					text3 = SQLiteConnection.MapUriPath(text3);
					flag4 = true;
				}
			}
			bool flag6 = string.Compare(text3, ":memory:", StringComparison.OrdinalIgnoreCase) == 0;
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.TraceWarning) && !flag4 && !flag5 && !flag6 && !string.IsNullOrEmpty(text3) && text3.StartsWith("\\", StringComparison.OrdinalIgnoreCase) && !text3.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
			{
				global::System.Diagnostics.Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "WARNING: Detected a possibly malformed UNC database file name \"{0}\" that may have originally started with two backslashes; however, four leading backslashes may be required, e.g.: \"Data Source=\\\\\\{0};\"", new object[] { text3 }));
			}
			if (!flag5)
			{
				if (flag6)
				{
					text3 = ":memory:";
				}
				else
				{
					bool flag7 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "ToFullPath", true.ToString()));
					text3 = SQLiteConnection.ExpandFileName(text3, flag7);
				}
			}
			try
			{
				bool flag8 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "Pooling", this.GetDefaultPooling().ToString()));
				int num = Convert.ToInt32(SQLiteConnection.FindKey(sortedList, "Max Pool Size", SQLiteConvert.ToString(100)), CultureInfo.InvariantCulture);
				text = SQLiteConnection.FindKey(sortedList, "Default Timeout", null);
				if (text != null)
				{
					this._defaultTimeout = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				text = SQLiteConnection.FindKey(sortedList, "BusyTimeout", null);
				if (text != null)
				{
					this._busyTimeout = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				text = SQLiteConnection.FindKey(sortedList, "WaitTimeout", null);
				if (text != null)
				{
					this._waitTimeout = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				text = SQLiteConnection.FindKey(sortedList, "PrepareRetries", null);
				if (text != null)
				{
					this._prepareRetries = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				text = SQLiteConnection.FindKey(sortedList, "ProgressOps", null);
				if (text != null)
				{
					this._progressOps = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				text = SQLiteConnection.FindKey(sortedList, "Default IsolationLevel", null);
				if (text != null)
				{
					obj = SQLiteConnection.TryParseEnum(typeof(IsolationLevel), text, true);
					this._defaultIsolation = ((obj is IsolationLevel) ? ((IsolationLevel)obj) : IsolationLevel.Serializable);
				}
				IsolationLevel effectiveIsolationLevel = this.GetEffectiveIsolationLevel(this._defaultIsolation);
				if (effectiveIsolationLevel != IsolationLevel.Serializable && effectiveIsolationLevel != IsolationLevel.ReadCommitted)
				{
					throw new NotSupportedException("Invalid Default IsolationLevel specified");
				}
				text = SQLiteConnection.FindKey(sortedList, "BaseSchemaName", null);
				if (text != null)
				{
					this._baseSchemaName = text;
				}
				if (this._sql == null)
				{
					this.SetupSQLiteBase(sortedList);
				}
				SQLiteOpenFlagsEnum sqliteOpenFlagsEnum = SQLiteOpenFlagsEnum.None;
				if (!SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "FailIfMissing", false.ToString())))
				{
					sqliteOpenFlagsEnum |= SQLiteOpenFlagsEnum.Create;
				}
				if (SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "Read Only", false.ToString())))
				{
					sqliteOpenFlagsEnum |= SQLiteOpenFlagsEnum.ReadOnly;
					sqliteOpenFlagsEnum &= ~SQLiteOpenFlagsEnum.Create;
				}
				else
				{
					sqliteOpenFlagsEnum |= SQLiteOpenFlagsEnum.ReadWrite;
				}
				if (flag5)
				{
					sqliteOpenFlagsEnum |= SQLiteOpenFlagsEnum.Uri;
				}
				this._sql.Open(text3, this._vfsName, this._flags, sqliteOpenFlagsEnum, num, flag8);
				text = SQLiteConnection.FindKey(sortedList, "BinaryGUID", null);
				if (text != null)
				{
					this._binaryGuid = SQLiteConvert.ToBoolean(text);
				}
				string text4 = SQLiteConnection.FindKey(sortedList, "TextPassword", null);
				if (text4 != null)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(text4);
					Array.Resize<byte>(ref bytes, bytes.Length + 1);
					this._sql.SetPassword(bytes, true);
					this._passwordWasText = true;
				}
				else
				{
					string text5 = SQLiteConnection.FindKey(sortedList, "HexPassword", null);
					if (text5 != null)
					{
						string text6 = null;
						byte[] array = SQLiteConnection.FromHexString(text5, ref text6);
						if (array == null)
						{
							throw new FormatException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Cannot parse 'HexPassword' property value into byte values: {0}", new object[] { text6 }));
						}
						this._sql.SetPassword(array, false);
						this._passwordWasText = false;
					}
					else
					{
						string text7 = SQLiteConnection.FindKey(sortedList, "Password", null);
						if (text7 != null)
						{
							byte[] bytes2 = Encoding.UTF8.GetBytes(text7);
							this._sql.SetPassword(bytes2, false);
							this._passwordWasText = false;
						}
						else if (this._password != null)
						{
							this._sql.SetPassword(this._password, this._passwordWasText);
						}
					}
				}
				this._password = null;
				if (flag3)
				{
					if (sortedList.ContainsKey("TextPassword"))
					{
						sortedList["TextPassword"] = string.Empty;
					}
					if (sortedList.ContainsKey("HexPassword"))
					{
						sortedList["HexPassword"] = string.Empty;
					}
					if (sortedList.ContainsKey("Password"))
					{
						sortedList["Password"] = string.Empty;
					}
					this._connectionString = SQLiteConnection.BuildConnectionString(sortedList);
				}
				if (!flag5)
				{
					this._dataSource = Path.GetFileNameWithoutExtension(text3);
				}
				else
				{
					this._dataSource = text3;
				}
				this._version++;
				ConnectionState connectionState = this._connectionState;
				this._connectionState = ConnectionState.Open;
				try
				{
					text = SQLiteConnection.FindKey(sortedList, "SetDefaults", null);
					bool flag9 = text == null || SQLiteConvert.ToBoolean(text);
					if (flag9)
					{
						using (SQLiteCommand sqliteCommand = this.CreateCommand())
						{
							if (this._busyTimeout != 0)
							{
								sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA busy_timeout={0}", new object[] { this._busyTimeout });
								sqliteCommand.ExecuteNonQuery();
							}
							if (!flag5 && !flag6)
							{
								text = SQLiteConnection.FindKey(sortedList, "Page Size", null);
								if (text != null)
								{
									int num2 = Convert.ToInt32(text, CultureInfo.InvariantCulture);
									if (num2 != 4096)
									{
										sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA page_size={0}", new object[] { num2 });
										sqliteCommand.ExecuteNonQuery();
									}
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Max Page Count", null);
							if (text != null)
							{
								int num2 = Convert.ToInt32(text, CultureInfo.InvariantCulture);
								if (num2 != 0)
								{
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA max_page_count={0}", new object[] { num2 });
									sqliteCommand.ExecuteNonQuery();
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Legacy Format", null);
							if (text != null)
							{
								flag9 = SQLiteConvert.ToBoolean(text);
								if (flag9)
								{
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA legacy_file_format={0}", new object[] { flag9 ? "ON" : "OFF" });
									sqliteCommand.ExecuteNonQuery();
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Synchronous", null);
							if (text != null)
							{
								obj = SQLiteConnection.TryParseEnum(typeof(SQLiteSynchronousEnum), text, true);
								if (!(obj is SQLiteSynchronousEnum) || (SQLiteSynchronousEnum)obj != SQLiteSynchronousEnum.Default)
								{
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA synchronous={0}", new object[] { text });
									sqliteCommand.ExecuteNonQuery();
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Cache Size", null);
							if (text != null)
							{
								int num2 = Convert.ToInt32(text, CultureInfo.InvariantCulture);
								if (num2 != -2000)
								{
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA cache_size={0}", new object[] { num2 });
									sqliteCommand.ExecuteNonQuery();
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Journal Mode", null);
							if (text != null)
							{
								obj = SQLiteConnection.TryParseEnum(typeof(SQLiteJournalModeEnum), text, true);
								if (!(obj is SQLiteJournalModeEnum) || (SQLiteJournalModeEnum)obj != SQLiteJournalModeEnum.Default)
								{
									string text8 = "PRAGMA journal_mode={0}";
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, text8, new object[] { text });
									sqliteCommand.ExecuteNonQuery();
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Foreign Keys", null);
							if (text != null)
							{
								flag9 = SQLiteConvert.ToBoolean(text);
								if (flag9)
								{
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA foreign_keys={0}", new object[] { flag9 ? "ON" : "OFF" });
									sqliteCommand.ExecuteNonQuery();
								}
							}
							text = SQLiteConnection.FindKey(sortedList, "Recursive Triggers", null);
							if (text != null)
							{
								flag9 = SQLiteConvert.ToBoolean(text);
								if (flag9)
								{
									sqliteCommand.CommandText = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA recursive_triggers={0}", new object[] { flag9 ? "ON" : "OFF" });
									sqliteCommand.ExecuteNonQuery();
								}
							}
						}
					}
					if (this._busyHandler != null)
					{
						this._sql.SetBusyHook(this._busyCallback);
					}
					if (this._progressHandler != null)
					{
						this._sql.SetProgressHook(this._progressOps, this._progressCallback);
					}
					if (this._authorizerHandler != null)
					{
						this._sql.SetAuthorizerHook(this._authorizerCallback);
					}
					if (this._commitHandler != null)
					{
						this._sql.SetCommitHook(this._commitCallback);
					}
					if (this._updateHandler != null)
					{
						this._sql.SetUpdateHook(this._updateCallback);
					}
					if (this._rollbackHandler != null)
					{
						this._sql.SetRollbackHook(this._rollbackCallback);
					}
					Transaction transaction = Transaction.Current;
					if (transaction != null && SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(sortedList, "Enlist", true.ToString())))
					{
						this.EnlistTransaction(transaction);
					}
					this._connectionState = connectionState;
					StateChangeEventArgs stateChangeEventArgs = null;
					this.OnStateChange(ConnectionState.Open, ref stateChangeEventArgs);
					SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Opened, stateChangeEventArgs, null, null, null, null, text2, new object[] { sortedList2 }));
				}
				catch
				{
					this._connectionState = connectionState;
					throw;
				}
			}
			catch (Exception)
			{
				this.Close();
				throw;
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0004BFCC File Offset: 0x0004A1CC
		public SQLiteConnection OpenAndReturn()
		{
			this.CheckDisposed();
			this.Open();
			return this;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0004BFDC File Offset: 0x0004A1DC
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x0004BFEC File Offset: 0x0004A1EC
		public int DefaultTimeout
		{
			get
			{
				this.CheckDisposed();
				return this._defaultTimeout;
			}
			set
			{
				this.CheckDisposed();
				this._defaultTimeout = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0004BFFC File Offset: 0x0004A1FC
		// (set) Token: 0x06001051 RID: 4177 RVA: 0x0004C00C File Offset: 0x0004A20C
		public int BusyTimeout
		{
			get
			{
				this.CheckDisposed();
				return this._busyTimeout;
			}
			set
			{
				this.CheckDisposed();
				this._busyTimeout = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0004C01C File Offset: 0x0004A21C
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x0004C02C File Offset: 0x0004A22C
		public int WaitTimeout
		{
			get
			{
				this.CheckDisposed();
				return this._waitTimeout;
			}
			set
			{
				this.CheckDisposed();
				this._waitTimeout = value;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0004C03C File Offset: 0x0004A23C
		// (set) Token: 0x06001055 RID: 4181 RVA: 0x0004C04C File Offset: 0x0004A24C
		public int PrepareRetries
		{
			get
			{
				this.CheckDisposed();
				return this._prepareRetries;
			}
			set
			{
				this.CheckDisposed();
				this._prepareRetries = value;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0004C05C File Offset: 0x0004A25C
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x0004C06C File Offset: 0x0004A26C
		public int ProgressOps
		{
			get
			{
				this.CheckDisposed();
				return this._progressOps;
			}
			set
			{
				this.CheckDisposed();
				this._progressOps = value;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0004C07C File Offset: 0x0004A27C
		// (set) Token: 0x06001059 RID: 4185 RVA: 0x0004C08C File Offset: 0x0004A28C
		public bool ParseViaFramework
		{
			get
			{
				this.CheckDisposed();
				return this._parseViaFramework;
			}
			set
			{
				this.CheckDisposed();
				this._parseViaFramework = value;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0004C09C File Offset: 0x0004A29C
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x0004C0AC File Offset: 0x0004A2AC
		public SQLiteConnectionFlags Flags
		{
			get
			{
				this.CheckDisposed();
				return this._flags;
			}
			set
			{
				this.CheckDisposed();
				this._flags = value;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0004C0BC File Offset: 0x0004A2BC
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x0004C0CC File Offset: 0x0004A2CC
		public DbType? DefaultDbType
		{
			get
			{
				this.CheckDisposed();
				return this._defaultDbType;
			}
			set
			{
				this.CheckDisposed();
				this._defaultDbType = value;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0004C0DC File Offset: 0x0004A2DC
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x0004C0EC File Offset: 0x0004A2EC
		public string DefaultTypeName
		{
			get
			{
				this.CheckDisposed();
				return this._defaultTypeName;
			}
			set
			{
				this.CheckDisposed();
				this._defaultTypeName = value;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0004C0FC File Offset: 0x0004A2FC
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x0004C10C File Offset: 0x0004A30C
		public string VfsName
		{
			get
			{
				this.CheckDisposed();
				return this._vfsName;
			}
			set
			{
				this.CheckDisposed();
				this._vfsName = value;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x0004C11C File Offset: 0x0004A31C
		public bool OwnHandle
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for checking handle.");
				}
				return this._sql.OwnHandle;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0004C148 File Offset: 0x0004A348
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override string ServerVersion
		{
			get
			{
				this.CheckDisposed();
				return SQLiteConnection.SQLiteVersion;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0004C158 File Offset: 0x0004A358
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long LastInsertRowId
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for getting last insert rowid.");
				}
				return this._sql.LastInsertRowId;
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0004C184 File Offset: 0x0004A384
		public void Cancel()
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for query cancellation.");
			}
			this._sql.Cancel();
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x0004C1B0 File Offset: 0x0004A3B0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Changes
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for getting number of changes.");
				}
				return this._sql.Changes;
			}
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0004C1DC File Offset: 0x0004A3DC
		public bool IsReadOnly(string name)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for checking read-only status.");
			}
			return this._sql.IsReadOnly(name);
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x0004C208 File Offset: 0x0004A408
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AutoCommit
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for getting autocommit mode.");
				}
				return this._sql.AutoCommit;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x0004C234 File Offset: 0x0004A434
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long MemoryUsed
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for getting memory used.");
				}
				return this._sql.MemoryUsed;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0004C260 File Offset: 0x0004A460
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public long MemoryHighwater
		{
			get
			{
				this.CheckDisposed();
				if (this._sql == null)
				{
					throw new InvalidOperationException("Database connection not valid for getting maximum memory used.");
				}
				return this._sql.MemoryHighwater;
			}
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0004C28C File Offset: 0x0004A48C
		public static void GetMemoryStatistics(ref IDictionary<string, long> statistics)
		{
			if (statistics == null)
			{
				statistics = new Dictionary<string, long>();
			}
			statistics["MemoryUsed"] = SQLite3.StaticMemoryUsed;
			statistics["MemoryHighwater"] = SQLite3.StaticMemoryHighwater;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0004C2C0 File Offset: 0x0004A4C0
		public void ReleaseMemory()
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for releasing memory.");
			}
			SQLiteErrorCode sqliteErrorCode = this._sql.ReleaseMemory();
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this._sql.GetLastError("Could not release connection memory."));
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0004C318 File Offset: 0x0004A518
		public static SQLiteErrorCode ReleaseMemory(int nBytes, bool reset, bool compact, ref int nFree, ref bool resetOk, ref uint nLargest)
		{
			return SQLite3.StaticReleaseMemory(nBytes, reset, compact, ref nFree, ref resetOk, ref nLargest);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0004C328 File Offset: 0x0004A528
		public static SQLiteErrorCode SetMemoryStatus(bool value)
		{
			return SQLite3.StaticSetMemoryStatus(value);
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0004C330 File Offset: 0x0004A530
		public static string DefineConstants
		{
			get
			{
				return SQLite3.DefineConstants;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0004C338 File Offset: 0x0004A538
		public static string SQLiteVersion
		{
			get
			{
				return SQLite3.SQLiteVersion;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0004C340 File Offset: 0x0004A540
		public static string SQLiteSourceId
		{
			get
			{
				return SQLite3.SQLiteSourceId;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0004C348 File Offset: 0x0004A548
		public static string SQLiteCompileOptions
		{
			get
			{
				return SQLite3.SQLiteCompileOptions;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0004C350 File Offset: 0x0004A550
		public static string InteropVersion
		{
			get
			{
				return SQLite3.InteropVersion;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0004C358 File Offset: 0x0004A558
		public static string InteropSourceId
		{
			get
			{
				return SQLite3.InteropSourceId;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x0004C360 File Offset: 0x0004A560
		public static string InteropCompileOptions
		{
			get
			{
				return SQLite3.InteropCompileOptions;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0004C368 File Offset: 0x0004A568
		public static string ProviderVersion
		{
			get
			{
				if (!(SQLiteConnection._assembly != null))
				{
					return null;
				}
				return SQLiteConnection._assembly.GetName().Version.ToString();
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x0004C390 File Offset: 0x0004A590
		public static string ProviderSourceId
		{
			get
			{
				if (SQLiteConnection._assembly == null)
				{
					return null;
				}
				string text = null;
				if (SQLiteConnection._assembly.IsDefined(typeof(AssemblySourceIdAttribute), false))
				{
					AssemblySourceIdAttribute assemblySourceIdAttribute = (AssemblySourceIdAttribute)SQLiteConnection._assembly.GetCustomAttributes(typeof(AssemblySourceIdAttribute), false)[0];
					text = assemblySourceIdAttribute.SourceId;
				}
				string text2 = null;
				if (SQLiteConnection._assembly.IsDefined(typeof(AssemblySourceTimeStampAttribute), false))
				{
					AssemblySourceTimeStampAttribute assemblySourceTimeStampAttribute = (AssemblySourceTimeStampAttribute)SQLiteConnection._assembly.GetCustomAttributes(typeof(AssemblySourceTimeStampAttribute), false)[0];
					text2 = assemblySourceTimeStampAttribute.SourceTimeStamp;
				}
				if (text != null || text2 != null)
				{
					if (text == null)
					{
						text = "0000000000000000000000000000000000000000";
					}
					if (text2 == null)
					{
						text2 = "0000-00-00 00:00:00 UTC";
					}
					return HelperMethods.StringFormat(CultureInfo.InvariantCulture, "{0} {1}", new object[] { text, text2 });
				}
				return null;
			}
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0004C47C File Offset: 0x0004A67C
		private static bool TryGetLastCachedSetting(string name, object @default, out object value)
		{
			if (SQLiteConnection._lastConnectionInOpen == null)
			{
				value = @default;
				return false;
			}
			return SQLiteConnection._lastConnectionInOpen.TryGetCachedSetting(name, @default, out value);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0004C49C File Offset: 0x0004A69C
		private static void SetLastCachedSetting(string name, object value)
		{
			if (SQLiteConnection._lastConnectionInOpen == null)
			{
				return;
			}
			SQLiteConnection._lastConnectionInOpen.SetCachedSetting(name, value);
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0004C4B8 File Offset: 0x0004A6B8
		public static SQLiteConnectionFlags DefaultFlags
		{
			get
			{
				string text = "DefaultFlags_SQLiteConnection";
				object settingValue;
				if (!SQLiteConnection.TryGetLastCachedSetting(text, null, out settingValue))
				{
					settingValue = UnsafeNativeMethods.GetSettingValue(text, null);
					SQLiteConnection.SetLastCachedSetting(text, settingValue);
				}
				if (settingValue == null)
				{
					return SQLiteConnectionFlags.Default;
				}
				object obj = SQLiteConnection.TryParseEnum(typeof(SQLiteConnectionFlags), settingValue.ToString(), true);
				if (obj is SQLiteConnectionFlags)
				{
					return (SQLiteConnectionFlags)obj;
				}
				return SQLiteConnectionFlags.Default;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x0004C530 File Offset: 0x0004A730
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x0004C578 File Offset: 0x0004A778
		public static SQLiteConnectionFlags SharedFlags
		{
			get
			{
				SQLiteConnectionFlags sharedFlags;
				lock (SQLiteConnection._syncRoot)
				{
					sharedFlags = SQLiteConnection._sharedFlags;
				}
				return sharedFlags;
			}
			set
			{
				lock (SQLiteConnection._syncRoot)
				{
					SQLiteConnection._sharedFlags = value;
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004C5C0 File Offset: 0x0004A7C0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override ConnectionState State
		{
			get
			{
				this.CheckDisposed();
				return this._connectionState;
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0004C5D0 File Offset: 0x0004A7D0
		public SQLiteErrorCode Shutdown()
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for shutdown.");
			}
			this._sql.Close(false);
			return this._sql.Shutdown();
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0004C618 File Offset: 0x0004A818
		public static void Shutdown(bool directories, bool noThrow)
		{
			SQLiteErrorCode sqliteErrorCode = SQLite3.StaticShutdown(directories);
			if (sqliteErrorCode != SQLiteErrorCode.Ok && !noThrow)
			{
				throw new SQLiteException(sqliteErrorCode, null);
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0004C644 File Offset: 0x0004A844
		public void SetExtendedResultCodes(bool bOnOff)
		{
			this.CheckDisposed();
			if (this._sql != null)
			{
				this._sql.SetExtendedResultCodes(bOnOff);
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0004C664 File Offset: 0x0004A864
		public SQLiteErrorCode ResultCode()
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for getting result code.");
			}
			return this._sql.ResultCode();
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0004C690 File Offset: 0x0004A890
		public SQLiteErrorCode ExtendedResultCode()
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for getting extended result code.");
			}
			return this._sql.ExtendedResultCode();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0004C6BC File Offset: 0x0004A8BC
		public void LogMessage(SQLiteErrorCode iErrCode, string zMessage)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for logging message.");
			}
			this._sql.LogMessage(iErrCode, zMessage);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0004C6E8 File Offset: 0x0004A8E8
		public void LogMessage(int iErrCode, string zMessage)
		{
			this.CheckDisposed();
			if (this._sql == null)
			{
				throw new InvalidOperationException("Database connection not valid for logging message.");
			}
			this._sql.LogMessage((SQLiteErrorCode)iErrCode, zMessage);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0004C714 File Offset: 0x0004A914
		public static string DecryptLegacyDatabase(string fileName, byte[] passwordBytes, int? pageSize, SQLiteProgressEventHandler progress)
		{
			return SQLite3.DecryptLegacyDatabase(fileName, passwordBytes, pageSize, progress);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0004C720 File Offset: 0x0004A920
		public void ChangePassword(string newPassword)
		{
			this.CheckDisposed();
			if (!string.IsNullOrEmpty(newPassword))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(newPassword);
				this.ChangePassword(bytes);
				return;
			}
			this.ChangePassword(null);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0004C760 File Offset: 0x0004A960
		public void ChangePassword(byte[] newPassword)
		{
			this.CheckDisposed();
			if (this._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException("Database must be opened before changing the password.");
			}
			this._sql.ChangePassword(newPassword, this._passwordWasText);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0004C794 File Offset: 0x0004A994
		public void SetPassword(string databasePassword)
		{
			this.CheckDisposed();
			if (!string.IsNullOrEmpty(databasePassword))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(databasePassword);
				this.SetPassword(bytes);
				return;
			}
			this.SetPassword(null);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0004C7D4 File Offset: 0x0004A9D4
		public void SetPassword(byte[] databasePassword)
		{
			this.CheckDisposed();
			if (this._connectionState != ConnectionState.Closed)
			{
				throw new InvalidOperationException("Password can only be set before the database is opened.");
			}
			if (databasePassword != null && databasePassword.Length == 0)
			{
				databasePassword = null;
			}
			if (databasePassword != null && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword))
			{
				throw new InvalidOperationException("With 'HidePassword' enabled, passwords can only be set via the connection string.");
			}
			this._password = databasePassword;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0004C844 File Offset: 0x0004AA44
		public SQLiteErrorCode SetAvRetry(ref int count, ref int interval)
		{
			this.CheckDisposed();
			if (this._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException("Database must be opened before changing the AV retry parameters.");
			}
			IntPtr intPtr = IntPtr.Zero;
			SQLiteErrorCode sqliteErrorCode;
			try
			{
				intPtr = Marshal.AllocHGlobal(8);
				Marshal.WriteInt32(intPtr, 0, count);
				Marshal.WriteInt32(intPtr, 4, interval);
				sqliteErrorCode = this._sql.FileControl(null, 9, intPtr);
				if (sqliteErrorCode == SQLiteErrorCode.Ok)
				{
					count = Marshal.ReadInt32(intPtr, 0);
					interval = Marshal.ReadInt32(intPtr, 4);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return sqliteErrorCode;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0004C8E4 File Offset: 0x0004AAE4
		public SQLiteErrorCode SetChunkSize(int size)
		{
			this.CheckDisposed();
			if (this._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException("Database must be opened before changing the chunk size.");
			}
			IntPtr intPtr = IntPtr.Zero;
			SQLiteErrorCode sqliteErrorCode;
			try
			{
				intPtr = Marshal.AllocHGlobal(4);
				Marshal.WriteInt32(intPtr, 0, size);
				sqliteErrorCode = this._sql.FileControl(null, 6, intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return sqliteErrorCode;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0004C960 File Offset: 0x0004AB60
		private static string UnwrapString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			int length = value.Length;
			if (value[0] == '"' && value[length - 1] == '"')
			{
				return value.Substring(1, length - 2);
			}
			if (value[0] == '\'' && value[length - 1] == '\'')
			{
				return value.Substring(1, length - 2);
			}
			return value;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0004C9D8 File Offset: 0x0004ABD8
		private static string GetDataDirectory()
		{
			string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
			if (string.IsNullOrEmpty(text))
			{
				text = AppDomain.CurrentDomain.BaseDirectory;
			}
			return text;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0004CA18 File Offset: 0x0004AC18
		private static string ExpandFileName(string sourceFile, bool toFullPath)
		{
			if (string.IsNullOrEmpty(sourceFile))
			{
				return sourceFile;
			}
			if (sourceFile.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
			{
				string dataDirectory = SQLiteConnection.GetDataDirectory();
				if (sourceFile.Length > "|DataDirectory|".Length && (sourceFile["|DataDirectory|".Length] == Path.DirectorySeparatorChar || sourceFile["|DataDirectory|".Length] == Path.AltDirectorySeparatorChar))
				{
					sourceFile = sourceFile.Remove("|DataDirectory|".Length, 1);
				}
				sourceFile = Path.Combine(dataDirectory, sourceFile.Substring("|DataDirectory|".Length));
			}
			if (toFullPath)
			{
				sourceFile = Path.GetFullPath(sourceFile);
			}
			return sourceFile;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004CAD0 File Offset: 0x0004ACD0
		public override DataTable GetSchema()
		{
			this.CheckDisposed();
			return this.GetSchema("MetaDataCollections", null);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0004CAE4 File Offset: 0x0004ACE4
		public override DataTable GetSchema(string collectionName)
		{
			this.CheckDisposed();
			return this.GetSchema(collectionName, new string[0]);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0004CAFC File Offset: 0x0004ACFC
		public override DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			this.CheckDisposed();
			if (this._connectionState != ConnectionState.Open)
			{
				throw new InvalidOperationException();
			}
			string[] array = new string[5];
			if (restrictionValues == null)
			{
				restrictionValues = new string[0];
			}
			restrictionValues.CopyTo(array, 0);
			string text;
			switch (text = collectionName.ToUpper(CultureInfo.InvariantCulture))
			{
			case "METADATACOLLECTIONS":
				return SQLiteConnection.Schema_MetaDataCollections();
			case "DATASOURCEINFORMATION":
				return this.Schema_DataSourceInformation();
			case "DATATYPES":
				return this.Schema_DataTypes();
			case "COLUMNS":
			case "TABLECOLUMNS":
				return this.Schema_Columns(array[0], array[2], array[3]);
			case "INDEXES":
				return this.Schema_Indexes(array[0], array[2], array[3]);
			case "TRIGGERS":
				return this.Schema_Triggers(array[0], array[2], array[3]);
			case "INDEXCOLUMNS":
				return this.Schema_IndexColumns(array[0], array[2], array[3], array[4]);
			case "TABLES":
				return this.Schema_Tables(array[0], array[2], array[3]);
			case "VIEWS":
				return this.Schema_Views(array[0], array[2]);
			case "VIEWCOLUMNS":
				return this.Schema_ViewColumns(array[0], array[2], array[3]);
			case "FOREIGNKEYS":
				return this.Schema_ForeignKeys(array[0], array[2], array[3]);
			case "CATALOGS":
				return this.Schema_Catalogs(array[0]);
			case "RESERVEDWORDS":
				return SQLiteConnection.Schema_ReservedWords();
			}
			throw new NotSupportedException();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0004CD80 File Offset: 0x0004AF80
		private static DataTable Schema_ReservedWords()
		{
			DataTable dataTable = new DataTable("ReservedWords");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("ReservedWord", typeof(string));
			dataTable.Columns.Add("MaximumVersion", typeof(string));
			dataTable.Columns.Add("MinimumVersion", typeof(string));
			dataTable.BeginLoadData();
			foreach (string text in SR.Keywords.Split(new char[] { ',' }))
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[0] = text;
				dataTable.Rows.Add(dataRow);
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0004CE5C File Offset: 0x0004B05C
		private static DataTable Schema_MetaDataCollections()
		{
			DataTable dataTable = new DataTable("MetaDataCollections");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("CollectionName", typeof(string));
			dataTable.Columns.Add("NumberOfRestrictions", typeof(int));
			dataTable.Columns.Add("NumberOfIdentifierParts", typeof(int));
			dataTable.BeginLoadData();
			StringReader stringReader = new StringReader(SR.MetaDataCollections);
			dataTable.ReadXml(stringReader);
			stringReader.Close();
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0004CF00 File Offset: 0x0004B100
		private DataTable Schema_DataSourceInformation()
		{
			DataTable dataTable = new DataTable("DataSourceInformation");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductName, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductVersion, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductVersionNormalized, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.GroupByBehavior, typeof(int));
			dataTable.Columns.Add(DbMetaDataColumnNames.IdentifierPattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.IdentifierCase, typeof(int));
			dataTable.Columns.Add(DbMetaDataColumnNames.OrderByColumnsInSelect, typeof(bool));
			dataTable.Columns.Add(DbMetaDataColumnNames.ParameterMarkerFormat, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.ParameterMarkerPattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.ParameterNameMaxLength, typeof(int));
			dataTable.Columns.Add(DbMetaDataColumnNames.ParameterNamePattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.QuotedIdentifierPattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.QuotedIdentifierCase, typeof(int));
			dataTable.Columns.Add(DbMetaDataColumnNames.StatementSeparatorPattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.StringLiteralPattern, typeof(string));
			dataTable.Columns.Add(DbMetaDataColumnNames.SupportedJoinOperators, typeof(int));
			dataTable.BeginLoadData();
			DataRow dataRow = dataTable.NewRow();
			dataRow.ItemArray = new object[]
			{
				null,
				"SQLite",
				this._sql.Version,
				this._sql.Version,
				3,
				"(^\\[\\p{Lo}\\p{Lu}\\p{Ll}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Nd}@$#_]*$)|(^\\[[^\\]\\0]|\\]\\]+\\]$)|(^\\\"[^\\\"\\0]|\\\"\\\"+\\\"$)",
				1,
				false,
				"{0}",
				"@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
				255,
				"^[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
				"(([^\\[]|\\]\\])*)",
				1,
				";",
				"'(([^']|'')*)'",
				15
			};
			dataTable.Rows.Add(dataRow);
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0004D1C8 File Offset: 0x0004B3C8
		private DataTable Schema_Columns(string strCatalog, string strTable, string strColumn)
		{
			DataTable dataTable = new DataTable("Columns");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_GUID", typeof(Guid));
			dataTable.Columns.Add("COLUMN_PROPID", typeof(long));
			dataTable.Columns.Add("ORDINAL_POSITION", typeof(int));
			dataTable.Columns.Add("COLUMN_HASDEFAULT", typeof(bool));
			dataTable.Columns.Add("COLUMN_DEFAULT", typeof(string));
			dataTable.Columns.Add("COLUMN_FLAGS", typeof(long));
			dataTable.Columns.Add("IS_NULLABLE", typeof(bool));
			dataTable.Columns.Add("DATA_TYPE", typeof(string));
			dataTable.Columns.Add("TYPE_GUID", typeof(Guid));
			dataTable.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof(int));
			dataTable.Columns.Add("CHARACTER_OCTET_LENGTH", typeof(int));
			dataTable.Columns.Add("NUMERIC_PRECISION", typeof(int));
			dataTable.Columns.Add("NUMERIC_SCALE", typeof(int));
			dataTable.Columns.Add("DATETIME_PRECISION", typeof(long));
			dataTable.Columns.Add("CHARACTER_SET_CATALOG", typeof(string));
			dataTable.Columns.Add("CHARACTER_SET_SCHEMA", typeof(string));
			dataTable.Columns.Add("CHARACTER_SET_NAME", typeof(string));
			dataTable.Columns.Add("COLLATION_CATALOG", typeof(string));
			dataTable.Columns.Add("COLLATION_SCHEMA", typeof(string));
			dataTable.Columns.Add("COLLATION_NAME", typeof(string));
			dataTable.Columns.Add("DOMAIN_CATALOG", typeof(string));
			dataTable.Columns.Add("DOMAIN_NAME", typeof(string));
			dataTable.Columns.Add("DESCRIPTION", typeof(string));
			dataTable.Columns.Add("PRIMARY_KEY", typeof(bool));
			dataTable.Columns.Add("EDM_TYPE", typeof(string));
			dataTable.Columns.Add("AUTOINCREMENT", typeof(bool));
			dataTable.Columns.Add("UNIQUE", typeof(bool));
			dataTable.BeginLoadData();
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table' OR [type] LIKE 'view'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						if (!string.IsNullOrEmpty(strTable))
						{
							if (string.Compare(strTable, sqliteDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) != 0)
							{
								continue;
							}
						}
						try
						{
							using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}]", new object[]
							{
								strCatalog,
								sqliteDataReader.GetString(2)
							}), this))
							{
								using (SQLiteDataReader sqliteDataReader2 = sqliteCommand2.ExecuteReader(CommandBehavior.SchemaOnly))
								{
									using (DataTable schemaTable = sqliteDataReader2.GetSchemaTable(true, true))
									{
										foreach (object obj in schemaTable.Rows)
										{
											DataRow dataRow = (DataRow)obj;
											if (string.Compare(dataRow[SchemaTableColumn.ColumnName].ToString(), strColumn, StringComparison.OrdinalIgnoreCase) == 0 || strColumn == null)
											{
												DataRow dataRow2 = dataTable.NewRow();
												dataRow2["NUMERIC_PRECISION"] = dataRow[SchemaTableColumn.NumericPrecision];
												dataRow2["NUMERIC_SCALE"] = dataRow[SchemaTableColumn.NumericScale];
												dataRow2["TABLE_NAME"] = sqliteDataReader.GetString(2);
												dataRow2["COLUMN_NAME"] = dataRow[SchemaTableColumn.ColumnName];
												dataRow2["TABLE_CATALOG"] = strCatalog;
												dataRow2["ORDINAL_POSITION"] = dataRow[SchemaTableColumn.ColumnOrdinal];
												dataRow2["COLUMN_HASDEFAULT"] = dataRow[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value;
												dataRow2["COLUMN_DEFAULT"] = dataRow[SchemaTableOptionalColumn.DefaultValue];
												dataRow2["IS_NULLABLE"] = dataRow[SchemaTableColumn.AllowDBNull];
												dataRow2["DATA_TYPE"] = dataRow["DataTypeName"].ToString().ToLower(CultureInfo.InvariantCulture);
												dataRow2["EDM_TYPE"] = SQLiteConvert.DbTypeToTypeName(this, (DbType)dataRow[SchemaTableColumn.ProviderType], this._flags).ToString().ToLower(CultureInfo.InvariantCulture);
												dataRow2["CHARACTER_MAXIMUM_LENGTH"] = dataRow[SchemaTableColumn.ColumnSize];
												dataRow2["TABLE_SCHEMA"] = dataRow[SchemaTableColumn.BaseSchemaName];
												dataRow2["PRIMARY_KEY"] = dataRow[SchemaTableColumn.IsKey];
												dataRow2["AUTOINCREMENT"] = dataRow[SchemaTableOptionalColumn.IsAutoIncrement];
												dataRow2["COLLATION_NAME"] = dataRow["CollationType"];
												dataRow2["UNIQUE"] = dataRow[SchemaTableColumn.IsUnique];
												dataTable.Rows.Add(dataRow2);
											}
										}
									}
								}
							}
						}
						catch (SQLiteException)
						{
						}
					}
				}
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0004D968 File Offset: 0x0004BB68
		private DataTable Schema_Indexes(string strCatalog, string strTable, string strIndex)
		{
			DataTable dataTable = new DataTable("Indexes");
			List<int> list = new List<int>();
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("INDEX_CATALOG", typeof(string));
			dataTable.Columns.Add("INDEX_SCHEMA", typeof(string));
			dataTable.Columns.Add("INDEX_NAME", typeof(string));
			dataTable.Columns.Add("PRIMARY_KEY", typeof(bool));
			dataTable.Columns.Add("UNIQUE", typeof(bool));
			dataTable.Columns.Add("CLUSTERED", typeof(bool));
			dataTable.Columns.Add("TYPE", typeof(int));
			dataTable.Columns.Add("FILL_FACTOR", typeof(int));
			dataTable.Columns.Add("INITIAL_SIZE", typeof(int));
			dataTable.Columns.Add("NULLS", typeof(int));
			dataTable.Columns.Add("SORT_BOOKMARKS", typeof(bool));
			dataTable.Columns.Add("AUTO_UPDATE", typeof(bool));
			dataTable.Columns.Add("NULL_COLLATION", typeof(int));
			dataTable.Columns.Add("ORDINAL_POSITION", typeof(int));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_GUID", typeof(Guid));
			dataTable.Columns.Add("COLUMN_PROPID", typeof(long));
			dataTable.Columns.Add("COLLATION", typeof(short));
			dataTable.Columns.Add("CARDINALITY", typeof(decimal));
			dataTable.Columns.Add("PAGES", typeof(int));
			dataTable.Columns.Add("FILTER_CONDITION", typeof(string));
			dataTable.Columns.Add("INTEGRATED", typeof(bool));
			dataTable.Columns.Add("INDEX_DEFINITION", typeof(string));
			dataTable.BeginLoadData();
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						bool flag = false;
						list.Clear();
						if (!string.IsNullOrEmpty(strTable))
						{
							if (string.Compare(sqliteDataReader.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) != 0)
							{
								continue;
							}
						}
						try
						{
							using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].table_info([{1}])", new object[]
							{
								strCatalog,
								sqliteDataReader.GetString(2)
							}), this))
							{
								using (SQLiteDataReader sqliteDataReader2 = sqliteCommand2.ExecuteReader())
								{
									while (sqliteDataReader2.Read())
									{
										if (sqliteDataReader2.GetInt32(5) != 0)
										{
											list.Add(sqliteDataReader2.GetInt32(0));
											if (string.Compare(sqliteDataReader2.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0)
											{
												flag = true;
											}
										}
									}
								}
							}
						}
						catch (SQLiteException)
						{
						}
						if (list.Count == 1 && flag)
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["TABLE_CATALOG"] = strCatalog;
							dataRow["TABLE_NAME"] = sqliteDataReader.GetString(2);
							dataRow["INDEX_CATALOG"] = strCatalog;
							dataRow["PRIMARY_KEY"] = true;
							dataRow["INDEX_NAME"] = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "{1}_PK_{0}", new object[]
							{
								sqliteDataReader.GetString(2),
								masterTableName
							});
							dataRow["UNIQUE"] = true;
							if (string.Compare((string)dataRow["INDEX_NAME"], strIndex, StringComparison.OrdinalIgnoreCase) == 0 || strIndex == null)
							{
								dataTable.Rows.Add(dataRow);
							}
							list.Clear();
						}
						try
						{
							using (SQLiteCommand sqliteCommand3 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].index_list([{1}])", new object[]
							{
								strCatalog,
								sqliteDataReader.GetString(2)
							}), this))
							{
								using (SQLiteDataReader sqliteDataReader3 = sqliteCommand3.ExecuteReader())
								{
									while (sqliteDataReader3.Read())
									{
										if (string.Compare(sqliteDataReader3.GetString(1), strIndex, StringComparison.OrdinalIgnoreCase) == 0 || strIndex == null)
										{
											DataRow dataRow = dataTable.NewRow();
											dataRow["TABLE_CATALOG"] = strCatalog;
											dataRow["TABLE_NAME"] = sqliteDataReader.GetString(2);
											dataRow["INDEX_CATALOG"] = strCatalog;
											dataRow["INDEX_NAME"] = sqliteDataReader3.GetString(1);
											dataRow["UNIQUE"] = SQLiteConvert.ToBoolean(sqliteDataReader3.GetValue(2), CultureInfo.InvariantCulture, false);
											dataRow["PRIMARY_KEY"] = false;
											using (SQLiteCommand sqliteCommand4 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{2}] WHERE [type] LIKE 'index' AND [name] LIKE '{1}'", new object[]
											{
												strCatalog,
												sqliteDataReader3.GetString(1).Replace("'", "''"),
												masterTableName
											}), this))
											{
												using (SQLiteDataReader sqliteDataReader4 = sqliteCommand4.ExecuteReader())
												{
													if (sqliteDataReader4.Read() && !sqliteDataReader4.IsDBNull(4))
													{
														dataRow["INDEX_DEFINITION"] = sqliteDataReader4.GetString(4);
													}
												}
											}
											if (list.Count > 0 && sqliteDataReader3.GetString(1).StartsWith("sqlite_autoindex_" + sqliteDataReader.GetString(2), StringComparison.InvariantCultureIgnoreCase))
											{
												using (SQLiteCommand sqliteCommand5 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].index_info([{1}])", new object[]
												{
													strCatalog,
													sqliteDataReader3.GetString(1)
												}), this))
												{
													using (SQLiteDataReader sqliteDataReader5 = sqliteCommand5.ExecuteReader())
													{
														int num = 0;
														while (sqliteDataReader5.Read())
														{
															if (!list.Contains(sqliteDataReader5.GetInt32(1)))
															{
																num = 0;
																break;
															}
															num++;
														}
														if (num == list.Count)
														{
															dataRow["PRIMARY_KEY"] = true;
															list.Clear();
														}
													}
												}
											}
											dataTable.Rows.Add(dataRow);
										}
									}
								}
							}
						}
						catch (SQLiteException)
						{
						}
					}
				}
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0004E27C File Offset: 0x0004C47C
		private DataTable Schema_Triggers(string catalog, string table, string triggerName)
		{
			DataTable dataTable = new DataTable("Triggers");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("TRIGGER_NAME", typeof(string));
			dataTable.Columns.Add("TRIGGER_DEFINITION", typeof(string));
			dataTable.BeginLoadData();
			if (string.IsNullOrEmpty(table))
			{
				table = null;
			}
			if (string.IsNullOrEmpty(catalog))
			{
				catalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(catalog));
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{0}].[{1}] WHERE [type] LIKE 'trigger'", new object[] { catalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						if ((string.Compare(sqliteDataReader.GetString(1), triggerName, StringComparison.OrdinalIgnoreCase) == 0 || triggerName == null) && (table == null || string.Compare(table, sqliteDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) == 0))
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["TABLE_CATALOG"] = catalog;
							dataRow["TABLE_NAME"] = sqliteDataReader.GetString(2);
							dataRow["TRIGGER_NAME"] = sqliteDataReader.GetString(1);
							dataRow["TRIGGER_DEFINITION"] = sqliteDataReader.GetString(4);
							dataTable.Rows.Add(dataRow);
						}
					}
				}
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0004E478 File Offset: 0x0004C678
		private DataTable Schema_Tables(string strCatalog, string strTable, string strType)
		{
			DataTable dataTable = new DataTable("Tables");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("TABLE_TYPE", typeof(string));
			dataTable.Columns.Add("TABLE_ID", typeof(long));
			dataTable.Columns.Add("TABLE_ROOTPAGE", typeof(int));
			dataTable.Columns.Add("TABLE_DEFINITION", typeof(string));
			dataTable.BeginLoadData();
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{0}].[{1}] WHERE [type] LIKE 'table'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						string text = sqliteDataReader.GetString(0);
						if (string.Compare(sqliteDataReader.GetString(2), 0, "SQLITE_", 0, 7, StringComparison.OrdinalIgnoreCase) == 0)
						{
							text = "SYSTEM_TABLE";
						}
						if ((string.Compare(strType, text, StringComparison.OrdinalIgnoreCase) == 0 || strType == null) && (string.Compare(sqliteDataReader.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) == 0 || strTable == null))
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["TABLE_CATALOG"] = strCatalog;
							dataRow["TABLE_NAME"] = sqliteDataReader.GetString(2);
							dataRow["TABLE_TYPE"] = text;
							dataRow["TABLE_ID"] = sqliteDataReader.GetInt64(5);
							dataRow["TABLE_ROOTPAGE"] = sqliteDataReader.GetInt32(3);
							dataRow["TABLE_DEFINITION"] = sqliteDataReader.GetString(4);
							dataTable.Rows.Add(dataRow);
						}
					}
				}
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0004E708 File Offset: 0x0004C908
		private DataTable Schema_Views(string strCatalog, string strView)
		{
			DataTable dataTable = new DataTable("Views");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("VIEW_DEFINITION", typeof(string));
			dataTable.Columns.Add("CHECK_OPTION", typeof(bool));
			dataTable.Columns.Add("IS_UPDATABLE", typeof(bool));
			dataTable.Columns.Add("DESCRIPTION", typeof(string));
			dataTable.Columns.Add("DATE_CREATED", typeof(DateTime));
			dataTable.Columns.Add("DATE_MODIFIED", typeof(DateTime));
			dataTable.BeginLoadData();
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'view'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						if (string.Compare(sqliteDataReader.GetString(1), strView, StringComparison.OrdinalIgnoreCase) == 0 || string.IsNullOrEmpty(strView))
						{
							string text = sqliteDataReader.GetString(4).Replace('\r', ' ').Replace('\n', ' ')
								.Replace('\t', ' ');
							int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(text, " AS ", CompareOptions.IgnoreCase);
							if (num > -1)
							{
								text = text.Substring(num + 4).Trim();
								DataRow dataRow = dataTable.NewRow();
								dataRow["TABLE_CATALOG"] = strCatalog;
								dataRow["TABLE_NAME"] = sqliteDataReader.GetString(2);
								dataRow["IS_UPDATABLE"] = false;
								dataRow["VIEW_DEFINITION"] = text;
								dataTable.Rows.Add(dataRow);
							}
						}
					}
				}
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0004E9A0 File Offset: 0x0004CBA0
		private DataTable Schema_Catalogs(string strCatalog)
		{
			DataTable dataTable = new DataTable("Catalogs");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("CATALOG_NAME", typeof(string));
			dataTable.Columns.Add("DESCRIPTION", typeof(string));
			dataTable.Columns.Add("ID", typeof(long));
			dataTable.BeginLoadData();
			using (SQLiteCommand sqliteCommand = new SQLiteCommand("PRAGMA database_list", this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						if (string.Compare(sqliteDataReader.GetString(1), strCatalog, StringComparison.OrdinalIgnoreCase) == 0 || strCatalog == null)
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["CATALOG_NAME"] = sqliteDataReader.GetString(1);
							dataRow["DESCRIPTION"] = sqliteDataReader.GetString(2);
							dataRow["ID"] = sqliteDataReader.GetInt64(0);
							dataTable.Rows.Add(dataRow);
						}
					}
				}
			}
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0004EAF0 File Offset: 0x0004CCF0
		private DataTable Schema_DataTypes()
		{
			DataTable dataTable = new DataTable("DataTypes");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("TypeName", typeof(string));
			dataTable.Columns.Add("ProviderDbType", typeof(int));
			dataTable.Columns.Add("ColumnSize", typeof(long));
			dataTable.Columns.Add("CreateFormat", typeof(string));
			dataTable.Columns.Add("CreateParameters", typeof(string));
			dataTable.Columns.Add("DataType", typeof(string));
			dataTable.Columns.Add("IsAutoIncrementable", typeof(bool));
			dataTable.Columns.Add("IsBestMatch", typeof(bool));
			dataTable.Columns.Add("IsCaseSensitive", typeof(bool));
			dataTable.Columns.Add("IsFixedLength", typeof(bool));
			dataTable.Columns.Add("IsFixedPrecisionScale", typeof(bool));
			dataTable.Columns.Add("IsLong", typeof(bool));
			dataTable.Columns.Add("IsNullable", typeof(bool));
			dataTable.Columns.Add("IsSearchable", typeof(bool));
			dataTable.Columns.Add("IsSearchableWithLike", typeof(bool));
			dataTable.Columns.Add("IsLiteralSupported", typeof(bool));
			dataTable.Columns.Add("LiteralPrefix", typeof(string));
			dataTable.Columns.Add("LiteralSuffix", typeof(string));
			dataTable.Columns.Add("IsUnsigned", typeof(bool));
			dataTable.Columns.Add("MaximumScale", typeof(short));
			dataTable.Columns.Add("MinimumScale", typeof(short));
			dataTable.Columns.Add("IsConcurrencyType", typeof(bool));
			dataTable.BeginLoadData();
			StringReader stringReader = new StringReader(SR.DataTypes);
			dataTable.ReadXml(stringReader);
			stringReader.Close();
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0004ED98 File Offset: 0x0004CF98
		private DataTable Schema_IndexColumns(string strCatalog, string strTable, string strIndex, string strColumn)
		{
			DataTable dataTable = new DataTable("IndexColumns");
			List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("CONSTRAINT_CATALOG", typeof(string));
			dataTable.Columns.Add("CONSTRAINT_SCHEMA", typeof(string));
			dataTable.Columns.Add("CONSTRAINT_NAME", typeof(string));
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("ORDINAL_POSITION", typeof(int));
			dataTable.Columns.Add("INDEX_NAME", typeof(string));
			dataTable.Columns.Add("COLLATION_NAME", typeof(string));
			dataTable.Columns.Add("SORT_MODE", typeof(string));
			dataTable.Columns.Add("CONFLICT_OPTION", typeof(int));
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			dataTable.BeginLoadData();
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						bool flag = false;
						list.Clear();
						if (!string.IsNullOrEmpty(strTable))
						{
							if (string.Compare(sqliteDataReader.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) != 0)
							{
								continue;
							}
						}
						try
						{
							using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].table_info([{1}])", new object[]
							{
								strCatalog,
								sqliteDataReader.GetString(2)
							}), this))
							{
								using (SQLiteDataReader sqliteDataReader2 = sqliteCommand2.ExecuteReader())
								{
									while (sqliteDataReader2.Read())
									{
										if (sqliteDataReader2.GetInt32(5) == 1)
										{
											list.Add(new KeyValuePair<int, string>(sqliteDataReader2.GetInt32(0), sqliteDataReader2.GetString(1)));
											if (string.Compare(sqliteDataReader2.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0)
											{
												flag = true;
											}
										}
									}
								}
							}
						}
						catch (SQLiteException)
						{
						}
						if (list.Count == 1 && flag)
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["CONSTRAINT_CATALOG"] = strCatalog;
							dataRow["CONSTRAINT_NAME"] = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "{1}_PK_{0}", new object[]
							{
								sqliteDataReader.GetString(2),
								masterTableName
							});
							dataRow["TABLE_CATALOG"] = strCatalog;
							dataRow["TABLE_NAME"] = sqliteDataReader.GetString(2);
							dataRow["COLUMN_NAME"] = list[0].Value;
							dataRow["INDEX_NAME"] = dataRow["CONSTRAINT_NAME"];
							dataRow["ORDINAL_POSITION"] = 0;
							dataRow["COLLATION_NAME"] = "BINARY";
							dataRow["SORT_MODE"] = "ASC";
							dataRow["CONFLICT_OPTION"] = 2;
							if (string.IsNullOrEmpty(strIndex) || string.Compare(strIndex, (string)dataRow["INDEX_NAME"], StringComparison.OrdinalIgnoreCase) == 0)
							{
								dataTable.Rows.Add(dataRow);
							}
						}
						using (SQLiteCommand sqliteCommand3 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{2}] WHERE [type] LIKE 'index' AND [tbl_name] LIKE '{1}'", new object[]
						{
							strCatalog,
							sqliteDataReader.GetString(2).Replace("'", "''"),
							masterTableName
						}), this))
						{
							using (SQLiteDataReader sqliteDataReader3 = sqliteCommand3.ExecuteReader())
							{
								while (sqliteDataReader3.Read())
								{
									int num = 0;
									if (!string.IsNullOrEmpty(strIndex))
									{
										if (string.Compare(strIndex, sqliteDataReader3.GetString(1), StringComparison.OrdinalIgnoreCase) != 0)
										{
											continue;
										}
									}
									try
									{
										using (SQLiteCommand sqliteCommand4 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].index_info([{1}])", new object[]
										{
											strCatalog,
											sqliteDataReader3.GetString(1)
										}), this))
										{
											using (SQLiteDataReader sqliteDataReader4 = sqliteCommand4.ExecuteReader())
											{
												while (sqliteDataReader4.Read())
												{
													string text = (sqliteDataReader4.IsDBNull(2) ? null : sqliteDataReader4.GetString(2));
													DataRow dataRow = dataTable.NewRow();
													dataRow["CONSTRAINT_CATALOG"] = strCatalog;
													dataRow["CONSTRAINT_NAME"] = sqliteDataReader3.GetString(1);
													dataRow["TABLE_CATALOG"] = strCatalog;
													dataRow["TABLE_NAME"] = sqliteDataReader3.GetString(2);
													dataRow["COLUMN_NAME"] = text;
													dataRow["INDEX_NAME"] = sqliteDataReader3.GetString(1);
													dataRow["ORDINAL_POSITION"] = num;
													string text2 = null;
													int num2 = 0;
													int num3 = 0;
													if (text != null)
													{
														this._sql.GetIndexColumnExtendedInfo(strCatalog, sqliteDataReader3.GetString(1), text, ref num2, ref num3, ref text2);
													}
													if (!string.IsNullOrEmpty(text2))
													{
														dataRow["COLLATION_NAME"] = text2;
													}
													dataRow["SORT_MODE"] = ((num2 == 0) ? "ASC" : "DESC");
													dataRow["CONFLICT_OPTION"] = num3;
													num++;
													if (strColumn == null || string.Compare(strColumn, text, StringComparison.OrdinalIgnoreCase) == 0)
													{
														dataTable.Rows.Add(dataRow);
													}
												}
											}
										}
									}
									catch (SQLiteException)
									{
									}
								}
							}
						}
					}
				}
			}
			dataTable.EndLoadData();
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0004F514 File Offset: 0x0004D714
		private DataTable Schema_ViewColumns(string strCatalog, string strView, string strColumn)
		{
			DataTable dataTable = new DataTable("ViewColumns");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("VIEW_CATALOG", typeof(string));
			dataTable.Columns.Add("VIEW_SCHEMA", typeof(string));
			dataTable.Columns.Add("VIEW_NAME", typeof(string));
			dataTable.Columns.Add("VIEW_COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("ORDINAL_POSITION", typeof(int));
			dataTable.Columns.Add("COLUMN_HASDEFAULT", typeof(bool));
			dataTable.Columns.Add("COLUMN_DEFAULT", typeof(string));
			dataTable.Columns.Add("COLUMN_FLAGS", typeof(long));
			dataTable.Columns.Add("IS_NULLABLE", typeof(bool));
			dataTable.Columns.Add("DATA_TYPE", typeof(string));
			dataTable.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof(int));
			dataTable.Columns.Add("NUMERIC_PRECISION", typeof(int));
			dataTable.Columns.Add("NUMERIC_SCALE", typeof(int));
			dataTable.Columns.Add("DATETIME_PRECISION", typeof(long));
			dataTable.Columns.Add("CHARACTER_SET_CATALOG", typeof(string));
			dataTable.Columns.Add("CHARACTER_SET_SCHEMA", typeof(string));
			dataTable.Columns.Add("CHARACTER_SET_NAME", typeof(string));
			dataTable.Columns.Add("COLLATION_CATALOG", typeof(string));
			dataTable.Columns.Add("COLLATION_SCHEMA", typeof(string));
			dataTable.Columns.Add("COLLATION_NAME", typeof(string));
			dataTable.Columns.Add("PRIMARY_KEY", typeof(bool));
			dataTable.Columns.Add("EDM_TYPE", typeof(string));
			dataTable.Columns.Add("AUTOINCREMENT", typeof(bool));
			dataTable.Columns.Add("UNIQUE", typeof(bool));
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			dataTable.BeginLoadData();
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'view'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						if (string.IsNullOrEmpty(strView) || string.Compare(strView, sqliteDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) == 0)
						{
							using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}]", new object[]
							{
								strCatalog,
								sqliteDataReader.GetString(2)
							}), this))
							{
								string text = sqliteDataReader.GetString(4).Replace('\r', ' ').Replace('\n', ' ')
									.Replace('\t', ' ');
								int i = CultureInfo.InvariantCulture.CompareInfo.IndexOf(text, " AS ", CompareOptions.IgnoreCase);
								if (i >= 0)
								{
									text = text.Substring(i + 4);
									using (SQLiteCommand sqliteCommand3 = new SQLiteCommand(text, this))
									{
										using (SQLiteDataReader sqliteDataReader2 = sqliteCommand2.ExecuteReader(CommandBehavior.SchemaOnly))
										{
											using (SQLiteDataReader sqliteDataReader3 = sqliteCommand3.ExecuteReader(CommandBehavior.SchemaOnly))
											{
												using (DataTable schemaTable = sqliteDataReader2.GetSchemaTable(false, false))
												{
													using (DataTable schemaTable2 = sqliteDataReader3.GetSchemaTable(false, false))
													{
														for (i = 0; i < schemaTable2.Rows.Count; i++)
														{
															DataRow dataRow = schemaTable.Rows[i];
															DataRow dataRow2 = schemaTable2.Rows[i];
															if (string.Compare(dataRow[SchemaTableColumn.ColumnName].ToString(), strColumn, StringComparison.OrdinalIgnoreCase) == 0 || strColumn == null)
															{
																DataRow dataRow3 = dataTable.NewRow();
																dataRow3["VIEW_CATALOG"] = strCatalog;
																dataRow3["VIEW_NAME"] = sqliteDataReader.GetString(2);
																dataRow3["TABLE_CATALOG"] = strCatalog;
																dataRow3["TABLE_SCHEMA"] = dataRow2[SchemaTableColumn.BaseSchemaName];
																dataRow3["TABLE_NAME"] = dataRow2[SchemaTableColumn.BaseTableName];
																dataRow3["COLUMN_NAME"] = dataRow2[SchemaTableColumn.BaseColumnName];
																dataRow3["VIEW_COLUMN_NAME"] = dataRow[SchemaTableColumn.ColumnName];
																dataRow3["COLUMN_HASDEFAULT"] = dataRow[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value;
																dataRow3["COLUMN_DEFAULT"] = dataRow[SchemaTableOptionalColumn.DefaultValue];
																dataRow3["ORDINAL_POSITION"] = dataRow[SchemaTableColumn.ColumnOrdinal];
																dataRow3["IS_NULLABLE"] = dataRow[SchemaTableColumn.AllowDBNull];
																dataRow3["DATA_TYPE"] = dataRow["DataTypeName"];
																dataRow3["EDM_TYPE"] = SQLiteConvert.DbTypeToTypeName(this, (DbType)dataRow[SchemaTableColumn.ProviderType], this._flags).ToString().ToLower(CultureInfo.InvariantCulture);
																dataRow3["CHARACTER_MAXIMUM_LENGTH"] = dataRow[SchemaTableColumn.ColumnSize];
																dataRow3["TABLE_SCHEMA"] = dataRow[SchemaTableColumn.BaseSchemaName];
																dataRow3["PRIMARY_KEY"] = dataRow[SchemaTableColumn.IsKey];
																dataRow3["AUTOINCREMENT"] = dataRow[SchemaTableOptionalColumn.IsAutoIncrement];
																dataRow3["COLLATION_NAME"] = dataRow["CollationType"];
																dataRow3["UNIQUE"] = dataRow[SchemaTableColumn.IsUnique];
																dataTable.Rows.Add(dataRow3);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			dataTable.EndLoadData();
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0004FD30 File Offset: 0x0004DF30
		private DataTable Schema_ForeignKeys(string strCatalog, string strTable, string strKeyName)
		{
			DataTable dataTable = new DataTable("ForeignKeys");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("CONSTRAINT_CATALOG", typeof(string));
			dataTable.Columns.Add("CONSTRAINT_SCHEMA", typeof(string));
			dataTable.Columns.Add("CONSTRAINT_NAME", typeof(string));
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("CONSTRAINT_TYPE", typeof(string));
			dataTable.Columns.Add("IS_DEFERRABLE", typeof(bool));
			dataTable.Columns.Add("INITIALLY_DEFERRED", typeof(bool));
			dataTable.Columns.Add("FKEY_ID", typeof(int));
			dataTable.Columns.Add("FKEY_FROM_COLUMN", typeof(string));
			dataTable.Columns.Add("FKEY_FROM_ORDINAL_POSITION", typeof(int));
			dataTable.Columns.Add("FKEY_TO_CATALOG", typeof(string));
			dataTable.Columns.Add("FKEY_TO_SCHEMA", typeof(string));
			dataTable.Columns.Add("FKEY_TO_TABLE", typeof(string));
			dataTable.Columns.Add("FKEY_TO_COLUMN", typeof(string));
			dataTable.Columns.Add("FKEY_ON_UPDATE", typeof(string));
			dataTable.Columns.Add("FKEY_ON_DELETE", typeof(string));
			dataTable.Columns.Add("FKEY_MATCH", typeof(string));
			if (string.IsNullOrEmpty(strCatalog))
			{
				strCatalog = SQLiteConnection.GetDefaultCatalogName();
			}
			string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
			dataTable.BeginLoadData();
			using (SQLiteCommand sqliteCommand = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", new object[] { strCatalog, masterTableName }), this))
			{
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						if (!string.IsNullOrEmpty(strTable))
						{
							if (string.Compare(strTable, sqliteDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) != 0)
							{
								continue;
							}
						}
						try
						{
							using (SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder())
							{
								using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "PRAGMA [{0}].foreign_key_list([{1}])", new object[]
								{
									strCatalog,
									sqliteDataReader.GetString(2)
								}), this))
								{
									using (SQLiteDataReader sqliteDataReader2 = sqliteCommand2.ExecuteReader())
									{
										while (sqliteDataReader2.Read())
										{
											DataRow dataRow = dataTable.NewRow();
											dataRow["CONSTRAINT_CATALOG"] = strCatalog;
											dataRow["CONSTRAINT_NAME"] = HelperMethods.StringFormat(CultureInfo.InvariantCulture, "FK_{0}_{1}_{2}", new object[]
											{
												sqliteDataReader[2],
												sqliteDataReader2.GetInt32(0),
												sqliteDataReader2.GetInt32(1)
											});
											dataRow["TABLE_CATALOG"] = strCatalog;
											dataRow["TABLE_NAME"] = sqliteCommandBuilder.UnquoteIdentifier(sqliteDataReader.GetString(2));
											dataRow["CONSTRAINT_TYPE"] = "FOREIGN KEY";
											dataRow["IS_DEFERRABLE"] = false;
											dataRow["INITIALLY_DEFERRED"] = false;
											dataRow["FKEY_ID"] = sqliteDataReader2[0];
											dataRow["FKEY_FROM_COLUMN"] = sqliteCommandBuilder.UnquoteIdentifier(sqliteDataReader2[3].ToString());
											dataRow["FKEY_TO_CATALOG"] = strCatalog;
											dataRow["FKEY_TO_TABLE"] = sqliteCommandBuilder.UnquoteIdentifier(sqliteDataReader2[2].ToString());
											dataRow["FKEY_TO_COLUMN"] = sqliteCommandBuilder.UnquoteIdentifier(sqliteDataReader2[4].ToString());
											dataRow["FKEY_FROM_ORDINAL_POSITION"] = sqliteDataReader2[1];
											dataRow["FKEY_ON_UPDATE"] = ((sqliteDataReader2.FieldCount > 5) ? sqliteDataReader2[5] : string.Empty);
											dataRow["FKEY_ON_DELETE"] = ((sqliteDataReader2.FieldCount > 6) ? sqliteDataReader2[6] : string.Empty);
											dataRow["FKEY_MATCH"] = ((sqliteDataReader2.FieldCount > 7) ? sqliteDataReader2[7] : string.Empty);
											if (string.IsNullOrEmpty(strKeyName) || string.Compare(strKeyName, dataRow["CONSTRAINT_NAME"].ToString(), StringComparison.OrdinalIgnoreCase) == 0)
											{
												dataTable.Rows.Add(dataRow);
											}
										}
									}
								}
							}
						}
						catch (SQLiteException)
						{
						}
					}
				}
			}
			dataTable.EndLoadData();
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600109F RID: 4255 RVA: 0x00050348 File Offset: 0x0004E548
		// (remove) Token: 0x060010A0 RID: 4256 RVA: 0x000503A0 File Offset: 0x0004E5A0
		public event SQLiteBusyEventHandler Busy
		{
			add
			{
				this.CheckDisposed();
				if (this._busyHandler == null)
				{
					this._busyCallback = new SQLiteBusyCallback(this.BusyCallback);
					if (this._sql != null)
					{
						this._sql.SetBusyHook(this._busyCallback);
					}
				}
				this._busyHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._busyHandler -= value;
				if (this._busyHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetBusyHook(null);
					}
					this._busyCallback = null;
				}
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060010A1 RID: 4257 RVA: 0x000503D8 File Offset: 0x0004E5D8
		// (remove) Token: 0x060010A2 RID: 4258 RVA: 0x00050438 File Offset: 0x0004E638
		public event SQLiteProgressEventHandler Progress
		{
			add
			{
				this.CheckDisposed();
				if (this._progressHandler == null)
				{
					this._progressCallback = new SQLiteProgressCallback(this.ProgressCallback);
					if (this._sql != null)
					{
						this._sql.SetProgressHook(this._progressOps, this._progressCallback);
					}
				}
				this._progressHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._progressHandler -= value;
				if (this._progressHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetProgressHook(0, null);
					}
					this._progressCallback = null;
				}
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060010A3 RID: 4259 RVA: 0x00050474 File Offset: 0x0004E674
		// (remove) Token: 0x060010A4 RID: 4260 RVA: 0x000504CC File Offset: 0x0004E6CC
		public event SQLiteAuthorizerEventHandler Authorize
		{
			add
			{
				this.CheckDisposed();
				if (this._authorizerHandler == null)
				{
					this._authorizerCallback = new SQLiteAuthorizerCallback(this.AuthorizerCallback);
					if (this._sql != null)
					{
						this._sql.SetAuthorizerHook(this._authorizerCallback);
					}
				}
				this._authorizerHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._authorizerHandler -= value;
				if (this._authorizerHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetAuthorizerHook(null);
					}
					this._authorizerCallback = null;
				}
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060010A5 RID: 4261 RVA: 0x00050504 File Offset: 0x0004E704
		// (remove) Token: 0x060010A6 RID: 4262 RVA: 0x0005055C File Offset: 0x0004E75C
		public event SQLiteUpdateEventHandler Update
		{
			add
			{
				this.CheckDisposed();
				if (this._updateHandler == null)
				{
					this._updateCallback = new SQLiteUpdateCallback(this.UpdateCallback);
					if (this._sql != null)
					{
						this._sql.SetUpdateHook(this._updateCallback);
					}
				}
				this._updateHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._updateHandler -= value;
				if (this._updateHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetUpdateHook(null);
					}
					this._updateCallback = null;
				}
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00050594 File Offset: 0x0004E794
		private SQLiteBusyReturnCode BusyCallback(IntPtr pUserData, int count)
		{
			try
			{
				BusyEventArgs busyEventArgs = new BusyEventArgs(pUserData, count, SQLiteBusyReturnCode.Retry);
				if (this._busyHandler != null)
				{
					this._busyHandler(this, busyEventArgs);
				}
				return busyEventArgs.ReturnCode;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Busy", ex }));
					}
				}
				catch
				{
				}
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StopOnException))
			{
				return SQLiteBusyReturnCode.Stop;
			}
			return SQLiteBusyReturnCode.Retry;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00050658 File Offset: 0x0004E858
		private SQLiteProgressReturnCode ProgressCallback(IntPtr pUserData)
		{
			try
			{
				ProgressEventArgs progressEventArgs = new ProgressEventArgs(pUserData, SQLiteProgressReturnCode.Continue);
				if (this._progressHandler != null)
				{
					this._progressHandler(this, progressEventArgs);
				}
				return progressEventArgs.ReturnCode;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Progress", ex }));
					}
				}
				catch
				{
				}
			}
			if (HelperMethods.HasFlags(this._flags, (SQLiteConnectionFlags)((ulong)(-2147483648))))
			{
				return SQLiteProgressReturnCode.Interrupt;
			}
			return SQLiteProgressReturnCode.Continue;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00050718 File Offset: 0x0004E918
		private SQLiteAuthorizerReturnCode AuthorizerCallback(IntPtr pUserData, SQLiteAuthorizerActionCode actionCode, IntPtr pArgument1, IntPtr pArgument2, IntPtr pDatabase, IntPtr pAuthContext)
		{
			try
			{
				AuthorizerEventArgs authorizerEventArgs = new AuthorizerEventArgs(pUserData, actionCode, SQLiteConvert.UTF8ToString(pArgument1, -1), SQLiteConvert.UTF8ToString(pArgument2, -1), SQLiteConvert.UTF8ToString(pDatabase, -1), SQLiteConvert.UTF8ToString(pAuthContext, -1), SQLiteAuthorizerReturnCode.Ok);
				if (this._authorizerHandler != null)
				{
					this._authorizerHandler(this, authorizerEventArgs);
				}
				return authorizerEventArgs.ReturnCode;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Authorize", ex }));
					}
				}
				catch
				{
				}
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.DenyOnException))
			{
				return SQLiteAuthorizerReturnCode.Deny;
			}
			return SQLiteAuthorizerReturnCode.Ok;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x000507F8 File Offset: 0x0004E9F8
		private void UpdateCallback(IntPtr puser, int type, IntPtr database, IntPtr table, long rowid)
		{
			try
			{
				this._updateHandler(this, new UpdateEventArgs(SQLiteConvert.UTF8ToString(database, -1), SQLiteConvert.UTF8ToString(table, -1), (UpdateEventType)type, rowid));
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Update", ex }));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060010AB RID: 4267 RVA: 0x00050898 File Offset: 0x0004EA98
		// (remove) Token: 0x060010AC RID: 4268 RVA: 0x000508F0 File Offset: 0x0004EAF0
		public event SQLiteCommitHandler Commit
		{
			add
			{
				this.CheckDisposed();
				if (this._commitHandler == null)
				{
					this._commitCallback = new SQLiteCommitCallback(this.CommitCallback);
					if (this._sql != null)
					{
						this._sql.SetCommitHook(this._commitCallback);
					}
				}
				this._commitHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._commitHandler -= value;
				if (this._commitHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetCommitHook(null);
					}
					this._commitCallback = null;
				}
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060010AD RID: 4269 RVA: 0x00050928 File Offset: 0x0004EB28
		// (remove) Token: 0x060010AE RID: 4270 RVA: 0x00050980 File Offset: 0x0004EB80
		public event SQLiteTraceEventHandler Trace
		{
			add
			{
				this.CheckDisposed();
				if (this._traceHandler == null)
				{
					this._traceCallback = new SQLiteTraceCallback(this.TraceCallback);
					if (this._sql != null)
					{
						this._sql.SetTraceCallback(this._traceCallback);
					}
				}
				this._traceHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._traceHandler -= value;
				if (this._traceHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetTraceCallback(null);
					}
					this._traceCallback = null;
				}
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x000509B8 File Offset: 0x0004EBB8
		private void TraceCallback(IntPtr puser, IntPtr statement)
		{
			try
			{
				if (this._traceHandler != null)
				{
					this._traceHandler(this, new TraceEventArgs(SQLiteConvert.UTF8ToString(statement, -1)));
				}
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Trace", ex }));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060010B0 RID: 4272 RVA: 0x00050A58 File Offset: 0x0004EC58
		// (remove) Token: 0x060010B1 RID: 4273 RVA: 0x00050AB0 File Offset: 0x0004ECB0
		public event EventHandler RollBack
		{
			add
			{
				this.CheckDisposed();
				if (this._rollbackHandler == null)
				{
					this._rollbackCallback = new SQLiteRollbackCallback(this.RollbackCallback);
					if (this._sql != null)
					{
						this._sql.SetRollbackHook(this._rollbackCallback);
					}
				}
				this._rollbackHandler += value;
			}
			remove
			{
				this.CheckDisposed();
				this._rollbackHandler -= value;
				if (this._rollbackHandler == null)
				{
					if (this._sql != null)
					{
						this._sql.SetRollbackHook(null);
					}
					this._rollbackCallback = null;
				}
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00050AE8 File Offset: 0x0004ECE8
		private int CommitCallback(IntPtr parg)
		{
			try
			{
				CommitEventArgs commitEventArgs = new CommitEventArgs();
				if (this._commitHandler != null)
				{
					this._commitHandler(this, commitEventArgs);
				}
				return commitEventArgs.AbortTransaction ? 1 : 0;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Commit", ex }));
					}
				}
				catch
				{
				}
			}
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.RollbackOnException))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00050BB0 File Offset: 0x0004EDB0
		private void RollbackCallback(IntPtr parg)
		{
			try
			{
				if (this._rollbackHandler != null)
				{
					this._rollbackHandler(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Rollback", ex }));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00050C48 File Offset: 0x0004EE48
		protected override DbProviderFactory DbProviderFactory
		{
			get
			{
				DbProviderFactory instance = SQLiteFactory.Instance;
				if (SQLite3.ForceLogLifecycle())
				{
					SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Returning \"{0}\" from SQLiteConnection.DbProviderFactory...", new object[] { (instance != null) ? instance.ToString() : "<null>" }));
				}
				return instance;
			}
		}

		// Token: 0x04000631 RID: 1585
		internal const DbType BadDbType = (DbType)(-1);

		// Token: 0x04000632 RID: 1586
		internal const string DefaultBaseSchemaName = "sqlite_default_schema";

		// Token: 0x04000633 RID: 1587
		private const string MemoryFileName = ":memory:";

		// Token: 0x04000634 RID: 1588
		internal const IsolationLevel DeferredIsolationLevel = IsolationLevel.ReadCommitted;

		// Token: 0x04000635 RID: 1589
		internal const IsolationLevel ImmediateIsolationLevel = IsolationLevel.Serializable;

		// Token: 0x04000636 RID: 1590
		private const SQLiteConnectionFlags FallbackDefaultFlags = SQLiteConnectionFlags.Default;

		// Token: 0x04000637 RID: 1591
		private const SQLiteSynchronousEnum DefaultSynchronous = SQLiteSynchronousEnum.Default;

		// Token: 0x04000638 RID: 1592
		private const SQLiteJournalModeEnum DefaultJournalMode = SQLiteJournalModeEnum.Default;

		// Token: 0x04000639 RID: 1593
		private const IsolationLevel DefaultIsolationLevel = IsolationLevel.Serializable;

		// Token: 0x0400063A RID: 1594
		internal const SQLiteDateFormats DefaultDateTimeFormat = SQLiteDateFormats.ISO8601;

		// Token: 0x0400063B RID: 1595
		internal const DateTimeKind DefaultDateTimeKind = DateTimeKind.Unspecified;

		// Token: 0x0400063C RID: 1596
		internal const string DefaultDateTimeFormatString = null;

		// Token: 0x0400063D RID: 1597
		private const string DefaultDataSource = null;

		// Token: 0x0400063E RID: 1598
		private const string DefaultUri = null;

		// Token: 0x0400063F RID: 1599
		private const string DefaultFullUri = null;

		// Token: 0x04000640 RID: 1600
		private const string DefaultTextPassword = null;

		// Token: 0x04000641 RID: 1601
		private const string DefaultHexPassword = null;

		// Token: 0x04000642 RID: 1602
		private const string DefaultPassword = null;

		// Token: 0x04000643 RID: 1603
		private const int DefaultVersion = 3;

		// Token: 0x04000644 RID: 1604
		private const int DefaultPageSize = 4096;

		// Token: 0x04000645 RID: 1605
		private const int DefaultMaxPageCount = 0;

		// Token: 0x04000646 RID: 1606
		private const int DefaultCacheSize = -2000;

		// Token: 0x04000647 RID: 1607
		private const int DefaultMaxPoolSize = 100;

		// Token: 0x04000648 RID: 1608
		private const int DefaultConnectionTimeout = 30;

		// Token: 0x04000649 RID: 1609
		private const int DefaultBusyTimeout = 0;

		// Token: 0x0400064A RID: 1610
		private const int DefaultWaitTimeout = 30000;

		// Token: 0x0400064B RID: 1611
		private const bool DefaultNoDefaultFlags = false;

		// Token: 0x0400064C RID: 1612
		private const bool DefaultNoSharedFlags = false;

		// Token: 0x0400064D RID: 1613
		private const bool DefaultFailIfMissing = false;

		// Token: 0x0400064E RID: 1614
		private const bool DefaultReadOnly = false;

		// Token: 0x0400064F RID: 1615
		internal const bool DefaultBinaryGUID = true;

		// Token: 0x04000650 RID: 1616
		private const bool DefaultUseUTF16Encoding = false;

		// Token: 0x04000651 RID: 1617
		private const bool DefaultToFullPath = true;

		// Token: 0x04000652 RID: 1618
		private const bool DefaultPooling = false;

		// Token: 0x04000653 RID: 1619
		private const bool DefaultLegacyFormat = false;

		// Token: 0x04000654 RID: 1620
		private const bool DefaultForeignKeys = false;

		// Token: 0x04000655 RID: 1621
		private const bool DefaultRecursiveTriggers = false;

		// Token: 0x04000656 RID: 1622
		private const bool DefaultEnlist = true;

		// Token: 0x04000657 RID: 1623
		private const bool DefaultSetDefaults = true;

		// Token: 0x04000658 RID: 1624
		internal const int DefaultPrepareRetries = 3;

		// Token: 0x04000659 RID: 1625
		private const string _DefaultTypeName = null;

		// Token: 0x0400065A RID: 1626
		private const string DefaultVfsName = null;

		// Token: 0x0400065B RID: 1627
		private const int DefaultProgressOps = 0;

		// Token: 0x0400065C RID: 1628
		private const int SQLITE_FCNTL_CHUNK_SIZE = 6;

		// Token: 0x0400065D RID: 1629
		private const int SQLITE_FCNTL_WIN32_AV_RETRY = 9;

		// Token: 0x0400065E RID: 1630
		private const string _dataDirectory = "|DataDirectory|";

		// Token: 0x0400065F RID: 1631
		private static readonly DbType? _DefaultDbType = null;

		// Token: 0x04000660 RID: 1632
		private static string _defaultCatalogName = "main";

		// Token: 0x04000661 RID: 1633
		private static string _defaultMasterTableName = "sqlite_master";

		// Token: 0x04000662 RID: 1634
		private static string _temporaryCatalogName = "temp";

		// Token: 0x04000663 RID: 1635
		private static string _temporaryMasterTableName = "sqlite_temp_master";

		// Token: 0x04000664 RID: 1636
		private static readonly Assembly _assembly = typeof(SQLiteConnection).Assembly;

		// Token: 0x04000665 RID: 1637
		private static readonly object _syncRoot = new object();

		// Token: 0x04000667 RID: 1639
		private static SQLiteConnectionFlags _sharedFlags;

		// Token: 0x04000668 RID: 1640
		[ThreadStatic]
		private static SQLiteConnection _lastConnectionInOpen;

		// Token: 0x04000669 RID: 1641
		private ConnectionState _connectionState;

		// Token: 0x0400066A RID: 1642
		private string _connectionString;

		// Token: 0x0400066B RID: 1643
		internal int _transactionLevel;

		// Token: 0x0400066C RID: 1644
		internal int _transactionSequence;

		// Token: 0x0400066D RID: 1645
		internal bool _noDispose;

		// Token: 0x0400066E RID: 1646
		private bool _disposing;

		// Token: 0x0400066F RID: 1647
		private IsolationLevel _defaultIsolation;

		// Token: 0x04000670 RID: 1648
		internal readonly object _enlistmentSyncRoot = new object();

		// Token: 0x04000671 RID: 1649
		internal SQLiteEnlistment _enlistment;

		// Token: 0x04000672 RID: 1650
		internal SQLiteDbTypeMap _typeNames;

		// Token: 0x04000673 RID: 1651
		private SQLiteTypeCallbacksMap _typeCallbacks;

		// Token: 0x04000674 RID: 1652
		internal SQLiteBase _sql;

		// Token: 0x04000675 RID: 1653
		private string _dataSource;

		// Token: 0x04000676 RID: 1654
		private byte[] _password;

		// Token: 0x04000677 RID: 1655
		private bool _passwordWasText;

		// Token: 0x04000678 RID: 1656
		internal string _baseSchemaName;

		// Token: 0x04000679 RID: 1657
		private SQLiteConnectionFlags _flags;

		// Token: 0x0400067A RID: 1658
		private Dictionary<string, object> _cachedSettings;

		// Token: 0x0400067B RID: 1659
		private DbType? _defaultDbType;

		// Token: 0x0400067C RID: 1660
		private string _defaultTypeName;

		// Token: 0x0400067D RID: 1661
		private string _vfsName;

		// Token: 0x0400067E RID: 1662
		private int _defaultTimeout;

		// Token: 0x0400067F RID: 1663
		private int _busyTimeout;

		// Token: 0x04000680 RID: 1664
		private int _waitTimeout;

		// Token: 0x04000681 RID: 1665
		internal int _prepareRetries;

		// Token: 0x04000682 RID: 1666
		private int _progressOps;

		// Token: 0x04000683 RID: 1667
		private bool _parseViaFramework;

		// Token: 0x04000684 RID: 1668
		internal bool _binaryGuid;

		// Token: 0x04000685 RID: 1669
		internal int _version;

		// Token: 0x0400068D RID: 1677
		private SQLiteBusyCallback _busyCallback;

		// Token: 0x0400068E RID: 1678
		private SQLiteProgressCallback _progressCallback;

		// Token: 0x0400068F RID: 1679
		private SQLiteAuthorizerCallback _authorizerCallback;

		// Token: 0x04000690 RID: 1680
		private SQLiteUpdateCallback _updateCallback;

		// Token: 0x04000691 RID: 1681
		private SQLiteCommitCallback _commitCallback;

		// Token: 0x04000692 RID: 1682
		private SQLiteTraceCallback _traceCallback;

		// Token: 0x04000693 RID: 1683
		private SQLiteRollbackCallback _rollbackCallback;

		// Token: 0x04000695 RID: 1685
		private bool disposed;
	}
}
