using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x0200014D RID: 333
	internal class SQLite3 : SQLiteBase
	{
		// Token: 0x06000E84 RID: 3716 RVA: 0x00042688 File Offset: 0x00040888
		internal SQLite3(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString, IntPtr db, string fileName, bool ownHandle)
			: base(fmt, kind, fmtString)
		{
			this.InitializeForceLogPrepare();
			SQLiteConnectionPool.CreateAndInitialize(null, UnsafeNativeMethods.GetSettingValue("SQLite_StrongConnectionPool", null) != null, false);
			if (db != IntPtr.Zero)
			{
				this._sql = new SQLiteConnectionHandle(db, ownHandle);
				this._fileName = fileName;
				this._returnToFileName = fileName;
				SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, this._sql, fileName, new object[]
				{
					typeof(SQLite3),
					fmt,
					kind,
					fmtString,
					db,
					fileName,
					ownHandle
				}));
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00042770 File Offset: 0x00040970
		private void InitializeForceLogPrepare()
		{
			if (UnsafeNativeMethods.GetSettingValue("SQLite_ForceLogPrepare", null) != null)
			{
				this._forceLogPrepare = true;
				return;
			}
			this._forceLogPrepare = false;
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00042794 File Offset: 0x00040994
		internal override bool ForceLogPrepare
		{
			get
			{
				return this._forceLogPrepare;
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0004279C File Offset: 0x0004099C
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLite3).Name);
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x000427C0 File Offset: 0x000409C0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed)
				{
					this.DisposeModules();
					this.Close(true);
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0004280C File Offset: 0x00040A0C
		private void DisposeModules()
		{
			if (this._modules != null)
			{
				foreach (KeyValuePair<string, SQLiteModule> keyValuePair in this._modules)
				{
					SQLiteModule value = keyValuePair.Value;
					if (value != null)
					{
						value.Dispose();
					}
				}
				this._modules.Clear();
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00042888 File Offset: 0x00040A88
		internal override bool Close(bool disposing)
		{
			SQLiteBase.BumpCloseCount();
			if (this._sql != null)
			{
				if (!this._sql.OwnHandle)
				{
					this._sql = null;
					return this.wasDisposed;
				}
				bool flag = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UnbindFunctionsOnClose);
				while (this._returnToPool || this._usePool)
				{
					if (SQLiteBase.ResetConnection(this._sql, this._sql, !disposing) && this.UnhookNativeCallbacks(true, !disposing))
					{
						if (flag)
						{
							SQLiteFunction.UnbindAllFunctions(this, this._flags, false);
						}
						this.DisposeModules();
						SQLiteConnectionPool.Add(this._returnToFileName, this._sql, this._poolVersion);
						SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.ClosedToPool, null, null, null, null, this._sql, this._returnToFileName, new object[]
						{
							typeof(SQLite3),
							!disposing,
							this._returnToFileName,
							this._poolVersion
						}));
						IL_0170:
						this._sql = null;
						goto IL_0177;
					}
					this._returnToFileName = this._fileName;
					this._returnToPool = false;
					this._usePool = false;
				}
				this.UnhookNativeCallbacks(disposing, !disposing);
				if (flag)
				{
					SQLiteFunction.UnbindAllFunctions(this, this._flags, false);
				}
				this._sql.Dispose();
				this.wasDisposed = true;
				this.FreeDbName(!disposing);
				goto IL_0170;
			}
			IL_0177:
			return this.wasDisposed;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00042A18 File Offset: 0x00040C18
		private int GetCancelCount()
		{
			return Interlocked.CompareExchange(ref this._cancelCount, 0, 0);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00042A28 File Offset: 0x00040C28
		private bool ShouldThrowForCancel()
		{
			return this.GetCancelCount() > 0;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00042A34 File Offset: 0x00040C34
		private int ResetCancelCount()
		{
			return Interlocked.CompareExchange(ref this._cancelCount, 0, this._cancelCount);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00042A48 File Offset: 0x00040C48
		internal override void Cancel()
		{
			try
			{
			}
			finally
			{
				Interlocked.Increment(ref this._cancelCount);
				UnsafeNativeMethods.sqlite3_interrupt(this._sql);
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00042A88 File Offset: 0x00040C88
		internal override void BindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteFunction function, SQLiteConnectionFlags flags)
		{
			if (functionAttribute == null)
			{
				throw new ArgumentNullException("functionAttribute");
			}
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			SQLiteFunction.BindFunction(this, functionAttribute, function, flags);
			if (this._functions == null)
			{
				this._functions = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
			}
			this._functions[functionAttribute] = function;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00042AE8 File Offset: 0x00040CE8
		internal override bool UnbindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteConnectionFlags flags)
		{
			if (functionAttribute == null)
			{
				throw new ArgumentNullException("functionAttribute");
			}
			SQLiteFunction sqliteFunction;
			return this._functions != null && (this._functions.TryGetValue(functionAttribute, out sqliteFunction) && SQLiteFunction.UnbindFunction(this, functionAttribute, sqliteFunction, flags) && this._functions.Remove(functionAttribute));
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00042B4C File Offset: 0x00040D4C
		internal override string Version
		{
			get
			{
				return SQLite3.SQLiteVersion;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x00042B54 File Offset: 0x00040D54
		internal override int VersionNumber
		{
			get
			{
				return SQLite3.SQLiteVersionNumber;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00042B5C File Offset: 0x00040D5C
		internal static string DefineConstants
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				IList<string> optionList = SQLiteDefineConstants.OptionList;
				if (optionList != null)
				{
					foreach (string text in optionList)
					{
						if (text != null)
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(' ');
							}
							stringBuilder.Append(text);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x00042BE4 File Offset: 0x00040DE4
		internal static string SQLiteVersion
		{
			get
			{
				return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_libversion(), -1);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00042BF4 File Offset: 0x00040DF4
		internal static int SQLiteVersionNumber
		{
			get
			{
				return UnsafeNativeMethods.sqlite3_libversion_number();
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00042BFC File Offset: 0x00040DFC
		internal static string SQLiteSourceId
		{
			get
			{
				return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_sourceid(), -1);
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00042C0C File Offset: 0x00040E0C
		internal static string SQLiteCompileOptions
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				IntPtr intPtr = UnsafeNativeMethods.sqlite3_compileoption_get(num++);
				while (intPtr != IntPtr.Zero)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(SQLiteConvert.UTF8ToString(intPtr, -1));
					intPtr = UnsafeNativeMethods.sqlite3_compileoption_get(num++);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00042C78 File Offset: 0x00040E78
		internal static string InteropVersion
		{
			get
			{
				return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.interop_libversion(), -1);
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00042C88 File Offset: 0x00040E88
		internal static string InteropSourceId
		{
			get
			{
				return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.interop_sourceid(), -1);
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x00042C98 File Offset: 0x00040E98
		internal static string InteropCompileOptions
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				IntPtr intPtr = UnsafeNativeMethods.interop_compileoption_get(num++);
				while (intPtr != IntPtr.Zero)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(SQLiteConvert.UTF8ToString(intPtr, -1));
					intPtr = UnsafeNativeMethods.interop_compileoption_get(num++);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x00042D04 File Offset: 0x00040F04
		internal override bool AutoCommit
		{
			get
			{
				return SQLiteBase.IsAutocommit(this._sql, this._sql);
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00042D1C File Offset: 0x00040F1C
		internal override bool IsReadOnly(string name)
		{
			IntPtr intPtr = IntPtr.Zero;
			bool flag;
			try
			{
				if (name != null)
				{
					intPtr = SQLiteString.Utf8IntPtrFromString(name);
				}
				int num = UnsafeNativeMethods.sqlite3_db_readonly(this._sql, intPtr);
				if (num == -1)
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "database \"{0}\" not found", new object[] { name }));
				}
				flag = num != 0;
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return flag;
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x00042DB8 File Offset: 0x00040FB8
		internal override long LastInsertRowId
		{
			get
			{
				return UnsafeNativeMethods.sqlite3_last_insert_rowid(this._sql);
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00042DCC File Offset: 0x00040FCC
		internal override int Changes
		{
			get
			{
				return UnsafeNativeMethods.sqlite3_changes_interop(this._sql);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x00042DE0 File Offset: 0x00040FE0
		internal override long MemoryUsed
		{
			get
			{
				return SQLite3.StaticMemoryUsed;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00042DE8 File Offset: 0x00040FE8
		internal static long StaticMemoryUsed
		{
			get
			{
				return UnsafeNativeMethods.sqlite3_memory_used();
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00042DF0 File Offset: 0x00040FF0
		internal override long MemoryHighwater
		{
			get
			{
				return SQLite3.StaticMemoryHighwater;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00042DF8 File Offset: 0x00040FF8
		internal static long StaticMemoryHighwater
		{
			get
			{
				return UnsafeNativeMethods.sqlite3_memory_highwater(0);
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x00042E00 File Offset: 0x00041000
		internal override bool OwnHandle
		{
			get
			{
				if (this._sql == null)
				{
					throw new SQLiteException("no connection handle available");
				}
				return this._sql.OwnHandle;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00042E24 File Offset: 0x00041024
		internal override IDictionary<SQLiteFunctionAttribute, SQLiteFunction> Functions
		{
			get
			{
				return this._functions;
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00042E2C File Offset: 0x0004102C
		internal override SQLiteErrorCode SetMemoryStatus(bool value)
		{
			return SQLite3.StaticSetMemoryStatus(value);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00042E34 File Offset: 0x00041034
		internal static SQLiteErrorCode StaticSetMemoryStatus(bool value)
		{
			return UnsafeNativeMethods.sqlite3_config_int(SQLiteConfigOpsEnum.SQLITE_CONFIG_MEMSTATUS, value ? 1 : 0);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00042E5C File Offset: 0x0004105C
		internal override SQLiteErrorCode ReleaseMemory()
		{
			return UnsafeNativeMethods.sqlite3_db_release_memory(this._sql);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00042E80 File Offset: 0x00041080
		internal static SQLiteErrorCode StaticReleaseMemory(int nBytes, bool reset, bool compact, ref int nFree, ref bool resetOk, ref uint nLargest)
		{
			SQLiteErrorCode sqliteErrorCode = SQLiteErrorCode.Ok;
			int num = UnsafeNativeMethods.sqlite3_release_memory(nBytes);
			uint num2 = 0U;
			bool flag = false;
			if (HelperMethods.IsWindows())
			{
				if (sqliteErrorCode == SQLiteErrorCode.Ok && reset)
				{
					sqliteErrorCode = UnsafeNativeMethods.sqlite3_win32_reset_heap();
					if (sqliteErrorCode == SQLiteErrorCode.Ok)
					{
						flag = true;
					}
				}
				if (sqliteErrorCode == SQLiteErrorCode.Ok && compact)
				{
					sqliteErrorCode = UnsafeNativeMethods.sqlite3_win32_compact_heap(ref num2);
				}
			}
			else if (reset || compact)
			{
				sqliteErrorCode = SQLiteErrorCode.NotFound;
			}
			nFree = num;
			nLargest = num2;
			resetOk = flag;
			return sqliteErrorCode;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00042EF8 File Offset: 0x000410F8
		internal override SQLiteErrorCode Shutdown()
		{
			return SQLite3.StaticShutdown(false);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00042F00 File Offset: 0x00041100
		internal static SQLiteErrorCode StaticShutdown(bool directories)
		{
			SQLiteErrorCode sqliteErrorCode = SQLiteErrorCode.Ok;
			if (directories && HelperMethods.IsWindows())
			{
				if (sqliteErrorCode == SQLiteErrorCode.Ok)
				{
					sqliteErrorCode = UnsafeNativeMethods.sqlite3_win32_set_directory(1U, null);
				}
				if (sqliteErrorCode == SQLiteErrorCode.Ok)
				{
					sqliteErrorCode = UnsafeNativeMethods.sqlite3_win32_set_directory(2U, null);
				}
			}
			if (sqliteErrorCode == SQLiteErrorCode.Ok)
			{
				sqliteErrorCode = UnsafeNativeMethods.sqlite3_shutdown();
			}
			return sqliteErrorCode;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00042F4C File Offset: 0x0004114C
		internal override bool IsOpen()
		{
			return this._sql != null && !this._sql.IsInvalid && !this._sql.IsClosed;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00042F7C File Offset: 0x0004117C
		internal override string GetFileName(string dbName)
		{
			if (this._sql == null)
			{
				return null;
			}
			return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_db_filename_bytes(this._sql, SQLiteConvert.ToUTF8(dbName)), -1);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00042FA8 File Offset: 0x000411A8
		protected static bool IsAllowedToUsePool(SQLiteOpenFlagsEnum openFlags)
		{
			return openFlags == SQLiteOpenFlagsEnum.Default;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00042FB0 File Offset: 0x000411B0
		internal override void Open(string strFilename, string vfsName, SQLiteConnectionFlags connectionFlags, SQLiteOpenFlagsEnum openFlags, int maxPoolSize, bool usePool)
		{
			SQLiteBase.BumpOpenCount();
			if (this._sql != null)
			{
				this.Close(false);
			}
			if (this._sql != null)
			{
				throw new SQLiteException("connection handle is still active");
			}
			this._maxPoolSize = maxPoolSize;
			this._returnToPool = false;
			this._usePool = usePool;
			if (this._usePool && !SQLite3.IsAllowedToUsePool(openFlags))
			{
				this._usePool = false;
			}
			this._fileName = strFilename;
			this._returnToFileName = strFilename;
			this._flags = connectionFlags;
			if (usePool)
			{
				this._sql = SQLiteConnectionPool.Remove(strFilename, maxPoolSize, out this._poolVersion);
				SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.OpenedFromPool, null, null, null, null, this._sql, strFilename, new object[]
				{
					typeof(SQLite3),
					strFilename,
					vfsName,
					connectionFlags,
					openFlags,
					maxPoolSize,
					usePool,
					this._poolVersion
				}));
			}
			if (this._sql == null)
			{
				try
				{
				}
				finally
				{
					IntPtr intPtr = IntPtr.Zero;
					uint id = (uint)Process.GetCurrentProcess().Id;
					if (IntPtr.Size == 8)
					{
						intPtr = new IntPtr((long)(15153171595752648096UL | (ulong)id));
					}
					else
					{
						intPtr = new IntPtr((int)(157298080U | id));
					}
					int num = 0;
					if (!HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoExtensionFunctions))
					{
						num |= 1;
					}
					if (HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoCoreFunctions))
					{
						num |= 2;
					}
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_open_interop(SQLiteConvert.ToUTF8(strFilename), SQLiteConvert.ToUTF8(vfsName), openFlags, num, ref intPtr);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, null);
					}
					this._sql = new SQLiteConnectionHandle(intPtr, true);
					SQLiteBase.BumpCreateCount();
				}
				lock (this._sql)
				{
				}
				SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, this._sql, strFilename, new object[]
				{
					typeof(SQLite3),
					strFilename,
					vfsName,
					connectionFlags,
					openFlags,
					maxPoolSize,
					usePool
				}));
			}
			if (!HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoBindFunctions))
			{
				if (this._functions == null)
				{
					this._functions = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
				}
				foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> keyValuePair in SQLiteFunction.BindFunctions(this, connectionFlags))
				{
					this._functions[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			this.SetTimeout(0);
			GC.KeepAlive(this._sql);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x000432B8 File Offset: 0x000414B8
		internal override void ClearPool()
		{
			SQLiteConnectionPool.ClearPool(this._fileName);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000432C8 File Offset: 0x000414C8
		internal override int CountPool()
		{
			Dictionary<string, int> dictionary = null;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			SQLiteConnectionPool.GetCounts(this._fileName, ref dictionary, ref num, ref num2, ref num3);
			return num3;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000432F8 File Offset: 0x000414F8
		internal override void SetTimeout(int nTimeoutMS)
		{
			IntPtr intPtr = this._sql;
			if (intPtr == IntPtr.Zero)
			{
				throw new SQLiteException("no connection handle available");
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_busy_timeout(intPtr, nTimeoutMS);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0004334C File Offset: 0x0004154C
		internal override bool Step(SQLiteStatement stmt)
		{
			Random random = null;
			uint tickCount = (uint)Environment.TickCount;
			uint num = (uint)(stmt._command._commandTimeout * 1000);
			this.ResetCancelCount();
			SQLiteErrorCode sqliteErrorCode;
			SQLiteErrorCode sqliteErrorCode2;
			for (;;)
			{
				try
				{
				}
				finally
				{
					sqliteErrorCode = UnsafeNativeMethods.sqlite3_step(stmt._sqlite_stmt);
				}
				if (this.ShouldThrowForCancel())
				{
					break;
				}
				if (sqliteErrorCode == SQLiteErrorCode.Interrupt)
				{
					return false;
				}
				if (sqliteErrorCode == SQLiteErrorCode.Row)
				{
					return true;
				}
				if (sqliteErrorCode == SQLiteErrorCode.Done)
				{
					return false;
				}
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					sqliteErrorCode2 = this.Reset(stmt);
					if (sqliteErrorCode2 == SQLiteErrorCode.Ok)
					{
						goto Block_9;
					}
					if ((sqliteErrorCode2 == SQLiteErrorCode.Locked || sqliteErrorCode2 == SQLiteErrorCode.Busy) && stmt._command != null)
					{
						if (random == null)
						{
							random = new Random();
						}
						if (Environment.TickCount - (int)tickCount > (int)num)
						{
							goto Block_13;
						}
						Thread.Sleep(random.Next(1, 150));
					}
				}
			}
			if (sqliteErrorCode == SQLiteErrorCode.Ok || sqliteErrorCode == SQLiteErrorCode.Row || sqliteErrorCode == SQLiteErrorCode.Done)
			{
				sqliteErrorCode = SQLiteErrorCode.Interrupt;
			}
			throw new SQLiteException(sqliteErrorCode, null);
			Block_9:
			throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			Block_13:
			throw new SQLiteException(sqliteErrorCode2, this.GetLastError());
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00043464 File Offset: 0x00041664
		internal static string GetErrorString(SQLiteErrorCode rc)
		{
			try
			{
				if (SQLite3.have_errstr == null)
				{
					int sqliteVersionNumber = SQLite3.SQLiteVersionNumber;
					SQLite3.have_errstr = new bool?(sqliteVersionNumber >= 3007015);
				}
				if (SQLite3.have_errstr.Value)
				{
					IntPtr intPtr = UnsafeNativeMethods.sqlite3_errstr(rc);
					if (intPtr != IntPtr.Zero)
					{
						return Marshal.PtrToStringAnsi(intPtr);
					}
				}
			}
			catch (EntryPointNotFoundException)
			{
			}
			return SQLiteBase.FallbackGetErrorString(rc);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x000434F0 File Offset: 0x000416F0
		internal override bool IsReadOnly(SQLiteStatement stmt)
		{
			try
			{
				if (SQLite3.have_stmt_readonly == null)
				{
					int sqliteVersionNumber = SQLite3.SQLiteVersionNumber;
					SQLite3.have_stmt_readonly = new bool?(sqliteVersionNumber >= 3007004);
				}
				if (SQLite3.have_stmt_readonly.Value)
				{
					return UnsafeNativeMethods.sqlite3_stmt_readonly(stmt._sqlite_stmt) != 0;
				}
			}
			catch (EntryPointNotFoundException)
			{
			}
			return false;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00043570 File Offset: 0x00041770
		internal override SQLiteErrorCode Reset(SQLiteStatement stmt)
		{
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_reset_interop(stmt._sqlite_stmt);
			if (sqliteErrorCode == SQLiteErrorCode.Schema)
			{
				string text = null;
				using (SQLiteStatement sqliteStatement = this.Prepare(null, stmt._sqlStatement, null, (uint)(stmt._command._commandTimeout * 1000), ref text))
				{
					stmt._sqlite_stmt.Dispose();
					if (sqliteStatement != null)
					{
						stmt._sqlite_stmt = sqliteStatement._sqlite_stmt;
						sqliteStatement._sqlite_stmt = null;
					}
					stmt.BindParameters();
				}
				return SQLiteErrorCode.Unknown;
			}
			if (sqliteErrorCode == SQLiteErrorCode.Locked || sqliteErrorCode == SQLiteErrorCode.Busy)
			{
				return sqliteErrorCode;
			}
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
			return sqliteErrorCode;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0004362C File Offset: 0x0004182C
		internal override string GetLastError()
		{
			return this.GetLastError(null);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00043638 File Offset: 0x00041838
		internal override string GetLastError(string defValue)
		{
			string text = SQLiteBase.GetLastError(this._sql, this._sql);
			if (string.IsNullOrEmpty(text))
			{
				text = defValue;
			}
			return text;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00043670 File Offset: 0x00041870
		internal static bool ForceLogLifecycle()
		{
			bool value;
			lock (SQLite3.syncRoot)
			{
				if (SQLite3.forceLogLifecycle == null)
				{
					if (UnsafeNativeMethods.GetSettingValue("SQLite_ForceLogLifecycle", null) != null)
					{
						SQLite3.forceLogLifecycle = new bool?(true);
					}
					else
					{
						SQLite3.forceLogLifecycle = new bool?(false);
					}
				}
				value = SQLite3.forceLogLifecycle.Value;
			}
			return value;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x000436F8 File Offset: 0x000418F8
		internal override SQLiteStatement Prepare(SQLiteConnection cnn, string strSql, SQLiteStatement previous, uint timeoutMS, ref string strRemain)
		{
			if (!string.IsNullOrEmpty(strSql))
			{
				strSql = strSql.Trim();
			}
			if (!string.IsNullOrEmpty(strSql))
			{
				string text = ((cnn != null) ? cnn._baseSchemaName : null);
				if (!string.IsNullOrEmpty(text))
				{
					strSql = strSql.Replace(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "[{0}].", new object[] { text }), string.Empty);
					strSql = strSql.Replace(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "{0}.", new object[] { text }), string.Empty);
				}
			}
			SQLiteConnectionFlags sqliteConnectionFlags = ((cnn != null) ? cnn.Flags : SQLiteConnectionFlags.Default);
			if (this._forceLogPrepare || HelperMethods.LogPrepare(sqliteConnectionFlags))
			{
				if (strSql == null || strSql.Length == 0 || strSql.Trim().Length == 0)
				{
					SQLiteLog.LogMessage("Preparing {<nothing>}...");
				}
				else
				{
					SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Preparing {{{0}}}...", new object[] { strSql }));
				}
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			int num = 0;
			SQLiteErrorCode sqliteErrorCode = SQLiteErrorCode.Schema;
			int num2 = 0;
			int num3 = ((cnn != null) ? cnn._prepareRetries : 3);
			byte[] array = SQLiteConvert.ToUTF8(strSql);
			SQLiteStatement sqliteStatement = null;
			Random random = null;
			uint tickCount = (uint)Environment.TickCount;
			this.ResetCancelCount();
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr intPtr3 = gchandle.AddrOfPinnedObject();
			SQLiteStatementHandle sqliteStatementHandle = null;
			SQLiteStatement sqliteStatement2;
			try
			{
				while ((sqliteErrorCode == SQLiteErrorCode.Schema || sqliteErrorCode == SQLiteErrorCode.Locked || sqliteErrorCode == SQLiteErrorCode.Busy) && num2 < num3)
				{
					try
					{
					}
					finally
					{
						intPtr = IntPtr.Zero;
						intPtr2 = IntPtr.Zero;
						num = 0;
						sqliteErrorCode = UnsafeNativeMethods.sqlite3_prepare_interop(this._sql, intPtr3, array.Length - 1, ref intPtr, ref intPtr2, ref num);
						if (sqliteErrorCode == SQLiteErrorCode.Ok && intPtr != IntPtr.Zero)
						{
							if (sqliteStatementHandle != null)
							{
								sqliteStatementHandle.Dispose();
							}
							sqliteStatementHandle = new SQLiteStatementHandle(this._sql, intPtr);
						}
					}
					if (sqliteStatementHandle != null)
					{
						SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, sqliteStatementHandle, strSql, new object[]
						{
							typeof(SQLite3),
							cnn,
							strSql,
							previous,
							timeoutMS
						}));
					}
					if (this.ShouldThrowForCancel())
					{
						if (sqliteErrorCode == SQLiteErrorCode.Ok || sqliteErrorCode == SQLiteErrorCode.Row || sqliteErrorCode == SQLiteErrorCode.Done)
						{
							sqliteErrorCode = SQLiteErrorCode.Interrupt;
						}
						throw new SQLiteException(sqliteErrorCode, null);
					}
					if (sqliteErrorCode == SQLiteErrorCode.Interrupt)
					{
						break;
					}
					if (sqliteErrorCode == SQLiteErrorCode.Schema)
					{
						num2++;
					}
					else
					{
						if (sqliteErrorCode == SQLiteErrorCode.Error)
						{
							if (string.Compare(this.GetLastError(), "near \"TYPES\": syntax error", StringComparison.OrdinalIgnoreCase) == 0)
							{
								int num4 = strSql.IndexOf(';');
								if (num4 == -1)
								{
									num4 = strSql.Length - 1;
								}
								string text2 = strSql.Substring(0, num4 + 1);
								strSql = strSql.Substring(num4 + 1);
								strRemain = string.Empty;
								while (sqliteStatement == null && strSql.Length > 0)
								{
									sqliteStatement = this.Prepare(cnn, strSql, previous, timeoutMS, ref strRemain);
									strSql = strRemain;
								}
								if (sqliteStatement != null)
								{
									sqliteStatement.SetTypes(text2);
								}
								return sqliteStatement;
							}
							if (this._buildingSchema || string.Compare(this.GetLastError(), 0, "no such table: TEMP.SCHEMA", 0, 26, StringComparison.OrdinalIgnoreCase) != 0)
							{
								continue;
							}
							strRemain = string.Empty;
							this._buildingSchema = true;
							try
							{
								ISQLiteSchemaExtensions isqliteSchemaExtensions = ((IServiceProvider)SQLiteFactory.Instance).GetService(typeof(ISQLiteSchemaExtensions)) as ISQLiteSchemaExtensions;
								if (isqliteSchemaExtensions != null)
								{
									isqliteSchemaExtensions.BuildTempSchema(cnn);
								}
								while (sqliteStatement == null && strSql.Length > 0)
								{
									sqliteStatement = this.Prepare(cnn, strSql, previous, timeoutMS, ref strRemain);
									strSql = strRemain;
								}
								return sqliteStatement;
							}
							finally
							{
								this._buildingSchema = false;
							}
						}
						if (sqliteErrorCode == SQLiteErrorCode.Locked || sqliteErrorCode == SQLiteErrorCode.Busy)
						{
							if (random == null)
							{
								random = new Random();
							}
							if (Environment.TickCount - (int)tickCount > (int)timeoutMS)
							{
								throw new SQLiteException(sqliteErrorCode, this.GetLastError());
							}
							Thread.Sleep(random.Next(1, 150));
						}
					}
				}
				if (this.ShouldThrowForCancel())
				{
					if (sqliteErrorCode == SQLiteErrorCode.Ok || sqliteErrorCode == SQLiteErrorCode.Row || sqliteErrorCode == SQLiteErrorCode.Done)
					{
						sqliteErrorCode = SQLiteErrorCode.Interrupt;
					}
					throw new SQLiteException(sqliteErrorCode, null);
				}
				if (sqliteErrorCode == SQLiteErrorCode.Interrupt)
				{
					sqliteStatement2 = null;
				}
				else
				{
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, this.GetLastError());
					}
					strRemain = SQLiteConvert.UTF8ToString(intPtr2, num);
					if (sqliteStatementHandle != null)
					{
						sqliteStatement = new SQLiteStatement(this, sqliteConnectionFlags, sqliteStatementHandle, strSql.Substring(0, strSql.Length - strRemain.Length), previous);
					}
					sqliteStatement2 = sqliteStatement;
				}
			}
			finally
			{
				gchandle.Free();
			}
			return sqliteStatement2;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00043C14 File Offset: 0x00041E14
		protected static void LogBind(SQLiteStatementHandle handle, int index)
		{
			IntPtr intPtr = handle;
			SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as NULL...", new object[] { intPtr, index }));
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00043C5C File Offset: 0x00041E5C
		protected static void LogBind(SQLiteStatementHandle handle, int index, ValueType value)
		{
			IntPtr intPtr = handle;
			SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", new object[]
			{
				intPtr,
				index,
				value.GetType(),
				value
			}));
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00043CB0 File Offset: 0x00041EB0
		private static string FormatDateTime(DateTime value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK"));
			stringBuilder.Append(' ');
			stringBuilder.Append(value.Kind);
			stringBuilder.Append(' ');
			stringBuilder.Append(value.Ticks);
			return stringBuilder.ToString();
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00043D14 File Offset: 0x00041F14
		protected static void LogBind(SQLiteStatementHandle handle, int index, DateTime value)
		{
			IntPtr intPtr = handle;
			SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", new object[]
			{
				intPtr,
				index,
				typeof(DateTime),
				SQLite3.FormatDateTime(value)
			}));
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00043D70 File Offset: 0x00041F70
		protected static void LogBind(SQLiteStatementHandle handle, int index, string value)
		{
			IntPtr intPtr = handle;
			SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", new object[]
			{
				intPtr,
				index,
				typeof(string),
				(value != null) ? value : "<null>"
			}));
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00043DD8 File Offset: 0x00041FD8
		private static string ToHexadecimalString(byte[] array)
		{
			if (array == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00043E30 File Offset: 0x00042030
		protected static void LogBind(SQLiteStatementHandle handle, int index, byte[] value)
		{
			IntPtr intPtr = handle;
			SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", new object[]
			{
				intPtr,
				index,
				typeof(byte[]),
				(value != null) ? SQLite3.ToHexadecimalString(value) : "<null>"
			}));
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00043E9C File Offset: 0x0004209C
		internal override void Bind_Double(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, double value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_double(sqlite_stmt, index, value);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00043EFC File Offset: 0x000420FC
		internal override void Bind_Int32(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, int value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_int(sqlite_stmt, index, value);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00043F5C File Offset: 0x0004215C
		internal override void Bind_UInt32(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, uint value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			SQLiteErrorCode sqliteErrorCode;
			if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.BindUInt32AsInt64))
			{
				long num = (long)((ulong)value);
				sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_int64(sqlite_stmt, index, num);
			}
			else
			{
				sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_uint(sqlite_stmt, index, value);
			}
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00043FE0 File Offset: 0x000421E0
		internal override void Bind_Int64(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, long value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_int64(sqlite_stmt, index, value);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00044040 File Offset: 0x00042240
		internal override void Bind_UInt64(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, ulong value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_uint64(sqlite_stmt, index, value);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000440A0 File Offset: 0x000422A0
		internal override void Bind_Boolean(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, bool value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			int num = (value ? 1 : 0);
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_int(sqlite_stmt, index, num);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0004410C File Offset: 0x0004230C
		internal override void Bind_Text(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, string value)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, value);
			}
			byte[] array = SQLiteConvert.ToUTF8(value);
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, array);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_text(sqlite_stmt, index, array, array.Length - 1, (IntPtr)(-1));
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00044198 File Offset: 0x00042398
		internal override void Bind_DateTime(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, DateTime dt)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, dt);
			}
			if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.BindDateTimeWithKind) && this._datetimeKind != DateTimeKind.Unspecified && dt.Kind != DateTimeKind.Unspecified && dt.Kind != this._datetimeKind)
			{
				if (this._datetimeKind == DateTimeKind.Utc)
				{
					dt = dt.ToUniversalTime();
				}
				else if (this._datetimeKind == DateTimeKind.Local)
				{
					dt = dt.ToLocalTime();
				}
			}
			switch (this._datetimeFormat)
			{
			case SQLiteDateFormats.Ticks:
			{
				long ticks = dt.Ticks;
				if (this._forceLogPrepare || HelperMethods.LogBind(flags))
				{
					SQLite3.LogBind(sqlite_stmt, index, ticks);
				}
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_int64(sqlite_stmt, index, ticks);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, this.GetLastError());
				}
				return;
			}
			case SQLiteDateFormats.JulianDay:
			{
				double num = SQLiteConvert.ToJulianDay(dt);
				if (this._forceLogPrepare || HelperMethods.LogBind(flags))
				{
					SQLite3.LogBind(sqlite_stmt, index, num);
				}
				SQLiteErrorCode sqliteErrorCode2 = UnsafeNativeMethods.sqlite3_bind_double(sqlite_stmt, index, num);
				if (sqliteErrorCode2 != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode2, this.GetLastError());
				}
				return;
			}
			case SQLiteDateFormats.UnixEpoch:
			{
				long num2 = Convert.ToInt64(dt.Subtract(SQLiteConvert.UnixEpoch).TotalSeconds);
				if (this._forceLogPrepare || HelperMethods.LogBind(flags))
				{
					SQLite3.LogBind(sqlite_stmt, index, num2);
				}
				SQLiteErrorCode sqliteErrorCode3 = UnsafeNativeMethods.sqlite3_bind_int64(sqlite_stmt, index, num2);
				if (sqliteErrorCode3 != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode3, this.GetLastError());
				}
				return;
			}
			}
			byte[] array = base.ToUTF8(dt);
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, array);
			}
			SQLiteErrorCode sqliteErrorCode4 = UnsafeNativeMethods.sqlite3_bind_text(sqlite_stmt, index, array, array.Length - 1, (IntPtr)(-1));
			if (sqliteErrorCode4 != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode4, this.GetLastError());
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000443B8 File Offset: 0x000425B8
		internal override void Bind_Blob(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, byte[] blobData)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index, blobData);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_blob(sqlite_stmt, index, blobData, blobData.Length, (IntPtr)(-1));
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0004441C File Offset: 0x0004261C
		internal override void Bind_Null(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				SQLite3.LogBind(sqlite_stmt, index);
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_bind_null(sqlite_stmt, index);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00044474 File Offset: 0x00042674
		internal override int Bind_ParamCount(SQLiteStatement stmt, SQLiteConnectionFlags flags)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			int num = UnsafeNativeMethods.sqlite3_bind_parameter_count(sqlite_stmt);
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				IntPtr intPtr = sqlite_stmt;
				SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Statement {0} paramter count is {1}.", new object[] { intPtr, num }));
			}
			return num;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000444E4 File Offset: 0x000426E4
		internal override string Bind_ParamName(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			int num = 0;
			string text = SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_bind_parameter_name_interop(sqlite_stmt, index, ref num), num);
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				IntPtr intPtr = sqlite_stmt;
				SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Statement {0} paramter #{1} name is {{{2}}}.", new object[] { intPtr, index, text }));
			}
			return text;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00044568 File Offset: 0x00042768
		internal override int Bind_ParamIndex(SQLiteStatement stmt, SQLiteConnectionFlags flags, string paramName)
		{
			SQLiteStatementHandle sqlite_stmt = stmt._sqlite_stmt;
			int num = UnsafeNativeMethods.sqlite3_bind_parameter_index(sqlite_stmt, SQLiteConvert.ToUTF8(paramName));
			if (this._forceLogPrepare || HelperMethods.LogBind(flags))
			{
				IntPtr intPtr = sqlite_stmt;
				SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Statement {0} paramter index of name {{{1}}} is #{2}.", new object[] { intPtr, paramName, num }));
			}
			return num;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000445E4 File Offset: 0x000427E4
		internal override int ColumnCount(SQLiteStatement stmt)
		{
			return UnsafeNativeMethods.sqlite3_column_count(stmt._sqlite_stmt);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000445F8 File Offset: 0x000427F8
		internal override string ColumnName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			IntPtr intPtr = UnsafeNativeMethods.sqlite3_column_name_interop(stmt._sqlite_stmt, index, ref num);
			if (intPtr == IntPtr.Zero)
			{
				throw new SQLiteException(SQLiteErrorCode.NoMem, this.GetLastError());
			}
			return SQLiteConvert.UTF8ToString(intPtr, num);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00044644 File Offset: 0x00042844
		internal override TypeAffinity ColumnAffinity(SQLiteStatement stmt, int index)
		{
			return UnsafeNativeMethods.sqlite3_column_type(stmt._sqlite_stmt, index);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00044658 File Offset: 0x00042858
		internal override string ColumnType(SQLiteStatement stmt, int index, ref TypeAffinity nAffinity)
		{
			int num = 0;
			IntPtr intPtr = UnsafeNativeMethods.sqlite3_column_decltype_interop(stmt._sqlite_stmt, index, ref num);
			nAffinity = this.ColumnAffinity(stmt, index);
			if (intPtr != IntPtr.Zero && (num > 0 || num == -1))
			{
				string text = SQLiteConvert.UTF8ToString(intPtr, num);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			string[] typeDefinitions = stmt.TypeDefinitions;
			if (typeDefinitions != null && index < typeDefinitions.Length && typeDefinitions[index] != null)
			{
				return typeDefinitions[index];
			}
			return string.Empty;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000446EC File Offset: 0x000428EC
		internal override int ColumnIndex(SQLiteStatement stmt, string columnName)
		{
			int num = this.ColumnCount(stmt);
			for (int i = 0; i < num; i++)
			{
				if (string.Compare(columnName, this.ColumnName(stmt, i), StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0004472C File Offset: 0x0004292C
		internal override string ColumnOriginalName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_origin_name_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00044758 File Offset: 0x00042958
		internal override string ColumnDatabaseName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_database_name_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00044784 File Offset: 0x00042984
		internal override string ColumnTableName(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_table_name_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x000447B0 File Offset: 0x000429B0
		internal override bool DoesTableExist(string dataBase, string table)
		{
			string text = null;
			string text2 = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			return this.ColumnMetaData(dataBase, table, null, false, ref text, ref text2, ref flag, ref flag2, ref flag3);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000447E0 File Offset: 0x000429E0
		internal override bool ColumnMetaData(string dataBase, string table, string column, bool canThrow, ref string dataType, ref string collateSequence, ref bool notNull, ref bool primaryKey, ref bool autoIncrement)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_table_column_metadata_interop(this._sql, SQLiteConvert.ToUTF8(dataBase), SQLiteConvert.ToUTF8(table), SQLiteConvert.ToUTF8(column), ref zero, ref zero2, ref num, ref num2, ref num3, ref num4, ref num5);
			if (canThrow && sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
			dataType = SQLiteConvert.UTF8ToString(zero, num4);
			collateSequence = SQLiteConvert.UTF8ToString(zero2, num5);
			notNull = num == 1;
			primaryKey = num2 == 1;
			autoIncrement = num3 == 1;
			return sqliteErrorCode == SQLiteErrorCode.Ok;
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0004488C File Offset: 0x00042A8C
		internal override object GetObject(SQLiteStatement stmt, int index)
		{
			switch (this.ColumnAffinity(stmt, index))
			{
			case TypeAffinity.Int64:
				return this.GetInt64(stmt, index);
			case TypeAffinity.Double:
				return this.GetDouble(stmt, index);
			case TypeAffinity.Text:
				return this.GetText(stmt, index);
			case TypeAffinity.Blob:
			{
				long bytes = this.GetBytes(stmt, index, 0, null, 0, 0);
				if (bytes > 0L && bytes <= 2147483647L)
				{
					byte[] array = new byte[(int)bytes];
					this.GetBytes(stmt, index, 0, array, 0, (int)bytes);
					return array;
				}
				break;
			}
			case TypeAffinity.Null:
				return DBNull.Value;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00044930 File Offset: 0x00042B30
		internal override double GetDouble(SQLiteStatement stmt, int index)
		{
			return UnsafeNativeMethods.sqlite3_column_double(stmt._sqlite_stmt, index);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00044944 File Offset: 0x00042B44
		internal override bool GetBoolean(SQLiteStatement stmt, int index)
		{
			return SQLiteConvert.ToBoolean(this.GetObject(stmt, index), CultureInfo.InvariantCulture, false);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0004495C File Offset: 0x00042B5C
		internal override sbyte GetSByte(SQLiteStatement stmt, int index)
		{
			return (sbyte)(this.GetInt32(stmt, index) & 255);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00044970 File Offset: 0x00042B70
		internal override byte GetByte(SQLiteStatement stmt, int index)
		{
			return (byte)(this.GetInt32(stmt, index) & 255);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00044984 File Offset: 0x00042B84
		internal override short GetInt16(SQLiteStatement stmt, int index)
		{
			return (short)(this.GetInt32(stmt, index) & 65535);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00044998 File Offset: 0x00042B98
		internal override ushort GetUInt16(SQLiteStatement stmt, int index)
		{
			return (ushort)(this.GetInt32(stmt, index) & 65535);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000449AC File Offset: 0x00042BAC
		internal override int GetInt32(SQLiteStatement stmt, int index)
		{
			return UnsafeNativeMethods.sqlite3_column_int(stmt._sqlite_stmt, index);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000449C0 File Offset: 0x00042BC0
		internal override uint GetUInt32(SQLiteStatement stmt, int index)
		{
			return (uint)this.GetInt32(stmt, index);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x000449CC File Offset: 0x00042BCC
		internal override long GetInt64(SQLiteStatement stmt, int index)
		{
			return UnsafeNativeMethods.sqlite3_column_int64(stmt._sqlite_stmt, index);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x000449E0 File Offset: 0x00042BE0
		internal override ulong GetUInt64(SQLiteStatement stmt, int index)
		{
			return (ulong)this.GetInt64(stmt, index);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000449EC File Offset: 0x00042BEC
		internal override string GetText(SQLiteStatement stmt, int index)
		{
			int num = 0;
			return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_text_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00044A18 File Offset: 0x00042C18
		internal override DateTime GetDateTime(SQLiteStatement stmt, int index)
		{
			if (this._datetimeFormat == SQLiteDateFormats.Ticks)
			{
				return SQLiteConvert.TicksToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
			}
			if (this._datetimeFormat == SQLiteDateFormats.JulianDay)
			{
				return SQLiteConvert.ToDateTime(this.GetDouble(stmt, index), this._datetimeKind);
			}
			if (this._datetimeFormat == SQLiteDateFormats.UnixEpoch)
			{
				return SQLiteConvert.UnixEpochToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
			}
			int num = 0;
			return base.ToDateTime(UnsafeNativeMethods.sqlite3_column_text_interop(stmt._sqlite_stmt, index, ref num), num);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00044AA4 File Offset: 0x00042CA4
		internal override long GetBytes(SQLiteStatement stmt, int index, int nDataOffset, byte[] bDest, int nStart, int nLength)
		{
			int num = UnsafeNativeMethods.sqlite3_column_bytes(stmt._sqlite_stmt, index);
			if (bDest == null)
			{
				return (long)num;
			}
			int num2 = nLength;
			if (num2 + nStart > bDest.Length)
			{
				num2 = bDest.Length - nStart;
			}
			if (num2 + nDataOffset > num)
			{
				num2 = num - nDataOffset;
			}
			if (num2 > 0)
			{
				Marshal.Copy((IntPtr)(UnsafeNativeMethods.sqlite3_column_blob(stmt._sqlite_stmt, index).ToInt64() + (long)nDataOffset), bDest, nStart, num2);
			}
			else
			{
				num2 = 0;
			}
			return (long)num2;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00044B34 File Offset: 0x00042D34
		internal override char GetChar(SQLiteStatement stmt, int index)
		{
			return Convert.ToChar(this.GetUInt16(stmt, index));
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00044B44 File Offset: 0x00042D44
		internal override long GetChars(SQLiteStatement stmt, int index, int nDataOffset, char[] bDest, int nStart, int nLength)
		{
			int num = nLength;
			string text = this.GetText(stmt, index);
			int length = text.Length;
			if (bDest == null)
			{
				return (long)length;
			}
			if (num + nStart > bDest.Length)
			{
				num = bDest.Length - nStart;
			}
			if (num + nDataOffset > length)
			{
				num = length - nDataOffset;
			}
			if (num > 0)
			{
				text.CopyTo(nDataOffset, bDest, nStart, num);
			}
			else
			{
				num = 0;
			}
			return (long)num;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00044BB0 File Offset: 0x00042DB0
		internal override bool IsNull(SQLiteStatement stmt, int index)
		{
			return this.ColumnAffinity(stmt, index) == TypeAffinity.Null;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00044BC0 File Offset: 0x00042DC0
		internal override int AggregateCount(IntPtr context)
		{
			return UnsafeNativeMethods.sqlite3_aggregate_count(context);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00044BC8 File Offset: 0x00042DC8
		internal override SQLiteErrorCode CreateFunction(string strFunction, int nArgs, bool needCollSeq, SQLiteCallback func, SQLiteCallback funcstep, SQLiteFinalCallback funcfinal, bool canThrow)
		{
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_create_function_interop(this._sql, SQLiteConvert.ToUTF8(strFunction), nArgs, 4, IntPtr.Zero, func, funcstep, funcfinal, needCollSeq ? 1 : 0);
			if (sqliteErrorCode == SQLiteErrorCode.Ok)
			{
				sqliteErrorCode = UnsafeNativeMethods.sqlite3_create_function_interop(this._sql, SQLiteConvert.ToUTF8(strFunction), nArgs, 1, IntPtr.Zero, func, funcstep, funcfinal, needCollSeq ? 1 : 0);
			}
			if (canThrow && sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
			return sqliteErrorCode;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00044C5C File Offset: 0x00042E5C
		internal override SQLiteErrorCode CreateCollation(string strCollation, SQLiteCollation func, SQLiteCollation func16, bool canThrow)
		{
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_create_collation(this._sql, SQLiteConvert.ToUTF8(strCollation), 2, IntPtr.Zero, func16);
			if (sqliteErrorCode == SQLiteErrorCode.Ok)
			{
				sqliteErrorCode = UnsafeNativeMethods.sqlite3_create_collation(this._sql, SQLiteConvert.ToUTF8(strCollation), 1, IntPtr.Zero, func);
			}
			if (canThrow && sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
			return sqliteErrorCode;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00044CCC File Offset: 0x00042ECC
		internal override int ContextCollateCompare(CollationEncodingEnum enc, IntPtr context, string s1, string s2)
		{
			Encoding encoding = null;
			switch (enc)
			{
			case CollationEncodingEnum.UTF8:
				encoding = Encoding.UTF8;
				break;
			case CollationEncodingEnum.UTF16LE:
				encoding = Encoding.Unicode;
				break;
			case CollationEncodingEnum.UTF16BE:
				encoding = Encoding.BigEndianUnicode;
				break;
			}
			byte[] bytes = encoding.GetBytes(s1);
			byte[] bytes2 = encoding.GetBytes(s2);
			return UnsafeNativeMethods.sqlite3_context_collcompare_interop(context, bytes, bytes.Length, bytes2, bytes2.Length);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00044D38 File Offset: 0x00042F38
		internal override int ContextCollateCompare(CollationEncodingEnum enc, IntPtr context, char[] c1, char[] c2)
		{
			Encoding encoding = null;
			switch (enc)
			{
			case CollationEncodingEnum.UTF8:
				encoding = Encoding.UTF8;
				break;
			case CollationEncodingEnum.UTF16LE:
				encoding = Encoding.Unicode;
				break;
			case CollationEncodingEnum.UTF16BE:
				encoding = Encoding.BigEndianUnicode;
				break;
			}
			byte[] bytes = encoding.GetBytes(c1);
			byte[] bytes2 = encoding.GetBytes(c2);
			return UnsafeNativeMethods.sqlite3_context_collcompare_interop(context, bytes, bytes.Length, bytes2, bytes2.Length);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00044DA4 File Offset: 0x00042FA4
		internal override CollationSequence GetCollationSequence(SQLiteFunction func, IntPtr context)
		{
			CollationSequence collationSequence = default(CollationSequence);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			IntPtr intPtr = UnsafeNativeMethods.sqlite3_context_collseq_interop(context, ref num2, ref num3, ref num);
			collationSequence.Name = SQLiteConvert.UTF8ToString(intPtr, num);
			collationSequence.Type = (CollationTypeEnum)num2;
			collationSequence._func = func;
			collationSequence.Encoding = (CollationEncodingEnum)num3;
			return collationSequence;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00044DFC File Offset: 0x00042FFC
		internal override long GetParamValueBytes(IntPtr p, int nDataOffset, byte[] bDest, int nStart, int nLength)
		{
			int num = UnsafeNativeMethods.sqlite3_value_bytes(p);
			if (bDest == null)
			{
				return (long)num;
			}
			int num2 = nLength;
			if (num2 + nStart > bDest.Length)
			{
				num2 = bDest.Length - nStart;
			}
			if (num2 + nDataOffset > num)
			{
				num2 = num - nDataOffset;
			}
			if (num2 > 0)
			{
				Marshal.Copy((IntPtr)(UnsafeNativeMethods.sqlite3_value_blob(p).ToInt64() + (long)nDataOffset), bDest, nStart, num2);
			}
			else
			{
				num2 = 0;
			}
			return (long)num2;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00044E70 File Offset: 0x00043070
		internal override double GetParamValueDouble(IntPtr ptr)
		{
			return UnsafeNativeMethods.sqlite3_value_double(ptr);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00044E78 File Offset: 0x00043078
		internal override int GetParamValueInt32(IntPtr ptr)
		{
			return UnsafeNativeMethods.sqlite3_value_int(ptr);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00044E80 File Offset: 0x00043080
		internal override long GetParamValueInt64(IntPtr ptr)
		{
			return UnsafeNativeMethods.sqlite3_value_int64(ptr);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00044E88 File Offset: 0x00043088
		internal override string GetParamValueText(IntPtr ptr)
		{
			int num = 0;
			return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_value_text_interop(ptr, ref num), num);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00044EAC File Offset: 0x000430AC
		internal override TypeAffinity GetParamValueType(IntPtr ptr)
		{
			return UnsafeNativeMethods.sqlite3_value_type(ptr);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00044EB4 File Offset: 0x000430B4
		internal override void ReturnBlob(IntPtr context, byte[] value)
		{
			UnsafeNativeMethods.sqlite3_result_blob(context, value, value.Length, (IntPtr)(-1));
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00044EC8 File Offset: 0x000430C8
		internal override void ReturnDouble(IntPtr context, double value)
		{
			UnsafeNativeMethods.sqlite3_result_double(context, value);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00044ED4 File Offset: 0x000430D4
		internal override void ReturnError(IntPtr context, string value)
		{
			UnsafeNativeMethods.sqlite3_result_error(context, SQLiteConvert.ToUTF8(value), value.Length);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00044EE8 File Offset: 0x000430E8
		internal override void ReturnInt32(IntPtr context, int value)
		{
			UnsafeNativeMethods.sqlite3_result_int(context, value);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00044EF4 File Offset: 0x000430F4
		internal override void ReturnInt64(IntPtr context, long value)
		{
			UnsafeNativeMethods.sqlite3_result_int64(context, value);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00044F00 File Offset: 0x00043100
		internal override void ReturnNull(IntPtr context)
		{
			UnsafeNativeMethods.sqlite3_result_null(context);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00044F08 File Offset: 0x00043108
		internal override void ReturnText(IntPtr context, string value)
		{
			byte[] array = SQLiteConvert.ToUTF8(value);
			UnsafeNativeMethods.sqlite3_result_text(context, SQLiteConvert.ToUTF8(value), array.Length - 1, (IntPtr)(-1));
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00044F38 File Offset: 0x00043138
		private string GetShimExtensionFileName(ref bool isLoadNeeded)
		{
			if (this._shimIsLoadNeeded != null)
			{
				isLoadNeeded = this._shimIsLoadNeeded.Value;
			}
			else
			{
				isLoadNeeded = HelperMethods.IsWindows();
			}
			string shimExtensionFileName = this._shimExtensionFileName;
			if (shimExtensionFileName != null)
			{
				return shimExtensionFileName;
			}
			return UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00044F88 File Offset: 0x00043188
		internal override void CreateModule(SQLiteModule module, SQLiteConnectionFlags flags)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			if (HelperMethods.NoLogModule(flags))
			{
				module.LogErrors = HelperMethods.LogModuleError(flags);
				module.LogExceptions = HelperMethods.LogModuleException(flags);
			}
			if (this._sql == null)
			{
				throw new SQLiteException("connection has an invalid handle");
			}
			bool flag = false;
			string shimExtensionFileName = this.GetShimExtensionFileName(ref flag);
			if (flag)
			{
				if (shimExtensionFileName == null)
				{
					throw new SQLiteException("the file name for the \"vtshim\" extension is unknown");
				}
				if (this._shimExtensionProcName == null)
				{
					throw new SQLiteException("the entry point for the \"vtshim\" extension is unknown");
				}
				this.SetLoadExtension(true);
				this.LoadExtension(shimExtensionFileName, this._shimExtensionProcName);
			}
			if (!module.CreateDisposableModule(this._sql))
			{
				throw new SQLiteException(this.GetLastError());
			}
			if (this._modules == null)
			{
				this._modules = new Dictionary<string, SQLiteModule>();
			}
			this._modules.Add(module.Name, module);
			if (this._usePool)
			{
				this._usePool = false;
				return;
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0004508C File Offset: 0x0004328C
		internal override void DisposeModule(SQLiteModule module, SQLiteConnectionFlags flags)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			module.Dispose();
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x000450A8 File Offset: 0x000432A8
		internal override IntPtr AggregateContext(IntPtr context)
		{
			return UnsafeNativeMethods.sqlite3_aggregate_context(context, 1);
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000450B4 File Offset: 0x000432B4
		internal override SQLiteErrorCode DeclareVirtualTable(SQLiteModule module, string strSql, ref string error)
		{
			if (this._sql == null)
			{
				error = "connection has an invalid handle";
				return SQLiteErrorCode.Error;
			}
			IntPtr intPtr = IntPtr.Zero;
			SQLiteErrorCode sqliteErrorCode2;
			try
			{
				intPtr = SQLiteString.Utf8IntPtrFromString(strSql);
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_declare_vtab(this._sql, intPtr);
				if (sqliteErrorCode == SQLiteErrorCode.Ok && module != null)
				{
					module.Declared = true;
				}
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					error = this.GetLastError();
				}
				sqliteErrorCode2 = sqliteErrorCode;
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return sqliteErrorCode2;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0004514C File Offset: 0x0004334C
		internal override SQLiteErrorCode DeclareVirtualFunction(SQLiteModule module, int argumentCount, string name, ref string error)
		{
			if (this._sql == null)
			{
				error = "connection has an invalid handle";
				return SQLiteErrorCode.Error;
			}
			IntPtr intPtr = IntPtr.Zero;
			SQLiteErrorCode sqliteErrorCode2;
			try
			{
				intPtr = SQLiteString.Utf8IntPtrFromString(name);
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_overload_function(this._sql, intPtr, argumentCount);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					error = this.GetLastError();
				}
				sqliteErrorCode2 = sqliteErrorCode;
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return sqliteErrorCode2;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000451D4 File Offset: 0x000433D4
		private static string GetStatusDbOpsNames()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in Enum.GetNames(typeof(SQLiteStatusOpsEnum)))
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00045248 File Offset: 0x00043448
		private static string GetLimitOpsNames()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in Enum.GetNames(typeof(SQLiteLimitOpsEnum)))
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x000452BC File Offset: 0x000434BC
		private static string GetConfigDbOpsNames()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in Enum.GetNames(typeof(SQLiteConfigDbOpsEnum)))
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00045330 File Offset: 0x00043530
		internal override SQLiteErrorCode GetStatusParameter(SQLiteStatusOpsEnum option, bool reset, ref int current, ref int highwater)
		{
			if (!Enum.IsDefined(typeof(SQLiteStatusOpsEnum), option))
			{
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "unrecognized status option, must be: {0}", new object[] { SQLite3.GetStatusDbOpsNames() }));
			}
			return UnsafeNativeMethods.sqlite3_db_status(this._sql, option, ref current, ref highwater, reset ? 1 : 0);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000453A4 File Offset: 0x000435A4
		internal override int SetLimitOption(SQLiteLimitOpsEnum option, int value)
		{
			if (!Enum.IsDefined(typeof(SQLiteLimitOpsEnum), option))
			{
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "unrecognized limit option, must be: {0}", new object[] { SQLite3.GetLimitOpsNames() }));
			}
			return UnsafeNativeMethods.sqlite3_limit(this._sql, option, value);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00045408 File Offset: 0x00043608
		internal override SQLiteErrorCode SetConfigurationOption(SQLiteConfigDbOpsEnum option, object value)
		{
			if (!Enum.IsDefined(typeof(SQLiteConfigDbOpsEnum), option))
			{
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "unrecognized configuration option, must be: {0}", new object[] { SQLite3.GetConfigDbOpsNames() }));
			}
			if (option == SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_NONE)
			{
				return SQLiteErrorCode.Ok;
			}
			switch (option)
			{
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_MAINDBNAME:
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(value is string))
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "configuration value type mismatch, must be of type {0}", new object[] { typeof(string) }));
				}
				SQLiteErrorCode sqliteErrorCode = SQLiteErrorCode.Error;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = SQLiteString.Utf8IntPtrFromString((string)value);
					if (intPtr == IntPtr.Zero)
					{
						throw new SQLiteException(SQLiteErrorCode.NoMem, "cannot allocate database name");
					}
					sqliteErrorCode = UnsafeNativeMethods.sqlite3_db_config_charptr(this._sql, option, intPtr);
					if (sqliteErrorCode == SQLiteErrorCode.Ok)
					{
						this.FreeDbName(true);
						this.dbName = intPtr;
						intPtr = IntPtr.Zero;
					}
				}
				finally
				{
					if (sqliteErrorCode != SQLiteErrorCode.Ok && intPtr != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr);
						intPtr = IntPtr.Zero;
					}
				}
				return sqliteErrorCode;
			}
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_LOOKASIDE:
			{
				object[] array = value as object[];
				if (array == null)
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "configuration value type mismatch, must be of type {0}", new object[] { typeof(object[]) }));
				}
				if (!(array[0] is IntPtr))
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "configuration element zero (0) type mismatch, must be of type {0}", new object[] { typeof(IntPtr) }));
				}
				if (!(array[1] is int))
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "configuration element one (1) type mismatch, must be of type {0}", new object[] { typeof(int) }));
				}
				if (!(array[2] is int))
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "configuration element two (2) type mismatch, must be of type {0}", new object[] { typeof(int) }));
				}
				return UnsafeNativeMethods.sqlite3_db_config_intptr_two_ints(this._sql, option, (IntPtr)array[0], (int)array[1], (int)array[2]);
			}
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_FKEY:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_TRIGGER:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_FTS3_TOKENIZER:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_NO_CKPT_ON_CLOSE:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_QPSG:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_TRIGGER_EQP:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_RESET_DATABASE:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_DEFENSIVE:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_WRITABLE_SCHEMA:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_LEGACY_ALTER_TABLE:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_DQS_DML:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_DQS_DDL:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_VIEW:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_LEGACY_FILE_FORMAT:
			case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_TRUSTED_SCHEMA:
			{
				if (!(value is bool))
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "configuration value type mismatch, must be of type {0}", new object[] { typeof(bool) }));
				}
				int num = 0;
				return UnsafeNativeMethods.sqlite3_db_config_int_refint(this._sql, option, ((bool)value) ? 1 : 0, ref num);
			}
			default:
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "unsupported configuration option {0}", new object[] { option }));
			}
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00045744 File Offset: 0x00043944
		internal override void SetLoadExtension(bool bOnOff)
		{
			SQLiteErrorCode sqliteErrorCode;
			if (SQLite3.SQLiteVersionNumber >= 3013000)
			{
				sqliteErrorCode = this.SetConfigurationOption(SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION, bOnOff);
			}
			else
			{
				sqliteErrorCode = UnsafeNativeMethods.sqlite3_enable_load_extension(this._sql, bOnOff ? (-1) : 0);
			}
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000457AC File Offset: 0x000439AC
		internal override void LoadExtension(string fileName, string procName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(fileName + '\0');
				byte[] array = null;
				if (procName != null)
				{
					array = Encoding.UTF8.GetBytes(procName + '\0');
				}
				SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_load_extension(this._sql, bytes, array, ref intPtr);
				if (sqliteErrorCode != SQLiteErrorCode.Ok)
				{
					throw new SQLiteException(sqliteErrorCode, SQLiteConvert.UTF8ToString(intPtr, -1));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.sqlite3_free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00045864 File Offset: 0x00043A64
		internal override void SetExtendedResultCodes(bool bOnOff)
		{
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_extended_result_codes(this._sql, bOnOff ? (-1) : 0);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x000458A8 File Offset: 0x00043AA8
		internal override SQLiteErrorCode ResultCode()
		{
			return UnsafeNativeMethods.sqlite3_errcode(this._sql);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x000458BC File Offset: 0x00043ABC
		internal override SQLiteErrorCode ExtendedResultCode()
		{
			return UnsafeNativeMethods.sqlite3_extended_errcode(this._sql);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x000458D0 File Offset: 0x00043AD0
		internal override void LogMessage(SQLiteErrorCode iErrCode, string zMessage)
		{
			SQLite3.StaticLogMessage(iErrCode, zMessage);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x000458DC File Offset: 0x00043ADC
		internal static void StaticLogMessage(SQLiteErrorCode iErrCode, string zMessage)
		{
			UnsafeNativeMethods.sqlite3_log(iErrCode, SQLiteConvert.ToUTF8(zMessage));
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x000458EC File Offset: 0x00043AEC
		private static int GetLegacyDatabasePageSize(SQLiteConnection connection, string fileName, byte[] passwordBytes, int? pageSize)
		{
			if (pageSize != null)
			{
				return pageSize.Value;
			}
			int num5;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] array = new byte[512];
				int num = array.Length;
				int num2 = fileStream.Read(array, 0, num);
				if (num2 != num)
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "read {0} encrypted page bytes, expected {1} (1)", new object[] { num2, num }));
				}
				byte[] array2 = null;
				SQLite3.DecryptLegacyDatabasePage(connection, passwordBytes, array, ref array2);
				if (array2 == null)
				{
					throw new SQLiteException("failed to decrypt page (1)");
				}
				int num3 = array2.Length;
				if (num3 != num)
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "got {0} decrypted page bytes, expected {1} (1)", new object[] { num3, num }));
				}
				int num4 = 18;
				if (num3 < num4)
				{
					throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "got {0} decrypted page bytes, need at least {1}", new object[] { num3, num4 }));
				}
				num5 = ((int)array2[16] << 8) | (int)array2[17];
			}
			return num5;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00045A4C File Offset: 0x00043C4C
		private static void DecryptLegacyDatabasePage(SQLiteConnection connection, byte[] passwordBytes, byte[] inputBytes, ref byte[] outputBytes)
		{
			using (SQLiteCommand sqliteCommand = connection.CreateCommand())
			{
				sqliteCommand.CommandText = "SELECT cryptoapi_decrypt(?, ?);";
				SQLiteParameter sqliteParameter = sqliteCommand.CreateParameter();
				sqliteParameter.ParameterName = "dataBlob";
				sqliteParameter.DbType = DbType.Binary;
				sqliteParameter.Value = inputBytes;
				sqliteCommand.Parameters.Add(sqliteParameter);
				sqliteParameter = sqliteCommand.CreateParameter();
				sqliteParameter.ParameterName = "passwordBlob";
				sqliteParameter.DbType = DbType.Binary;
				sqliteParameter.Value = passwordBytes;
				sqliteCommand.Parameters.Add(sqliteParameter);
				using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					List<byte> list = null;
					while (sqliteDataReader.Read())
					{
						byte[] array = sqliteDataReader[0] as byte[];
						if (array != null)
						{
							if (list == null)
							{
								list = new List<byte>(array.Length);
							}
							list.AddRange(array);
						}
					}
					if (list != null)
					{
						outputBytes = list.ToArray();
					}
				}
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00045B58 File Offset: 0x00043D58
		private static void DecryptLegacyDatabasePage(SQLiteConnection connection, FileStream inputStream, FileStream outputStream, int pageSize, byte[] passwordBytes, byte[] inputBytes, ref long totalReadCount, ref long totalWriteCount)
		{
			if (inputBytes == null)
			{
				throw new SQLiteException("invalid input buffer");
			}
			int num = inputBytes.Length;
			if (num != pageSize)
			{
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "input buffer is sized {0} bytes, need {1}", new object[] { num, pageSize }));
			}
			Array.Clear(inputBytes, 0, num);
			int num2 = inputStream.Read(inputBytes, 0, pageSize);
			if (num2 != pageSize)
			{
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "read {0} encrypted page bytes, expected {1} (2)", new object[] { num2, pageSize }));
			}
			totalReadCount += (long)num2;
			byte[] array = null;
			SQLite3.DecryptLegacyDatabasePage(connection, passwordBytes, inputBytes, ref array);
			if (array == null)
			{
				throw new SQLiteException("failed to decrypt page (2)");
			}
			int num3 = array.Length;
			if (num3 != num)
			{
				throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "got {0} decrypted page bytes, expected {1} (2)", new object[] { num3, num }));
			}
			outputStream.Write(array, 0, num3);
			totalWriteCount += (long)num3;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00045C80 File Offset: 0x00043E80
		internal static string DecryptLegacyDatabase(string fileName, byte[] passwordBytes, int? pageSize, SQLiteProgressEventHandler progress)
		{
			SQLiteExtra.Verify(null);
			if (string.IsNullOrEmpty(fileName))
			{
				throw new SQLiteException("invalid file name");
			}
			if (!File.Exists(fileName))
			{
				throw new SQLiteException("named file does not exist");
			}
			string text2;
			using (SQLiteConnection sqliteConnection = new SQLiteConnection())
			{
				if (progress != null)
				{
					sqliteConnection.Progress += progress;
				}
				sqliteConnection.ConnectionString = SQLiteCommand.DefaultConnectionString;
				sqliteConnection.Open();
				sqliteConnection.EnableExtensions(true);
				sqliteConnection.LoadExtension(UnsafeNativeMethods.GetNativeModuleFileName(), "sqlite3_cryptoapi_init");
				int legacyDatabasePageSize = SQLite3.GetLegacyDatabasePageSize(sqliteConnection, fileName, passwordBytes, pageSize);
				if (legacyDatabasePageSize == 0)
				{
					throw new SQLiteException("page size cannot be zero");
				}
				if (legacyDatabasePageSize < 512 || legacyDatabasePageSize > 65536)
				{
					throw new SQLiteException("page size out-of-range");
				}
				if ((legacyDatabasePageSize & 1) != 0)
				{
					throw new SQLiteException("page size is odd");
				}
				FileInfo fileInfo = new FileInfo(fileName);
				long length = fileInfo.Length;
				if (length % (long)legacyDatabasePageSize != 0L)
				{
					throw new SQLiteException("data not multiple of page size");
				}
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					string text = string.Format("{0}-decrypted-{1}-{2}", fileName, DateTime.UtcNow.ToString("yyyy_MM_dd"), Environment.TickCount.ToString("X"));
					using (FileStream fileStream2 = new FileStream(text, FileMode.CreateNew, FileAccess.Write, FileShare.None))
					{
						byte[] array = new byte[legacyDatabasePageSize];
						long num = 0L;
						long num2 = 0L;
						while (num < length)
						{
							SQLite3.DecryptLegacyDatabasePage(sqliteConnection, fileStream, fileStream2, legacyDatabasePageSize, passwordBytes, array, ref num, ref num2);
						}
						if (num != length)
						{
							throw new SQLiteException("encrypted data was not totally read");
						}
						if (num2 != num)
						{
							throw new SQLiteException("decrypted data was not totally written");
						}
					}
					text2 = text;
				}
			}
			return text2;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00045E94 File Offset: 0x00044094
		private static void ZeroPassword(byte[] passwordBytes)
		{
			if (passwordBytes == null)
			{
				return;
			}
			for (int i = 0; i < passwordBytes.Length; i++)
			{
				byte b = (byte)((i + 1) % 255);
				passwordBytes[i] = b;
				int num = i;
				passwordBytes[num] ^= b;
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00045EDC File Offset: 0x000440DC
		private static byte[] CalculatePoolHash(string fileName, byte[] passwordBytes, bool asText)
		{
			try
			{
				using (SHA512 sha = SHA512.Create())
				{
					sha.Initialize();
					byte[] array;
					if (fileName != null)
					{
						array = Encoding.Unicode.GetBytes(fileName);
						sha.TransformBlock(array, 0, array.Length, null, 0);
					}
					if (passwordBytes != null)
					{
						sha.TransformBlock(passwordBytes, 0, passwordBytes.Length, null, 0);
					}
					array = BitConverter.GetBytes(asText);
					sha.TransformFinalBlock(array, 0, array.Length);
					return sha.Hash;
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00045F7C File Offset: 0x0004417C
		private static string CalculatePoolFileName(string fileName, byte[] poolHash)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return null;
			}
			if (poolHash == null)
			{
				return null;
			}
			string text = "SQLITE_POOL_HASH:";
			string text2 = HelperMethods.StringFormat(CultureInfo.InvariantCulture, ":{0}", new object[] { Convert.ToBase64String(poolHash) });
			if (fileName.StartsWith(text, StringComparison.Ordinal))
			{
				return null;
			}
			return HelperMethods.StringFormat(CultureInfo.InvariantCulture, "{0}:{1}{2}", new object[] { text, fileName, text2 });
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00045FFC File Offset: 0x000441FC
		private bool TryToUsePool(int maxPoolSize, string fileName, byte[] passwordBytes, bool asText, ref string returnToFileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return false;
			}
			byte[] array = SQLite3.CalculatePoolHash(fileName, passwordBytes, asText);
			if (array == null)
			{
				return false;
			}
			string text = SQLite3.CalculatePoolFileName(fileName, array);
			if (text == null)
			{
				return false;
			}
			bool flag = false;
			SQLiteConnectionHandle sqliteConnectionHandle = null;
			try
			{
				int num;
				sqliteConnectionHandle = SQLiteConnectionPool.Remove(text, maxPoolSize, out num);
				if (sqliteConnectionHandle != null)
				{
					SQLiteConnectionPool.ClearPool(fileName);
					SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.OpenedFromPool, null, null, null, null, sqliteConnectionHandle, text, new object[] { fileName }));
					if (this._sql != null)
					{
						this.UnhookNativeCallbacks(false, true);
						this._sql.Dispose();
						this._sql = null;
						this.FreeDbName(true);
					}
					this._fileName = text;
					this._sql = sqliteConnectionHandle;
					this._poolVersion = num;
					flag = true;
				}
			}
			finally
			{
				if (!flag && sqliteConnectionHandle != null)
				{
					sqliteConnectionHandle.Dispose();
					sqliteConnectionHandle = null;
				}
			}
			returnToFileName = text;
			return flag;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x000460EC File Offset: 0x000442EC
		internal override void SetPassword(byte[] passwordBytes, bool asText)
		{
			string text = null;
			if (this._usePool && this.TryToUsePool(this._maxPoolSize, this._fileName, passwordBytes, asText, ref text))
			{
				if (text != null)
				{
					this._returnToFileName = text;
				}
				this._returnToPool = true;
				return;
			}
			SQLiteExtra.Verify(null);
			int num = (asText ? (-1) : ((passwordBytes == null) ? 0 : passwordBytes.Length));
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_key(this._sql, passwordBytes, num);
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword))
			{
				SQLite3.ZeroPassword(passwordBytes);
			}
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
			if (this._usePool)
			{
				if (text != null)
				{
					this._returnToFileName = text;
				}
				this._usePool = false;
				this._returnToPool = true;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000461C8 File Offset: 0x000443C8
		internal override void ChangePassword(byte[] newPasswordBytes, bool asText)
		{
			SQLiteExtra.Verify(null);
			int num = (asText ? (-1) : ((newPasswordBytes == null) ? 0 : newPasswordBytes.Length));
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_rekey(this._sql, newPasswordBytes, num);
			if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword))
			{
				SQLite3.ZeroPassword(newPasswordBytes);
			}
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
			if (this._usePool)
			{
				this._returnToFileName = this._fileName;
				this._usePool = false;
				this._returnToPool = false;
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00046268 File Offset: 0x00044468
		internal override void SetBusyHook(SQLiteBusyCallback func)
		{
			UnsafeNativeMethods.sqlite3_busy_handler(this._sql, func, IntPtr.Zero);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00046280 File Offset: 0x00044480
		internal override void SetProgressHook(int nOps, SQLiteProgressCallback func)
		{
			UnsafeNativeMethods.sqlite3_progress_handler(this._sql, nOps, func, IntPtr.Zero);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0004629C File Offset: 0x0004449C
		internal override void SetAuthorizerHook(SQLiteAuthorizerCallback func)
		{
			UnsafeNativeMethods.sqlite3_set_authorizer(this._sql, func, IntPtr.Zero);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x000462B8 File Offset: 0x000444B8
		internal override void SetUpdateHook(SQLiteUpdateCallback func)
		{
			UnsafeNativeMethods.sqlite3_update_hook(this._sql, func, IntPtr.Zero);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x000462D4 File Offset: 0x000444D4
		internal override void SetCommitHook(SQLiteCommitCallback func)
		{
			UnsafeNativeMethods.sqlite3_commit_hook(this._sql, func, IntPtr.Zero);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000462F0 File Offset: 0x000444F0
		internal override void SetTraceCallback(SQLiteTraceCallback func)
		{
			UnsafeNativeMethods.sqlite3_trace(this._sql, func, IntPtr.Zero);
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0004630C File Offset: 0x0004450C
		internal override void SetTraceCallback2(SQLiteTraceFlags mask, SQLiteTraceCallback2 func)
		{
			UnsafeNativeMethods.sqlite3_trace_v2(this._sql, mask, func, IntPtr.Zero);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00046328 File Offset: 0x00044528
		internal override void SetRollbackHook(SQLiteRollbackCallback func)
		{
			UnsafeNativeMethods.sqlite3_rollback_hook(this._sql, func, IntPtr.Zero);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00046344 File Offset: 0x00044544
		internal override SQLiteErrorCode SetLogCallback(SQLiteLogCallback func)
		{
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_config_log(SQLiteConfigOpsEnum.SQLITE_CONFIG_LOG, func, IntPtr.Zero);
			if (sqliteErrorCode == SQLiteErrorCode.Ok)
			{
				this._setLogCallback = func != null;
			}
			return sqliteErrorCode;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00046378 File Offset: 0x00044578
		private static void AppendError(StringBuilder builder, string message)
		{
			if (builder == null)
			{
				return;
			}
			builder.AppendLine(message);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0004638C File Offset: 0x0004458C
		private bool UnhookNativeCallbacks(bool includeGlobal, bool canThrow)
		{
			bool flag = true;
			SQLiteErrorCode sqliteErrorCode = SQLiteErrorCode.Ok;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				this.SetRollbackHook(null);
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset rollback hook");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			try
			{
				if (UnsafeNativeMethods.sqlite3_libversion_number() >= 3014000)
				{
					this.SetTraceCallback2(SQLiteTraceFlags.SQLITE_TRACE_NONE, null);
				}
				else
				{
					this.SetTraceCallback(null);
				}
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset trace callback");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			try
			{
				this.SetCommitHook(null);
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset commit hook");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			try
			{
				this.SetUpdateHook(null);
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset update hook");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			try
			{
				this.SetAuthorizerHook(null);
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset authorizer hook");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			try
			{
				this.SetBusyHook(null);
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset busy hook");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			try
			{
				this.SetProgressHook(0, null);
			}
			catch (Exception)
			{
				SQLite3.AppendError(stringBuilder, "failed to unset progress hook");
				sqliteErrorCode = SQLiteErrorCode.Error;
				flag = false;
			}
			if (includeGlobal && this._setLogCallback)
			{
				try
				{
					SQLiteErrorCode sqliteErrorCode2 = this.SetLogCallback(null);
					if (sqliteErrorCode2 != SQLiteErrorCode.Ok)
					{
						SQLite3.AppendError(stringBuilder, "could not unset log callback");
						sqliteErrorCode = sqliteErrorCode2;
						flag = false;
					}
				}
				catch (Exception)
				{
					SQLite3.AppendError(stringBuilder, "failed to unset log callback");
					sqliteErrorCode = SQLiteErrorCode.Error;
					flag = false;
				}
			}
			if (!flag && canThrow)
			{
				throw new SQLiteException(sqliteErrorCode, stringBuilder.ToString());
			}
			return flag;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00046570 File Offset: 0x00044770
		private bool FreeDbName(bool canThrow)
		{
			try
			{
				if (this.dbName != IntPtr.Zero)
				{
					SQLiteMemory.Free(this.dbName);
					this.dbName = IntPtr.Zero;
				}
				return true;
			}
			catch (Exception)
			{
				if (canThrow)
				{
					throw;
				}
			}
			return false;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000465D0 File Offset: 0x000447D0
		internal override SQLiteBackup InitializeBackup(SQLiteConnection destCnn, string destName, string sourceName)
		{
			if (destCnn == null)
			{
				throw new ArgumentNullException("destCnn");
			}
			if (destName == null)
			{
				throw new ArgumentNullException("destName");
			}
			if (sourceName == null)
			{
				throw new ArgumentNullException("sourceName");
			}
			SQLite3 sqlite = destCnn._sql as SQLite3;
			if (sqlite == null)
			{
				throw new ArgumentException("Destination connection has no wrapper.", "destCnn");
			}
			SQLiteConnectionHandle sql = sqlite._sql;
			if (sql == null)
			{
				throw new ArgumentException("Destination connection has an invalid handle.", "destCnn");
			}
			SQLiteConnectionHandle sql2 = this._sql;
			if (sql2 == null)
			{
				throw new InvalidOperationException("Source connection has an invalid handle.");
			}
			byte[] array = SQLiteConvert.ToUTF8(destName);
			byte[] array2 = SQLiteConvert.ToUTF8(sourceName);
			SQLiteBackupHandle sqliteBackupHandle = null;
			try
			{
			}
			finally
			{
				IntPtr intPtr = UnsafeNativeMethods.sqlite3_backup_init(sql, array, sql2, array2);
				if (intPtr == IntPtr.Zero)
				{
					SQLiteErrorCode sqliteErrorCode = this.ResultCode();
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, this.GetLastError());
					}
					throw new SQLiteException("failed to initialize backup");
				}
				else
				{
					sqliteBackupHandle = new SQLiteBackupHandle(sql, intPtr);
				}
			}
			SQLiteConnection.OnChanged(null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, null, null, null, null, sqliteBackupHandle, null, new object[]
			{
				typeof(SQLite3),
				destCnn,
				destName,
				sourceName
			}));
			return new SQLiteBackup(this, sqliteBackupHandle, sql, array, sql2, array2);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0004673C File Offset: 0x0004493C
		internal override bool StepBackup(SQLiteBackup backup, int nPage, ref bool retry)
		{
			retry = false;
			if (backup == null)
			{
				throw new ArgumentNullException("backup");
			}
			SQLiteBackupHandle sqlite_backup = backup._sqlite_backup;
			if (sqlite_backup == null)
			{
				throw new InvalidOperationException("Backup object has an invalid handle.");
			}
			IntPtr intPtr = sqlite_backup;
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("Backup object has an invalid handle pointer.");
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_backup_step(intPtr, nPage);
			backup._stepResult = sqliteErrorCode;
			if (sqliteErrorCode == SQLiteErrorCode.Ok)
			{
				return true;
			}
			if (sqliteErrorCode == SQLiteErrorCode.Busy)
			{
				retry = true;
				return true;
			}
			if (sqliteErrorCode == SQLiteErrorCode.Locked)
			{
				retry = true;
				return true;
			}
			if (sqliteErrorCode == SQLiteErrorCode.Done)
			{
				return false;
			}
			throw new SQLiteException(sqliteErrorCode, this.GetLastError());
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x000467E0 File Offset: 0x000449E0
		internal override int RemainingBackup(SQLiteBackup backup)
		{
			if (backup == null)
			{
				throw new ArgumentNullException("backup");
			}
			SQLiteBackupHandle sqlite_backup = backup._sqlite_backup;
			if (sqlite_backup == null)
			{
				throw new InvalidOperationException("Backup object has an invalid handle.");
			}
			IntPtr intPtr = sqlite_backup;
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("Backup object has an invalid handle pointer.");
			}
			return UnsafeNativeMethods.sqlite3_backup_remaining(intPtr);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00046844 File Offset: 0x00044A44
		internal override int PageCountBackup(SQLiteBackup backup)
		{
			if (backup == null)
			{
				throw new ArgumentNullException("backup");
			}
			SQLiteBackupHandle sqlite_backup = backup._sqlite_backup;
			if (sqlite_backup == null)
			{
				throw new InvalidOperationException("Backup object has an invalid handle.");
			}
			IntPtr intPtr = sqlite_backup;
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("Backup object has an invalid handle pointer.");
			}
			return UnsafeNativeMethods.sqlite3_backup_pagecount(intPtr);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000468A8 File Offset: 0x00044AA8
		internal override void FinishBackup(SQLiteBackup backup)
		{
			if (backup == null)
			{
				throw new ArgumentNullException("backup");
			}
			SQLiteBackupHandle sqlite_backup = backup._sqlite_backup;
			if (sqlite_backup == null)
			{
				throw new InvalidOperationException("Backup object has an invalid handle.");
			}
			IntPtr intPtr = sqlite_backup;
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("Backup object has an invalid handle pointer.");
			}
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_backup_finish_interop(intPtr);
			sqlite_backup.SetHandleAsInvalid();
			if (sqliteErrorCode != SQLiteErrorCode.Ok && sqliteErrorCode != backup._stepResult)
			{
				throw new SQLiteException(sqliteErrorCode, this.GetLastError());
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00046930 File Offset: 0x00044B30
		internal override bool IsInitialized()
		{
			return SQLite3.StaticIsInitialized();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00046938 File Offset: 0x00044B38
		internal static bool StaticIsInitialized()
		{
			bool flag2;
			lock (SQLite3.syncRoot)
			{
				bool internalEnabled = SQLiteLog.InternalEnabled;
				SQLiteLog.InternalEnabled = false;
				try
				{
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_config_none(SQLiteConfigOpsEnum.SQLITE_CONFIG_NONE);
					flag2 = sqliteErrorCode == SQLiteErrorCode.Misuse;
				}
				finally
				{
					SQLiteLog.InternalEnabled = internalEnabled;
				}
			}
			return flag2;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000469A8 File Offset: 0x00044BA8
		internal override object GetValue(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, SQLiteType typ)
		{
			TypeAffinity typeAffinity = typ.Affinity;
			if (typeAffinity == TypeAffinity.Null)
			{
				return DBNull.Value;
			}
			Type type = null;
			if (typ.Type != DbType.Object)
			{
				type = SQLiteConvert.SQLiteTypeToType(typ);
				typeAffinity = SQLiteConvert.TypeToAffinity(type, flags);
			}
			if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetAllAsText))
			{
				return this.GetText(stmt, index);
			}
			TypeAffinity typeAffinity2 = typeAffinity;
			switch (typeAffinity2)
			{
			case TypeAffinity.Int64:
				if (type == null)
				{
					return this.GetInt64(stmt, index);
				}
				if (type == typeof(bool))
				{
					return this.GetBoolean(stmt, index);
				}
				if (type == typeof(sbyte))
				{
					return this.GetSByte(stmt, index);
				}
				if (type == typeof(byte))
				{
					return this.GetByte(stmt, index);
				}
				if (type == typeof(short))
				{
					return this.GetInt16(stmt, index);
				}
				if (type == typeof(ushort))
				{
					return this.GetUInt16(stmt, index);
				}
				if (type == typeof(int))
				{
					return this.GetInt32(stmt, index);
				}
				if (type == typeof(uint))
				{
					return this.GetUInt32(stmt, index);
				}
				if (type == typeof(long))
				{
					return this.GetInt64(stmt, index);
				}
				if (type == typeof(ulong))
				{
					return this.GetUInt64(stmt, index);
				}
				return Convert.ChangeType(this.GetInt64(stmt, index), type, HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetInvariantInt64) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			case TypeAffinity.Double:
				if (type == null)
				{
					return this.GetDouble(stmt, index);
				}
				return Convert.ChangeType(this.GetDouble(stmt, index), type, HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetInvariantDouble) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			case TypeAffinity.Text:
				break;
			case TypeAffinity.Blob:
			{
				if (typ.Type == DbType.Guid && typ.Affinity == TypeAffinity.Text)
				{
					return new Guid(this.GetText(stmt, index));
				}
				int num = (int)this.GetBytes(stmt, index, 0, null, 0, 0);
				byte[] array = new byte[num];
				this.GetBytes(stmt, index, 0, array, 0, num);
				if (typ.Type == DbType.Guid && num == 16)
				{
					return new Guid(array);
				}
				return array;
			}
			default:
				if (typeAffinity2 == TypeAffinity.DateTime)
				{
					return this.GetDateTime(stmt, index);
				}
				break;
			}
			return this.GetText(stmt, index);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00046C9C File Offset: 0x00044E9C
		internal override int GetCursorForTable(SQLiteStatement stmt, int db, int rootPage)
		{
			return UnsafeNativeMethods.sqlite3_table_cursor_interop(stmt._sqlite_stmt, db, rootPage);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00046CB0 File Offset: 0x00044EB0
		internal override long GetRowIdForCursor(SQLiteStatement stmt, int cursor)
		{
			long num = 0L;
			if (UnsafeNativeMethods.sqlite3_cursor_rowid_interop(stmt._sqlite_stmt, cursor, ref num) == SQLiteErrorCode.Ok)
			{
				return num;
			}
			return 0L;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00046CE4 File Offset: 0x00044EE4
		internal override void GetIndexColumnExtendedInfo(string database, string index, string column, ref int sortMode, ref int onError, ref string collationSequence)
		{
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_index_column_info_interop(this._sql, SQLiteConvert.ToUTF8(database), SQLiteConvert.ToUTF8(index), SQLiteConvert.ToUTF8(column), ref sortMode, ref onError, ref zero, ref num);
			if (sqliteErrorCode != SQLiteErrorCode.Ok)
			{
				throw new SQLiteException(sqliteErrorCode, null);
			}
			collationSequence = SQLiteConvert.UTF8ToString(zero, num);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00046D40 File Offset: 0x00044F40
		internal override SQLiteErrorCode FileControl(string zDbName, int op, IntPtr pArg)
		{
			return UnsafeNativeMethods.sqlite3_file_control(this._sql, (zDbName != null) ? SQLiteConvert.ToUTF8(zDbName) : null, op, pArg);
		}

		// Token: 0x04000539 RID: 1337
		internal const string PublicKey = "002400000480000094000000060200000024000052534131000400000100010005a288de5687c4e1b621ddff5d844727418956997f475eb829429e411aff3e93f97b70de698b972640925bdd44280df0a25a843266973704137cbb0e7441c1fe7cae4e2440ae91ab8cde3933febcb1ac48dd33b40e13c421d8215c18a4349a436dd499e3c385cc683015f886f6c10bd90115eb2bd61b67750839e3a19941dc9c";

		// Token: 0x0400053A RID: 1338
		internal const string DesignerVersion = "1.0.116.0";

		// Token: 0x0400053B RID: 1339
		private const string PoolHashFileNamePrefix = "SQLITE_POOL_HASH:";

		// Token: 0x0400053C RID: 1340
		private const int MINIMUM_PAGE_SIZE = 512;

		// Token: 0x0400053D RID: 1341
		private const int MAXIMUM_PAGE_SIZE = 65536;

		// Token: 0x0400053E RID: 1342
		private const int PAGE_SIZE_OFFSET = 16;

		// Token: 0x0400053F RID: 1343
		private static object syncRoot = new object();

		// Token: 0x04000540 RID: 1344
		private IntPtr dbName = IntPtr.Zero;

		// Token: 0x04000541 RID: 1345
		protected internal SQLiteConnectionHandle _sql;

		// Token: 0x04000542 RID: 1346
		protected string _fileName;

		// Token: 0x04000543 RID: 1347
		protected string _returnToFileName;

		// Token: 0x04000544 RID: 1348
		protected int _maxPoolSize;

		// Token: 0x04000545 RID: 1349
		protected SQLiteConnectionFlags _flags;

		// Token: 0x04000546 RID: 1350
		private bool _setLogCallback;

		// Token: 0x04000547 RID: 1351
		protected bool _usePool;

		// Token: 0x04000548 RID: 1352
		private bool _returnToPool;

		// Token: 0x04000549 RID: 1353
		protected int _poolVersion;

		// Token: 0x0400054A RID: 1354
		private int _cancelCount;

		// Token: 0x0400054B RID: 1355
		private bool _buildingSchema;

		// Token: 0x0400054C RID: 1356
		protected Dictionary<SQLiteFunctionAttribute, SQLiteFunction> _functions;

		// Token: 0x0400054D RID: 1357
		protected string _shimExtensionFileName;

		// Token: 0x0400054E RID: 1358
		protected bool? _shimIsLoadNeeded = null;

		// Token: 0x0400054F RID: 1359
		protected string _shimExtensionProcName = "sqlite3_vtshim_init";

		// Token: 0x04000550 RID: 1360
		protected Dictionary<string, SQLiteModule> _modules;

		// Token: 0x04000551 RID: 1361
		private bool _forceLogPrepare;

		// Token: 0x04000552 RID: 1362
		private bool disposed;

		// Token: 0x04000553 RID: 1363
		private static bool? have_errstr = null;

		// Token: 0x04000554 RID: 1364
		private static bool? have_stmt_readonly = null;

		// Token: 0x04000555 RID: 1365
		private static bool? forceLogLifecycle = null;
	}
}
