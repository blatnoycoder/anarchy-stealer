using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C2 RID: 194
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x060006E1 RID: 1761 RVA: 0x00023220 File Offset: 0x00021420
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, [Nullable(2)] JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00023254 File Offset: 0x00021454
		private T GetTokenValue<[Nullable(2)] T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			return (T)((object)global::System.Convert.ChangeType(((JValue)value).Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00023288 File Offset: 0x00021488
		[return: Nullable(2)]
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(jtoken, type, this._contract, this._member);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000232DC File Offset: 0x000214DC
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue = value as JValue;
			return global::System.Convert.ChangeType((jvalue != null) ? jvalue.Value : value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002331C File Offset: 0x0002151C
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00023328 File Offset: 0x00021528
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00023334 File Offset: 0x00021534
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00023340 File Offset: 0x00021540
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002334C File Offset: 0x0002154C
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00023358 File Offset: 0x00021558
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00023364 File Offset: 0x00021564
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00023370 File Offset: 0x00021570
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002337C File Offset: 0x0002157C
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00023388 File Offset: 0x00021588
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00023394 File Offset: 0x00021594
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000233A0 File Offset: 0x000215A0
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000233AC File Offset: 0x000215AC
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000233B8 File Offset: 0x000215B8
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000233C4 File Offset: 0x000215C4
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x0400033C RID: 828
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x0400033D RID: 829
		private readonly JsonISerializableContract _contract;

		// Token: 0x0400033E RID: 830
		[Nullable(2)]
		private readonly JsonProperty _member;
	}
}
