using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000F7 RID: 247
	public class JsonLoadSettings
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x000324BC File Offset: 0x000306BC
		public JsonLoadSettings()
		{
			this._lineInfoHandling = LineInfoHandling.Load;
			this._commentHandling = CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x000324DC File Offset: 0x000306DC
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x000324E4 File Offset: 0x000306E4
		public CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < CommentHandling.Ignore || value > CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00032508 File Offset: 0x00030708
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00032510 File Offset: 0x00030710
		public LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00032534 File Offset: 0x00030734
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0003253C File Offset: 0x0003073C
		public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < DuplicatePropertyNameHandling.Replace || value > DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		// Token: 0x04000437 RID: 1079
		private CommentHandling _commentHandling;

		// Token: 0x04000438 RID: 1080
		private LineInfoHandling _lineInfoHandling;

		// Token: 0x04000439 RID: 1081
		private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;
	}
}
