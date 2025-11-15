using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C5 RID: 197
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00023404 File Offset: 0x00021604
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0002340C File Offset: 0x0002160C
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00023418 File Offset: 0x00021618
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x00023420 File Offset: 0x00021620
		public MissingMemberHandling? MissingMemberHandling { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002342C File Offset: 0x0002162C
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x00023434 File Offset: 0x00021634
		public Required? ItemRequired { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00023440 File Offset: 0x00021640
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x00023448 File Offset: 0x00021648
		public NullValueHandling? ItemNullValueHandling { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00023454 File Offset: 0x00021654
		[Nullable(1)]
		public JsonPropertyCollection Properties
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0002345C File Offset: 0x0002165C
		[Nullable(1)]
		public JsonPropertyCollection CreatorParameters
		{
			[NullableContext(1)]
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00023480 File Offset: 0x00021680
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x00023488 File Offset: 0x00021688
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00023494 File Offset: 0x00021694
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0002349C File Offset: 0x0002169C
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._parameterizedCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._parameterizedCreator = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000234A8 File Offset: 0x000216A8
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x000234B0 File Offset: 0x000216B0
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x000234BC File Offset: 0x000216BC
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x000234C4 File Offset: 0x000216C4
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000234D0 File Offset: 0x000216D0
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x000234D8 File Offset: 0x000216D8
		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = value != null && typeof(JToken).IsAssignableFrom(value);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0002350C File Offset: 0x0002170C
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00023514 File Offset: 0x00021714
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> ExtensionDataNameResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00023520 File Offset: 0x00021720
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate;
								DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
								if (!((defaultValueHandling.GetValueOrDefault() == defaultValueHandling2) & (defaultValueHandling != null)))
								{
									continue;
								}
							}
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
							break;
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00023624 File Offset: 0x00021824
		[NullableContext(1)]
		public JsonObjectContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00023648 File Offset: 0x00021848
		[NullableContext(1)]
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x04000348 RID: 840
		internal bool ExtensionDataIsJToken;

		// Token: 0x04000349 RID: 841
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x0400034A RID: 842
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x0400034B RID: 843
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x0400034C RID: 844
		private JsonPropertyCollection _creatorParameters;

		// Token: 0x0400034D RID: 845
		private Type _extensionDataValueType;
	}
}
