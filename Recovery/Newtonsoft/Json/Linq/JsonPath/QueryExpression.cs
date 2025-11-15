using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200010A RID: 266
	internal abstract class QueryExpression
	{
		// Token: 0x06000BE5 RID: 3045 RVA: 0x000383B4 File Offset: 0x000365B4
		public QueryExpression(QueryOperator @operator)
		{
			this.Operator = @operator;
		}

		// Token: 0x06000BE6 RID: 3046
		[NullableContext(1)]
		public abstract bool IsMatch(JToken root, JToken t);

		// Token: 0x0400048D RID: 1165
		internal QueryOperator Operator;
	}
}
