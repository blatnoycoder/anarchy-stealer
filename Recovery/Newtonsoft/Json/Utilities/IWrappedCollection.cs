using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000079 RID: 121
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600044E RID: 1102
		[Nullable(1)]
		object UnderlyingCollection
		{
			[NullableContext(1)]
			get;
		}
	}
}
