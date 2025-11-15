using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000114 RID: 276
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CustomCreationConverter<[Nullable(2)] T> : JsonConverter
	{
		// Token: 0x06000C08 RID: 3080 RVA: 0x00038DF8 File Offset: 0x00036FF8
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00038E04 File Offset: 0x00037004
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000C0A RID: 3082
		public abstract T Create(Type objectType);

		// Token: 0x06000C0B RID: 3083 RVA: 0x00038E58 File Offset: 0x00037058
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00038E6C File Offset: 0x0003706C
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
