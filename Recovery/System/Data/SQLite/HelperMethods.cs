using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.Data.SQLite
{
	// Token: 0x020001B5 RID: 437
	internal static class HelperMethods
	{
		// Token: 0x06001321 RID: 4897 RVA: 0x0005B778 File Offset: 0x00059978
		private static int GetProcessId()
		{
			Process currentProcess = Process.GetCurrentProcess();
			if (currentProcess == null)
			{
				return 0;
			}
			return currentProcess.Id;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0005B7A0 File Offset: 0x000599A0
		private static bool IsMono()
		{
			try
			{
				lock (HelperMethods.staticSyncRoot)
				{
					if (HelperMethods.isMono == null)
					{
						HelperMethods.isMono = new bool?(Type.GetType(HelperMethods.MonoRuntimeType) != null);
					}
					return HelperMethods.isMono.Value;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0005B828 File Offset: 0x00059A28
		public static bool IsDotNetCore()
		{
			try
			{
				lock (HelperMethods.staticSyncRoot)
				{
					if (HelperMethods.isDotNetCore == null)
					{
						HelperMethods.isDotNetCore = new bool?(Type.GetType(HelperMethods.DotNetCoreLibType) != null);
					}
					return HelperMethods.isDotNetCore.Value;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0005B8B0 File Offset: 0x00059AB0
		internal static void ResetBreakIntoDebugger()
		{
			lock (HelperMethods.staticSyncRoot)
			{
				HelperMethods.debuggerBreak = null;
			}
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0005B8FC File Offset: 0x00059AFC
		internal static void MaybeBreakIntoDebugger()
		{
			lock (HelperMethods.staticSyncRoot)
			{
				if (HelperMethods.debuggerBreak != null)
				{
					return;
				}
			}
			if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_BreakIntoDebugger", null) != null)
			{
				try
				{
					Console.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Attach a debugger to process {0} and press any key to continue.", new object[] { HelperMethods.GetProcessId() }));
					Console.ReadKey();
				}
				catch (Exception ex)
				{
					try
					{
						Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Failed to issue debugger prompt, {0} may be unusable: {1}", new object[]
						{
							typeof(Console),
							ex
						}));
					}
					catch
					{
					}
				}
				try
				{
					Debugger.Break();
					lock (HelperMethods.staticSyncRoot)
					{
						HelperMethods.debuggerBreak = new bool?(true);
					}
					return;
				}
				catch
				{
					lock (HelperMethods.staticSyncRoot)
					{
						HelperMethods.debuggerBreak = new bool?(false);
					}
					throw;
				}
			}
			lock (HelperMethods.staticSyncRoot)
			{
				HelperMethods.debuggerBreak = new bool?(false);
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0005BAC0 File Offset: 0x00059CC0
		internal static int GetThreadId()
		{
			return AppDomain.GetCurrentThreadId();
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0005BAC8 File Offset: 0x00059CC8
		internal static bool HasFlags(SQLiteConnectionFlags flags, SQLiteConnectionFlags hasFlags)
		{
			return (flags & hasFlags) == hasFlags;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0005BAD0 File Offset: 0x00059CD0
		internal static bool LogPrepare(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogPrepare);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0005BADC File Offset: 0x00059CDC
		internal static bool LogPreBind(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogPreBind);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0005BAE8 File Offset: 0x00059CE8
		internal static bool LogBind(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogBind);
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0005BAF4 File Offset: 0x00059CF4
		internal static bool LogCallbackExceptions(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogCallbackException);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0005BB00 File Offset: 0x00059D00
		internal static bool LogBackup(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogBackup);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0005BB0C File Offset: 0x00059D0C
		internal static bool NoLogModule(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.NoLogModule);
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0005BB1C File Offset: 0x00059D1C
		internal static bool LogModuleError(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogModuleError);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0005BB2C File Offset: 0x00059D2C
		internal static bool LogModuleException(SQLiteConnectionFlags flags)
		{
			return HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogModuleException);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0005BB3C File Offset: 0x00059D3C
		internal static bool IsWindows()
		{
			PlatformID platform = Environment.OSVersion.Platform;
			return platform == PlatformID.Win32S || platform == PlatformID.Win32Windows || platform == PlatformID.Win32NT || platform == PlatformID.WinCE;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0005BB78 File Offset: 0x00059D78
		internal static string StringFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (HelperMethods.IsMono())
			{
				return string.Format(format, args);
			}
			return string.Format(provider, format, args);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0005BB94 File Offset: 0x00059D94
		public static string ToDisplayString(object value)
		{
			if (value == null)
			{
				return "<nullObject>";
			}
			string text = value.ToString();
			if (text.Length == 0)
			{
				return "<emptyString>";
			}
			if (text.IndexOfAny(HelperMethods.SpaceChars) < 0)
			{
				return text;
			}
			return HelperMethods.StringFormat(CultureInfo.InvariantCulture, "\"{0}\"", new object[] { text });
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0005BBF8 File Offset: 0x00059DF8
		public static string ToDisplayString(Array array)
		{
			if (array == null)
			{
				return "<nullArray>";
			}
			if (array.Length == 0)
			{
				return "<emptyArray>";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in array)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(HelperMethods.ToDisplayString(obj));
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Insert(0, '[');
				stringBuilder.Append(']');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400081C RID: 2076
		private const string DisplayNullObject = "<nullObject>";

		// Token: 0x0400081D RID: 2077
		private const string DisplayEmptyString = "<emptyString>";

		// Token: 0x0400081E RID: 2078
		private const string DisplayStringFormat = "\"{0}\"";

		// Token: 0x0400081F RID: 2079
		private const string DisplayNullArray = "<nullArray>";

		// Token: 0x04000820 RID: 2080
		private const string DisplayEmptyArray = "<emptyArray>";

		// Token: 0x04000821 RID: 2081
		private const char ArrayOpen = '[';

		// Token: 0x04000822 RID: 2082
		private const string ElementSeparator = ", ";

		// Token: 0x04000823 RID: 2083
		private const char ArrayClose = ']';

		// Token: 0x04000824 RID: 2084
		private static readonly char[] SpaceChars = new char[] { '\t', '\n', '\r', '\v', '\f', ' ' };

		// Token: 0x04000825 RID: 2085
		private static readonly object staticSyncRoot = new object();

		// Token: 0x04000826 RID: 2086
		private static readonly string MonoRuntimeType = "Mono.Runtime";

		// Token: 0x04000827 RID: 2087
		private static readonly string DotNetCoreLibType = "System.CoreLib";

		// Token: 0x04000828 RID: 2088
		private static bool? isMono = null;

		// Token: 0x04000829 RID: 2089
		private static bool? isDotNetCore = null;

		// Token: 0x0400082A RID: 2090
		private static bool? debuggerBreak = null;
	}
}
