using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000125 RID: 293
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000C7A RID: 3194 RVA: 0x0003B22C File Offset: 0x0003942C
		public XmlDocumentTypeWrapper(XmlDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0003B23C File Offset: 0x0003943C
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0003B24C File Offset: 0x0003944C
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0003B25C File Offset: 0x0003945C
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0003B26C File Offset: 0x0003946C
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0003B27C File Offset: 0x0003947C
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040004B1 RID: 1201
		private readonly XmlDocumentType _documentType;
	}
}
