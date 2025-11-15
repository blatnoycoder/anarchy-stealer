using System;

namespace System.Data.SQLite
{
	// Token: 0x02000162 RID: 354
	public sealed class SQLiteTypeCallbacks
	{
		// Token: 0x06000FCC RID: 4044 RVA: 0x00048EA8 File Offset: 0x000470A8
		private SQLiteTypeCallbacks(SQLiteBindValueCallback bindValueCallback, SQLiteReadValueCallback readValueCallback, object bindValueUserData, object readValueUserData)
		{
			this.bindValueCallback = bindValueCallback;
			this.readValueCallback = readValueCallback;
			this.bindValueUserData = bindValueUserData;
			this.readValueUserData = readValueUserData;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00048ED0 File Offset: 0x000470D0
		public static SQLiteTypeCallbacks Create(SQLiteBindValueCallback bindValueCallback, SQLiteReadValueCallback readValueCallback, object bindValueUserData, object readValueUserData)
		{
			return new SQLiteTypeCallbacks(bindValueCallback, readValueCallback, bindValueUserData, readValueUserData);
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00048EDC File Offset: 0x000470DC
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x00048EE4 File Offset: 0x000470E4
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
			internal set
			{
				this.typeName = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00048EF0 File Offset: 0x000470F0
		public SQLiteBindValueCallback BindValueCallback
		{
			get
			{
				return this.bindValueCallback;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x00048EF8 File Offset: 0x000470F8
		public SQLiteReadValueCallback ReadValueCallback
		{
			get
			{
				return this.readValueCallback;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x00048F00 File Offset: 0x00047100
		public object BindValueUserData
		{
			get
			{
				return this.bindValueUserData;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00048F08 File Offset: 0x00047108
		public object ReadValueUserData
		{
			get
			{
				return this.readValueUserData;
			}
		}

		// Token: 0x04000624 RID: 1572
		private string typeName;

		// Token: 0x04000625 RID: 1573
		private SQLiteBindValueCallback bindValueCallback;

		// Token: 0x04000626 RID: 1574
		private SQLiteReadValueCallback readValueCallback;

		// Token: 0x04000627 RID: 1575
		private object bindValueUserData;

		// Token: 0x04000628 RID: 1576
		private object readValueUserData;
	}
}
