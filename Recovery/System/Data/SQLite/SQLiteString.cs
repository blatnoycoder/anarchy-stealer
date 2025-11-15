using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.SQLite
{
	// Token: 0x020001D1 RID: 465
	internal static class SQLiteString
	{
		// Token: 0x060014EE RID: 5358 RVA: 0x0005FF18 File Offset: 0x0005E118
		public static byte[] GetUtf8BytesFromString(string value)
		{
			if (value == null)
			{
				return null;
			}
			return SQLiteString.Utf8Encoding.GetBytes(value);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0005FF30 File Offset: 0x0005E130
		public static string GetStringFromUtf8Bytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return SQLiteString.Utf8Encoding.GetString(bytes);
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0005FF48 File Offset: 0x0005E148
		public static int ProbeForUtf8ByteLength(IntPtr pValue, int limit)
		{
			int num = 0;
			if (pValue != IntPtr.Zero && limit > 0)
			{
				while (Marshal.ReadByte(pValue, num) != 0 && num < limit)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0005FF8C File Offset: 0x0005E18C
		public static string StringFromUtf8IntPtr(IntPtr pValue)
		{
			return SQLiteString.StringFromUtf8IntPtr(pValue, SQLiteString.ProbeForUtf8ByteLength(pValue, SQLiteString.ThirtyBits));
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0005FFA0 File Offset: 0x0005E1A0
		public static string StringFromUtf8IntPtr(IntPtr pValue, int length)
		{
			if (pValue == IntPtr.Zero)
			{
				return null;
			}
			if (length > 0)
			{
				byte[] array = new byte[length];
				Marshal.Copy(pValue, array, 0, length);
				return SQLiteString.GetStringFromUtf8Bytes(array);
			}
			return string.Empty;
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0005FFE8 File Offset: 0x0005E1E8
		public static IntPtr Utf8IntPtrFromString(string value)
		{
			return SQLiteString.Utf8IntPtrFromString(value, true);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0005FFF4 File Offset: 0x0005E1F4
		public static IntPtr Utf8IntPtrFromString(string value, bool tracked)
		{
			int num = 0;
			return SQLiteString.Utf8IntPtrFromString(value, tracked, ref num);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00060010 File Offset: 0x0005E210
		public static IntPtr Utf8IntPtrFromString(string value, ref int length)
		{
			return SQLiteString.Utf8IntPtrFromString(value, true, ref length);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0006001C File Offset: 0x0005E21C
		public static IntPtr Utf8IntPtrFromString(string value, bool tracked, ref int length)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr = IntPtr.Zero;
			byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
			if (utf8BytesFromString == null)
			{
				return IntPtr.Zero;
			}
			length = utf8BytesFromString.Length;
			if (tracked)
			{
				intPtr = SQLiteMemory.Allocate(length + 1);
			}
			else
			{
				intPtr = SQLiteMemory.AllocateUntracked(length + 1);
			}
			if (intPtr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			Marshal.Copy(utf8BytesFromString, 0, intPtr, length);
			Marshal.WriteByte(intPtr, length, 0);
			return intPtr;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000600A0 File Offset: 0x0005E2A0
		public static string[] StringArrayFromUtf8SizeAndIntPtr(int argc, IntPtr argv)
		{
			if (argc < 0)
			{
				return null;
			}
			if (argv == IntPtr.Zero)
			{
				return null;
			}
			string[] array = new string[argc];
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				IntPtr intPtr = SQLiteMarshal.ReadIntPtr(argv, num);
				array[i] = ((intPtr != IntPtr.Zero) ? SQLiteString.StringFromUtf8IntPtr(intPtr) : null);
				i++;
				num += IntPtr.Size;
			}
			return array;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00060118 File Offset: 0x0005E318
		public static IntPtr[] Utf8IntPtrArrayFromStringArray(string[] values, bool tracked)
		{
			if (values == null)
			{
				return null;
			}
			IntPtr[] array = new IntPtr[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = SQLiteString.Utf8IntPtrFromString(values[i], tracked);
			}
			return array;
		}

		// Token: 0x040008B0 RID: 2224
		private static int ThirtyBits = 1073741823;

		// Token: 0x040008B1 RID: 2225
		private static readonly Encoding Utf8Encoding = Encoding.UTF8;
	}
}
