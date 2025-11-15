using System;

namespace System.Data.SQLite
{
	// Token: 0x02000161 RID: 353
	// (Invoke) Token: 0x06000FC9 RID: 4041
	public delegate void SQLiteReadValueCallback(SQLiteConvert convert, SQLiteDataReader dataReader, SQLiteConnectionFlags flags, SQLiteReadEventArgs eventArgs, string typeName, int index, object userData, out bool complete);
}
