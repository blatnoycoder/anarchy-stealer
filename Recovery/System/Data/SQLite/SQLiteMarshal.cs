using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001D3 RID: 467
	internal static class SQLiteMarshal
	{
		// Token: 0x060014FD RID: 5373 RVA: 0x00060234 File Offset: 0x0005E434
		public static IntPtr IntPtrForOffset(IntPtr pointer, int offset)
		{
			return new IntPtr(pointer.ToInt64() + (long)offset);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00060248 File Offset: 0x0005E448
		public static int RoundUp(int size, int alignment)
		{
			int num = alignment - 1;
			return (size + num) & ~num;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00060264 File Offset: 0x0005E464
		public static int NextOffsetOf(int offset, int size, int alignment)
		{
			return SQLiteMarshal.RoundUp(offset + size, alignment);
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00060270 File Offset: 0x0005E470
		public static int ReadInt32(IntPtr pointer, int offset)
		{
			return Marshal.ReadInt32(pointer, offset);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0006027C File Offset: 0x0005E47C
		public static long ReadInt64(IntPtr pointer, int offset)
		{
			return Marshal.ReadInt64(pointer, offset);
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00060288 File Offset: 0x0005E488
		public static double ReadDouble(IntPtr pointer, int offset)
		{
			return BitConverter.Int64BitsToDouble(Marshal.ReadInt64(pointer, offset));
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00060298 File Offset: 0x0005E498
		public static IntPtr ReadIntPtr(IntPtr pointer, int offset)
		{
			return Marshal.ReadIntPtr(pointer, offset);
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000602A4 File Offset: 0x0005E4A4
		public static void WriteInt32(IntPtr pointer, int offset, int value)
		{
			Marshal.WriteInt32(pointer, offset, value);
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000602B0 File Offset: 0x0005E4B0
		public static void WriteInt64(IntPtr pointer, int offset, long value)
		{
			Marshal.WriteInt64(pointer, offset, value);
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x000602BC File Offset: 0x0005E4BC
		public static void WriteDouble(IntPtr pointer, int offset, double value)
		{
			Marshal.WriteInt64(pointer, offset, BitConverter.DoubleToInt64Bits(value));
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x000602CC File Offset: 0x0005E4CC
		public static void WriteIntPtr(IntPtr pointer, int offset, IntPtr value)
		{
			Marshal.WriteIntPtr(pointer, offset, value);
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x000602D8 File Offset: 0x0005E4D8
		public static int GetHashCode(object value, bool identity)
		{
			if (identity)
			{
				return RuntimeHelpers.GetHashCode(value);
			}
			if (value == null)
			{
				return 0;
			}
			return value.GetHashCode();
		}
	}
}
