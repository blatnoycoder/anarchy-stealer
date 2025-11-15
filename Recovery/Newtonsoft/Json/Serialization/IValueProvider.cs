using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	public interface IValueProvider
	{
		// Token: 0x06000681 RID: 1665
		void SetValue(object target, [Nullable(2)] object value);

		// Token: 0x06000682 RID: 1666
		[return: Nullable(2)]
		object GetValue(object target);
	}
}
