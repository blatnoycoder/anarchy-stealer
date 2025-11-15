using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000133 RID: 307
	[NullableContext(2)]
	[Nullable(0)]
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x06000CE7 RID: 3303 RVA: 0x0003BB14 File Offset: 0x00039D14
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0003BB24 File Offset: 0x00039D24
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0003BB2C File Offset: 0x00039D2C
		public virtual XmlNodeType NodeType
		{
			get
			{
				XObject xmlObject = this._xmlObject;
				if (xmlObject == null)
				{
					return XmlNodeType.None;
				}
				return xmlObject.NodeType;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0003BB44 File Offset: 0x00039D44
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0003BB48 File Offset: 0x00039D48
		[Nullable(1)]
		public virtual List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0003BB50 File Offset: 0x00039D50
		[Nullable(1)]
		public virtual List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x0003BB58 File Offset: 0x00039D58
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0003BB5C File Offset: 0x00039D5C
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0003BB60 File Offset: 0x00039D60
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003BB68 File Offset: 0x00039D68
		[NullableContext(1)]
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0003BB70 File Offset: 0x00039D70
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040004B8 RID: 1208
		private readonly XObject _xmlObject;
	}
}
