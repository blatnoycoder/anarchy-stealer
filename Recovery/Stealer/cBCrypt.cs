using System;
using System.Runtime.InteropServices;

namespace Stealer
{
	// Token: 0x0200000B RID: 11
	public static class cBCrypt
	{
		// Token: 0x0600002E RID: 46
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

		// Token: 0x0600002F RID: 47
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

		// Token: 0x06000030 RID: 48
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

		// Token: 0x06000031 RID: 49
		[DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
		internal static extern uint BCryptSetAlgorithmProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

		// Token: 0x06000032 RID: 50
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

		// Token: 0x06000033 RID: 51
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptDestroyKey(IntPtr hKey);

		// Token: 0x06000034 RID: 52
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptEncrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, uint dwFlags);

		// Token: 0x06000035 RID: 53
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

		// Token: 0x04000029 RID: 41
		public const uint ERROR_SUCCESS = 0U;

		// Token: 0x0400002A RID: 42
		public const uint BCRYPT_PAD_PSS = 8U;

		// Token: 0x0400002B RID: 43
		public const uint BCRYPT_PAD_OAEP = 4U;

		// Token: 0x0400002C RID: 44
		public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

		// Token: 0x0400002D RID: 45
		public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";

		// Token: 0x0400002E RID: 46
		public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

		// Token: 0x0400002F RID: 47
		public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

		// Token: 0x04000030 RID: 48
		public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";

		// Token: 0x04000031 RID: 49
		public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";

		// Token: 0x04000032 RID: 50
		public static readonly string BCRYPT_AES_ALGORITHM = "AES";

		// Token: 0x04000033 RID: 51
		public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

		// Token: 0x04000034 RID: 52
		public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

		// Token: 0x04000035 RID: 53
		public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

		// Token: 0x04000036 RID: 54
		public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

		// Token: 0x020001F6 RID: 502
		public struct BCRYPT_PSS_PADDING_INFO
		{
			// Token: 0x0600167D RID: 5757 RVA: 0x00065288 File Offset: 0x00063488
			public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
			{
				this.pszAlgId = pszAlgId;
				this.cbSalt = cbSalt;
			}

			// Token: 0x04000919 RID: 2329
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x0400091A RID: 2330
			public int cbSalt;
		}

		// Token: 0x020001F7 RID: 503
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
		{
			// Token: 0x0600167E RID: 5758 RVA: 0x0006529C File Offset: 0x0006349C
			public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
			{
				this = default(cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
				this.dwInfoVersion = cBCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
				this.cbSize = Marshal.SizeOf(typeof(cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
				bool flag = iv != null;
				if (flag)
				{
					this.cbNonce = iv.Length;
					this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
					Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
				}
				bool flag2 = aad != null;
				if (flag2)
				{
					this.cbAuthData = aad.Length;
					this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
					Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
				}
				bool flag3 = tag != null;
				if (flag3)
				{
					this.cbTag = tag.Length;
					this.pbTag = Marshal.AllocHGlobal(this.cbTag);
					Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
					this.cbMacContext = tag.Length;
					this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
				}
			}

			// Token: 0x0600167F RID: 5759 RVA: 0x000653A0 File Offset: 0x000635A0
			public void Dispose()
			{
				bool flag = this.pbNonce != IntPtr.Zero;
				if (flag)
				{
					Marshal.FreeHGlobal(this.pbNonce);
				}
				bool flag2 = this.pbTag != IntPtr.Zero;
				if (flag2)
				{
					Marshal.FreeHGlobal(this.pbTag);
				}
				bool flag3 = this.pbAuthData != IntPtr.Zero;
				if (flag3)
				{
					Marshal.FreeHGlobal(this.pbAuthData);
				}
				bool flag4 = this.pbMacContext != IntPtr.Zero;
				if (flag4)
				{
					Marshal.FreeHGlobal(this.pbMacContext);
				}
			}

			// Token: 0x0400091B RID: 2331
			public int cbSize;

			// Token: 0x0400091C RID: 2332
			public int dwInfoVersion;

			// Token: 0x0400091D RID: 2333
			public IntPtr pbNonce;

			// Token: 0x0400091E RID: 2334
			public int cbNonce;

			// Token: 0x0400091F RID: 2335
			public IntPtr pbAuthData;

			// Token: 0x04000920 RID: 2336
			public int cbAuthData;

			// Token: 0x04000921 RID: 2337
			public IntPtr pbTag;

			// Token: 0x04000922 RID: 2338
			public int cbTag;

			// Token: 0x04000923 RID: 2339
			public IntPtr pbMacContext;

			// Token: 0x04000924 RID: 2340
			public int cbMacContext;

			// Token: 0x04000925 RID: 2341
			public int cbAAD;

			// Token: 0x04000926 RID: 2342
			public long cbData;

			// Token: 0x04000927 RID: 2343
			public int dwFlags;
		}

		// Token: 0x020001F8 RID: 504
		public struct BCRYPT_KEY_LENGTHS_STRUCT
		{
			// Token: 0x04000928 RID: 2344
			public int dwMinLength;

			// Token: 0x04000929 RID: 2345
			public int dwMaxLength;

			// Token: 0x0400092A RID: 2346
			public int dwIncrement;
		}

		// Token: 0x020001F9 RID: 505
		public struct BCRYPT_OAEP_PADDING_INFO
		{
			// Token: 0x06001680 RID: 5760 RVA: 0x00065440 File Offset: 0x00063640
			public BCRYPT_OAEP_PADDING_INFO(string alg)
			{
				this.pszAlgId = alg;
				this.pbLabel = IntPtr.Zero;
				this.cbLabel = 0;
			}

			// Token: 0x0400092B RID: 2347
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x0400092C RID: 2348
			public IntPtr pbLabel;

			// Token: 0x0400092D RID: 2349
			public int cbLabel;
		}
	}
}
