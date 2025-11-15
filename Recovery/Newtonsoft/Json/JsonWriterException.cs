using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonWriterException : JsonException
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00015534 File Offset: 0x00013734
		[Nullable(2)]
		public string Path
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001553C File Offset: 0x0001373C
		public JsonWriterException()
		{
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00015544 File Offset: 0x00013744
		public JsonWriterException(string message)
			: base(message)
		{
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00015550 File Offset: 0x00013750
		public JsonWriterException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001555C File Offset: 0x0001375C
		public JsonWriterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00015568 File Offset: 0x00013768
		public JsonWriterException(string message, string path, [Nullable(2)] Exception innerException)
			: base(message, innerException)
		{
			this.Path = path;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001557C File Offset: 0x0001377C
		internal static JsonWriterException Create(JsonWriter writer, string message, [Nullable(2)] Exception ex)
		{
			return JsonWriterException.Create(writer.ContainerPath, message, ex);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001558C File Offset: 0x0001378C
		internal static JsonWriterException Create(string path, string message, [Nullable(2)] Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, path, ex);
		}
	}
}
