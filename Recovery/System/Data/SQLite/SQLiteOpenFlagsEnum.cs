using System;

namespace System.Data.SQLite
{
	// Token: 0x02000151 RID: 337
	[Flags]
	internal enum SQLiteOpenFlagsEnum
	{
		// Token: 0x04000560 RID: 1376
		None = 0,
		// Token: 0x04000561 RID: 1377
		ReadOnly = 1,
		// Token: 0x04000562 RID: 1378
		ReadWrite = 2,
		// Token: 0x04000563 RID: 1379
		Create = 4,
		// Token: 0x04000564 RID: 1380
		Uri = 64,
		// Token: 0x04000565 RID: 1381
		Memory = 128,
		// Token: 0x04000566 RID: 1382
		Default = 6
	}
}
