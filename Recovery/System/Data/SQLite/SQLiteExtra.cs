using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001BF RID: 447
	[Obfuscation(Feature = "renaming")]
	public static class SQLiteExtra
	{
		// Token: 0x06001438 RID: 5176 RVA: 0x0005D930 File Offset: 0x0005BB30
		private static string GetAssemblyTitle(Assembly assembly)
		{
			if (assembly == null)
			{
				return null;
			}
			try
			{
				if (assembly.IsDefined(typeof(AssemblyTitleAttribute), false))
				{
					object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
					if (customAttributes == null || customAttributes.Length == 0)
					{
						return null;
					}
					AssemblyTitleAttribute assemblyTitleAttribute = customAttributes[0] as AssemblyTitleAttribute;
					if (assemblyTitleAttribute != null)
					{
						return assemblyTitleAttribute.Title;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0005D9C4 File Offset: 0x0005BBC4
		private static string BuildUri(string baseUri, Assembly assembly, string error)
		{
			if (string.IsNullOrEmpty(baseUri))
			{
				return baseUri;
			}
			string assemblyTitle = SQLiteExtra.GetAssemblyTitle(assembly);
			return string.Format("{0}?app={1}&err={2}", baseUri, Uri.EscapeUriString((assemblyTitle != null) ? assemblyTitle : "<unknown>"), Uri.EscapeUriString((error != null) ? error : "<unknown>"));
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0005DA20 File Offset: 0x0005BC20
		private static bool ShouldVerify()
		{
			if (Interlocked.CompareExchange(ref SQLiteExtra.verifyCount, 0, 0) == 0)
			{
				return true;
			}
			if (Interlocked.Increment(ref SQLiteExtra.verifyEveryCount) % 20 == 0)
			{
				return true;
			}
			lock (SQLiteExtra.syncRoot)
			{
				if (SQLiteExtra.verifyEveryWhen == null)
				{
					return true;
				}
				double totalSeconds = DateTime.UtcNow.Subtract(SQLiteExtra.verifyEveryWhen.Value).TotalSeconds;
				if (totalSeconds < 0.0 || totalSeconds > 600.0)
				{
					return true;
				}
			}
			return Environment.GetEnvironmentVariable("AlwaysVerifyLicense") != null;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0005DAFC File Offset: 0x0005BCFC
		private static int InnerVerify(string argument)
		{
			lock (SQLiteExtra.syncRoot)
			{
				if (SQLiteExtra.verifyAssembly == null)
				{
					SQLiteExtra.verifyAssembly = Assembly.Load(string.Format("System.Data.SQLite.SEE.License, Version=1.0.116.0, Culture=neutral, PublicKeyToken={0}, processorArchitecture=MSIL", "433d9874d0bb98c5"));
				}
				if (SQLiteExtra.verifyType == null)
				{
					SQLiteExtra.verifyType = Type.GetType(Assembly.CreateQualifiedName(SQLiteExtra.verifyAssembly.GetName().FullName, "License.Sdk.Library"));
				}
				if (SQLiteExtra.verifyMethodInfo == null)
				{
					SQLiteExtra.verifyMethodInfo = SQLiteExtra.verifyType.GetMethod("Verify");
				}
				string text = Environment.GetEnvironmentVariable(string.Format("Override_{0}_Certificate", "SEE"));
				if (text == null)
				{
					if (SQLiteExtra.verifyDirectory == null)
					{
						if (SQLiteExtra.EntryAssembly != null)
						{
							SQLiteExtra.verifyDirectory = Path.GetDirectoryName(SQLiteExtra.EntryAssembly.Location);
						}
						else if (SQLiteExtra.ThisAssembly != null)
						{
							SQLiteExtra.verifyDirectory = Path.GetDirectoryName(SQLiteExtra.ThisAssembly.Location);
						}
					}
					if (SQLiteExtra.verifyDirectory == null)
					{
						SQLiteExtra.verifyDirectory = Directory.GetCurrentDirectory();
					}
					text = Path.Combine(SQLiteExtra.verifyDirectory, "SDS-SEE.exml");
				}
				IList<string> list = null;
				string text2 = null;
				object[] array = new object[] { null, text, list, text2 };
				if ((bool)SQLiteExtra.verifyMethodInfo.Invoke(null, array))
				{
					list = array[2] as IList<string>;
					if (list == null)
					{
						text2 = "invalid license certificate";
					}
					else if (list.Count < 4)
					{
						text2 = "malformed license certificate";
					}
					else if (!string.Equals(list[0], "fileName", StringComparison.OrdinalIgnoreCase) || !string.Equals(list[1], text, StringComparison.OrdinalIgnoreCase))
					{
						text2 = "bad certificate file name";
					}
					else
					{
						if (string.Equals(list[2], "publicKeyToken", StringComparison.OrdinalIgnoreCase) && string.Equals(list[3], "433d9874d0bb98c5", StringComparison.OrdinalIgnoreCase))
						{
							SQLiteExtra.verifyEveryWhen = new DateTime?(DateTime.UtcNow);
							return 0;
						}
						if (string.Equals(list[2], "error", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(list[3]))
						{
							text2 = list[3];
						}
						else
						{
							text2 = "bad certificate public key";
						}
					}
				}
				else
				{
					text2 = array[3] as string;
				}
				if (Interlocked.CompareExchange(ref SQLiteExtra.preVerifyPendingCount, 0, 0) == 0 && Environment.GetEnvironmentVariable(string.Format("No_{0}_PurchaseUri", "SEE")) == null && Interlocked.Increment(ref SQLiteExtra.verifyPurchaseCount) == 1 && Environment.UserInteractive)
				{
					try
					{
						Process.Start(SQLiteExtra.BuildUri("https://urn.to/r/sds_see", SQLiteExtra.EntryAssembly, text2));
					}
					catch
					{
					}
				}
				throw new NotSupportedException(text2);
			}
			int num;
			return num;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0005DE14 File Offset: 0x0005C014
		private static int InnerConfigure(string argument)
		{
			int num;
			lock (SQLiteExtra.syncRoot)
			{
				if (SQLiteExtra.verifyAssembly == null)
				{
					SQLiteExtra.verifyAssembly = Assembly.Load(string.Format("System.Data.SQLite.SEE.License, Version=1.0.116.0, Culture=neutral, PublicKeyToken={0}, processorArchitecture=MSIL", "433d9874d0bb98c5"));
				}
				if (SQLiteExtra.verifyType == null)
				{
					SQLiteExtra.verifyType = Type.GetType(Assembly.CreateQualifiedName(SQLiteExtra.verifyAssembly.GetName().FullName, "License.Sdk.Library"));
				}
				if (SQLiteExtra.configureMethodInfo == null)
				{
					SQLiteExtra.configureMethodInfo = SQLiteExtra.verifyType.GetMethod("Configure");
				}
				string text = null;
				object[] array = new object[] { argument, text };
				if (!(bool)SQLiteExtra.configureMethodInfo.Invoke(null, array))
				{
					text = array[1] as string;
					throw new NotSupportedException(text);
				}
				SQLiteExtra.configureEveryWhen = new DateTime?(DateTime.UtcNow);
				num = 0;
			}
			return num;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0005DF24 File Offset: 0x0005C124
		private static void PreVerifyCallback(object state)
		{
			if (Interlocked.CompareExchange(ref SQLiteExtra.verifyCount, 0, 0) > 0)
			{
				return;
			}
			Interlocked.Increment(ref SQLiteExtra.preVerifyPendingCount);
			try
			{
				SQLiteExtra.Verify(null);
			}
			catch
			{
			}
			finally
			{
				Interlocked.Decrement(ref SQLiteExtra.preVerifyPendingCount);
			}
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0005DF90 File Offset: 0x0005C190
		private static void TrackNativeEnvironmentVariable(string variable)
		{
			lock (SQLiteExtra.syncRoot)
			{
				if (SQLiteExtra.nativeEnvironmentVariables == null)
				{
					SQLiteExtra.AddNativeExitedEventHandler();
					SQLiteExtra.nativeEnvironmentVariables = new Dictionary<string, string>();
				}
				if (!string.IsNullOrEmpty(variable) && !SQLiteExtra.nativeEnvironmentVariables.ContainsKey(variable))
				{
					SQLiteExtra.nativeEnvironmentVariables.Add(variable, null);
				}
			}
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0005E010 File Offset: 0x0005C210
		private static void CleanupNativeEnvironmentVariables(object sender, EventArgs e)
		{
			lock (SQLiteExtra.syncRoot)
			{
				if (SQLiteExtra.nativeEnvironmentVariables != null)
				{
					int num = 0;
					foreach (KeyValuePair<string, string> keyValuePair in SQLiteExtra.nativeEnvironmentVariables)
					{
						string key = keyValuePair.Key;
						if (!string.IsNullOrEmpty(key))
						{
							try
							{
								Environment.SetEnvironmentVariable(key, null);
							}
							catch (Exception ex)
							{
								Trace.WriteLine(string.Format("Could not delete environment variable \"{0}\": {1}", key, ex));
							}
							num++;
						}
					}
					Trace.WriteLine(string.Format("Deleted {0} native verify callbacks from within application domain {1}.", num, AppDomain.CurrentDomain.Id));
					SQLiteExtra.nativeEnvironmentVariables.Clear();
					SQLiteExtra.nativeEnvironmentVariables = null;
				}
			}
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0005E11C File Offset: 0x0005C31C
		private static void AddNativeExitedEventHandler()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			if (currentDomain.IsDefaultAppDomain())
			{
				currentDomain.ProcessExit -= SQLiteExtra.CleanupNativeEnvironmentVariables;
				currentDomain.ProcessExit += SQLiteExtra.CleanupNativeEnvironmentVariables;
				return;
			}
			currentDomain.DomainUnload -= SQLiteExtra.CleanupNativeEnvironmentVariables;
			currentDomain.DomainUnload += SQLiteExtra.CleanupNativeEnvironmentVariables;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0005E188 File Offset: 0x0005C388
		private static int NativeVerify(IntPtr pCookie)
		{
			int num = SQLiteExtra.Verify(null);
			if (pCookie != IntPtr.Zero)
			{
				Marshal.WriteInt32(pCookie, num);
			}
			return 0;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0005E1B8 File Offset: 0x0005C3B8
		private static int SetupNativeVerify(string variable)
		{
			bool flag = false;
			IntPtr intPtr;
			lock (SQLiteExtra.syncRoot)
			{
				if (SQLiteExtra.nativeMethodInfo == null)
				{
					SQLiteExtra.nativeMethodInfo = typeof(SQLiteExtra).GetMethod("NativeVerify", BindingFlags.Static | BindingFlags.NonPublic);
				}
				if (SQLiteExtra.nativeDelegate == null && SQLiteExtra.nativeMethodInfo != null)
				{
					SQLiteExtra.nativeDelegate = Delegate.CreateDelegate(typeof(SQLiteExtra.FExecuteInAppDomainCallback), SQLiteExtra.nativeMethodInfo);
				}
				if (SQLiteExtra.pNativeCallback == IntPtr.Zero && SQLiteExtra.nativeDelegate != null)
				{
					SQLiteExtra.pNativeCallback = Marshal.GetFunctionPointerForDelegate(SQLiteExtra.nativeDelegate);
					flag = true;
				}
				intPtr = SQLiteExtra.pNativeCallback;
			}
			if (!string.IsNullOrEmpty(variable))
			{
				SQLiteExtra.TrackNativeEnvironmentVariable(variable);
				Environment.SetEnvironmentVariable(variable, intPtr.ToInt64().ToString());
				Trace.WriteLine(string.Format("{0} native verify callback \"{1}\" with value {2} from within application domain {3}.", new object[]
				{
					flag ? "Created" : "Set",
					variable,
					intPtr,
					AppDomain.CurrentDomain.Id
				}));
				return 0;
			}
			return 1;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0005E30C File Offset: 0x0005C50C
		internal static void PreVerify()
		{
			if (Environment.GetEnvironmentVariable(string.Format("No_{0}_PreVerify", "SEE")) == null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(SQLiteExtra.PreVerifyCallback));
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005E33C File Offset: 0x0005C53C
		public static int Verify(string argument)
		{
			DateTime utcNow = DateTime.UtcNow;
			int num;
			try
			{
				if (Environment.GetEnvironmentVariable("LicenseOtherAppDomain") != null && SQLiteExtra.SetupNativeVerify(string.Format("SdkCallback_{0:X}_{1:X}_{2:X}", Process.GetCurrentProcess().Id, AppDomain.CurrentDomain.Id, AppDomain.GetCurrentThreadId())) != 0)
				{
					num = 1;
				}
				else if (!SQLiteExtra.ShouldVerify())
				{
					num = 0;
				}
				else
				{
					num = SQLiteExtra.InnerVerify(argument);
				}
			}
			finally
			{
				int num2 = Interlocked.Increment(ref SQLiteExtra.verifyCount);
				double totalMilliseconds = DateTime.UtcNow.Subtract(utcNow).TotalMilliseconds;
				SQLiteExtra.verifyMilliseconds += totalMilliseconds;
				Trace.WriteLine(string.Format("Verify completed in {0} milliseconds, total of {1} times in {2} milliseconds.", totalMilliseconds, num2, SQLiteExtra.verifyMilliseconds));
			}
			return num;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0005E428 File Offset: 0x0005C628
		public static int Configure(string argument)
		{
			DateTime utcNow = DateTime.UtcNow;
			int num;
			try
			{
				num = SQLiteExtra.InnerConfigure(argument);
			}
			finally
			{
				int num2 = Interlocked.Increment(ref SQLiteExtra.configureCount);
				double totalMilliseconds = DateTime.UtcNow.Subtract(utcNow).TotalMilliseconds;
				SQLiteExtra.configureMilliseconds += totalMilliseconds;
				Trace.WriteLine(string.Format("Configure completed in {0} milliseconds, total of {1} times in {2} milliseconds.", totalMilliseconds, num2, SQLiteExtra.configureMilliseconds));
			}
			return num;
		}

		// Token: 0x0400084B RID: 2123
		private const string VerifyPublicKeyToken = "433d9874d0bb98c5";

		// Token: 0x0400084C RID: 2124
		private const string VerifyAssemblyNameFormat = "System.Data.SQLite.SEE.License, Version=1.0.116.0, Culture=neutral, PublicKeyToken={0}, processorArchitecture=MSIL";

		// Token: 0x0400084D RID: 2125
		private const string VerifyTypeName = "License.Sdk.Library";

		// Token: 0x0400084E RID: 2126
		private const string VerifyMethodName = "Verify";

		// Token: 0x0400084F RID: 2127
		private const string ConfigureMethodName = "Configure";

		// Token: 0x04000850 RID: 2128
		private const string NativeVerifyEnvVarFormat = "SdkCallback_{0:X}_{1:X}_{2:X}";

		// Token: 0x04000851 RID: 2129
		private const string NativeVerifyMethodName = "NativeVerify";

		// Token: 0x04000852 RID: 2130
		private const string FileName = "SDS-SEE.exml";

		// Token: 0x04000853 RID: 2131
		private const string BasePurchaseUri = "https://urn.to/r/sds_see";

		// Token: 0x04000854 RID: 2132
		private const string UnknownValue = "<unknown>";

		// Token: 0x04000855 RID: 2133
		private const string BaseEnvVarName = "SEE";

		// Token: 0x04000856 RID: 2134
		private const string FileNameEnvVarFormat = "Override_{0}_Certificate";

		// Token: 0x04000857 RID: 2135
		private const string NoPurchaseUriEnvVarFormat = "No_{0}_PurchaseUri";

		// Token: 0x04000858 RID: 2136
		private const string NoPreVerifyEnvVarFormat = "No_{0}_PreVerify";

		// Token: 0x04000859 RID: 2137
		private const string OtherAppDomainEnvVarName = "LicenseOtherAppDomain";

		// Token: 0x0400085A RID: 2138
		private const string AlwaysVerifyEnvVarName = "AlwaysVerifyLicense";

		// Token: 0x0400085B RID: 2139
		private const int VerifyEveryCountMaximum = 20;

		// Token: 0x0400085C RID: 2140
		private const int VerifyEverySecondsMaximum = 600;

		// Token: 0x0400085D RID: 2141
		private static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly();

		// Token: 0x0400085E RID: 2142
		private static readonly Assembly ThisAssembly = Assembly.GetExecutingAssembly();

		// Token: 0x0400085F RID: 2143
		private static readonly object syncRoot = new object();

		// Token: 0x04000860 RID: 2144
		private static Assembly verifyAssembly = null;

		// Token: 0x04000861 RID: 2145
		private static Type verifyType = null;

		// Token: 0x04000862 RID: 2146
		private static MethodInfo verifyMethodInfo = null;

		// Token: 0x04000863 RID: 2147
		private static MethodInfo configureMethodInfo = null;

		// Token: 0x04000864 RID: 2148
		private static int preVerifyPendingCount = 0;

		// Token: 0x04000865 RID: 2149
		private static string verifyDirectory = null;

		// Token: 0x04000866 RID: 2150
		private static int verifyCount = 0;

		// Token: 0x04000867 RID: 2151
		private static int configureCount = 0;

		// Token: 0x04000868 RID: 2152
		private static int verifyEveryCount = 0;

		// Token: 0x04000869 RID: 2153
		private static DateTime? verifyEveryWhen = null;

		// Token: 0x0400086A RID: 2154
		private static DateTime? configureEveryWhen = null;

		// Token: 0x0400086B RID: 2155
		private static int verifyPurchaseCount = 0;

		// Token: 0x0400086C RID: 2156
		private static double verifyMilliseconds = 0.0;

		// Token: 0x0400086D RID: 2157
		private static double configureMilliseconds = 0.0;

		// Token: 0x0400086E RID: 2158
		private static MethodInfo nativeMethodInfo = null;

		// Token: 0x0400086F RID: 2159
		private static Delegate nativeDelegate = null;

		// Token: 0x04000870 RID: 2160
		private static IntPtr pNativeCallback = IntPtr.Zero;

		// Token: 0x04000871 RID: 2161
		private static Dictionary<string, string> nativeEnvironmentVariables = null;

		// Token: 0x020002C5 RID: 709
		// (Invoke) Token: 0x06001900 RID: 6400
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FExecuteInAppDomainCallback(IntPtr pCookie);
	}
}
