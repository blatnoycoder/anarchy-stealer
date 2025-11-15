using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000FE RID: 254
	[NullableContext(1)]
	[Nullable(0)]
	public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>, IConvertible
	{
		// Token: 0x06000B7A RID: 2938 RVA: 0x00035AC8 File Offset: 0x00033CC8
		[NullableContext(2)]
		internal JValue(object value, JTokenType type)
		{
			this._value = value;
			this._valueType = type;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00035AE0 File Offset: 0x00033CE0
		public JValue(JValue other)
			: this(other.Value, other.Type)
		{
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00035AF4 File Offset: 0x00033CF4
		public JValue(long value)
			: this(value, JTokenType.Integer)
		{
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00035B04 File Offset: 0x00033D04
		public JValue(decimal value)
			: this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00035B14 File Offset: 0x00033D14
		public JValue(char value)
			: this(value, JTokenType.String)
		{
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00035B24 File Offset: 0x00033D24
		[CLSCompliant(false)]
		public JValue(ulong value)
			: this(value, JTokenType.Integer)
		{
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00035B34 File Offset: 0x00033D34
		public JValue(double value)
			: this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00035B44 File Offset: 0x00033D44
		public JValue(float value)
			: this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00035B54 File Offset: 0x00033D54
		public JValue(DateTime value)
			: this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00035B64 File Offset: 0x00033D64
		public JValue(DateTimeOffset value)
			: this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00035B74 File Offset: 0x00033D74
		public JValue(bool value)
			: this(value, JTokenType.Boolean)
		{
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00035B84 File Offset: 0x00033D84
		[NullableContext(2)]
		public JValue(string value)
			: this(value, JTokenType.String)
		{
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00035B90 File Offset: 0x00033D90
		public JValue(Guid value)
			: this(value, JTokenType.Guid)
		{
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00035BA0 File Offset: 0x00033DA0
		[NullableContext(2)]
		public JValue(Uri value)
			: this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
		{
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00035BC0 File Offset: 0x00033DC0
		public JValue(TimeSpan value)
			: this(value, JTokenType.TimeSpan)
		{
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00035BD0 File Offset: 0x00033DD0
		[NullableContext(2)]
		public JValue(object value)
			: this(value, JValue.GetValueType(null, value))
		{
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00035BF8 File Offset: 0x00033DF8
		internal override bool DeepEquals(JToken node)
		{
			JValue jvalue = node as JValue;
			return jvalue != null && (jvalue == this || JValue.ValuesEquals(this, jvalue));
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00035C28 File Offset: 0x00033E28
		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00035C2C File Offset: 0x00033E2C
		private static int CompareBigInteger(BigInteger i1, object i2)
		{
			int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
			if (num != 0)
			{
				return num;
			}
			if (i2 is decimal)
			{
				decimal num2 = (decimal)i2;
				return 0m.CompareTo(Math.Abs(num2 - Math.Truncate(num2)));
			}
			if (i2 is double || i2 is float)
			{
				double num3 = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
				return 0.0.CompareTo(Math.Abs(num3 - Math.Truncate(num3)));
			}
			return num;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00035CC8 File Offset: 0x00033EC8
		[NullableContext(2)]
		internal static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == objB)
			{
				return 0;
			}
			if (objB == null)
			{
				return 1;
			}
			if (objA == null)
			{
				return -1;
			}
			switch (valueType)
			{
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Raw:
			{
				string text = Convert.ToString(objA, CultureInfo.InvariantCulture);
				string text2 = Convert.ToString(objB, CultureInfo.InvariantCulture);
				return string.CompareOrdinal(text, text2);
			}
			case JTokenType.Integer:
				if (objA is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)objA;
					return JValue.CompareBigInteger(bigInteger, objB);
				}
				if (objB is BigInteger)
				{
					BigInteger bigInteger2 = (BigInteger)objB;
					return -JValue.CompareBigInteger(bigInteger2, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				if (objA is float || objB is float || objA is double || objB is double)
				{
					return JValue.CompareFloat(objA, objB);
				}
				return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
			case JTokenType.Float:
				if (objA is BigInteger)
				{
					BigInteger bigInteger3 = (BigInteger)objA;
					return JValue.CompareBigInteger(bigInteger3, objB);
				}
				if (objB is BigInteger)
				{
					BigInteger bigInteger4 = (BigInteger)objB;
					return -JValue.CompareBigInteger(bigInteger4, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				return JValue.CompareFloat(objA, objB);
			case JTokenType.Boolean:
			{
				bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
				bool flag2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
				return flag.CompareTo(flag2);
			}
			case JTokenType.Date:
			{
				if (objA is DateTime)
				{
					DateTime dateTime = (DateTime)objA;
					DateTime dateTime2;
					if (objB is DateTimeOffset)
					{
						dateTime2 = ((DateTimeOffset)objB).DateTime;
					}
					else
					{
						dateTime2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
					}
					return dateTime.CompareTo(dateTime2);
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)objA;
				DateTimeOffset dateTimeOffset2;
				if (objB is DateTimeOffset)
				{
					dateTimeOffset2 = (DateTimeOffset)objB;
				}
				else
				{
					dateTimeOffset2 = new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture));
				}
				return dateTimeOffset.CompareTo(dateTimeOffset2);
			}
			case JTokenType.Bytes:
			{
				byte[] array = objB as byte[];
				if (array == null)
				{
					throw new ArgumentException("Object must be of type byte[].");
				}
				return MiscellaneousUtils.ByteArrayCompare(objA as byte[], array);
			}
			case JTokenType.Guid:
			{
				if (!(objB is Guid))
				{
					throw new ArgumentException("Object must be of type Guid.");
				}
				Guid guid = (Guid)objA;
				Guid guid2 = (Guid)objB;
				return guid.CompareTo(guid2);
			}
			case JTokenType.Uri:
			{
				Uri uri = objB as Uri;
				if (uri == null)
				{
					throw new ArgumentException("Object must be of type Uri.");
				}
				Uri uri2 = (Uri)objA;
				return Comparer<string>.Default.Compare(uri2.ToString(), uri.ToString());
			}
			case JTokenType.TimeSpan:
			{
				if (!(objB is TimeSpan))
				{
					throw new ArgumentException("Object must be of type TimeSpan.");
				}
				TimeSpan timeSpan = (TimeSpan)objA;
				TimeSpan timeSpan2 = (TimeSpan)objB;
				return timeSpan.CompareTo(timeSpan2);
			}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0003604C File Offset: 0x0003424C
		private static int CompareFloat(object objA, object objB)
		{
			double num = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(num, num2))
			{
				return 0;
			}
			return num.CompareTo(num2);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0003608C File Offset: 0x0003428C
		[NullableContext(2)]
		private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
		{
			if ((objA is string || objB is string) && (operation == ExpressionType.Add || operation == ExpressionType.AddAssign))
			{
				result = ((objA != null) ? objA.ToString() : null) + ((objB != null) ? objB.ToString() : null);
				return true;
			}
			if (objA is BigInteger || objB is BigInteger)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				BigInteger bigInteger = ConvertUtils.ToBigInteger(objA);
				BigInteger bigInteger2 = ConvertUtils.ToBigInteger(objB);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_048F;
							}
							goto IL_0120;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_0110;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_048F;
						}
						goto IL_0100;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_048F;
						}
						goto IL_0120;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_0110;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_048F;
					}
					goto IL_0100;
				}
				result = bigInteger + bigInteger2;
				return true;
				IL_0100:
				result = bigInteger - bigInteger2;
				return true;
				IL_0110:
				result = bigInteger * bigInteger2;
				return true;
				IL_0120:
				result = bigInteger / bigInteger2;
				return true;
			}
			else if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				decimal num = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
				decimal num2 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_048F;
							}
							goto IL_021F;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_020F;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_048F;
						}
						goto IL_01FF;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_048F;
						}
						goto IL_021F;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_020F;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_048F;
					}
					goto IL_01FF;
				}
				result = num + num2;
				return true;
				IL_01FF:
				result = num - num2;
				return true;
				IL_020F:
				result = num * num2;
				return true;
				IL_021F:
				result = num / num2;
				return true;
			}
			else if (objA is float || objB is float || objA is double || objB is double)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				double num3 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
				double num4 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_048F;
							}
							goto IL_031A;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_030C;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_048F;
						}
						goto IL_02FE;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_048F;
						}
						goto IL_031A;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_030C;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_048F;
					}
					goto IL_02FE;
				}
				result = num3 + num4;
				return true;
				IL_02FE:
				result = num3 - num4;
				return true;
				IL_030C:
				result = num3 * num4;
				return true;
				IL_031A:
				result = num3 / num4;
				return true;
			}
			else if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte || objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				long num5 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
				long num6 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_048F;
							}
							goto IL_0481;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_0473;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_048F;
						}
						goto IL_0465;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_048F;
						}
						goto IL_0481;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_0473;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_048F;
					}
					goto IL_0465;
				}
				result = num5 + num6;
				return true;
				IL_0465:
				result = num5 - num6;
				return true;
				IL_0473:
				result = num5 * num6;
				return true;
				IL_0481:
				result = num5 / num6;
				return true;
			}
			IL_048F:
			result = null;
			return false;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00036530 File Offset: 0x00034730
		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00036538 File Offset: 0x00034738
		public static JValue CreateComment([Nullable(2)] string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00036544 File Offset: 0x00034744
		public static JValue CreateString([Nullable(2)] string value)
		{
			return new JValue(value, JTokenType.String);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00036550 File Offset: 0x00034750
		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0003655C File Offset: 0x0003475C
		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00036568 File Offset: 0x00034768
		[NullableContext(2)]
		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is BigInteger)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (value is TimeSpan)
			{
				return JTokenType.TimeSpan;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000366B8 File Offset: 0x000348B8
		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null)
			{
				return JTokenType.String;
			}
			JTokenType valueOrDefault = current.GetValueOrDefault();
			if (valueOrDefault == JTokenType.Comment || valueOrDefault == JTokenType.String || valueOrDefault == JTokenType.Raw)
			{
				return current.GetValueOrDefault();
			}
			return JTokenType.String;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00036700 File Offset: 0x00034900
		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00036708 File Offset: 0x00034908
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00036710 File Offset: 0x00034910
		[Nullable(2)]
		public new object Value
		{
			[NullableContext(2)]
			get
			{
				return this._value;
			}
			[NullableContext(2)]
			set
			{
				object value2 = this._value;
				Type type = ((value2 != null) ? value2.GetType() : null);
				Type type2 = ((value != null) ? value.GetType() : null);
				if (type != type2)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00036778 File Offset: 0x00034978
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			if (converters != null && converters.Length != 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return;
				}
			}
			switch (this._valueType)
			{
			case JTokenType.Comment:
			{
				object value = this._value;
				writer.WriteComment((value != null) ? value.ToString() : null);
				return;
			}
			case JTokenType.Integer:
			{
				object obj = this._value;
				if (obj is int)
				{
					int num = (int)obj;
					writer.WriteValue(num);
					return;
				}
				obj = this._value;
				if (obj is long)
				{
					long num2 = (long)obj;
					writer.WriteValue(num2);
					return;
				}
				obj = this._value;
				if (obj is ulong)
				{
					ulong num3 = (ulong)obj;
					writer.WriteValue(num3);
					return;
				}
				obj = this._value;
				if (obj is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)obj;
					writer.WriteValue(bigInteger);
					return;
				}
				writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.Float:
			{
				object obj = this._value;
				if (obj is decimal)
				{
					decimal num4 = (decimal)obj;
					writer.WriteValue(num4);
					return;
				}
				obj = this._value;
				if (obj is double)
				{
					double num5 = (double)obj;
					writer.WriteValue(num5);
					return;
				}
				obj = this._value;
				if (obj is float)
				{
					float num6 = (float)obj;
					writer.WriteValue(num6);
					return;
				}
				writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.String:
			{
				object value2 = this._value;
				writer.WriteValue((value2 != null) ? value2.ToString() : null);
				return;
			}
			case JTokenType.Boolean:
				writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Null:
				writer.WriteNull();
				return;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			case JTokenType.Date:
			{
				object obj = this._value;
				if (obj is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)obj;
					writer.WriteValue(dateTimeOffset);
					return;
				}
				writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.Raw:
			{
				object value3 = this._value;
				writer.WriteRawValue((value3 != null) ? value3.ToString() : null);
				return;
			}
			case JTokenType.Bytes:
				writer.WriteValue((byte[])this._value);
				return;
			case JTokenType.Guid:
				writer.WriteValue((this._value != null) ? ((Guid?)this._value) : null);
				return;
			case JTokenType.Uri:
				writer.WriteValue((Uri)this._value);
				return;
			case JTokenType.TimeSpan:
				writer.WriteValue((this._value != null) ? ((TimeSpan?)this._value) : null);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", this._valueType, "Unexpected token type.");
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00036A98 File Offset: 0x00034C98
		internal override int GetDeepHashCode()
		{
			int num = ((this._value != null) ? this._value.GetHashCode() : 0);
			int valueType = (int)this._valueType;
			return valueType.GetHashCode() ^ num;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00036AD8 File Offset: 0x00034CD8
		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			return v1 == v2 || (v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00036B10 File Offset: 0x00034D10
		public bool Equals([AllowNull] JValue other)
		{
			return other != null && JValue.ValuesEquals(this, other);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00036B24 File Offset: 0x00034D24
		public override bool Equals(object obj)
		{
			JValue jvalue = obj as JValue;
			return jvalue != null && this.Equals(jvalue);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00036B4C File Offset: 0x00034D4C
		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00036B68 File Offset: 0x00034D68
		public override string ToString()
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			return this._value.ToString();
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00036B88 File Offset: 0x00034D88
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00036B98 File Offset: 0x00034D98
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00036BA4 File Offset: 0x00034DA4
		public string ToString([Nullable(2)] string format, IFormatProvider formatProvider)
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			IFormattable formattable = this._value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return this._value.ToString();
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00036BEC File Offset: 0x00034DEC
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JValue>(parameter, this, new JValue.JValueDynamicProxy());
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00036BFC File Offset: 0x00034DFC
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			JValue jvalue = obj as JValue;
			object obj2;
			JTokenType jtokenType;
			if (jvalue != null)
			{
				obj2 = jvalue.Value;
				jtokenType = ((this._valueType == JTokenType.String && this._valueType != jvalue._valueType) ? jvalue._valueType : this._valueType);
			}
			else
			{
				obj2 = obj;
				jtokenType = this._valueType;
			}
			return JValue.Compare(jtokenType, this._value, obj2);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00036C74 File Offset: 0x00034E74
		public int CompareTo(JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return JValue.Compare((this._valueType == JTokenType.String && this._valueType != obj._valueType) ? obj._valueType : this._valueType, this._value, obj._value);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00036CCC File Offset: 0x00034ECC
		TypeCode IConvertible.GetTypeCode()
		{
			if (this._value == null)
			{
				return TypeCode.Empty;
			}
			IConvertible convertible = this._value as IConvertible;
			if (convertible != null)
			{
				return convertible.GetTypeCode();
			}
			return TypeCode.Object;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00036D04 File Offset: 0x00034F04
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return (bool)this;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00036D0C File Offset: 0x00034F0C
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return (char)this;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00036D14 File Offset: 0x00034F14
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)this;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00036D1C File Offset: 0x00034F1C
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return (byte)this;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00036D24 File Offset: 0x00034F24
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return (short)this;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00036D2C File Offset: 0x00034F2C
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return (ushort)this;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00036D34 File Offset: 0x00034F34
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00036D3C File Offset: 0x00034F3C
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return (uint)this;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00036D44 File Offset: 0x00034F44
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return (long)this;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00036D4C File Offset: 0x00034F4C
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return (ulong)this;
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00036D54 File Offset: 0x00034F54
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return (float)this;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00036D60 File Offset: 0x00034F60
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return (double)this;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00036D6C File Offset: 0x00034F6C
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return (decimal)this;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00036D74 File Offset: 0x00034F74
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return (DateTime)this;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00036D7C File Offset: 0x00034F7C
		[return: Nullable(2)]
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return base.ToObject(conversionType);
		}

		// Token: 0x04000467 RID: 1127
		private JTokenType _valueType;

		// Token: 0x04000468 RID: 1128
		[Nullable(2)]
		private object _value;

		// Token: 0x02000282 RID: 642
		[Nullable(new byte[] { 0, 1 })]
		private class JValueDynamicProxy : DynamicProxy<JValue>
		{
			// Token: 0x06001816 RID: 6166 RVA: 0x00067E70 File Offset: 0x00066070
			public override bool TryConvert(JValue instance, ConvertBinder binder, [Nullable(2)] [NotNullWhen(true)] out object result)
			{
				if (binder.Type == typeof(JValue) || binder.Type == typeof(JToken))
				{
					result = instance;
					return true;
				}
				object value = instance.Value;
				if (value == null)
				{
					result = null;
					return ReflectionUtils.IsNullable(binder.Type);
				}
				result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
				return true;
			}

			// Token: 0x06001817 RID: 6167 RVA: 0x00067EEC File Offset: 0x000660EC
			public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, [Nullable(2)] [NotNullWhen(true)] out object result)
			{
				JValue jvalue = arg as JValue;
				object obj = ((jvalue != null) ? jvalue.Value : arg);
				ExpressionType operation = binder.Operation;
				if (operation <= ExpressionType.NotEqual)
				{
					if (operation <= ExpressionType.LessThanOrEqual)
					{
						if (operation != ExpressionType.Add)
						{
							switch (operation)
							{
							case ExpressionType.Divide:
								break;
							case ExpressionType.Equal:
								result = JValue.Compare(instance.Type, instance.Value, obj) == 0;
								return true;
							case ExpressionType.ExclusiveOr:
							case ExpressionType.Invoke:
							case ExpressionType.Lambda:
							case ExpressionType.LeftShift:
								goto IL_01A2;
							case ExpressionType.GreaterThan:
								result = JValue.Compare(instance.Type, instance.Value, obj) > 0;
								return true;
							case ExpressionType.GreaterThanOrEqual:
								result = JValue.Compare(instance.Type, instance.Value, obj) >= 0;
								return true;
							case ExpressionType.LessThan:
								result = JValue.Compare(instance.Type, instance.Value, obj) < 0;
								return true;
							case ExpressionType.LessThanOrEqual:
								result = JValue.Compare(instance.Type, instance.Value, obj) <= 0;
								return true;
							default:
								goto IL_01A2;
							}
						}
					}
					else if (operation != ExpressionType.Multiply)
					{
						if (operation != ExpressionType.NotEqual)
						{
							goto IL_01A2;
						}
						result = JValue.Compare(instance.Type, instance.Value, obj) != 0;
						return true;
					}
				}
				else if (operation <= ExpressionType.AddAssign)
				{
					if (operation != ExpressionType.Subtract && operation != ExpressionType.AddAssign)
					{
						goto IL_01A2;
					}
				}
				else if (operation != ExpressionType.DivideAssign && operation != ExpressionType.MultiplyAssign && operation != ExpressionType.SubtractAssign)
				{
					goto IL_01A2;
				}
				if (JValue.Operation(binder.Operation, instance.Value, obj, out result))
				{
					result = new JValue(result);
					return true;
				}
				IL_01A2:
				result = null;
				return false;
			}
		}
	}
}
