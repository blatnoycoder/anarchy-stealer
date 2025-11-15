using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001D4 RID: 468
	public abstract class SQLiteModule : ISQLiteManagedModule, IDisposable
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x000602F8 File Offset: 0x0005E4F8
		public SQLiteModule(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
			this.tables = new Dictionary<IntPtr, SQLiteVirtualTable>();
			this.cursors = new Dictionary<IntPtr, SQLiteVirtualTableCursor>();
			this.functions = new Dictionary<string, SQLiteFunction>();
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00060348 File Offset: 0x0005E548
		internal bool CreateDisposableModule(IntPtr pDb)
		{
			if (this.disposableModule != IntPtr.Zero)
			{
				return true;
			}
			IntPtr intPtr = IntPtr.Zero;
			bool flag;
			try
			{
				intPtr = SQLiteString.Utf8IntPtrFromString(this.name);
				UnsafeNativeMethods.sqlite3_module sqlite3_module = this.AllocateNativeModule();
				this.destroyModule = new UnsafeNativeMethods.xDestroyModule(this.xDestroyModule);
				this.disposableModule = UnsafeNativeMethods.sqlite3_create_disposable_module(pDb, intPtr, ref sqlite3_module, IntPtr.Zero, this.destroyModule);
				flag = this.disposableModule != IntPtr.Zero;
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

		// Token: 0x0600150B RID: 5387 RVA: 0x000603F8 File Offset: 0x0005E5F8
		private void xDestroyModule(IntPtr pClientData)
		{
			this.disposableModule = IntPtr.Zero;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00060408 File Offset: 0x0005E608
		private UnsafeNativeMethods.sqlite3_module AllocateNativeModule()
		{
			return this.AllocateNativeModule(this.GetNativeModuleImpl());
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00060418 File Offset: 0x0005E618
		private UnsafeNativeMethods.sqlite3_module AllocateNativeModule(ISQLiteNativeModule module)
		{
			this.nativeModule = default(UnsafeNativeMethods.sqlite3_module);
			this.nativeModule.iVersion = SQLiteModule.DefaultModuleVersion;
			if (module != null)
			{
				this.nativeModule.xCreate = new UnsafeNativeMethods.xCreate(module.xCreate);
				this.nativeModule.xConnect = new UnsafeNativeMethods.xConnect(module.xConnect);
				this.nativeModule.xBestIndex = new UnsafeNativeMethods.xBestIndex(module.xBestIndex);
				this.nativeModule.xDisconnect = new UnsafeNativeMethods.xDisconnect(module.xDisconnect);
				this.nativeModule.xDestroy = new UnsafeNativeMethods.xDestroy(module.xDestroy);
				this.nativeModule.xOpen = new UnsafeNativeMethods.xOpen(module.xOpen);
				this.nativeModule.xClose = new UnsafeNativeMethods.xClose(module.xClose);
				this.nativeModule.xFilter = new UnsafeNativeMethods.xFilter(module.xFilter);
				this.nativeModule.xNext = new UnsafeNativeMethods.xNext(module.xNext);
				this.nativeModule.xEof = new UnsafeNativeMethods.xEof(module.xEof);
				this.nativeModule.xColumn = new UnsafeNativeMethods.xColumn(module.xColumn);
				this.nativeModule.xRowId = new UnsafeNativeMethods.xRowId(module.xRowId);
				this.nativeModule.xUpdate = new UnsafeNativeMethods.xUpdate(module.xUpdate);
				this.nativeModule.xBegin = new UnsafeNativeMethods.xBegin(module.xBegin);
				this.nativeModule.xSync = new UnsafeNativeMethods.xSync(module.xSync);
				this.nativeModule.xCommit = new UnsafeNativeMethods.xCommit(module.xCommit);
				this.nativeModule.xRollback = new UnsafeNativeMethods.xRollback(module.xRollback);
				this.nativeModule.xFindFunction = new UnsafeNativeMethods.xFindFunction(module.xFindFunction);
				this.nativeModule.xRename = new UnsafeNativeMethods.xRename(module.xRename);
				this.nativeModule.xSavepoint = new UnsafeNativeMethods.xSavepoint(module.xSavepoint);
				this.nativeModule.xRelease = new UnsafeNativeMethods.xRelease(module.xRelease);
				this.nativeModule.xRollbackTo = new UnsafeNativeMethods.xRollbackTo(module.xRollbackTo);
			}
			else
			{
				this.nativeModule.xCreate = new UnsafeNativeMethods.xCreate(this.xCreate);
				this.nativeModule.xConnect = new UnsafeNativeMethods.xConnect(this.xConnect);
				this.nativeModule.xBestIndex = new UnsafeNativeMethods.xBestIndex(this.xBestIndex);
				this.nativeModule.xDisconnect = new UnsafeNativeMethods.xDisconnect(this.xDisconnect);
				this.nativeModule.xDestroy = new UnsafeNativeMethods.xDestroy(this.xDestroy);
				this.nativeModule.xOpen = new UnsafeNativeMethods.xOpen(this.xOpen);
				this.nativeModule.xClose = new UnsafeNativeMethods.xClose(this.xClose);
				this.nativeModule.xFilter = new UnsafeNativeMethods.xFilter(this.xFilter);
				this.nativeModule.xNext = new UnsafeNativeMethods.xNext(this.xNext);
				this.nativeModule.xEof = new UnsafeNativeMethods.xEof(this.xEof);
				this.nativeModule.xColumn = new UnsafeNativeMethods.xColumn(this.xColumn);
				this.nativeModule.xRowId = new UnsafeNativeMethods.xRowId(this.xRowId);
				this.nativeModule.xUpdate = new UnsafeNativeMethods.xUpdate(this.xUpdate);
				this.nativeModule.xBegin = new UnsafeNativeMethods.xBegin(this.xBegin);
				this.nativeModule.xSync = new UnsafeNativeMethods.xSync(this.xSync);
				this.nativeModule.xCommit = new UnsafeNativeMethods.xCommit(this.xCommit);
				this.nativeModule.xRollback = new UnsafeNativeMethods.xRollback(this.xRollback);
				this.nativeModule.xFindFunction = new UnsafeNativeMethods.xFindFunction(this.xFindFunction);
				this.nativeModule.xRename = new UnsafeNativeMethods.xRename(this.xRename);
				this.nativeModule.xSavepoint = new UnsafeNativeMethods.xSavepoint(this.xSavepoint);
				this.nativeModule.xRelease = new UnsafeNativeMethods.xRelease(this.xRelease);
				this.nativeModule.xRollbackTo = new UnsafeNativeMethods.xRollbackTo(this.xRollbackTo);
			}
			return this.nativeModule;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00060860 File Offset: 0x0005EA60
		private UnsafeNativeMethods.sqlite3_module CopyNativeModule(UnsafeNativeMethods.sqlite3_module module)
		{
			return new UnsafeNativeMethods.sqlite3_module
			{
				iVersion = module.iVersion,
				xCreate = new UnsafeNativeMethods.xCreate(((module.xCreate != null) ? module.xCreate : new UnsafeNativeMethods.xCreate(this, ldftn(xCreate))).Invoke),
				xConnect = new UnsafeNativeMethods.xConnect(((module.xConnect != null) ? module.xConnect : new UnsafeNativeMethods.xConnect(this, ldftn(xConnect))).Invoke),
				xBestIndex = new UnsafeNativeMethods.xBestIndex(((module.xBestIndex != null) ? module.xBestIndex : new UnsafeNativeMethods.xBestIndex(this, ldftn(xBestIndex))).Invoke),
				xDisconnect = new UnsafeNativeMethods.xDisconnect(((module.xDisconnect != null) ? module.xDisconnect : new UnsafeNativeMethods.xDisconnect(this, ldftn(xDisconnect))).Invoke),
				xDestroy = new UnsafeNativeMethods.xDestroy(((module.xDestroy != null) ? module.xDestroy : new UnsafeNativeMethods.xDestroy(this, ldftn(xDestroy))).Invoke),
				xOpen = new UnsafeNativeMethods.xOpen(((module.xOpen != null) ? module.xOpen : new UnsafeNativeMethods.xOpen(this, ldftn(xOpen))).Invoke),
				xClose = new UnsafeNativeMethods.xClose(((module.xClose != null) ? module.xClose : new UnsafeNativeMethods.xClose(this, ldftn(xClose))).Invoke),
				xFilter = new UnsafeNativeMethods.xFilter(((module.xFilter != null) ? module.xFilter : new UnsafeNativeMethods.xFilter(this, ldftn(xFilter))).Invoke),
				xNext = new UnsafeNativeMethods.xNext(((module.xNext != null) ? module.xNext : new UnsafeNativeMethods.xNext(this, ldftn(xNext))).Invoke),
				xEof = new UnsafeNativeMethods.xEof(((module.xEof != null) ? module.xEof : new UnsafeNativeMethods.xEof(this, ldftn(xEof))).Invoke),
				xColumn = new UnsafeNativeMethods.xColumn(((module.xColumn != null) ? module.xColumn : new UnsafeNativeMethods.xColumn(this, ldftn(xColumn))).Invoke),
				xRowId = new UnsafeNativeMethods.xRowId(((module.xRowId != null) ? module.xRowId : new UnsafeNativeMethods.xRowId(this, ldftn(xRowId))).Invoke),
				xUpdate = new UnsafeNativeMethods.xUpdate(((module.xUpdate != null) ? module.xUpdate : new UnsafeNativeMethods.xUpdate(this, ldftn(xUpdate))).Invoke),
				xBegin = new UnsafeNativeMethods.xBegin(((module.xBegin != null) ? module.xBegin : new UnsafeNativeMethods.xBegin(this, ldftn(xBegin))).Invoke),
				xSync = new UnsafeNativeMethods.xSync(((module.xSync != null) ? module.xSync : new UnsafeNativeMethods.xSync(this, ldftn(xSync))).Invoke),
				xCommit = new UnsafeNativeMethods.xCommit(((module.xCommit != null) ? module.xCommit : new UnsafeNativeMethods.xCommit(this, ldftn(xCommit))).Invoke),
				xRollback = new UnsafeNativeMethods.xRollback(((module.xRollback != null) ? module.xRollback : new UnsafeNativeMethods.xRollback(this, ldftn(xRollback))).Invoke),
				xFindFunction = new UnsafeNativeMethods.xFindFunction(((module.xFindFunction != null) ? module.xFindFunction : new UnsafeNativeMethods.xFindFunction(this, ldftn(xFindFunction))).Invoke),
				xRename = new UnsafeNativeMethods.xRename(((module.xRename != null) ? module.xRename : new UnsafeNativeMethods.xRename(this, ldftn(xRename))).Invoke),
				xSavepoint = new UnsafeNativeMethods.xSavepoint(((module.xSavepoint != null) ? module.xSavepoint : new UnsafeNativeMethods.xSavepoint(this, ldftn(xSavepoint))).Invoke),
				xRelease = new UnsafeNativeMethods.xRelease(((module.xRelease != null) ? module.xRelease : new UnsafeNativeMethods.xRelease(this, ldftn(xRelease))).Invoke),
				xRollbackTo = new UnsafeNativeMethods.xRollbackTo(((module.xRollbackTo != null) ? module.xRollbackTo : new UnsafeNativeMethods.xRollbackTo(this, ldftn(xRollbackTo))).Invoke)
			};
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00060D2C File Offset: 0x0005EF2C
		private SQLiteErrorCode CreateOrConnect(bool create, IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError)
		{
			try
			{
				string text = SQLiteString.StringFromUtf8IntPtr(UnsafeNativeMethods.sqlite3_db_filename(pDb, IntPtr.Zero));
				using (SQLiteConnection sqliteConnection = new SQLiteConnection(pDb, text, false))
				{
					SQLiteVirtualTable sqliteVirtualTable = null;
					string text2 = null;
					if ((create && this.Create(sqliteConnection, pAux, SQLiteString.StringArrayFromUtf8SizeAndIntPtr(argc, argv), ref sqliteVirtualTable, ref text2) == SQLiteErrorCode.Ok) || (!create && this.Connect(sqliteConnection, pAux, SQLiteString.StringArrayFromUtf8SizeAndIntPtr(argc, argv), ref sqliteVirtualTable, ref text2) == SQLiteErrorCode.Ok))
					{
						if (sqliteVirtualTable != null)
						{
							pVtab = this.TableToIntPtr(sqliteVirtualTable);
							return SQLiteErrorCode.Ok;
						}
						pError = SQLiteString.Utf8IntPtrFromString("no table was created");
					}
					else
					{
						pError = SQLiteString.Utf8IntPtrFromString(text2);
					}
				}
			}
			catch (Exception ex)
			{
				pError = SQLiteString.Utf8IntPtrFromString(ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00060E18 File Offset: 0x0005F018
		private SQLiteErrorCode DestroyOrDisconnect(bool destroy, IntPtr pVtab)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null && ((destroy && this.Destroy(sqliteVirtualTable) == SQLiteErrorCode.Ok) || (!destroy && this.Disconnect(sqliteVirtualTable) == SQLiteErrorCode.Ok)))
				{
					if (this.tables != null)
					{
						this.tables.Remove(pVtab);
					}
					return SQLiteErrorCode.Ok;
				}
			}
			catch (Exception ex)
			{
				try
				{
					if (this.LogExceptionsNoThrow)
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[]
						{
							destroy ? "xDestroy" : "xDisconnect",
							ex
						}));
					}
				}
				catch
				{
				}
			}
			finally
			{
				this.FreeTable(pVtab);
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00060F0C File Offset: 0x0005F10C
		private static bool SetTableError(SQLiteModule module, IntPtr pVtab, bool logErrors, bool logExceptions, string error)
		{
			try
			{
				if (logErrors && error != null)
				{
					SQLiteLog.LogMessage(SQLiteErrorCode.Error, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Virtual table error: {0}", new object[] { error }));
				}
			}
			catch
			{
			}
			bool flag = false;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				if (pVtab == IntPtr.Zero)
				{
					return false;
				}
				int num = 0;
				num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
				num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
				IntPtr intPtr2 = SQLiteMarshal.ReadIntPtr(pVtab, num);
				if (intPtr2 != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr2);
					intPtr2 = IntPtr.Zero;
					SQLiteMarshal.WriteIntPtr(pVtab, num, intPtr2);
				}
				if (error == null)
				{
					return true;
				}
				intPtr = SQLiteString.Utf8IntPtrFromString(error);
				SQLiteMarshal.WriteIntPtr(pVtab, num, intPtr);
				flag = true;
			}
			catch (Exception ex)
			{
				try
				{
					if (logExceptions)
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "SetTableError", ex }));
					}
				}
				catch
				{
				}
			}
			finally
			{
				if (!flag && intPtr != IntPtr.Zero)
				{
					SQLiteMemory.Free(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return flag;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00061098 File Offset: 0x0005F298
		private static bool SetTableError(SQLiteModule module, SQLiteVirtualTable table, bool logErrors, bool logExceptions, string error)
		{
			if (table == null)
			{
				return false;
			}
			IntPtr nativeHandle = table.NativeHandle;
			return !(nativeHandle == IntPtr.Zero) && SQLiteModule.SetTableError(module, nativeHandle, logErrors, logExceptions, error);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000610D8 File Offset: 0x0005F2D8
		private static bool SetCursorError(SQLiteModule module, IntPtr pCursor, bool logErrors, bool logExceptions, string error)
		{
			if (pCursor == IntPtr.Zero)
			{
				return false;
			}
			IntPtr intPtr = SQLiteModule.TableFromCursor(module, pCursor);
			return !(intPtr == IntPtr.Zero) && SQLiteModule.SetTableError(module, intPtr, logErrors, logExceptions, error);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00061120 File Offset: 0x0005F320
		private static bool SetCursorError(SQLiteModule module, SQLiteVirtualTableCursor cursor, bool logErrors, bool logExceptions, string error)
		{
			if (cursor == null)
			{
				return false;
			}
			IntPtr nativeHandle = cursor.NativeHandle;
			return !(nativeHandle == IntPtr.Zero) && SQLiteModule.SetCursorError(module, nativeHandle, logErrors, logExceptions, error);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00061160 File Offset: 0x0005F360
		protected virtual ISQLiteNativeModule GetNativeModuleImpl()
		{
			return null;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00061164 File Offset: 0x0005F364
		protected virtual ISQLiteNativeModule CreateNativeModuleImpl()
		{
			return new SQLiteModule.SQLiteNativeModule(this);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0006116C File Offset: 0x0005F36C
		protected virtual IntPtr AllocateTable()
		{
			int num = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_vtab));
			return SQLiteMemory.Allocate(num);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00061194 File Offset: 0x0005F394
		protected virtual void ZeroTable(IntPtr pVtab)
		{
			if (pVtab == IntPtr.Zero)
			{
				return;
			}
			int num = 0;
			SQLiteMarshal.WriteIntPtr(pVtab, num, IntPtr.Zero);
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
			SQLiteMarshal.WriteInt32(pVtab, num, 0);
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			SQLiteMarshal.WriteIntPtr(pVtab, num, IntPtr.Zero);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000611F4 File Offset: 0x0005F3F4
		protected virtual void FreeTable(IntPtr pVtab)
		{
			this.SetTableError(pVtab, null);
			SQLiteMemory.Free(pVtab);
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00061208 File Offset: 0x0005F408
		protected virtual IntPtr AllocateCursor()
		{
			int num = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_vtab_cursor));
			return SQLiteMemory.Allocate(num);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00061230 File Offset: 0x0005F430
		protected virtual void FreeCursor(IntPtr pCursor)
		{
			SQLiteMemory.Free(pCursor);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00061238 File Offset: 0x0005F438
		private static IntPtr TableFromCursor(SQLiteModule module, IntPtr pCursor)
		{
			if (pCursor == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			return Marshal.ReadIntPtr(pCursor);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00061258 File Offset: 0x0005F458
		protected virtual IntPtr TableFromCursor(IntPtr pCursor)
		{
			return SQLiteModule.TableFromCursor(this, pCursor);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00061264 File Offset: 0x0005F464
		protected virtual SQLiteVirtualTable TableFromIntPtr(IntPtr pVtab)
		{
			if (pVtab == IntPtr.Zero)
			{
				this.SetTableError(pVtab, "invalid native table");
				return null;
			}
			SQLiteVirtualTable sqliteVirtualTable;
			if (this.tables != null && this.tables.TryGetValue(pVtab, out sqliteVirtualTable))
			{
				return sqliteVirtualTable;
			}
			this.SetTableError(pVtab, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "managed table for {0} not found", new object[] { pVtab }));
			return null;
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x000612E0 File Offset: 0x0005F4E0
		protected virtual IntPtr TableToIntPtr(SQLiteVirtualTable table)
		{
			if (table == null || this.tables == null)
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr = IntPtr.Zero;
			bool flag = false;
			try
			{
				intPtr = this.AllocateTable();
				if (intPtr != IntPtr.Zero)
				{
					this.ZeroTable(intPtr);
					table.NativeHandle = intPtr;
					this.tables.Add(intPtr, table);
					flag = true;
				}
			}
			finally
			{
				if (!flag && intPtr != IntPtr.Zero)
				{
					this.FreeTable(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return intPtr;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0006137C File Offset: 0x0005F57C
		protected virtual SQLiteVirtualTableCursor CursorFromIntPtr(IntPtr pVtab, IntPtr pCursor)
		{
			if (pCursor == IntPtr.Zero)
			{
				this.SetTableError(pVtab, "invalid native cursor");
				return null;
			}
			SQLiteVirtualTableCursor sqliteVirtualTableCursor;
			if (this.cursors != null && this.cursors.TryGetValue(pCursor, out sqliteVirtualTableCursor))
			{
				return sqliteVirtualTableCursor;
			}
			this.SetTableError(pVtab, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "managed cursor for {0} not found", new object[] { pCursor }));
			return null;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000613F8 File Offset: 0x0005F5F8
		protected virtual IntPtr CursorToIntPtr(SQLiteVirtualTableCursor cursor)
		{
			if (cursor == null || this.cursors == null)
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr = IntPtr.Zero;
			bool flag = false;
			try
			{
				intPtr = this.AllocateCursor();
				if (intPtr != IntPtr.Zero)
				{
					cursor.NativeHandle = intPtr;
					this.cursors.Add(intPtr, cursor);
					flag = true;
				}
			}
			finally
			{
				if (!flag && intPtr != IntPtr.Zero)
				{
					this.FreeCursor(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			return intPtr;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0006148C File Offset: 0x0005F68C
		protected virtual string GetFunctionKey(int argumentCount, string name, SQLiteFunction function)
		{
			return HelperMethods.StringFormat(CultureInfo.InvariantCulture, "{0}:{1}", new object[] { argumentCount, name });
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x000614C4 File Offset: 0x0005F6C4
		protected virtual SQLiteErrorCode DeclareTable(SQLiteConnection connection, string sql, ref string error)
		{
			if (connection == null)
			{
				error = "invalid connection";
				return SQLiteErrorCode.Error;
			}
			SQLiteBase sql2 = connection._sql;
			if (sql2 == null)
			{
				error = "connection has invalid handle";
				return SQLiteErrorCode.Error;
			}
			if (sql == null)
			{
				error = "invalid SQL statement";
				return SQLiteErrorCode.Error;
			}
			return sql2.DeclareVirtualTable(this, sql, ref error);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00061514 File Offset: 0x0005F714
		protected virtual SQLiteErrorCode DeclareFunction(SQLiteConnection connection, int argumentCount, string name, ref string error)
		{
			if (connection == null)
			{
				error = "invalid connection";
				return SQLiteErrorCode.Error;
			}
			SQLiteBase sql = connection._sql;
			if (sql == null)
			{
				error = "connection has invalid handle";
				return SQLiteErrorCode.Error;
			}
			return sql.DeclareVirtualFunction(this, argumentCount, name, ref error);
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00061558 File Offset: 0x0005F758
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x00061560 File Offset: 0x0005F760
		protected virtual bool LogErrorsNoThrow
		{
			get
			{
				return this.logErrors;
			}
			set
			{
				this.logErrors = value;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0006156C File Offset: 0x0005F76C
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x00061574 File Offset: 0x0005F774
		protected virtual bool LogExceptionsNoThrow
		{
			get
			{
				return this.logExceptions;
			}
			set
			{
				this.logExceptions = value;
			}
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00061580 File Offset: 0x0005F780
		protected virtual bool SetTableError(IntPtr pVtab, string error)
		{
			return SQLiteModule.SetTableError(this, pVtab, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000615A8 File Offset: 0x0005F7A8
		protected virtual bool SetTableError(SQLiteVirtualTable table, string error)
		{
			return SQLiteModule.SetTableError(this, table, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x000615D0 File Offset: 0x0005F7D0
		protected virtual bool SetCursorError(SQLiteVirtualTableCursor cursor, string error)
		{
			return SQLiteModule.SetCursorError(this, cursor, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x000615F8 File Offset: 0x0005F7F8
		protected virtual bool SetEstimatedCost(SQLiteIndex index, double? estimatedCost)
		{
			if (index == null || index.Outputs == null)
			{
				return false;
			}
			index.Outputs.EstimatedCost = estimatedCost;
			return true;
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0006161C File Offset: 0x0005F81C
		protected virtual bool SetEstimatedCost(SQLiteIndex index)
		{
			return this.SetEstimatedCost(index, null);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00061640 File Offset: 0x0005F840
		protected virtual bool SetEstimatedRows(SQLiteIndex index, long? estimatedRows)
		{
			if (index == null || index.Outputs == null)
			{
				return false;
			}
			index.Outputs.EstimatedRows = estimatedRows;
			return true;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00061664 File Offset: 0x0005F864
		protected virtual bool SetEstimatedRows(SQLiteIndex index)
		{
			return this.SetEstimatedRows(index, null);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00061688 File Offset: 0x0005F888
		protected virtual bool SetIndexFlags(SQLiteIndex index, SQLiteIndexFlags? indexFlags)
		{
			if (index == null || index.Outputs == null)
			{
				return false;
			}
			index.Outputs.IndexFlags = indexFlags;
			return true;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000616AC File Offset: 0x0005F8AC
		protected virtual bool SetIndexFlags(SQLiteIndex index)
		{
			return this.SetIndexFlags(index, null);
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x000616D0 File Offset: 0x0005F8D0
		// (set) Token: 0x06001533 RID: 5427 RVA: 0x000616E0 File Offset: 0x0005F8E0
		public virtual bool LogErrors
		{
			get
			{
				this.CheckDisposed();
				return this.LogErrorsNoThrow;
			}
			set
			{
				this.CheckDisposed();
				this.LogErrorsNoThrow = value;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x000616F0 File Offset: 0x0005F8F0
		// (set) Token: 0x06001535 RID: 5429 RVA: 0x00061700 File Offset: 0x0005F900
		public virtual bool LogExceptions
		{
			get
			{
				this.CheckDisposed();
				return this.LogExceptionsNoThrow;
			}
			set
			{
				this.CheckDisposed();
				this.LogExceptionsNoThrow = value;
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00061710 File Offset: 0x0005F910
		private SQLiteErrorCode xCreate(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError)
		{
			return this.CreateOrConnect(true, pDb, pAux, argc, argv, ref pVtab, ref pError);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00061734 File Offset: 0x0005F934
		private SQLiteErrorCode xConnect(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError)
		{
			return this.CreateOrConnect(false, pDb, pAux, argc, argv, ref pVtab, ref pError);
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00061758 File Offset: 0x0005F958
		private SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					SQLiteIndex sqliteIndex = null;
					SQLiteIndex.FromIntPtr(pIndex, true, ref sqliteIndex);
					if (this.BestIndex(sqliteVirtualTable, sqliteIndex) == SQLiteErrorCode.Ok)
					{
						SQLiteIndex.ToIntPtr(sqliteIndex, pIndex, true);
						return SQLiteErrorCode.Ok;
					}
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000617C8 File Offset: 0x0005F9C8
		private SQLiteErrorCode xDisconnect(IntPtr pVtab)
		{
			return this.DestroyOrDisconnect(false, pVtab);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x000617D4 File Offset: 0x0005F9D4
		private SQLiteErrorCode xDestroy(IntPtr pVtab)
		{
			return this.DestroyOrDisconnect(true, pVtab);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x000617E0 File Offset: 0x0005F9E0
		private SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					SQLiteVirtualTableCursor sqliteVirtualTableCursor = null;
					if (this.Open(sqliteVirtualTable, ref sqliteVirtualTableCursor) == SQLiteErrorCode.Ok)
					{
						if (sqliteVirtualTableCursor != null)
						{
							pCursor = this.CursorToIntPtr(sqliteVirtualTableCursor);
							if (pCursor != IntPtr.Zero)
							{
								return SQLiteErrorCode.Ok;
							}
							this.SetTableError(pVtab, "no native cursor was created");
						}
						else
						{
							this.SetTableError(pVtab, "no managed cursor was created");
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00061884 File Offset: 0x0005FA84
		private SQLiteErrorCode xClose(IntPtr pCursor)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = this.TableFromCursor(pCursor);
				SQLiteVirtualTableCursor sqliteVirtualTableCursor = this.CursorFromIntPtr(intPtr, pCursor);
				if (sqliteVirtualTableCursor != null && this.Close(sqliteVirtualTableCursor) == SQLiteErrorCode.Ok)
				{
					if (this.cursors != null)
					{
						this.cursors.Remove(pCursor);
					}
					return SQLiteErrorCode.Ok;
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(intPtr, ex.ToString());
			}
			finally
			{
				this.FreeCursor(pCursor);
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00061920 File Offset: 0x0005FB20
		private SQLiteErrorCode xFilter(IntPtr pCursor, int idxNum, IntPtr idxStr, int argc, IntPtr argv)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = this.TableFromCursor(pCursor);
				SQLiteVirtualTableCursor sqliteVirtualTableCursor = this.CursorFromIntPtr(intPtr, pCursor);
				if (sqliteVirtualTableCursor != null && this.Filter(sqliteVirtualTableCursor, idxNum, SQLiteString.StringFromUtf8IntPtr(idxStr), SQLiteValue.ArrayFromSizeAndIntPtr(argc, argv)) == SQLiteErrorCode.Ok)
				{
					return SQLiteErrorCode.Ok;
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(intPtr, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0006199C File Offset: 0x0005FB9C
		private SQLiteErrorCode xNext(IntPtr pCursor)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = this.TableFromCursor(pCursor);
				SQLiteVirtualTableCursor sqliteVirtualTableCursor = this.CursorFromIntPtr(intPtr, pCursor);
				if (sqliteVirtualTableCursor != null && this.Next(sqliteVirtualTableCursor) == SQLiteErrorCode.Ok)
				{
					return SQLiteErrorCode.Ok;
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(intPtr, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00061A08 File Offset: 0x0005FC08
		private int xEof(IntPtr pCursor)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = this.TableFromCursor(pCursor);
				SQLiteVirtualTableCursor sqliteVirtualTableCursor = this.CursorFromIntPtr(intPtr, pCursor);
				if (sqliteVirtualTableCursor != null)
				{
					return this.Eof(sqliteVirtualTableCursor) ? 1 : 0;
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(intPtr, ex.ToString());
			}
			return 1;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00061A78 File Offset: 0x0005FC78
		private SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = this.TableFromCursor(pCursor);
				SQLiteVirtualTableCursor sqliteVirtualTableCursor = this.CursorFromIntPtr(intPtr, pCursor);
				if (sqliteVirtualTableCursor != null)
				{
					SQLiteContext sqliteContext = new SQLiteContext(pContext);
					return this.Column(sqliteVirtualTableCursor, sqliteContext, index);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(intPtr, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00061AE8 File Offset: 0x0005FCE8
		private SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = this.TableFromCursor(pCursor);
				SQLiteVirtualTableCursor sqliteVirtualTableCursor = this.CursorFromIntPtr(intPtr, pCursor);
				if (sqliteVirtualTableCursor != null)
				{
					return this.RowId(sqliteVirtualTableCursor, ref rowId);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(intPtr, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00061B4C File Offset: 0x0005FD4C
		private SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Update(sqliteVirtualTable, SQLiteValue.ArrayFromSizeAndIntPtr(argc, argv), ref rowId);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00061BAC File Offset: 0x0005FDAC
		private SQLiteErrorCode xBegin(IntPtr pVtab)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Begin(sqliteVirtualTable);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00061C00 File Offset: 0x0005FE00
		private SQLiteErrorCode xSync(IntPtr pVtab)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Sync(sqliteVirtualTable);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00061C54 File Offset: 0x0005FE54
		private SQLiteErrorCode xCommit(IntPtr pVtab)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Commit(sqliteVirtualTable);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00061CA8 File Offset: 0x0005FEA8
		private SQLiteErrorCode xRollback(IntPtr pVtab)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Rollback(sqliteVirtualTable);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00061CFC File Offset: 0x0005FEFC
		private int xFindFunction(IntPtr pVtab, int nArg, IntPtr zName, ref SQLiteCallback callback, ref IntPtr pClientData)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					string text = SQLiteString.StringFromUtf8IntPtr(zName);
					SQLiteFunction sqliteFunction = null;
					if (this.FindFunction(sqliteVirtualTable, nArg, text, ref sqliteFunction, ref pClientData))
					{
						if (sqliteFunction != null)
						{
							string functionKey = this.GetFunctionKey(nArg, text, sqliteFunction);
							this.functions[functionKey] = sqliteFunction;
							callback = new SQLiteCallback(sqliteFunction.ScalarCallback);
							return 1;
						}
						this.SetTableError(pVtab, "no function was created");
					}
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return 0;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00061DA4 File Offset: 0x0005FFA4
		private SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Rename(sqliteVirtualTable, SQLiteString.StringFromUtf8IntPtr(zNew));
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00061E00 File Offset: 0x00060000
		private SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Savepoint(sqliteVirtualTable, iSavepoint);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00061E58 File Offset: 0x00060058
		private SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.Release(sqliteVirtualTable, iSavepoint);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00061EB0 File Offset: 0x000600B0
		private SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint)
		{
			try
			{
				SQLiteVirtualTable sqliteVirtualTable = this.TableFromIntPtr(pVtab);
				if (sqliteVirtualTable != null)
				{
					return this.RollbackTo(sqliteVirtualTable, iSavepoint);
				}
			}
			catch (Exception ex)
			{
				this.SetTableError(pVtab, ex.ToString());
			}
			return SQLiteErrorCode.Error;
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00061F08 File Offset: 0x00060108
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x00061F18 File Offset: 0x00060118
		public virtual bool Declared
		{
			get
			{
				this.CheckDisposed();
				return this.declared;
			}
			internal set
			{
				this.declared = value;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00061F24 File Offset: 0x00060124
		public virtual string Name
		{
			get
			{
				this.CheckDisposed();
				return this.name;
			}
		}

		// Token: 0x0600154F RID: 5455
		public abstract SQLiteErrorCode Create(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error);

		// Token: 0x06001550 RID: 5456
		public abstract SQLiteErrorCode Connect(SQLiteConnection connection, IntPtr pClientData, string[] arguments, ref SQLiteVirtualTable table, ref string error);

		// Token: 0x06001551 RID: 5457
		public abstract SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index);

		// Token: 0x06001552 RID: 5458
		public abstract SQLiteErrorCode Disconnect(SQLiteVirtualTable table);

		// Token: 0x06001553 RID: 5459
		public abstract SQLiteErrorCode Destroy(SQLiteVirtualTable table);

		// Token: 0x06001554 RID: 5460
		public abstract SQLiteErrorCode Open(SQLiteVirtualTable table, ref SQLiteVirtualTableCursor cursor);

		// Token: 0x06001555 RID: 5461
		public abstract SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor);

		// Token: 0x06001556 RID: 5462
		public abstract SQLiteErrorCode Filter(SQLiteVirtualTableCursor cursor, int indexNumber, string indexString, SQLiteValue[] values);

		// Token: 0x06001557 RID: 5463
		public abstract SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor);

		// Token: 0x06001558 RID: 5464
		public abstract bool Eof(SQLiteVirtualTableCursor cursor);

		// Token: 0x06001559 RID: 5465
		public abstract SQLiteErrorCode Column(SQLiteVirtualTableCursor cursor, SQLiteContext context, int index);

		// Token: 0x0600155A RID: 5466
		public abstract SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId);

		// Token: 0x0600155B RID: 5467
		public abstract SQLiteErrorCode Update(SQLiteVirtualTable table, SQLiteValue[] values, ref long rowId);

		// Token: 0x0600155C RID: 5468
		public abstract SQLiteErrorCode Begin(SQLiteVirtualTable table);

		// Token: 0x0600155D RID: 5469
		public abstract SQLiteErrorCode Sync(SQLiteVirtualTable table);

		// Token: 0x0600155E RID: 5470
		public abstract SQLiteErrorCode Commit(SQLiteVirtualTable table);

		// Token: 0x0600155F RID: 5471
		public abstract SQLiteErrorCode Rollback(SQLiteVirtualTable table);

		// Token: 0x06001560 RID: 5472
		public abstract bool FindFunction(SQLiteVirtualTable table, int argumentCount, string name, ref SQLiteFunction function, ref IntPtr pClientData);

		// Token: 0x06001561 RID: 5473
		public abstract SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName);

		// Token: 0x06001562 RID: 5474
		public abstract SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint);

		// Token: 0x06001563 RID: 5475
		public abstract SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint);

		// Token: 0x06001564 RID: 5476
		public abstract SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint);

		// Token: 0x06001565 RID: 5477 RVA: 0x00061F34 File Offset: 0x00060134
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00061F44 File Offset: 0x00060144
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteModule).Name);
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00061F68 File Offset: 0x00060168
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing && this.functions != null)
				{
					this.functions.Clear();
				}
				try
				{
					if (this.disposableModule != IntPtr.Zero)
					{
						UnsafeNativeMethods.sqlite3_dispose_module(this.disposableModule);
						this.disposableModule = IntPtr.Zero;
					}
				}
				catch (Exception ex)
				{
					try
					{
						if (this.LogExceptionsNoThrow)
						{
							SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Dispose", ex }));
						}
					}
					catch
					{
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00062038 File Offset: 0x00060238
		~SQLiteModule()
		{
			this.Dispose(false);
		}

		// Token: 0x040008B2 RID: 2226
		private static readonly int DefaultModuleVersion = 2;

		// Token: 0x040008B3 RID: 2227
		private UnsafeNativeMethods.sqlite3_module nativeModule;

		// Token: 0x040008B4 RID: 2228
		private UnsafeNativeMethods.xDestroyModule destroyModule;

		// Token: 0x040008B5 RID: 2229
		private IntPtr disposableModule;

		// Token: 0x040008B6 RID: 2230
		private Dictionary<IntPtr, SQLiteVirtualTable> tables;

		// Token: 0x040008B7 RID: 2231
		private Dictionary<IntPtr, SQLiteVirtualTableCursor> cursors;

		// Token: 0x040008B8 RID: 2232
		private Dictionary<string, SQLiteFunction> functions;

		// Token: 0x040008B9 RID: 2233
		private bool logErrors;

		// Token: 0x040008BA RID: 2234
		private bool logExceptions;

		// Token: 0x040008BB RID: 2235
		private bool declared;

		// Token: 0x040008BC RID: 2236
		private string name;

		// Token: 0x040008BD RID: 2237
		private bool disposed;

		// Token: 0x020002C6 RID: 710
		private sealed class SQLiteNativeModule : ISQLiteNativeModule, IDisposable
		{
			// Token: 0x06001903 RID: 6403 RVA: 0x0006A0D8 File Offset: 0x000682D8
			public SQLiteNativeModule(SQLiteModule module)
			{
				this.module = module;
			}

			// Token: 0x06001904 RID: 6404 RVA: 0x0006A0E8 File Offset: 0x000682E8
			private static SQLiteErrorCode ModuleNotAvailableTableError(IntPtr pVtab)
			{
				SQLiteModule.SetTableError(null, pVtab, true, true, "native module implementation not available");
				return SQLiteErrorCode.Error;
			}

			// Token: 0x06001905 RID: 6405 RVA: 0x0006A0FC File Offset: 0x000682FC
			private static SQLiteErrorCode ModuleNotAvailableCursorError(IntPtr pCursor)
			{
				SQLiteModule.SetCursorError(null, pCursor, true, true, "native module implementation not available");
				return SQLiteErrorCode.Error;
			}

			// Token: 0x06001906 RID: 6406 RVA: 0x0006A110 File Offset: 0x00068310
			public SQLiteErrorCode xCreate(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError)
			{
				if (this.module == null)
				{
					pError = SQLiteString.Utf8IntPtrFromString("native module implementation not available");
					return SQLiteErrorCode.Error;
				}
				return this.module.xCreate(pDb, pAux, argc, argv, ref pVtab, ref pError);
			}

			// Token: 0x06001907 RID: 6407 RVA: 0x0006A140 File Offset: 0x00068340
			public SQLiteErrorCode xConnect(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError)
			{
				if (this.module == null)
				{
					pError = SQLiteString.Utf8IntPtrFromString("native module implementation not available");
					return SQLiteErrorCode.Error;
				}
				return this.module.xConnect(pDb, pAux, argc, argv, ref pVtab, ref pError);
			}

			// Token: 0x06001908 RID: 6408 RVA: 0x0006A170 File Offset: 0x00068370
			public SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xBestIndex(pVtab, pIndex);
			}

			// Token: 0x06001909 RID: 6409 RVA: 0x0006A194 File Offset: 0x00068394
			public SQLiteErrorCode xDisconnect(IntPtr pVtab)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xDisconnect(pVtab);
			}

			// Token: 0x0600190A RID: 6410 RVA: 0x0006A1B4 File Offset: 0x000683B4
			public SQLiteErrorCode xDestroy(IntPtr pVtab)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xDestroy(pVtab);
			}

			// Token: 0x0600190B RID: 6411 RVA: 0x0006A1D4 File Offset: 0x000683D4
			public SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xOpen(pVtab, ref pCursor);
			}

			// Token: 0x0600190C RID: 6412 RVA: 0x0006A1F8 File Offset: 0x000683F8
			public SQLiteErrorCode xClose(IntPtr pCursor)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
				}
				return this.module.xClose(pCursor);
			}

			// Token: 0x0600190D RID: 6413 RVA: 0x0006A218 File Offset: 0x00068418
			public SQLiteErrorCode xFilter(IntPtr pCursor, int idxNum, IntPtr idxStr, int argc, IntPtr argv)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
				}
				return this.module.xFilter(pCursor, idxNum, idxStr, argc, argv);
			}

			// Token: 0x0600190E RID: 6414 RVA: 0x0006A240 File Offset: 0x00068440
			public SQLiteErrorCode xNext(IntPtr pCursor)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
				}
				return this.module.xNext(pCursor);
			}

			// Token: 0x0600190F RID: 6415 RVA: 0x0006A260 File Offset: 0x00068460
			public int xEof(IntPtr pCursor)
			{
				if (this.module == null)
				{
					SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
					return 1;
				}
				return this.module.xEof(pCursor);
			}

			// Token: 0x06001910 RID: 6416 RVA: 0x0006A284 File Offset: 0x00068484
			public SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
				}
				return this.module.xColumn(pCursor, pContext, index);
			}

			// Token: 0x06001911 RID: 6417 RVA: 0x0006A2A8 File Offset: 0x000684A8
			public SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
				}
				return this.module.xRowId(pCursor, ref rowId);
			}

			// Token: 0x06001912 RID: 6418 RVA: 0x0006A2CC File Offset: 0x000684CC
			public SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xUpdate(pVtab, argc, argv, ref rowId);
			}

			// Token: 0x06001913 RID: 6419 RVA: 0x0006A2F0 File Offset: 0x000684F0
			public SQLiteErrorCode xBegin(IntPtr pVtab)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xBegin(pVtab);
			}

			// Token: 0x06001914 RID: 6420 RVA: 0x0006A310 File Offset: 0x00068510
			public SQLiteErrorCode xSync(IntPtr pVtab)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xSync(pVtab);
			}

			// Token: 0x06001915 RID: 6421 RVA: 0x0006A330 File Offset: 0x00068530
			public SQLiteErrorCode xCommit(IntPtr pVtab)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xCommit(pVtab);
			}

			// Token: 0x06001916 RID: 6422 RVA: 0x0006A350 File Offset: 0x00068550
			public SQLiteErrorCode xRollback(IntPtr pVtab)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xRollback(pVtab);
			}

			// Token: 0x06001917 RID: 6423 RVA: 0x0006A370 File Offset: 0x00068570
			public int xFindFunction(IntPtr pVtab, int nArg, IntPtr zName, ref SQLiteCallback callback, ref IntPtr pClientData)
			{
				if (this.module == null)
				{
					SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
					return 0;
				}
				return this.module.xFindFunction(pVtab, nArg, zName, ref callback, ref pClientData);
			}

			// Token: 0x06001918 RID: 6424 RVA: 0x0006A398 File Offset: 0x00068598
			public SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xRename(pVtab, zNew);
			}

			// Token: 0x06001919 RID: 6425 RVA: 0x0006A3BC File Offset: 0x000685BC
			public SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xSavepoint(pVtab, iSavepoint);
			}

			// Token: 0x0600191A RID: 6426 RVA: 0x0006A3E0 File Offset: 0x000685E0
			public SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xRelease(pVtab, iSavepoint);
			}

			// Token: 0x0600191B RID: 6427 RVA: 0x0006A404 File Offset: 0x00068604
			public SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint)
			{
				if (this.module == null)
				{
					return SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
				}
				return this.module.xRollbackTo(pVtab, iSavepoint);
			}

			// Token: 0x0600191C RID: 6428 RVA: 0x0006A428 File Offset: 0x00068628
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x0600191D RID: 6429 RVA: 0x0006A438 File Offset: 0x00068638
			private void CheckDisposed()
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(typeof(SQLiteModule.SQLiteNativeModule).Name);
				}
			}

			// Token: 0x0600191E RID: 6430 RVA: 0x0006A45C File Offset: 0x0006865C
			private void Dispose(bool disposing)
			{
				if (!this.disposed)
				{
					if (this.module != null)
					{
						this.module = null;
					}
					this.disposed = true;
				}
			}

			// Token: 0x0600191F RID: 6431 RVA: 0x0006A484 File Offset: 0x00068684
			~SQLiteNativeModule()
			{
				this.Dispose(false);
			}

			// Token: 0x04000B9E RID: 2974
			private const bool DefaultLogErrors = true;

			// Token: 0x04000B9F RID: 2975
			private const bool DefaultLogExceptions = true;

			// Token: 0x04000BA0 RID: 2976
			private const string ModuleNotAvailableErrorMessage = "native module implementation not available";

			// Token: 0x04000BA1 RID: 2977
			private SQLiteModule module;

			// Token: 0x04000BA2 RID: 2978
			private bool disposed;
		}
	}
}
