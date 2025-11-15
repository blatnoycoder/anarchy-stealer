using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001A3 RID: 419
	// (Invoke) Token: 0x06001250 RID: 4688
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SQLiteCallback(IntPtr context, int argc, IntPtr argv);
}
