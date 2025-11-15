using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000065 RID: 101
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonTextWriter : JsonWriter
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600033B RID: 827 RVA: 0x000115D0 File Offset: 0x0000F7D0
		[Nullable(1)]
		private Base64Encoder Base64Encoder
		{
			[NullableContext(1)]
			get
			{
				if (this._base64Encoder == null)
				{
					this._base64Encoder = new Base64Encoder(this._writer);
				}
				return this._base64Encoder;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000115F4 File Offset: 0x0000F7F4
		// (set) Token: 0x0600033D RID: 829 RVA: 0x000115FC File Offset: 0x0000F7FC
		public IArrayPool<char> ArrayPool
		{
			get
			{
				return this._arrayPool;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._arrayPool = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00011618 File Offset: 0x0000F818
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00011620 File Offset: 0x0000F820
		public int Indentation
		{
			get
			{
				return this._indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				this._indentation = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0001163C File Offset: 0x0000F83C
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00011644 File Offset: 0x0000F844
		public char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				this._quoteChar = value;
				this.UpdateCharEscapeFlags();
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00011670 File Offset: 0x0000F870
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00011678 File Offset: 0x0000F878
		public char IndentChar
		{
			get
			{
				return this._indentChar;
			}
			set
			{
				if (value != this._indentChar)
				{
					this._indentChar = value;
					this._indentChars = null;
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00011694 File Offset: 0x0000F894
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0001169C File Offset: 0x0000F89C
		public bool QuoteName
		{
			get
			{
				return this._quoteName;
			}
			set
			{
				this._quoteName = value;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000116A8 File Offset: 0x0000F8A8
		[NullableContext(1)]
		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this._writer = textWriter;
			this._quoteChar = '"';
			this._quoteName = true;
			this._indentChar = ' ';
			this._indentation = 2;
			this.UpdateCharEscapeFlags();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000116FC File Offset: 0x0000F8FC
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001170C File Offset: 0x0000F90C
		public override void Close()
		{
			base.Close();
			this.CloseBufferAndWriter();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001171C File Offset: 0x0000F91C
		private void CloseBufferAndWriter()
		{
			if (this._writeBuffer != null)
			{
				BufferUtils.ReturnBuffer(this._arrayPool, this._writeBuffer);
				this._writeBuffer = null;
			}
			if (base.CloseOutput)
			{
				TextWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00011770 File Offset: 0x0000F970
		public override void WriteStartObject()
		{
			base.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
			this._writer.Write('{');
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00011788 File Offset: 0x0000F988
		public override void WriteStartArray()
		{
			base.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
			this._writer.Write('[');
		}

		// Token: 0x0600034C RID: 844 RVA: 0x000117A0 File Offset: 0x0000F9A0
		[NullableContext(1)]
		public override void WriteStartConstructor(string name)
		{
			base.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
			this._writer.Write("new ");
			this._writer.Write(name);
			this._writer.Write('(');
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000117E4 File Offset: 0x0000F9E4
		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				this._writer.Write('}');
				return;
			case JsonToken.EndArray:
				this._writer.Write(']');
				return;
			case JsonToken.EndConstructor:
				this._writer.Write(')');
				return;
			default:
				throw JsonWriterException.Create(this, "Invalid JsonToken: " + token.ToString(), null);
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00011858 File Offset: 0x0000FA58
		[NullableContext(1)]
		public override void WritePropertyName(string name)
		{
			base.InternalWritePropertyName(name);
			this.WriteEscapedString(name, this._quoteName);
			this._writer.Write(':');
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001187C File Offset: 0x0000FA7C
		[NullableContext(1)]
		public override void WritePropertyName(string name, bool escape)
		{
			base.InternalWritePropertyName(name);
			if (escape)
			{
				this.WriteEscapedString(name, this._quoteName);
			}
			else
			{
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
				this._writer.Write(name);
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
			}
			this._writer.Write(':');
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00011900 File Offset: 0x0000FB00
		internal override void OnStringEscapeHandlingChanged()
		{
			this.UpdateCharEscapeFlags();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00011908 File Offset: 0x0000FB08
		private void UpdateCharEscapeFlags()
		{
			this._charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(base.StringEscapeHandling, this._quoteChar);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011924 File Offset: 0x0000FB24
		protected override void WriteIndent()
		{
			int num = base.Top * this._indentation;
			int num2 = this.SetIndentChars();
			this._writer.Write(this._indentChars, 0, num2 + Math.Min(num, 12));
			while ((num -= 12) > 0)
			{
				this._writer.Write(this._indentChars, num2, Math.Min(num, 12));
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00011990 File Offset: 0x0000FB90
		private int SetIndentChars()
		{
			string newLine = this._writer.NewLine;
			int length = newLine.Length;
			bool flag = this._indentChars != null && this._indentChars.Length == 12 + length;
			if (flag)
			{
				for (int num = 0; num != length; num++)
				{
					if (newLine[num] != this._indentChars[num])
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				this._indentChars = (newLine + new string(this._indentChar, 12)).ToCharArray();
			}
			return length;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00011A2C File Offset: 0x0000FC2C
		protected override void WriteValueDelimiter()
		{
			this._writer.Write(',');
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00011A3C File Offset: 0x0000FC3C
		protected override void WriteIndentSpace()
		{
			this._writer.Write(' ');
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00011A4C File Offset: 0x0000FC4C
		[NullableContext(1)]
		private void WriteValueInternal(string value, JsonToken token)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011A5C File Offset: 0x0000FC5C
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value;
				base.InternalWriteValue(JsonToken.Integer);
				this.WriteValueInternal(bigInteger.ToString(CultureInfo.InvariantCulture), JsonToken.String);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011AA4 File Offset: 0x0000FCA4
		public override void WriteNull()
		{
			base.InternalWriteValue(JsonToken.Null);
			this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011ABC File Offset: 0x0000FCBC
		public override void WriteUndefined()
		{
			base.InternalWriteValue(JsonToken.Undefined);
			this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011AD4 File Offset: 0x0000FCD4
		public override void WriteRaw(string json)
		{
			base.InternalWriteRaw();
			this._writer.Write(json);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00011AE8 File Offset: 0x0000FCE8
		public override void WriteValue(string value)
		{
			base.InternalWriteValue(JsonToken.String);
			if (value == null)
			{
				this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
				return;
			}
			this.WriteEscapedString(value, true);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00011B10 File Offset: 0x0000FD10
		[NullableContext(1)]
		private void WriteEscapedString(string value, bool quote)
		{
			this.EnsureWriteBuffer();
			JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, base.StringEscapeHandling, this._arrayPool, ref this._writeBuffer);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00011B54 File Offset: 0x0000FD54
		public override void WriteValue(int value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00011B64 File Offset: 0x0000FD64
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)((ulong)value));
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00011B78 File Offset: 0x0000FD78
		public override void WriteValue(long value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00011B88 File Offset: 0x0000FD88
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value, false);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00011B9C File Offset: 0x0000FD9C
		public override void WriteValue(float value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		public override void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00011C1C File Offset: 0x0000FE1C
		public override void WriteValue(double value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00011C50 File Offset: 0x0000FE50
		public override void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00011C9C File Offset: 0x0000FE9C
		public override void WriteValue(bool value)
		{
			base.InternalWriteValue(JsonToken.Boolean);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00011CB4 File Offset: 0x0000FEB4
		public override void WriteValue(short value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00011CC4 File Offset: 0x0000FEC4
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00011CD4 File Offset: 0x0000FED4
		public override void WriteValue(char value)
		{
			base.InternalWriteValue(JsonToken.String);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00011CEC File Offset: 0x0000FEEC
		public override void WriteValue(byte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00011CFC File Offset: 0x0000FEFC
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00011D0C File Offset: 0x0000FF0C
		public override void WriteValue(decimal value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00011D24 File Offset: 0x0000FF24
		public override void WriteValue(DateTime value)
		{
			base.InternalWriteValue(JsonToken.Date);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (StringUtils.IsNullOrEmpty(base.DateFormatString))
			{
				int num = this.WriteValueToBuffer(value);
				this._writer.Write(this._writeBuffer, 0, num);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00011DB8 File Offset: 0x0000FFB8
		private int WriteValueToBuffer(DateTime value)
		{
			this.EnsureWriteBuffer();
			int num = 0;
			this._writeBuffer[num++] = this._quoteChar;
			num = DateTimeUtils.WriteDateTimeString(this._writeBuffer, num, value, null, value.Kind, base.DateFormatHandling);
			this._writeBuffer[num++] = this._quoteChar;
			return num;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00011E1C File Offset: 0x0001001C
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Bytes);
			this._writer.Write(this._quoteChar);
			this.Base64Encoder.Encode(value, 0, value.Length);
			this.Base64Encoder.Flush();
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00011E80 File Offset: 0x00010080
		public override void WriteValue(DateTimeOffset value)
		{
			base.InternalWriteValue(JsonToken.Date);
			if (StringUtils.IsNullOrEmpty(base.DateFormatString))
			{
				int num = this.WriteValueToBuffer(value);
				this._writer.Write(this._writeBuffer, 0, num);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00011F08 File Offset: 0x00010108
		private int WriteValueToBuffer(DateTimeOffset value)
		{
			this.EnsureWriteBuffer();
			int num = 0;
			this._writeBuffer[num++] = this._quoteChar;
			num = DateTimeUtils.WriteDateTimeString(this._writeBuffer, num, (base.DateFormatHandling == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, base.DateFormatHandling);
			this._writeBuffer[num++] = this._quoteChar;
			return num;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00011F84 File Offset: 0x00010184
		public override void WriteValue(Guid value)
		{
			base.InternalWriteValue(JsonToken.String);
			string text = value.ToString("D", CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(text);
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00011FE0 File Offset: 0x000101E0
		public override void WriteValue(TimeSpan value)
		{
			base.InternalWriteValue(JsonToken.String);
			string text = value.ToString(null, CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(text);
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00012038 File Offset: 0x00010238
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.String);
			this.WriteEscapedString(value.OriginalString, true);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00012074 File Offset: 0x00010274
		public override void WriteComment(string text)
		{
			base.InternalWriteComment();
			this._writer.Write("/*");
			this._writer.Write(text);
			this._writer.Write("*/");
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000120B8 File Offset: 0x000102B8
		[NullableContext(1)]
		public override void WriteWhitespace(string ws)
		{
			base.InternalWriteWhitespace(ws);
			this._writer.Write(ws);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000120D0 File Offset: 0x000102D0
		private void EnsureWriteBuffer()
		{
			if (this._writeBuffer == null)
			{
				this._writeBuffer = BufferUtils.RentBuffer(this._arrayPool, 35);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000120F0 File Offset: 0x000102F0
		private void WriteIntegerValue(long value)
		{
			if (value >= 0L && value <= 9L)
			{
				this._writer.Write((char)(48L + value));
				return;
			}
			bool flag = value < 0L;
			this.WriteIntegerValue((ulong)(flag ? (-(ulong)value) : value), flag);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00012140 File Offset: 0x00010340
		private void WriteIntegerValue(ulong value, bool negative)
		{
			if (!negative & (value <= 9UL))
			{
				this._writer.Write((char)(48UL + value));
				return;
			}
			int num = this.WriteNumberToBuffer(value, negative);
			this._writer.Write(this._writeBuffer, 0, num);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00012194 File Offset: 0x00010394
		private int WriteNumberToBuffer(ulong value, bool negative)
		{
			if (value <= (ulong)(-1))
			{
				return this.WriteNumberToBuffer((uint)value, negative);
			}
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength(value);
			if (negative)
			{
				num++;
				this._writeBuffer[0] = '-';
			}
			int num2 = num;
			do
			{
				ulong num3 = value / 10UL;
				ulong num4 = value - num3 * 10UL;
				this._writeBuffer[--num2] = (char)(48UL + num4);
				value = num3;
			}
			while (value != 0UL);
			return num;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00012204 File Offset: 0x00010404
		private void WriteIntegerValue(int value)
		{
			if (value >= 0 && value <= 9)
			{
				this._writer.Write((char)(48 + value));
				return;
			}
			bool flag = value < 0;
			this.WriteIntegerValue((uint)(flag ? (-(uint)value) : value), flag);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00012250 File Offset: 0x00010450
		private void WriteIntegerValue(uint value, bool negative)
		{
			if (!negative & (value <= 9U))
			{
				this._writer.Write((char)(48U + value));
				return;
			}
			int num = this.WriteNumberToBuffer(value, negative);
			this._writer.Write(this._writeBuffer, 0, num);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000122A0 File Offset: 0x000104A0
		private int WriteNumberToBuffer(uint value, bool negative)
		{
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength((ulong)value);
			if (negative)
			{
				num++;
				this._writeBuffer[0] = '-';
			}
			int num2 = num;
			do
			{
				uint num3 = value / 10U;
				uint num4 = value - num3 * 10U;
				this._writeBuffer[--num2] = (char)(48U + num4);
				value = num3;
			}
			while (value != 0U);
			return num;
		}

		// Token: 0x040001AE RID: 430
		private const int IndentCharBufferSize = 12;

		// Token: 0x040001AF RID: 431
		[Nullable(1)]
		private readonly TextWriter _writer;

		// Token: 0x040001B0 RID: 432
		private Base64Encoder _base64Encoder;

		// Token: 0x040001B1 RID: 433
		private char _indentChar;

		// Token: 0x040001B2 RID: 434
		private int _indentation;

		// Token: 0x040001B3 RID: 435
		private char _quoteChar;

		// Token: 0x040001B4 RID: 436
		private bool _quoteName;

		// Token: 0x040001B5 RID: 437
		private bool[] _charEscapeFlags;

		// Token: 0x040001B6 RID: 438
		private char[] _writeBuffer;

		// Token: 0x040001B7 RID: 439
		private IArrayPool<char> _arrayPool;

		// Token: 0x040001B8 RID: 440
		private char[] _indentChars;
	}
}
