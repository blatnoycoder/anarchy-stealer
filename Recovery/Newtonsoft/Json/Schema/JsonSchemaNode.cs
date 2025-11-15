using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000E2 RID: 226
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaNode
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0002E878 File Offset: 0x0002CA78
		public string Id { get; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0002E880 File Offset: 0x0002CA80
		public ReadOnlyCollection<JsonSchema> Schemas { get; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0002E888 File Offset: 0x0002CA88
		public Dictionary<string, JsonSchemaNode> Properties { get; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0002E890 File Offset: 0x0002CA90
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0002E898 File Offset: 0x0002CA98
		public List<JsonSchemaNode> Items { get; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0002E8A0 File Offset: 0x0002CAA0
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x0002E8A8 File Offset: 0x0002CAA8
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0002E8B4 File Offset: 0x0002CAB4
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0002E8BC File Offset: 0x0002CABC
		public JsonSchemaNode AdditionalItems { get; set; }

		// Token: 0x06000955 RID: 2389 RVA: 0x0002E8C8 File Offset: 0x0002CAC8
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[] { schema });
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002E928 File Offset: 0x0002CB28
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union(new JsonSchema[] { schema }).ToList<JsonSchema>());
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.AdditionalItems = source.AdditionalItems;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002E9C0 File Offset: 0x0002CBC0
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002E9CC File Offset: 0x0002CBCC
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", schemata.Select((JsonSchema s) => s.InternalId).OrderBy((string id) => id, StringComparer.Ordinal));
		}
	}
}
