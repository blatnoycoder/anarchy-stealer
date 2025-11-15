using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Stealer
{
	// Token: 0x0200000A RID: 10
	internal class cAesGcm
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000336C File Offset: 0x0000156C
		public byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
		{
			IntPtr intPtr = this.OpenAlgorithmProvider(cBCrypt.BCRYPT_AES_ALGORITHM, cBCrypt.MS_PRIMITIVE_PROVIDER, cBCrypt.BCRYPT_CHAIN_MODE_GCM);
			IntPtr intPtr3;
			IntPtr intPtr2 = this.ImportKey(intPtr, key, out intPtr3);
			cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
			byte[] array2;
			using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
			{
				byte[] array = new byte[this.MaxAuthTagSize(intPtr)];
				int num = 0;
				uint num2 = cBCrypt.BCryptDecrypt(intPtr3, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
				bool flag = num2 > 0U;
				if (flag)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() (get size) failed with status code: {0}", num2));
				}
				array2 = new byte[num];
				num2 = cBCrypt.BCryptDecrypt(intPtr3, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
				bool flag2 = num2 == cBCrypt.STATUS_AUTH_TAG_MISMATCH;
				if (flag2)
				{
					throw new CryptographicException("BCrypt.BCryptDecrypt(): authentication tag mismatch");
				}
				bool flag3 = num2 > 0U;
				if (flag3)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() failed with status code:{0}", num2));
				}
			}
			cBCrypt.BCryptDestroyKey(intPtr3);
			Marshal.FreeHGlobal(intPtr2);
			cBCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
			return array2;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000034AC File Offset: 0x000016AC
		private int MaxAuthTagSize(IntPtr hAlg)
		{
			byte[] property = this.GetProperty(hAlg, cBCrypt.BCRYPT_AUTH_TAG_LENGTH);
			return BitConverter.ToInt32(new byte[]
			{
				property[4],
				property[5],
				property[6],
				property[7]
			}, 0);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000034F8 File Offset: 0x000016F8
		private IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = cBCrypt.BCryptOpenAlgorithmProvider(out zero, alg, provider, 0U);
			bool flag = num > 0U;
			if (flag)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{0}", num));
			}
			byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
			num = cBCrypt.BCryptSetAlgorithmProperty(zero, cBCrypt.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
			bool flag2 = num > 0U;
			if (flag2)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptSetAlgorithmProperty(BCrypt.BCRYPT_CHAINING_MODE, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{0}", num));
			}
			return zero;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003588 File Offset: 0x00001788
		private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
		{
			byte[] property = this.GetProperty(hAlg, cBCrypt.BCRYPT_OBJECT_LENGTH);
			int num = BitConverter.ToInt32(property, 0);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			byte[] array = this.Concat(new byte[][]
			{
				cBCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC,
				BitConverter.GetBytes(1),
				BitConverter.GetBytes(key.Length),
				key
			});
			uint num2 = cBCrypt.BCryptImportKey(hAlg, IntPtr.Zero, cBCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U);
			bool flag = num2 > 0U;
			if (flag)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptImportKey() failed with status code:{0}", num2));
			}
			return intPtr;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000362C File Offset: 0x0000182C
		private byte[] GetProperty(IntPtr hAlg, string name)
		{
			int num = 0;
			uint num2 = cBCrypt.BCryptGetProperty(hAlg, name, null, 0, ref num, 0U);
			bool flag = num2 > 0U;
			if (flag)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() (get size) failed with status code:{0}", num2));
			}
			byte[] array = new byte[num];
			num2 = cBCrypt.BCryptGetProperty(hAlg, name, array, array.Length, ref num, 0U);
			bool flag2 = num2 > 0U;
			if (flag2)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() failed with status code:{0}", num2));
			}
			return array;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000036B4 File Offset: 0x000018B4
		public byte[] Concat(params byte[][] arrays)
		{
			int num = 0;
			foreach (byte[] array in arrays)
			{
				bool flag = array == null;
				if (!flag)
				{
					num += array.Length;
				}
			}
			byte[] array2 = new byte[num - 1 + 1];
			int num2 = 0;
			foreach (byte[] array3 in arrays)
			{
				bool flag2 = array3 == null;
				if (!flag2)
				{
					Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
					num2 += array3.Length;
				}
			}
			return array2;
		}
	}
}
