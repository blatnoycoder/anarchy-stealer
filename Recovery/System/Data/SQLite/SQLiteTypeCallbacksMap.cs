using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000163 RID: 355
	internal sealed class SQLiteTypeCallbacksMap : Dictionary<string, SQLiteTypeCallbacks>
	{
		// Token: 0x06000FD4 RID: 4052 RVA: 0x00048F10 File Offset: 0x00047110
		public SQLiteTypeCallbacksMap()
			: base(new TypeNameStringComparer())
		{
		}
	}
}
