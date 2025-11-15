using System;
using System.Text;

namespace MessagePackLib.MessagePack
{
	// Token: 0x0200002F RID: 47
	public class BytesTools
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x0000810C File Offset: 0x0000630C
		public static byte[] GetUtf8Bytes(string s)
		{
			return BytesTools.utf8Encode.GetBytes(s);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000811C File Offset: 0x0000631C
		public static string GetString(byte[] utf8Bytes)
		{
			return BytesTools.utf8Encode.GetString(utf8Bytes);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000812C File Offset: 0x0000632C
		public static string BytesAsString(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				stringBuilder.Append(string.Format("{0:D3} ", b));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00008178 File Offset: 0x00006378
		public static string BytesAsHexString(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				stringBuilder.Append(string.Format("{0:X2} ", b));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000081C4 File Offset: 0x000063C4
		public static byte[] SwapBytes(byte[] v)
		{
			byte[] array = new byte[v.Length];
			int num = v.Length - 1;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = v[num];
				num--;
			}
			return array;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00008200 File Offset: 0x00006400
		public static byte[] SwapInt64(long v)
		{
			return BytesTools.SwapBytes(BitConverter.GetBytes(v));
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00008210 File Offset: 0x00006410
		public static byte[] SwapInt32(int v)
		{
			byte[] array = new byte[]
			{
				0,
				0,
				0,
				(byte)v
			};
			array[2] = (byte)(v >> 8);
			array[1] = (byte)(v >> 16);
			array[0] = (byte)(v >> 24);
			return array;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00008234 File Offset: 0x00006434
		public static byte[] SwapInt16(short v)
		{
			byte[] array = new byte[]
			{
				0,
				(byte)v
			};
			array[0] = (byte)(v >> 8);
			return array;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00008248 File Offset: 0x00006448
		public static byte[] SwapDouble(double v)
		{
			return BytesTools.SwapBytes(BitConverter.GetBytes(v));
		}

		// Token: 0x040000B6 RID: 182
		private static UTF8Encoding utf8Encode = new UTF8Encoding();
	}
}
