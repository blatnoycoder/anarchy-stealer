using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x0200005C RID: 92
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000AF00 File Offset: 0x00009100
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000AF08 File Offset: 0x00009108
		public Type ItemConverterType { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000AF14 File Offset: 0x00009114
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000AF1C File Offset: 0x0000911C
		[Nullable(new byte[] { 2, 1 })]
		public object[] ItemConverterParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000AF28 File Offset: 0x00009128
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000AF30 File Offset: 0x00009130
		public Type NamingStrategyType { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000AF3C File Offset: 0x0000913C
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000AF44 File Offset: 0x00009144
		[Nullable(new byte[] { 2, 1 })]
		public object[] NamingStrategyParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000AF50 File Offset: 0x00009150
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000AF60 File Offset: 0x00009160
		public NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000AF70 File Offset: 0x00009170
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000AF80 File Offset: 0x00009180
		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling.GetValueOrDefault();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000AF90 File Offset: 0x00009190
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000AFB0 File Offset: 0x000091B0
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000AFC0 File Offset: 0x000091C0
		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling.GetValueOrDefault();
			}
			set
			{
				this._objectCreationHandling = new ObjectCreationHandling?(value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000AFD0 File Offset: 0x000091D0
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000AFE0 File Offset: 0x000091E0
		public TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000AFF0 File Offset: 0x000091F0
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000B000 File Offset: 0x00009200
		public bool IsReference
		{
			get
			{
				return this._isReference.GetValueOrDefault();
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000B010 File Offset: 0x00009210
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000B020 File Offset: 0x00009220
		public int Order
		{
			get
			{
				return this._order.GetValueOrDefault();
			}
			set
			{
				this._order = new int?(value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000B030 File Offset: 0x00009230
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000B040 File Offset: 0x00009240
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B050 File Offset: 0x00009250
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000B058 File Offset: 0x00009258
		public string PropertyName { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000B064 File Offset: 0x00009264
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000B074 File Offset: 0x00009274
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				return this._itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000B084 File Offset: 0x00009284
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000B094 File Offset: 0x00009294
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				return this._itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000B0A4 File Offset: 0x000092A4
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000B0B4 File Offset: 0x000092B4
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference.GetValueOrDefault();
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000B0C4 File Offset: 0x000092C4
		public JsonPropertyAttribute()
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B0CC File Offset: 0x000092CC
		[NullableContext(1)]
		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x0400011D RID: 285
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x0400011E RID: 286
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x0400011F RID: 287
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x04000120 RID: 288
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x04000121 RID: 289
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x04000122 RID: 290
		internal bool? _isReference;

		// Token: 0x04000123 RID: 291
		internal int? _order;

		// Token: 0x04000124 RID: 292
		internal Required? _required;

		// Token: 0x04000125 RID: 293
		internal bool? _itemIsReference;

		// Token: 0x04000126 RID: 294
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x04000127 RID: 295
		internal TypeNameHandling? _itemTypeNameHandling;
	}
}
