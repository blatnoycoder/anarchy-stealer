using System;

namespace System.Data.SQLite
{
	// Token: 0x020001CB RID: 459
	public sealed class SQLiteIndexOutputs
	{
		// Token: 0x06001491 RID: 5265 RVA: 0x0005F00C File Offset: 0x0005D20C
		internal SQLiteIndexOutputs(int nConstraint)
		{
			this.constraintUsages = new SQLiteIndexConstraintUsage[nConstraint];
			for (int i = 0; i < nConstraint; i++)
			{
				this.constraintUsages[i] = new SQLiteIndexConstraintUsage();
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0005F050 File Offset: 0x0005D250
		public bool CanUseEstimatedRows()
		{
			return UnsafeNativeMethods.sqlite3_libversion_number() >= 3008002;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0005F064 File Offset: 0x0005D264
		public bool CanUseIndexFlags()
		{
			return UnsafeNativeMethods.sqlite3_libversion_number() >= 3009000;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0005F078 File Offset: 0x0005D278
		public bool CanUseColumnsUsed()
		{
			return UnsafeNativeMethods.sqlite3_libversion_number() >= 3010000;
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0005F08C File Offset: 0x0005D28C
		public SQLiteIndexConstraintUsage[] ConstraintUsages
		{
			get
			{
				return this.constraintUsages;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x0005F094 File Offset: 0x0005D294
		// (set) Token: 0x06001497 RID: 5271 RVA: 0x0005F09C File Offset: 0x0005D29C
		public int IndexNumber
		{
			get
			{
				return this.indexNumber;
			}
			set
			{
				this.indexNumber = value;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0005F0A8 File Offset: 0x0005D2A8
		// (set) Token: 0x06001499 RID: 5273 RVA: 0x0005F0B0 File Offset: 0x0005D2B0
		public string IndexString
		{
			get
			{
				return this.indexString;
			}
			set
			{
				this.indexString = value;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0005F0BC File Offset: 0x0005D2BC
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x0005F0C4 File Offset: 0x0005D2C4
		public int NeedToFreeIndexString
		{
			get
			{
				return this.needToFreeIndexString;
			}
			set
			{
				this.needToFreeIndexString = value;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0005F0D0 File Offset: 0x0005D2D0
		// (set) Token: 0x0600149D RID: 5277 RVA: 0x0005F0D8 File Offset: 0x0005D2D8
		public int OrderByConsumed
		{
			get
			{
				return this.orderByConsumed;
			}
			set
			{
				this.orderByConsumed = value;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0005F0E4 File Offset: 0x0005D2E4
		// (set) Token: 0x0600149F RID: 5279 RVA: 0x0005F0EC File Offset: 0x0005D2EC
		public double? EstimatedCost
		{
			get
			{
				return this.estimatedCost;
			}
			set
			{
				this.estimatedCost = value;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x0005F0F8 File Offset: 0x0005D2F8
		// (set) Token: 0x060014A1 RID: 5281 RVA: 0x0005F100 File Offset: 0x0005D300
		public long? EstimatedRows
		{
			get
			{
				return this.estimatedRows;
			}
			set
			{
				this.estimatedRows = value;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x0005F10C File Offset: 0x0005D30C
		// (set) Token: 0x060014A3 RID: 5283 RVA: 0x0005F114 File Offset: 0x0005D314
		public SQLiteIndexFlags? IndexFlags
		{
			get
			{
				return this.indexFlags;
			}
			set
			{
				this.indexFlags = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0005F120 File Offset: 0x0005D320
		// (set) Token: 0x060014A5 RID: 5285 RVA: 0x0005F128 File Offset: 0x0005D328
		public long? ColumnsUsed
		{
			get
			{
				return this.columnsUsed;
			}
			set
			{
				this.columnsUsed = value;
			}
		}

		// Token: 0x04000896 RID: 2198
		private SQLiteIndexConstraintUsage[] constraintUsages;

		// Token: 0x04000897 RID: 2199
		private int indexNumber;

		// Token: 0x04000898 RID: 2200
		private string indexString;

		// Token: 0x04000899 RID: 2201
		private int needToFreeIndexString;

		// Token: 0x0400089A RID: 2202
		private int orderByConsumed;

		// Token: 0x0400089B RID: 2203
		private double? estimatedCost;

		// Token: 0x0400089C RID: 2204
		private long? estimatedRows;

		// Token: 0x0400089D RID: 2205
		private SQLiteIndexFlags? indexFlags;

		// Token: 0x0400089E RID: 2206
		private long? columnsUsed;
	}
}
