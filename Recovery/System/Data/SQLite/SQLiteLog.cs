using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001AD RID: 429
	public static class SQLiteLog
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001294 RID: 4756 RVA: 0x000592F0 File Offset: 0x000574F0
		// (remove) Token: 0x06001295 RID: 4757 RVA: 0x00059328 File Offset: 0x00057528
		private static event SQLiteLogEventHandler _handlers;

		// Token: 0x06001296 RID: 4758 RVA: 0x00059360 File Offset: 0x00057560
		private static EventWaitHandle CreateAndOrGetTheEvent()
		{
			bool flag = false;
			EventWaitHandle eventWaitHandle = null;
			EventWaitHandle eventWaitHandle3;
			try
			{
				EventWaitHandle eventWaitHandle2 = Interlocked.CompareExchange<EventWaitHandle>(ref SQLiteLog._initializeEvent, null, null);
				if (eventWaitHandle2 == null)
				{
					eventWaitHandle = new ManualResetEvent(false);
					eventWaitHandle2 = Interlocked.CompareExchange<EventWaitHandle>(ref SQLiteLog._initializeEvent, eventWaitHandle, null);
				}
				if (eventWaitHandle2 == null)
				{
					eventWaitHandle2 = eventWaitHandle;
					flag = true;
				}
				eventWaitHandle3 = eventWaitHandle2;
			}
			finally
			{
				if (!flag && eventWaitHandle != null)
				{
					eventWaitHandle.Close();
					eventWaitHandle = null;
				}
			}
			return eventWaitHandle3;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x000593D0 File Offset: 0x000575D0
		public static void Initialize()
		{
			SQLiteLog.Initialize(null);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000593D8 File Offset: 0x000575D8
		internal static void Initialize(string className)
		{
			if (UnsafeNativeMethods.GetSettingValue("No_SQLiteLog", null) != null)
			{
				return;
			}
			EventWaitHandle eventWaitHandle = SQLiteLog.CreateAndOrGetTheEvent();
			if (Interlocked.Increment(ref SQLiteLog._initializeDoneCount) == 1)
			{
				bool flag = false;
				try
				{
					flag = SQLiteLog.PrivateInitialize(className);
					goto IL_005F;
				}
				finally
				{
					if (eventWaitHandle != null)
					{
						eventWaitHandle.Set();
					}
					if (!flag)
					{
						Interlocked.Decrement(ref SQLiteLog._initializeDoneCount);
					}
				}
			}
			Interlocked.Decrement(ref SQLiteLog._initializeDoneCount);
			IL_005F:
			if (eventWaitHandle != null && !eventWaitHandle.WaitOne(1000, false))
			{
				Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "TIMED OUT ({0}) waiting for logging subsystem", new object[] { 1000 }));
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00059494 File Offset: 0x00057694
		private static bool PrivateInitialize(string className)
		{
			Interlocked.Increment(ref SQLiteLog._initializeCallCount);
			if (UnsafeNativeMethods.GetSettingValue("Initialize_SQLiteLog", null) == null && Interlocked.Increment(ref SQLiteLog._attemptedInitialize) > 1)
			{
				Interlocked.Decrement(ref SQLiteLog._attemptedInitialize);
				return false;
			}
			if (SQLite3.StaticIsInitialized())
			{
				return false;
			}
			if (!AppDomain.CurrentDomain.IsDefaultAppDomain() && UnsafeNativeMethods.GetSettingValue("Force_SQLiteLog", null) == null)
			{
				return false;
			}
			lock (SQLiteLog.syncRoot)
			{
				if (SQLite3.StaticIsInitialized())
				{
					return false;
				}
				if (SQLiteLog._domainUnload == null)
				{
					SQLiteLog._domainUnload = new EventHandler(SQLiteLog.DomainUnload);
					AppDomain.CurrentDomain.DomainUnload += SQLiteLog._domainUnload;
				}
				if (SQLiteLog._sql == null)
				{
					SQLiteLog._sql = new SQLite3(SQLiteDateFormats.ISO8601, DateTimeKind.Unspecified, null, IntPtr.Zero, null, false);
				}
				if (SQLiteLog._callback == null)
				{
					SQLiteLog._callback = new SQLiteLogCallback(SQLiteLog.LogCallback);
					SQLiteErrorCode sqliteErrorCode = SQLiteLog._sql.SetLogCallback(SQLiteLog._callback);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						SQLiteLog._callback = null;
						throw new SQLiteException(sqliteErrorCode, "Failed to configure managed assembly logging.");
					}
				}
				if (UnsafeNativeMethods.GetSettingValue("Disable_SQLiteLog", null) == null)
				{
					SQLiteLog._enabled = true;
				}
				SQLiteLog.AddDefaultHandler();
			}
			return true;
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000595F4 File Offset: 0x000577F4
		public static void Uninitialize()
		{
			SQLiteLog.Uninitialize(null, false);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00059600 File Offset: 0x00057800
		internal static void Uninitialize(string className, bool shutdown)
		{
			Interlocked.Increment(ref SQLiteLog._uninitializeCallCount);
			lock (SQLiteLog.syncRoot)
			{
				SQLiteLog.RemoveDefaultHandler();
				SQLiteLog._enabled = false;
				if (SQLiteLog._sql != null)
				{
					SQLiteErrorCode sqliteErrorCode;
					if (shutdown)
					{
						sqliteErrorCode = SQLiteLog._sql.Shutdown();
						if (sqliteErrorCode != SQLiteErrorCode.Ok)
						{
							throw new SQLiteException(sqliteErrorCode, "Failed to shutdown interface.");
						}
					}
					sqliteErrorCode = SQLiteLog._sql.SetLogCallback(null);
					if (sqliteErrorCode != SQLiteErrorCode.Ok)
					{
						throw new SQLiteException(sqliteErrorCode, "Failed to shutdown logging.");
					}
				}
				if (SQLiteLog._callback != null)
				{
					SQLiteLog._callback = null;
				}
				if (SQLiteLog._domainUnload != null)
				{
					AppDomain.CurrentDomain.DomainUnload -= SQLiteLog._domainUnload;
					SQLiteLog._domainUnload = null;
				}
				EventWaitHandle eventWaitHandle = SQLiteLog.CreateAndOrGetTheEvent();
				if (eventWaitHandle != null)
				{
					eventWaitHandle.Reset();
				}
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000596E4 File Offset: 0x000578E4
		private static void DomainUnload(object sender, EventArgs e)
		{
			SQLiteLog.Uninitialize(null, true);
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600129D RID: 4765 RVA: 0x000596F0 File Offset: 0x000578F0
		// (remove) Token: 0x0600129E RID: 4766 RVA: 0x0005973C File Offset: 0x0005793C
		public static event SQLiteLogEventHandler Log
		{
			add
			{
				lock (SQLiteLog.syncRoot)
				{
					SQLiteLog._handlers -= value;
					SQLiteLog._handlers += value;
				}
			}
			remove
			{
				lock (SQLiteLog.syncRoot)
				{
					SQLiteLog._handlers -= value;
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00059784 File Offset: 0x00057984
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x000597CC File Offset: 0x000579CC
		public static bool Enabled
		{
			get
			{
				bool internalEnabled;
				lock (SQLiteLog.syncRoot)
				{
					internalEnabled = SQLiteLog.InternalEnabled;
				}
				return internalEnabled;
			}
			set
			{
				lock (SQLiteLog.syncRoot)
				{
					SQLiteLog.InternalEnabled = value;
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x00059814 File Offset: 0x00057A14
		// (set) Token: 0x060012A2 RID: 4770 RVA: 0x0005981C File Offset: 0x00057A1C
		internal static bool InternalEnabled
		{
			get
			{
				return SQLiteLog._enabled;
			}
			set
			{
				SQLiteLog._enabled = value;
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00059824 File Offset: 0x00057A24
		public static void LogMessage(string message)
		{
			SQLiteLog.LogMessage(null, message);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00059830 File Offset: 0x00057A30
		public static void LogMessage(SQLiteErrorCode errorCode, string message)
		{
			SQLiteLog.LogMessage(errorCode, message);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00059840 File Offset: 0x00057A40
		public static void LogMessage(int errorCode, string message)
		{
			SQLiteLog.LogMessage(errorCode, message);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00059850 File Offset: 0x00057A50
		private static void LogMessage(object errorCode, string message)
		{
			bool flag2;
			SQLiteLogEventHandler sqliteLogEventHandler;
			lock (SQLiteLog.syncRoot)
			{
				if (SQLiteLog._enabled && SQLiteLog._handlers != null)
				{
					flag2 = true;
					sqliteLogEventHandler = SQLiteLog._handlers.Clone() as SQLiteLogEventHandler;
				}
				else
				{
					flag2 = false;
					sqliteLogEventHandler = null;
				}
			}
			if (flag2 && sqliteLogEventHandler != null)
			{
				sqliteLogEventHandler(null, new LogEventArgs(IntPtr.Zero, errorCode, message, null));
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x000598E0 File Offset: 0x00057AE0
		private static void InitializeDefaultHandler()
		{
			lock (SQLiteLog.syncRoot)
			{
				if (SQLiteLog._defaultHandler == null)
				{
					SQLiteLog._defaultHandler = new SQLiteLogEventHandler(SQLiteLog.LogEventHandler);
				}
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0005993C File Offset: 0x00057B3C
		public static void AddDefaultHandler()
		{
			lock (SQLiteLog.syncRoot)
			{
				SQLiteLog.InitializeDefaultHandler();
				SQLiteLog.Log += SQLiteLog._defaultHandler;
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0005998C File Offset: 0x00057B8C
		public static void RemoveDefaultHandler()
		{
			lock (SQLiteLog.syncRoot)
			{
				SQLiteLog.InitializeDefaultHandler();
				SQLiteLog.Log -= SQLiteLog._defaultHandler;
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x000599DC File Offset: 0x00057BDC
		private static void LogCallback(IntPtr pUserData, int errorCode, IntPtr pMessage)
		{
			bool flag2;
			SQLiteLogEventHandler sqliteLogEventHandler;
			lock (SQLiteLog.syncRoot)
			{
				if (SQLiteLog._enabled && SQLiteLog._handlers != null)
				{
					flag2 = true;
					sqliteLogEventHandler = SQLiteLog._handlers.Clone() as SQLiteLogEventHandler;
				}
				else
				{
					flag2 = false;
					sqliteLogEventHandler = null;
				}
			}
			if (flag2 && sqliteLogEventHandler != null)
			{
				sqliteLogEventHandler(null, new LogEventArgs(pUserData, errorCode, SQLiteConvert.UTF8ToString(pMessage, -1), null));
			}
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00059A74 File Offset: 0x00057C74
		private static void LogEventHandler(object sender, LogEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			string text = e.Message;
			if (text == null)
			{
				text = "<null>";
			}
			else
			{
				text = text.Trim();
				if (text.Length == 0)
				{
					text = "<empty>";
				}
			}
			object errorCode = e.ErrorCode;
			string text2 = "error";
			if (errorCode is SQLiteErrorCode || errorCode is int)
			{
				SQLiteErrorCode sqliteErrorCode = (SQLiteErrorCode)((int)errorCode);
				sqliteErrorCode &= SQLiteErrorCode.NonExtendedMask;
				if (sqliteErrorCode == SQLiteErrorCode.Ok)
				{
					text2 = "message";
				}
				else if (sqliteErrorCode == SQLiteErrorCode.Notice)
				{
					text2 = "notice";
				}
				else if (sqliteErrorCode == SQLiteErrorCode.Warning)
				{
					text2 = "warning";
				}
				else if (sqliteErrorCode == SQLiteErrorCode.Row || sqliteErrorCode == SQLiteErrorCode.Done)
				{
					text2 = "data";
				}
			}
			else if (errorCode == null)
			{
				text2 = "trace";
			}
			if (errorCode != null && !object.ReferenceEquals(errorCode, string.Empty))
			{
				Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "SQLite {0} ({1}): {2}", new object[] { text2, errorCode, text }));
				return;
			}
			Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "SQLite {0}: {1}", new object[] { text2, text }));
		}

		// Token: 0x040007E6 RID: 2022
		private const int _initializeTimeout = 1000;

		// Token: 0x040007E7 RID: 2023
		private static object syncRoot = new object();

		// Token: 0x040007E8 RID: 2024
		private static EventWaitHandle _initializeEvent;

		// Token: 0x040007E9 RID: 2025
		private static EventHandler _domainUnload;

		// Token: 0x040007EB RID: 2027
		private static SQLiteLogEventHandler _defaultHandler;

		// Token: 0x040007EC RID: 2028
		private static SQLiteLogCallback _callback;

		// Token: 0x040007ED RID: 2029
		private static SQLiteBase _sql;

		// Token: 0x040007EE RID: 2030
		private static int _initializeCallCount;

		// Token: 0x040007EF RID: 2031
		private static int _uninitializeCallCount;

		// Token: 0x040007F0 RID: 2032
		private static int _initializeDoneCount;

		// Token: 0x040007F1 RID: 2033
		private static int _attemptedInitialize;

		// Token: 0x040007F2 RID: 2034
		private static bool _enabled;
	}
}
