using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x02000104 RID: 260
	internal class ArraySliceFilter : PathFilter
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00036DF4 File Offset: 0x00034FF4
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x00036DFC File Offset: 0x00034FFC
		public int? Start { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00036E08 File Offset: 0x00035008
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x00036E10 File Offset: 0x00035010
		public int? End { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00036E1C File Offset: 0x0003501C
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x00036E24 File Offset: 0x00035024
		public int? Step { get; set; }

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00036E30 File Offset: 0x00035030
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			int? num = this.Step;
			int num2 = 0;
			if ((num.GetValueOrDefault() == num2) & (num != null))
			{
				throw new JsonException("Step cannot be zero.");
			}
			foreach (JToken jtoken in current)
			{
				JArray a = jtoken as JArray;
				if (a != null)
				{
					int stepCount = this.Step ?? 1;
					int num3 = this.Start ?? ((stepCount > 0) ? 0 : (a.Count - 1));
					int stopIndex = this.End ?? ((stepCount > 0) ? a.Count : (-1));
					num = this.Start;
					num2 = 0;
					if ((num.GetValueOrDefault() < num2) & (num != null))
					{
						num3 = a.Count + num3;
					}
					num = this.End;
					num2 = 0;
					if ((num.GetValueOrDefault() < num2) & (num != null))
					{
						stopIndex = a.Count + stopIndex;
					}
					num3 = Math.Max(num3, (stepCount > 0) ? 0 : int.MinValue);
					num3 = Math.Min(num3, (stepCount > 0) ? a.Count : (a.Count - 1));
					stopIndex = Math.Max(stopIndex, -1);
					stopIndex = Math.Min(stopIndex, a.Count);
					bool positiveStep = stepCount > 0;
					if (this.IsValid(num3, stopIndex, positiveStep))
					{
						int i = num3;
						while (this.IsValid(i, stopIndex, positiveStep))
						{
							yield return a[i];
							i += stepCount;
						}
					}
					else if (errorWhenNoMatch)
					{
						throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith(CultureInfo.InvariantCulture, (this.Start != null) ? this.Start.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) : "*", (this.End != null) ? this.End.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) : "*"));
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Array slice is not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, jtoken.GetType().Name));
				}
				a = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00036E50 File Offset: 0x00035050
		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}
