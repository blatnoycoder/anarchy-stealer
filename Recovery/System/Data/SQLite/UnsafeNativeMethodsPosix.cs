using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Data.SQLite
{
	// Token: 0x020001B7 RID: 439
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethodsPosix
	{
		// Token: 0x0600133B RID: 4923
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int uname(out UnsafeNativeMethodsPosix.utsname_interop name);

		// Token: 0x0600133C RID: 4924
		[DllImport("__Internal", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr dlopen(string fileName, int mode);

		// Token: 0x0600133D RID: 4925
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		internal static extern int dlclose(IntPtr module);

		// Token: 0x0600133E RID: 4926 RVA: 0x0005BE40 File Offset: 0x0005A040
		internal static bool GetOsVersionInfo(ref UnsafeNativeMethodsPosix.utsname utsName)
		{
			try
			{
				UnsafeNativeMethodsPosix.utsname_interop utsname_interop;
				if (UnsafeNativeMethodsPosix.uname(out utsname_interop) < 0)
				{
					return false;
				}
				if (utsname_interop.buffer == null)
				{
					return false;
				}
				string text = Encoding.UTF8.GetString(utsname_interop.buffer);
				if (text == null || UnsafeNativeMethodsPosix.utsNameSeparators == null)
				{
					return false;
				}
				text = text.Trim(UnsafeNativeMethodsPosix.utsNameSeparators);
				string[] array = text.Split(UnsafeNativeMethodsPosix.utsNameSeparators, StringSplitOptions.RemoveEmptyEntries);
				if (array == null)
				{
					return false;
				}
				UnsafeNativeMethodsPosix.utsname utsname = new UnsafeNativeMethodsPosix.utsname();
				if (array.Length >= 1)
				{
					utsname.sysname = array[0];
				}
				if (array.Length >= 2)
				{
					utsname.nodename = array[1];
				}
				if (array.Length >= 3)
				{
					utsname.release = array[2];
				}
				if (array.Length >= 4)
				{
					utsname.version = array[3];
				}
				if (array.Length >= 5)
				{
					utsname.machine = array[4];
				}
				utsName = utsname;
				return true;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0005BF64 File Offset: 0x0005A164
		// Note: this type is marked as 'beforefieldinit'.
		static UnsafeNativeMethodsPosix()
		{
			char[] array = new char[1];
			UnsafeNativeMethodsPosix.utsNameSeparators = array;
		}

		// Token: 0x0400082B RID: 2091
		internal const int RTLD_LAZY = 1;

		// Token: 0x0400082C RID: 2092
		internal const int RTLD_NOW = 2;

		// Token: 0x0400082D RID: 2093
		internal const int RTLD_GLOBAL = 256;

		// Token: 0x0400082E RID: 2094
		internal const int RTLD_LOCAL = 0;

		// Token: 0x0400082F RID: 2095
		internal const int RTLD_DEFAULT = 258;

		// Token: 0x04000830 RID: 2096
		private static readonly char[] utsNameSeparators;

		// Token: 0x0200029F RID: 671
		internal sealed class utsname
		{
			// Token: 0x04000B4E RID: 2894
			public string sysname;

			// Token: 0x04000B4F RID: 2895
			public string nodename;

			// Token: 0x04000B50 RID: 2896
			public string release;

			// Token: 0x04000B51 RID: 2897
			public string version;

			// Token: 0x04000B52 RID: 2898
			public string machine;
		}

		// Token: 0x020002A0 RID: 672
		private struct utsname_interop
		{
			// Token: 0x04000B53 RID: 2899
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
			public byte[] buffer;
		}
	}
}
