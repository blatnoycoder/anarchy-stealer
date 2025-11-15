using System;
using System.Runtime.InteropServices;
using Plugin;

namespace Utils
{
	// Token: 0x02000003 RID: 3
	internal class MiscUtils
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000209C File Offset: 0x0000029C
		public static void BCRYPT_INIT_AUTH_MODE_INFO(out BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO _AUTH_INFO_STRUCT_)
		{
			_AUTH_INFO_STRUCT_ = default(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
			_AUTH_INFO_STRUCT_.cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
			_AUTH_INFO_STRUCT_.dwInfoVersion = 1U;
		}
	}
}
