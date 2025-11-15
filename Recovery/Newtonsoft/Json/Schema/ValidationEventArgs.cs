using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000E8 RID: 232
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x06000966 RID: 2406 RVA: 0x0002F2A4 File Offset: 0x0002D4A4
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0002F2C0 File Offset: 0x0002D4C0
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002F2C8 File Offset: 0x0002D4C8
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0002F2D8 File Offset: 0x0002D4D8
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x0400041E RID: 1054
		private readonly JsonSchemaException _ex;
	}
}
