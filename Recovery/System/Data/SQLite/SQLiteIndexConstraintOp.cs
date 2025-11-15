using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C5 RID: 453
	public enum SQLiteIndexConstraintOp : byte
	{
		// Token: 0x0400087B RID: 2171
		EqualTo = 2,
		// Token: 0x0400087C RID: 2172
		GreaterThan = 4,
		// Token: 0x0400087D RID: 2173
		LessThanOrEqualTo = 8,
		// Token: 0x0400087E RID: 2174
		LessThan = 16,
		// Token: 0x0400087F RID: 2175
		GreaterThanOrEqualTo = 32,
		// Token: 0x04000880 RID: 2176
		Match = 64,
		// Token: 0x04000881 RID: 2177
		Like,
		// Token: 0x04000882 RID: 2178
		Glob,
		// Token: 0x04000883 RID: 2179
		Regexp,
		// Token: 0x04000884 RID: 2180
		NotEqualTo,
		// Token: 0x04000885 RID: 2181
		IsNot,
		// Token: 0x04000886 RID: 2182
		IsNotNull,
		// Token: 0x04000887 RID: 2183
		IsNull,
		// Token: 0x04000888 RID: 2184
		Is
	}
}
