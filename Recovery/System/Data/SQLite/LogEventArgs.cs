using System;

namespace System.Data.SQLite
{
	// Token: 0x020001AB RID: 427
	public class LogEventArgs : EventArgs
	{
		// Token: 0x0600128F RID: 4751 RVA: 0x000592D0 File Offset: 0x000574D0
		internal LogEventArgs(IntPtr pUserData, object errorCode, string message, object data)
		{
			this.ErrorCode = errorCode;
			this.Message = message;
			this.Data = data;
		}

		// Token: 0x040007E3 RID: 2019
		public readonly object ErrorCode;

		// Token: 0x040007E4 RID: 2020
		public readonly string Message;

		// Token: 0x040007E5 RID: 2021
		public readonly object Data;
	}
}
