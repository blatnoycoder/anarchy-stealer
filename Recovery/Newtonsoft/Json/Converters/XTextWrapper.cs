using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200012F RID: 303
	[NullableContext(2)]
	[Nullable(0)]
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0003B83C File Offset: 0x00039A3C
		[Nullable(1)]
		private XText Text
		{
			[NullableContext(1)]
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0003B84C File Offset: 0x00039A4C
		[NullableContext(1)]
		public XTextWrapper(XText text)
			: base(text)
		{
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0003B858 File Offset: 0x00039A58
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x0003B868 File Offset: 0x00039A68
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

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0003B878 File Offset: 0x00039A78
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
