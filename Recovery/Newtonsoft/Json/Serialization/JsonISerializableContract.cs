using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C3 RID: 195
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000233D0 File Offset: 0x000215D0
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x000233D8 File Offset: 0x000215D8
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> ISerializableCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000233E4 File Offset: 0x000215E4
		[NullableContext(1)]
		public JsonISerializableContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
