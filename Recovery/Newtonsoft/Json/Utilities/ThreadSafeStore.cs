using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000A3 RID: 163
	[NullableContext(1)]
	[Nullable(0)]
	internal class ThreadSafeStore<[Nullable(2)] TKey, [Nullable(2)] TValue>
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x0001F330 File Offset: 0x0001D530
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			this._creator = creator;
			this._concurrentStore = new ConcurrentDictionary<TKey, TValue>();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001F358 File Offset: 0x0001D558
		public TValue Get(TKey key)
		{
			return this._concurrentStore.GetOrAdd(key, this._creator);
		}

		// Token: 0x040002D5 RID: 725
		private readonly ConcurrentDictionary<TKey, TValue> _concurrentStore;

		// Token: 0x040002D6 RID: 726
		private readonly Func<TKey, TValue> _creator;
	}
}
