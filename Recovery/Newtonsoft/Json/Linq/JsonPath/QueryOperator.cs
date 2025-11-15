using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x02000109 RID: 265
	internal enum QueryOperator
	{
		// Token: 0x04000480 RID: 1152
		None,
		// Token: 0x04000481 RID: 1153
		Equals,
		// Token: 0x04000482 RID: 1154
		NotEquals,
		// Token: 0x04000483 RID: 1155
		Exists,
		// Token: 0x04000484 RID: 1156
		LessThan,
		// Token: 0x04000485 RID: 1157
		LessThanOrEquals,
		// Token: 0x04000486 RID: 1158
		GreaterThan,
		// Token: 0x04000487 RID: 1159
		GreaterThanOrEquals,
		// Token: 0x04000488 RID: 1160
		And,
		// Token: 0x04000489 RID: 1161
		Or,
		// Token: 0x0400048A RID: 1162
		RegexEquals,
		// Token: 0x0400048B RID: 1163
		StrictEquals,
		// Token: 0x0400048C RID: 1164
		StrictNotEquals
	}
}
