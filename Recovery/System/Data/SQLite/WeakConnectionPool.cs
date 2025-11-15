using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000180 RID: 384
	internal sealed class WeakConnectionPool : ISQLiteConnectionPool2, ISQLiteConnectionPool
	{
		// Token: 0x06001104 RID: 4356 RVA: 0x00050E3C File Offset: 0x0004F03C
		public void GetCounts(string fileName, ref Dictionary<string, int> counts, ref int openCount, ref int closeCount, ref int totalCount)
		{
			StaticWeakConnectionPool<WeakReference>.GetCounts(fileName, ref counts, ref openCount, ref closeCount, ref totalCount);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00050E4C File Offset: 0x0004F04C
		public void ClearPool(string fileName)
		{
			StaticWeakConnectionPool<WeakReference>.ClearPool(fileName);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00050E54 File Offset: 0x0004F054
		public void ClearAllPools()
		{
			StaticWeakConnectionPool<WeakReference>.ClearAllPools();
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00050E5C File Offset: 0x0004F05C
		public void Add(string fileName, object handle, int version)
		{
			StaticWeakConnectionPool<WeakReference>.Add(fileName, handle as SQLiteConnectionHandle, version);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00050E6C File Offset: 0x0004F06C
		public object Remove(string fileName, int maxPoolSize, out int version)
		{
			return StaticWeakConnectionPool<WeakReference>.Remove(fileName, maxPoolSize, out version);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00050E78 File Offset: 0x0004F078
		public void Initialize(object argument)
		{
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00050E7C File Offset: 0x0004F07C
		public void Terminate(object argument)
		{
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00050E80 File Offset: 0x0004F080
		public void GetCounts(ref int openCount, ref int closeCount)
		{
			StaticWeakConnectionPool<WeakReference>.GetCounts(ref openCount, ref closeCount);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00050E8C File Offset: 0x0004F08C
		public void ResetCounts()
		{
			StaticWeakConnectionPool<WeakReference>.ResetCounts();
		}
	}
}
