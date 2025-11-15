using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x02000164 RID: 356
	public class ConnectionEventArgs : EventArgs
	{
		// Token: 0x06000FD5 RID: 4053 RVA: 0x00048F20 File Offset: 0x00047120
		internal ConnectionEventArgs(SQLiteConnectionEventType eventType, StateChangeEventArgs eventArgs, IDbTransaction transaction, IDbCommand command, IDataReader dataReader, CriticalHandle criticalHandle, string text, object data)
		{
			this.EventType = eventType;
			this.EventArgs = eventArgs;
			this.Transaction = transaction;
			this.Command = command;
			this.DataReader = dataReader;
			this.CriticalHandle = criticalHandle;
			this.Text = text;
			this.Data = data;
		}

		// Token: 0x04000629 RID: 1577
		public readonly SQLiteConnectionEventType EventType;

		// Token: 0x0400062A RID: 1578
		public readonly StateChangeEventArgs EventArgs;

		// Token: 0x0400062B RID: 1579
		public readonly IDbTransaction Transaction;

		// Token: 0x0400062C RID: 1580
		public readonly IDbCommand Command;

		// Token: 0x0400062D RID: 1581
		public readonly IDataReader DataReader;

		// Token: 0x0400062E RID: 1582
		public readonly CriticalHandle CriticalHandle;

		// Token: 0x0400062F RID: 1583
		public readonly string Text;

		// Token: 0x04000630 RID: 1584
		public readonly object Data;
	}
}
