using System;

namespace System.Data.SQLite
{
	// Token: 0x0200015F RID: 351
	public class SQLiteReadValueEventArgs : SQLiteReadEventArgs
	{
		// Token: 0x06000FC0 RID: 4032 RVA: 0x00048E70 File Offset: 0x00047070
		internal SQLiteReadValueEventArgs(string methodName, SQLiteReadEventArgs extraEventArgs, SQLiteDataReaderValue value)
		{
			this.methodName = methodName;
			this.extraEventArgs = extraEventArgs;
			this.value = value;
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x00048E90 File Offset: 0x00047090
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00048E98 File Offset: 0x00047098
		public SQLiteReadEventArgs ExtraEventArgs
		{
			get
			{
				return this.extraEventArgs;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x00048EA0 File Offset: 0x000470A0
		public SQLiteDataReaderValue Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000621 RID: 1569
		private string methodName;

		// Token: 0x04000622 RID: 1570
		private SQLiteReadEventArgs extraEventArgs;

		// Token: 0x04000623 RID: 1571
		private SQLiteDataReaderValue value;
	}
}
