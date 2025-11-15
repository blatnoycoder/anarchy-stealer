using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000CD RID: 205
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060007FD RID: 2045 RVA: 0x0002A330 File Offset: 0x00028530
		[NullableContext(1)]
		public JsonStringContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
