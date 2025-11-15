using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonSerializationException : JsonException
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
		public int LineNumber { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		public int LinePosition { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000C804 File Offset: 0x0000AA04
		[Nullable(2)]
		public string Path
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000C80C File Offset: 0x0000AA0C
		public JsonSerializationException()
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C814 File Offset: 0x0000AA14
		public JsonSerializationException(string message)
			: base(message)
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C820 File Offset: 0x0000AA20
		public JsonSerializationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C82C File Offset: 0x0000AA2C
		public JsonSerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000C838 File Offset: 0x0000AA38
		public JsonSerializationException(string message, string path, int lineNumber, int linePosition, [Nullable(2)] Exception innerException)
			: base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C85C File Offset: 0x0000AA5C
		internal static JsonSerializationException Create(JsonReader reader, string message)
		{
			return JsonSerializationException.Create(reader, message, null);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C868 File Offset: 0x0000AA68
		internal static JsonSerializationException Create(JsonReader reader, string message, [Nullable(2)] Exception ex)
		{
			return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000C880 File Offset: 0x0000AA80
		internal static JsonSerializationException Create([Nullable(2)] IJsonLineInfo lineInfo, string path, string message, [Nullable(2)] Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			int num;
			int num2;
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				num = lineInfo.LineNumber;
				num2 = lineInfo.LinePosition;
			}
			else
			{
				num = 0;
				num2 = 0;
			}
			return new JsonSerializationException(message, path, num, num2, ex);
		}
	}
}
