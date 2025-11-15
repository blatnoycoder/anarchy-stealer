using System;
using System.Runtime.InteropServices;

namespace Plugin
{
	// Token: 0x0200002A RID: 42
	public class BCrypt
	{
		// Token: 0x060000B5 RID: 181
		[DllImport("BCrypt", ExactSpelling = true, SetLastError = true)]
		public static extern uint BCryptDestroyKey(IntPtr hKey);

		// Token: 0x060000B6 RID: 182
		[DllImport("BCrypt", ExactSpelling = true, SetLastError = true)]
		public static extern uint BCryptCloseAlgorithmProvider(IntPtr algorithmHandle, BCrypt.BCryptCloseAlgorithmProviderFlags flags = BCrypt.BCryptCloseAlgorithmProviderFlags.None);

		// Token: 0x060000B7 RID: 183
		[DllImport("BCrypt", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern uint BCryptOpenAlgorithmProvider(out BCrypt.SafeAlgorithmHandle phAlgorithm, string pszAlgId, string pszImplementation, BCrypt.BCryptOpenAlgorithmProviderFlags dwFlags);

		// Token: 0x060000B8 RID: 184
		[DllImport("BCrypt", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern uint BCryptSetProperty(SafeHandle hObject, string pszProperty, string pbInput, int cbInput, BCrypt.BCryptSetPropertyFlags dwFlags = BCrypt.BCryptSetPropertyFlags.None);

		// Token: 0x060000B9 RID: 185
		[DllImport("BCrypt", SetLastError = true)]
		public static extern uint BCryptGenerateSymmetricKey(BCrypt.SafeAlgorithmHandle hAlgorithm, out BCrypt.SafeKeyHandle phKey, byte[] pbKeyObject, int cbKeyObject, byte[] pbSecret, int cbSecret, BCrypt.BCryptGenerateSymmetricKeyFlags flags = BCrypt.BCryptGenerateSymmetricKeyFlags.None);

		// Token: 0x060000BA RID: 186
		[DllImport("BCrypt", SetLastError = true)]
		public unsafe static extern uint BCryptDecrypt(BCrypt.SafeKeyHandle hKey, byte* pbInput, int cbInput, void* pPaddingInfo, byte* pbIV, int cbIV, byte* pbOutput, int cbOutput, out int pcbResult, BCrypt.BCryptEncryptFlags dwFlags);

		// Token: 0x060000BB RID: 187 RVA: 0x00007A08 File Offset: 0x00005C08
		public static void BCRYPT_INIT_AUTH_MODE_INFO(out BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO _AUTH_INFO_STRUCT_)
		{
			_AUTH_INFO_STRUCT_ = default(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
			_AUTH_INFO_STRUCT_.cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
			_AUTH_INFO_STRUCT_.dwInfoVersion = 1U;
		}

		// Token: 0x0200020A RID: 522
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
		{
			// Token: 0x060016B5 RID: 5813 RVA: 0x000655F0 File Offset: 0x000637F0
			public static BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO Create()
			{
				return new BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
				{
					cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO)),
					dwInfoVersion = 1U
				};
			}

			// Token: 0x0400097E RID: 2430
			public const uint BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO_VERSION = 1U;

			// Token: 0x0400097F RID: 2431
			public int cbSize;

			// Token: 0x04000980 RID: 2432
			public uint dwInfoVersion;

			// Token: 0x04000981 RID: 2433
			public unsafe byte* pbNonce;

			// Token: 0x04000982 RID: 2434
			public int cbNonce;

			// Token: 0x04000983 RID: 2435
			public unsafe byte* pbAuthData;

			// Token: 0x04000984 RID: 2436
			public int cbAuthData;

			// Token: 0x04000985 RID: 2437
			public unsafe byte* pbTag;

			// Token: 0x04000986 RID: 2438
			public int cbTag;

			// Token: 0x04000987 RID: 2439
			public unsafe byte* pbMacContext;

			// Token: 0x04000988 RID: 2440
			public int cbMacContext;

			// Token: 0x04000989 RID: 2441
			public int cbAAD;

			// Token: 0x0400098A RID: 2442
			public long cbData;

			// Token: 0x0400098B RID: 2443
			public BCrypt.AuthModeFlags dwFlags;
		}

		// Token: 0x0200020B RID: 523
		[Flags]
		public enum AuthModeFlags
		{
			// Token: 0x0400098D RID: 2445
			None = 0,
			// Token: 0x0400098E RID: 2446
			BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1,
			// Token: 0x0400098F RID: 2447
			BCRYPT_AUTH_MODE_IN_PROGRESS_FLAG = 2
		}

		// Token: 0x0200020C RID: 524
		[Flags]
		public enum BCryptCloseAlgorithmProviderFlags
		{
			// Token: 0x04000991 RID: 2449
			None = 0
		}

		// Token: 0x0200020D RID: 525
		[Flags]
		public enum BCryptOpenAlgorithmProviderFlags
		{
			// Token: 0x04000993 RID: 2451
			None = 0,
			// Token: 0x04000994 RID: 2452
			BCRYPT_ALG_HANDLE_HMAC_FLAG = 8,
			// Token: 0x04000995 RID: 2453
			BCRYPT_HASH_REUSABLE_FLAG = 32,
			// Token: 0x04000996 RID: 2454
			BCRYPT_MULTI_FLAG = 64
		}

		// Token: 0x0200020E RID: 526
		public enum BCryptSetPropertyFlags
		{
			// Token: 0x04000998 RID: 2456
			None
		}

		// Token: 0x0200020F RID: 527
		[Flags]
		public enum BCryptGenerateSymmetricKeyFlags
		{
			// Token: 0x0400099A RID: 2458
			None = 0
		}

		// Token: 0x02000210 RID: 528
		[Flags]
		public enum BCryptEncryptFlags
		{
			// Token: 0x0400099C RID: 2460
			None = 0,
			// Token: 0x0400099D RID: 2461
			BCRYPT_BLOCK_PADDING = 1,
			// Token: 0x0400099E RID: 2462
			BCRYPT_PAD_NONE = 1,
			// Token: 0x0400099F RID: 2463
			BCRYPT_PAD_PKCS1 = 2,
			// Token: 0x040009A0 RID: 2464
			BCRYPT_PAD_OAEP = 4
		}

		// Token: 0x02000211 RID: 529
		public class SafeKeyHandle : SafeHandle
		{
			// Token: 0x060016B6 RID: 5814 RVA: 0x00065630 File Offset: 0x00063830
			public SafeKeyHandle()
				: base(IntPtr.Zero, true)
			{
			}

			// Token: 0x060016B7 RID: 5815 RVA: 0x00065640 File Offset: 0x00063840
			public SafeKeyHandle(IntPtr preexistingHandle, bool ownsHandle = true)
				: base(IntPtr.Zero, ownsHandle)
			{
				base.SetHandle(preexistingHandle);
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00065658 File Offset: 0x00063858
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero;
				}
			}

			// Token: 0x060016B9 RID: 5817 RVA: 0x0006566C File Offset: 0x0006386C
			protected override bool ReleaseHandle()
			{
				return BCrypt.BCryptDestroyKey(this.handle) == 0U;
			}

			// Token: 0x040009A1 RID: 2465
			public static readonly BCrypt.SafeKeyHandle Null = new BCrypt.SafeKeyHandle();
		}

		// Token: 0x02000212 RID: 530
		public class SafeAlgorithmHandle : SafeHandle
		{
			// Token: 0x060016BB RID: 5819 RVA: 0x000656A0 File Offset: 0x000638A0
			public SafeAlgorithmHandle()
				: base(IntPtr.Zero, true)
			{
			}

			// Token: 0x060016BC RID: 5820 RVA: 0x000656B0 File Offset: 0x000638B0
			public SafeAlgorithmHandle(IntPtr preexistingHandle, bool ownsHandle = true)
				: base(IntPtr.Zero, ownsHandle)
			{
				base.SetHandle(preexistingHandle);
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x060016BD RID: 5821 RVA: 0x000656C8 File Offset: 0x000638C8
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero;
				}
			}

			// Token: 0x060016BE RID: 5822 RVA: 0x000656DC File Offset: 0x000638DC
			protected override bool ReleaseHandle()
			{
				return BCrypt.BCryptCloseAlgorithmProvider(this.handle, BCrypt.BCryptCloseAlgorithmProviderFlags.None) == 0U;
			}

			// Token: 0x040009A2 RID: 2466
			public static readonly BCrypt.SafeAlgorithmHandle Null = new BCrypt.SafeAlgorithmHandle();
		}
	}
}
