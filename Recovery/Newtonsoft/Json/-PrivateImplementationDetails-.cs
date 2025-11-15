using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000147 RID: 327
[CompilerGenerated]
internal sealed class Newtonsoft.Json.<PrivateImplementationDetails>
{
	// Token: 0x06000DB3 RID: 3507 RVA: 0x0003FA74 File Offset: 0x0003DC74
	internal static uint ComputeStringHash(string s)
	{
		uint num;
		if (s != null)
		{
			num = 2166136261U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((uint)s[i] ^ num) * 16777619U;
			}
		}
		return num;
	}

	// Token: 0x0400050D RID: 1293 RVA: 0x0006AE9A File Offset: 0x0006909A
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.__StaticArrayInitTypeSize=6 3DE43C11C7130AF9014115BCDC2584DFE6B50579;

	// Token: 0x0400050E RID: 1294 RVA: 0x0006AEA0 File Offset: 0x000690A0
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.__StaticArrayInitTypeSize=28 9E31F24F64765FCAA589F589324D17C9FCF6A06D;

	// Token: 0x0400050F RID: 1295 RVA: 0x0006AEBC File Offset: 0x000690BC
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.__StaticArrayInitTypeSize=10 D40004AB0E92BF6C8DFE481B56BE3D04ABDA76EB;

	// Token: 0x04000510 RID: 1296 RVA: 0x0006AEC6 File Offset: 0x000690C6
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.__StaticArrayInitTypeSize=52 DD3AEFEADB1CD615F3017763F1568179FEE640B0;

	// Token: 0x04000511 RID: 1297 RVA: 0x0006AEFA File Offset: 0x000690FA
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 E289D9D3D233BC253E8C0FA8C2AFDD86A407CE30;

	// Token: 0x04000512 RID: 1298 RVA: 0x0006AF1E File Offset: 0x0006911E
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.__StaticArrayInitTypeSize=52 E92B39D8233061927D9ACDE54665E68E7535635A;

	// Token: 0x02000293 RID: 659
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 6)]
	private struct __StaticArrayInitTypeSize=6
	{
	}

	// Token: 0x02000294 RID: 660
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 10)]
	private struct __StaticArrayInitTypeSize=10
	{
	}

	// Token: 0x02000295 RID: 661
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
	private struct __StaticArrayInitTypeSize=28
	{
	}

	// Token: 0x02000296 RID: 662
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
	private struct __StaticArrayInitTypeSize=36
	{
	}

	// Token: 0x02000297 RID: 663
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 52)]
	private struct __StaticArrayInitTypeSize=52
	{
	}
}
