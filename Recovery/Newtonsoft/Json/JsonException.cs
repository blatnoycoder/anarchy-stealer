using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonException : Exception
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x0000AAE0 File Offset: 0x00008CE0
		public JsonException()
		{
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000AAE8 File Offset: 0x00008CE8
		public JsonException(string message)
			: base(message)
		{
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public JsonException(string message, [Nullable(2)] Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000AB00 File Offset: 0x00008D00
		public JsonException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000AB0C File Offset: 0x00008D0C
		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
}
