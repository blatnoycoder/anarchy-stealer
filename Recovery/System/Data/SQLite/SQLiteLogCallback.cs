using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x0200014A RID: 330
	// (Invoke) Token: 0x06000DB9 RID: 3513
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SQLiteLogCallback(IntPtr pUserData, int errorCode, IntPtr pMessage);
}
