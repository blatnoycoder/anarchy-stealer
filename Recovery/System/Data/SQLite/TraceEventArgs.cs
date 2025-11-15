using System;

namespace System.Data.SQLite
{
	// Token: 0x0200017D RID: 381
	public class TraceEventArgs : EventArgs
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x00050E2C File Offset: 0x0004F02C
		internal TraceEventArgs(string statement)
		{
			this.Statement = statement;
		}

		// Token: 0x040006AF RID: 1711
		public readonly string Statement;
	}
}
