using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x02000054 RID: 84
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonDictionaryAttribute : JsonContainerAttribute
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000AACC File Offset: 0x00008CCC
		public JsonDictionaryAttribute()
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		[NullableContext(1)]
		public JsonDictionaryAttribute(string id)
			: base(id)
		{
		}
	}
}
