using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;

namespace System.Data.SQLite
{
	// Token: 0x02000159 RID: 345
	[ToolboxItem(true)]
	[Designer("SQLite.Designer.SQLiteCommandDesigner, SQLite.Designer, Version=1.0.116.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
	public sealed class SQLiteCommand : DbCommand, ICloneable
	{
		// Token: 0x06000F58 RID: 3928 RVA: 0x00047954 File Offset: 0x00045B54
		public SQLiteCommand()
			: this(null, null)
		{
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00047960 File Offset: 0x00045B60
		public SQLiteCommand(string commandText)
			: this(commandText, null, null)
		{
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0004796C File Offset: 0x00045B6C
		public SQLiteCommand(string commandText, SQLiteConnection connection)
			: this(commandText, connection, null)
		{
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00047978 File Offset: 0x00045B78
		public SQLiteCommand(SQLiteConnection connection)
			: this(null, connection, null)
		{
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00047984 File Offset: 0x00045B84
		private SQLiteCommand(SQLiteCommand source)
			: this(source.CommandText, source.Connection, source.Transaction)
		{
			this.CommandTimeout = source.CommandTimeout;
			this.DesignTimeVisible = source.DesignTimeVisible;
			this.UpdatedRowSource = source.UpdatedRowSource;
			foreach (object obj in source._parameterCollection)
			{
				SQLiteParameter sqliteParameter = (SQLiteParameter)obj;
				this.Parameters.Add(sqliteParameter.Clone());
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00047A30 File Offset: 0x00045C30
		public SQLiteCommand(string commandText, SQLiteConnection connection, SQLiteTransaction transaction)
		{
			this._commandTimeout = 30;
			this._parameterCollection = new SQLiteParameterCollection(this);
			this._designTimeVisible = true;
			this._updateRowSource = UpdateRowSource.None;
			if (commandText != null)
			{
				this.CommandText = commandText;
			}
			if (connection != null)
			{
				this.DbConnection = connection;
				this._commandTimeout = connection.DefaultTimeout;
			}
			if (transaction != null)
			{
				this.Transaction = transaction;
			}
			SQLiteConnection.OnChanged(connection, new ConnectionEventArgs(SQLiteConnectionEventType.NewCommand, null, transaction, this, null, null, null, null));
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00047AB0 File Offset: 0x00045CB0
		[Conditional("CHECK_STATE")]
		internal static void Check(SQLiteCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			command.CheckDisposed();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00047ACC File Offset: 0x00045CCC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteCommand).Name);
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00047AF0 File Offset: 0x00045CF0
		protected override void Dispose(bool disposing)
		{
			SQLiteConnection.OnChanged(this._cnn, new ConnectionEventArgs(SQLiteConnectionEventType.DisposingCommand, null, this._transaction, this, null, null, null, new object[] { disposing, this.disposed }));
			bool flag = false;
			try
			{
				if (!this.disposed && disposing)
				{
					SQLiteDataReader sqliteDataReader = null;
					if (this._activeReader != null)
					{
						try
						{
							sqliteDataReader = this._activeReader.Target as SQLiteDataReader;
						}
						catch (InvalidOperationException)
						{
						}
					}
					if (sqliteDataReader != null)
					{
						sqliteDataReader._disposeCommand = true;
						this._activeReader = null;
						flag = true;
					}
					else
					{
						this.Connection = null;
						this._parameterCollection.Clear();
						this._commandText = null;
					}
				}
			}
			finally
			{
				if (!flag)
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00047BE0 File Offset: 0x00045DE0
		internal static SQLiteConnectionFlags GetFlags(SQLiteCommand command)
		{
			try
			{
				if (command != null)
				{
					SQLiteConnection cnn = command._cnn;
					if (cnn != null)
					{
						return cnn.Flags;
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
			return SQLiteConnectionFlags.Default;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00047C34 File Offset: 0x00045E34
		private void DisposeStatements()
		{
			if (this._statementList == null)
			{
				return;
			}
			int count = this._statementList.Count;
			for (int i = 0; i < count; i++)
			{
				SQLiteStatement sqliteStatement = this._statementList[i];
				if (sqliteStatement != null)
				{
					sqliteStatement.Dispose();
				}
			}
			this._statementList = null;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00047C8C File Offset: 0x00045E8C
		private void ClearDataReader()
		{
			if (this._activeReader != null)
			{
				SQLiteDataReader sqliteDataReader = null;
				try
				{
					sqliteDataReader = this._activeReader.Target as SQLiteDataReader;
				}
				catch (InvalidOperationException)
				{
				}
				if (sqliteDataReader != null)
				{
					sqliteDataReader.Close();
				}
				this._activeReader = null;
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00047CE8 File Offset: 0x00045EE8
		internal void ClearCommands()
		{
			this.ClearDataReader();
			this.DisposeStatements();
			this._parameterCollection.Unbind();
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00047D04 File Offset: 0x00045F04
		internal SQLiteStatement BuildNextCommand()
		{
			SQLiteStatement sqliteStatement = null;
			SQLiteStatement sqliteStatement2;
			try
			{
				if (this._cnn != null && this._cnn._sql != null)
				{
					if (this._statementList == null)
					{
						this._remainingText = this._commandText;
					}
					sqliteStatement = this._cnn._sql.Prepare(this._cnn, this._remainingText, (this._statementList == null) ? null : this._statementList[this._statementList.Count - 1], (uint)(this._commandTimeout * 1000), ref this._remainingText);
					if (sqliteStatement != null)
					{
						sqliteStatement._command = this;
						if (this._statementList == null)
						{
							this._statementList = new List<SQLiteStatement>();
						}
						this._statementList.Add(sqliteStatement);
						this._parameterCollection.MapParameters(sqliteStatement);
						sqliteStatement.BindParameters();
					}
				}
				sqliteStatement2 = sqliteStatement;
			}
			catch (Exception)
			{
				if (sqliteStatement != null)
				{
					if (this._statementList != null && this._statementList.Contains(sqliteStatement))
					{
						this._statementList.Remove(sqliteStatement);
					}
					sqliteStatement.Dispose();
				}
				this._remainingText = null;
				throw;
			}
			return sqliteStatement2;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00047E38 File Offset: 0x00046038
		internal SQLiteStatement GetStatement(int index)
		{
			if (this._statementList == null)
			{
				return this.BuildNextCommand();
			}
			if (index != this._statementList.Count)
			{
				SQLiteStatement sqliteStatement = this._statementList[index];
				sqliteStatement.BindParameters();
				return sqliteStatement;
			}
			if (!string.IsNullOrEmpty(this._remainingText))
			{
				return this.BuildNextCommand();
			}
			return null;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00047E9C File Offset: 0x0004609C
		public override void Cancel()
		{
			this.CheckDisposed();
			if (this._activeReader != null)
			{
				SQLiteDataReader sqliteDataReader = this._activeReader.Target as SQLiteDataReader;
				if (sqliteDataReader != null)
				{
					sqliteDataReader.Cancel();
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x00047EDC File Offset: 0x000460DC
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x00047EEC File Offset: 0x000460EC
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
		[Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public override string CommandText
		{
			get
			{
				this.CheckDisposed();
				return this._commandText;
			}
			set
			{
				this.CheckDisposed();
				if (this._commandText == value)
				{
					return;
				}
				if (this._activeReader != null && this._activeReader.IsAlive)
				{
					throw new InvalidOperationException("Cannot set CommandText while a DataReader is active");
				}
				this.ClearCommands();
				this._commandText = value;
				SQLiteConnection cnn = this._cnn;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x00047F50 File Offset: 0x00046150
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x00047F60 File Offset: 0x00046160
		[DefaultValue(30)]
		public override int CommandTimeout
		{
			get
			{
				this.CheckDisposed();
				return this._commandTimeout;
			}
			set
			{
				this.CheckDisposed();
				this._commandTimeout = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00047F70 File Offset: 0x00046170
		// (set) Token: 0x06000F6D RID: 3949 RVA: 0x00047F7C File Offset: 0x0004617C
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(CommandType.Text)]
		public override CommandType CommandType
		{
			get
			{
				this.CheckDisposed();
				return CommandType.Text;
			}
			set
			{
				this.CheckDisposed();
				if (value != CommandType.Text)
				{
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00047F94 File Offset: 0x00046194
		protected override DbParameter CreateDbParameter()
		{
			return this.CreateParameter();
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00047F9C File Offset: 0x0004619C
		public new SQLiteParameter CreateParameter()
		{
			this.CheckDisposed();
			return new SQLiteParameter(this);
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x00047FAC File Offset: 0x000461AC
		// (set) Token: 0x06000F71 RID: 3953 RVA: 0x00047FBC File Offset: 0x000461BC
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public new SQLiteConnection Connection
		{
			get
			{
				this.CheckDisposed();
				return this._cnn;
			}
			set
			{
				this.CheckDisposed();
				if (this._activeReader != null && this._activeReader.IsAlive)
				{
					throw new InvalidOperationException("Cannot set Connection while a DataReader is active");
				}
				if (this._cnn != null)
				{
					this.ClearCommands();
				}
				this._cnn = value;
				if (this._cnn != null)
				{
					this._version = this._cnn._version;
				}
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00048030 File Offset: 0x00046230
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x00048038 File Offset: 0x00046238
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				this.Connection = (SQLiteConnection)value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x00048048 File Offset: 0x00046248
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new SQLiteParameterCollection Parameters
		{
			get
			{
				this.CheckDisposed();
				return this._parameterCollection;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x00048058 File Offset: 0x00046258
		protected override DbParameterCollection DbParameterCollection
		{
			get
			{
				return this.Parameters;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x00048060 File Offset: 0x00046260
		// (set) Token: 0x06000F77 RID: 3959 RVA: 0x00048070 File Offset: 0x00046270
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new SQLiteTransaction Transaction
		{
			get
			{
				this.CheckDisposed();
				return this._transaction;
			}
			set
			{
				this.CheckDisposed();
				if (this._cnn == null)
				{
					if (value != null)
					{
						this.Connection = value.Connection;
					}
					this._transaction = value;
					return;
				}
				if (this._activeReader != null && this._activeReader.IsAlive)
				{
					throw new InvalidOperationException("Cannot set Transaction while a DataReader is active");
				}
				if (value != null && value._cnn != this._cnn)
				{
					throw new ArgumentException("Transaction is not associated with the command's connection");
				}
				this._transaction = value;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000F78 RID: 3960 RVA: 0x000480FC File Offset: 0x000462FC
		// (set) Token: 0x06000F79 RID: 3961 RVA: 0x00048104 File Offset: 0x00046304
		protected override DbTransaction DbTransaction
		{
			get
			{
				return this.Transaction;
			}
			set
			{
				this.Transaction = (SQLiteTransaction)value;
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00048114 File Offset: 0x00046314
		public void VerifyOnly()
		{
			this.CheckDisposed();
			SQLiteConnection cnn = this._cnn;
			SQLiteBase sql = cnn._sql;
			if (cnn == null || sql == null)
			{
				throw new SQLiteException("invalid or unusable connection");
			}
			List<SQLiteStatement> list = null;
			SQLiteStatement sqliteStatement = null;
			try
			{
				string text = this._commandText;
				uint num = (uint)(this._commandTimeout * 1000);
				SQLiteStatement sqliteStatement2 = null;
				while (text != null && text.Length > 0)
				{
					sqliteStatement = sql.Prepare(cnn, text, sqliteStatement2, num, ref text);
					sqliteStatement2 = sqliteStatement;
					if (sqliteStatement != null)
					{
						if (list == null)
						{
							list = new List<SQLiteStatement>();
						}
						list.Add(sqliteStatement);
						sqliteStatement = null;
					}
					if (text != null)
					{
						text = text.Trim();
					}
				}
			}
			finally
			{
				if (sqliteStatement != null)
				{
					sqliteStatement.Dispose();
					sqliteStatement = null;
				}
				if (list != null)
				{
					foreach (SQLiteStatement sqliteStatement3 in list)
					{
						if (sqliteStatement3 != null)
						{
							sqliteStatement3.Dispose();
						}
					}
					list.Clear();
					list = null;
				}
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0004823C File Offset: 0x0004643C
		private void InitializeForReader()
		{
			if (this._activeReader != null && this._activeReader.IsAlive)
			{
				throw new InvalidOperationException("DataReader already active on this command");
			}
			if (this._cnn == null)
			{
				throw new InvalidOperationException("No connection associated with this command");
			}
			if (this._cnn.State != ConnectionState.Open)
			{
				throw new InvalidOperationException("Database is not open");
			}
			if (this._cnn._version != this._version)
			{
				this._version = this._cnn._version;
				this.ClearCommands();
			}
			this._parameterCollection.MapParameters(null);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000482E0 File Offset: 0x000464E0
		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000482EC File Offset: 0x000464EC
		public static object Execute(string commandText, SQLiteExecuteType executeType, string connectionString, params object[] args)
		{
			return SQLiteCommand.Execute(commandText, executeType, CommandBehavior.Default, connectionString, args);
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000482F8 File Offset: 0x000464F8
		public static object Execute(string commandText, SQLiteExecuteType executeType, CommandBehavior commandBehavior, string connectionString, params object[] args)
		{
			SQLiteConnection sqliteConnection = null;
			try
			{
				if (connectionString == null)
				{
					connectionString = SQLiteCommand.DefaultConnectionString;
				}
				SQLiteConnection sqliteConnection2;
				sqliteConnection = (sqliteConnection2 = new SQLiteConnection(connectionString));
				try
				{
					sqliteConnection.Open();
					using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
					{
						sqliteCommand.CommandText = commandText;
						if (args != null)
						{
							foreach (object obj in args)
							{
								SQLiteParameter sqliteParameter = obj as SQLiteParameter;
								if (sqliteParameter == null)
								{
									sqliteParameter = sqliteCommand.CreateParameter();
									sqliteParameter.DbType = DbType.Object;
									sqliteParameter.Value = obj;
								}
								sqliteCommand.Parameters.Add(sqliteParameter);
							}
						}
						switch (executeType)
						{
						case SQLiteExecuteType.NonQuery:
							return sqliteCommand.ExecuteNonQuery(commandBehavior);
						case SQLiteExecuteType.Scalar:
							return sqliteCommand.ExecuteScalar(commandBehavior);
						case SQLiteExecuteType.Reader:
						{
							bool flag = true;
							try
							{
								return sqliteCommand.ExecuteReader(commandBehavior | CommandBehavior.CloseConnection);
							}
							catch
							{
								flag = false;
								throw;
							}
							finally
							{
								if (flag)
								{
									sqliteConnection._noDispose = true;
								}
							}
							break;
						}
						}
					}
				}
				finally
				{
					if (sqliteConnection2 != null)
					{
						((IDisposable)sqliteConnection2).Dispose();
					}
				}
			}
			finally
			{
				if (sqliteConnection != null)
				{
					sqliteConnection._noDispose = false;
				}
			}
			return null;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000484B0 File Offset: 0x000466B0
		public static object Execute(string commandText, SQLiteExecuteType executeType, CommandBehavior commandBehavior, SQLiteConnection connection, params object[] args)
		{
			using (SQLiteCommand sqliteCommand = connection.CreateCommand())
			{
				sqliteCommand.CommandText = commandText;
				if (args != null)
				{
					foreach (object obj in args)
					{
						SQLiteParameter sqliteParameter = obj as SQLiteParameter;
						if (sqliteParameter == null)
						{
							sqliteParameter = sqliteCommand.CreateParameter();
							sqliteParameter.DbType = DbType.Object;
							sqliteParameter.Value = obj;
						}
						sqliteCommand.Parameters.Add(sqliteParameter);
					}
				}
				switch (executeType)
				{
				case SQLiteExecuteType.NonQuery:
					return sqliteCommand.ExecuteNonQuery(commandBehavior);
				case SQLiteExecuteType.Scalar:
					return sqliteCommand.ExecuteScalar(commandBehavior);
				case SQLiteExecuteType.Reader:
					return sqliteCommand.ExecuteReader(commandBehavior);
				}
			}
			return null;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00048594 File Offset: 0x00046794
		public new SQLiteDataReader ExecuteReader(CommandBehavior behavior)
		{
			this.CheckDisposed();
			this.InitializeForReader();
			SQLiteDataReader sqliteDataReader = new SQLiteDataReader(this, behavior);
			this._activeReader = new WeakReference(sqliteDataReader, false);
			return sqliteDataReader;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000485C8 File Offset: 0x000467C8
		public new SQLiteDataReader ExecuteReader()
		{
			this.CheckDisposed();
			return this.ExecuteReader(CommandBehavior.Default);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000485D8 File Offset: 0x000467D8
		internal void ResetDataReader()
		{
			this._activeReader = null;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x000485E4 File Offset: 0x000467E4
		public override int ExecuteNonQuery()
		{
			this.CheckDisposed();
			return this.ExecuteNonQuery(CommandBehavior.Default);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000485F4 File Offset: 0x000467F4
		public int ExecuteNonQuery(CommandBehavior behavior)
		{
			this.CheckDisposed();
			int recordsAffected;
			using (SQLiteDataReader sqliteDataReader = this.ExecuteReader(behavior | CommandBehavior.SingleRow | CommandBehavior.SingleResult))
			{
				while (sqliteDataReader.NextResult())
				{
				}
				recordsAffected = sqliteDataReader.RecordsAffected;
			}
			return recordsAffected;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00048648 File Offset: 0x00046848
		public override object ExecuteScalar()
		{
			this.CheckDisposed();
			return this.ExecuteScalar(CommandBehavior.Default);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00048658 File Offset: 0x00046858
		public object ExecuteScalar(CommandBehavior behavior)
		{
			this.CheckDisposed();
			using (SQLiteDataReader sqliteDataReader = this.ExecuteReader(behavior | CommandBehavior.SingleRow | CommandBehavior.SingleResult))
			{
				if (sqliteDataReader.Read() && sqliteDataReader.FieldCount > 0)
				{
					return sqliteDataReader[0];
				}
			}
			return null;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x000486C0 File Offset: 0x000468C0
		public void Reset()
		{
			this.CheckDisposed();
			this.Reset(true, false);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000486D0 File Offset: 0x000468D0
		public void Reset(bool clearBindings, bool ignoreErrors)
		{
			this.CheckDisposed();
			if (clearBindings && this._parameterCollection != null)
			{
				this._parameterCollection.Unbind();
			}
			this.ClearDataReader();
			if (this._statementList == null)
			{
				return;
			}
			SQLiteBase sql = this._cnn._sql;
			foreach (SQLiteStatement sqliteStatement in this._statementList)
			{
				if (sqliteStatement != null)
				{
					SQLiteStatementHandle sqlite_stmt = sqliteStatement._sqlite_stmt;
					if (sqlite_stmt != null)
					{
						SQLiteErrorCode sqliteErrorCode = sql.Reset(sqliteStatement);
						if (sqliteErrorCode == SQLiteErrorCode.Ok && clearBindings && SQLite3.SQLiteVersionNumber >= 3003007)
						{
							sqliteErrorCode = UnsafeNativeMethods.sqlite3_clear_bindings(sqlite_stmt);
						}
						if (!ignoreErrors && sqliteErrorCode != SQLiteErrorCode.Ok)
						{
							throw new SQLiteException(sqliteErrorCode, sql.GetLastError());
						}
					}
				}
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000487C4 File Offset: 0x000469C4
		public override void Prepare()
		{
			this.CheckDisposed();
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x000487CC File Offset: 0x000469CC
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x000487DC File Offset: 0x000469DC
		[DefaultValue(UpdateRowSource.None)]
		public override UpdateRowSource UpdatedRowSource
		{
			get
			{
				this.CheckDisposed();
				return this._updateRowSource;
			}
			set
			{
				this.CheckDisposed();
				this._updateRowSource = value;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x000487EC File Offset: 0x000469EC
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x000487FC File Offset: 0x000469FC
		[DefaultValue(true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignOnly(true)]
		[Browsable(false)]
		public override bool DesignTimeVisible
		{
			get
			{
				this.CheckDisposed();
				return this._designTimeVisible;
			}
			set
			{
				this.CheckDisposed();
				this._designTimeVisible = value;
				TypeDescriptor.Refresh(this);
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00048814 File Offset: 0x00046A14
		public object Clone()
		{
			this.CheckDisposed();
			return new SQLiteCommand(this);
		}

		// Token: 0x040005FD RID: 1533
		internal static readonly string DefaultConnectionString = "Data Source=:memory:;";

		// Token: 0x040005FE RID: 1534
		private string _commandText;

		// Token: 0x040005FF RID: 1535
		private SQLiteConnection _cnn;

		// Token: 0x04000600 RID: 1536
		private int _version;

		// Token: 0x04000601 RID: 1537
		private WeakReference _activeReader;

		// Token: 0x04000602 RID: 1538
		internal int _commandTimeout;

		// Token: 0x04000603 RID: 1539
		private bool _designTimeVisible;

		// Token: 0x04000604 RID: 1540
		private UpdateRowSource _updateRowSource;

		// Token: 0x04000605 RID: 1541
		private SQLiteParameterCollection _parameterCollection;

		// Token: 0x04000606 RID: 1542
		internal List<SQLiteStatement> _statementList;

		// Token: 0x04000607 RID: 1543
		internal string _remainingText;

		// Token: 0x04000608 RID: 1544
		private SQLiteTransaction _transaction;

		// Token: 0x04000609 RID: 1545
		private bool disposed;
	}
}
