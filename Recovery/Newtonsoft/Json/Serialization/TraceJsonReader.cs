using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000D8 RID: 216
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x0002AEA4 File Offset: 0x000290A4
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Deserialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002AF0C File Offset: 0x0002910C
		public string GetDeserializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002AF1C File Offset: 0x0002911C
		public override bool Read()
		{
			bool flag = this._innerReader.Read();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002AF30 File Offset: 0x00029130
		public override int? ReadAsInt32()
		{
			int? num = this._innerReader.ReadAsInt32();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002AF44 File Offset: 0x00029144
		[NullableContext(2)]
		public override string ReadAsString()
		{
			string text = this._innerReader.ReadAsString();
			this.WriteCurrentToken();
			return text;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002AF58 File Offset: 0x00029158
		[NullableContext(2)]
		public override byte[] ReadAsBytes()
		{
			byte[] array = this._innerReader.ReadAsBytes();
			this.WriteCurrentToken();
			return array;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002AF6C File Offset: 0x0002916C
		public override decimal? ReadAsDecimal()
		{
			decimal? num = this._innerReader.ReadAsDecimal();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002AF80 File Offset: 0x00029180
		public override double? ReadAsDouble()
		{
			double? num = this._innerReader.ReadAsDouble();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002AF94 File Offset: 0x00029194
		public override bool? ReadAsBoolean()
		{
			bool? flag = this._innerReader.ReadAsBoolean();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0002AFA8 File Offset: 0x000291A8
		public override DateTime? ReadAsDateTime()
		{
			DateTime? dateTime = this._innerReader.ReadAsDateTime();
			this.WriteCurrentToken();
			return dateTime;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002AFBC File Offset: 0x000291BC
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? dateTimeOffset = this._innerReader.ReadAsDateTimeOffset();
			this.WriteCurrentToken();
			return dateTimeOffset;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0002AFD0 File Offset: 0x000291D0
		public void WriteCurrentToken()
		{
			this._textWriter.WriteToken(this._innerReader, false, false, true);
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0002AFE8 File Offset: 0x000291E8
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x0002AFF8 File Offset: 0x000291F8
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0002B008 File Offset: 0x00029208
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0002B018 File Offset: 0x00029218
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0002B028 File Offset: 0x00029228
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0002B038 File Offset: 0x00029238
		[Nullable(2)]
		public override object Value
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0002B048 File Offset: 0x00029248
		[Nullable(2)]
		public override Type ValueType
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002B058 File Offset: 0x00029258
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002B068 File Offset: 0x00029268
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0002B094 File Offset: 0x00029294
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0002B0C0 File Offset: 0x000292C0
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x04000393 RID: 915
		private readonly JsonReader _innerReader;

		// Token: 0x04000394 RID: 916
		private readonly JsonTextWriter _textWriter;

		// Token: 0x04000395 RID: 917
		private readonly StringWriter _sw;
	}
}
