using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x02000050 RID: 80
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonConverter
	{
		// Token: 0x060001A3 RID: 419
		public abstract void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer);

		// Token: 0x060001A4 RID: 420
		[return: Nullable(2)]
		public abstract object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer);

		// Token: 0x060001A5 RID: 421
		public abstract bool CanConvert(Type objectType);

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000A974 File Offset: 0x00008B74
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000A978 File Offset: 0x00008B78
		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}
	}
}
