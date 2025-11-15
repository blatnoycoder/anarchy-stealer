using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200012C RID: 300
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0003B5A0 File Offset: 0x000397A0
		internal XDeclaration Declaration { get; }

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0003B5A8 File Offset: 0x000397A8
		public XDeclarationWrapper(XDeclaration declaration)
			: base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0003B5B8 File Offset: 0x000397B8
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0003B5BC File Offset: 0x000397BC
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0003B5CC File Offset: 0x000397CC
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x0003B5DC File Offset: 0x000397DC
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0003B5EC File Offset: 0x000397EC
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x0003B5FC File Offset: 0x000397FC
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
