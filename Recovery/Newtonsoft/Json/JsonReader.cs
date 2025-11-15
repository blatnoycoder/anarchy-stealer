using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200005D RID: 93
	[NullableContext(2)]
	[Nullable(0)]
	public abstract class JsonReader : IDisposable
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000B0DC File Offset: 0x000092DC
		protected JsonReader.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000B0E4 File Offset: 0x000092E4
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000B0EC File Offset: 0x000092EC
		public bool CloseInput { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000B0F8 File Offset: 0x000092F8
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000B100 File Offset: 0x00009300
		public bool SupportMultipleContent { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000B10C File Offset: 0x0000930C
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000B114 File Offset: 0x00009314
		public virtual char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			protected internal set
			{
				this._quoteChar = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000B120 File Offset: 0x00009320
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000B128 File Offset: 0x00009328
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._dateTimeZoneHandling;
			}
			set
			{
				if (value < DateTimeZoneHandling.Local || value > DateTimeZoneHandling.RoundtripKind)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000B14C File Offset: 0x0000934C
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000B154 File Offset: 0x00009354
		public DateParseHandling DateParseHandling
		{
			get
			{
				return this._dateParseHandling;
			}
			set
			{
				if (value < DateParseHandling.None || value > DateParseHandling.DateTimeOffset)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateParseHandling = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000B178 File Offset: 0x00009378
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000B180 File Offset: 0x00009380
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._floatParseHandling;
			}
			set
			{
				if (value < FloatParseHandling.Double || value > FloatParseHandling.Decimal)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatParseHandling = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000B1A4 File Offset: 0x000093A4
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000B1AC File Offset: 0x000093AC
		public string DateFormatString
		{
			get
			{
				return this._dateFormatString;
			}
			set
			{
				this._dateFormatString = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000B1B8 File Offset: 0x000093B8
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000B1C0 File Offset: 0x000093C0
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
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000B208 File Offset: 0x00009408
		public virtual JsonToken TokenType
		{
			get
			{
				return this._tokenType;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000B210 File Offset: 0x00009410
		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000B218 File Offset: 0x00009418
		public virtual Type ValueType
		{
			get
			{
				object value = this._value;
				if (value == null)
				{
					return null;
				}
				return value.GetType();
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000B230 File Offset: 0x00009430
		public virtual int Depth
		{
			get
			{
				List<JsonPosition> stack = this._stack;
				int num = ((stack != null) ? stack.Count : 0);
				if (JsonTokenUtils.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None)
				{
					return num;
				}
				return num + 1;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000B280 File Offset: 0x00009480
		[Nullable(1)]
		public virtual string Path
		{
			[NullableContext(1)]
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				JsonPosition? jsonPosition = ((this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.ConstructorStart && this._currentState != JsonReader.State.ObjectStart) ? new JsonPosition?(this._currentPosition) : null);
				return JsonPosition.BuildPath(this._stack, jsonPosition);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000B300 File Offset: 0x00009500
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000B314 File Offset: 0x00009514
		[Nullable(1)]
		public CultureInfo Culture
		{
			[NullableContext(1)]
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			[NullableContext(1)]
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000B320 File Offset: 0x00009520
		internal JsonPosition GetPosition(int depth)
		{
			if (this._stack != null && depth < this._stack.Count)
			{
				return this._stack[depth];
			}
			return this._currentPosition;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B354 File Offset: 0x00009554
		protected JsonReader()
		{
			this._currentState = JsonReader.State.Start;
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this._dateParseHandling = DateParseHandling.DateTime;
			this._floatParseHandling = FloatParseHandling.Double;
			this.CloseInput = true;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B380 File Offset: 0x00009580
		private void Push(JsonContainerType value)
		{
			this.UpdateScopeWithFinishedValue();
			if (this._currentPosition.Type == JsonContainerType.None)
			{
				this._currentPosition = new JsonPosition(value);
				return;
			}
			if (this._stack == null)
			{
				this._stack = new List<JsonPosition>();
			}
			this._stack.Add(this._currentPosition);
			this._currentPosition = new JsonPosition(value);
			if (this._maxDepth != null)
			{
				int num = this.Depth + 1;
				int? maxDepth = this._maxDepth;
				if (((num > maxDepth.GetValueOrDefault()) & (maxDepth != null)) && !this._hasExceededMaxDepth)
				{
					this._hasExceededMaxDepth = true;
					throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, this._maxDepth));
				}
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B450 File Offset: 0x00009650
		private JsonContainerType Pop()
		{
			JsonPosition jsonPosition;
			if (this._stack != null && this._stack.Count > 0)
			{
				jsonPosition = this._currentPosition;
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				jsonPosition = this._currentPosition;
				this._currentPosition = default(JsonPosition);
			}
			if (this._maxDepth != null)
			{
				int depth = this.Depth;
				int? maxDepth = this._maxDepth;
				if ((depth <= maxDepth.GetValueOrDefault()) & (maxDepth != null))
				{
					this._hasExceededMaxDepth = false;
				}
			}
			return jsonPosition.Type;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B518 File Offset: 0x00009718
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x06000214 RID: 532
		public abstract bool Read();

		// Token: 0x06000215 RID: 533 RVA: 0x0000B528 File Offset: 0x00009728
		public virtual int? ReadAsInt32()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					int num;
					if (value is int)
					{
						num = (int)value;
						return new int?(num);
					}
					if (value is BigInteger)
					{
						BigInteger bigInteger = (BigInteger)value;
						num = (int)bigInteger;
					}
					else
					{
						try
						{
							num = Convert.ToInt32(value, CultureInfo.InvariantCulture);
						}
						catch (Exception ex)
						{
							throw JsonReaderException.Create(this, "Could not convert to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, value), ex);
						}
					}
					this.SetToken(JsonToken.Integer, num, false);
					return new int?(num);
				}
				case JsonToken.String:
				{
					string text = (string)this.Value;
					return this.ReadInt32String(text);
				}
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_003A;
				}
				throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_003A:
			return null;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B63C File Offset: 0x0000983C
		internal int? ReadInt32String(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			int num;
			if (int.TryParse(s, NumberStyles.Integer, this.Culture, out num))
			{
				this.SetToken(JsonToken.Integer, num, false);
				return new int?(num);
			}
			this.SetToken(JsonToken.String, s, false);
			throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B6B8 File Offset: 0x000098B8
		public virtual string ReadAsString()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken != JsonToken.None)
				{
					if (contentToken != JsonToken.String)
					{
						goto IL_0040;
					}
					return (string)this.Value;
				}
			}
			else if (contentToken != JsonToken.Null && contentToken != JsonToken.EndArray)
			{
				goto IL_0040;
			}
			return null;
			IL_0040:
			if (JsonTokenUtils.IsPrimitiveToken(contentToken))
			{
				object value = this.Value;
				if (value != null)
				{
					IFormattable formattable = value as IFormattable;
					string text;
					if (formattable != null)
					{
						text = formattable.ToString(null, this.Culture);
					}
					else
					{
						Uri uri = value as Uri;
						text = ((uri != null) ? uri.OriginalString : value.ToString());
					}
					this.SetToken(JsonToken.String, text, false);
					return text;
				}
			}
			throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000B78C File Offset: 0x0000998C
		public virtual byte[] ReadAsBytes()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				switch (contentToken)
				{
				case JsonToken.None:
					break;
				case JsonToken.StartObject:
				{
					this.ReadIntoWrappedTypeObject();
					byte[] array = this.ReadAsBytes();
					this.ReaderReadAndAssert();
					if (this.TokenType != JsonToken.EndObject)
					{
						throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
					}
					this.SetToken(JsonToken.Bytes, array, false);
					return array;
				}
				case JsonToken.StartArray:
					return this.ReadArrayIntoByteArray();
				default:
				{
					if (contentToken != JsonToken.String)
					{
						goto IL_0130;
					}
					string text = (string)this.Value;
					byte[] array2;
					Guid guid;
					if (text.Length == 0)
					{
						array2 = CollectionUtils.ArrayEmpty<byte>();
					}
					else if (ConvertUtils.TryConvertGuid(text, out guid))
					{
						array2 = guid.ToByteArray();
					}
					else
					{
						array2 = Convert.FromBase64String(text);
					}
					this.SetToken(JsonToken.Bytes, array2, false);
					return array2;
				}
				}
			}
			else if (contentToken != JsonToken.Null && contentToken != JsonToken.EndArray)
			{
				if (contentToken != JsonToken.Bytes)
				{
					goto IL_0130;
				}
				object value = this.Value;
				if (value is Guid)
				{
					byte[] array3 = ((Guid)value).ToByteArray();
					this.SetToken(JsonToken.Bytes, array3, false);
					return array3;
				}
				return (byte[])this.Value;
			}
			return null;
			IL_0130:
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B8E8 File Offset: 0x00009AE8
		[NullableContext(1)]
		internal byte[] ReadArrayIntoByteArray()
		{
			List<byte> list = new List<byte>();
			do
			{
				if (!this.Read())
				{
					this.SetToken(JsonToken.None);
				}
			}
			while (!this.ReadArrayElementIntoByteArrayReportDone(list));
			byte[] array = list.ToArray();
			this.SetToken(JsonToken.Bytes, array, false);
			return array;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000B92C File Offset: 0x00009B2C
		[NullableContext(1)]
		private bool ReadArrayElementIntoByteArrayReportDone(List<byte> buffer)
		{
			JsonToken tokenType = this.TokenType;
			if (tokenType <= JsonToken.Comment)
			{
				if (tokenType == JsonToken.None)
				{
					throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
				}
				if (tokenType == JsonToken.Comment)
				{
					return false;
				}
			}
			else
			{
				if (tokenType == JsonToken.Integer)
				{
					buffer.Add(Convert.ToByte(this.Value, CultureInfo.InvariantCulture));
					return false;
				}
				if (tokenType == JsonToken.EndArray)
				{
					return true;
				}
			}
			throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B9BC File Offset: 0x00009BBC
		public virtual double? ReadAsDouble()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					double num;
					if (value is double)
					{
						num = (double)value;
						return new double?(num);
					}
					if (value is BigInteger)
					{
						BigInteger bigInteger = (BigInteger)value;
						num = (double)bigInteger;
					}
					else
					{
						num = Convert.ToDouble(value, CultureInfo.InvariantCulture);
					}
					this.SetToken(JsonToken.Float, num, false);
					return new double?(num);
				}
				case JsonToken.String:
					return this.ReadDoubleString((string)this.Value);
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_003A;
				}
				throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_003A:
			return null;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000BAA4 File Offset: 0x00009CA4
		internal double? ReadDoubleString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			double num;
			if (double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, this.Culture, out num))
			{
				this.SetToken(JsonToken.Float, num, false);
				return new double?(num);
			}
			this.SetToken(JsonToken.String, s, false);
			throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000BB24 File Offset: 0x00009D24
		public virtual bool? ReadAsBoolean()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					bool flag;
					if (value is BigInteger)
					{
						BigInteger bigInteger = (BigInteger)value;
						flag = bigInteger != 0L;
					}
					else
					{
						flag = Convert.ToBoolean(this.Value, CultureInfo.InvariantCulture);
					}
					this.SetToken(JsonToken.Boolean, flag, false);
					return new bool?(flag);
				}
				case JsonToken.String:
					return this.ReadBooleanString((string)this.Value);
				case JsonToken.Boolean:
					return new bool?((bool)this.Value);
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_003A;
				}
				throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_003A:
			return null;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000BC0C File Offset: 0x00009E0C
		internal bool? ReadBooleanString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			bool flag;
			if (bool.TryParse(s, out flag))
			{
				this.SetToken(JsonToken.Boolean, flag, false);
				return new bool?(flag);
			}
			this.SetToken(JsonToken.String, s, false);
			throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000BC80 File Offset: 0x00009E80
		public virtual decimal? ReadAsDecimal()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					decimal num;
					if (value is decimal)
					{
						num = (decimal)value;
						return new decimal?(num);
					}
					if (value is BigInteger)
					{
						BigInteger bigInteger = (BigInteger)value;
						num = (decimal)bigInteger;
					}
					else
					{
						try
						{
							num = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
						}
						catch (Exception ex)
						{
							throw JsonReaderException.Create(this, "Could not convert to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, value), ex);
						}
					}
					this.SetToken(JsonToken.Float, num, false);
					return new decimal?(num);
				}
				case JsonToken.String:
					return this.ReadDecimalString((string)this.Value);
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_003A;
				}
				throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_003A:
			return null;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000BD90 File Offset: 0x00009F90
		internal decimal? ReadDecimalString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			decimal num;
			if (decimal.TryParse(s, NumberStyles.Number, this.Culture, out num))
			{
				this.SetToken(JsonToken.Float, num, false);
				return new decimal?(num);
			}
			if (ConvertUtils.DecimalTryParse(s.ToCharArray(), 0, s.Length, out num) == ParseResult.Success)
			{
				this.SetToken(JsonToken.Float, num, false);
				return new decimal?(num);
			}
			this.SetToken(JsonToken.String, s, false);
			throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000BE3C File Offset: 0x0000A03C
		public virtual DateTime? ReadAsDateTime()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken != JsonToken.None)
				{
					if (contentToken != JsonToken.String)
					{
						goto IL_009A;
					}
					return this.ReadDateTimeString((string)this.Value);
				}
			}
			else if (contentToken != JsonToken.Null && contentToken != JsonToken.EndArray)
			{
				if (contentToken != JsonToken.Date)
				{
					goto IL_009A;
				}
				object value = this.Value;
				if (value is DateTimeOffset)
				{
					this.SetToken(JsonToken.Date, ((DateTimeOffset)value).DateTime, false);
				}
				return new DateTime?((DateTime)this.Value);
			}
			return null;
			IL_009A:
			throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000BF08 File Offset: 0x0000A108
		internal DateTime? ReadDateTimeString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			DateTime dateTime;
			if (DateTimeUtils.TryParseDateTime(s, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out dateTime))
			{
				dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
				this.SetToken(JsonToken.Date, dateTime, false);
				return new DateTime?(dateTime);
			}
			if (DateTime.TryParse(s, this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
				this.SetToken(JsonToken.Date, dateTime, false);
				return new DateTime?(dateTime);
			}
			throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		public virtual DateTimeOffset? ReadAsDateTimeOffset()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken != JsonToken.None)
				{
					if (contentToken != JsonToken.String)
					{
						goto IL_009D;
					}
					string text = (string)this.Value;
					return this.ReadDateTimeOffsetString(text);
				}
			}
			else if (contentToken != JsonToken.Null && contentToken != JsonToken.EndArray)
			{
				if (contentToken != JsonToken.Date)
				{
					goto IL_009D;
				}
				object value = this.Value;
				if (value is DateTime)
				{
					DateTime dateTime = (DateTime)value;
					this.SetToken(JsonToken.Date, new DateTimeOffset(dateTime), false);
				}
				return new DateTimeOffset?((DateTimeOffset)this.Value);
			}
			return null;
			IL_009D:
			throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C098 File Offset: 0x0000A298
		internal DateTimeOffset? ReadDateTimeOffsetString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			DateTimeOffset dateTimeOffset;
			if (DateTimeUtils.TryParseDateTimeOffset(s, this._dateFormatString, this.Culture, out dateTimeOffset))
			{
				this.SetToken(JsonToken.Date, dateTimeOffset, false);
				return new DateTimeOffset?(dateTimeOffset);
			}
			if (DateTimeOffset.TryParse(s, this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
			{
				this.SetToken(JsonToken.Date, dateTimeOffset, false);
				return new DateTimeOffset?(dateTimeOffset);
			}
			this.SetToken(JsonToken.String, s, false);
			throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000C148 File Offset: 0x0000A348
		internal void ReaderReadAndAssert()
		{
			if (!this.Read())
			{
				throw this.CreateUnexpectedEndException();
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C15C File Offset: 0x0000A35C
		[NullableContext(1)]
		internal JsonReaderException CreateUnexpectedEndException()
		{
			return JsonReaderException.Create(this, "Unexpected end when reading JSON.");
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C16C File Offset: 0x0000A36C
		internal void ReadIntoWrappedTypeObject()
		{
			this.ReaderReadAndAssert();
			if (this.Value != null && this.Value.ToString() == "$type")
			{
				this.ReaderReadAndAssert();
				if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
				{
					this.ReaderReadAndAssert();
					if (this.Value.ToString() == "$value")
					{
						return;
					}
				}
			}
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C210 File Offset: 0x0000A410
		public void Skip()
		{
			if (this.TokenType == JsonToken.PropertyName)
			{
				this.Read();
			}
			if (JsonTokenUtils.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C260 File Offset: 0x0000A460
		protected void SetToken(JsonToken newToken)
		{
			this.SetToken(newToken, null, true);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C26C File Offset: 0x0000A46C
		protected void SetToken(JsonToken newToken, object value)
		{
			this.SetToken(newToken, value, true);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C278 File Offset: 0x0000A478
		protected void SetToken(JsonToken newToken, object value, bool updateIndex)
		{
			this._tokenType = newToken;
			this._value = value;
			switch (newToken)
			{
			case JsonToken.StartObject:
				this._currentState = JsonReader.State.ObjectStart;
				this.Push(JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this._currentState = JsonReader.State.ArrayStart;
				this.Push(JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this._currentState = JsonReader.State.ConstructorStart;
				this.Push(JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
				this._currentState = JsonReader.State.Property;
				this._currentPosition.PropertyName = (string)value;
				return;
			case JsonToken.Comment:
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.SetPostValueState(updateIndex);
				break;
			case JsonToken.EndObject:
				this.ValidateEnd(JsonToken.EndObject);
				return;
			case JsonToken.EndArray:
				this.ValidateEnd(JsonToken.EndArray);
				return;
			case JsonToken.EndConstructor:
				this.ValidateEnd(JsonToken.EndConstructor);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C350 File Offset: 0x0000A550
		internal void SetPostValueState(bool updateIndex)
		{
			if (this.Peek() != JsonContainerType.None || this.SupportMultipleContent)
			{
				this._currentState = JsonReader.State.PostValue;
			}
			else
			{
				this.SetFinished();
			}
			if (updateIndex)
			{
				this.UpdateScopeWithFinishedValue();
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C388 File Offset: 0x0000A588
		private void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		private void ValidateEnd(JsonToken endToken)
		{
			JsonContainerType jsonContainerType = this.Pop();
			if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
			{
				throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, jsonContainerType));
			}
			if (this.Peek() != JsonContainerType.None || this.SupportMultipleContent)
			{
				this._currentState = JsonReader.State.PostValue;
				return;
			}
			this.SetFinished();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000C418 File Offset: 0x0000A618
		protected void SetStateBasedOnCurrent()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.None:
				this.SetFinished();
				return;
			case JsonContainerType.Object:
				this._currentState = JsonReader.State.Object;
				return;
			case JsonContainerType.Array:
				this._currentState = JsonReader.State.Array;
				return;
			case JsonContainerType.Constructor:
				this._currentState = JsonReader.State.Constructor;
				return;
			default:
				throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContainerType));
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C488 File Offset: 0x0000A688
		private void SetFinished()
		{
			this._currentState = (this.SupportMultipleContent ? JsonReader.State.Start : JsonReader.State.Finished);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		private JsonContainerType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				return JsonContainerType.Object;
			case JsonToken.EndArray:
				return JsonContainerType.Array;
			case JsonToken.EndConstructor:
				return JsonContainerType.Constructor;
			default:
				throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token));
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C4F4 File Offset: 0x0000A6F4
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonReader.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C510 File Offset: 0x0000A710
		public virtual void Close()
		{
			this._currentState = JsonReader.State.Closed;
			this._tokenType = JsonToken.None;
			this._value = null;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C528 File Offset: 0x0000A728
		internal void ReadAndAssert()
		{
			if (!this.Read())
			{
				throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C544 File Offset: 0x0000A744
		internal void ReadForTypeAndAssert(JsonContract contract, bool hasConverter)
		{
			if (!this.ReadForType(contract, hasConverter))
			{
				throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C560 File Offset: 0x0000A760
		internal bool ReadForType(JsonContract contract, bool hasConverter)
		{
			if (hasConverter)
			{
				return this.Read();
			}
			switch ((contract != null) ? contract.InternalReadType : ReadType.Read)
			{
			case ReadType.Read:
				return this.ReadAndMoveToContent();
			case ReadType.ReadAsInt32:
				this.ReadAsInt32();
				break;
			case ReadType.ReadAsInt64:
			{
				bool flag = this.ReadAndMoveToContent();
				if (this.TokenType == JsonToken.Undefined)
				{
					throw JsonReaderException.Create(this, "An undefined token is not a valid {0}.".FormatWith(CultureInfo.InvariantCulture, ((contract != null) ? contract.UnderlyingType : null) ?? typeof(long)));
				}
				return flag;
			}
			case ReadType.ReadAsBytes:
				this.ReadAsBytes();
				break;
			case ReadType.ReadAsString:
				this.ReadAsString();
				break;
			case ReadType.ReadAsDecimal:
				this.ReadAsDecimal();
				break;
			case ReadType.ReadAsDateTime:
				this.ReadAsDateTime();
				break;
			case ReadType.ReadAsDateTimeOffset:
				this.ReadAsDateTimeOffset();
				break;
			case ReadType.ReadAsDouble:
				this.ReadAsDouble();
				break;
			case ReadType.ReadAsBoolean:
				this.ReadAsBoolean();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return this.TokenType > JsonToken.None;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C688 File Offset: 0x0000A888
		internal bool ReadAndMoveToContent()
		{
			return this.Read() && this.MoveToContent();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
		internal bool MoveToContent()
		{
			JsonToken jsonToken = this.TokenType;
			while (jsonToken == JsonToken.None || jsonToken == JsonToken.Comment)
			{
				if (!this.Read())
				{
					return false;
				}
				jsonToken = this.TokenType;
			}
			return true;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000C6DC File Offset: 0x0000A8DC
		private JsonToken GetContentToken()
		{
			while (this.Read())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					return tokenType;
				}
			}
			this.SetToken(JsonToken.None);
			return JsonToken.None;
		}

		// Token: 0x0400012D RID: 301
		private JsonToken _tokenType;

		// Token: 0x0400012E RID: 302
		private object _value;

		// Token: 0x0400012F RID: 303
		internal char _quoteChar;

		// Token: 0x04000130 RID: 304
		internal JsonReader.State _currentState;

		// Token: 0x04000131 RID: 305
		private JsonPosition _currentPosition;

		// Token: 0x04000132 RID: 306
		private CultureInfo _culture;

		// Token: 0x04000133 RID: 307
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x04000134 RID: 308
		private int? _maxDepth;

		// Token: 0x04000135 RID: 309
		private bool _hasExceededMaxDepth;

		// Token: 0x04000136 RID: 310
		internal DateParseHandling _dateParseHandling;

		// Token: 0x04000137 RID: 311
		internal FloatParseHandling _floatParseHandling;

		// Token: 0x04000138 RID: 312
		private string _dateFormatString;

		// Token: 0x04000139 RID: 313
		private List<JsonPosition> _stack;

		// Token: 0x0200021D RID: 541
		[NullableContext(0)]
		protected internal enum State
		{
			// Token: 0x040009AA RID: 2474
			Start,
			// Token: 0x040009AB RID: 2475
			Complete,
			// Token: 0x040009AC RID: 2476
			Property,
			// Token: 0x040009AD RID: 2477
			ObjectStart,
			// Token: 0x040009AE RID: 2478
			Object,
			// Token: 0x040009AF RID: 2479
			ArrayStart,
			// Token: 0x040009B0 RID: 2480
			Array,
			// Token: 0x040009B1 RID: 2481
			Closed,
			// Token: 0x040009B2 RID: 2482
			PostValue,
			// Token: 0x040009B3 RID: 2483
			ConstructorStart,
			// Token: 0x040009B4 RID: 2484
			Constructor,
			// Token: 0x040009B5 RID: 2485
			Error,
			// Token: 0x040009B6 RID: 2486
			Finished
		}
	}
}
