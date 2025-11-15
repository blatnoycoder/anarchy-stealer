using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000DB RID: 219
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0002BD2C File Offset: 0x00029F2C
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0002BD34 File Offset: 0x00029F34
		public string Id { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x0002BD40 File Offset: 0x00029F40
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x0002BD48 File Offset: 0x00029F48
		public string Title { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0002BD54 File Offset: 0x00029F54
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x0002BD5C File Offset: 0x00029F5C
		public bool? Required { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0002BD68 File Offset: 0x00029F68
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x0002BD70 File Offset: 0x00029F70
		public bool? ReadOnly { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0002BD7C File Offset: 0x00029F7C
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x0002BD84 File Offset: 0x00029F84
		public bool? Hidden { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0002BD90 File Offset: 0x00029F90
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0002BD98 File Offset: 0x00029F98
		public bool? Transient { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0002BDA4 File Offset: 0x00029FA4
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x0002BDAC File Offset: 0x00029FAC
		public string Description { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0002BDB8 File Offset: 0x00029FB8
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x0002BDC0 File Offset: 0x00029FC0
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0002BDCC File Offset: 0x00029FCC
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x0002BDD4 File Offset: 0x00029FD4
		public string Pattern { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0002BDE0 File Offset: 0x00029FE0
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x0002BDE8 File Offset: 0x00029FE8
		public int? MinimumLength { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0002BDF4 File Offset: 0x00029FF4
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x0002BDFC File Offset: 0x00029FFC
		public int? MaximumLength { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0002BE08 File Offset: 0x0002A008
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x0002BE10 File Offset: 0x0002A010
		public double? DivisibleBy { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0002BE1C File Offset: 0x0002A01C
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0002BE24 File Offset: 0x0002A024
		public double? Minimum { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0002BE30 File Offset: 0x0002A030
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x0002BE38 File Offset: 0x0002A038
		public double? Maximum { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0002BE44 File Offset: 0x0002A044
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x0002BE4C File Offset: 0x0002A04C
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0002BE58 File Offset: 0x0002A058
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x0002BE60 File Offset: 0x0002A060
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002BE6C File Offset: 0x0002A06C
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x0002BE74 File Offset: 0x0002A074
		public int? MinimumItems { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002BE80 File Offset: 0x0002A080
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x0002BE88 File Offset: 0x0002A088
		public int? MaximumItems { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0002BE94 File Offset: 0x0002A094
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x0002BE9C File Offset: 0x0002A09C
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0002BEA8 File Offset: 0x0002A0A8
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x0002BEB0 File Offset: 0x0002A0B0
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0002BEBC File Offset: 0x0002A0BC
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0002BED0 File Offset: 0x0002A0D0
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x0002BED8 File Offset: 0x0002A0D8
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0002BEE4 File Offset: 0x0002A0E4
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x0002BEEC File Offset: 0x0002A0EC
		public bool UniqueItems { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0002BEF8 File Offset: 0x0002A0F8
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x0002BF00 File Offset: 0x0002A100
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0002BF0C File Offset: 0x0002A10C
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x0002BF14 File Offset: 0x0002A114
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0002BF20 File Offset: 0x0002A120
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0002BF28 File Offset: 0x0002A128
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0002BF34 File Offset: 0x0002A134
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x0002BF3C File Offset: 0x0002A13C
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0002BF48 File Offset: 0x0002A148
		// (set) Token: 0x060008C7 RID: 2247 RVA: 0x0002BF50 File Offset: 0x0002A150
		public string Requires { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0002BF5C File Offset: 0x0002A15C
		// (set) Token: 0x060008C9 RID: 2249 RVA: 0x0002BF64 File Offset: 0x0002A164
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0002BF70 File Offset: 0x0002A170
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x0002BF78 File Offset: 0x0002A178
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0002BF84 File Offset: 0x0002A184
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x0002BF8C File Offset: 0x0002A18C
		public JToken Default { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0002BF98 File Offset: 0x0002A198
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0002BFAC File Offset: 0x0002A1AC
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x0002BFB4 File Offset: 0x0002A1B4
		public string Format { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0002BFC0 File Offset: 0x0002A1C0
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x0002BFC8 File Offset: 0x0002A1C8
		internal string Location { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x0002BFD4 File Offset: 0x0002A1D4
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0002BFDC File Offset: 0x0002A1DC
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0002BFE4 File Offset: 0x0002A1E4
		internal string DeferredReference { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
		internal bool ReferencesResolved { get; set; }

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002C004 File Offset: 0x0002A204
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0002C044 File Offset: 0x0002A244
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002C054 File Offset: 0x0002A254
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return new JsonSchemaBuilder(resolver).Read(reader);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0002C078 File Offset: 0x0002A278
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002C088 File Offset: 0x0002A288
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema jsonSchema;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				jsonSchema = JsonSchema.Read(jsonReader, resolver);
			}
			return jsonSchema;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002C0D8 File Offset: 0x0002A2D8
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002C0E8 File Offset: 0x0002A2E8
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			new JsonSchemaWriter(writer, resolver).WriteSchema(this);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002C110 File Offset: 0x0002A310
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x040003BB RID: 955
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
