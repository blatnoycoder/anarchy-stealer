using System;

namespace System.Data.SQLite
{
	// Token: 0x020001E4 RID: 484
	internal static class SQLiteSessionHelpers
	{
		// Token: 0x060015E1 RID: 5601 RVA: 0x00062C54 File Offset: 0x00060E54
		public static void CheckRawData(byte[] rawData)
		{
			if (rawData == null)
			{
				throw new ArgumentNullException("rawData");
			}
			if (rawData.Length == 0)
			{
				throw new ArgumentException("empty change set data", "rawData");
			}
		}
	}
}
