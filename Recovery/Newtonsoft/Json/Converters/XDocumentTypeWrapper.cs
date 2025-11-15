using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200012D RID: 301
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000CB9 RID: 3257 RVA: 0x0003B60C File Offset: 0x0003980C
		public XDocumentTypeWrapper(XDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0003B61C File Offset: 0x0003981C
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0003B62C File Offset: 0x0003982C
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0003B63C File Offset: 0x0003983C
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0003B64C File Offset: 0x0003984C
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0003B65C File Offset: 0x0003985C
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040004B6 RID: 1206
		private readonly XDocumentType _documentType;
	}
}
