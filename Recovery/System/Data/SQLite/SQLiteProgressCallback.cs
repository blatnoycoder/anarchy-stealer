using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x02000169 RID: 361
	// (Invoke) Token: 0x060010BB RID: 4283
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate SQLiteProgressReturnCode SQLiteProgressCallback(IntPtr pUserData);
}
