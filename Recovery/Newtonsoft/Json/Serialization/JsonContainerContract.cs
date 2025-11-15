using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B9 RID: 185
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00022750 File Offset: 0x00020950
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x00022758 File Offset: 0x00020958
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x000227AC File Offset: 0x000209AC
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x000227B4 File Offset: 0x000209B4
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x000227BC File Offset: 0x000209BC
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000227C8 File Offset: 0x000209C8
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000227D0 File Offset: 0x000209D0
		public bool? ItemIsReference { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x000227DC File Offset: 0x000209DC
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x000227E4 File Offset: 0x000209E4
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x000227F0 File Offset: 0x000209F0
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x000227F8 File Offset: 0x000209F8
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x0600069E RID: 1694 RVA: 0x00022804 File Offset: 0x00020A04
		[NullableContext(1)]
		internal JsonContainerContract(Type underlyingType)
			: base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x04000306 RID: 774
		private JsonContract _itemContract;

		// Token: 0x04000307 RID: 775
		private JsonContract _finalItemContract;
	}
}
