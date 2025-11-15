using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Data.SQLite
{
	// Token: 0x020001B8 RID: 440
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethodsWin32
	{
		// Token: 0x06001340 RID: 4928
		[DllImport("kernel32", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr LoadLibrary(string fileName);

		// Token: 0x06001341 RID: 4929
		[DllImport("kernel32")]
		internal static extern void GetSystemInfo(out UnsafeNativeMethodsWin32.SYSTEM_INFO systemInfo);

		// Token: 0x020002A1 RID: 673
		internal enum ProcessorArchitecture : ushort
		{
			// Token: 0x04000B55 RID: 2901
			Intel,
			// Token: 0x04000B56 RID: 2902
			MIPS,
			// Token: 0x04000B57 RID: 2903
			Alpha,
			// Token: 0x04000B58 RID: 2904
			PowerPC,
			// Token: 0x04000B59 RID: 2905
			SHx,
			// Token: 0x04000B5A RID: 2906
			ARM,
			// Token: 0x04000B5B RID: 2907
			IA64,
			// Token: 0x04000B5C RID: 2908
			Alpha64,
			// Token: 0x04000B5D RID: 2909
			MSIL,
			// Token: 0x04000B5E RID: 2910
			AMD64,
			// Token: 0x04000B5F RID: 2911
			IA32_on_Win64,
			// Token: 0x04000B60 RID: 2912
			Neutral,
			// Token: 0x04000B61 RID: 2913
			ARM64,
			// Token: 0x04000B62 RID: 2914
			Unknown = 65535
		}

		// Token: 0x020002A2 RID: 674
		internal struct SYSTEM_INFO
		{
			// Token: 0x04000B63 RID: 2915
			public UnsafeNativeMethodsWin32.ProcessorArchitecture wProcessorArchitecture;

			// Token: 0x04000B64 RID: 2916
			public ushort wReserved;

			// Token: 0x04000B65 RID: 2917
			public uint dwPageSize;

			// Token: 0x04000B66 RID: 2918
			public IntPtr lpMinimumApplicationAddress;

			// Token: 0x04000B67 RID: 2919
			public IntPtr lpMaximumApplicationAddress;

			// Token: 0x04000B68 RID: 2920
			public IntPtr dwActiveProcessorMask;

			// Token: 0x04000B69 RID: 2921
			public uint dwNumberOfProcessors;

			// Token: 0x04000B6A RID: 2922
			public uint dwProcessorType;

			// Token: 0x04000B6B RID: 2923
			public uint dwAllocationGranularity;

			// Token: 0x04000B6C RID: 2924
			public ushort wProcessorLevel;

			// Token: 0x04000B6D RID: 2925
			public ushort wProcessorRevision;
		}
	}
}
