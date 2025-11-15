using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000126 RID: 294
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlNodeWrapper : IXmlNode
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x0003B284 File Offset: 0x00039484
		[NullableContext(1)]
		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x0003B294 File Offset: 0x00039494
		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0003B29C File Offset: 0x0003949C
		public XmlNodeType NodeType
		{
			get
			{
				return this._node.NodeType;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0003B2AC File Offset: 0x000394AC
		public virtual string LocalName
		{
			get
			{
				return this._node.LocalName;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0003B2BC File Offset: 0x000394BC
		[Nullable(1)]
		public List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				if (this._childNodes == null)
				{
					if (!this._node.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
						foreach (object obj in this._node.ChildNodes)
						{
							XmlNode xmlNode = (XmlNode)obj;
							this._childNodes.Add(XmlNodeWrapper.WrapNode(xmlNode));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0003B37C File Offset: 0x0003957C
		protected virtual bool HasChildNodes
		{
			get
			{
				return this._node.HasChildNodes;
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0003B38C File Offset: 0x0003958C
		[NullableContext(1)]
		internal static IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType == XmlNodeType.DocumentType)
			{
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			}
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0003B3EC File Offset: 0x000395EC
		[Nullable(1)]
		public List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				if (this._attributes == null)
				{
					if (!this.HasAttributes)
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
						foreach (object obj in this._node.Attributes)
						{
							XmlAttribute xmlAttribute = (XmlAttribute)obj;
							this._attributes.Add(XmlNodeWrapper.WrapNode(xmlAttribute));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0003B4A4 File Offset: 0x000396A4
		private bool HasAttributes
		{
			get
			{
				XmlElement xmlElement = this._node as XmlElement;
				if (xmlElement != null)
				{
					return xmlElement.HasAttributes;
				}
				XmlAttributeCollection attributes = this._node.Attributes;
				return attributes != null && attributes.Count > 0;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0003B4EC File Offset: 0x000396EC
		public IXmlNode ParentNode
		{
			get
			{
				XmlAttribute xmlAttribute = this._node as XmlAttribute;
				XmlNode xmlNode = ((xmlAttribute != null) ? xmlAttribute.OwnerElement : this._node.ParentNode);
				if (xmlNode == null)
				{
					return null;
				}
				return XmlNodeWrapper.WrapNode(xmlNode);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x0003B534 File Offset: 0x00039734
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x0003B544 File Offset: 0x00039744
		public string Value
		{
			get
			{
				return this._node.Value;
			}
			set
			{
				this._node.Value = value;
			}
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003B554 File Offset: 0x00039754
		[NullableContext(1)]
		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			this._childNodes = null;
			this._attributes = null;
			return newChild;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003B590 File Offset: 0x00039790
		public string NamespaceUri
		{
			get
			{
				return this._node.NamespaceURI;
			}
		}

		// Token: 0x040004B2 RID: 1202
		[Nullable(1)]
		private readonly XmlNode _node;

		// Token: 0x040004B3 RID: 1203
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _childNodes;

		// Token: 0x040004B4 RID: 1204
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _attributes;
	}
}
