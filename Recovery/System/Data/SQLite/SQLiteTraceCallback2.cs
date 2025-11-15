using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x0200016E RID: 366
	// (Invoke) Token: 0x060010CF RID: 4303
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SQLiteTraceCallback2(SQLiteTraceFlags type, IntPtr puser, IntPtr pCtx1, IntPtr pCtx2);
}
