using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x0200016D RID: 365
	// (Invoke) Token: 0x060010CB RID: 4299
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SQLiteTraceCallback(IntPtr puser, IntPtr statement);
}
