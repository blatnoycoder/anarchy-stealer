using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000D7 RID: 215
	public class SnakeCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000839 RID: 2105 RVA: 0x0002AE68 File Offset: 0x00029068
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002AE80 File Offset: 0x00029080
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002AE94 File Offset: 0x00029094
		public SnakeCaseNamingStrategy()
		{
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002AE9C File Offset: 0x0002909C
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToSnakeCase(name);
		}
	}
}
