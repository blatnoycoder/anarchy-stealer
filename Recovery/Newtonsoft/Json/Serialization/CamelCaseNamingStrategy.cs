using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A7 RID: 167
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000607 RID: 1543 RVA: 0x0001F54C File Offset: 0x0001D74C
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001F564 File Offset: 0x0001D764
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001F578 File Offset: 0x0001D778
		public CamelCaseNamingStrategy()
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001F580 File Offset: 0x0001D780
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}
