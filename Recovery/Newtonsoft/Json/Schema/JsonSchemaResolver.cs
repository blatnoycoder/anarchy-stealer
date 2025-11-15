using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000E4 RID: 228
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0002EA4C File Offset: 0x0002CC4C
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x0002EA54 File Offset: 0x0002CC54
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x0600095D RID: 2397 RVA: 0x0002EA60 File Offset: 0x0002CC60
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002EA74 File Offset: 0x0002CC74
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
