using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001A4 RID: 420
	// (Invoke) Token: 0x06001254 RID: 4692
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SQLiteFinalCallback(IntPtr context);
}
