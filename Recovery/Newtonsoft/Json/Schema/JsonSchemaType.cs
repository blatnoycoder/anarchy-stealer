using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000E5 RID: 229
	[Flags]
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum JsonSchemaType
	{
		// Token: 0x0400040F RID: 1039
		None = 0,
		// Token: 0x04000410 RID: 1040
		String = 1,
		// Token: 0x04000411 RID: 1041
		Float = 2,
		// Token: 0x04000412 RID: 1042
		Integer = 4,
		// Token: 0x04000413 RID: 1043
		Boolean = 8,
		// Token: 0x04000414 RID: 1044
		Object = 16,
		// Token: 0x04000415 RID: 1045
		Array = 32,
		// Token: 0x04000416 RID: 1046
		Null = 64,
		// Token: 0x04000417 RID: 1047
		Any = 127
	}
}
