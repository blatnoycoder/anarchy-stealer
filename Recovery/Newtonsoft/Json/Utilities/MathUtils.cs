using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000096 RID: 150
	internal static class MathUtils
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x0001CE68 File Offset: 0x0001B068
		public static int IntLength(ulong i)
		{
			if (i < 10000000000UL)
			{
				if (i < 10UL)
				{
					return 1;
				}
				if (i < 100UL)
				{
					return 2;
				}
				if (i < 1000UL)
				{
					return 3;
				}
				if (i < 10000UL)
				{
					return 4;
				}
				if (i < 100000UL)
				{
					return 5;
				}
				if (i < 1000000UL)
				{
					return 6;
				}
				if (i < 10000000UL)
				{
					return 7;
				}
				if (i < 100000000UL)
				{
					return 8;
				}
				if (i < 1000000000UL)
				{
					return 9;
				}
				return 10;
			}
			else
			{
				if (i < 100000000000UL)
				{
					return 11;
				}
				if (i < 1000000000000UL)
				{
					return 12;
				}
				if (i < 10000000000000UL)
				{
					return 13;
				}
				if (i < 100000000000000UL)
				{
					return 14;
				}
				if (i < 1000000000000000UL)
				{
					return 15;
				}
				if (i < 10000000000000000UL)
				{
					return 16;
				}
				if (i < 100000000000000000UL)
				{
					return 17;
				}
				if (i < 1000000000000000000UL)
				{
					return 18;
				}
				if (i < 10000000000000000000UL)
				{
					return 19;
				}
				return 20;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001CFA8 File Offset: 0x0001B1A8
		public static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
		public static int? Min(int? val1, int? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new int?(Math.Min(val1.GetValueOrDefault(), val2.GetValueOrDefault()));
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001CFF8 File Offset: 0x0001B1F8
		public static int? Max(int? val1, int? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new int?(Math.Max(val1.GetValueOrDefault(), val2.GetValueOrDefault()));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001D030 File Offset: 0x0001B230
		public static double? Max(double? val1, double? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new double?(Math.Max(val1.GetValueOrDefault(), val2.GetValueOrDefault()));
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001D068 File Offset: 0x0001B268
		public static bool ApproxEquals(double d1, double d2)
		{
			if (d1 == d2)
			{
				return true;
			}
			double num = (Math.Abs(d1) + Math.Abs(d2) + 10.0) * 2.220446049250313E-16;
			double num2 = d1 - d2;
			return -num < num2 && num > num2;
		}
	}
}
