using System;

namespace System.Data.SQLite
{
	// Token: 0x02000178 RID: 376
	public class ProgressEventArgs : EventArgs
	{
		// Token: 0x060010F4 RID: 4340 RVA: 0x00050D48 File Offset: 0x0004EF48
		private ProgressEventArgs()
		{
			this.UserData = IntPtr.Zero;
			this.ReturnCode = SQLiteProgressReturnCode.Continue;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00050D64 File Offset: 0x0004EF64
		internal ProgressEventArgs(IntPtr pUserData, SQLiteProgressReturnCode returnCode)
			: this()
		{
			this.UserData = pUserData;
			this.ReturnCode = returnCode;
		}

		// Token: 0x0400069D RID: 1693
		public readonly IntPtr UserData;

		// Token: 0x0400069E RID: 1694
		public SQLiteProgressReturnCode ReturnCode;
	}
}
