using System;

namespace System.Data.SQLite
{
	// Token: 0x0200017B RID: 379
	public class UpdateEventArgs : EventArgs
	{
		// Token: 0x060010F8 RID: 4344 RVA: 0x00050DFC File Offset: 0x0004EFFC
		internal UpdateEventArgs(string database, string table, UpdateEventType eventType, long rowid)
		{
			this.Database = database;
			this.Table = table;
			this.Event = eventType;
			this.RowId = rowid;
		}

		// Token: 0x040006AA RID: 1706
		public readonly string Database;

		// Token: 0x040006AB RID: 1707
		public readonly string Table;

		// Token: 0x040006AC RID: 1708
		public readonly UpdateEventType Event;

		// Token: 0x040006AD RID: 1709
		public readonly long RowId;
	}
}
