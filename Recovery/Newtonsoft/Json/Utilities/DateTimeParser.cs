using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	[Nullable(0)]
	internal struct DateTimeParser
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x00017944 File Offset: 0x00015B44
		public bool Parse(char[] text, int startIndex, int length)
		{
			this._text = text;
			this._end = startIndex + length;
			return this.ParseDate(startIndex) && this.ParseChar(DateTimeParser.Lzyyyy_MM_dd + startIndex, 'T') && this.ParseTimeAndZoneAndWhitespace(DateTimeParser.Lzyyyy_MM_ddT + startIndex);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001799C File Offset: 0x00015B9C
		private bool ParseDate(int start)
		{
			return this.Parse4Digit(start, out this.Year) && 1 <= this.Year && this.ParseChar(start + DateTimeParser.Lzyyyy, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_, out this.Month) && 1 <= this.Month && this.Month <= 12 && this.ParseChar(start + DateTimeParser.Lzyyyy_MM, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_MM_, out this.Day) && 1 <= this.Day && this.Day <= DateTime.DaysInMonth(this.Year, this.Month);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00017A68 File Offset: 0x00015C68
		private bool ParseTimeAndZoneAndWhitespace(int start)
		{
			return this.ParseTime(ref start) && this.ParseZone(start);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00017A80 File Offset: 0x00015C80
		private bool ParseTime(ref int start)
		{
			if (!this.Parse2Digit(start, out this.Hour) || this.Hour > 24 || !this.ParseChar(start + DateTimeParser.LzHH, ':') || !this.Parse2Digit(start + DateTimeParser.LzHH_, out this.Minute) || this.Minute >= 60 || !this.ParseChar(start + DateTimeParser.LzHH_mm, ':') || !this.Parse2Digit(start + DateTimeParser.LzHH_mm_, out this.Second) || this.Second >= 60 || (this.Hour == 24 && (this.Minute != 0 || this.Second != 0)))
			{
				return false;
			}
			start += DateTimeParser.LzHH_mm_ss;
			if (this.ParseChar(start, '.'))
			{
				this.Fraction = 0;
				int num = 0;
				for (;;)
				{
					int num2 = start + 1;
					start = num2;
					if (num2 >= this._end || num >= 7)
					{
						break;
					}
					int num3 = (int)(this._text[start] - '0');
					if (num3 < 0 || num3 > 9)
					{
						break;
					}
					this.Fraction = this.Fraction * 10 + num3;
					num++;
				}
				if (num < 7)
				{
					if (num == 0)
					{
						return false;
					}
					this.Fraction *= DateTimeParser.Power10[7 - num];
				}
				if (this.Hour == 24 && this.Fraction != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00017BFC File Offset: 0x00015DFC
		private bool ParseZone(int start)
		{
			if (start < this._end)
			{
				char c = this._text[start];
				if (c == 'Z' || c == 'z')
				{
					this.Zone = ParserTimeZone.Utc;
					start++;
				}
				else
				{
					if (start + 2 < this._end && this.Parse2Digit(start + DateTimeParser.Lz_, out this.ZoneHour) && this.ZoneHour <= 99)
					{
						if (c != '+')
						{
							if (c == '-')
							{
								this.Zone = ParserTimeZone.LocalWestOfUtc;
								start += DateTimeParser.Lz_zz;
							}
						}
						else
						{
							this.Zone = ParserTimeZone.LocalEastOfUtc;
							start += DateTimeParser.Lz_zz;
						}
					}
					if (start < this._end)
					{
						if (this.ParseChar(start, ':'))
						{
							start++;
							if (start + 1 < this._end && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
							{
								start += 2;
							}
						}
						else if (start + 1 < this._end && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
						{
							start += 2;
						}
					}
				}
			}
			return start == this._end;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00017D3C File Offset: 0x00015F3C
		private bool Parse4Digit(int start, out int num)
		{
			if (start + 3 < this._end)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				int num4 = (int)(this._text[start + 2] - '0');
				int num5 = (int)(this._text[start + 3] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10 && 0 <= num4 && num4 < 10 && 0 <= num5 && num5 < 10)
				{
					num = ((num2 * 10 + num3) * 10 + num4) * 10 + num5;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00017DE8 File Offset: 0x00015FE8
		private bool Parse2Digit(int start, out int num)
		{
			if (start + 1 < this._end)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10)
				{
					num = num2 * 10 + num3;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00017E50 File Offset: 0x00016050
		private bool ParseChar(int start, char ch)
		{
			return start < this._end && this._text[start] == ch;
		}

		// Token: 0x0400025E RID: 606
		public int Year;

		// Token: 0x0400025F RID: 607
		public int Month;

		// Token: 0x04000260 RID: 608
		public int Day;

		// Token: 0x04000261 RID: 609
		public int Hour;

		// Token: 0x04000262 RID: 610
		public int Minute;

		// Token: 0x04000263 RID: 611
		public int Second;

		// Token: 0x04000264 RID: 612
		public int Fraction;

		// Token: 0x04000265 RID: 613
		public int ZoneHour;

		// Token: 0x04000266 RID: 614
		public int ZoneMinute;

		// Token: 0x04000267 RID: 615
		public ParserTimeZone Zone;

		// Token: 0x04000268 RID: 616
		private char[] _text;

		// Token: 0x04000269 RID: 617
		private int _end;

		// Token: 0x0400026A RID: 618
		private static readonly int[] Power10 = new int[] { -1, 10, 100, 1000, 10000, 100000, 1000000 };

		// Token: 0x0400026B RID: 619
		private static readonly int Lzyyyy = "yyyy".Length;

		// Token: 0x0400026C RID: 620
		private static readonly int Lzyyyy_ = "yyyy-".Length;

		// Token: 0x0400026D RID: 621
		private static readonly int Lzyyyy_MM = "yyyy-MM".Length;

		// Token: 0x0400026E RID: 622
		private static readonly int Lzyyyy_MM_ = "yyyy-MM-".Length;

		// Token: 0x0400026F RID: 623
		private static readonly int Lzyyyy_MM_dd = "yyyy-MM-dd".Length;

		// Token: 0x04000270 RID: 624
		private static readonly int Lzyyyy_MM_ddT = "yyyy-MM-ddT".Length;

		// Token: 0x04000271 RID: 625
		private static readonly int LzHH = "HH".Length;

		// Token: 0x04000272 RID: 626
		private static readonly int LzHH_ = "HH:".Length;

		// Token: 0x04000273 RID: 627
		private static readonly int LzHH_mm = "HH:mm".Length;

		// Token: 0x04000274 RID: 628
		private static readonly int LzHH_mm_ = "HH:mm:".Length;

		// Token: 0x04000275 RID: 629
		private static readonly int LzHH_mm_ss = "HH:mm:ss".Length;

		// Token: 0x04000276 RID: 630
		private static readonly int Lz_ = "-".Length;

		// Token: 0x04000277 RID: 631
		private static readonly int Lz_zz = "-zz".Length;

		// Token: 0x04000278 RID: 632
		private const short MaxFractionDigits = 7;
	}
}
