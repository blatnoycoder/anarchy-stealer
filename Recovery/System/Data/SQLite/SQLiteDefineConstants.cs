using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000197 RID: 407
	internal static class SQLiteDefineConstants
	{
		// Token: 0x060011F0 RID: 4592 RVA: 0x00055E74 File Offset: 0x00054074
		// Note: this type is marked as 'beforefieldinit'.
		static SQLiteDefineConstants()
		{
			string[] array = new string[23];
			array[0] = "INCLUDE_EXTRA";
			array[1] = "INTEROP_CRYPTOAPI_EXTENSION";
			array[2] = "INTEROP_EXTENSION_FUNCTIONS";
			array[3] = "INTEROP_FTS5_EXTENSION";
			array[4] = "INTEROP_INCLUDE_SEE";
			array[5] = "INTEROP_JSON1_EXTENSION";
			array[6] = "INTEROP_PERCENTILE_EXTENSION";
			array[7] = "INTEROP_REGEXP_EXTENSION";
			array[8] = "INTEROP_SESSION_EXTENSION";
			array[9] = "INTEROP_SHA1_EXTENSION";
			array[10] = "INTEROP_TOTYPE_EXTENSION";
			array[11] = "INTEROP_VIRTUAL_TABLE";
			array[12] = "NET_40";
			array[13] = "PRELOAD_NATIVE_LIBRARY";
			array[14] = "THROW_ON_DISPOSED";
			array[15] = "TRACE";
			array[16] = "TRACE_PRELOAD";
			array[17] = "TRACE_SHARED";
			array[18] = "TRACE_WARNING";
			array[19] = "USE_INTEROP_DLL";
			array[20] = "USE_PREPARE_V2";
			array[21] = "WINDOWS";
			SQLiteDefineConstants.OptionList = new List<string>(array);
		}

		// Token: 0x04000740 RID: 1856
		public static readonly IList<string> OptionList;
	}
}
