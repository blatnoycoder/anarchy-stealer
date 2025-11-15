using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000127 RID: 295
	[NullableContext(1)]
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06000C8E RID: 3214
		IXmlNode CreateComment([Nullable(2)] string text);

		// Token: 0x06000C8F RID: 3215
		IXmlNode CreateTextNode([Nullable(2)] string text);

		// Token: 0x06000C90 RID: 3216
		IXmlNode CreateCDataSection([Nullable(2)] string data);

		// Token: 0x06000C91 RID: 3217
		IXmlNode CreateWhitespace([Nullable(2)] string text);

		// Token: 0x06000C92 RID: 3218
		IXmlNode CreateSignificantWhitespace([Nullable(2)] string text);

		// Token: 0x06000C93 RID: 3219
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		// Token: 0x06000C94 RID: 3220
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset);

		// Token: 0x06000C95 RID: 3221
		IXmlNode CreateProcessingInstruction(string target, [Nullable(2)] string data);

		// Token: 0x06000C96 RID: 3222
		IXmlElement CreateElement(string elementName);

		// Token: 0x06000C97 RID: 3223
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x06000C98 RID: 3224
		IXmlNode CreateAttribute(string name, [Nullable(2)] string value);

		// Token: 0x06000C99 RID: 3225
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, [Nullable(2)] string value);

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000C9A RID: 3226
		[Nullable(2)]
		IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get;
		}
	}
}
