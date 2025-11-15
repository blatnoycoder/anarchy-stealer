using System;

namespace System.Data.SQLite
{
	// Token: 0x020001CA RID: 458
	public sealed class SQLiteIndexInputs
	{
		// Token: 0x0600148E RID: 5262 RVA: 0x0005EFDC File Offset: 0x0005D1DC
		internal SQLiteIndexInputs(int nConstraint, int nOrderBy)
		{
			this.constraints = new SQLiteIndexConstraint[nConstraint];
			this.orderBys = new SQLiteIndexOrderBy[nOrderBy];
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0005EFFC File Offset: 0x0005D1FC
		public SQLiteIndexConstraint[] Constraints
		{
			get
			{
				return this.constraints;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0005F004 File Offset: 0x0005D204
		public SQLiteIndexOrderBy[] OrderBys
		{
			get
			{
				return this.orderBys;
			}
		}

		// Token: 0x04000894 RID: 2196
		private SQLiteIndexConstraint[] constraints;

		// Token: 0x04000895 RID: 2197
		private SQLiteIndexOrderBy[] orderBys;
	}
}
