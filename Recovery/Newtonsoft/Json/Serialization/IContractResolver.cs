using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B3 RID: 179
	[NullableContext(1)]
	public interface IContractResolver
	{
		// Token: 0x06000678 RID: 1656
		JsonContract ResolveContract(Type type);
	}
}
