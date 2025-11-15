using System;

namespace System.Data.SQLite
{
	// Token: 0x0200017F RID: 383
	public interface ISQLiteConnectionPool2 : ISQLiteConnectionPool
	{
		// Token: 0x06001100 RID: 4352
		void Initialize(object argument);

		// Token: 0x06001101 RID: 4353
		void Terminate(object argument);

		// Token: 0x06001102 RID: 4354
		void GetCounts(ref int openCount, ref int closeCount);

		// Token: 0x06001103 RID: 4355
		void ResetCounts();
	}
}
