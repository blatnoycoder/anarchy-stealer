using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B4 RID: 180
	[NullableContext(1)]
	public interface IReferenceResolver
	{
		// Token: 0x06000679 RID: 1657
		object ResolveReference(object context, string reference);

		// Token: 0x0600067A RID: 1658
		string GetReference(object context, object value);

		// Token: 0x0600067B RID: 1659
		bool IsReferenced(object context, object value);

		// Token: 0x0600067C RID: 1660
		void AddReference(object context, string reference, object value);
	}
}
