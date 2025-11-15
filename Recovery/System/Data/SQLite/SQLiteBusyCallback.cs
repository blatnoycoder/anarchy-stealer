using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x02000168 RID: 360
	// (Invoke) Token: 0x060010B7 RID: 4279
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate SQLiteBusyReturnCode SQLiteBusyCallback(IntPtr pUserData, int count);
}
