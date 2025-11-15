using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000CC RID: 204
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007BA RID: 1978 RVA: 0x00029E74 File Offset: 0x00028074
		// (remove) Token: 0x060007BB RID: 1979 RVA: 0x00029E84 File Offset: 0x00028084
		[Nullable(new byte[] { 2, 1 })]
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00029E94 File Offset: 0x00028094
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x00029EA4 File Offset: 0x000280A4
		[Nullable(2)]
		public override IReferenceResolver ReferenceResolver
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.ReferenceResolver;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x00029EB4 File Offset: 0x000280B4
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x00029EC4 File Offset: 0x000280C4
		[Nullable(2)]
		public override ITraceWriter TraceWriter
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.TraceWriter;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.TraceWriter = value;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00029ED4 File Offset: 0x000280D4
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00029EE4 File Offset: 0x000280E4
		[Nullable(2)]
		public override IEqualityComparer EqualityComparer
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.EqualityComparer;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.EqualityComparer = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00029EF4 File Offset: 0x000280F4
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00029F04 File Offset: 0x00028104
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00029F14 File Offset: 0x00028114
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00029F24 File Offset: 0x00028124
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x00029F34 File Offset: 0x00028134
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00029F44 File Offset: 0x00028144
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00029F54 File Offset: 0x00028154
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00029F64 File Offset: 0x00028164
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x00029F74 File Offset: 0x00028174
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00029F84 File Offset: 0x00028184
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x00029F94 File Offset: 0x00028194
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00029FA4 File Offset: 0x000281A4
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x00029FB4 File Offset: 0x000281B4
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00029FC4 File Offset: 0x000281C4
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x00029FD4 File Offset: 0x000281D4
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00029FE4 File Offset: 0x000281E4
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00029FF4 File Offset: 0x000281F4
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002A004 File Offset: 0x00028204
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x0002A014 File Offset: 0x00028214
		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._serializer.MetadataPropertyHandling;
			}
			set
			{
				this._serializer.MetadataPropertyHandling = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002A024 File Offset: 0x00028224
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0002A034 File Offset: 0x00028234
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002A044 File Offset: 0x00028244
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0002A054 File Offset: 0x00028254
		public override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0002A064 File Offset: 0x00028264
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0002A074 File Offset: 0x00028274
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002A084 File Offset: 0x00028284
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0002A094 File Offset: 0x00028294
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002A0A4 File Offset: 0x000282A4
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x0002A0B4 File Offset: 0x000282B4
		public override ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializer.SerializationBinder;
			}
			set
			{
				this._serializer.SerializationBinder = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002A0C4 File Offset: 0x000282C4
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0002A0D4 File Offset: 0x000282D4
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0002A0E4 File Offset: 0x000282E4
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0002A0F4 File Offset: 0x000282F4
		public override Formatting Formatting
		{
			get
			{
				return this._serializer.Formatting;
			}
			set
			{
				this._serializer.Formatting = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002A104 File Offset: 0x00028304
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0002A114 File Offset: 0x00028314
		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._serializer.DateFormatHandling;
			}
			set
			{
				this._serializer.DateFormatHandling = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0002A124 File Offset: 0x00028324
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0002A134 File Offset: 0x00028334
		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._serializer.DateTimeZoneHandling;
			}
			set
			{
				this._serializer.DateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0002A144 File Offset: 0x00028344
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x0002A154 File Offset: 0x00028354
		public override DateParseHandling DateParseHandling
		{
			get
			{
				return this._serializer.DateParseHandling;
			}
			set
			{
				this._serializer.DateParseHandling = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0002A164 File Offset: 0x00028364
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x0002A174 File Offset: 0x00028374
		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._serializer.FloatFormatHandling;
			}
			set
			{
				this._serializer.FloatFormatHandling = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0002A184 File Offset: 0x00028384
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0002A194 File Offset: 0x00028394
		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._serializer.FloatParseHandling;
			}
			set
			{
				this._serializer.FloatParseHandling = value;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002A1A4 File Offset: 0x000283A4
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0002A1B4 File Offset: 0x000283B4
		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._serializer.StringEscapeHandling;
			}
			set
			{
				this._serializer.StringEscapeHandling = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002A1C4 File Offset: 0x000283C4
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0002A1D4 File Offset: 0x000283D4
		public override string DateFormatString
		{
			get
			{
				return this._serializer.DateFormatString;
			}
			set
			{
				this._serializer.DateFormatString = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002A1E4 File Offset: 0x000283E4
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0002A1F4 File Offset: 0x000283F4
		public override CultureInfo Culture
		{
			get
			{
				return this._serializer.Culture;
			}
			set
			{
				this._serializer.Culture = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002A204 File Offset: 0x00028404
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0002A214 File Offset: 0x00028414
		public override int? MaxDepth
		{
			get
			{
				return this._serializer.MaxDepth;
			}
			set
			{
				this._serializer.MaxDepth = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002A224 File Offset: 0x00028424
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0002A234 File Offset: 0x00028434
		public override bool CheckAdditionalContent
		{
			get
			{
				return this._serializer.CheckAdditionalContent;
			}
			set
			{
				this._serializer.CheckAdditionalContent = value;
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0002A244 File Offset: 0x00028444
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002A260 File Offset: 0x00028460
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002A288 File Offset: 0x00028488
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002A2B0 File Offset: 0x000284B0
		[NullableContext(2)]
		internal override object DeserializeInternal([Nullable(1)] JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType, false);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002A2DC File Offset: 0x000284DC
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002A304 File Offset: 0x00028504
		[NullableContext(2)]
		internal override void SerializeInternal([Nullable(1)] JsonWriter jsonWriter, object value, Type rootType)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value, rootType);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x0400037A RID: 890
		[Nullable(2)]
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x0400037B RID: 891
		[Nullable(2)]
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x0400037C RID: 892
		private readonly JsonSerializer _serializer;
	}
}
