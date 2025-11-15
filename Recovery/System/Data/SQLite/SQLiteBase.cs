using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x0200014C RID: 332
	internal abstract class SQLiteBase : SQLiteConvert, IDisposable
	{
		// Token: 0x06000DF0 RID: 3568 RVA: 0x00041E8C File Offset: 0x0004008C
		internal SQLiteBase(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString)
			: base(fmt, kind, fmtString)
		{
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000DF1 RID: 3569
		internal abstract string Version { get; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000DF2 RID: 3570
		internal abstract int VersionNumber { get; }

		// Token: 0x06000DF3 RID: 3571
		internal abstract bool IsReadOnly(string name);

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000DF4 RID: 3572
		internal abstract long LastInsertRowId { get; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000DF5 RID: 3573
		internal abstract int Changes { get; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000DF6 RID: 3574
		internal abstract long MemoryUsed { get; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000DF7 RID: 3575
		internal abstract long MemoryHighwater { get; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000DF8 RID: 3576
		internal abstract bool OwnHandle { get; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000DF9 RID: 3577
		internal abstract bool ForceLogPrepare { get; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000DFA RID: 3578
		internal abstract IDictionary<SQLiteFunctionAttribute, SQLiteFunction> Functions { get; }

		// Token: 0x06000DFB RID: 3579
		internal abstract SQLiteErrorCode SetMemoryStatus(bool value);

		// Token: 0x06000DFC RID: 3580
		internal abstract SQLiteErrorCode ReleaseMemory();

		// Token: 0x06000DFD RID: 3581
		internal abstract SQLiteErrorCode Shutdown();

		// Token: 0x06000DFE RID: 3582
		internal abstract bool IsOpen();

		// Token: 0x06000DFF RID: 3583
		internal abstract string GetFileName(string dbName);

		// Token: 0x06000E00 RID: 3584
		internal abstract void Open(string strFilename, string vfsName, SQLiteConnectionFlags connectionFlags, SQLiteOpenFlagsEnum openFlags, int maxPoolSize, bool usePool);

		// Token: 0x06000E01 RID: 3585
		internal abstract bool Close(bool disposing);

		// Token: 0x06000E02 RID: 3586
		internal abstract void SetTimeout(int nTimeoutMS);

		// Token: 0x06000E03 RID: 3587
		internal abstract string GetLastError();

		// Token: 0x06000E04 RID: 3588
		internal abstract string GetLastError(string defValue);

		// Token: 0x06000E05 RID: 3589
		internal abstract void ClearPool();

		// Token: 0x06000E06 RID: 3590
		internal abstract int CountPool();

		// Token: 0x06000E07 RID: 3591
		internal abstract SQLiteStatement Prepare(SQLiteConnection cnn, string strSql, SQLiteStatement previous, uint timeoutMS, ref string strRemain);

		// Token: 0x06000E08 RID: 3592
		internal abstract bool Step(SQLiteStatement stmt);

		// Token: 0x06000E09 RID: 3593
		internal abstract bool IsReadOnly(SQLiteStatement stmt);

		// Token: 0x06000E0A RID: 3594
		internal abstract SQLiteErrorCode Reset(SQLiteStatement stmt);

		// Token: 0x06000E0B RID: 3595
		internal abstract void Cancel();

		// Token: 0x06000E0C RID: 3596
		internal abstract void BindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteFunction function, SQLiteConnectionFlags flags);

		// Token: 0x06000E0D RID: 3597
		internal abstract bool UnbindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteConnectionFlags flags);

		// Token: 0x06000E0E RID: 3598
		internal abstract void Bind_Double(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, double value);

		// Token: 0x06000E0F RID: 3599
		internal abstract void Bind_Int32(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, int value);

		// Token: 0x06000E10 RID: 3600
		internal abstract void Bind_UInt32(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, uint value);

		// Token: 0x06000E11 RID: 3601
		internal abstract void Bind_Int64(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, long value);

		// Token: 0x06000E12 RID: 3602
		internal abstract void Bind_UInt64(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, ulong value);

		// Token: 0x06000E13 RID: 3603
		internal abstract void Bind_Boolean(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, bool value);

		// Token: 0x06000E14 RID: 3604
		internal abstract void Bind_Text(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, string value);

		// Token: 0x06000E15 RID: 3605
		internal abstract void Bind_Blob(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, byte[] blobData);

		// Token: 0x06000E16 RID: 3606
		internal abstract void Bind_DateTime(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, DateTime dt);

		// Token: 0x06000E17 RID: 3607
		internal abstract void Bind_Null(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index);

		// Token: 0x06000E18 RID: 3608
		internal abstract int Bind_ParamCount(SQLiteStatement stmt, SQLiteConnectionFlags flags);

		// Token: 0x06000E19 RID: 3609
		internal abstract string Bind_ParamName(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index);

		// Token: 0x06000E1A RID: 3610
		internal abstract int Bind_ParamIndex(SQLiteStatement stmt, SQLiteConnectionFlags flags, string paramName);

		// Token: 0x06000E1B RID: 3611
		internal abstract int ColumnCount(SQLiteStatement stmt);

		// Token: 0x06000E1C RID: 3612
		internal abstract string ColumnName(SQLiteStatement stmt, int index);

		// Token: 0x06000E1D RID: 3613
		internal abstract TypeAffinity ColumnAffinity(SQLiteStatement stmt, int index);

		// Token: 0x06000E1E RID: 3614
		internal abstract string ColumnType(SQLiteStatement stmt, int index, ref TypeAffinity nAffinity);

		// Token: 0x06000E1F RID: 3615
		internal abstract int ColumnIndex(SQLiteStatement stmt, string columnName);

		// Token: 0x06000E20 RID: 3616
		internal abstract string ColumnOriginalName(SQLiteStatement stmt, int index);

		// Token: 0x06000E21 RID: 3617
		internal abstract string ColumnDatabaseName(SQLiteStatement stmt, int index);

		// Token: 0x06000E22 RID: 3618
		internal abstract string ColumnTableName(SQLiteStatement stmt, int index);

		// Token: 0x06000E23 RID: 3619
		internal abstract bool DoesTableExist(string dataBase, string table);

		// Token: 0x06000E24 RID: 3620
		internal abstract bool ColumnMetaData(string dataBase, string table, string column, bool canThrow, ref string dataType, ref string collateSequence, ref bool notNull, ref bool primaryKey, ref bool autoIncrement);

		// Token: 0x06000E25 RID: 3621
		internal abstract void GetIndexColumnExtendedInfo(string database, string index, string column, ref int sortMode, ref int onError, ref string collationSequence);

		// Token: 0x06000E26 RID: 3622
		internal abstract object GetObject(SQLiteStatement stmt, int index);

		// Token: 0x06000E27 RID: 3623
		internal abstract double GetDouble(SQLiteStatement stmt, int index);

		// Token: 0x06000E28 RID: 3624
		internal abstract bool GetBoolean(SQLiteStatement stmt, int index);

		// Token: 0x06000E29 RID: 3625
		internal abstract sbyte GetSByte(SQLiteStatement stmt, int index);

		// Token: 0x06000E2A RID: 3626
		internal abstract byte GetByte(SQLiteStatement stmt, int index);

		// Token: 0x06000E2B RID: 3627
		internal abstract short GetInt16(SQLiteStatement stmt, int index);

		// Token: 0x06000E2C RID: 3628
		internal abstract ushort GetUInt16(SQLiteStatement stmt, int index);

		// Token: 0x06000E2D RID: 3629
		internal abstract int GetInt32(SQLiteStatement stmt, int index);

		// Token: 0x06000E2E RID: 3630
		internal abstract uint GetUInt32(SQLiteStatement stmt, int index);

		// Token: 0x06000E2F RID: 3631
		internal abstract long GetInt64(SQLiteStatement stmt, int index);

		// Token: 0x06000E30 RID: 3632
		internal abstract ulong GetUInt64(SQLiteStatement stmt, int index);

		// Token: 0x06000E31 RID: 3633
		internal abstract string GetText(SQLiteStatement stmt, int index);

		// Token: 0x06000E32 RID: 3634
		internal abstract long GetBytes(SQLiteStatement stmt, int index, int nDataoffset, byte[] bDest, int nStart, int nLength);

		// Token: 0x06000E33 RID: 3635
		internal abstract char GetChar(SQLiteStatement stmt, int index);

		// Token: 0x06000E34 RID: 3636
		internal abstract long GetChars(SQLiteStatement stmt, int index, int nDataoffset, char[] bDest, int nStart, int nLength);

		// Token: 0x06000E35 RID: 3637
		internal abstract DateTime GetDateTime(SQLiteStatement stmt, int index);

		// Token: 0x06000E36 RID: 3638
		internal abstract bool IsNull(SQLiteStatement stmt, int index);

		// Token: 0x06000E37 RID: 3639
		internal abstract SQLiteErrorCode CreateCollation(string strCollation, SQLiteCollation func, SQLiteCollation func16, bool @throw);

		// Token: 0x06000E38 RID: 3640
		internal abstract SQLiteErrorCode CreateFunction(string strFunction, int nArgs, bool needCollSeq, SQLiteCallback func, SQLiteCallback funcstep, SQLiteFinalCallback funcfinal, bool @throw);

		// Token: 0x06000E39 RID: 3641
		internal abstract CollationSequence GetCollationSequence(SQLiteFunction func, IntPtr context);

		// Token: 0x06000E3A RID: 3642
		internal abstract int ContextCollateCompare(CollationEncodingEnum enc, IntPtr context, string s1, string s2);

		// Token: 0x06000E3B RID: 3643
		internal abstract int ContextCollateCompare(CollationEncodingEnum enc, IntPtr context, char[] c1, char[] c2);

		// Token: 0x06000E3C RID: 3644
		internal abstract int AggregateCount(IntPtr context);

		// Token: 0x06000E3D RID: 3645
		internal abstract IntPtr AggregateContext(IntPtr context);

		// Token: 0x06000E3E RID: 3646
		internal abstract long GetParamValueBytes(IntPtr ptr, int nDataOffset, byte[] bDest, int nStart, int nLength);

		// Token: 0x06000E3F RID: 3647
		internal abstract double GetParamValueDouble(IntPtr ptr);

		// Token: 0x06000E40 RID: 3648
		internal abstract int GetParamValueInt32(IntPtr ptr);

		// Token: 0x06000E41 RID: 3649
		internal abstract long GetParamValueInt64(IntPtr ptr);

		// Token: 0x06000E42 RID: 3650
		internal abstract string GetParamValueText(IntPtr ptr);

		// Token: 0x06000E43 RID: 3651
		internal abstract TypeAffinity GetParamValueType(IntPtr ptr);

		// Token: 0x06000E44 RID: 3652
		internal abstract void ReturnBlob(IntPtr context, byte[] value);

		// Token: 0x06000E45 RID: 3653
		internal abstract void ReturnDouble(IntPtr context, double value);

		// Token: 0x06000E46 RID: 3654
		internal abstract void ReturnError(IntPtr context, string value);

		// Token: 0x06000E47 RID: 3655
		internal abstract void ReturnInt32(IntPtr context, int value);

		// Token: 0x06000E48 RID: 3656
		internal abstract void ReturnInt64(IntPtr context, long value);

		// Token: 0x06000E49 RID: 3657
		internal abstract void ReturnNull(IntPtr context);

		// Token: 0x06000E4A RID: 3658
		internal abstract void ReturnText(IntPtr context, string value);

		// Token: 0x06000E4B RID: 3659
		internal abstract void CreateModule(SQLiteModule module, SQLiteConnectionFlags flags);

		// Token: 0x06000E4C RID: 3660
		internal abstract void DisposeModule(SQLiteModule module, SQLiteConnectionFlags flags);

		// Token: 0x06000E4D RID: 3661
		internal abstract SQLiteErrorCode DeclareVirtualTable(SQLiteModule module, string strSql, ref string error);

		// Token: 0x06000E4E RID: 3662
		internal abstract SQLiteErrorCode DeclareVirtualFunction(SQLiteModule module, int argumentCount, string name, ref string error);

		// Token: 0x06000E4F RID: 3663
		internal abstract SQLiteErrorCode GetStatusParameter(SQLiteStatusOpsEnum option, bool reset, ref int current, ref int highwater);

		// Token: 0x06000E50 RID: 3664
		internal abstract int SetLimitOption(SQLiteLimitOpsEnum option, int value);

		// Token: 0x06000E51 RID: 3665
		internal abstract SQLiteErrorCode SetConfigurationOption(SQLiteConfigDbOpsEnum option, object value);

		// Token: 0x06000E52 RID: 3666
		internal abstract void SetLoadExtension(bool bOnOff);

		// Token: 0x06000E53 RID: 3667
		internal abstract void LoadExtension(string fileName, string procName);

		// Token: 0x06000E54 RID: 3668
		internal abstract void SetExtendedResultCodes(bool bOnOff);

		// Token: 0x06000E55 RID: 3669
		internal abstract SQLiteErrorCode ResultCode();

		// Token: 0x06000E56 RID: 3670
		internal abstract SQLiteErrorCode ExtendedResultCode();

		// Token: 0x06000E57 RID: 3671
		internal abstract void LogMessage(SQLiteErrorCode iErrCode, string zMessage);

		// Token: 0x06000E58 RID: 3672
		internal abstract void SetPassword(byte[] passwordBytes, bool asText);

		// Token: 0x06000E59 RID: 3673
		internal abstract void ChangePassword(byte[] newPasswordBytes, bool asText);

		// Token: 0x06000E5A RID: 3674
		internal abstract void SetBusyHook(SQLiteBusyCallback func);

		// Token: 0x06000E5B RID: 3675
		internal abstract void SetProgressHook(int nOps, SQLiteProgressCallback func);

		// Token: 0x06000E5C RID: 3676
		internal abstract void SetAuthorizerHook(SQLiteAuthorizerCallback func);

		// Token: 0x06000E5D RID: 3677
		internal abstract void SetUpdateHook(SQLiteUpdateCallback func);

		// Token: 0x06000E5E RID: 3678
		internal abstract void SetCommitHook(SQLiteCommitCallback func);

		// Token: 0x06000E5F RID: 3679
		internal abstract void SetTraceCallback(SQLiteTraceCallback func);

		// Token: 0x06000E60 RID: 3680
		internal abstract void SetTraceCallback2(SQLiteTraceFlags mask, SQLiteTraceCallback2 func);

		// Token: 0x06000E61 RID: 3681
		internal abstract void SetRollbackHook(SQLiteRollbackCallback func);

		// Token: 0x06000E62 RID: 3682
		internal abstract SQLiteErrorCode SetLogCallback(SQLiteLogCallback func);

		// Token: 0x06000E63 RID: 3683
		internal abstract bool IsInitialized();

		// Token: 0x06000E64 RID: 3684
		internal abstract int GetCursorForTable(SQLiteStatement stmt, int database, int rootPage);

		// Token: 0x06000E65 RID: 3685
		internal abstract long GetRowIdForCursor(SQLiteStatement stmt, int cursor);

		// Token: 0x06000E66 RID: 3686
		internal abstract object GetValue(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index, SQLiteType typ);

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000E67 RID: 3687
		internal abstract bool AutoCommit { get; }

		// Token: 0x06000E68 RID: 3688
		internal abstract SQLiteErrorCode FileControl(string zDbName, int op, IntPtr pArg);

		// Token: 0x06000E69 RID: 3689
		internal abstract SQLiteBackup InitializeBackup(SQLiteConnection destCnn, string destName, string sourceName);

		// Token: 0x06000E6A RID: 3690
		internal abstract bool StepBackup(SQLiteBackup backup, int nPage, ref bool retry);

		// Token: 0x06000E6B RID: 3691
		internal abstract int RemainingBackup(SQLiteBackup backup);

		// Token: 0x06000E6C RID: 3692
		internal abstract int PageCountBackup(SQLiteBackup backup);

		// Token: 0x06000E6D RID: 3693
		internal abstract void FinishBackup(SQLiteBackup backup);

		// Token: 0x06000E6E RID: 3694 RVA: 0x00041E98 File Offset: 0x00040098
		protected static long BumpCreateCount()
		{
			return Interlocked.Increment(ref SQLiteBase._createCount);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00041EA4 File Offset: 0x000400A4
		protected static long BumpOpenCount()
		{
			return Interlocked.Increment(ref SQLiteBase._openCount);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00041EB0 File Offset: 0x000400B0
		protected static long BumpCloseCount()
		{
			return Interlocked.Increment(ref SQLiteBase._closeCount);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00041EBC File Offset: 0x000400BC
		protected static long BumpDisposeCount()
		{
			return Interlocked.Increment(ref SQLiteBase._disposeCount);
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00041EC8 File Offset: 0x000400C8
		internal static long CreateCount
		{
			get
			{
				return Interlocked.CompareExchange(ref SQLiteBase._createCount, 0L, 0L);
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00041ED8 File Offset: 0x000400D8
		internal static long OpenCount
		{
			get
			{
				return Interlocked.CompareExchange(ref SQLiteBase._openCount, 0L, 0L);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00041EE8 File Offset: 0x000400E8
		internal static long CloseCount
		{
			get
			{
				return Interlocked.CompareExchange(ref SQLiteBase._closeCount, 0L, 0L);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00041EF8 File Offset: 0x000400F8
		internal static long DisposeCount
		{
			get
			{
				return Interlocked.CompareExchange(ref SQLiteBase._disposeCount, 0L, 0L);
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00041F08 File Offset: 0x00040108
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00041F18 File Offset: 0x00040118
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteBase).Name);
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00041F3C File Offset: 0x0004013C
		protected virtual void Dispose(bool disposing)
		{
			if (this.wasDisposed)
			{
				SQLiteBase.BumpDisposeCount();
			}
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00041F64 File Offset: 0x00040164
		~SQLiteBase()
		{
			this.Dispose(false);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00041F94 File Offset: 0x00040194
		protected static string FallbackGetErrorString(SQLiteErrorCode rc)
		{
			switch (rc)
			{
			case SQLiteErrorCode.Row:
				return "another row available";
			case SQLiteErrorCode.Done:
				return "no more rows available";
			default:
			{
				if (rc == SQLiteErrorCode.Abort_Rollback)
				{
					return "abort due to ROLLBACK";
				}
				if (SQLiteBase._errorMessages == null)
				{
					return null;
				}
				int num = (int)(rc & SQLiteErrorCode.NonExtendedMask);
				if (num < 0 || num >= SQLiteBase._errorMessages.Length)
				{
					num = 1;
				}
				return SQLiteBase._errorMessages[num];
			}
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0004200C File Offset: 0x0004020C
		internal static string GetLastError(SQLiteConnectionHandle hdl, IntPtr db)
		{
			if (hdl == null || db == IntPtr.Zero)
			{
				return "null connection or database handle";
			}
			string text = null;
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					if (!hdl.IsInvalid && !hdl.IsClosed)
					{
						int num = 0;
						text = SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_errmsg_interop(db, ref num), num);
					}
					else
					{
						text = "closed or invalid connection handle";
					}
				}
			}
			GC.KeepAlive(hdl);
			return text;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x000420B0 File Offset: 0x000402B0
		internal static void FinishBackup(SQLiteConnectionHandle hdl, IntPtr backup)
		{
			if (hdl == null || backup == IntPtr.Zero)
			{
				return;
			}
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_backup_finish_interop(backup);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, null);
					}
				}
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0004212C File Offset: 0x0004032C
		internal static void CloseBlob(SQLiteConnectionHandle hdl, IntPtr blob)
		{
			if (hdl == null || blob == IntPtr.Zero)
			{
				return;
			}
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_blob_close_interop(blob);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, null);
					}
				}
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000421A8 File Offset: 0x000403A8
		internal static void FinalizeStatement(SQLiteConnectionHandle hdl, IntPtr stmt)
		{
			if (hdl == null || stmt == IntPtr.Zero)
			{
				return;
			}
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_finalize_interop(stmt);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, null);
					}
				}
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00042224 File Offset: 0x00040424
		internal static void CloseConnection(SQLiteConnectionHandle hdl, IntPtr db)
		{
			if (hdl == null || db == IntPtr.Zero)
			{
				return;
			}
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_close_interop(db);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, SQLiteBase.GetLastError(hdl, db));
					}
				}
			}
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000422A4 File Offset: 0x000404A4
		internal static void CloseConnectionV2(SQLiteConnectionHandle hdl, IntPtr db)
		{
			if (hdl == null || db == IntPtr.Zero)
			{
				return;
			}
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_close_interop(db);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, SQLiteBase.GetLastError(hdl, db));
					}
				}
			}
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00042324 File Offset: 0x00040524
		internal static bool ResetConnection(SQLiteConnectionHandle hdl, IntPtr db, bool canThrow)
		{
			if (hdl == null || db == IntPtr.Zero)
			{
				return false;
			}
			bool flag = false;
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					if (canThrow && hdl.IsInvalid)
					{
						throw new InvalidOperationException("The connection handle is invalid.");
					}
					if (canThrow && hdl.IsClosed)
					{
						throw new InvalidOperationException("The connection handle is closed.");
					}
					if (!hdl.IsInvalid && !hdl.IsClosed)
					{
						IntPtr intPtr = IntPtr.Zero;
						do
						{
							intPtr = UnsafeNativeMethods.sqlite3_next_stmt(db, intPtr);
							if (intPtr != IntPtr.Zero)
							{
								SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_reset_interop(intPtr);
							}
						}
						while (intPtr != IntPtr.Zero);
						if (SQLiteBase.IsAutocommit(hdl, db))
						{
							flag = true;
						}
						else
						{
							SQLiteErrorCode sqliteErrorCode = UnsafeNativeMethods.sqlite3_exec(db, SQLiteConvert.ToUTF8("ROLLBACK"), IntPtr.Zero, IntPtr.Zero, ref intPtr);
							if (sqliteErrorCode == SQLiteErrorCode.Ok)
							{
								flag = true;
							}
							else if (canThrow)
							{
								throw new SQLiteException(sqliteErrorCode, SQLiteBase.GetLastError(hdl, db));
							}
						}
					}
				}
			}
			GC.KeepAlive(hdl);
			return flag;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00042468 File Offset: 0x00040668
		internal static bool IsAutocommit(SQLiteConnectionHandle hdl, IntPtr db)
		{
			if (hdl == null || db == IntPtr.Zero)
			{
				return false;
			}
			bool flag = false;
			try
			{
			}
			finally
			{
				lock (hdl)
				{
					if (!hdl.IsInvalid && !hdl.IsClosed)
					{
						flag = UnsafeNativeMethods.sqlite3_get_autocommit(db) == 1;
					}
				}
			}
			GC.KeepAlive(hdl);
			return flag;
		}

		// Token: 0x04000531 RID: 1329
		internal const int COR_E_EXCEPTION = -2146233088;

		// Token: 0x04000532 RID: 1330
		private static long _createCount;

		// Token: 0x04000533 RID: 1331
		private static long _openCount;

		// Token: 0x04000534 RID: 1332
		private static long _closeCount;

		// Token: 0x04000535 RID: 1333
		private static long _disposeCount;

		// Token: 0x04000536 RID: 1334
		private bool disposed;

		// Token: 0x04000537 RID: 1335
		protected bool wasDisposed;

		// Token: 0x04000538 RID: 1336
		private static string[] _errorMessages = new string[]
		{
			"not an error", "SQL logic error", "internal logic error", "access permission denied", "query aborted", "database is locked", "database table is locked", "out of memory", "attempt to write a readonly database", "interrupted",
			"disk I/O error", "database disk image is malformed", "unknown operation", "database or disk is full", "unable to open database file", "locking protocol", "table contains no data", "database schema has changed", "string or blob too big", "constraint failed",
			"datatype mismatch", "bad parameter or other API misuse", "large file support is disabled", "authorization denied", "auxiliary database format error", "column index out of range", "file is not a database", "notification message", "warning message"
		};
	}
}
