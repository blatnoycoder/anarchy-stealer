using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200008B RID: 139
	[NullableContext(1)]
	[Nullable(0)]
	internal class EnumInfo
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0001AB94 File Offset: 0x00018D94
		public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
		{
			this.IsFlags = isFlags;
			this.Values = values;
			this.Names = names;
			this.ResolvedNames = resolvedNames;
		}

		// Token: 0x0400028A RID: 650
		public readonly bool IsFlags;

		// Token: 0x0400028B RID: 651
		public readonly ulong[] Values;

		// Token: 0x0400028C RID: 652
		public readonly string[] Names;

		// Token: 0x0400028D RID: 653
		public readonly string[] ResolvedNames;
	}
}
