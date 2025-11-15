using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C4 RID: 196
	public class JsonLinqContract : JsonContract
	{
		// Token: 0x060006F7 RID: 1783 RVA: 0x000233F4 File Offset: 0x000215F4
		[NullableContext(1)]
		public JsonLinqContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Linq;
		}
	}
}
