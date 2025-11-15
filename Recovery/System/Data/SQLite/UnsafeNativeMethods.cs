using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Data.SQLite
{
	// Token: 0x020001B9 RID: 441
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x0005BF84 File Offset: 0x0005A184
		static UnsafeNativeMethods()
		{
			UnsafeNativeMethods.Initialize();
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0005C040 File Offset: 0x0005A240
		internal static void Initialize()
		{
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				if (UnsafeNativeMethods._SQLiteNativeModuleHandle != IntPtr.Zero)
				{
					return;
				}
			}
			HelperMethods.MaybeBreakIntoDebugger();
			if (UnsafeNativeMethods.GetSettingValue("No_PreLoadSQLite", null) == null)
			{
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					if (UnsafeNativeMethods.targetFrameworkAbbreviations == null)
					{
						UnsafeNativeMethods.targetFrameworkAbbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v2.0", "net20");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v3.5", "net35");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.0", "net40");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.5", "net45");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.5.1", "net451");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.5.2", "net452");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.6", "net46");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.6.1", "net461");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.6.2", "net462");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.7", "net47");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.7.1", "net471");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.7.2", "net472");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.8", "net48");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETStandard,Version=v2.0", "netstandard2.0");
						UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETStandard,Version=v2.1", "netstandard2.1");
					}
					if (UnsafeNativeMethods.processorArchitecturePlatforms == null)
					{
						UnsafeNativeMethods.processorArchitecturePlatforms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						UnsafeNativeMethods.processorArchitecturePlatforms.Add("x86", "Win32");
						UnsafeNativeMethods.processorArchitecturePlatforms.Add("x86_64", "x64");
						UnsafeNativeMethods.processorArchitecturePlatforms.Add("AMD64", "x64");
						UnsafeNativeMethods.processorArchitecturePlatforms.Add("IA64", "Itanium");
						UnsafeNativeMethods.processorArchitecturePlatforms.Add("ARM", "WinCE");
					}
					if (UnsafeNativeMethods._SQLiteNativeModuleHandle == IntPtr.Zero)
					{
						string text = null;
						string text2 = null;
						bool flag3 = false;
						UnsafeNativeMethods.SearchForDirectory(ref text, ref text2, ref flag3);
						UnsafeNativeMethods.PreLoadSQLiteDll(text, text2, flag3, ref UnsafeNativeMethods._SQLiteNativeModuleFileName, ref UnsafeNativeMethods._SQLiteNativeModuleHandle);
					}
				}
				return;
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0005C2F8 File Offset: 0x0005A4F8
		private static string MaybeCombinePath(string path1, string path2)
		{
			if (path1 != null)
			{
				if (path2 != null)
				{
					return Path.Combine(path1, path2);
				}
				return path1;
			}
			else
			{
				if (path2 != null)
				{
					return path2;
				}
				return null;
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0005C31C File Offset: 0x0005A51C
		private static void ResetCachedXmlConfigFileName()
		{
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				UnsafeNativeMethods.cachedXmlConfigFileName = null;
				UnsafeNativeMethods.noXmlConfigFileName = false;
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0005C368 File Offset: 0x0005A568
		private static string GetCachedXmlConfigFileName()
		{
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				if (UnsafeNativeMethods.cachedXmlConfigFileName != null)
				{
					return UnsafeNativeMethods.cachedXmlConfigFileName;
				}
				if (UnsafeNativeMethods.noXmlConfigFileName)
				{
					return null;
				}
			}
			return UnsafeNativeMethods.GetXmlConfigFileName();
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0005C3D4 File Offset: 0x0005A5D4
		private static string GetXmlConfigFileName()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string text = UnsafeNativeMethods.MaybeCombinePath(baseDirectory, UnsafeNativeMethods.XmlConfigFileName);
			if (File.Exists(text))
			{
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					UnsafeNativeMethods.cachedXmlConfigFileName = text;
				}
				return text;
			}
			text = UnsafeNativeMethods.MaybeCombinePath(baseDirectory, UnsafeNativeMethods.XmlAltConfigFileName);
			if (File.Exists(text))
			{
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					UnsafeNativeMethods.cachedXmlConfigFileName = text;
				}
				return text;
			}
			baseDirectory = UnsafeNativeMethods.GetCachedAssemblyDirectory();
			text = UnsafeNativeMethods.MaybeCombinePath(baseDirectory, UnsafeNativeMethods.XmlConfigFileName);
			if (File.Exists(text))
			{
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					UnsafeNativeMethods.cachedXmlConfigFileName = text;
				}
				return text;
			}
			text = UnsafeNativeMethods.MaybeCombinePath(baseDirectory, UnsafeNativeMethods.XmlAltConfigFileName);
			if (File.Exists(text))
			{
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					UnsafeNativeMethods.cachedXmlConfigFileName = text;
				}
				return text;
			}
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				UnsafeNativeMethods.noXmlConfigFileName = true;
			}
			return null;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0005C570 File Offset: 0x0005A770
		private static string ReplaceXmlConfigFileTokens(string fileName, string value)
		{
			if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(fileName) && value.IndexOf(UnsafeNativeMethods.XmlConfigDirectoryToken) != -1)
			{
				try
				{
					string directoryName = Path.GetDirectoryName(fileName);
					if (!string.IsNullOrEmpty(directoryName))
					{
						value = value.Replace(UnsafeNativeMethods.XmlConfigDirectoryToken, directoryName);
					}
				}
				catch (Exception ex)
				{
					try
					{
						Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to replace XML configuration file \"{0}\" tokens: {1}", new object[] { fileName, ex }));
					}
					catch
					{
					}
				}
			}
			return value;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0005C61C File Offset: 0x0005A81C
		private static string GetSettingValueViaXmlConfigFile(string fileName, string name, string @default, bool expand, bool tokens)
		{
			try
			{
				if (fileName == null || name == null)
				{
					return @default;
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(fileName);
				XmlElement xmlElement = xmlDocument.SelectSingleNode(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "/configuration/appSettings/add[@key='{0}']", new object[] { name })) as XmlElement;
				if (xmlElement != null)
				{
					string text = null;
					if (xmlElement.HasAttribute("value"))
					{
						text = xmlElement.GetAttribute("value");
					}
					if (!string.IsNullOrEmpty(text))
					{
						if (expand)
						{
							text = Environment.ExpandEnvironmentVariables(text);
						}
						if (tokens)
						{
							text = UnsafeNativeMethods.ReplaceEnvironmentVariableTokens(text);
						}
						if (tokens)
						{
							text = UnsafeNativeMethods.ReplaceXmlConfigFileTokens(fileName, text);
						}
					}
					if (text != null)
					{
						return text;
					}
				}
			}
			catch (Exception ex)
			{
				try
				{
					Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to get setting \"{0}\" value from XML configuration file \"{1}\": {2}", new object[] { name, fileName, ex }));
				}
				catch
				{
				}
			}
			return @default;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0005C748 File Offset: 0x0005A948
		private static string GetAssemblyTargetFramework(Assembly assembly)
		{
			if (assembly != null)
			{
				try
				{
					if (assembly.IsDefined(typeof(TargetFrameworkAttribute), false))
					{
						TargetFrameworkAttribute targetFrameworkAttribute = (TargetFrameworkAttribute)assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), false)[0];
						return targetFrameworkAttribute.FrameworkName;
					}
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0005C7BC File Offset: 0x0005A9BC
		private static string AbbreviateTargetFramework(string targetFramework)
		{
			if (!string.IsNullOrEmpty(targetFramework))
			{
				string text;
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					if (UnsafeNativeMethods.targetFrameworkAbbreviations != null && UnsafeNativeMethods.targetFrameworkAbbreviations.TryGetValue(targetFramework, out text))
					{
						return text;
					}
				}
				int num = targetFramework.IndexOf(".NETFramework,Version=v");
				if (num == -1)
				{
					return targetFramework;
				}
				text = targetFramework;
				text = text.Replace(".NETFramework,Version=v", "net");
				text = text.Replace(".", string.Empty);
				num = text.IndexOf(',');
				if (num != -1)
				{
					return text.Substring(0, num);
				}
				return text;
			}
			return targetFramework;
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0005C884 File Offset: 0x0005AA84
		private static string ReplaceEnvironmentVariableTokens(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf(UnsafeNativeMethods.AssemblyDirectoryToken) != -1)
				{
					string text = UnsafeNativeMethods.GetCachedAssemblyDirectory();
					if (!string.IsNullOrEmpty(text))
					{
						try
						{
							value = value.Replace(UnsafeNativeMethods.AssemblyDirectoryToken, text);
						}
						catch (Exception ex)
						{
							try
							{
								Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to replace assembly directory token: {0}", new object[] { ex }));
							}
							catch
							{
							}
						}
					}
				}
				if (value.IndexOf(UnsafeNativeMethods.TargetFrameworkToken) != -1)
				{
					Assembly assembly = null;
					try
					{
						assembly = Assembly.GetExecutingAssembly();
					}
					catch (Exception ex2)
					{
						try
						{
							Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to obtain executing assembly: {0}", new object[] { ex2 }));
						}
						catch
						{
						}
					}
					string text2 = UnsafeNativeMethods.AbbreviateTargetFramework(UnsafeNativeMethods.GetAssemblyTargetFramework(assembly));
					if (!string.IsNullOrEmpty(text2))
					{
						try
						{
							value = value.Replace(UnsafeNativeMethods.TargetFrameworkToken, text2);
						}
						catch (Exception ex3)
						{
							try
							{
								Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to replace target framework token: {0}", new object[] { ex3 }));
							}
							catch
							{
							}
						}
					}
				}
			}
			return value;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0005CA08 File Offset: 0x0005AC08
		internal static string GetSettingValue(string name, string @default)
		{
			if (Environment.GetEnvironmentVariable("No_SQLiteGetSettingValue") != null)
			{
				return @default;
			}
			if (name == null)
			{
				return @default;
			}
			bool flag = true;
			bool flag2 = true;
			if (Environment.GetEnvironmentVariable("No_Expand") != null)
			{
				flag = false;
			}
			else if (Environment.GetEnvironmentVariable(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "No_Expand_{0}", new object[] { name })) != null)
			{
				flag = false;
			}
			if (Environment.GetEnvironmentVariable("No_Tokens") != null)
			{
				flag2 = false;
			}
			else if (Environment.GetEnvironmentVariable(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "No_Tokens_{0}", new object[] { name })) != null)
			{
				flag2 = false;
			}
			string text = Environment.GetEnvironmentVariable(name);
			if (!string.IsNullOrEmpty(text))
			{
				if (flag)
				{
					text = Environment.ExpandEnvironmentVariables(text);
				}
				if (flag2)
				{
					text = UnsafeNativeMethods.ReplaceEnvironmentVariableTokens(text);
				}
			}
			if (text != null)
			{
				return text;
			}
			if (Environment.GetEnvironmentVariable("No_SQLiteXmlConfigFile") != null)
			{
				return @default;
			}
			return UnsafeNativeMethods.GetSettingValueViaXmlConfigFile(UnsafeNativeMethods.GetCachedXmlConfigFileName(), name, @default, flag, flag2);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0005CB08 File Offset: 0x0005AD08
		private static string ListToString(IList<string> list)
		{
			if (list == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in list)
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
			return stringBuilder.ToString();
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0005CB8C File Offset: 0x0005AD8C
		private static int CheckForArchitecturesAndPlatforms(string directory, ref List<string> matches)
		{
			int num = 0;
			if (matches == null)
			{
				matches = new List<string>();
			}
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				if (!string.IsNullOrEmpty(directory) && UnsafeNativeMethods.processorArchitecturePlatforms != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in UnsafeNativeMethods.processorArchitecturePlatforms)
					{
						if (Directory.Exists(UnsafeNativeMethods.MaybeCombinePath(directory, keyValuePair.Key)))
						{
							matches.Add(keyValuePair.Key);
							num++;
						}
						string value = keyValuePair.Value;
						if (value != null && Directory.Exists(UnsafeNativeMethods.MaybeCombinePath(directory, value)))
						{
							matches.Add(value);
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0005CC8C File Offset: 0x0005AE8C
		private static bool CheckAssemblyCodeBase(Assembly assembly, ref string fileName)
		{
			try
			{
				if (assembly == null)
				{
					return false;
				}
				string codeBase = assembly.CodeBase;
				if (string.IsNullOrEmpty(codeBase))
				{
					return false;
				}
				Uri uri = new Uri(codeBase);
				string localPath = uri.LocalPath;
				if (!File.Exists(localPath))
				{
					return false;
				}
				string directoryName = Path.GetDirectoryName(localPath);
				string text = UnsafeNativeMethods.MaybeCombinePath(directoryName, UnsafeNativeMethods.XmlConfigFileName);
				if (File.Exists(text))
				{
					fileName = localPath;
					return true;
				}
				string text2 = UnsafeNativeMethods.MaybeCombinePath(directoryName, UnsafeNativeMethods.XmlAltConfigFileName);
				if (File.Exists(text2))
				{
					fileName = localPath;
					return true;
				}
				List<string> list = null;
				if (UnsafeNativeMethods.CheckForArchitecturesAndPlatforms(directoryName, ref list) > 0)
				{
					fileName = localPath;
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				try
				{
					Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to check code base for currently executing assembly: {0}", new object[] { ex }));
				}
				catch
				{
				}
			}
			return false;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0005CDB0 File Offset: 0x0005AFB0
		private static void ResetCachedAssemblyDirectory()
		{
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				UnsafeNativeMethods.cachedAssemblyDirectory = null;
				UnsafeNativeMethods.noAssemblyDirectory = false;
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0005CDFC File Offset: 0x0005AFFC
		private static string GetCachedAssemblyDirectory()
		{
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				if (UnsafeNativeMethods.cachedAssemblyDirectory != null)
				{
					return UnsafeNativeMethods.cachedAssemblyDirectory;
				}
				if (UnsafeNativeMethods.noAssemblyDirectory)
				{
					return null;
				}
			}
			return UnsafeNativeMethods.GetAssemblyDirectory();
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0005CE68 File Offset: 0x0005B068
		private static string GetAssemblyDirectory()
		{
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				if (executingAssembly == null)
				{
					lock (UnsafeNativeMethods.staticSyncRoot)
					{
						UnsafeNativeMethods.noAssemblyDirectory = true;
					}
					return null;
				}
				string text = null;
				if (!UnsafeNativeMethods.CheckAssemblyCodeBase(executingAssembly, ref text))
				{
					text = executingAssembly.Location;
				}
				if (string.IsNullOrEmpty(text))
				{
					lock (UnsafeNativeMethods.staticSyncRoot)
					{
						UnsafeNativeMethods.noAssemblyDirectory = true;
					}
					return null;
				}
				string directoryName = Path.GetDirectoryName(text);
				if (string.IsNullOrEmpty(directoryName))
				{
					lock (UnsafeNativeMethods.staticSyncRoot)
					{
						UnsafeNativeMethods.noAssemblyDirectory = true;
					}
					return null;
				}
				lock (UnsafeNativeMethods.staticSyncRoot)
				{
					UnsafeNativeMethods.cachedAssemblyDirectory = directoryName;
				}
				return directoryName;
			}
			catch (Exception ex)
			{
				try
				{
					Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to get directory for currently executing assembly: {0}", new object[] { ex }));
				}
				catch
				{
				}
			}
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				UnsafeNativeMethods.noAssemblyDirectory = true;
			}
			return null;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0005D09C File Offset: 0x0005B29C
		internal static string GetNativeModuleFileName()
		{
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				if (UnsafeNativeMethods._SQLiteNativeModuleFileName != null)
				{
					return UnsafeNativeMethods._SQLiteNativeModuleFileName;
				}
			}
			return "SQLite.Interop.dll";
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005D0F8 File Offset: 0x0005B2F8
		internal static string GetNativeLibraryFileNameOnly()
		{
			string settingValue = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_LibraryFileNameOnly", null);
			if (settingValue != null)
			{
				return settingValue;
			}
			return "SQLite.Interop.dll";
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0005D124 File Offset: 0x0005B324
		private static bool SearchForDirectory(ref string baseDirectory, ref string processorArchitecture, ref bool allowBaseDirectoryOnly)
		{
			if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_NoSearchForDirectory", null) != null)
			{
				return false;
			}
			string nativeLibraryFileNameOnly = UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
			if (nativeLibraryFileNameOnly == null)
			{
				return false;
			}
			string[] array = new string[]
			{
				UnsafeNativeMethods.GetAssemblyDirectory(),
				AppDomain.CurrentDomain.BaseDirectory
			};
			string text = null;
			if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_AllowBaseDirectoryOnly", null) != null || (HelperMethods.IsDotNetCore() && !HelperMethods.IsWindows()))
			{
				text = string.Empty;
			}
			string[] array2 = new string[]
			{
				UnsafeNativeMethods.GetProcessorArchitecture(),
				UnsafeNativeMethods.GetPlatformName(null),
				text
			};
			foreach (string text2 in array)
			{
				if (text2 != null)
				{
					foreach (string text3 in array2)
					{
						if (text3 != null)
						{
							string text4 = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(text2, text3), nativeLibraryFileNameOnly));
							if (File.Exists(text4))
							{
								baseDirectory = text2;
								processorArchitecture = text3;
								allowBaseDirectoryOnly = text3.Length == 0;
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0005D274 File Offset: 0x0005B474
		private static string GetBaseDirectory()
		{
			string text = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_BaseDirectory", null);
			if (text != null)
			{
				return text;
			}
			if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_UseAssemblyDirectory", null) != null)
			{
				text = UnsafeNativeMethods.GetAssemblyDirectory();
				if (text != null)
				{
					return text;
				}
			}
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005D2C4 File Offset: 0x0005B4C4
		private static string FixUpDllFileName(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName) && HelperMethods.IsWindows() && !fileName.EndsWith(UnsafeNativeMethods.DllFileExtension, StringComparison.OrdinalIgnoreCase))
			{
				return fileName + UnsafeNativeMethods.DllFileExtension;
			}
			return fileName;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0005D2FC File Offset: 0x0005B4FC
		private static string GetProcessorArchitecture()
		{
			string text = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_ProcessorArchitecture", null);
			if (text != null)
			{
				return text;
			}
			text = UnsafeNativeMethods.GetSettingValue(UnsafeNativeMethods.PROCESSOR_ARCHITECTURE, null);
			if (IntPtr.Size == 4 && string.Equals(text, "AMD64", StringComparison.OrdinalIgnoreCase))
			{
				text = "x86";
			}
			if (text == null)
			{
				text = NativeLibraryHelper.GetMachine();
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0005D368 File Offset: 0x0005B568
		private static string GetPlatformName(string processorArchitecture)
		{
			if (processorArchitecture == null)
			{
				processorArchitecture = UnsafeNativeMethods.GetProcessorArchitecture();
			}
			if (string.IsNullOrEmpty(processorArchitecture))
			{
				return null;
			}
			lock (UnsafeNativeMethods.staticSyncRoot)
			{
				if (UnsafeNativeMethods.processorArchitecturePlatforms == null)
				{
					return null;
				}
				string text;
				if (UnsafeNativeMethods.processorArchitecturePlatforms.TryGetValue(processorArchitecture, out text))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005D3F0 File Offset: 0x0005B5F0
		private static bool PreLoadSQLiteDll(string baseDirectory, string processorArchitecture, bool allowBaseDirectoryOnly, ref string nativeModuleFileName, ref IntPtr nativeModuleHandle)
		{
			if (baseDirectory == null)
			{
				baseDirectory = UnsafeNativeMethods.GetBaseDirectory();
			}
			if (baseDirectory == null)
			{
				return false;
			}
			string nativeLibraryFileNameOnly = UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
			if (nativeLibraryFileNameOnly == null)
			{
				return false;
			}
			string text = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, nativeLibraryFileNameOnly));
			if (File.Exists(text))
			{
				if (!allowBaseDirectoryOnly || !string.IsNullOrEmpty(processorArchitecture))
				{
					return false;
				}
			}
			else
			{
				if (processorArchitecture == null)
				{
					processorArchitecture = UnsafeNativeMethods.GetProcessorArchitecture();
				}
				if (processorArchitecture == null)
				{
					return false;
				}
				text = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, processorArchitecture), nativeLibraryFileNameOnly));
				if (!File.Exists(text))
				{
					string platformName = UnsafeNativeMethods.GetPlatformName(processorArchitecture);
					if (platformName == null)
					{
						return false;
					}
					text = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, platformName), nativeLibraryFileNameOnly));
					if (!File.Exists(text))
					{
						return false;
					}
				}
			}
			try
			{
				try
				{
					Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader is trying to load native SQLite library \"{0}\"...", new object[] { text }));
				}
				catch
				{
				}
				nativeModuleFileName = text;
				nativeModuleHandle = NativeLibraryHelper.LoadLibrary(text);
				return nativeModuleHandle != IntPtr.Zero;
			}
			catch (Exception ex)
			{
				try
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Native library pre-loader failed to load native SQLite library \"{0}\" (getLastError = {1}): {2}", new object[] { text, lastWin32Error, ex }));
				}
				catch
				{
				}
			}
			return false;
		}

		// Token: 0x0600135C RID: 4956
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI6e43416f0f0e0e16")]
		internal static extern IntPtr sqlite3_bind_parameter_name_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x0600135D RID: 4957
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIe8488cc730a7a082")]
		internal static extern IntPtr sqlite3_column_database_name_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x0600135E RID: 4958
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI506e50fb03c2d89a")]
		internal static extern IntPtr sqlite3_column_database_name16_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x0600135F RID: 4959
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIb534977cefbb73a0")]
		internal static extern IntPtr sqlite3_column_decltype_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001360 RID: 4960
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI57cb62b7ae61cb1b")]
		internal static extern IntPtr sqlite3_column_decltype16_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001361 RID: 4961
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIa3d8d3d7074939f8")]
		internal static extern IntPtr sqlite3_column_name_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001362 RID: 4962
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIefe0985609bde5e8")]
		internal static extern IntPtr sqlite3_column_name16_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001363 RID: 4963
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIc28680a63df4f588")]
		internal static extern IntPtr sqlite3_column_origin_name_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001364 RID: 4964
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI2205f161df7092bc")]
		internal static extern IntPtr sqlite3_column_origin_name16_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001365 RID: 4965
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI2a4d8b6ad44eebf1")]
		internal static extern IntPtr sqlite3_column_table_name_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001366 RID: 4966
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI27b50b0098946eb0")]
		internal static extern IntPtr sqlite3_column_table_name16_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001367 RID: 4967
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIac721dd2e74c9941")]
		internal static extern IntPtr sqlite3_column_text_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001368 RID: 4968
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI917c1d969e6de20e")]
		internal static extern IntPtr sqlite3_column_text16_interop(IntPtr stmt, int index, ref int len);

		// Token: 0x06001369 RID: 4969
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIea4bbb6d1ee2a307")]
		internal static extern IntPtr sqlite3_errmsg_interop(IntPtr db, ref int len);

		// Token: 0x0600136A RID: 4970
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI4edf79ad44a9cff4")]
		internal static extern SQLiteErrorCode sqlite3_prepare_interop(IntPtr db, IntPtr pSql, int nBytes, ref IntPtr stmt, ref IntPtr ptrRemain, ref int nRemain);

		// Token: 0x0600136B RID: 4971
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI3f6b70ecace50eb3")]
		internal static extern SQLiteErrorCode sqlite3_table_column_metadata_interop(IntPtr db, byte[] dbName, byte[] tblName, byte[] colName, ref IntPtr ptrDataType, ref IntPtr ptrCollSeq, ref int notNull, ref int primaryKey, ref int autoInc, ref int dtLen, ref int csLen);

		// Token: 0x0600136C RID: 4972
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIc62437a375ae6e3e")]
		internal static extern IntPtr sqlite3_value_text_interop(IntPtr p, ref int len);

		// Token: 0x0600136D RID: 4973
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIe51ef6b50d4bca6d")]
		internal static extern IntPtr sqlite3_value_text16_interop(IntPtr p, ref int len);

		// Token: 0x0600136E RID: 4974
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI772bd4b1b0106e5b")]
		internal static extern int sqlite3_malloc_size_interop(IntPtr p);

		// Token: 0x0600136F RID: 4975
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI4ee0a8e4fbaae675")]
		internal static extern IntPtr interop_libversion();

		// Token: 0x06001370 RID: 4976
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI7b32bbd817c65bb8")]
		internal static extern IntPtr interop_sourceid();

		// Token: 0x06001371 RID: 4977
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI4ca8ca7236e6114c")]
		internal static extern int interop_compileoption_used(IntPtr zOptName);

		// Token: 0x06001372 RID: 4978
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI8a4f24784a6e0dd7")]
		internal static extern IntPtr interop_compileoption_get(int N);

		// Token: 0x06001373 RID: 4979
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI4b285cf3ba62c252")]
		internal static extern SQLiteErrorCode sqlite3_close_interop(IntPtr db);

		// Token: 0x06001374 RID: 4980
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIe2c7699ad7342993")]
		internal static extern SQLiteErrorCode sqlite3_create_function_interop(IntPtr db, byte[] strName, int nArgs, int nType, IntPtr pvUser, SQLiteCallback func, SQLiteCallback fstep, SQLiteFinalCallback ffinal, int needCollSeq);

		// Token: 0x06001375 RID: 4981
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIc6bf4b86ca97d9ec")]
		internal static extern SQLiteErrorCode sqlite3_finalize_interop(IntPtr stmt);

		// Token: 0x06001376 RID: 4982
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI9ee262799bd281d7")]
		internal static extern SQLiteErrorCode sqlite3_backup_finish_interop(IntPtr backup);

		// Token: 0x06001377 RID: 4983
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI577edba978146092")]
		internal static extern SQLiteErrorCode sqlite3_blob_close_interop(IntPtr blob);

		// Token: 0x06001378 RID: 4984
		[DllImport("SQLite.Interop.dll", EntryPoint = "SIb4c632894b76cc1d")]
		internal static extern SQLiteErrorCode sqlite3_open_interop(byte[] utf8Filename, byte[] vfsName, SQLiteOpenFlagsEnum flags, int extFuncs, ref IntPtr db);

		// Token: 0x06001379 RID: 4985
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI6527c62a1dabea11")]
		internal static extern SQLiteErrorCode sqlite3_open16_interop(byte[] utf8Filename, byte[] vfsName, SQLiteOpenFlagsEnum flags, int extFuncs, ref IntPtr db);

		// Token: 0x0600137A RID: 4986
		[DllImport("SQLite.Interop.dll", EntryPoint = "SId8f792fefa4b5275")]
		internal static extern SQLiteErrorCode sqlite3_reset_interop(IntPtr stmt);

		// Token: 0x0600137B RID: 4987
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI5bb9251d9841cbec")]
		internal static extern int sqlite3_changes_interop(IntPtr db);

		// Token: 0x0600137C RID: 4988
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI56aabcf29f5e6720")]
		internal static extern IntPtr sqlite3_context_collseq_interop(IntPtr context, ref int type, ref int enc, ref int len);

		// Token: 0x0600137D RID: 4989
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI9a7f75f0346092f5")]
		internal static extern int sqlite3_context_collcompare_interop(IntPtr context, byte[] p1, int p1len, byte[] p2, int p2len);

		// Token: 0x0600137E RID: 4990
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI782e3aea35580db5")]
		internal static extern SQLiteErrorCode sqlite3_cursor_rowid_interop(IntPtr stmt, int cursor, ref long rowid);

		// Token: 0x0600137F RID: 4991
		[DllImport("SQLite.Interop.dll", EntryPoint = "SI9cb60bb60df261b3")]
		internal static extern SQLiteErrorCode sqlite3_index_column_info_interop(IntPtr db, byte[] catalog, byte[] IndexName, byte[] ColumnName, ref int sortOrder, ref int onError, ref IntPtr Collation, ref int colllen);

		// Token: 0x06001380 RID: 4992
		[DllImport("SQLite.Interop.dll", EntryPoint = "SId8f3329973466732")]
		internal static extern int sqlite3_table_cursor_interop(IntPtr stmt, int db, int tableRootPage);

		// Token: 0x06001381 RID: 4993
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf36d68994f8a3dbc")]
		internal static extern IntPtr sqlite3_libversion();

		// Token: 0x06001382 RID: 4994
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI386737cd2a0fdd19")]
		internal static extern int sqlite3_libversion_number();

		// Token: 0x06001383 RID: 4995
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI24f086e117ed378d")]
		internal static extern IntPtr sqlite3_sourceid();

		// Token: 0x06001384 RID: 4996
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7520d000053e0d14")]
		internal static extern int sqlite3_compileoption_used(IntPtr zOptName);

		// Token: 0x06001385 RID: 4997
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI609a818d5fbe9ac9")]
		internal static extern IntPtr sqlite3_compileoption_get(int N);

		// Token: 0x06001386 RID: 4998
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa6ac334abbe174be")]
		internal static extern SQLiteErrorCode sqlite3_enable_shared_cache(int enable);

		// Token: 0x06001387 RID: 4999
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2711207af0065fc8")]
		internal static extern SQLiteErrorCode sqlite3_enable_load_extension(IntPtr db, int enable);

		// Token: 0x06001388 RID: 5000
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7c80035d85f12db7")]
		internal static extern SQLiteErrorCode sqlite3_load_extension(IntPtr db, byte[] fileName, byte[] procName, ref IntPtr pError);

		// Token: 0x06001389 RID: 5001
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI1f188bbd4af5db07")]
		internal static extern SQLiteErrorCode sqlite3_overload_function(IntPtr db, IntPtr zName, int nArgs);

		// Token: 0x0600138A RID: 5002
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "SI6199891c0152daea")]
		internal static extern SQLiteErrorCode sqlite3_win32_set_directory(uint type, string value);

		// Token: 0x0600138B RID: 5003
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI5f81f83484be3abd")]
		internal static extern SQLiteErrorCode sqlite3_win32_reset_heap();

		// Token: 0x0600138C RID: 5004
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIb90da090982f4b08")]
		internal static extern SQLiteErrorCode sqlite3_win32_compact_heap(ref uint largest);

		// Token: 0x0600138D RID: 5005
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId2c7880f24f06d7c")]
		internal static extern IntPtr sqlite3_malloc(int n);

		// Token: 0x0600138E RID: 5006
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI528b790fd6ac4633")]
		internal static extern IntPtr sqlite3_malloc64(ulong n);

		// Token: 0x0600138F RID: 5007
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI294b84466eaff39a")]
		internal static extern IntPtr sqlite3_realloc(IntPtr p, int n);

		// Token: 0x06001390 RID: 5008
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0628e029b7e108db")]
		internal static extern IntPtr sqlite3_realloc64(IntPtr p, ulong n);

		// Token: 0x06001391 RID: 5009
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIee287d5c749c90fb")]
		internal static extern ulong sqlite3_msize(IntPtr p);

		// Token: 0x06001392 RID: 5010
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2f014207a555e535")]
		internal static extern void sqlite3_free(IntPtr p);

		// Token: 0x06001393 RID: 5011
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa97f15c7a9a3fcec")]
		internal static extern void sqlite3_interrupt(IntPtr db);

		// Token: 0x06001394 RID: 5012
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbadadf3b9880be1c")]
		internal static extern long sqlite3_last_insert_rowid(IntPtr db);

		// Token: 0x06001395 RID: 5013
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIac17d6653268c29d")]
		internal static extern int sqlite3_changes(IntPtr db);

		// Token: 0x06001396 RID: 5014
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI98872790d4e8efd1")]
		internal static extern long sqlite3_memory_used();

		// Token: 0x06001397 RID: 5015
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIef4f0710e0d4fe79")]
		internal static extern long sqlite3_memory_highwater(int resetFlag);

		// Token: 0x06001398 RID: 5016
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI9d59c342e257f634")]
		internal static extern SQLiteErrorCode sqlite3_shutdown();

		// Token: 0x06001399 RID: 5017
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI6d1c17b1fb262865")]
		internal static extern SQLiteErrorCode sqlite3_busy_timeout(IntPtr db, int ms);

		// Token: 0x0600139A RID: 5018
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI9ce43ab9f0988669")]
		internal static extern SQLiteErrorCode sqlite3_clear_bindings(IntPtr stmt);

		// Token: 0x0600139B RID: 5019
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2b645959c9369085")]
		internal static extern SQLiteErrorCode sqlite3_bind_blob(IntPtr stmt, int index, byte[] value, int nSize, IntPtr nTransient);

		// Token: 0x0600139C RID: 5020
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbae16952f377e56d")]
		internal static extern SQLiteErrorCode sqlite3_bind_double(IntPtr stmt, int index, double value);

		// Token: 0x0600139D RID: 5021
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa56f9b6b87626f82")]
		internal static extern SQLiteErrorCode sqlite3_bind_int(IntPtr stmt, int index, int value);

		// Token: 0x0600139E RID: 5022
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa56f9b6b87626f82")]
		internal static extern SQLiteErrorCode sqlite3_bind_uint(IntPtr stmt, int index, uint value);

		// Token: 0x0600139F RID: 5023
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0cff507887d45bcd")]
		internal static extern SQLiteErrorCode sqlite3_bind_int64(IntPtr stmt, int index, long value);

		// Token: 0x060013A0 RID: 5024
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0cff507887d45bcd")]
		internal static extern SQLiteErrorCode sqlite3_bind_uint64(IntPtr stmt, int index, ulong value);

		// Token: 0x060013A1 RID: 5025
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbbe901f293754481")]
		internal static extern SQLiteErrorCode sqlite3_bind_null(IntPtr stmt, int index);

		// Token: 0x060013A2 RID: 5026
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI6d7b2699ee23d3f8")]
		internal static extern SQLiteErrorCode sqlite3_bind_text(IntPtr stmt, int index, byte[] value, int nlen, IntPtr pvReserved);

		// Token: 0x060013A3 RID: 5027
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI9a83e170c880ff66")]
		internal static extern int sqlite3_bind_parameter_count(IntPtr stmt);

		// Token: 0x060013A4 RID: 5028
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId6fdbb9d0d4025e5")]
		internal static extern int sqlite3_bind_parameter_index(IntPtr stmt, byte[] strName);

		// Token: 0x060013A5 RID: 5029
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0bdc6d0c384d2e10")]
		internal static extern int sqlite3_column_count(IntPtr stmt);

		// Token: 0x060013A6 RID: 5030
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI670f4f88bee34d06")]
		internal static extern SQLiteErrorCode sqlite3_step(IntPtr stmt);

		// Token: 0x060013A7 RID: 5031
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI258150823cbe6bde")]
		internal static extern int sqlite3_stmt_readonly(IntPtr stmt);

		// Token: 0x060013A8 RID: 5032
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc15bffd7e1940157")]
		internal static extern double sqlite3_column_double(IntPtr stmt, int index);

		// Token: 0x060013A9 RID: 5033
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI157f8e08193aa53b")]
		internal static extern int sqlite3_column_int(IntPtr stmt, int index);

		// Token: 0x060013AA RID: 5034
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf55420fc5e7a71c7")]
		internal static extern long sqlite3_column_int64(IntPtr stmt, int index);

		// Token: 0x060013AB RID: 5035
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI6986c52c0c452cc1")]
		internal static extern IntPtr sqlite3_column_blob(IntPtr stmt, int index);

		// Token: 0x060013AC RID: 5036
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI45fe9413d5eae770")]
		internal static extern int sqlite3_column_bytes(IntPtr stmt, int index);

		// Token: 0x060013AD RID: 5037
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIb8b2ca972163fd1d")]
		internal static extern int sqlite3_column_bytes16(IntPtr stmt, int index);

		// Token: 0x060013AE RID: 5038
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIfc766c3d655f3fe2")]
		internal static extern TypeAffinity sqlite3_column_type(IntPtr stmt, int index);

		// Token: 0x060013AF RID: 5039
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0da8b85fb82501d5")]
		internal static extern SQLiteErrorCode sqlite3_create_collation(IntPtr db, byte[] strName, int nType, IntPtr pvUser, SQLiteCollation func);

		// Token: 0x060013B0 RID: 5040
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI95b0bda85d917b2e")]
		internal static extern int sqlite3_aggregate_count(IntPtr context);

		// Token: 0x060013B1 RID: 5041
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI475081909f1229d5")]
		internal static extern IntPtr sqlite3_value_blob(IntPtr p);

		// Token: 0x060013B2 RID: 5042
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI1773c1143669a2d4")]
		internal static extern int sqlite3_value_bytes(IntPtr p);

		// Token: 0x060013B3 RID: 5043
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI3c17a08e4bd9b715")]
		internal static extern int sqlite3_value_bytes16(IntPtr p);

		// Token: 0x060013B4 RID: 5044
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI11726a584883eb5b")]
		internal static extern double sqlite3_value_double(IntPtr p);

		// Token: 0x060013B5 RID: 5045
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI179fdc030276548d")]
		internal static extern int sqlite3_value_int(IntPtr p);

		// Token: 0x060013B6 RID: 5046
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI74043e952a2cd403")]
		internal static extern long sqlite3_value_int64(IntPtr p);

		// Token: 0x060013B7 RID: 5047
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI33ef63550c5242fb")]
		internal static extern TypeAffinity sqlite3_value_type(IntPtr p);

		// Token: 0x060013B8 RID: 5048
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI962c27000aacea2e")]
		internal static extern void sqlite3_result_blob(IntPtr context, byte[] value, int nSize, IntPtr pvReserved);

		// Token: 0x060013B9 RID: 5049
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId81cf9ee807123a7")]
		internal static extern void sqlite3_result_double(IntPtr context, double value);

		// Token: 0x060013BA RID: 5050
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIebbb430d08e906d0")]
		internal static extern void sqlite3_result_error(IntPtr context, byte[] strErr, int nLen);

		// Token: 0x060013BB RID: 5051
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId81be8ee89587e08")]
		internal static extern void sqlite3_result_error_code(IntPtr context, SQLiteErrorCode value);

		// Token: 0x060013BC RID: 5052
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI8181a48bde70c668")]
		internal static extern void sqlite3_result_error_toobig(IntPtr context);

		// Token: 0x060013BD RID: 5053
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIdeafee00e45abdd7")]
		internal static extern void sqlite3_result_error_nomem(IntPtr context);

		// Token: 0x060013BE RID: 5054
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2d11646dfe5d0c6a")]
		internal static extern void sqlite3_result_value(IntPtr context, IntPtr value);

		// Token: 0x060013BF RID: 5055
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa487294f7b3982b5")]
		internal static extern void sqlite3_result_zeroblob(IntPtr context, int nLen);

		// Token: 0x060013C0 RID: 5056
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2560e79b9e8e0efc")]
		internal static extern void sqlite3_result_int(IntPtr context, int value);

		// Token: 0x060013C1 RID: 5057
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI870da7e43adb4ee2")]
		internal static extern void sqlite3_result_int64(IntPtr context, long value);

		// Token: 0x060013C2 RID: 5058
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIff65f01991d44537")]
		internal static extern void sqlite3_result_null(IntPtr context);

		// Token: 0x060013C3 RID: 5059
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbbfce5c96068face")]
		internal static extern void sqlite3_result_text(IntPtr context, byte[] value, int nLen, IntPtr pvReserved);

		// Token: 0x060013C4 RID: 5060
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf78f8f1191d53f8b")]
		internal static extern IntPtr sqlite3_aggregate_context(IntPtr context, int nBytes);

		// Token: 0x060013C5 RID: 5061
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "SI486b02ac296e0b17")]
		internal static extern SQLiteErrorCode sqlite3_bind_text16(IntPtr stmt, int index, string value, int nlen, IntPtr pvReserved);

		// Token: 0x060013C6 RID: 5062
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "SIb7ca6c18f9b627e3")]
		internal static extern void sqlite3_result_error16(IntPtr context, string strName, int nLen);

		// Token: 0x060013C7 RID: 5063
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "SIcd04f8e391a31c9f")]
		internal static extern void sqlite3_result_text16(IntPtr context, string strName, int nLen, IntPtr pvReserved);

		// Token: 0x060013C8 RID: 5064
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI1def9b218f2b4647")]
		internal static extern SQLiteErrorCode sqlite3_key(IntPtr db, byte[] key, int keylen);

		// Token: 0x060013C9 RID: 5065
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0938f783b19237a0")]
		internal static extern SQLiteErrorCode sqlite3_rekey(IntPtr db, byte[] key, int keylen);

		// Token: 0x060013CA RID: 5066
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId2c89445431c35db")]
		internal static extern void sqlite3_busy_handler(IntPtr db, SQLiteBusyCallback func, IntPtr pvUser);

		// Token: 0x060013CB RID: 5067
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIeab73374c21d5265")]
		internal static extern void sqlite3_progress_handler(IntPtr db, int ops, SQLiteProgressCallback func, IntPtr pvUser);

		// Token: 0x060013CC RID: 5068
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIfdd143ea824fb4f0")]
		internal static extern IntPtr sqlite3_set_authorizer(IntPtr db, SQLiteAuthorizerCallback func, IntPtr pvUser);

		// Token: 0x060013CD RID: 5069
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI1d81196412d3ef05")]
		internal static extern IntPtr sqlite3_update_hook(IntPtr db, SQLiteUpdateCallback func, IntPtr pvUser);

		// Token: 0x060013CE RID: 5070
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI054e3c8468772b2b")]
		internal static extern IntPtr sqlite3_commit_hook(IntPtr db, SQLiteCommitCallback func, IntPtr pvUser);

		// Token: 0x060013CF RID: 5071
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2a62764d0ab77e14")]
		internal static extern IntPtr sqlite3_trace(IntPtr db, SQLiteTraceCallback func, IntPtr pvUser);

		// Token: 0x060013D0 RID: 5072
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIe1f990609c49e19e")]
		internal static extern IntPtr sqlite3_trace_v2(IntPtr db, SQLiteTraceFlags mask, SQLiteTraceCallback2 func, IntPtr pvUser);

		// Token: 0x060013D1 RID: 5073
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI690e97ad40a6d636")]
		internal static extern int sqlite3_limit(IntPtr db, SQLiteLimitOpsEnum op, int value);

		// Token: 0x060013D2 RID: 5074
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7fca2652f71267db")]
		internal static extern SQLiteErrorCode sqlite3_config_none(SQLiteConfigOpsEnum op);

		// Token: 0x060013D3 RID: 5075
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7fca2652f71267db")]
		internal static extern SQLiteErrorCode sqlite3_config_int(SQLiteConfigOpsEnum op, int value);

		// Token: 0x060013D4 RID: 5076
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7fca2652f71267db")]
		internal static extern SQLiteErrorCode sqlite3_config_log(SQLiteConfigOpsEnum op, SQLiteLogCallback func, IntPtr pvUser);

		// Token: 0x060013D5 RID: 5077
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc5e7050b98faec6c")]
		internal static extern SQLiteErrorCode sqlite3_db_config_charptr(IntPtr db, SQLiteConfigDbOpsEnum op, IntPtr charPtr);

		// Token: 0x060013D6 RID: 5078
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc5e7050b98faec6c")]
		internal static extern SQLiteErrorCode sqlite3_db_config_int_refint(IntPtr db, SQLiteConfigDbOpsEnum op, int value, ref int result);

		// Token: 0x060013D7 RID: 5079
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc5e7050b98faec6c")]
		internal static extern SQLiteErrorCode sqlite3_db_config_intptr_two_ints(IntPtr db, SQLiteConfigDbOpsEnum op, IntPtr ptr, int int0, int int1);

		// Token: 0x060013D8 RID: 5080
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbaf8241fbd0ce62c")]
		internal static extern SQLiteErrorCode sqlite3_db_status(IntPtr db, SQLiteStatusOpsEnum op, ref int current, ref int highwater, int resetFlag);

		// Token: 0x060013D9 RID: 5081
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI61339d837097a253")]
		internal static extern IntPtr sqlite3_rollback_hook(IntPtr db, SQLiteRollbackCallback func, IntPtr pvUser);

		// Token: 0x060013DA RID: 5082
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIb3a698858b4f4c1e")]
		internal static extern IntPtr sqlite3_db_handle(IntPtr stmt);

		// Token: 0x060013DB RID: 5083
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf17d2cb586e5ed0b")]
		internal static extern SQLiteErrorCode sqlite3_db_release_memory(IntPtr db);

		// Token: 0x060013DC RID: 5084
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI92a9d705cf067ce9")]
		internal static extern IntPtr sqlite3_db_filename(IntPtr db, IntPtr dbName);

		// Token: 0x060013DD RID: 5085
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbcc66e0ffc3a4a1a")]
		internal static extern int sqlite3_db_readonly(IntPtr db, IntPtr dbName);

		// Token: 0x060013DE RID: 5086
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI92a9d705cf067ce9")]
		internal static extern IntPtr sqlite3_db_filename_bytes(IntPtr db, byte[] dbName);

		// Token: 0x060013DF RID: 5087
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI077534168313d3ed")]
		internal static extern IntPtr sqlite3_next_stmt(IntPtr db, IntPtr stmt);

		// Token: 0x060013E0 RID: 5088
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI13396627163b7740")]
		internal static extern SQLiteErrorCode sqlite3_exec(IntPtr db, byte[] strSql, IntPtr pvCallback, IntPtr pvParam, ref IntPtr errMsg);

		// Token: 0x060013E1 RID: 5089
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI28dd7560c8be508f")]
		internal static extern int sqlite3_release_memory(int nBytes);

		// Token: 0x060013E2 RID: 5090
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI43bfa22e745af6d8")]
		internal static extern int sqlite3_get_autocommit(IntPtr db);

		// Token: 0x060013E3 RID: 5091
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI6b1ea8359a4bab8f")]
		internal static extern SQLiteErrorCode sqlite3_extended_result_codes(IntPtr db, int onoff);

		// Token: 0x060013E4 RID: 5092
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI5b8ed01c5811b4d5")]
		internal static extern SQLiteErrorCode sqlite3_errcode(IntPtr db);

		// Token: 0x060013E5 RID: 5093
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIbf23d3c9d85dbdc9")]
		internal static extern SQLiteErrorCode sqlite3_extended_errcode(IntPtr db);

		// Token: 0x060013E6 RID: 5094
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI55f8c019b2e5ed58")]
		internal static extern IntPtr sqlite3_errstr(SQLiteErrorCode rc);

		// Token: 0x060013E7 RID: 5095
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI23fce3fb79cbb177")]
		internal static extern void sqlite3_log(SQLiteErrorCode iErrCode, byte[] zFormat);

		// Token: 0x060013E8 RID: 5096
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7fbe039e0a0a9e80")]
		internal static extern SQLiteErrorCode sqlite3_file_control(IntPtr db, byte[] zDbName, int op, IntPtr pArg);

		// Token: 0x060013E9 RID: 5097
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI21a888837ee363be")]
		internal static extern IntPtr sqlite3_backup_init(IntPtr destDb, byte[] zDestName, IntPtr sourceDb, byte[] zSourceName);

		// Token: 0x060013EA RID: 5098
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI72ec7143af42ede1")]
		internal static extern SQLiteErrorCode sqlite3_backup_step(IntPtr backup, int nPage);

		// Token: 0x060013EB RID: 5099
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc6d60081c2db6a17")]
		internal static extern int sqlite3_backup_remaining(IntPtr backup);

		// Token: 0x060013EC RID: 5100
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIdff2308d05ffee18")]
		internal static extern int sqlite3_backup_pagecount(IntPtr backup);

		// Token: 0x060013ED RID: 5101
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa3f72e1fcd28493d")]
		internal static extern SQLiteErrorCode sqlite3_blob_close(IntPtr blob);

		// Token: 0x060013EE RID: 5102
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI8a4e5681c1906ed7")]
		internal static extern int sqlite3_blob_bytes(IntPtr blob);

		// Token: 0x060013EF RID: 5103
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIb3a01e7c57f0fe15")]
		internal static extern SQLiteErrorCode sqlite3_blob_open(IntPtr db, byte[] dbName, byte[] tblName, byte[] colName, long rowId, int flags, ref IntPtr ptrBlob);

		// Token: 0x060013F0 RID: 5104
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7f61887459ad69d9")]
		internal static extern SQLiteErrorCode sqlite3_blob_read(IntPtr blob, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, int count, int offset);

		// Token: 0x060013F1 RID: 5105
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI89eaf1bd7a956980")]
		internal static extern SQLiteErrorCode sqlite3_blob_reopen(IntPtr blob, long rowId);

		// Token: 0x060013F2 RID: 5106
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI9142d5013e275b68")]
		internal static extern SQLiteErrorCode sqlite3_blob_write(IntPtr blob, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, int count, int offset);

		// Token: 0x060013F3 RID: 5107
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI255d44cceccc3fc7")]
		internal static extern SQLiteErrorCode sqlite3_declare_vtab(IntPtr db, IntPtr zSQL);

		// Token: 0x060013F4 RID: 5108
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId644b1bddd2a2bbd")]
		internal static extern IntPtr sqlite3_mprintf(IntPtr format, __arglist);

		// Token: 0x060013F5 RID: 5109
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf7e752fd3d0936c7")]
		internal static extern IntPtr sqlite3_create_disposable_module(IntPtr db, IntPtr name, ref UnsafeNativeMethods.sqlite3_module module, IntPtr pClientData, UnsafeNativeMethods.xDestroyModule xDestroy);

		// Token: 0x060013F6 RID: 5110
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIcdd4568419784a8a")]
		internal static extern void sqlite3_dispose_module(IntPtr pModule);

		// Token: 0x060013F7 RID: 5111
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI5f6198a74f6a66c1")]
		internal static extern long sqlite3session_memory_used(IntPtr session);

		// Token: 0x060013F8 RID: 5112
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI47e966d3c8ed1d1c")]
		internal static extern SQLiteErrorCode sqlite3session_create(IntPtr db, byte[] dbName, ref IntPtr session);

		// Token: 0x060013F9 RID: 5113
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0d6b1c4a8f9f99d4")]
		internal static extern void sqlite3session_delete(IntPtr session);

		// Token: 0x060013FA RID: 5114
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa2d290f3cce83950")]
		internal static extern int sqlite3session_enable(IntPtr session, int enable);

		// Token: 0x060013FB RID: 5115
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI44d6ee46b9c95458")]
		internal static extern int sqlite3session_indirect(IntPtr session, int indirect);

		// Token: 0x060013FC RID: 5116
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI02815e94e2c315f7")]
		internal static extern SQLiteErrorCode sqlite3session_attach(IntPtr session, byte[] tblName);

		// Token: 0x060013FD RID: 5117
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf317063645427306")]
		internal static extern void sqlite3session_table_filter(IntPtr session, UnsafeNativeMethods.xSessionFilter xFilter, IntPtr context);

		// Token: 0x060013FE RID: 5118
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7d59e220321e2f41")]
		internal static extern SQLiteErrorCode sqlite3session_changeset(IntPtr session, ref int nChangeSet, ref IntPtr pChangeSet);

		// Token: 0x060013FF RID: 5119
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI039413da2b10b773")]
		internal static extern SQLiteErrorCode sqlite3session_diff(IntPtr session, byte[] fromDbName, byte[] tblName, ref IntPtr errMsg);

		// Token: 0x06001400 RID: 5120
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI5ca7a2a9e7f1b871")]
		internal static extern SQLiteErrorCode sqlite3session_patchset(IntPtr session, ref int nPatchSet, ref IntPtr pPatchSet);

		// Token: 0x06001401 RID: 5121
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIea85a8543a7ed080")]
		internal static extern int sqlite3session_isempty(IntPtr session);

		// Token: 0x06001402 RID: 5122
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIb01052dff9582bba")]
		internal static extern SQLiteErrorCode sqlite3changeset_start(ref IntPtr iterator, int nChangeSet, IntPtr pChangeSet);

		// Token: 0x06001403 RID: 5123
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SId93ded5c6e4c47cf")]
		internal static extern SQLiteErrorCode sqlite3changeset_start_v2(ref IntPtr iterator, int nChangeSet, IntPtr pChangeSet, SQLiteChangeSetStartFlags flags);

		// Token: 0x06001404 RID: 5124
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI56bf96ba405a1160")]
		internal static extern SQLiteErrorCode sqlite3changeset_next(IntPtr iterator);

		// Token: 0x06001405 RID: 5125
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI3ce20b46a489236f")]
		internal static extern SQLiteErrorCode sqlite3changeset_op(IntPtr iterator, ref IntPtr pTblName, ref int nColumns, ref SQLiteAuthorizerActionCode op, ref int bIndirect);

		// Token: 0x06001406 RID: 5126
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc8187efca8c1cb7a")]
		internal static extern SQLiteErrorCode sqlite3changeset_pk(IntPtr iterator, ref IntPtr pPrimaryKeys, ref int nColumns);

		// Token: 0x06001407 RID: 5127
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIa6adfe554ffc7cbf")]
		internal static extern SQLiteErrorCode sqlite3changeset_old(IntPtr iterator, int columnIndex, ref IntPtr pValue);

		// Token: 0x06001408 RID: 5128
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI599b44278adc9bc1")]
		internal static extern SQLiteErrorCode sqlite3changeset_new(IntPtr iterator, int columnIndex, ref IntPtr pValue);

		// Token: 0x06001409 RID: 5129
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI2eb4079ca3a05cbf")]
		internal static extern SQLiteErrorCode sqlite3changeset_conflict(IntPtr iterator, int columnIndex, ref IntPtr pValue);

		// Token: 0x0600140A RID: 5130
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI6c336868b0a5fe32")]
		internal static extern SQLiteErrorCode sqlite3changeset_fk_conflicts(IntPtr iterator, ref int conflicts);

		// Token: 0x0600140B RID: 5131
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7e589f074eae40b8")]
		internal static extern SQLiteErrorCode sqlite3changeset_finalize(IntPtr iterator);

		// Token: 0x0600140C RID: 5132
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI0a886fd4c720116f")]
		internal static extern SQLiteErrorCode sqlite3changeset_invert(int nIn, IntPtr pIn, ref int nOut, ref IntPtr pOut);

		// Token: 0x0600140D RID: 5133
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIcf8c2c23760a4499")]
		internal static extern SQLiteErrorCode sqlite3changeset_concat(int nA, IntPtr pA, int nB, IntPtr pB, ref int nOut, ref IntPtr pOut);

		// Token: 0x0600140E RID: 5134
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI341218eede1b5e9b")]
		internal static extern SQLiteErrorCode sqlite3changegroup_new(ref IntPtr changeGroup);

		// Token: 0x0600140F RID: 5135
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI1e14e4ca4a95bf49")]
		internal static extern SQLiteErrorCode sqlite3changegroup_add(IntPtr changeGroup, int nData, IntPtr pData);

		// Token: 0x06001410 RID: 5136
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIb3adde975ae37b85")]
		internal static extern SQLiteErrorCode sqlite3changegroup_output(IntPtr changeGroup, ref int nData, ref IntPtr pData);

		// Token: 0x06001411 RID: 5137
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI5f8c56b9ec22dac6")]
		internal static extern void sqlite3changegroup_delete(IntPtr changeGroup);

		// Token: 0x06001412 RID: 5138
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIc2f1bb5274db50ad")]
		internal static extern SQLiteErrorCode sqlite3changeset_apply(IntPtr db, int nChangeSet, IntPtr pChangeSet, UnsafeNativeMethods.xSessionFilter xFilter, UnsafeNativeMethods.xSessionConflict xConflict, IntPtr context);

		// Token: 0x06001413 RID: 5139
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI66864656aba1350f")]
		internal static extern SQLiteErrorCode sqlite3changeset_apply_strm(IntPtr db, UnsafeNativeMethods.xSessionInput xInput, IntPtr pIn, UnsafeNativeMethods.xSessionFilter xFilter, UnsafeNativeMethods.xSessionConflict xConflict, IntPtr context);

		// Token: 0x06001414 RID: 5140
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SIf5788ad6b55cce99")]
		internal static extern SQLiteErrorCode sqlite3changeset_concat_strm(UnsafeNativeMethods.xSessionInput xInputA, IntPtr pInA, UnsafeNativeMethods.xSessionInput xInputB, IntPtr pInB, UnsafeNativeMethods.xSessionOutput xOutput, IntPtr pOut);

		// Token: 0x06001415 RID: 5141
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI61ebe33d35212c42")]
		internal static extern SQLiteErrorCode sqlite3changeset_invert_strm(UnsafeNativeMethods.xSessionInput xInput, IntPtr pIn, UnsafeNativeMethods.xSessionOutput xOutput, IntPtr pOut);

		// Token: 0x06001416 RID: 5142
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI9f7fed40b32f2518")]
		internal static extern SQLiteErrorCode sqlite3changeset_start_strm(ref IntPtr iterator, UnsafeNativeMethods.xSessionInput xInput, IntPtr pIn);

		// Token: 0x06001417 RID: 5143
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7ba05ce94ad6ec39")]
		internal static extern SQLiteErrorCode sqlite3changeset_start_v2_strm(ref IntPtr iterator, UnsafeNativeMethods.xSessionInput xInput, IntPtr pIn, SQLiteChangeSetStartFlags flags);

		// Token: 0x06001418 RID: 5144
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7a5090ee686ee20d")]
		internal static extern SQLiteErrorCode sqlite3session_changeset_strm(IntPtr session, UnsafeNativeMethods.xSessionOutput xOutput, IntPtr pOut);

		// Token: 0x06001419 RID: 5145
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI3997e7de4e90e1b3")]
		internal static extern SQLiteErrorCode sqlite3session_patchset_strm(IntPtr session, UnsafeNativeMethods.xSessionOutput xOutput, IntPtr pOut);

		// Token: 0x0600141A RID: 5146
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI7cec4139b0e05c44")]
		internal static extern SQLiteErrorCode sqlite3changegroup_add_strm(IntPtr changeGroup, UnsafeNativeMethods.xSessionInput xInput, IntPtr pIn);

		// Token: 0x0600141B RID: 5147
		[DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SI9086cecb6f6cb06e")]
		internal static extern SQLiteErrorCode sqlite3changegroup_output_strm(IntPtr changeGroup, UnsafeNativeMethods.xSessionOutput xOutput, IntPtr pOut);

		// Token: 0x04000831 RID: 2097
		public const string ExceptionMessageFormat = "Caught exception in \"{0}\" method: {1}";

		// Token: 0x04000832 RID: 2098
		internal const string SQLITE_DLL = "SQLite.Interop.dll";

		// Token: 0x04000833 RID: 2099
		private static readonly string DllFileExtension = ".dll";

		// Token: 0x04000834 RID: 2100
		private static readonly string ConfigFileExtension = ".config";

		// Token: 0x04000835 RID: 2101
		private static readonly string AltConfigFileExtension = ".altconfig";

		// Token: 0x04000836 RID: 2102
		private static readonly string XmlConfigFileName = typeof(UnsafeNativeMethods).Namespace + UnsafeNativeMethods.DllFileExtension + UnsafeNativeMethods.ConfigFileExtension;

		// Token: 0x04000837 RID: 2103
		private static readonly string XmlAltConfigFileName = typeof(UnsafeNativeMethods).Namespace + UnsafeNativeMethods.DllFileExtension + UnsafeNativeMethods.AltConfigFileExtension;

		// Token: 0x04000838 RID: 2104
		private static readonly string XmlConfigDirectoryToken = "%PreLoadSQLite_XmlConfigDirectory%";

		// Token: 0x04000839 RID: 2105
		private static readonly string AssemblyDirectoryToken = "%PreLoadSQLite_AssemblyDirectory%";

		// Token: 0x0400083A RID: 2106
		private static readonly string TargetFrameworkToken = "%PreLoadSQLite_TargetFramework%";

		// Token: 0x0400083B RID: 2107
		private static readonly object staticSyncRoot = new object();

		// Token: 0x0400083C RID: 2108
		private static Dictionary<string, string> targetFrameworkAbbreviations;

		// Token: 0x0400083D RID: 2109
		private static Dictionary<string, string> processorArchitecturePlatforms;

		// Token: 0x0400083E RID: 2110
		private static string cachedAssemblyDirectory;

		// Token: 0x0400083F RID: 2111
		private static bool noAssemblyDirectory;

		// Token: 0x04000840 RID: 2112
		private static string cachedXmlConfigFileName;

		// Token: 0x04000841 RID: 2113
		private static bool noXmlConfigFileName;

		// Token: 0x04000842 RID: 2114
		private static readonly string PROCESSOR_ARCHITECTURE = "PROCESSOR_ARCHITECTURE";

		// Token: 0x04000843 RID: 2115
		private static string _SQLiteNativeModuleFileName = null;

		// Token: 0x04000844 RID: 2116
		private static IntPtr _SQLiteNativeModuleHandle = IntPtr.Zero;

		// Token: 0x020002A3 RID: 675
		// (Invoke) Token: 0x06001891 RID: 6289
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate int xSessionFilter(IntPtr context, IntPtr pTblName);

		// Token: 0x020002A4 RID: 676
		// (Invoke) Token: 0x06001895 RID: 6293
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate SQLiteChangeSetConflictResult xSessionConflict(IntPtr context, SQLiteChangeSetConflictType type, IntPtr iterator);

		// Token: 0x020002A5 RID: 677
		// (Invoke) Token: 0x06001899 RID: 6297
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate SQLiteErrorCode xSessionInput(IntPtr context, IntPtr pData, ref int nData);

		// Token: 0x020002A6 RID: 678
		// (Invoke) Token: 0x0600189D RID: 6301
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate SQLiteErrorCode xSessionOutput(IntPtr context, IntPtr pData, int nData);

		// Token: 0x020002A7 RID: 679
		// (Invoke) Token: 0x060018A1 RID: 6305
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xCreate(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError);

		// Token: 0x020002A8 RID: 680
		// (Invoke) Token: 0x060018A5 RID: 6309
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xConnect(IntPtr pDb, IntPtr pAux, int argc, IntPtr argv, ref IntPtr pVtab, ref IntPtr pError);

		// Token: 0x020002A9 RID: 681
		// (Invoke) Token: 0x060018A9 RID: 6313
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex);

		// Token: 0x020002AA RID: 682
		// (Invoke) Token: 0x060018AD RID: 6317
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xDisconnect(IntPtr pVtab);

		// Token: 0x020002AB RID: 683
		// (Invoke) Token: 0x060018B1 RID: 6321
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xDestroy(IntPtr pVtab);

		// Token: 0x020002AC RID: 684
		// (Invoke) Token: 0x060018B5 RID: 6325
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor);

		// Token: 0x020002AD RID: 685
		// (Invoke) Token: 0x060018B9 RID: 6329
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xClose(IntPtr pCursor);

		// Token: 0x020002AE RID: 686
		// (Invoke) Token: 0x060018BD RID: 6333
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xFilter(IntPtr pCursor, int idxNum, IntPtr idxStr, int argc, IntPtr argv);

		// Token: 0x020002AF RID: 687
		// (Invoke) Token: 0x060018C1 RID: 6337
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xNext(IntPtr pCursor);

		// Token: 0x020002B0 RID: 688
		// (Invoke) Token: 0x060018C5 RID: 6341
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int xEof(IntPtr pCursor);

		// Token: 0x020002B1 RID: 689
		// (Invoke) Token: 0x060018C9 RID: 6345
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index);

		// Token: 0x020002B2 RID: 690
		// (Invoke) Token: 0x060018CD RID: 6349
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId);

		// Token: 0x020002B3 RID: 691
		// (Invoke) Token: 0x060018D1 RID: 6353
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId);

		// Token: 0x020002B4 RID: 692
		// (Invoke) Token: 0x060018D5 RID: 6357
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xBegin(IntPtr pVtab);

		// Token: 0x020002B5 RID: 693
		// (Invoke) Token: 0x060018D9 RID: 6361
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xSync(IntPtr pVtab);

		// Token: 0x020002B6 RID: 694
		// (Invoke) Token: 0x060018DD RID: 6365
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xCommit(IntPtr pVtab);

		// Token: 0x020002B7 RID: 695
		// (Invoke) Token: 0x060018E1 RID: 6369
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xRollback(IntPtr pVtab);

		// Token: 0x020002B8 RID: 696
		// (Invoke) Token: 0x060018E5 RID: 6373
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int xFindFunction(IntPtr pVtab, int nArg, IntPtr zName, ref SQLiteCallback callback, ref IntPtr pUserData);

		// Token: 0x020002B9 RID: 697
		// (Invoke) Token: 0x060018E9 RID: 6377
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew);

		// Token: 0x020002BA RID: 698
		// (Invoke) Token: 0x060018ED RID: 6381
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint);

		// Token: 0x020002BB RID: 699
		// (Invoke) Token: 0x060018F1 RID: 6385
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint);

		// Token: 0x020002BC RID: 700
		// (Invoke) Token: 0x060018F5 RID: 6389
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint);

		// Token: 0x020002BD RID: 701
		// (Invoke) Token: 0x060018F9 RID: 6393
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void xDestroyModule(IntPtr pClientData);

		// Token: 0x020002BE RID: 702
		internal struct sqlite3_module
		{
			// Token: 0x04000B6E RID: 2926
			public int iVersion;

			// Token: 0x04000B6F RID: 2927
			public UnsafeNativeMethods.xCreate xCreate;

			// Token: 0x04000B70 RID: 2928
			public UnsafeNativeMethods.xConnect xConnect;

			// Token: 0x04000B71 RID: 2929
			public UnsafeNativeMethods.xBestIndex xBestIndex;

			// Token: 0x04000B72 RID: 2930
			public UnsafeNativeMethods.xDisconnect xDisconnect;

			// Token: 0x04000B73 RID: 2931
			public UnsafeNativeMethods.xDestroy xDestroy;

			// Token: 0x04000B74 RID: 2932
			public UnsafeNativeMethods.xOpen xOpen;

			// Token: 0x04000B75 RID: 2933
			public UnsafeNativeMethods.xClose xClose;

			// Token: 0x04000B76 RID: 2934
			public UnsafeNativeMethods.xFilter xFilter;

			// Token: 0x04000B77 RID: 2935
			public UnsafeNativeMethods.xNext xNext;

			// Token: 0x04000B78 RID: 2936
			public UnsafeNativeMethods.xEof xEof;

			// Token: 0x04000B79 RID: 2937
			public UnsafeNativeMethods.xColumn xColumn;

			// Token: 0x04000B7A RID: 2938
			public UnsafeNativeMethods.xRowId xRowId;

			// Token: 0x04000B7B RID: 2939
			public UnsafeNativeMethods.xUpdate xUpdate;

			// Token: 0x04000B7C RID: 2940
			public UnsafeNativeMethods.xBegin xBegin;

			// Token: 0x04000B7D RID: 2941
			public UnsafeNativeMethods.xSync xSync;

			// Token: 0x04000B7E RID: 2942
			public UnsafeNativeMethods.xCommit xCommit;

			// Token: 0x04000B7F RID: 2943
			public UnsafeNativeMethods.xRollback xRollback;

			// Token: 0x04000B80 RID: 2944
			public UnsafeNativeMethods.xFindFunction xFindFunction;

			// Token: 0x04000B81 RID: 2945
			public UnsafeNativeMethods.xRename xRename;

			// Token: 0x04000B82 RID: 2946
			public UnsafeNativeMethods.xSavepoint xSavepoint;

			// Token: 0x04000B83 RID: 2947
			public UnsafeNativeMethods.xRelease xRelease;

			// Token: 0x04000B84 RID: 2948
			public UnsafeNativeMethods.xRollbackTo xRollbackTo;
		}

		// Token: 0x020002BF RID: 703
		internal struct sqlite3_vtab
		{
			// Token: 0x04000B85 RID: 2949
			public IntPtr pModule;

			// Token: 0x04000B86 RID: 2950
			public int nRef;

			// Token: 0x04000B87 RID: 2951
			public IntPtr zErrMsg;
		}

		// Token: 0x020002C0 RID: 704
		internal struct sqlite3_vtab_cursor
		{
			// Token: 0x04000B88 RID: 2952
			public IntPtr pVTab;
		}

		// Token: 0x020002C1 RID: 705
		internal struct sqlite3_index_constraint
		{
			// Token: 0x060018FC RID: 6396 RVA: 0x0006A048 File Offset: 0x00068248
			public sqlite3_index_constraint(SQLiteIndexConstraint constraint)
			{
				this = default(UnsafeNativeMethods.sqlite3_index_constraint);
				if (constraint != null)
				{
					this.iColumn = constraint.iColumn;
					this.op = constraint.op;
					this.usable = constraint.usable;
					this.iTermOffset = constraint.iTermOffset;
				}
			}

			// Token: 0x04000B89 RID: 2953
			public int iColumn;

			// Token: 0x04000B8A RID: 2954
			public SQLiteIndexConstraintOp op;

			// Token: 0x04000B8B RID: 2955
			public byte usable;

			// Token: 0x04000B8C RID: 2956
			public int iTermOffset;
		}

		// Token: 0x020002C2 RID: 706
		internal struct sqlite3_index_orderby
		{
			// Token: 0x060018FD RID: 6397 RVA: 0x0006A088 File Offset: 0x00068288
			public sqlite3_index_orderby(SQLiteIndexOrderBy orderBy)
			{
				this = default(UnsafeNativeMethods.sqlite3_index_orderby);
				if (orderBy != null)
				{
					this.iColumn = orderBy.iColumn;
					this.desc = orderBy.desc;
				}
			}

			// Token: 0x04000B8D RID: 2957
			public int iColumn;

			// Token: 0x04000B8E RID: 2958
			public byte desc;
		}

		// Token: 0x020002C3 RID: 707
		internal struct sqlite3_index_constraint_usage
		{
			// Token: 0x060018FE RID: 6398 RVA: 0x0006A0B0 File Offset: 0x000682B0
			public sqlite3_index_constraint_usage(SQLiteIndexConstraintUsage constraintUsage)
			{
				this = default(UnsafeNativeMethods.sqlite3_index_constraint_usage);
				if (constraintUsage != null)
				{
					this.argvIndex = constraintUsage.argvIndex;
					this.omit = constraintUsage.omit;
				}
			}

			// Token: 0x04000B8F RID: 2959
			public int argvIndex;

			// Token: 0x04000B90 RID: 2960
			public byte omit;
		}

		// Token: 0x020002C4 RID: 708
		internal struct sqlite3_index_info
		{
			// Token: 0x04000B91 RID: 2961
			public int nConstraint;

			// Token: 0x04000B92 RID: 2962
			public IntPtr aConstraint;

			// Token: 0x04000B93 RID: 2963
			public int nOrderBy;

			// Token: 0x04000B94 RID: 2964
			public IntPtr aOrderBy;

			// Token: 0x04000B95 RID: 2965
			public IntPtr aConstraintUsage;

			// Token: 0x04000B96 RID: 2966
			public int idxNum;

			// Token: 0x04000B97 RID: 2967
			public string idxStr;

			// Token: 0x04000B98 RID: 2968
			public int needToFreeIdxStr;

			// Token: 0x04000B99 RID: 2969
			public int orderByConsumed;

			// Token: 0x04000B9A RID: 2970
			public double estimatedCost;

			// Token: 0x04000B9B RID: 2971
			public long estimatedRows;

			// Token: 0x04000B9C RID: 2972
			public SQLiteIndexFlags idxFlags;

			// Token: 0x04000B9D RID: 2973
			public long colUsed;
		}
	}
}
