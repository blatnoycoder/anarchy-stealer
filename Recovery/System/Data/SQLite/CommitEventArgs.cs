using System;

namespace System.Data.SQLite
{
	// Token: 0x0200017C RID: 380
	public class CommitEventArgs : EventArgs
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x00050E24 File Offset: 0x0004F024
		internal CommitEventArgs()
		{
		}

		// Token: 0x040006AE RID: 1710
		public bool AbortTransaction;
	}
}
