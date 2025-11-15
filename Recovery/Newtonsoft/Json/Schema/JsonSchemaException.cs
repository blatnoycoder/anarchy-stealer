using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000DE RID: 222
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0002D3F4 File Offset: 0x0002B5F4
		public int LineNumber { get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0002D3FC File Offset: 0x0002B5FC
		public int LinePosition { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0002D404 File Offset: 0x0002B604
		public string Path { get; }

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002D40C File Offset: 0x0002B60C
		public JsonSchemaException()
		{
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002D414 File Offset: 0x0002B614
		public JsonSchemaException(string message)
			: base(message)
		{
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002D420 File Offset: 0x0002B620
		public JsonSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002D42C File Offset: 0x0002B62C
		public JsonSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002D438 File Offset: 0x0002B638
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
