using System;

namespace System.Data.SQLite
{
	// Token: 0x0200015D RID: 349
	public class SQLiteReadBlobEventArgs : SQLiteReadEventArgs
	{
		// Token: 0x06000FB3 RID: 4019 RVA: 0x00048DB0 File Offset: 0x00046FB0
		internal SQLiteReadBlobEventArgs(bool readOnly)
		{
			this.readOnly = readOnly;
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x00048DC0 File Offset: 0x00046FC0
		// (set) Token: 0x06000FB5 RID: 4021 RVA: 0x00048DC8 File Offset: 0x00046FC8
		public bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
			}
		}

		// Token: 0x0400061B RID: 1563
		private bool readOnly;
	}
}
