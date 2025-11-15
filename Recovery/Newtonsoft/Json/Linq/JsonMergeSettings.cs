using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000F8 RID: 248
	public class JsonMergeSettings
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00032560 File Offset: 0x00030760
		public JsonMergeSettings()
		{
			this._propertyNameComparison = StringComparison.Ordinal;
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x00032570 File Offset: 0x00030770
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x00032578 File Offset: 0x00030778
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0003259C File Offset: 0x0003079C
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x000325A4 File Offset: 0x000307A4
		public MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x000325C8 File Offset: 0x000307C8
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x000325D0 File Offset: 0x000307D0
		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		// Token: 0x0400043A RID: 1082
		private MergeArrayHandling _mergeArrayHandling;

		// Token: 0x0400043B RID: 1083
		private MergeNullValueHandling _mergeNullValueHandling;

		// Token: 0x0400043C RID: 1084
		private StringComparison _propertyNameComparison;
	}
}
