using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000128 RID: 296
	[NullableContext(1)]
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000C9B RID: 3227
		string Version { get; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000C9C RID: 3228
		// (set) Token: 0x06000C9D RID: 3229
		string Encoding { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000C9E RID: 3230
		// (set) Token: 0x06000C9F RID: 3231
		string Standalone { get; set; }
	}
}
