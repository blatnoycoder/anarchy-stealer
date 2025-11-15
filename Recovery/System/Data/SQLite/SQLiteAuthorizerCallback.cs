using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x0200016A RID: 362
	// (Invoke) Token: 0x060010BF RID: 4287
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate SQLiteAuthorizerReturnCode SQLiteAuthorizerCallback(IntPtr pUserData, SQLiteAuthorizerActionCode actionCode, IntPtr pArgument1, IntPtr pArgument2, IntPtr pDatabase, IntPtr pAuthContext);
}
