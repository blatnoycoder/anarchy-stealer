using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000E0 RID: 224
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0002DEFC File Offset: 0x0002C0FC
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x0002DF04 File Offset: 0x0002C104
		public bool Required { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0002DF10 File Offset: 0x0002C110
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x0002DF18 File Offset: 0x0002C118
		public JsonSchemaType Type { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0002DF24 File Offset: 0x0002C124
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x0002DF2C File Offset: 0x0002C12C
		public int? MinimumLength { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0002DF38 File Offset: 0x0002C138
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x0002DF40 File Offset: 0x0002C140
		public int? MaximumLength { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002DF4C File Offset: 0x0002C14C
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x0002DF54 File Offset: 0x0002C154
		public double? DivisibleBy { get; set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x0002DF60 File Offset: 0x0002C160
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x0002DF68 File Offset: 0x0002C168
		public double? Minimum { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0002DF74 File Offset: 0x0002C174
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x0002DF7C File Offset: 0x0002C17C
		public double? Maximum { get; set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0002DF88 File Offset: 0x0002C188
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x0002DF90 File Offset: 0x0002C190
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x0002DF9C File Offset: 0x0002C19C
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x0002DFA4 File Offset: 0x0002C1A4
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0002DFB0 File Offset: 0x0002C1B0
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
		public int? MinimumItems { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0002DFCC File Offset: 0x0002C1CC
		public int? MaximumItems { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0002DFD8 File Offset: 0x0002C1D8
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x0002DFE0 File Offset: 0x0002C1E0
		public IList<string> Patterns { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002DFEC File Offset: 0x0002C1EC
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0002DFF4 File Offset: 0x0002C1F4
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0002E000 File Offset: 0x0002C200
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0002E008 File Offset: 0x0002C208
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0002E014 File Offset: 0x0002C214
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x0002E01C File Offset: 0x0002C21C
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0002E028 File Offset: 0x0002C228
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x0002E030 File Offset: 0x0002C230
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0002E03C File Offset: 0x0002C23C
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0002E044 File Offset: 0x0002C244
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0002E050 File Offset: 0x0002C250
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x0002E058 File Offset: 0x0002C258
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0002E064 File Offset: 0x0002C264
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0002E06C File Offset: 0x0002C26C
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0002E078 File Offset: 0x0002C278
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x0002E080 File Offset: 0x0002C280
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0002E08C File Offset: 0x0002C28C
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x0002E094 File Offset: 0x0002C294
		public bool UniqueItems { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0002E0A0 File Offset: 0x0002C2A0
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x0002E0A8 File Offset: 0x0002C2A8
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0002E0B4 File Offset: 0x0002C2B4
		// (set) Token: 0x0600093F RID: 2367 RVA: 0x0002E0BC File Offset: 0x0002C2BC
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x06000940 RID: 2368 RVA: 0x0002E0C8 File Offset: 0x0002C2C8
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002E0FC File Offset: 0x0002C2FC
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema jsonSchema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, jsonSchema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002E154 File Offset: 0x0002C354
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = model.Required || schema.Required.GetValueOrDefault();
			model.Type &= schema.Type ?? JsonSchemaType.Any;
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = model.ExclusiveMinimum || schema.ExclusiveMinimum.GetValueOrDefault();
			model.ExclusiveMaximum = model.ExclusiveMaximum || schema.ExclusiveMaximum.GetValueOrDefault();
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = model.PositionalItemsValidation || schema.PositionalItemsValidation;
			model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
			model.AllowAdditionalItems = model.AllowAdditionalItems && schema.AllowAdditionalItems;
			model.UniqueItems = model.UniqueItems || schema.UniqueItems;
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= schema.Disallow.GetValueOrDefault();
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
