using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x02000058 RID: 88
	public abstract class JsonNameTable
	{
		// Token: 0x060001C1 RID: 449
		[NullableContext(1)]
		[return: Nullable(2)]
		public abstract string Get(char[] key, int start, int length);
	}
}
