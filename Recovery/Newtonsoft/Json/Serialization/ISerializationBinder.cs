using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B5 RID: 181
	[NullableContext(1)]
	public interface ISerializationBinder
	{
		// Token: 0x0600067D RID: 1661
		Type BindToType([Nullable(2)] string assemblyName, string typeName);

		// Token: 0x0600067E RID: 1662
		[NullableContext(2)]
		void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName);
	}
}
