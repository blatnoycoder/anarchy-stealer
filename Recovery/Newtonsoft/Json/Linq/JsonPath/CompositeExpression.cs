using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200010B RID: 267
	[NullableContext(1)]
	[Nullable(0)]
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x000383C4 File Offset: 0x000365C4
		// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x000383CC File Offset: 0x000365CC
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x06000BE9 RID: 3049 RVA: 0x000383D8 File Offset: 0x000365D8
		public CompositeExpression(QueryOperator @operator)
			: base(@operator)
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000383EC File Offset: 0x000365EC
		public override bool IsMatch(JToken root, JToken t)
		{
			QueryOperator @operator = this.Operator;
			if (@operator == QueryOperator.And)
			{
				using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsMatch(root, t))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (@operator != QueryOperator.Or)
			{
				throw new ArgumentOutOfRangeException();
			}
			using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMatch(root, t))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
