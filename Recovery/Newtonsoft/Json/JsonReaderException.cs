using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200005E RID: 94
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonReaderException : JsonException
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000C710 File Offset: 0x0000A910
		public int LineNumber { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000C718 File Offset: 0x0000A918
		public int LinePosition { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000C720 File Offset: 0x0000A920
		[Nullable(2)]
		public string Path
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C728 File Offset: 0x0000A928
		public JsonReaderException()
		{
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C730 File Offset: 0x0000A930
		public JsonReaderException(string message)
			: base(message)
		{
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C73C File Offset: 0x0000A93C
		public JsonReaderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C748 File Offset: 0x0000A948
		public JsonReaderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C754 File Offset: 0x0000A954
		public JsonReaderException(string message, string path, int lineNumber, int linePosition, [Nullable(2)] Exception innerException)
			: base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000C778 File Offset: 0x0000A978
		internal static JsonReaderException Create(JsonReader reader, string message)
		{
			return JsonReaderException.Create(reader, message, null);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000C784 File Offset: 0x0000A984
		internal static JsonReaderException Create(JsonReader reader, string message, [Nullable(2)] Exception ex)
		{
			return JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C79C File Offset: 0x0000A99C
		internal static JsonReaderException Create([Nullable(2)] IJsonLineInfo lineInfo, string path, string message, [Nullable(2)] Exception ex)
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
			return new JsonReaderException(message, path, num, num2, ex);
		}
	}
}
