using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001D2 RID: 466
	internal static class SQLiteBytes
	{
		// Token: 0x060014FA RID: 5370 RVA: 0x0006017C File Offset: 0x0005E37C
		public static byte[] FromIntPtr(IntPtr pValue, int length)
		{
			if (pValue == IntPtr.Zero)
			{
				return null;
			}
			if (length == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[length];
			Marshal.Copy(pValue, array, 0, length);
			return array;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x000601C0 File Offset: 0x0005E3C0
		public static IntPtr ToIntPtr(byte[] value)
		{
			int num = 0;
			return SQLiteBytes.ToIntPtr(value, ref num);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000601DC File Offset: 0x0005E3DC
		public static IntPtr ToIntPtr(byte[] value, ref int length)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			length = value.Length;
			if (length == 0)
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr = SQLiteMemory.Allocate(length);
			if (intPtr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			Marshal.Copy(value, 0, intPtr, length);
			return intPtr;
		}
	}
}
