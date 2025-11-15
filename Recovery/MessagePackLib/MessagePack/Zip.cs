using System;
using System.IO;
using System.IO.Compression;

namespace MessagePackLib.MessagePack
{
	// Token: 0x02000036 RID: 54
	public static class Zip
	{
		// Token: 0x06000121 RID: 289 RVA: 0x0000989C File Offset: 0x00007A9C
		public static byte[] Decompress(byte[] input)
		{
			byte[] array3;
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				byte[] array = new byte[4];
				memoryStream.Read(array, 0, 4);
				int num = BitConverter.ToInt32(array, 0);
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					byte[] array2 = new byte[num];
					gzipStream.Read(array2, 0, num);
					array3 = array2;
				}
			}
			return array3;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00009928 File Offset: 0x00007B28
		public static byte[] Compress(byte[] input)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] bytes = BitConverter.GetBytes(input.Length);
				memoryStream.Write(bytes, 0, 4);
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gzipStream.Write(input, 0, input.Length);
					gzipStream.Flush();
				}
				array = memoryStream.ToArray();
			}
			return array;
		}
	}
}
