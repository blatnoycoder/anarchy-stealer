using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AA RID: 170
	public class DefaultNamingStrategy : NamingStrategy
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x0002192C File Offset: 0x0001FB2C
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return name;
		}
	}
}
