using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	public static class JsonConvert
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00009EA8 File Offset: 0x000080A8
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00009EB0 File Offset: 0x000080B0
		[Nullable(new byte[] { 2, 1 })]
		public static Func<JsonSerializerSettings> DefaultSettings
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009EB8 File Offset: 0x000080B8
		public static string ToString(DateTime value)
		{
			return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009EC4 File Offset: 0x000080C4
		public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
		{
			DateTime dateTime = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);
			string text;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeString(stringWriter, dateTime, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				text = stringWriter.ToString();
			}
			return text;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00009F2C File Offset: 0x0000812C
		public static string ToString(DateTimeOffset value)
		{
			return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009F38 File Offset: 0x00008138
		public static string ToString(DateTimeOffset value, DateFormatHandling format)
		{
			string text;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				text = stringWriter.ToString();
			}
			return text;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009F98 File Offset: 0x00008198
		public static string ToString(bool value)
		{
			if (!value)
			{
				return JsonConvert.False;
			}
			return JsonConvert.True;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009FAC File Offset: 0x000081AC
		public static string ToString(char value)
		{
			return JsonConvert.ToString(char.ToString(value));
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009FBC File Offset: 0x000081BC
		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009FCC File Offset: 0x000081CC
		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009FDC File Offset: 0x000081DC
		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009FEC File Offset: 0x000081EC
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009FFC File Offset: 0x000081FC
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000A00C File Offset: 0x0000820C
		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A01C File Offset: 0x0000821C
		private static string ToStringInternal(BigInteger value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A02C File Offset: 0x0000822C
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A03C File Offset: 0x0000823C
		public static string ToString(float value)
		{
			return JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A058 File Offset: 0x00008258
		internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return JsonConvert.EnsureFloatFormat((double)value, JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A07C File Offset: 0x0000827C
		private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			if (floatFormatHandling == FloatFormatHandling.Symbol || (!double.IsInfinity(value) && !double.IsNaN(value)))
			{
				return text;
			}
			if (floatFormatHandling != FloatFormatHandling.DefaultValue)
			{
				return quoteChar.ToString() + text + quoteChar.ToString();
			}
			if (nullable)
			{
				return JsonConvert.Null;
			}
			return "0.0";
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A0DC File Offset: 0x000082DC
		public static string ToString(double value)
		{
			return JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A0F8 File Offset: 0x000082F8
		internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return JsonConvert.EnsureFloatFormat(value, JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A11C File Offset: 0x0000831C
		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1 || text.IndexOf('e') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A17C File Offset: 0x0000837C
		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A19C File Offset: 0x0000839C
		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A1AC File Offset: 0x000083AC
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A1BC File Offset: 0x000083BC
		public static string ToString(decimal value)
		{
			return JsonConvert.EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A1D0 File Offset: 0x000083D0
		public static string ToString(Guid value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A1DC File Offset: 0x000083DC
		internal static string ToString(Guid value, char quoteChar)
		{
			string text = value.ToString("D", CultureInfo.InvariantCulture);
			string text2 = quoteChar.ToString(CultureInfo.InvariantCulture);
			return text2 + text + text2;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A214 File Offset: 0x00008414
		public static string ToString(TimeSpan value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A220 File Offset: 0x00008420
		internal static string ToString(TimeSpan value, char quoteChar)
		{
			return JsonConvert.ToString(value.ToString(), quoteChar);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A238 File Offset: 0x00008438
		public static string ToString([Nullable(2)] Uri value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A254 File Offset: 0x00008454
		internal static string ToString(Uri value, char quoteChar)
		{
			return JsonConvert.ToString(value.OriginalString, quoteChar);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A264 File Offset: 0x00008464
		public static string ToString([Nullable(2)] string value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A270 File Offset: 0x00008470
		public static string ToString([Nullable(2)] string value, char delimiter)
		{
			return JsonConvert.ToString(value, delimiter, StringEscapeHandling.Default);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A27C File Offset: 0x0000847C
		public static string ToString([Nullable(2)] string value, char delimiter, StringEscapeHandling stringEscapeHandling)
		{
			if (delimiter != '"' && delimiter != '\'')
			{
				throw new ArgumentException("Delimiter must be a single or double quote.", "delimiter");
			}
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, true, stringEscapeHandling);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A2A8 File Offset: 0x000084A8
		public static string ToString([Nullable(2)] object value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			switch (ConvertUtils.GetTypeCode(value.GetType()))
			{
			case PrimitiveTypeCode.Char:
				return JsonConvert.ToString((char)value);
			case PrimitiveTypeCode.Boolean:
				return JsonConvert.ToString((bool)value);
			case PrimitiveTypeCode.SByte:
				return JsonConvert.ToString((sbyte)value);
			case PrimitiveTypeCode.Int16:
				return JsonConvert.ToString((short)value);
			case PrimitiveTypeCode.UInt16:
				return JsonConvert.ToString((ushort)value);
			case PrimitiveTypeCode.Int32:
				return JsonConvert.ToString((int)value);
			case PrimitiveTypeCode.Byte:
				return JsonConvert.ToString((byte)value);
			case PrimitiveTypeCode.UInt32:
				return JsonConvert.ToString((uint)value);
			case PrimitiveTypeCode.Int64:
				return JsonConvert.ToString((long)value);
			case PrimitiveTypeCode.UInt64:
				return JsonConvert.ToString((ulong)value);
			case PrimitiveTypeCode.Single:
				return JsonConvert.ToString((float)value);
			case PrimitiveTypeCode.Double:
				return JsonConvert.ToString((double)value);
			case PrimitiveTypeCode.DateTime:
				return JsonConvert.ToString((DateTime)value);
			case PrimitiveTypeCode.DateTimeOffset:
				return JsonConvert.ToString((DateTimeOffset)value);
			case PrimitiveTypeCode.Decimal:
				return JsonConvert.ToString((decimal)value);
			case PrimitiveTypeCode.Guid:
				return JsonConvert.ToString((Guid)value);
			case PrimitiveTypeCode.TimeSpan:
				return JsonConvert.ToString((TimeSpan)value);
			case PrimitiveTypeCode.BigInteger:
				return JsonConvert.ToStringInternal((BigInteger)value);
			case PrimitiveTypeCode.Uri:
				return JsonConvert.ToString((Uri)value);
			case PrimitiveTypeCode.String:
				return JsonConvert.ToString((string)value);
			case PrimitiveTypeCode.DBNull:
				return JsonConvert.Null;
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A490 File Offset: 0x00008690
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value)
		{
			return JsonConvert.SerializeObject(value, null, null);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000A49C File Offset: 0x0000869C
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, Formatting formatting)
		{
			return JsonConvert.SerializeObject(value, formatting, null);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A4A8 File Offset: 0x000086A8
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length == 0)
			{
				obj = null;
			}
			else
			{
				(obj = new JsonSerializerSettings()).Converters = converters;
			}
			JsonSerializerSettings jsonSerializerSettings = obj;
			return JsonConvert.SerializeObject(value, null, jsonSerializerSettings);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A4E4 File Offset: 0x000086E4
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, Formatting formatting, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length == 0)
			{
				obj = null;
			}
			else
			{
				(obj = new JsonSerializerSettings()).Converters = converters;
			}
			JsonSerializerSettings jsonSerializerSettings = obj;
			return JsonConvert.SerializeObject(value, null, formatting, jsonSerializerSettings);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000A520 File Offset: 0x00008720
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(value, null, settings);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000A52C File Offset: 0x0000872C
		[NullableContext(2)]
		[DebuggerStepThrough]
		[return: Nullable(1)]
		public static string SerializeObject(object value, Type type, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A54C File Offset: 0x0000874C
		[NullableContext(2)]
		[DebuggerStepThrough]
		[return: Nullable(1)]
		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(value, null, formatting, settings);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000A558 File Offset: 0x00008758
		[NullableContext(2)]
		[DebuggerStepThrough]
		[return: Nullable(1)]
		public static string SerializeObject(object value, Type type, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			jsonSerializer.Formatting = formatting;
			return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000A580 File Offset: 0x00008780
		private static string SerializeObjectInternal([Nullable(2)] object value, [Nullable(2)] Type type, JsonSerializer jsonSerializer)
		{
			StringWriter stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = jsonSerializer.Formatting;
				jsonSerializer.Serialize(jsonTextWriter, value, type);
			}
			return stringWriter.ToString();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000A5E8 File Offset: 0x000087E8
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value)
		{
			return JsonConvert.DeserializeObject(value, null, null);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000A5F4 File Offset: 0x000087F4
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject(value, null, settings);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000A600 File Offset: 0x00008800
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value, Type type)
		{
			return JsonConvert.DeserializeObject(value, type, null);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000A60C File Offset: 0x0000880C
		[DebuggerStepThrough]
		public static T DeserializeObject<[Nullable(2)] T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, null);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000A618 File Offset: 0x00008818
		[DebuggerStepThrough]
		public static T DeserializeAnonymousType<[Nullable(2)] T>(string value, T anonymousTypeObject)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000A620 File Offset: 0x00008820
		[DebuggerStepThrough]
		public static T DeserializeAnonymousType<[Nullable(2)] T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(value, settings);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A62C File Offset: 0x0000882C
		[DebuggerStepThrough]
		[return: MaybeNull]
		public static T DeserializeObject<[Nullable(2)] T>(string value, params JsonConverter[] converters)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), converters));
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A644 File Offset: 0x00008844
		[DebuggerStepThrough]
		[return: MaybeNull]
		public static T DeserializeObject<[Nullable(2)] T>(string value, [Nullable(2)] JsonSerializerSettings settings)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), settings));
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A65C File Offset: 0x0000885C
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length == 0)
			{
				obj = null;
			}
			else
			{
				(obj = new JsonSerializerSettings()).Converters = converters;
			}
			JsonSerializerSettings jsonSerializerSettings = obj;
			return JsonConvert.DeserializeObject(value, type, jsonSerializerSettings);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000A698 File Offset: 0x00008898
		[NullableContext(2)]
		public static object DeserializeObject([Nullable(1)] string value, Type type, JsonSerializerSettings settings)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			if (!jsonSerializer.IsCheckAdditionalContentSet())
			{
				jsonSerializer.CheckAdditionalContent = true;
			}
			object obj;
			using (JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(value)))
			{
				obj = jsonSerializer.Deserialize(jsonTextReader, type);
			}
			return obj;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A704 File Offset: 0x00008904
		[DebuggerStepThrough]
		public static void PopulateObject(string value, object target)
		{
			JsonConvert.PopulateObject(value, target, null);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000A710 File Offset: 0x00008910
		public static void PopulateObject(string value, object target, [Nullable(2)] JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(value)))
			{
				jsonSerializer.Populate(jsonReader, target);
				if (settings != null && settings.CheckAdditionalContent)
				{
					while (jsonReader.Read())
					{
						if (jsonReader.TokenType != JsonToken.Comment)
						{
							throw JsonSerializationException.Create(jsonReader, "Additional text found in JSON string after finishing deserializing object.");
						}
					}
				}
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000A790 File Offset: 0x00008990
		public static string SerializeXmlNode([Nullable(2)] XmlNode node)
		{
			return JsonConvert.SerializeXmlNode(node, Formatting.None);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000A79C File Offset: 0x0000899C
		public static string SerializeXmlNode([Nullable(2)] XmlNode node, Formatting formatting)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[] { xmlNodeConverter });
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000A7C4 File Offset: 0x000089C4
		public static string SerializeXmlNode([Nullable(2)] XmlNode node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[] { xmlNodeConverter });
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000A7F4 File Offset: 0x000089F4
		[return: Nullable(2)]
		public static XmlDocument DeserializeXmlNode(string value)
		{
			return JsonConvert.DeserializeXmlNode(value, null);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A800 File Offset: 0x00008A00
		[NullableContext(2)]
		public static XmlDocument DeserializeXmlNode([Nullable(1)] string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, false);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000A80C File Offset: 0x00008A0C
		[NullableContext(2)]
		public static XmlDocument DeserializeXmlNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute, false);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000A818 File Offset: 0x00008A18
		[NullableContext(2)]
		public static XmlDocument DeserializeXmlNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			xmlNodeConverter.EncodeSpecialCharacters = encodeSpecialCharacters;
			return (XmlDocument)JsonConvert.DeserializeObject(value, typeof(XmlDocument), new JsonConverter[] { xmlNodeConverter });
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000A864 File Offset: 0x00008A64
		public static string SerializeXNode([Nullable(2)] XObject node)
		{
			return JsonConvert.SerializeXNode(node, Formatting.None);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000A870 File Offset: 0x00008A70
		public static string SerializeXNode([Nullable(2)] XObject node, Formatting formatting)
		{
			return JsonConvert.SerializeXNode(node, formatting, false);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000A87C File Offset: 0x00008A7C
		public static string SerializeXNode([Nullable(2)] XObject node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[] { xmlNodeConverter });
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000A8AC File Offset: 0x00008AAC
		[return: Nullable(2)]
		public static XDocument DeserializeXNode(string value)
		{
			return JsonConvert.DeserializeXNode(value, null);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		[NullableContext(2)]
		public static XDocument DeserializeXNode([Nullable(1)] string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXNode(value, deserializeRootElementName, false);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000A8C4 File Offset: 0x00008AC4
		[NullableContext(2)]
		public static XDocument DeserializeXNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			return JsonConvert.DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute, false);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		[NullableContext(2)]
		public static XDocument DeserializeXNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			xmlNodeConverter.EncodeSpecialCharacters = encodeSpecialCharacters;
			return (XDocument)JsonConvert.DeserializeObject(value, typeof(XDocument), new JsonConverter[] { xmlNodeConverter });
		}

		// Token: 0x04000104 RID: 260
		public static readonly string True = "true";

		// Token: 0x04000105 RID: 261
		public static readonly string False = "false";

		// Token: 0x04000106 RID: 262
		public static readonly string Null = "null";

		// Token: 0x04000107 RID: 263
		public static readonly string Undefined = "undefined";

		// Token: 0x04000108 RID: 264
		public static readonly string PositiveInfinity = "Infinity";

		// Token: 0x04000109 RID: 265
		public static readonly string NegativeInfinity = "-Infinity";

		// Token: 0x0400010A RID: 266
		public static readonly string NaN = "NaN";
	}
}
