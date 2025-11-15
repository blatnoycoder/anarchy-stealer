using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000124 RID: 292
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06000C74 RID: 3188 RVA: 0x0003B1CC File Offset: 0x000393CC
		public XmlDeclarationWrapper(XmlDeclaration declaration)
			: base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0003B1DC File Offset: 0x000393DC
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0003B1EC File Offset: 0x000393EC
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0003B1FC File Offset: 0x000393FC
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0003B20C File Offset: 0x0003940C
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x0003B21C File Offset: 0x0003941C
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x040004B0 RID: 1200
		private readonly XmlDeclaration _declaration;
	}
}
