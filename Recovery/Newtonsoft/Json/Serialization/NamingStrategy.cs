using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NamingStrategy
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002AB90 File Offset: 0x00028D90
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0002AB98 File Offset: 0x00028D98
		public bool ProcessDictionaryKeys { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0002ABA4 File Offset: 0x00028DA4
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0002ABAC File Offset: 0x00028DAC
		public bool ProcessExtensionDataNames { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002ABB8 File Offset: 0x00028DB8
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0002ABC0 File Offset: 0x00028DC0
		public bool OverrideSpecifiedNames { get; set; }

		// Token: 0x06000823 RID: 2083 RVA: 0x0002ABCC File Offset: 0x00028DCC
		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002ABE8 File Offset: 0x00028DE8
		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0002AC00 File Offset: 0x00028E00
		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		// Token: 0x06000826 RID: 2086
		protected abstract string ResolvePropertyName(string name);

		// Token: 0x06000827 RID: 2087 RVA: 0x0002AC18 File Offset: 0x00028E18
		public override int GetHashCode()
		{
			return (((((base.GetType().GetHashCode() * 397) ^ this.ProcessDictionaryKeys.GetHashCode()) * 397) ^ this.ProcessExtensionDataNames.GetHashCode()) * 397) ^ this.OverrideSpecifiedNames.GetHashCode();
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0002AC74 File Offset: 0x00028E74
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NamingStrategy);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002AC84 File Offset: 0x00028E84
		[NullableContext(2)]
		protected bool Equals(NamingStrategy other)
		{
			return other != null && (base.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
		}
	}
}
