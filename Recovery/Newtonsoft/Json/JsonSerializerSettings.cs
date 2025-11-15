using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000062 RID: 98
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonSerializerSettings
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000DBB8 File Offset: 0x0000BDB8
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000DBC8 File Offset: 0x0000BDC8
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling.GetValueOrDefault();
			}
			set
			{
				this._missingMemberHandling = new MissingMemberHandling?(value);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000DBF8 File Offset: 0x0000BDF8
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000DC08 File Offset: 0x0000BE08
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000DC18 File Offset: 0x0000BE18
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000DC28 File Offset: 0x0000BE28
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000DC38 File Offset: 0x0000BE38
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000DC48 File Offset: 0x0000BE48
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000DC58 File Offset: 0x0000BE58
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000DC60 File Offset: 0x0000BE60
		[Nullable(1)]
		public IList<JsonConverter> Converters
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling.GetValueOrDefault();
			}
			set
			{
				this._preserveReferencesHandling = new PreserveReferencesHandling?(value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000DC8C File Offset: 0x0000BE8C
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x0000DC9C File Offset: 0x0000BE9C
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		public MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._metadataPropertyHandling.GetValueOrDefault();
			}
			set
			{
				this._metadataPropertyHandling = new MetadataPropertyHandling?(value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000DCCC File Offset: 0x0000BECC
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return (FormatterAssemblyStyle)this.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this.TypeNameAssemblyFormatHandling = (TypeNameAssemblyFormatHandling)value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		public TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._typeNameAssemblyFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameAssemblyFormatHandling = new TypeNameAssemblyFormatHandling?(value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000DD00 File Offset: 0x0000BF00
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x0000DD10 File Offset: 0x0000BF10
		public ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling.GetValueOrDefault();
			}
			set
			{
				this._constructorHandling = new ConstructorHandling?(value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000DD20 File Offset: 0x0000BF20
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0000DD28 File Offset: 0x0000BF28
		public IContractResolver ContractResolver { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000DD34 File Offset: 0x0000BF34
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0000DD3C File Offset: 0x0000BF3C
		public IEqualityComparer EqualityComparer { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000DD48 File Offset: 0x0000BF48
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000DD60 File Offset: 0x0000BF60
		[Obsolete("ReferenceResolver property is obsolete. Use the ReferenceResolverProvider property to set the IReferenceResolver: settings.ReferenceResolverProvider = () => resolver")]
		public IReferenceResolver ReferenceResolver
		{
			get
			{
				Func<IReferenceResolver> referenceResolverProvider = this.ReferenceResolverProvider;
				if (referenceResolverProvider == null)
				{
					return null;
				}
				return referenceResolverProvider();
			}
			set
			{
				this.ReferenceResolverProvider = ((value != null) ? (() => value) : null);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000DDAC File Offset: 0x0000BFAC
		public Func<IReferenceResolver> ReferenceResolverProvider { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		// (set) Token: 0x060002CB RID: 715 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		public ITraceWriter TraceWriter { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000DE10 File Offset: 0x0000C010
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public SerializationBinder Binder
		{
			get
			{
				if (this.SerializationBinder == null)
				{
					return null;
				}
				SerializationBinderAdapter serializationBinderAdapter = this.SerializationBinder as SerializationBinderAdapter;
				if (serializationBinderAdapter != null)
				{
					return serializationBinderAdapter.SerializationBinder;
				}
				throw new InvalidOperationException("Cannot get SerializationBinder because an ISerializationBinder was previously set.");
			}
			set
			{
				this.SerializationBinder = ((value == null) ? null : new SerializationBinderAdapter(value));
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000DE2C File Offset: 0x0000C02C
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0000DE34 File Offset: 0x0000C034
		public ISerializationBinder SerializationBinder { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000DE40 File Offset: 0x0000C040
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000DE48 File Offset: 0x0000C048
		[Nullable(new byte[] { 2, 1 })]
		public EventHandler<ErrorEventArgs> Error
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000DE54 File Offset: 0x0000C054
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000DE88 File Offset: 0x0000C088
		public StreamingContext Context
		{
			get
			{
				StreamingContext? context = this._context;
				if (context == null)
				{
					return JsonSerializerSettings.DefaultContext;
				}
				return context.GetValueOrDefault();
			}
			set
			{
				this._context = new StreamingContext?(value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000DE98 File Offset: 0x0000C098
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		[Nullable(1)]
		public string DateFormatString
		{
			[NullableContext(1)]
			get
			{
				return this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			[NullableContext(1)]
			set
			{
				this._dateFormatString = value;
				this._dateFormatStringSet = true;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000DEBC File Offset: 0x0000C0BC
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000DEC4 File Offset: 0x0000C0C4
		public int? MaxDepth
		{
			get
			{
				return this._maxDepth;
			}
			set
			{
				int? num = value;
				int num2 = 0;
				if ((num.GetValueOrDefault() <= num2) & (num != null))
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				this._maxDepth = value;
				this._maxDepthSet = true;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000DF14 File Offset: 0x0000C114
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000DF24 File Offset: 0x0000C124
		public Formatting Formatting
		{
			get
			{
				return this._formatting.GetValueOrDefault();
			}
			set
			{
				this._formatting = new Formatting?(value);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000DF34 File Offset: 0x0000C134
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000DF44 File Offset: 0x0000C144
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._dateFormatHandling = new DateFormatHandling?(value);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000DF54 File Offset: 0x0000C154
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000DF84 File Offset: 0x0000C184
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				DateTimeZoneHandling? dateTimeZoneHandling = this._dateTimeZoneHandling;
				if (dateTimeZoneHandling == null)
				{
					return DateTimeZoneHandling.RoundtripKind;
				}
				return dateTimeZoneHandling.GetValueOrDefault();
			}
			set
			{
				this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000DF94 File Offset: 0x0000C194
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		public DateParseHandling DateParseHandling
		{
			get
			{
				DateParseHandling? dateParseHandling = this._dateParseHandling;
				if (dateParseHandling == null)
				{
					return DateParseHandling.DateTime;
				}
				return dateParseHandling.GetValueOrDefault();
			}
			set
			{
				this._dateParseHandling = new DateParseHandling?(value);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._floatFormatHandling = new FloatFormatHandling?(value);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000E004 File Offset: 0x0000C204
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._floatParseHandling.GetValueOrDefault();
			}
			set
			{
				this._floatParseHandling = new FloatParseHandling?(value);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000E014 File Offset: 0x0000C214
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000E024 File Offset: 0x0000C224
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling.GetValueOrDefault();
			}
			set
			{
				this._stringEscapeHandling = new StringEscapeHandling?(value);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000E034 File Offset: 0x0000C234
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000E048 File Offset: 0x0000C248
		[Nullable(1)]
		public CultureInfo Culture
		{
			[NullableContext(1)]
			get
			{
				return this._culture ?? JsonSerializerSettings.DefaultCulture;
			}
			[NullableContext(1)]
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000E054 File Offset: 0x0000C254
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000E064 File Offset: 0x0000C264
		public bool CheckAdditionalContent
		{
			get
			{
				return this._checkAdditionalContent.GetValueOrDefault();
			}
			set
			{
				this._checkAdditionalContent = new bool?(value);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E08C File Offset: 0x0000C28C
		[DebuggerStepThrough]
		public JsonSerializerSettings()
		{
			this.Converters = new List<JsonConverter>();
		}

		// Token: 0x04000161 RID: 353
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		// Token: 0x04000162 RID: 354
		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		// Token: 0x04000163 RID: 355
		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		// Token: 0x04000164 RID: 356
		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		// Token: 0x04000165 RID: 357
		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		// Token: 0x04000166 RID: 358
		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		// Token: 0x04000167 RID: 359
		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		// Token: 0x04000168 RID: 360
		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		// Token: 0x04000169 RID: 361
		internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;

		// Token: 0x0400016A RID: 362
		internal static readonly StreamingContext DefaultContext = default(StreamingContext);

		// Token: 0x0400016B RID: 363
		internal const Formatting DefaultFormatting = Formatting.None;

		// Token: 0x0400016C RID: 364
		internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;

		// Token: 0x0400016D RID: 365
		internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;

		// Token: 0x0400016E RID: 366
		internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;

		// Token: 0x0400016F RID: 367
		internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;

		// Token: 0x04000170 RID: 368
		internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;

		// Token: 0x04000171 RID: 369
		internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;

		// Token: 0x04000172 RID: 370
		internal const TypeNameAssemblyFormatHandling DefaultTypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

		// Token: 0x04000173 RID: 371
		[Nullable(1)]
		internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;

		// Token: 0x04000174 RID: 372
		internal const bool DefaultCheckAdditionalContent = false;

		// Token: 0x04000175 RID: 373
		[Nullable(1)]
		internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x04000176 RID: 374
		internal Formatting? _formatting;

		// Token: 0x04000177 RID: 375
		internal DateFormatHandling? _dateFormatHandling;

		// Token: 0x04000178 RID: 376
		internal DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x04000179 RID: 377
		internal DateParseHandling? _dateParseHandling;

		// Token: 0x0400017A RID: 378
		internal FloatFormatHandling? _floatFormatHandling;

		// Token: 0x0400017B RID: 379
		internal FloatParseHandling? _floatParseHandling;

		// Token: 0x0400017C RID: 380
		internal StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x0400017D RID: 381
		internal CultureInfo _culture;

		// Token: 0x0400017E RID: 382
		internal bool? _checkAdditionalContent;

		// Token: 0x0400017F RID: 383
		internal int? _maxDepth;

		// Token: 0x04000180 RID: 384
		internal bool _maxDepthSet;

		// Token: 0x04000181 RID: 385
		internal string _dateFormatString;

		// Token: 0x04000182 RID: 386
		internal bool _dateFormatStringSet;

		// Token: 0x04000183 RID: 387
		internal TypeNameAssemblyFormatHandling? _typeNameAssemblyFormatHandling;

		// Token: 0x04000184 RID: 388
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x04000185 RID: 389
		internal PreserveReferencesHandling? _preserveReferencesHandling;

		// Token: 0x04000186 RID: 390
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x04000187 RID: 391
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x04000188 RID: 392
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x04000189 RID: 393
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x0400018A RID: 394
		internal StreamingContext? _context;

		// Token: 0x0400018B RID: 395
		internal ConstructorHandling? _constructorHandling;

		// Token: 0x0400018C RID: 396
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x0400018D RID: 397
		internal MetadataPropertyHandling? _metadataPropertyHandling;
	}
}
