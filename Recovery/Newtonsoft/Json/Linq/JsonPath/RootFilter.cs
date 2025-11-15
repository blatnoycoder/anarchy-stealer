using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200010F RID: 271
	[NullableContext(1)]
	[Nullable(0)]
	internal class RootFilter : PathFilter
	{
		// Token: 0x06000BF6 RID: 3062 RVA: 0x000389E4 File Offset: 0x00036BE4
		private RootFilter()
		{
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000389EC File Offset: 0x00036BEC
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new JToken[] { root };
		}

		// Token: 0x04000493 RID: 1171
		public static readonly RootFilter Instance = new RootFilter();
	}
}
