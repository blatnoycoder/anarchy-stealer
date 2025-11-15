using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000117 RID: 279
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x000394B4 File Offset: 0x000376B4
		[NullableContext(1)]
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
