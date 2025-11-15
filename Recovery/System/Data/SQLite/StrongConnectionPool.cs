using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000181 RID: 385
	internal sealed class StrongConnectionPool : ISQLiteConnectionPool2, ISQLiteConnectionPool
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x00050E9C File Offset: 0x0004F09C
		public void GetCounts(string fileName, ref Dictionary<string, int> counts, ref int openCount, ref int closeCount, ref int totalCount)
		{
			StaticStrongConnectionPool<object>.GetCounts(fileName, ref counts, ref openCount, ref closeCount, ref totalCount);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00050EAC File Offset: 0x0004F0AC
		public void ClearPool(string fileName)
		{
			StaticStrongConnectionPool<object>.ClearPool(fileName);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00050EB4 File Offset: 0x0004F0B4
		public void ClearAllPools()
		{
			StaticStrongConnectionPool<object>.ClearAllPools();
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00050EBC File Offset: 0x0004F0BC
		public void Add(string fileName, object handle, int version)
		{
			StaticStrongConnectionPool<object>.Add(fileName, handle as SQLiteConnectionHandle, version);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00050ECC File Offset: 0x0004F0CC
		public object Remove(string fileName, int maxPoolSize, out int version)
		{
			return StaticStrongConnectionPool<object>.Remove(fileName, maxPoolSize, out version);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00050ED8 File Offset: 0x0004F0D8
		public void Initialize(object argument)
		{
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00050EDC File Offset: 0x0004F0DC
		public void Terminate(object argument)
		{
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00050EE0 File Offset: 0x0004F0E0
		public void GetCounts(ref int openCount, ref int closeCount)
		{
			StaticStrongConnectionPool<object>.GetCounts(ref openCount, ref closeCount);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00050EEC File Offset: 0x0004F0EC
		public void ResetCounts()
		{
			StaticStrongConnectionPool<object>.ResetCounts();
		}
	}
}
