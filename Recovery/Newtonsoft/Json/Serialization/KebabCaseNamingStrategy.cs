using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000CF RID: 207
	public class KebabCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000813 RID: 2067 RVA: 0x0002A990 File Offset: 0x00028B90
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002A9A8 File Offset: 0x00028BA8
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002A9BC File Offset: 0x00028BBC
		public KebabCaseNamingStrategy()
		{
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002A9C4 File Offset: 0x00028BC4
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToKebabCase(name);
		}
	}
}
