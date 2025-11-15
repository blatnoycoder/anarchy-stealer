using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000129 RID: 297
	[NullableContext(1)]
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CA0 RID: 3232
		string Name { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CA1 RID: 3233
		string System { get; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CA2 RID: 3234
		string Public { get; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CA3 RID: 3235
		string InternalSubset { get; }
	}
}
