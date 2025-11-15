using System;

namespace System.Data.SQLite
{
	// Token: 0x02000179 RID: 377
	public class AuthorizerEventArgs : EventArgs
	{
		// Token: 0x060010F6 RID: 4342 RVA: 0x00050D7C File Offset: 0x0004EF7C
		private AuthorizerEventArgs()
		{
			this.UserData = IntPtr.Zero;
			this.ActionCode = SQLiteAuthorizerActionCode.None;
			this.Argument1 = null;
			this.Argument2 = null;
			this.Database = null;
			this.Context = null;
			this.ReturnCode = SQLiteAuthorizerReturnCode.Ok;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00050DBC File Offset: 0x0004EFBC
		internal AuthorizerEventArgs(IntPtr pUserData, SQLiteAuthorizerActionCode actionCode, string argument1, string argument2, string database, string context, SQLiteAuthorizerReturnCode returnCode)
			: this()
		{
			this.UserData = pUserData;
			this.ActionCode = actionCode;
			this.Argument1 = argument1;
			this.Argument2 = argument2;
			this.Database = database;
			this.Context = context;
			this.ReturnCode = returnCode;
		}

		// Token: 0x0400069F RID: 1695
		public readonly IntPtr UserData;

		// Token: 0x040006A0 RID: 1696
		public readonly SQLiteAuthorizerActionCode ActionCode;

		// Token: 0x040006A1 RID: 1697
		public readonly string Argument1;

		// Token: 0x040006A2 RID: 1698
		public readonly string Argument2;

		// Token: 0x040006A3 RID: 1699
		public readonly string Database;

		// Token: 0x040006A4 RID: 1700
		public readonly string Context;

		// Token: 0x040006A5 RID: 1701
		public SQLiteAuthorizerReturnCode ReturnCode;
	}
}
