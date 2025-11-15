using System;

namespace System.Data.SQLite
{
	// Token: 0x02000160 RID: 352
	// (Invoke) Token: 0x06000FC5 RID: 4037
	public delegate void SQLiteBindValueCallback(SQLiteConvert convert, SQLiteCommand command, SQLiteConnectionFlags flags, SQLiteParameter parameter, string typeName, int index, object userData, out bool complete);
}
