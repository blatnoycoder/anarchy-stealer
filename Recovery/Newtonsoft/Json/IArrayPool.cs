using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x0200004A RID: 74
	[NullableContext(1)]
	public interface IArrayPool<[Nullable(2)] T>
	{
		// Token: 0x06000136 RID: 310
		T[] Rent(int minimumLength);

		// Token: 0x06000137 RID: 311
		void Return([Nullable(new byte[] { 2, 1 })] T[] array);
	}
}
