using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000130 RID: 304
	[NullableContext(2)]
	[Nullable(0)]
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0003B89C File Offset: 0x00039A9C
		[Nullable(1)]
		private XComment Text
		{
			[NullableContext(1)]
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003B8AC File Offset: 0x00039AAC
		[NullableContext(1)]
		public XCommentWrapper(XComment text)
			: base(text)
		{
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0003B8B8 File Offset: 0x00039AB8
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0003B8C8 File Offset: 0x00039AC8
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0003B8D8 File Offset: 0x00039AD8
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
