using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonWriter : IDisposable
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x00013BE8 File Offset: 0x00011DE8
		internal static JsonWriter.State[][] BuildStateArray()
		{
			List<JsonWriter.State[]> list = JsonWriter.StateArrayTempate.ToList<JsonWriter.State[]>();
			JsonWriter.State[] array = JsonWriter.StateArrayTempate[0];
			JsonWriter.State[] array2 = JsonWriter.StateArrayTempate[7];
			foreach (ulong num in EnumUtils.GetEnumValuesAndNames(typeof(JsonToken)).Values)
			{
				if (list.Count <= (int)num)
				{
					JsonToken jsonToken = (JsonToken)num;
					if (jsonToken - JsonToken.Integer <= 5 || jsonToken - JsonToken.Date <= 1)
					{
						list.Add(array2);
					}
					else
					{
						list.Add(array);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00013E68 File Offset: 0x00012068
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00013E70 File Offset: 0x00012070
		public bool CloseOutput { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00013E7C File Offset: 0x0001207C
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00013E84 File Offset: 0x00012084
		public bool AutoCompleteOnClose { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00013E90 File Offset: 0x00012090
		protected internal int Top
		{
			get
			{
				List<JsonPosition> stack = this._stack;
				int num = ((stack != null) ? stack.Count : 0);
				if (this.Peek() != JsonContainerType.None)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00013ECC File Offset: 0x000120CC
		public WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
				case JsonWriter.State.Start:
					return WriteState.Start;
				case JsonWriter.State.Property:
					return WriteState.Property;
				case JsonWriter.State.ObjectStart:
				case JsonWriter.State.Object:
					return WriteState.Object;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.Array:
					return WriteState.Array;
				case JsonWriter.State.ConstructorStart:
				case JsonWriter.State.Constructor:
					return WriteState.Constructor;
				case JsonWriter.State.Closed:
					return WriteState.Closed;
				case JsonWriter.State.Error:
					return WriteState.Error;
				default:
					throw JsonWriterException.Create(this, "Invalid state: " + this._currentState.ToString(), null);
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00013F48 File Offset: 0x00012148
		internal string ContainerPath
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None || this._stack == null)
				{
					return string.Empty;
				}
				return JsonPosition.BuildPath(this._stack, null);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00013F90 File Offset: 0x00012190
		public string Path
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				JsonPosition? jsonPosition = ((this._currentState != JsonWriter.State.ArrayStart && this._currentState != JsonWriter.State.ConstructorStart && this._currentState != JsonWriter.State.ObjectStart) ? new JsonPosition?(this._currentPosition) : null);
				return JsonPosition.BuildPath(this._stack, jsonPosition);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0001400C File Offset: 0x0001220C
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00014014 File Offset: 0x00012214
		public Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				if (value < Formatting.None || value > Formatting.Indented)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._formatting = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00014038 File Offset: 0x00012238
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00014040 File Offset: 0x00012240
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling;
			}
			set
			{
				if (value < DateFormatHandling.IsoDateFormat || value > DateFormatHandling.MicrosoftDateFormat)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateFormatHandling = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00014064 File Offset: 0x00012264
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0001406C File Offset: 0x0001226C
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00014090 File Offset: 0x00012290
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00014098 File Offset: 0x00012298
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling;
			}
			set
			{
				if (value < StringEscapeHandling.Default || value > StringEscapeHandling.EscapeHtml)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._stringEscapeHandling = value;
				this.OnStringEscapeHandlingChanged();
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000140C0 File Offset: 0x000122C0
		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000140C4 File Offset: 0x000122C4
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x000140CC File Offset: 0x000122CC
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling;
			}
			set
			{
				if (value < FloatFormatHandling.String || value > FloatFormatHandling.DefaultValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatFormatHandling = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x000140F0 File Offset: 0x000122F0
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x000140F8 File Offset: 0x000122F8
		[Nullable(2)]
		public string DateFormatString
		{
			[NullableContext(2)]
			get
			{
				return this._dateFormatString;
			}
			[NullableContext(2)]
			set
			{
				this._dateFormatString = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00014104 File Offset: 0x00012304
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x00014118 File Offset: 0x00012318
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014124 File Offset: 0x00012324
		protected JsonWriter()
		{
			this._currentState = JsonWriter.State.Start;
			this._formatting = Formatting.None;
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this.CloseOutput = true;
			this.AutoCompleteOnClose = true;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00014150 File Offset: 0x00012350
		internal void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00014174 File Offset: 0x00012374
		private void Push(JsonContainerType value)
		{
			if (this._currentPosition.Type != JsonContainerType.None)
			{
				if (this._stack == null)
				{
					this._stack = new List<JsonPosition>();
				}
				this._stack.Add(this._currentPosition);
			}
			this._currentPosition = new JsonPosition(value);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000141C8 File Offset: 0x000123C8
		private JsonContainerType Pop()
		{
			ref JsonPosition currentPosition = this._currentPosition;
			if (this._stack != null && this._stack.Count > 0)
			{
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				this._currentPosition = default(JsonPosition);
			}
			return currentPosition.Type;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014248 File Offset: 0x00012448
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x060003CF RID: 975
		public abstract void Flush();

		// Token: 0x060003D0 RID: 976 RVA: 0x00014258 File Offset: 0x00012458
		public virtual void Close()
		{
			if (this.AutoCompleteOnClose)
			{
				this.AutoCompleteAll();
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001426C File Offset: 0x0001246C
		public virtual void WriteStartObject()
		{
			this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00014278 File Offset: 0x00012478
		public virtual void WriteEndObject()
		{
			this.InternalWriteEnd(JsonContainerType.Object);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00014284 File Offset: 0x00012484
		public virtual void WriteStartArray()
		{
			this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00014290 File Offset: 0x00012490
		public virtual void WriteEndArray()
		{
			this.InternalWriteEnd(JsonContainerType.Array);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001429C File Offset: 0x0001249C
		public virtual void WriteStartConstructor(string name)
		{
			this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000142A8 File Offset: 0x000124A8
		public virtual void WriteEndConstructor()
		{
			this.InternalWriteEnd(JsonContainerType.Constructor);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000142B4 File Offset: 0x000124B4
		public virtual void WritePropertyName(string name)
		{
			this.InternalWritePropertyName(name);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000142C0 File Offset: 0x000124C0
		public virtual void WritePropertyName(string name, bool escape)
		{
			this.WritePropertyName(name);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000142CC File Offset: 0x000124CC
		public virtual void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000142DC File Offset: 0x000124DC
		public void WriteToken(JsonReader reader)
		{
			this.WriteToken(reader, true);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000142E8 File Offset: 0x000124E8
		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this.WriteToken(reader, writeChildren, true, true);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00014300 File Offset: 0x00012500
		[NullableContext(2)]
		public void WriteToken(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.None:
				return;
			case JsonToken.StartObject:
				this.WriteStartObject();
				return;
			case JsonToken.StartArray:
				this.WriteStartArray();
				return;
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteStartConstructor(value.ToString());
				return;
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WritePropertyName(value.ToString());
				return;
			case JsonToken.Comment:
				this.WriteComment((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Raw:
				this.WriteRawValue((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Integer:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)value;
					this.WriteValue(bigInteger);
					return;
				}
				this.WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Float:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is decimal)
				{
					decimal num = (decimal)value;
					this.WriteValue(num);
					return;
				}
				if (value is double)
				{
					double num2 = (double)value;
					this.WriteValue(num2);
					return;
				}
				if (value is float)
				{
					float num3 = (float)value;
					this.WriteValue(num3);
					return;
				}
				this.WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.String:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(value.ToString());
				return;
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Null:
				this.WriteNull();
				return;
			case JsonToken.Undefined:
				this.WriteUndefined();
				return;
			case JsonToken.EndObject:
				this.WriteEndObject();
				return;
			case JsonToken.EndArray:
				this.WriteEndArray();
				return;
			case JsonToken.EndConstructor:
				this.WriteEndConstructor();
				return;
			case JsonToken.Date:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
					this.WriteValue(dateTimeOffset);
					return;
				}
				this.WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Bytes:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is Guid)
				{
					Guid guid = (Guid)value;
					this.WriteValue(guid);
					return;
				}
				this.WriteValue((byte[])value);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00014558 File Offset: 0x00012758
		public void WriteToken(JsonToken token)
		{
			this.WriteToken(token, null);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00014564 File Offset: 0x00012764
		internal virtual void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			int num = this.CalculateWriteTokenInitialDepth(reader);
			for (;;)
			{
				if (!writeDateConstructorAsDate || reader.TokenType != JsonToken.StartConstructor)
				{
					goto IL_004E;
				}
				object value = reader.Value;
				if (!string.Equals((value != null) ? value.ToString() : null, "Date", StringComparison.Ordinal))
				{
					goto IL_004E;
				}
				this.WriteConstructorDate(reader);
				IL_0073:
				if (num - 1 >= reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) || !writeChildren || !reader.Read())
				{
					break;
				}
				continue;
				IL_004E:
				if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					this.WriteToken(reader.TokenType, reader.Value);
					goto IL_0073;
				}
				goto IL_0073;
			}
			if (this.IsWriteTokenIncomplete(reader, writeChildren, num))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00014638 File Offset: 0x00012838
		private bool IsWriteTokenIncomplete(JsonReader reader, bool writeChildren, int initialDepth)
		{
			int num = this.CalculateWriteTokenFinalDepth(reader);
			return initialDepth < num || (writeChildren && initialDepth == num && JsonTokenUtils.IsStartToken(reader.TokenType));
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014674 File Offset: 0x00012874
		private int CalculateWriteTokenInitialDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsStartToken(tokenType))
			{
				return reader.Depth + 1;
			}
			return reader.Depth;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000146B0 File Offset: 0x000128B0
		private int CalculateWriteTokenFinalDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsEndToken(tokenType))
			{
				return reader.Depth;
			}
			return reader.Depth - 1;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000146EC File Offset: 0x000128EC
		private void WriteConstructorDate(JsonReader reader)
		{
			DateTime dateTime;
			string text;
			if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out dateTime, out text))
			{
				throw JsonWriterException.Create(this, text, null);
			}
			this.WriteValue(dateTime);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001471C File Offset: 0x0001291C
		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				this.WriteEndObject();
				return;
			case JsonContainerType.Array:
				this.WriteEndArray();
				return;
			case JsonContainerType.Constructor:
				this.WriteEndConstructor();
				return;
			default:
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type.ToString(), null);
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001477C File Offset: 0x0001297C
		private void AutoCompleteAll()
		{
			while (this.Top > 0)
			{
				this.WriteEnd();
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014794 File Offset: 0x00012994
		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				return JsonToken.EndObject;
			case JsonContainerType.Array:
				return JsonToken.EndArray;
			case JsonContainerType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw JsonWriterException.Create(this, "No close token for type: " + type.ToString(), null);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000147E8 File Offset: 0x000129E8
		private void AutoCompleteClose(JsonContainerType type)
		{
			int num = this.CalculateLevelsToComplete(type);
			for (int i = 0; i < num; i++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteNull();
				}
				if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
				this.UpdateCurrentState();
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014868 File Offset: 0x00012A68
		private int CalculateLevelsToComplete(JsonContainerType type)
		{
			int num = 0;
			if (this._currentPosition.Type == type)
			{
				num = 1;
			}
			else
			{
				int num2 = this.Top - 2;
				for (int i = num2; i >= 0; i--)
				{
					int num3 = num2 - i;
					if (this._stack[num3].Type == type)
					{
						num = i + 2;
						break;
					}
				}
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			return num;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000148E4 File Offset: 0x00012AE4
		private void UpdateCurrentState()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.None:
				this._currentState = JsonWriter.State.Start;
				return;
			case JsonContainerType.Object:
				this._currentState = JsonWriter.State.Object;
				return;
			case JsonContainerType.Array:
				this._currentState = JsonWriter.State.Array;
				return;
			case JsonContainerType.Constructor:
				this._currentState = JsonWriter.State.Array;
				return;
			default:
				throw JsonWriterException.Create(this, "Unknown JsonType: " + jsonContainerType.ToString(), null);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00014958 File Offset: 0x00012B58
		protected virtual void WriteEnd(JsonToken token)
		{
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001495C File Offset: 0x00012B5C
		protected virtual void WriteIndent()
		{
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00014960 File Offset: 0x00012B60
		protected virtual void WriteValueDelimiter()
		{
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00014964 File Offset: 0x00012B64
		protected virtual void WriteIndentSpace()
		{
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00014968 File Offset: 0x00012B68
		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			JsonWriter.State state = JsonWriter.StateArray[(int)tokenBeingWritten][(int)this._currentState];
			if (state == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), this._currentState.ToString()), null);
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			if (this._formatting == Formatting.Indented)
			{
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteIndentSpace();
				}
				if (this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.Constructor || this._currentState == JsonWriter.State.ConstructorStart || (tokenBeingWritten == JsonToken.PropertyName && this._currentState != JsonWriter.State.Start))
				{
					this.WriteIndent();
				}
			}
			this._currentState = state;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014A68 File Offset: 0x00012C68
		public virtual void WriteNull()
		{
			this.InternalWriteValue(JsonToken.Null);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00014A74 File Offset: 0x00012C74
		public virtual void WriteUndefined()
		{
			this.InternalWriteValue(JsonToken.Undefined);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00014A80 File Offset: 0x00012C80
		[NullableContext(2)]
		public virtual void WriteRaw(string json)
		{
			this.InternalWriteRaw();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00014A88 File Offset: 0x00012C88
		[NullableContext(2)]
		public virtual void WriteRawValue(string json)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00014AA0 File Offset: 0x00012CA0
		[NullableContext(2)]
		public virtual void WriteValue(string value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00014AAC File Offset: 0x00012CAC
		public virtual void WriteValue(int value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00014AB8 File Offset: 0x00012CB8
		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00014AC4 File Offset: 0x00012CC4
		public virtual void WriteValue(long value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00014AD0 File Offset: 0x00012CD0
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00014ADC File Offset: 0x00012CDC
		public virtual void WriteValue(float value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00014AE8 File Offset: 0x00012CE8
		public virtual void WriteValue(double value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00014AF4 File Offset: 0x00012CF4
		public virtual void WriteValue(bool value)
		{
			this.InternalWriteValue(JsonToken.Boolean);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00014B00 File Offset: 0x00012D00
		public virtual void WriteValue(short value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00014B0C File Offset: 0x00012D0C
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00014B18 File Offset: 0x00012D18
		public virtual void WriteValue(char value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014B24 File Offset: 0x00012D24
		public virtual void WriteValue(byte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014B30 File Offset: 0x00012D30
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00014B3C File Offset: 0x00012D3C
		public virtual void WriteValue(decimal value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00014B48 File Offset: 0x00012D48
		public virtual void WriteValue(DateTime value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00014B54 File Offset: 0x00012D54
		public virtual void WriteValue(DateTimeOffset value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00014B60 File Offset: 0x00012D60
		public virtual void WriteValue(Guid value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00014B6C File Offset: 0x00012D6C
		public virtual void WriteValue(TimeSpan value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00014B78 File Offset: 0x00012D78
		public virtual void WriteValue(int? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00014B9C File Offset: 0x00012D9C
		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00014BC0 File Offset: 0x00012DC0
		public virtual void WriteValue(long? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00014BE4 File Offset: 0x00012DE4
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00014C08 File Offset: 0x00012E08
		public virtual void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00014C2C File Offset: 0x00012E2C
		public virtual void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00014C50 File Offset: 0x00012E50
		public virtual void WriteValue(bool? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00014C74 File Offset: 0x00012E74
		public virtual void WriteValue(short? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00014C98 File Offset: 0x00012E98
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00014CBC File Offset: 0x00012EBC
		public virtual void WriteValue(char? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00014CE0 File Offset: 0x00012EE0
		public virtual void WriteValue(byte? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00014D04 File Offset: 0x00012F04
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00014D28 File Offset: 0x00012F28
		public virtual void WriteValue(decimal? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00014D4C File Offset: 0x00012F4C
		public virtual void WriteValue(DateTime? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00014D70 File Offset: 0x00012F70
		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00014D94 File Offset: 0x00012F94
		public virtual void WriteValue(Guid? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00014DB8 File Offset: 0x00012FB8
		public virtual void WriteValue(TimeSpan? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00014DDC File Offset: 0x00012FDC
		[NullableContext(2)]
		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.Bytes);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00014DF4 File Offset: 0x00012FF4
		[NullableContext(2)]
		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00014E14 File Offset: 0x00013014
		[NullableContext(2)]
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is BigInteger)
			{
				throw JsonWriter.CreateUnsupportedTypeException(this, value);
			}
			JsonWriter.WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00014E48 File Offset: 0x00013048
		[NullableContext(2)]
		public virtual void WriteComment(string text)
		{
			this.InternalWriteComment();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00014E50 File Offset: 0x00013050
		public virtual void WriteWhitespace(string ws)
		{
			this.InternalWriteWhitespace(ws);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00014E5C File Offset: 0x0001305C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00014E6C File Offset: 0x0001306C
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonWriter.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00014E88 File Offset: 0x00013088
		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			for (;;)
			{
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					goto IL_00AD;
				case PrimitiveTypeCode.CharNullable:
					goto IL_00BA;
				case PrimitiveTypeCode.Boolean:
					goto IL_00E0;
				case PrimitiveTypeCode.BooleanNullable:
					goto IL_00ED;
				case PrimitiveTypeCode.SByte:
					goto IL_0113;
				case PrimitiveTypeCode.SByteNullable:
					goto IL_0120;
				case PrimitiveTypeCode.Int16:
					goto IL_0146;
				case PrimitiveTypeCode.Int16Nullable:
					goto IL_0153;
				case PrimitiveTypeCode.UInt16:
					goto IL_0179;
				case PrimitiveTypeCode.UInt16Nullable:
					goto IL_0186;
				case PrimitiveTypeCode.Int32:
					goto IL_01AD;
				case PrimitiveTypeCode.Int32Nullable:
					goto IL_01BA;
				case PrimitiveTypeCode.Byte:
					goto IL_01E1;
				case PrimitiveTypeCode.ByteNullable:
					goto IL_01EE;
				case PrimitiveTypeCode.UInt32:
					goto IL_0215;
				case PrimitiveTypeCode.UInt32Nullable:
					goto IL_0222;
				case PrimitiveTypeCode.Int64:
					goto IL_0249;
				case PrimitiveTypeCode.Int64Nullable:
					goto IL_0256;
				case PrimitiveTypeCode.UInt64:
					goto IL_027D;
				case PrimitiveTypeCode.UInt64Nullable:
					goto IL_028A;
				case PrimitiveTypeCode.Single:
					goto IL_02B1;
				case PrimitiveTypeCode.SingleNullable:
					goto IL_02BE;
				case PrimitiveTypeCode.Double:
					goto IL_02E5;
				case PrimitiveTypeCode.DoubleNullable:
					goto IL_02F2;
				case PrimitiveTypeCode.DateTime:
					goto IL_0319;
				case PrimitiveTypeCode.DateTimeNullable:
					goto IL_0326;
				case PrimitiveTypeCode.DateTimeOffset:
					goto IL_034D;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					goto IL_035A;
				case PrimitiveTypeCode.Decimal:
					goto IL_0381;
				case PrimitiveTypeCode.DecimalNullable:
					goto IL_038E;
				case PrimitiveTypeCode.Guid:
					goto IL_03B5;
				case PrimitiveTypeCode.GuidNullable:
					goto IL_03C2;
				case PrimitiveTypeCode.TimeSpan:
					goto IL_03E9;
				case PrimitiveTypeCode.TimeSpanNullable:
					goto IL_03F6;
				case PrimitiveTypeCode.BigInteger:
					goto IL_041D;
				case PrimitiveTypeCode.BigIntegerNullable:
					goto IL_042F;
				case PrimitiveTypeCode.Uri:
					goto IL_045B;
				case PrimitiveTypeCode.String:
					goto IL_0468;
				case PrimitiveTypeCode.Bytes:
					goto IL_0475;
				case PrimitiveTypeCode.DBNull:
					goto IL_0482;
				default:
				{
					IConvertible convertible = value as IConvertible;
					if (convertible == null)
					{
						goto IL_04A8;
					}
					JsonWriter.ResolveConvertibleValue(convertible, out typeCode, out value);
					break;
				}
				}
			}
			IL_00AD:
			writer.WriteValue((char)value);
			return;
			IL_00BA:
			writer.WriteValue((value == null) ? null : new char?((char)value));
			return;
			IL_00E0:
			writer.WriteValue((bool)value);
			return;
			IL_00ED:
			writer.WriteValue((value == null) ? null : new bool?((bool)value));
			return;
			IL_0113:
			writer.WriteValue((sbyte)value);
			return;
			IL_0120:
			writer.WriteValue((value == null) ? null : new sbyte?((sbyte)value));
			return;
			IL_0146:
			writer.WriteValue((short)value);
			return;
			IL_0153:
			writer.WriteValue((value == null) ? null : new short?((short)value));
			return;
			IL_0179:
			writer.WriteValue((ushort)value);
			return;
			IL_0186:
			writer.WriteValue((value == null) ? null : new ushort?((ushort)value));
			return;
			IL_01AD:
			writer.WriteValue((int)value);
			return;
			IL_01BA:
			writer.WriteValue((value == null) ? null : new int?((int)value));
			return;
			IL_01E1:
			writer.WriteValue((byte)value);
			return;
			IL_01EE:
			writer.WriteValue((value == null) ? null : new byte?((byte)value));
			return;
			IL_0215:
			writer.WriteValue((uint)value);
			return;
			IL_0222:
			writer.WriteValue((value == null) ? null : new uint?((uint)value));
			return;
			IL_0249:
			writer.WriteValue((long)value);
			return;
			IL_0256:
			writer.WriteValue((value == null) ? null : new long?((long)value));
			return;
			IL_027D:
			writer.WriteValue((ulong)value);
			return;
			IL_028A:
			writer.WriteValue((value == null) ? null : new ulong?((ulong)value));
			return;
			IL_02B1:
			writer.WriteValue((float)value);
			return;
			IL_02BE:
			writer.WriteValue((value == null) ? null : new float?((float)value));
			return;
			IL_02E5:
			writer.WriteValue((double)value);
			return;
			IL_02F2:
			writer.WriteValue((value == null) ? null : new double?((double)value));
			return;
			IL_0319:
			writer.WriteValue((DateTime)value);
			return;
			IL_0326:
			writer.WriteValue((value == null) ? null : new DateTime?((DateTime)value));
			return;
			IL_034D:
			writer.WriteValue((DateTimeOffset)value);
			return;
			IL_035A:
			writer.WriteValue((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value));
			return;
			IL_0381:
			writer.WriteValue((decimal)value);
			return;
			IL_038E:
			writer.WriteValue((value == null) ? null : new decimal?((decimal)value));
			return;
			IL_03B5:
			writer.WriteValue((Guid)value);
			return;
			IL_03C2:
			writer.WriteValue((value == null) ? null : new Guid?((Guid)value));
			return;
			IL_03E9:
			writer.WriteValue((TimeSpan)value);
			return;
			IL_03F6:
			writer.WriteValue((value == null) ? null : new TimeSpan?((TimeSpan)value));
			return;
			IL_041D:
			writer.WriteValue((BigInteger)value);
			return;
			IL_042F:
			writer.WriteValue((value == null) ? null : new BigInteger?((BigInteger)value));
			return;
			IL_045B:
			writer.WriteValue((Uri)value);
			return;
			IL_0468:
			writer.WriteValue((string)value);
			return;
			IL_0475:
			writer.WriteValue((byte[])value);
			return;
			IL_0482:
			writer.WriteNull();
			return;
			IL_04A8:
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			throw JsonWriter.CreateUnsupportedTypeException(writer, value);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00015358 File Offset: 0x00013558
		private static void ResolveConvertibleValue(IConvertible convertible, out PrimitiveTypeCode typeCode, out object value)
		{
			TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
			typeCode = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? PrimitiveTypeCode.String : typeInformation.TypeCode);
			Type type = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? typeof(string) : typeInformation.Type);
			value = convertible.ToType(type, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000153BC File Offset: 0x000135BC
		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000153DC File Offset: 0x000135DC
		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.StartObject:
				this.InternalWriteStart(token, JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this.InternalWriteStart(token, JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this.InternalWriteStart(token, JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
			{
				string text = value as string;
				if (text == null)
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				this.InternalWritePropertyName(text);
				return;
			}
			case JsonToken.Comment:
				this.InternalWriteComment();
				return;
			case JsonToken.Raw:
				this.InternalWriteRaw();
				return;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.InternalWriteValue(token);
				return;
			case JsonToken.EndObject:
				this.InternalWriteEnd(JsonContainerType.Object);
				return;
			case JsonToken.EndArray:
				this.InternalWriteEnd(JsonContainerType.Array);
				return;
			case JsonToken.EndConstructor:
				this.InternalWriteEnd(JsonContainerType.Constructor);
				return;
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000154B8 File Offset: 0x000136B8
		internal void InternalWriteEnd(JsonContainerType container)
		{
			this.AutoCompleteClose(container);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000154C4 File Offset: 0x000136C4
		internal void InternalWritePropertyName(string name)
		{
			this._currentPosition.PropertyName = name;
			this.AutoComplete(JsonToken.PropertyName);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000154DC File Offset: 0x000136DC
		internal void InternalWriteRaw()
		{
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000154E0 File Offset: 0x000136E0
		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
			this.Push(container);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000154F8 File Offset: 0x000136F8
		internal void InternalWriteValue(JsonToken token)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00015508 File Offset: 0x00013708
		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00015528 File Offset: 0x00013728
		internal void InternalWriteComment()
		{
			this.AutoComplete(JsonToken.Comment);
		}

		// Token: 0x040001D3 RID: 467
		private static readonly JsonWriter.State[][] StateArray = JsonWriter.BuildStateArray();

		// Token: 0x040001D4 RID: 468
		internal static readonly JsonWriter.State[][] StateArrayTempate = new JsonWriter.State[][]
		{
			new JsonWriter.State[]
			{
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Property,
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Object,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Array,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			}
		};

		// Token: 0x040001D5 RID: 469
		[Nullable(2)]
		private List<JsonPosition> _stack;

		// Token: 0x040001D6 RID: 470
		private JsonPosition _currentPosition;

		// Token: 0x040001D7 RID: 471
		private JsonWriter.State _currentState;

		// Token: 0x040001D8 RID: 472
		private Formatting _formatting;

		// Token: 0x040001DB RID: 475
		private DateFormatHandling _dateFormatHandling;

		// Token: 0x040001DC RID: 476
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x040001DD RID: 477
		private StringEscapeHandling _stringEscapeHandling;

		// Token: 0x040001DE RID: 478
		private FloatFormatHandling _floatFormatHandling;

		// Token: 0x040001DF RID: 479
		[Nullable(2)]
		private string _dateFormatString;

		// Token: 0x040001E0 RID: 480
		[Nullable(2)]
		private CultureInfo _culture;

		// Token: 0x02000221 RID: 545
		[NullableContext(0)]
		internal enum State
		{
			// Token: 0x040009C8 RID: 2504
			Start,
			// Token: 0x040009C9 RID: 2505
			Property,
			// Token: 0x040009CA RID: 2506
			ObjectStart,
			// Token: 0x040009CB RID: 2507
			Object,
			// Token: 0x040009CC RID: 2508
			ArrayStart,
			// Token: 0x040009CD RID: 2509
			Array,
			// Token: 0x040009CE RID: 2510
			ConstructorStart,
			// Token: 0x040009CF RID: 2511
			Constructor,
			// Token: 0x040009D0 RID: 2512
			Closed,
			// Token: 0x040009D1 RID: 2513
			Error
		}
	}
}
