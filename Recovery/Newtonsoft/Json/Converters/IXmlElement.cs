using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200012A RID: 298
	[NullableContext(1)]
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x06000CA4 RID: 3236
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x06000CA5 RID: 3237
		string GetPrefixOfNamespace(string namespaceUri);

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CA6 RID: 3238
		bool IsEmpty { get; }
	}
}
