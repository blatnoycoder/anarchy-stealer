using System;

namespace System.Data.SQLite
{
	// Token: 0x020001B6 RID: 438
	internal static class NativeLibraryHelper
	{
		// Token: 0x06001335 RID: 4917 RVA: 0x0005BD20 File Offset: 0x00059F20
		private static IntPtr LoadLibraryWin32(string fileName)
		{
			return UnsafeNativeMethodsWin32.LoadLibrary(fileName);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0005BD28 File Offset: 0x00059F28
		private static string GetMachineWin32()
		{
			try
			{
				UnsafeNativeMethodsWin32.SYSTEM_INFO system_INFO;
				UnsafeNativeMethodsWin32.GetSystemInfo(out system_INFO);
				return system_INFO.wProcessorArchitecture.ToString();
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0005BD6C File Offset: 0x00059F6C
		private static IntPtr LoadLibraryPosix(string fileName)
		{
			return UnsafeNativeMethodsPosix.dlopen(fileName, 258);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0005BD7C File Offset: 0x00059F7C
		private static string GetMachinePosix()
		{
			try
			{
				UnsafeNativeMethodsPosix.utsname utsname = null;
				if (UnsafeNativeMethodsPosix.GetOsVersionInfo(ref utsname) && utsname != null)
				{
					return utsname.machine;
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0005BDC8 File Offset: 0x00059FC8
		public static IntPtr LoadLibrary(string fileName)
		{
			NativeLibraryHelper.LoadLibraryCallback loadLibraryCallback = new NativeLibraryHelper.LoadLibraryCallback(NativeLibraryHelper.LoadLibraryWin32);
			if (!HelperMethods.IsWindows())
			{
				loadLibraryCallback = new NativeLibraryHelper.LoadLibraryCallback(NativeLibraryHelper.LoadLibraryPosix);
			}
			return loadLibraryCallback(fileName);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0005BE04 File Offset: 0x0005A004
		public static string GetMachine()
		{
			NativeLibraryHelper.GetMachineCallback getMachineCallback = new NativeLibraryHelper.GetMachineCallback(NativeLibraryHelper.GetMachineWin32);
			if (!HelperMethods.IsWindows())
			{
				getMachineCallback = new NativeLibraryHelper.GetMachineCallback(NativeLibraryHelper.GetMachinePosix);
			}
			return getMachineCallback();
		}

		// Token: 0x0200029D RID: 669
		// (Invoke) Token: 0x06001888 RID: 6280
		private delegate IntPtr LoadLibraryCallback(string fileName);

		// Token: 0x0200029E RID: 670
		// (Invoke) Token: 0x0600188C RID: 6284
		private delegate string GetMachineCallback();
	}
}
