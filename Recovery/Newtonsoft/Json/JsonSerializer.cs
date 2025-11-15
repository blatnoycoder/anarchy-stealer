using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000061 RID: 97
	[NullableContext(1)]
	[Nullable(0)]
	public class JsonSerializer
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000252 RID: 594 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		// (remove) Token: 0x06000253 RID: 595 RVA: 0x0000C90C File Offset: 0x0000AB0C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public virtual event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000C948 File Offset: 0x0000AB48
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000C950 File Offset: 0x0000AB50
		[Nullable(2)]
		public virtual IReferenceResolver ReferenceResolver
		{
			[NullableContext(2)]
			get
			{
				return this.GetReferenceResolver();
			}
			[NullableContext(2)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				this._referenceResolver = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000C970 File Offset: 0x0000AB70
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public virtual SerializationBinder Binder
		{
			get
			{
				SerializationBinder serializationBinder = this._serializationBinder as SerializationBinder;
				if (serializationBinder != null)
				{
					return serializationBinder;
				}
				SerializationBinderAdapter serializationBinderAdapter = this._serializationBinder as SerializationBinderAdapter;
				if (serializationBinderAdapter != null)
				{
					return serializationBinderAdapter.SerializationBinder;
				}
				throw new InvalidOperationException("Cannot get SerializationBinder because an ISerializationBinder was previously set.");
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._serializationBinder = (value as ISerializationBinder) ?? new SerializationBinderAdapter(value);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000C9EC File Offset: 0x0000ABEC
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000C9F4 File Offset: 0x0000ABF4
		public virtual ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializationBinder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._serializationBinder = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000CA14 File Offset: 0x0000AC14
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000CA1C File Offset: 0x0000AC1C
		[Nullable(2)]
		public virtual ITraceWriter TraceWriter
		{
			[NullableContext(2)]
			get
			{
				return this._traceWriter;
			}
			[NullableContext(2)]
			set
			{
				this._traceWriter = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000CA28 File Offset: 0x0000AC28
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000CA30 File Offset: 0x0000AC30
		[Nullable(2)]
		public virtual IEqualityComparer EqualityComparer
		{
			[NullableContext(2)]
			get
			{
				return this._equalityComparer;
			}
			[NullableContext(2)]
			set
			{
				this._equalityComparer = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000CA44 File Offset: 0x0000AC44
		public virtual TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling;
			}
			set
			{
				if (value < TypeNameHandling.None || value > TypeNameHandling.Auto)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameHandling = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000CA68 File Offset: 0x0000AC68
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000CA70 File Offset: 0x0000AC70
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return (FormatterAssemblyStyle)this._typeNameAssemblyFormatHandling;
			}
			set
			{
				if (value < FormatterAssemblyStyle.Simple || value > FormatterAssemblyStyle.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameAssemblyFormatHandling = (TypeNameAssemblyFormatHandling)value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000CA94 File Offset: 0x0000AC94
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		public virtual TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._typeNameAssemblyFormatHandling;
			}
			set
			{
				if (value < TypeNameAssemblyFormatHandling.Simple || value > TypeNameAssemblyFormatHandling.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000CAC0 File Offset: 0x0000ACC0
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		public virtual PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling;
			}
			set
			{
				if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._preserveReferencesHandling = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000CAEC File Offset: 0x0000ACEC
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000CAF4 File Offset: 0x0000ACF4
		public virtual ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling;
			}
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._referenceLoopHandling = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000CB18 File Offset: 0x0000AD18
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000CB20 File Offset: 0x0000AD20
		public virtual MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling;
			}
			set
			{
				if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._missingMemberHandling = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000CB44 File Offset: 0x0000AD44
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000CB4C File Offset: 0x0000AD4C
		public virtual NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling;
			}
			set
			{
				if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._nullValueHandling = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000CB70 File Offset: 0x0000AD70
		// (set) Token: 0x0600026D RID: 621 RVA: 0x0000CB78 File Offset: 0x0000AD78
		public virtual DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling;
			}
			set
			{
				if (value < DefaultValueHandling.Include || value > DefaultValueHandling.IgnoreAndPopulate)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._defaultValueHandling = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		// (set) Token: 0x0600026F RID: 623 RVA: 0x0000CBA4 File Offset: 0x0000ADA4
		public virtual ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling;
			}
			set
			{
				if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._objectCreationHandling = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000CBC8 File Offset: 0x0000ADC8
		// (set) Token: 0x06000271 RID: 625 RVA: 0x0000CBD0 File Offset: 0x0000ADD0
		public virtual ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling;
			}
			set
			{
				if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._constructorHandling = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		public virtual MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._metadataPropertyHandling;
			}
			set
			{
				if (value < MetadataPropertyHandling.Default || value > MetadataPropertyHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._metadataPropertyHandling = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000CC20 File Offset: 0x0000AE20
		public virtual JsonConverterCollection Converters
		{
			get
			{
				if (this._converters == null)
				{
					this._converters = new JsonConverterCollection();
				}
				return this._converters;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000CC40 File Offset: 0x0000AE40
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000CC48 File Offset: 0x0000AE48
		public virtual IContractResolver ContractResolver
		{
			get
			{
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value ?? DefaultContractResolver.Instance;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000CC60 File Offset: 0x0000AE60
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000CC68 File Offset: 0x0000AE68
		public virtual StreamingContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000CC74 File Offset: 0x0000AE74
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000CC84 File Offset: 0x0000AE84
		public virtual Formatting Formatting
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000CC94 File Offset: 0x0000AE94
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000CCA4 File Offset: 0x0000AEA4
		public virtual DateFormatHandling DateFormatHandling
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000CCB4 File Offset: 0x0000AEB4
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		public virtual DateTimeZoneHandling DateTimeZoneHandling
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
		// (set) Token: 0x06000280 RID: 640 RVA: 0x0000CD24 File Offset: 0x0000AF24
		public virtual DateParseHandling DateParseHandling
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000CD34 File Offset: 0x0000AF34
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0000CD44 File Offset: 0x0000AF44
		public virtual FloatParseHandling FloatParseHandling
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000CD54 File Offset: 0x0000AF54
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000CD64 File Offset: 0x0000AF64
		public virtual FloatFormatHandling FloatFormatHandling
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000CD74 File Offset: 0x0000AF74
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000CD84 File Offset: 0x0000AF84
		public virtual StringEscapeHandling StringEscapeHandling
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000CD94 File Offset: 0x0000AF94
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		public virtual string DateFormatString
		{
			get
			{
				return this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				this._dateFormatString = value;
				this._dateFormatStringSet = true;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000CDCC File Offset: 0x0000AFCC
		public virtual CultureInfo Culture
		{
			get
			{
				return this._culture ?? JsonSerializerSettings.DefaultCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000CDE0 File Offset: 0x0000AFE0
		public virtual int? MaxDepth
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000CE30 File Offset: 0x0000B030
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000CE40 File Offset: 0x0000B040
		public virtual bool CheckAdditionalContent
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

		// Token: 0x0600028F RID: 655 RVA: 0x0000CE50 File Offset: 0x0000B050
		internal bool IsCheckAdditionalContentSet()
		{
			return this._checkAdditionalContent != null;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000CE60 File Offset: 0x0000B060
		public JsonSerializer()
		{
			this._referenceLoopHandling = ReferenceLoopHandling.Error;
			this._missingMemberHandling = MissingMemberHandling.Ignore;
			this._nullValueHandling = NullValueHandling.Include;
			this._defaultValueHandling = DefaultValueHandling.Include;
			this._objectCreationHandling = ObjectCreationHandling.Auto;
			this._preserveReferencesHandling = PreserveReferencesHandling.None;
			this._constructorHandling = ConstructorHandling.Default;
			this._typeNameHandling = TypeNameHandling.None;
			this._metadataPropertyHandling = MetadataPropertyHandling.Default;
			this._context = JsonSerializerSettings.DefaultContext;
			this._serializationBinder = DefaultSerializationBinder.Instance;
			this._culture = JsonSerializerSettings.DefaultCulture;
			this._contractResolver = DefaultContractResolver.Instance;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000CEE4 File Offset: 0x0000B0E4
		public static JsonSerializer Create()
		{
			return new JsonSerializer();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000CEEC File Offset: 0x0000B0EC
		public static JsonSerializer Create([Nullable(2)] JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.Create();
			if (settings != null)
			{
				JsonSerializer.ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000CF14 File Offset: 0x0000B114
		public static JsonSerializer CreateDefault()
		{
			Func<JsonSerializerSettings> defaultSettings = JsonConvert.DefaultSettings;
			return JsonSerializer.Create((defaultSettings != null) ? defaultSettings() : null);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CF34 File Offset: 0x0000B134
		public static JsonSerializer CreateDefault([Nullable(2)] JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
			if (settings != null)
			{
				JsonSerializer.ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CF5C File Offset: 0x0000B15C
		private static void ApplySerializerSettings(JsonSerializer serializer, JsonSerializerSettings settings)
		{
			if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(settings.Converters))
			{
				for (int i = 0; i < settings.Converters.Count; i++)
				{
					serializer.Converters.Insert(i, settings.Converters[i]);
				}
			}
			if (settings._typeNameHandling != null)
			{
				serializer.TypeNameHandling = settings.TypeNameHandling;
			}
			if (settings._metadataPropertyHandling != null)
			{
				serializer.MetadataPropertyHandling = settings.MetadataPropertyHandling;
			}
			if (settings._typeNameAssemblyFormatHandling != null)
			{
				serializer.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
			}
			if (settings._preserveReferencesHandling != null)
			{
				serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
			}
			if (settings._referenceLoopHandling != null)
			{
				serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
			}
			if (settings._missingMemberHandling != null)
			{
				serializer.MissingMemberHandling = settings.MissingMemberHandling;
			}
			if (settings._objectCreationHandling != null)
			{
				serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
			}
			if (settings._nullValueHandling != null)
			{
				serializer.NullValueHandling = settings.NullValueHandling;
			}
			if (settings._defaultValueHandling != null)
			{
				serializer.DefaultValueHandling = settings.DefaultValueHandling;
			}
			if (settings._constructorHandling != null)
			{
				serializer.ConstructorHandling = settings.ConstructorHandling;
			}
			if (settings._context != null)
			{
				serializer.Context = settings.Context;
			}
			if (settings._checkAdditionalContent != null)
			{
				serializer._checkAdditionalContent = settings._checkAdditionalContent;
			}
			if (settings.Error != null)
			{
				serializer.Error += settings.Error;
			}
			if (settings.ContractResolver != null)
			{
				serializer.ContractResolver = settings.ContractResolver;
			}
			if (settings.ReferenceResolverProvider != null)
			{
				serializer.ReferenceResolver = settings.ReferenceResolverProvider();
			}
			if (settings.TraceWriter != null)
			{
				serializer.TraceWriter = settings.TraceWriter;
			}
			if (settings.EqualityComparer != null)
			{
				serializer.EqualityComparer = settings.EqualityComparer;
			}
			if (settings.SerializationBinder != null)
			{
				serializer.SerializationBinder = settings.SerializationBinder;
			}
			if (settings._formatting != null)
			{
				serializer._formatting = settings._formatting;
			}
			if (settings._dateFormatHandling != null)
			{
				serializer._dateFormatHandling = settings._dateFormatHandling;
			}
			if (settings._dateTimeZoneHandling != null)
			{
				serializer._dateTimeZoneHandling = settings._dateTimeZoneHandling;
			}
			if (settings._dateParseHandling != null)
			{
				serializer._dateParseHandling = settings._dateParseHandling;
			}
			if (settings._dateFormatStringSet)
			{
				serializer._dateFormatString = settings._dateFormatString;
				serializer._dateFormatStringSet = settings._dateFormatStringSet;
			}
			if (settings._floatFormatHandling != null)
			{
				serializer._floatFormatHandling = settings._floatFormatHandling;
			}
			if (settings._floatParseHandling != null)
			{
				serializer._floatParseHandling = settings._floatParseHandling;
			}
			if (settings._stringEscapeHandling != null)
			{
				serializer._stringEscapeHandling = settings._stringEscapeHandling;
			}
			if (settings._culture != null)
			{
				serializer._culture = settings._culture;
			}
			if (settings._maxDepthSet)
			{
				serializer._maxDepth = settings._maxDepth;
				serializer._maxDepthSet = settings._maxDepthSet;
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		[DebuggerStepThrough]
		public void Populate(TextReader reader, object target)
		{
			this.Populate(new JsonTextReader(reader), target);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000D2C0 File Offset: 0x0000B4C0
		[DebuggerStepThrough]
		public void Populate(JsonReader reader, object target)
		{
			this.PopulateInternal(reader, target);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D2CC File Offset: 0x0000B4CC
		internal virtual void PopulateInternal(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(target, "target");
			CultureInfo cultureInfo;
			DateTimeZoneHandling? dateTimeZoneHandling;
			DateParseHandling? dateParseHandling;
			FloatParseHandling? floatParseHandling;
			int? num;
			string text;
			this.SetupReader(reader, out cultureInfo, out dateTimeZoneHandling, out dateParseHandling, out floatParseHandling, out num, out text);
			TraceJsonReader traceJsonReader = ((this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose) ? this.CreateTraceJsonReader(reader) : null);
			new JsonSerializerInternalReader(this).Populate(traceJsonReader ?? reader, target);
			if (traceJsonReader != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, traceJsonReader.GetDeserializedJsonMessage(), null);
			}
			this.ResetReader(reader, cultureInfo, dateTimeZoneHandling, dateParseHandling, floatParseHandling, num, text);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D374 File Offset: 0x0000B574
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public object Deserialize(JsonReader reader)
		{
			return this.Deserialize(reader, null);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D380 File Offset: 0x0000B580
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public object Deserialize(TextReader reader, Type objectType)
		{
			return this.Deserialize(new JsonTextReader(reader), objectType);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000D390 File Offset: 0x0000B590
		[DebuggerStepThrough]
		[return: MaybeNull]
		public T Deserialize<[Nullable(2)] T>(JsonReader reader)
		{
			return (T)((object)this.Deserialize(reader, typeof(T)));
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
		[NullableContext(2)]
		[DebuggerStepThrough]
		public object Deserialize([Nullable(1)] JsonReader reader, Type objectType)
		{
			return this.DeserializeInternal(reader, objectType);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		[NullableContext(2)]
		internal virtual object DeserializeInternal([Nullable(1)] JsonReader reader, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			CultureInfo cultureInfo;
			DateTimeZoneHandling? dateTimeZoneHandling;
			DateParseHandling? dateParseHandling;
			FloatParseHandling? floatParseHandling;
			int? num;
			string text;
			this.SetupReader(reader, out cultureInfo, out dateTimeZoneHandling, out dateParseHandling, out floatParseHandling, out num, out text);
			TraceJsonReader traceJsonReader = ((this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose) ? this.CreateTraceJsonReader(reader) : null);
			object obj = new JsonSerializerInternalReader(this).Deserialize(traceJsonReader ?? reader, objectType, this.CheckAdditionalContent);
			if (traceJsonReader != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, traceJsonReader.GetDeserializedJsonMessage(), null);
			}
			this.ResetReader(reader, cultureInfo, dateTimeZoneHandling, dateParseHandling, floatParseHandling, num, text);
			return obj;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D454 File Offset: 0x0000B654
		[NullableContext(2)]
		private void SetupReader([Nullable(1)] JsonReader reader, out CultureInfo previousCulture, out DateTimeZoneHandling? previousDateTimeZoneHandling, out DateParseHandling? previousDateParseHandling, out FloatParseHandling? previousFloatParseHandling, out int? previousMaxDepth, out string previousDateFormatString)
		{
			if (this._culture != null && !this._culture.Equals(reader.Culture))
			{
				previousCulture = reader.Culture;
				reader.Culture = this._culture;
			}
			else
			{
				previousCulture = null;
			}
			if (this._dateTimeZoneHandling != null)
			{
				DateTimeZoneHandling dateTimeZoneHandling = reader.DateTimeZoneHandling;
				DateTimeZoneHandling? dateTimeZoneHandling2 = this._dateTimeZoneHandling;
				if (!((dateTimeZoneHandling == dateTimeZoneHandling2.GetValueOrDefault()) & (dateTimeZoneHandling2 != null)))
				{
					previousDateTimeZoneHandling = new DateTimeZoneHandling?(reader.DateTimeZoneHandling);
					reader.DateTimeZoneHandling = this._dateTimeZoneHandling.GetValueOrDefault();
					goto IL_009E;
				}
			}
			previousDateTimeZoneHandling = null;
			IL_009E:
			if (this._dateParseHandling != null)
			{
				DateParseHandling dateParseHandling = reader.DateParseHandling;
				DateParseHandling? dateParseHandling2 = this._dateParseHandling;
				if (!((dateParseHandling == dateParseHandling2.GetValueOrDefault()) & (dateParseHandling2 != null)))
				{
					previousDateParseHandling = new DateParseHandling?(reader.DateParseHandling);
					reader.DateParseHandling = this._dateParseHandling.GetValueOrDefault();
					goto IL_0101;
				}
			}
			previousDateParseHandling = null;
			IL_0101:
			if (this._floatParseHandling != null)
			{
				FloatParseHandling floatParseHandling = reader.FloatParseHandling;
				FloatParseHandling? floatParseHandling2 = this._floatParseHandling;
				if (!((floatParseHandling == floatParseHandling2.GetValueOrDefault()) & (floatParseHandling2 != null)))
				{
					previousFloatParseHandling = new FloatParseHandling?(reader.FloatParseHandling);
					reader.FloatParseHandling = this._floatParseHandling.GetValueOrDefault();
					goto IL_0164;
				}
			}
			previousFloatParseHandling = null;
			IL_0164:
			if (this._maxDepthSet)
			{
				int? maxDepth = reader.MaxDepth;
				int? maxDepth2 = this._maxDepth;
				if (!((maxDepth.GetValueOrDefault() == maxDepth2.GetValueOrDefault()) & (maxDepth != null == (maxDepth2 != null))))
				{
					previousMaxDepth = reader.MaxDepth;
					reader.MaxDepth = this._maxDepth;
					goto IL_01CA;
				}
			}
			previousMaxDepth = null;
			IL_01CA:
			if (this._dateFormatStringSet && reader.DateFormatString != this._dateFormatString)
			{
				previousDateFormatString = reader.DateFormatString;
				reader.DateFormatString = this._dateFormatString;
			}
			else
			{
				previousDateFormatString = null;
			}
			JsonTextReader jsonTextReader = reader as JsonTextReader;
			if (jsonTextReader != null && jsonTextReader.PropertyNameTable == null)
			{
				DefaultContractResolver defaultContractResolver = this._contractResolver as DefaultContractResolver;
				if (defaultContractResolver != null)
				{
					jsonTextReader.PropertyNameTable = defaultContractResolver.GetNameTable();
				}
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D6AC File Offset: 0x0000B8AC
		[NullableContext(2)]
		private void ResetReader([Nullable(1)] JsonReader reader, CultureInfo previousCulture, DateTimeZoneHandling? previousDateTimeZoneHandling, DateParseHandling? previousDateParseHandling, FloatParseHandling? previousFloatParseHandling, int? previousMaxDepth, string previousDateFormatString)
		{
			if (previousCulture != null)
			{
				reader.Culture = previousCulture;
			}
			if (previousDateTimeZoneHandling != null)
			{
				reader.DateTimeZoneHandling = previousDateTimeZoneHandling.GetValueOrDefault();
			}
			if (previousDateParseHandling != null)
			{
				reader.DateParseHandling = previousDateParseHandling.GetValueOrDefault();
			}
			if (previousFloatParseHandling != null)
			{
				reader.FloatParseHandling = previousFloatParseHandling.GetValueOrDefault();
			}
			if (this._maxDepthSet)
			{
				reader.MaxDepth = previousMaxDepth;
			}
			if (this._dateFormatStringSet)
			{
				reader.DateFormatString = previousDateFormatString;
			}
			JsonTextReader jsonTextReader = reader as JsonTextReader;
			if (jsonTextReader != null && jsonTextReader.PropertyNameTable != null)
			{
				DefaultContractResolver defaultContractResolver = this._contractResolver as DefaultContractResolver;
				if (defaultContractResolver != null && jsonTextReader.PropertyNameTable == defaultContractResolver.GetNameTable())
				{
					jsonTextReader.PropertyNameTable = null;
				}
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D780 File Offset: 0x0000B980
		public void Serialize(TextWriter textWriter, [Nullable(2)] object value)
		{
			this.Serialize(new JsonTextWriter(textWriter), value);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000D790 File Offset: 0x0000B990
		[NullableContext(2)]
		public void Serialize([Nullable(1)] JsonWriter jsonWriter, object value, Type objectType)
		{
			this.SerializeInternal(jsonWriter, value, objectType);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000D79C File Offset: 0x0000B99C
		public void Serialize(TextWriter textWriter, [Nullable(2)] object value, Type objectType)
		{
			this.Serialize(new JsonTextWriter(textWriter), value, objectType);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public void Serialize(JsonWriter jsonWriter, [Nullable(2)] object value)
		{
			this.SerializeInternal(jsonWriter, value, null);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
		private TraceJsonReader CreateTraceJsonReader(JsonReader reader)
		{
			TraceJsonReader traceJsonReader = new TraceJsonReader(reader);
			if (reader.TokenType != JsonToken.None)
			{
				traceJsonReader.WriteCurrentToken();
			}
			return traceJsonReader;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
		[NullableContext(2)]
		internal virtual void SerializeInternal([Nullable(1)] JsonWriter jsonWriter, object value, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			Formatting? formatting = null;
			if (this._formatting != null)
			{
				Formatting formatting2 = jsonWriter.Formatting;
				Formatting? formatting3 = this._formatting;
				if (!((formatting2 == formatting3.GetValueOrDefault()) & (formatting3 != null)))
				{
					formatting = new Formatting?(jsonWriter.Formatting);
					jsonWriter.Formatting = this._formatting.GetValueOrDefault();
				}
			}
			DateFormatHandling? dateFormatHandling = null;
			if (this._dateFormatHandling != null)
			{
				DateFormatHandling dateFormatHandling2 = jsonWriter.DateFormatHandling;
				DateFormatHandling? dateFormatHandling3 = this._dateFormatHandling;
				if (!((dateFormatHandling2 == dateFormatHandling3.GetValueOrDefault()) & (dateFormatHandling3 != null)))
				{
					dateFormatHandling = new DateFormatHandling?(jsonWriter.DateFormatHandling);
					jsonWriter.DateFormatHandling = this._dateFormatHandling.GetValueOrDefault();
				}
			}
			DateTimeZoneHandling? dateTimeZoneHandling = null;
			if (this._dateTimeZoneHandling != null)
			{
				DateTimeZoneHandling dateTimeZoneHandling2 = jsonWriter.DateTimeZoneHandling;
				DateTimeZoneHandling? dateTimeZoneHandling3 = this._dateTimeZoneHandling;
				if (!((dateTimeZoneHandling2 == dateTimeZoneHandling3.GetValueOrDefault()) & (dateTimeZoneHandling3 != null)))
				{
					dateTimeZoneHandling = new DateTimeZoneHandling?(jsonWriter.DateTimeZoneHandling);
					jsonWriter.DateTimeZoneHandling = this._dateTimeZoneHandling.GetValueOrDefault();
				}
			}
			FloatFormatHandling? floatFormatHandling = null;
			if (this._floatFormatHandling != null)
			{
				FloatFormatHandling floatFormatHandling2 = jsonWriter.FloatFormatHandling;
				FloatFormatHandling? floatFormatHandling3 = this._floatFormatHandling;
				if (!((floatFormatHandling2 == floatFormatHandling3.GetValueOrDefault()) & (floatFormatHandling3 != null)))
				{
					floatFormatHandling = new FloatFormatHandling?(jsonWriter.FloatFormatHandling);
					jsonWriter.FloatFormatHandling = this._floatFormatHandling.GetValueOrDefault();
				}
			}
			StringEscapeHandling? stringEscapeHandling = null;
			if (this._stringEscapeHandling != null)
			{
				StringEscapeHandling stringEscapeHandling2 = jsonWriter.StringEscapeHandling;
				StringEscapeHandling? stringEscapeHandling3 = this._stringEscapeHandling;
				if (!((stringEscapeHandling2 == stringEscapeHandling3.GetValueOrDefault()) & (stringEscapeHandling3 != null)))
				{
					stringEscapeHandling = new StringEscapeHandling?(jsonWriter.StringEscapeHandling);
					jsonWriter.StringEscapeHandling = this._stringEscapeHandling.GetValueOrDefault();
				}
			}
			CultureInfo cultureInfo = null;
			if (this._culture != null && !this._culture.Equals(jsonWriter.Culture))
			{
				cultureInfo = jsonWriter.Culture;
				jsonWriter.Culture = this._culture;
			}
			string text = null;
			if (this._dateFormatStringSet && jsonWriter.DateFormatString != this._dateFormatString)
			{
				text = jsonWriter.DateFormatString;
				jsonWriter.DateFormatString = this._dateFormatString;
			}
			TraceJsonWriter traceJsonWriter = ((this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonWriter(jsonWriter) : null);
			new JsonSerializerInternalWriter(this).Serialize(traceJsonWriter ?? jsonWriter, value, objectType);
			if (traceJsonWriter != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, traceJsonWriter.GetSerializedJsonMessage(), null);
			}
			if (formatting != null)
			{
				jsonWriter.Formatting = formatting.GetValueOrDefault();
			}
			if (dateFormatHandling != null)
			{
				jsonWriter.DateFormatHandling = dateFormatHandling.GetValueOrDefault();
			}
			if (dateTimeZoneHandling != null)
			{
				jsonWriter.DateTimeZoneHandling = dateTimeZoneHandling.GetValueOrDefault();
			}
			if (floatFormatHandling != null)
			{
				jsonWriter.FloatFormatHandling = floatFormatHandling.GetValueOrDefault();
			}
			if (stringEscapeHandling != null)
			{
				jsonWriter.StringEscapeHandling = stringEscapeHandling.GetValueOrDefault();
			}
			if (this._dateFormatStringSet)
			{
				jsonWriter.DateFormatString = text;
			}
			if (cultureInfo != null)
			{
				jsonWriter.Culture = cultureInfo;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		internal IReferenceResolver GetReferenceResolver()
		{
			if (this._referenceResolver == null)
			{
				this._referenceResolver = new DefaultReferenceResolver();
			}
			return this._referenceResolver;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		[return: Nullable(2)]
		internal JsonConverter GetMatchingConverter(Type type)
		{
			return JsonSerializer.GetMatchingConverter(this._converters, type);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		[return: Nullable(2)]
		internal static JsonConverter GetMatchingConverter([Nullable(new byte[] { 2, 1 })] IList<JsonConverter> converters, Type objectType)
		{
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						return jsonConverter;
					}
				}
			}
			return null;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> error = this.Error;
			if (error == null)
			{
				return;
			}
			error(this, e);
		}

		// Token: 0x04000142 RID: 322
		internal TypeNameHandling _typeNameHandling;

		// Token: 0x04000143 RID: 323
		internal TypeNameAssemblyFormatHandling _typeNameAssemblyFormatHandling;

		// Token: 0x04000144 RID: 324
		internal PreserveReferencesHandling _preserveReferencesHandling;

		// Token: 0x04000145 RID: 325
		internal ReferenceLoopHandling _referenceLoopHandling;

		// Token: 0x04000146 RID: 326
		internal MissingMemberHandling _missingMemberHandling;

		// Token: 0x04000147 RID: 327
		internal ObjectCreationHandling _objectCreationHandling;

		// Token: 0x04000148 RID: 328
		internal NullValueHandling _nullValueHandling;

		// Token: 0x04000149 RID: 329
		internal DefaultValueHandling _defaultValueHandling;

		// Token: 0x0400014A RID: 330
		internal ConstructorHandling _constructorHandling;

		// Token: 0x0400014B RID: 331
		internal MetadataPropertyHandling _metadataPropertyHandling;

		// Token: 0x0400014C RID: 332
		[Nullable(2)]
		internal JsonConverterCollection _converters;

		// Token: 0x0400014D RID: 333
		internal IContractResolver _contractResolver;

		// Token: 0x0400014E RID: 334
		[Nullable(2)]
		internal ITraceWriter _traceWriter;

		// Token: 0x0400014F RID: 335
		[Nullable(2)]
		internal IEqualityComparer _equalityComparer;

		// Token: 0x04000150 RID: 336
		internal ISerializationBinder _serializationBinder;

		// Token: 0x04000151 RID: 337
		internal StreamingContext _context;

		// Token: 0x04000152 RID: 338
		[Nullable(2)]
		private IReferenceResolver _referenceResolver;

		// Token: 0x04000153 RID: 339
		private Formatting? _formatting;

		// Token: 0x04000154 RID: 340
		private DateFormatHandling? _dateFormatHandling;

		// Token: 0x04000155 RID: 341
		private DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x04000156 RID: 342
		private DateParseHandling? _dateParseHandling;

		// Token: 0x04000157 RID: 343
		private FloatFormatHandling? _floatFormatHandling;

		// Token: 0x04000158 RID: 344
		private FloatParseHandling? _floatParseHandling;

		// Token: 0x04000159 RID: 345
		private StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x0400015A RID: 346
		private CultureInfo _culture;

		// Token: 0x0400015B RID: 347
		private int? _maxDepth;

		// Token: 0x0400015C RID: 348
		private bool _maxDepthSet;

		// Token: 0x0400015D RID: 349
		private bool? _checkAdditionalContent;

		// Token: 0x0400015E RID: 350
		[Nullable(2)]
		private string _dateFormatString;

		// Token: 0x0400015F RID: 351
		private bool _dateFormatStringSet;
	}
}
