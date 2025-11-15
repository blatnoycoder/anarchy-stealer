using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000134 RID: 308
	[NullableContext(2)]
	[Nullable(0)]
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0003BB74 File Offset: 0x00039D74
		[Nullable(1)]
		private XAttribute Attribute
		{
			[NullableContext(1)]
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003BB84 File Offset: 0x00039D84
		[NullableContext(1)]
		public XAttributeWrapper(XAttribute attribute)
			: base(attribute)
		{
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x0003BB90 File Offset: 0x00039D90
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x0003BBA0 File Offset: 0x00039DA0
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x0003BBB0 File Offset: 0x00039DB0
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0003BBC4 File Offset: 0x00039DC4
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x0003BBD8 File Offset: 0x00039DD8
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
