using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.SQLite
{
	// Token: 0x0200014B RID: 331
	public abstract class SQLiteConvert
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0003FAE8 File Offset: 0x0003DCE8
		internal SQLiteConvert(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString)
		{
			this._datetimeFormat = fmt;
			this._datetimeKind = kind;
			this._datetimeFormatString = fmtString;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0003FB08 File Offset: 0x0003DD08
		public static byte[] ToUTF8(string sourceText)
		{
			if (sourceText == null)
			{
				return null;
			}
			int num = SQLiteConvert._utf8.GetByteCount(sourceText) + 1;
			byte[] array = new byte[num];
			num = SQLiteConvert._utf8.GetBytes(sourceText, 0, sourceText.Length, array, 0);
			array[num] = 0;
			return array;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0003FB50 File Offset: 0x0003DD50
		public byte[] ToUTF8(DateTime dateTimeValue)
		{
			return SQLiteConvert.ToUTF8(this.ToString(dateTimeValue));
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0003FB60 File Offset: 0x0003DD60
		public virtual string ToString(IntPtr nativestring, int nativestringlen)
		{
			return SQLiteConvert.UTF8ToString(nativestring, nativestringlen);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0003FB6C File Offset: 0x0003DD6C
		public static string UTF8ToString(IntPtr nativestring, int nativestringlen)
		{
			if (nativestring == IntPtr.Zero || nativestringlen == 0)
			{
				return string.Empty;
			}
			if (nativestringlen < 0)
			{
				nativestringlen = 0;
				while (Marshal.ReadByte(nativestring, nativestringlen) != 0)
				{
					nativestringlen++;
				}
				if (nativestringlen == 0)
				{
					return string.Empty;
				}
			}
			byte[] array = new byte[nativestringlen];
			Marshal.Copy(nativestring, array, 0, nativestringlen);
			return SQLiteConvert._utf8.GetString(array, 0, nativestringlen);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0003FBE0 File Offset: 0x0003DDE0
		private static bool isValidJd(long jd)
		{
			return jd >= SQLiteConvert.MinimumJd && jd <= SQLiteConvert.MaximumJd;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0003FBFC File Offset: 0x0003DDFC
		private static long DoubleToJd(double julianDay)
		{
			return (long)Math.Round(julianDay * 86400000.0);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0003FC10 File Offset: 0x0003DE10
		private static double JdToDouble(long jd)
		{
			return (double)jd / 86400000.0;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0003FC20 File Offset: 0x0003DE20
		private static DateTime computeYMD(long jd, DateTime? badValue)
		{
			if (SQLiteConvert.isValidJd(jd))
			{
				int num = (int)((jd + 43200000L) / 86400000L);
				int num2 = (int)(((double)num - 1867216.25) / 36524.25);
				num2 = num + 1 + num2 - num2 / 4;
				int num3 = num2 + 1524;
				int num4 = (int)(((double)num3 - 122.1) / 365.25);
				int num5 = 36525 * num4 / 100;
				int num6 = (int)((double)(num3 - num5) / 30.6001);
				int num7 = (int)(30.6001 * (double)num6);
				int num8 = num3 - num5 - num7;
				int num9 = ((num6 < 14) ? (num6 - 1) : (num6 - 13));
				int num10 = ((num9 > 2) ? (num4 - 4716) : (num4 - 4715));
				DateTime dateTime;
				try
				{
					dateTime = new DateTime(num10, num9, num8);
				}
				catch
				{
					if (badValue == null)
					{
						throw;
					}
					dateTime = badValue.Value;
				}
				return dateTime;
			}
			if (badValue == null)
			{
				throw new ArgumentException("Not a supported Julian Day value.");
			}
			return badValue.Value;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0003FD58 File Offset: 0x0003DF58
		private static DateTime computeHMS(long jd, DateTime? badValue)
		{
			if (SQLiteConvert.isValidJd(jd))
			{
				int num = (int)((jd + 43200000L) % 86400000L);
				decimal num2 = num / 1000.0m;
				num = (int)num2;
				int num3 = (int)((num2 - num) * 1000.0m);
				num2 -= num;
				int num4 = num / 3600;
				num -= num4 * 3600;
				int num5 = num / 60;
				num2 += num - num5 * 60;
				int num6 = (int)num2;
				DateTime dateTime;
				try
				{
					DateTime minValue = DateTime.MinValue;
					dateTime = new DateTime(minValue.Year, minValue.Month, minValue.Day, num4, num5, num6, num3);
				}
				catch
				{
					if (badValue == null)
					{
						throw;
					}
					dateTime = badValue.Value;
				}
				return dateTime;
			}
			if (badValue == null)
			{
				throw new ArgumentException("Not a supported Julian Day value.");
			}
			return badValue.Value;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0003FE84 File Offset: 0x0003E084
		private static long computeJD(DateTime dateTime)
		{
			int num = dateTime.Year;
			int num2 = dateTime.Month;
			int day = dateTime.Day;
			if (num2 <= 2)
			{
				num--;
				num2 += 12;
			}
			int num3 = num / 100;
			int num4 = 2 - num3 + num3 / 4;
			int num5 = 36525 * (num + 4716) / 100;
			int num6 = 306001 * (num2 + 1) / 10000;
			long num7 = (long)(((double)(num5 + num6 + day + num4) - 1524.5) * 86400000.0);
			return num7 + (long)(dateTime.Hour * 3600000 + dateTime.Minute * 60000 + dateTime.Second * 1000 + dateTime.Millisecond);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0003FF4C File Offset: 0x0003E14C
		public DateTime ToDateTime(string dateText)
		{
			return SQLiteConvert.ToDateTime(dateText, this._datetimeFormat, this._datetimeKind, this._datetimeFormatString);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0003FF68 File Offset: 0x0003E168
		public static DateTime ToDateTime(string dateText, SQLiteDateFormats format, DateTimeKind kind, string formatString)
		{
			switch (format)
			{
			case SQLiteDateFormats.Ticks:
				return SQLiteConvert.TicksToDateTime(Convert.ToInt64(dateText, CultureInfo.InvariantCulture), kind);
			case SQLiteDateFormats.JulianDay:
				return SQLiteConvert.ToDateTime(Convert.ToDouble(dateText, CultureInfo.InvariantCulture), kind);
			case SQLiteDateFormats.UnixEpoch:
				return SQLiteConvert.UnixEpochToDateTime(Convert.ToInt64(dateText, CultureInfo.InvariantCulture), kind);
			case SQLiteDateFormats.InvariantCulture:
				if (formatString != null)
				{
					return DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, DateTimeFormatInfo.InvariantInfo, (kind == DateTimeKind.Utc) ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
				}
				return DateTime.SpecifyKind(DateTime.Parse(dateText, DateTimeFormatInfo.InvariantInfo, (kind == DateTimeKind.Utc) ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
			case SQLiteDateFormats.CurrentCulture:
				if (formatString != null)
				{
					return DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, DateTimeFormatInfo.CurrentInfo, (kind == DateTimeKind.Utc) ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
				}
				return DateTime.SpecifyKind(DateTime.Parse(dateText, DateTimeFormatInfo.CurrentInfo, (kind == DateTimeKind.Utc) ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
			}
			if (formatString != null)
			{
				return DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, DateTimeFormatInfo.InvariantInfo, (kind == DateTimeKind.Utc) ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
			}
			return DateTime.SpecifyKind(DateTime.ParseExact(dateText, SQLiteConvert._datetimeFormats, DateTimeFormatInfo.InvariantInfo, (kind == DateTimeKind.Utc) ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000400B4 File Offset: 0x0003E2B4
		public DateTime ToDateTime(double julianDay)
		{
			return SQLiteConvert.ToDateTime(julianDay, this._datetimeKind);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000400C4 File Offset: 0x0003E2C4
		public static DateTime ToDateTime(double julianDay, DateTimeKind kind)
		{
			long num = SQLiteConvert.DoubleToJd(julianDay);
			DateTime dateTime = SQLiteConvert.computeYMD(num, null);
			DateTime dateTime2 = SQLiteConvert.computeHMS(num, null);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime2.Hour, dateTime2.Minute, dateTime2.Second, dateTime2.Millisecond, kind);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00040134 File Offset: 0x0003E334
		internal static DateTime UnixEpochToDateTime(long seconds, DateTimeKind kind)
		{
			return DateTime.SpecifyKind(SQLiteConvert.UnixEpoch.AddSeconds((double)seconds), kind);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0004015C File Offset: 0x0003E35C
		internal static DateTime TicksToDateTime(long ticks, DateTimeKind kind)
		{
			return new DateTime(ticks, kind);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00040168 File Offset: 0x0003E368
		public static double ToJulianDay(DateTime value)
		{
			return SQLiteConvert.JdToDouble(SQLiteConvert.computeJD(value));
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00040178 File Offset: 0x0003E378
		public static long ToUnixEpoch(DateTime value)
		{
			return value.Subtract(SQLiteConvert.UnixEpoch).Ticks / 10000000L;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x000401A4 File Offset: 0x0003E3A4
		private static string GetDateTimeKindFormat(DateTimeKind kind, string formatString)
		{
			if (formatString != null)
			{
				return formatString;
			}
			if (kind != DateTimeKind.Utc)
			{
				return SQLiteConvert._datetimeFormatLocal;
			}
			return SQLiteConvert._datetimeFormatUtc;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000401C0 File Offset: 0x0003E3C0
		public string ToString(DateTime dateValue)
		{
			return SQLiteConvert.ToString(dateValue, this._datetimeFormat, this._datetimeKind, this._datetimeFormatString);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000401DC File Offset: 0x0003E3DC
		public static string ToString(DateTime dateValue, SQLiteDateFormats format, DateTimeKind kind, string formatString)
		{
			switch (format)
			{
			case SQLiteDateFormats.Ticks:
				return dateValue.Ticks.ToString(CultureInfo.InvariantCulture);
			case SQLiteDateFormats.JulianDay:
				return SQLiteConvert.ToJulianDay(dateValue).ToString(CultureInfo.InvariantCulture);
			case SQLiteDateFormats.UnixEpoch:
				return (dateValue.Subtract(SQLiteConvert.UnixEpoch).Ticks / 10000000L).ToString();
			case SQLiteDateFormats.InvariantCulture:
				return dateValue.ToString((formatString != null) ? formatString : "yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture);
			case SQLiteDateFormats.CurrentCulture:
				return dateValue.ToString((formatString != null) ? formatString : "yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.CurrentCulture);
			}
			if (dateValue.Kind != DateTimeKind.Unspecified)
			{
				return dateValue.ToString(SQLiteConvert.GetDateTimeKindFormat(dateValue.Kind, formatString), CultureInfo.InvariantCulture);
			}
			return DateTime.SpecifyKind(dateValue, kind).ToString(SQLiteConvert.GetDateTimeKindFormat(kind, formatString), CultureInfo.InvariantCulture);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000402E0 File Offset: 0x0003E4E0
		internal DateTime ToDateTime(IntPtr ptr, int len)
		{
			return this.ToDateTime(this.ToString(ptr, len));
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000402F0 File Offset: 0x0003E4F0
		public static string[] Split(string source, char separator)
		{
			char[] array = new char[] { '"', separator };
			char[] array2 = new char[] { '"' };
			int num = 0;
			List<string> list = new List<string>();
			while (source.Length > 0)
			{
				num = source.IndexOfAny(array, num);
				if (num == -1)
				{
					break;
				}
				if (source[num] == array[0])
				{
					num = source.IndexOfAny(array2, num + 1);
					if (num == -1)
					{
						break;
					}
					num++;
				}
				else
				{
					string text = source.Substring(0, num).Trim();
					if (text.Length > 1 && text[0] == array2[0] && text[text.Length - 1] == text[0])
					{
						text = text.Substring(1, text.Length - 2);
					}
					source = source.Substring(num + 1).Trim();
					if (text.Length > 0)
					{
						list.Add(text);
					}
					num = 0;
				}
			}
			if (source.Length > 0)
			{
				string text = source.Trim();
				if (text.Length > 1 && text[0] == array2[0] && text[text.Length - 1] == text[0])
				{
					text = text.Substring(1, text.Length - 2);
				}
				list.Add(text);
			}
			string[] array3 = new string[list.Count];
			list.CopyTo(array3, 0);
			return array3;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0004048C File Offset: 0x0003E68C
		internal static string[] NewSplit(string value, char separator, bool keepQuote, ref string error)
		{
			if (separator == '\\' || separator == '"')
			{
				error = "separator character cannot be the escape or quote characters";
				return null;
			}
			if (value == null)
			{
				error = "string value to split cannot be null";
				return null;
			}
			int length = value.Length;
			if (length == 0)
			{
				return new string[0];
			}
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			bool flag = false;
			bool flag2 = false;
			while (i < length)
			{
				char c = value[i++];
				if (flag)
				{
					if (c != '\\' && c != '"' && c != separator)
					{
						stringBuilder.Append('\\');
					}
					stringBuilder.Append(c);
					flag = false;
				}
				else if (c == '\\')
				{
					flag = true;
				}
				else if (c == '"')
				{
					if (keepQuote)
					{
						stringBuilder.Append(c);
					}
					flag2 = !flag2;
				}
				else if (c == separator)
				{
					if (flag2)
					{
						stringBuilder.Append(c);
					}
					else
					{
						list.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			if (flag || flag2)
			{
				error = "unbalanced escape or quote character found";
				return null;
			}
			if (stringBuilder.Length > 0)
			{
				list.Add(stringBuilder.ToString());
			}
			return list.ToArray();
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000405EC File Offset: 0x0003E7EC
		public static string ToStringWithProvider(object obj, IFormatProvider provider)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is string)
			{
				return (string)obj;
			}
			IConvertible convertible = obj as IConvertible;
			if (convertible != null)
			{
				return convertible.ToString(provider);
			}
			return obj.ToString();
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00040634 File Offset: 0x0003E834
		internal static bool ToBoolean(object obj, IFormatProvider provider, bool viaFramework)
		{
			if (obj == null)
			{
				return false;
			}
			TypeCode typeCode = Type.GetTypeCode(obj.GetType());
			switch (typeCode)
			{
			case TypeCode.Empty:
			case TypeCode.DBNull:
				return false;
			case TypeCode.Boolean:
				return (bool)obj;
			case TypeCode.Char:
				return (char)obj != '\0';
			case TypeCode.SByte:
				return (sbyte)obj != 0;
			case TypeCode.Byte:
				return (byte)obj != 0;
			case TypeCode.Int16:
				return (short)obj != 0;
			case TypeCode.UInt16:
				return (ushort)obj != 0;
			case TypeCode.Int32:
				return (int)obj != 0;
			case TypeCode.UInt32:
				return (uint)obj != 0U;
			case TypeCode.Int64:
				return (long)obj != 0L;
			case TypeCode.UInt64:
				return (ulong)obj != 0UL;
			case TypeCode.Single:
				return (float)obj != 0f;
			case TypeCode.Double:
				return (double)obj != 0.0;
			case TypeCode.Decimal:
				return (decimal)obj != 0m;
			case TypeCode.String:
				if (!viaFramework)
				{
					return SQLiteConvert.ToBoolean(SQLiteConvert.ToStringWithProvider(obj, provider));
				}
				return Convert.ToBoolean(obj, provider);
			}
			throw new SQLiteException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Cannot convert type {0} to boolean", new object[] { typeCode }));
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x000407CC File Offset: 0x0003E9CC
		public static bool ToBoolean(object source)
		{
			if (source is bool)
			{
				return (bool)source;
			}
			return SQLiteConvert.ToBoolean(SQLiteConvert.ToStringWithProvider(source, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000407F0 File Offset: 0x0003E9F0
		internal static string ToString(int value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00040800 File Offset: 0x0003EA00
		public static bool ToBoolean(string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (string.Compare(source, 0, bool.TrueString, 0, source.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			if (string.Compare(source, 0, bool.FalseString, 0, source.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return false;
			}
			string text;
			switch (text = source.ToLower(CultureInfo.InvariantCulture))
			{
			case "y":
			case "yes":
			case "on":
			case "1":
				return true;
			case "n":
			case "no":
			case "off":
			case "0":
				return false;
			}
			throw new ArgumentException("source");
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00040930 File Offset: 0x0003EB30
		internal static Type SQLiteTypeToType(SQLiteType t)
		{
			if (t.Type == DbType.Object)
			{
				return SQLiteConvert._affinitytotype[(int)t.Affinity];
			}
			return SQLiteConvert.DbTypeToType(t.Type);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0004095C File Offset: 0x0003EB5C
		internal static DbType TypeToDbType(Type typ)
		{
			TypeCode typeCode = Type.GetTypeCode(typ);
			if (typeCode != TypeCode.Object)
			{
				return SQLiteConvert._typetodbtype[(int)typeCode];
			}
			if (typ == typeof(byte[]))
			{
				return DbType.Binary;
			}
			if (typ == typeof(Guid))
			{
				return DbType.Guid;
			}
			return DbType.String;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x000409B4 File Offset: 0x0003EBB4
		internal static int DbTypeToColumnSize(DbType typ)
		{
			return SQLiteConvert._dbtypetocolumnsize[(int)typ];
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x000409C0 File Offset: 0x0003EBC0
		internal static object DbTypeToNumericPrecision(DbType typ)
		{
			return SQLiteConvert._dbtypetonumericprecision[(int)typ];
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x000409CC File Offset: 0x0003EBCC
		internal static object DbTypeToNumericScale(DbType typ)
		{
			return SQLiteConvert._dbtypetonumericscale[(int)typ];
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000409D8 File Offset: 0x0003EBD8
		private static string GetDefaultTypeName(SQLiteConnection connection)
		{
			SQLiteConnectionFlags sqliteConnectionFlags = ((connection != null) ? connection.Flags : SQLiteConnectionFlags.None);
			if (HelperMethods.HasFlags(sqliteConnectionFlags, SQLiteConnectionFlags.NoConvertSettings))
			{
				return SQLiteConvert.FallbackDefaultTypeName;
			}
			string text = "Use_SQLiteConvert_DefaultTypeName";
			object obj = null;
			string text2 = null;
			if (connection != null)
			{
				if (connection.TryGetCachedSetting(text, text2, out obj))
				{
					goto IL_0072;
				}
			}
			try
			{
				obj = UnsafeNativeMethods.GetSettingValue(text, text2);
				if (obj == null)
				{
					obj = SQLiteConvert.FallbackDefaultTypeName;
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.SetCachedSetting(text, obj);
				}
			}
			IL_0072:
			return SQLiteConvert.SettingValueToString(obj);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00040A70 File Offset: 0x0003EC70
		private static void DefaultTypeNameWarning(DbType dbType, SQLiteConnectionFlags flags, string typeName)
		{
			if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.TraceWarning))
			{
				Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "WARNING: Type mapping failed, returning default name \"{0}\" for type {1}.", new object[] { typeName, dbType }));
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00040ABC File Offset: 0x0003ECBC
		private static void DefaultDbTypeWarning(string typeName, SQLiteConnectionFlags flags, DbType? dbType)
		{
			if (!string.IsNullOrEmpty(typeName) && HelperMethods.HasFlags(flags, SQLiteConnectionFlags.TraceWarning))
			{
				Trace.WriteLine(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "WARNING: Type mapping failed, returning default type {0} for name \"{1}\".", new object[] { dbType, typeName }));
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00040B14 File Offset: 0x0003ED14
		internal static string DbTypeToTypeName(SQLiteConnection connection, DbType dbType, SQLiteConnectionFlags flags)
		{
			string text = null;
			if (connection != null)
			{
				flags |= connection.Flags;
				if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.UseConnectionTypes))
				{
					SQLiteDbTypeMap typeNames = connection._typeNames;
					SQLiteDbTypeMapping sqliteDbTypeMapping;
					if (typeNames != null && typeNames.TryGetValue(dbType, out sqliteDbTypeMapping))
					{
						return sqliteDbTypeMapping.typeName;
					}
				}
				text = connection.DefaultTypeName;
			}
			if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.NoGlobalTypes))
			{
				if (text != null)
				{
					return text;
				}
				text = SQLiteConvert.GetDefaultTypeName(connection);
				SQLiteConvert.DefaultTypeNameWarning(dbType, flags, text);
				return text;
			}
			else
			{
				SQLiteDbTypeMapping sqliteDbTypeMapping2;
				if (SQLiteConvert._typeNames != null && SQLiteConvert._typeNames.TryGetValue(dbType, out sqliteDbTypeMapping2))
				{
					return sqliteDbTypeMapping2.typeName;
				}
				if (text != null)
				{
					return text;
				}
				text = SQLiteConvert.GetDefaultTypeName(connection);
				SQLiteConvert.DefaultTypeNameWarning(dbType, flags, text);
				return text;
			}
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00040BD8 File Offset: 0x0003EDD8
		internal static Type DbTypeToType(DbType typ)
		{
			return SQLiteConvert._dbtypeToType[(int)typ];
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00040BE8 File Offset: 0x0003EDE8
		internal static TypeAffinity TypeToAffinity(Type typ, SQLiteConnectionFlags flags)
		{
			TypeCode typeCode = Type.GetTypeCode(typ);
			if (typeCode == TypeCode.Object)
			{
				if (typ == typeof(byte[]) || typ == typeof(Guid))
				{
					return TypeAffinity.Blob;
				}
				return TypeAffinity.Text;
			}
			else
			{
				if (typeCode == TypeCode.Decimal && HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetDecimalAsText))
				{
					return TypeAffinity.Text;
				}
				return SQLiteConvert._typecodeAffinities[(int)typeCode];
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00040C5C File Offset: 0x0003EE5C
		private static SQLiteDbTypeMap GetSQLiteDbTypeMap()
		{
			return new SQLiteDbTypeMap(new SQLiteDbTypeMapping[]
			{
				new SQLiteDbTypeMapping("BIGINT", DbType.Int64, false),
				new SQLiteDbTypeMapping("BIGUINT", DbType.UInt64, false),
				new SQLiteDbTypeMapping("BINARY", DbType.Binary, false),
				new SQLiteDbTypeMapping("BIT", DbType.Boolean, true),
				new SQLiteDbTypeMapping("BLOB", DbType.Binary, true),
				new SQLiteDbTypeMapping("BOOL", DbType.Boolean, false),
				new SQLiteDbTypeMapping("BOOLEAN", DbType.Boolean, false),
				new SQLiteDbTypeMapping("CHAR", DbType.AnsiStringFixedLength, true),
				new SQLiteDbTypeMapping("CLOB", DbType.String, false),
				new SQLiteDbTypeMapping("COUNTER", DbType.Int64, false),
				new SQLiteDbTypeMapping("CURRENCY", DbType.Decimal, false),
				new SQLiteDbTypeMapping("DATE", DbType.DateTime, false),
				new SQLiteDbTypeMapping("DATETIME", DbType.DateTime, true),
				new SQLiteDbTypeMapping("DECIMAL", DbType.Decimal, true),
				new SQLiteDbTypeMapping("DECIMALTEXT", DbType.Decimal, false),
				new SQLiteDbTypeMapping("DOUBLE", DbType.Double, false),
				new SQLiteDbTypeMapping("FLOAT", DbType.Double, false),
				new SQLiteDbTypeMapping("GENERAL", DbType.Binary, false),
				new SQLiteDbTypeMapping("GUID", DbType.Guid, false),
				new SQLiteDbTypeMapping("IDENTITY", DbType.Int64, false),
				new SQLiteDbTypeMapping("IMAGE", DbType.Binary, false),
				new SQLiteDbTypeMapping("INT", DbType.Int32, true),
				new SQLiteDbTypeMapping("INT8", DbType.SByte, false),
				new SQLiteDbTypeMapping("INT16", DbType.Int16, false),
				new SQLiteDbTypeMapping("INT32", DbType.Int32, false),
				new SQLiteDbTypeMapping("INT64", DbType.Int64, false),
				new SQLiteDbTypeMapping("INTEGER", DbType.Int64, true),
				new SQLiteDbTypeMapping("INTEGER8", DbType.SByte, false),
				new SQLiteDbTypeMapping("INTEGER16", DbType.Int16, false),
				new SQLiteDbTypeMapping("INTEGER32", DbType.Int32, false),
				new SQLiteDbTypeMapping("INTEGER64", DbType.Int64, false),
				new SQLiteDbTypeMapping("LOGICAL", DbType.Boolean, false),
				new SQLiteDbTypeMapping("LONG", DbType.Int64, false),
				new SQLiteDbTypeMapping("LONGCHAR", DbType.String, false),
				new SQLiteDbTypeMapping("LONGTEXT", DbType.String, false),
				new SQLiteDbTypeMapping("LONGVARCHAR", DbType.String, false),
				new SQLiteDbTypeMapping("MEDIUMINT", DbType.Int32, false),
				new SQLiteDbTypeMapping("MEDIUMUINT", DbType.UInt32, false),
				new SQLiteDbTypeMapping("MEMO", DbType.String, false),
				new SQLiteDbTypeMapping("MONEY", DbType.Decimal, false),
				new SQLiteDbTypeMapping("NCHAR", DbType.StringFixedLength, true),
				new SQLiteDbTypeMapping("NOTE", DbType.String, false),
				new SQLiteDbTypeMapping("NTEXT", DbType.String, false),
				new SQLiteDbTypeMapping("NUMBER", DbType.Decimal, false),
				new SQLiteDbTypeMapping("NUMERIC", DbType.Decimal, false),
				new SQLiteDbTypeMapping("NUMERICTEXT", DbType.Decimal, false),
				new SQLiteDbTypeMapping("NVARCHAR", DbType.String, true),
				new SQLiteDbTypeMapping("OLEOBJECT", DbType.Binary, false),
				new SQLiteDbTypeMapping("RAW", DbType.Binary, false),
				new SQLiteDbTypeMapping("REAL", DbType.Double, true),
				new SQLiteDbTypeMapping("SINGLE", DbType.Single, true),
				new SQLiteDbTypeMapping("SMALLDATE", DbType.DateTime, false),
				new SQLiteDbTypeMapping("SMALLINT", DbType.Int16, true),
				new SQLiteDbTypeMapping("SMALLUINT", DbType.UInt16, true),
				new SQLiteDbTypeMapping("STRING", DbType.String, false),
				new SQLiteDbTypeMapping("TEXT", DbType.String, false),
				new SQLiteDbTypeMapping("TIME", DbType.DateTime, false),
				new SQLiteDbTypeMapping("TIMESTAMP", DbType.DateTime, false),
				new SQLiteDbTypeMapping("TINYINT", DbType.Byte, true),
				new SQLiteDbTypeMapping("TINYSINT", DbType.SByte, true),
				new SQLiteDbTypeMapping("UINT", DbType.UInt32, true),
				new SQLiteDbTypeMapping("UINT8", DbType.Byte, false),
				new SQLiteDbTypeMapping("UINT16", DbType.UInt16, false),
				new SQLiteDbTypeMapping("UINT32", DbType.UInt32, false),
				new SQLiteDbTypeMapping("UINT64", DbType.UInt64, false),
				new SQLiteDbTypeMapping("ULONG", DbType.UInt64, false),
				new SQLiteDbTypeMapping("UNIQUEIDENTIFIER", DbType.Guid, true),
				new SQLiteDbTypeMapping("UNSIGNEDINTEGER", DbType.UInt64, true),
				new SQLiteDbTypeMapping("UNSIGNEDINTEGER8", DbType.Byte, false),
				new SQLiteDbTypeMapping("UNSIGNEDINTEGER16", DbType.UInt16, false),
				new SQLiteDbTypeMapping("UNSIGNEDINTEGER32", DbType.UInt32, false),
				new SQLiteDbTypeMapping("UNSIGNEDINTEGER64", DbType.UInt64, false),
				new SQLiteDbTypeMapping("VARBINARY", DbType.Binary, false),
				new SQLiteDbTypeMapping("VARCHAR", DbType.AnsiString, true),
				new SQLiteDbTypeMapping("VARCHAR2", DbType.AnsiString, false),
				new SQLiteDbTypeMapping("YESNO", DbType.Boolean, false)
			});
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00041290 File Offset: 0x0003F490
		internal static bool IsStringDbType(DbType type)
		{
			if (type != DbType.AnsiString && type != DbType.String)
			{
				switch (type)
				{
				case DbType.AnsiStringFixedLength:
				case DbType.StringFixedLength:
					break;
				default:
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x000412CC File Offset: 0x0003F4CC
		private static string SettingValueToString(object value)
		{
			if (value is string)
			{
				return (string)value;
			}
			if (value != null)
			{
				return value.ToString();
			}
			return null;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000412F0 File Offset: 0x0003F4F0
		private static DbType GetDefaultDbType(SQLiteConnection connection)
		{
			SQLiteConnectionFlags sqliteConnectionFlags = ((connection != null) ? connection.Flags : SQLiteConnectionFlags.None);
			if (HelperMethods.HasFlags(sqliteConnectionFlags, SQLiteConnectionFlags.NoConvertSettings))
			{
				return DbType.Object;
			}
			bool flag = false;
			string text = "Use_SQLiteConvert_DefaultDbType";
			object obj = null;
			string text2 = null;
			if (connection == null || !connection.TryGetCachedSetting(text, text2, out obj))
			{
				obj = UnsafeNativeMethods.GetSettingValue(text, text2);
				if (obj == null)
				{
					obj = DbType.Object;
				}
			}
			else
			{
				flag = true;
			}
			DbType dbType;
			try
			{
				if (!(obj is DbType))
				{
					obj = SQLiteConnection.TryParseEnum(typeof(DbType), SQLiteConvert.SettingValueToString(obj), true);
					if (!(obj is DbType))
					{
						obj = DbType.Object;
					}
				}
				dbType = (DbType)obj;
			}
			finally
			{
				if (!flag && connection != null)
				{
					connection.SetCachedSetting(text, obj);
				}
			}
			return dbType;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000413D0 File Offset: 0x0003F5D0
		public static string GetStringOrNull(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is string)
			{
				return (string)value;
			}
			if (value == DBNull.Value)
			{
				return null;
			}
			return value.ToString();
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00041400 File Offset: 0x0003F600
		internal static bool LooksLikeNull(string text)
		{
			return text == null;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00041408 File Offset: 0x0003F608
		internal static bool LooksLikeInt64(string text)
		{
			long num;
			return long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num) && string.Equals(num.ToString(CultureInfo.InvariantCulture), text, StringComparison.Ordinal);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00041444 File Offset: 0x0003F644
		internal static bool LooksLikeDouble(string text)
		{
			double num;
			return double.TryParse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out num) && string.Equals(num.ToString(CultureInfo.InvariantCulture), text, StringComparison.Ordinal);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00041484 File Offset: 0x0003F684
		internal static bool LooksLikeDateTime(SQLiteConvert convert, string text)
		{
			if (convert == null)
			{
				return false;
			}
			try
			{
				DateTime dateTime = convert.ToDateTime(text);
				if (string.Equals(convert.ToString(dateTime), text, StringComparison.Ordinal))
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x000414DC File Offset: 0x0003F6DC
		internal static DbType TypeNameToDbType(SQLiteConnection connection, string typeName, SQLiteConnectionFlags flags)
		{
			DbType? dbType = null;
			if (connection != null)
			{
				flags |= connection.Flags;
				if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.UseConnectionTypes))
				{
					SQLiteDbTypeMap typeNames = connection._typeNames;
					if (typeNames != null && typeName != null)
					{
						SQLiteDbTypeMapping sqliteDbTypeMapping;
						if (typeNames.TryGetValue(typeName, out sqliteDbTypeMapping))
						{
							return sqliteDbTypeMapping.dataType;
						}
						int num = typeName.IndexOf('(');
						if (num > 0 && typeNames.TryGetValue(typeName.Substring(0, num).TrimEnd(new char[0]), out sqliteDbTypeMapping))
						{
							return sqliteDbTypeMapping.dataType;
						}
					}
				}
				dbType = connection.DefaultDbType;
			}
			if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.NoGlobalTypes))
			{
				if (dbType != null)
				{
					return dbType.Value;
				}
				dbType = new DbType?(SQLiteConvert.GetDefaultDbType(connection));
				SQLiteConvert.DefaultDbTypeWarning(typeName, flags, dbType);
				return dbType.Value;
			}
			else
			{
				if (SQLiteConvert._typeNames != null && typeName != null)
				{
					SQLiteDbTypeMapping sqliteDbTypeMapping2;
					if (SQLiteConvert._typeNames.TryGetValue(typeName, out sqliteDbTypeMapping2))
					{
						return sqliteDbTypeMapping2.dataType;
					}
					int num2 = typeName.IndexOf('(');
					if (num2 > 0 && SQLiteConvert._typeNames.TryGetValue(typeName.Substring(0, num2).TrimEnd(new char[0]), out sqliteDbTypeMapping2))
					{
						return sqliteDbTypeMapping2.dataType;
					}
				}
				if (dbType != null)
				{
					return dbType.Value;
				}
				dbType = new DbType?(SQLiteConvert.GetDefaultDbType(connection));
				SQLiteConvert.DefaultDbTypeWarning(typeName, flags, dbType);
				return dbType.Value;
			}
		}

		// Token: 0x04000515 RID: 1301
		internal const char EscapeChar = '\\';

		// Token: 0x04000516 RID: 1302
		internal const char QuoteChar = '"';

		// Token: 0x04000517 RID: 1303
		internal const char AltQuoteChar = '\'';

		// Token: 0x04000518 RID: 1304
		internal const char ValueChar = '=';

		// Token: 0x04000519 RID: 1305
		internal const char PairChar = ';';

		// Token: 0x0400051A RID: 1306
		private const DbType FallbackDefaultDbType = DbType.Object;

		// Token: 0x0400051B RID: 1307
		private const string FullFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";

		// Token: 0x0400051C RID: 1308
		internal static readonly char[] SpecialChars = new char[] { '"', '\'', ';', '=', '\\' };

		// Token: 0x0400051D RID: 1309
		private static readonly string FallbackDefaultTypeName = string.Empty;

		// Token: 0x0400051E RID: 1310
		protected static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400051F RID: 1311
		private static readonly double OleAutomationEpochAsJulianDay = 2415018.5;

		// Token: 0x04000520 RID: 1312
		private static readonly long MinimumJd = SQLiteConvert.computeJD(DateTime.MinValue);

		// Token: 0x04000521 RID: 1313
		private static readonly long MaximumJd = SQLiteConvert.computeJD(DateTime.MaxValue);

		// Token: 0x04000522 RID: 1314
		private static string[] _datetimeFormats = new string[]
		{
			"THHmmssK", "THHmmK", "HH:mm:ss.FFFFFFFK", "HH:mm:ssK", "HH:mmK", "yyyy-MM-dd HH:mm:ss.FFFFFFFK", "yyyy-MM-dd HH:mm:ssK", "yyyy-MM-dd HH:mmK", "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", "yyyy-MM-ddTHH:mmK",
			"yyyy-MM-ddTHH:mm:ssK", "yyyyMMddHHmmssK", "yyyyMMddHHmmK", "yyyyMMddTHHmmssFFFFFFFK", "THHmmss", "THHmm", "HH:mm:ss.FFFFFFF", "HH:mm:ss", "HH:mm", "yyyy-MM-dd HH:mm:ss.FFFFFFF",
			"yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-ddTHH:mm:ss.FFFFFFF", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH:mm:ss", "yyyyMMddHHmmss", "yyyyMMddHHmm", "yyyyMMddTHHmmssFFFFFFF", "yyyy-MM-dd", "yyyyMMdd",
			"yy-MM-dd"
		};

		// Token: 0x04000523 RID: 1315
		private static readonly string _datetimeFormatUtc = SQLiteConvert._datetimeFormats[5];

		// Token: 0x04000524 RID: 1316
		private static readonly string _datetimeFormatLocal = SQLiteConvert._datetimeFormats[19];

		// Token: 0x04000525 RID: 1317
		private static Encoding _utf8 = new UTF8Encoding();

		// Token: 0x04000526 RID: 1318
		internal SQLiteDateFormats _datetimeFormat;

		// Token: 0x04000527 RID: 1319
		internal DateTimeKind _datetimeKind;

		// Token: 0x04000528 RID: 1320
		internal string _datetimeFormatString;

		// Token: 0x04000529 RID: 1321
		private static Type[] _affinitytotype = new Type[]
		{
			typeof(object),
			typeof(long),
			typeof(double),
			typeof(string),
			typeof(byte[]),
			typeof(object),
			null,
			null,
			null,
			null,
			typeof(DateTime),
			typeof(object)
		};

		// Token: 0x0400052A RID: 1322
		private static DbType[] _typetodbtype = new DbType[]
		{
			DbType.Object,
			DbType.Binary,
			DbType.Object,
			DbType.Boolean,
			DbType.SByte,
			DbType.SByte,
			DbType.Byte,
			DbType.Int16,
			DbType.UInt16,
			DbType.Int32,
			DbType.UInt32,
			DbType.Int64,
			DbType.UInt64,
			DbType.Single,
			DbType.Double,
			DbType.Decimal,
			DbType.DateTime,
			DbType.Object,
			DbType.String
		};

		// Token: 0x0400052B RID: 1323
		private static int[] _dbtypetocolumnsize = new int[]
		{
			int.MaxValue, int.MaxValue, 1, 1, 8, 8, 8, 8, 8, 16,
			2, 4, 8, int.MaxValue, 1, 4, int.MaxValue, 8, 2, 4,
			8, 8, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, 8, 10
		};

		// Token: 0x0400052C RID: 1324
		private static object[] _dbtypetonumericprecision = new object[]
		{
			DBNull.Value,
			DBNull.Value,
			3,
			DBNull.Value,
			19,
			DBNull.Value,
			DBNull.Value,
			53,
			53,
			DBNull.Value,
			5,
			10,
			19,
			DBNull.Value,
			3,
			24,
			DBNull.Value,
			DBNull.Value,
			5,
			10,
			19,
			53,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value
		};

		// Token: 0x0400052D RID: 1325
		private static object[] _dbtypetonumericscale = new object[]
		{
			DBNull.Value,
			DBNull.Value,
			0,
			DBNull.Value,
			4,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			0,
			0,
			0,
			DBNull.Value,
			0,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			0,
			0,
			0,
			0,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value,
			DBNull.Value
		};

		// Token: 0x0400052E RID: 1326
		private static Type[] _dbtypeToType = new Type[]
		{
			typeof(string),
			typeof(byte[]),
			typeof(byte),
			typeof(bool),
			typeof(decimal),
			typeof(DateTime),
			typeof(DateTime),
			typeof(decimal),
			typeof(double),
			typeof(Guid),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(object),
			typeof(sbyte),
			typeof(float),
			typeof(string),
			typeof(DateTime),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(double),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(DateTime),
			typeof(DateTimeOffset)
		};

		// Token: 0x0400052F RID: 1327
		private static TypeAffinity[] _typecodeAffinities = new TypeAffinity[]
		{
			TypeAffinity.Null,
			TypeAffinity.Blob,
			TypeAffinity.Null,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Int64,
			TypeAffinity.Double,
			TypeAffinity.Double,
			TypeAffinity.Double,
			TypeAffinity.DateTime,
			TypeAffinity.Null,
			TypeAffinity.Text
		};

		// Token: 0x04000530 RID: 1328
		private static readonly SQLiteDbTypeMap _typeNames = SQLiteConvert.GetSQLiteDbTypeMap();
	}
}
