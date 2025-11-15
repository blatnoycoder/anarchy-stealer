using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001A5 RID: 421
	// (Invoke) Token: 0x06001258 RID: 4696
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int SQLiteCollation(IntPtr puser, int len1, IntPtr pv1, int len2, IntPtr pv2);
}
