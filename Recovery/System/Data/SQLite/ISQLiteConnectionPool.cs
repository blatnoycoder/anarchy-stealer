using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x0200017E RID: 382
	public interface ISQLiteConnectionPool
	{
		// Token: 0x060010FB RID: 4347
		void GetCounts(string fileName, ref Dictionary<string, int> counts, ref int openCount, ref int closeCount, ref int totalCount);

		// Token: 0x060010FC RID: 4348
		void ClearPool(string fileName);

		// Token: 0x060010FD RID: 4349
		void ClearAllPools();

		// Token: 0x060010FE RID: 4350
		void Add(string fileName, object handle, int version);

		// Token: 0x060010FF RID: 4351
		object Remove(string fileName, int maxPoolSize, out int version);
	}
}
