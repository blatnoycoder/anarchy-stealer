using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000182 RID: 386
	internal sealed class PoolQueue<T>
	{
		// Token: 0x06001118 RID: 4376 RVA: 0x00050EFC File Offset: 0x0004F0FC
		internal PoolQueue(int version, int maxSize)
		{
			this.PoolVersion = version;
			this.MaxPoolSize = maxSize;
		}

		// Token: 0x040006B0 RID: 1712
		internal readonly Queue<T> Queue = new Queue<T>();

		// Token: 0x040006B1 RID: 1713
		internal int PoolVersion;

		// Token: 0x040006B2 RID: 1714
		internal int MaxPoolSize;
	}
}
