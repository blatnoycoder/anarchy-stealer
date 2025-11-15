using System;

namespace System.Data.SQLite
{
	// Token: 0x020001D0 RID: 464
	internal static class SQLiteMemory
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x0005FE9C File Offset: 0x0005E09C
		private static bool CanUseSize64()
		{
			return UnsafeNativeMethods.sqlite3_libversion_number() >= 3008007;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0005FEB0 File Offset: 0x0005E0B0
		public static IntPtr Allocate(int size)
		{
			return UnsafeNativeMethods.sqlite3_malloc(size);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0005FECC File Offset: 0x0005E0CC
		public static IntPtr Allocate64(ulong size)
		{
			return UnsafeNativeMethods.sqlite3_malloc64(size);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0005FEE8 File Offset: 0x0005E0E8
		public static IntPtr AllocateUntracked(int size)
		{
			return UnsafeNativeMethods.sqlite3_malloc(size);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0005FEF0 File Offset: 0x0005E0F0
		public static IntPtr Allocate64Untracked(ulong size)
		{
			return UnsafeNativeMethods.sqlite3_malloc64(size);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0005FEF8 File Offset: 0x0005E0F8
		public static int Size(IntPtr pMemory)
		{
			return UnsafeNativeMethods.sqlite3_malloc_size_interop(pMemory);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0005FF00 File Offset: 0x0005E100
		public static ulong Size64(IntPtr pMemory)
		{
			return UnsafeNativeMethods.sqlite3_msize(pMemory);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0005FF08 File Offset: 0x0005E108
		public static void Free(IntPtr pMemory)
		{
			UnsafeNativeMethods.sqlite3_free(pMemory);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0005FF10 File Offset: 0x0005E110
		public static void FreeUntracked(IntPtr pMemory)
		{
			UnsafeNativeMethods.sqlite3_free(pMemory);
		}
	}
}
