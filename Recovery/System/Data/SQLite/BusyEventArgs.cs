using System;

namespace System.Data.SQLite
{
	// Token: 0x02000177 RID: 375
	public class BusyEventArgs : EventArgs
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x00050D04 File Offset: 0x0004EF04
		private BusyEventArgs()
		{
			this.UserData = IntPtr.Zero;
			this.Count = 0;
			this.ReturnCode = SQLiteBusyReturnCode.Retry;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00050D28 File Offset: 0x0004EF28
		internal BusyEventArgs(IntPtr pUserData, int count, SQLiteBusyReturnCode returnCode)
			: this()
		{
			this.UserData = pUserData;
			this.Count = count;
			this.ReturnCode = returnCode;
		}

		// Token: 0x0400069A RID: 1690
		public readonly IntPtr UserData;

		// Token: 0x0400069B RID: 1691
		public readonly int Count;

		// Token: 0x0400069C RID: 1692
		public SQLiteBusyReturnCode ReturnCode;
	}
}
