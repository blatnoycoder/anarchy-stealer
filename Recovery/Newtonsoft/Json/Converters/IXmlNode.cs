using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200012B RID: 299
	[NullableContext(2)]
	internal interface IXmlNode
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000CA7 RID: 3239
		XmlNodeType NodeType { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000CA8 RID: 3240
		string LocalName { get; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000CA9 RID: 3241
		[Nullable(1)]
		List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000CAA RID: 3242
		[Nullable(1)]
		List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000CAB RID: 3243
		IXmlNode ParentNode { get; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000CAC RID: 3244
		// (set) Token: 0x06000CAD RID: 3245
		string Value { get; set; }

		// Token: 0x06000CAE RID: 3246
		[NullableContext(1)]
		IXmlNode AppendChild(IXmlNode newChild);

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000CAF RID: 3247
		string NamespaceUri { get; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000CB0 RID: 3248
		object WrappedNode { get; }
	}
}
