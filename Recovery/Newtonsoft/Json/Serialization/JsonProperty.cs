using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C7 RID: 199
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonProperty
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x000237F0 File Offset: 0x000219F0
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x000237F8 File Offset: 0x000219F8
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00023804 File Offset: 0x00021A04
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0002380C File Offset: 0x00021A0C
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00023830 File Offset: 0x00021A30
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00023838 File Offset: 0x00021A38
		public Type DeclaringType { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00023844 File Offset: 0x00021A44
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0002384C File Offset: 0x00021A4C
		public int? Order { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00023858 File Offset: 0x00021A58
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x00023860 File Offset: 0x00021A60
		public string UnderlyingName { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0002386C File Offset: 0x00021A6C
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x00023874 File Offset: 0x00021A74
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00023880 File Offset: 0x00021A80
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x00023888 File Offset: 0x00021A88
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00023894 File Offset: 0x00021A94
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0002389C File Offset: 0x00021A9C
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x000238C0 File Offset: 0x00021AC0
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x000238C8 File Offset: 0x00021AC8
		public JsonConverter Converter { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x000238D4 File Offset: 0x00021AD4
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x000238DC File Offset: 0x00021ADC
		[Obsolete("MemberConverter is obsolete. Use Converter instead.")]
		public JsonConverter MemberConverter
		{
			get
			{
				return this.Converter;
			}
			set
			{
				this.Converter = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000238E8 File Offset: 0x00021AE8
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x000238F0 File Offset: 0x00021AF0
		public bool Ignored { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x000238FC File Offset: 0x00021AFC
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00023904 File Offset: 0x00021B04
		public bool Readable { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00023910 File Offset: 0x00021B10
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00023918 File Offset: 0x00021B18
		public bool Writable { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00023924 File Offset: 0x00021B24
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0002392C File Offset: 0x00021B2C
		public bool HasMemberAttribute { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00023938 File Offset: 0x00021B38
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00023950 File Offset: 0x00021B50
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00023960 File Offset: 0x00021B60
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this._propertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x000239B8 File Offset: 0x00021BB8
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x000239C8 File Offset: 0x00021BC8
		public Required Required
		{
			get
			{
				return this._required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x000239D8 File Offset: 0x00021BD8
		public bool IsRequiredSpecified
		{
			get
			{
				return this._required != null;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x000239E8 File Offset: 0x00021BE8
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x000239F0 File Offset: 0x00021BF0
		public bool? IsReference { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x000239FC File Offset: 0x00021BFC
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00023A04 File Offset: 0x00021C04
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00023A10 File Offset: 0x00021C10
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x00023A18 File Offset: 0x00021C18
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00023A24 File Offset: 0x00021C24
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x00023A2C File Offset: 0x00021C2C
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00023A38 File Offset: 0x00021C38
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x00023A40 File Offset: 0x00021C40
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00023A4C File Offset: 0x00021C4C
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x00023A54 File Offset: 0x00021C54
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00023A60 File Offset: 0x00021C60
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x00023A68 File Offset: 0x00021C68
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldSerialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00023A74 File Offset: 0x00021C74
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x00023A7C File Offset: 0x00021C7C
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldDeserialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00023A88 File Offset: 0x00021C88
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x00023A90 File Offset: 0x00021C90
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> GetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00023A9C File Offset: 0x00021C9C
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x00023AA4 File Offset: 0x00021CA4
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> SetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00023AB0 File Offset: 0x00021CB0
		[NullableContext(1)]
		public override string ToString()
		{
			return this.PropertyName ?? string.Empty;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00023AC4 File Offset: 0x00021CC4
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x00023ACC File Offset: 0x00021CCC
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00023AD8 File Offset: 0x00021CD8
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x00023AE0 File Offset: 0x00021CE0
		public bool? ItemIsReference { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00023AEC File Offset: 0x00021CEC
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x00023AF4 File Offset: 0x00021CF4
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00023B00 File Offset: 0x00021D00
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x00023B08 File Offset: 0x00021D08
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x06000754 RID: 1876 RVA: 0x00023B14 File Offset: 0x00021D14
		[NullableContext(1)]
		internal void WritePropertyName(JsonWriter writer)
		{
			string propertyName = this.PropertyName;
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(propertyName, false);
				return;
			}
			writer.WritePropertyName(propertyName);
		}

		// Token: 0x04000350 RID: 848
		internal Required? _required;

		// Token: 0x04000351 RID: 849
		internal bool _hasExplicitDefaultValue;

		// Token: 0x04000352 RID: 850
		private object _defaultValue;

		// Token: 0x04000353 RID: 851
		private bool _hasGeneratedDefaultValue;

		// Token: 0x04000354 RID: 852
		private string _propertyName;

		// Token: 0x04000355 RID: 853
		internal bool _skipPropertyNameEscape;

		// Token: 0x04000356 RID: 854
		private Type _propertyType;
	}
}
