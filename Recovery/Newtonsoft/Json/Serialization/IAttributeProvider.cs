using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	public interface IAttributeProvider
	{
		// Token: 0x06000676 RID: 1654
		IList<Attribute> GetAttributes(bool inherit);

		// Token: 0x06000677 RID: 1655
		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
}
