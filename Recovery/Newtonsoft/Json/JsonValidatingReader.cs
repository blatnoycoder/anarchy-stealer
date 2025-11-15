using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000067 RID: 103
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600037D RID: 893 RVA: 0x000122FC File Offset: 0x000104FC
		// (remove) Token: 0x0600037E RID: 894 RVA: 0x00012338 File Offset: 0x00010538
		public event ValidationEventHandler ValidationEventHandler;

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00012374 File Offset: 0x00010574
		public override object Value
		{
			get
			{
				return this._reader.Value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00012384 File Offset: 0x00010584
		public override int Depth
		{
			get
			{
				return this._reader.Depth;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00012394 File Offset: 0x00010594
		public override string Path
		{
			get
			{
				return this._reader.Path;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000382 RID: 898 RVA: 0x000123A4 File Offset: 0x000105A4
		// (set) Token: 0x06000383 RID: 899 RVA: 0x000123B4 File Offset: 0x000105B4
		public override char QuoteChar
		{
			get
			{
				return this._reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000384 RID: 900 RVA: 0x000123B8 File Offset: 0x000105B8
		public override JsonToken TokenType
		{
			get
			{
				return this._reader.TokenType;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000385 RID: 901 RVA: 0x000123C8 File Offset: 0x000105C8
		public override Type ValueType
		{
			get
			{
				return this._reader.ValueType;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000123D8 File Offset: 0x000105D8
		private void Push(JsonValidatingReader.SchemaScope scope)
		{
			this._stack.Push(scope);
			this._currentScope = scope;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000123F0 File Offset: 0x000105F0
		private JsonValidatingReader.SchemaScope Pop()
		{
			JsonValidatingReader.SchemaScope schemaScope = this._stack.Pop();
			this._currentScope = ((this._stack.Count != 0) ? this._stack.Peek() : null);
			return schemaScope;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00012434 File Offset: 0x00010634
		private IList<JsonSchemaModel> CurrentSchemas
		{
			get
			{
				return this._currentScope.Schemas;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00012444 File Offset: 0x00010644
		private IList<JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				if (this._currentScope == null)
				{
					return new List<JsonSchemaModel>(new JsonSchemaModel[] { this._model });
				}
				if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
				{
					return JsonValidatingReader.EmptySchemaList;
				}
				switch (this._currentScope.TokenType)
				{
				case JTokenType.None:
					return this._currentScope.Schemas;
				case JTokenType.Object:
				{
					if (this._currentScope.CurrentPropertyName == null)
					{
						throw new JsonReaderException("CurrentPropertyName has not been set on scope.");
					}
					IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
					foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
					{
						JsonSchemaModel jsonSchemaModel2;
						if (jsonSchemaModel.Properties != null && jsonSchemaModel.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out jsonSchemaModel2))
						{
							list.Add(jsonSchemaModel2);
						}
						if (jsonSchemaModel.PatternProperties != null)
						{
							foreach (KeyValuePair<string, JsonSchemaModel> keyValuePair in jsonSchemaModel.PatternProperties)
							{
								if (Regex.IsMatch(this._currentScope.CurrentPropertyName, keyValuePair.Key))
								{
									list.Add(keyValuePair.Value);
								}
							}
						}
						if (list.Count == 0 && jsonSchemaModel.AllowAdditionalProperties && jsonSchemaModel.AdditionalProperties != null)
						{
							list.Add(jsonSchemaModel.AdditionalProperties);
						}
					}
					return list;
				}
				case JTokenType.Array:
				{
					IList<JsonSchemaModel> list2 = new List<JsonSchemaModel>();
					foreach (JsonSchemaModel jsonSchemaModel3 in this.CurrentSchemas)
					{
						if (!jsonSchemaModel3.PositionalItemsValidation)
						{
							if (jsonSchemaModel3.Items != null && jsonSchemaModel3.Items.Count > 0)
							{
								list2.Add(jsonSchemaModel3.Items[0]);
							}
						}
						else
						{
							if (jsonSchemaModel3.Items != null && jsonSchemaModel3.Items.Count > 0 && jsonSchemaModel3.Items.Count > this._currentScope.ArrayItemCount - 1)
							{
								list2.Add(jsonSchemaModel3.Items[this._currentScope.ArrayItemCount - 1]);
							}
							if (jsonSchemaModel3.AllowAdditionalItems && jsonSchemaModel3.AdditionalItems != null)
							{
								list2.Add(jsonSchemaModel3.AdditionalItems);
							}
						}
					}
					return list2;
				}
				case JTokenType.Constructor:
					return JsonValidatingReader.EmptySchemaList;
				default:
					throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, this._currentScope.TokenType));
				}
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001274C File Offset: 0x0001094C
		private void RaiseError(string message, JsonSchemaModel schema)
		{
			string text = (((IJsonLineInfo)this).HasLineInfo() ? (message + " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition)) : message);
			this.OnValidationEvent(new JsonSchemaException(text, null, this.Path, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000127BC File Offset: 0x000109BC
		private void OnValidationEvent(JsonSchemaException exception)
		{
			ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler != null)
			{
				validationEventHandler(this, new ValidationEventArgs(exception));
				return;
			}
			throw exception;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000127EC File Offset: 0x000109EC
		public JsonValidatingReader(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new Stack<JsonValidatingReader.SchemaScope>();
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00012814 File Offset: 0x00010A14
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0001281C File Offset: 0x00010A1C
		public JsonSchema Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				if (this.TokenType != JsonToken.None)
				{
					throw new InvalidOperationException("Cannot change schema while validating JSON.");
				}
				this._schema = value;
				this._model = null;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00012844 File Offset: 0x00010A44
		public JsonReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001284C File Offset: 0x00010A4C
		public override void Close()
		{
			base.Close();
			if (base.CloseInput)
			{
				JsonReader reader = this._reader;
				if (reader == null)
				{
					return;
				}
				reader.Close();
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00012874 File Offset: 0x00010A74
		private void ValidateNotDisallowed(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
			if (currentNodeSchemaType != null && JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.GetValueOrDefault()))
			{
				this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, currentNodeSchemaType), schema);
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000128D8 File Offset: 0x00010AD8
		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				return new JsonSchemaType?(JsonSchemaType.Object);
			case JsonToken.StartArray:
				return new JsonSchemaType?(JsonSchemaType.Array);
			case JsonToken.Integer:
				return new JsonSchemaType?(JsonSchemaType.Integer);
			case JsonToken.Float:
				return new JsonSchemaType?(JsonSchemaType.Float);
			case JsonToken.String:
				return new JsonSchemaType?(JsonSchemaType.String);
			case JsonToken.Boolean:
				return new JsonSchemaType?(JsonSchemaType.Boolean);
			case JsonToken.Null:
				return new JsonSchemaType?(JsonSchemaType.Null);
			}
			return null;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001296C File Offset: 0x00010B6C
		public override int? ReadAsInt32()
		{
			int? num = this._reader.ReadAsInt32();
			this.ValidateCurrentToken();
			return num;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00012980 File Offset: 0x00010B80
		public override byte[] ReadAsBytes()
		{
			byte[] array = this._reader.ReadAsBytes();
			this.ValidateCurrentToken();
			return array;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00012994 File Offset: 0x00010B94
		public override decimal? ReadAsDecimal()
		{
			decimal? num = this._reader.ReadAsDecimal();
			this.ValidateCurrentToken();
			return num;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000129A8 File Offset: 0x00010BA8
		public override double? ReadAsDouble()
		{
			double? num = this._reader.ReadAsDouble();
			this.ValidateCurrentToken();
			return num;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000129BC File Offset: 0x00010BBC
		public override bool? ReadAsBoolean()
		{
			bool? flag = this._reader.ReadAsBoolean();
			this.ValidateCurrentToken();
			return flag;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000129D0 File Offset: 0x00010BD0
		public override string ReadAsString()
		{
			string text = this._reader.ReadAsString();
			this.ValidateCurrentToken();
			return text;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000129E4 File Offset: 0x00010BE4
		public override DateTime? ReadAsDateTime()
		{
			DateTime? dateTime = this._reader.ReadAsDateTime();
			this.ValidateCurrentToken();
			return dateTime;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000129F8 File Offset: 0x00010BF8
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? dateTimeOffset = this._reader.ReadAsDateTimeOffset();
			this.ValidateCurrentToken();
			return dateTimeOffset;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00012A0C File Offset: 0x00010C0C
		public override bool Read()
		{
			if (!this._reader.Read())
			{
				return false;
			}
			if (this._reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			this.ValidateCurrentToken();
			return true;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00012A3C File Offset: 0x00010C3C
		private void ValidateCurrentToken()
		{
			if (this._model == null)
			{
				JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
				this._model = jsonSchemaModelBuilder.Build(this._schema);
				if (!JsonTokenUtils.IsStartToken(this._reader.TokenType))
				{
					this.Push(new JsonValidatingReader.SchemaScope(JTokenType.None, this.CurrentMemberSchemas));
				}
			}
			switch (this._reader.TokenType)
			{
			case JsonToken.None:
				return;
			case JsonToken.StartObject:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> list = this.CurrentMemberSchemas.Where(new Func<JsonSchemaModel, bool>(this.ValidateObject)).ToList<JsonSchemaModel>();
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, list));
				this.WriteToken(this.CurrentSchemas);
				return;
			}
			case JsonToken.StartArray:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> list2 = this.CurrentMemberSchemas.Where(new Func<JsonSchemaModel, bool>(this.ValidateArray)).ToList<JsonSchemaModel>();
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, list2));
				this.WriteToken(this.CurrentSchemas);
				return;
			}
			case JsonToken.StartConstructor:
				this.ProcessValue();
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
				this.WriteToken(this.CurrentSchemas);
				return;
			case JsonToken.PropertyName:
			{
				this.WriteToken(this.CurrentSchemas);
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel jsonSchemaModel = enumerator.Current;
						this.ValidatePropertyName(jsonSchemaModel);
					}
					return;
				}
				break;
			}
			case JsonToken.Comment:
				goto IL_03F9;
			case JsonToken.Raw:
				break;
			case JsonToken.Integer:
			{
				this.ProcessValue();
				this.WriteToken(this.CurrentMemberSchemas);
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel jsonSchemaModel2 = enumerator.Current;
						this.ValidateInteger(jsonSchemaModel2);
					}
					return;
				}
				goto IL_01E8;
			}
			case JsonToken.Float:
				goto IL_01E8;
			case JsonToken.String:
				goto IL_023A;
			case JsonToken.Boolean:
				goto IL_028C;
			case JsonToken.Null:
				goto IL_02DE;
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.WriteToken(this.CurrentMemberSchemas);
				return;
			case JsonToken.EndObject:
				goto IL_0330;
			case JsonToken.EndArray:
				this.WriteToken(this.CurrentSchemas);
				foreach (JsonSchemaModel jsonSchemaModel3 in this.CurrentSchemas)
				{
					this.ValidateEndArray(jsonSchemaModel3);
				}
				this.Pop();
				return;
			case JsonToken.EndConstructor:
				this.WriteToken(this.CurrentSchemas);
				this.Pop();
				return;
			default:
				goto IL_03F9;
			}
			this.ProcessValue();
			return;
			IL_01E8:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel jsonSchemaModel4 = enumerator.Current;
					this.ValidateFloat(jsonSchemaModel4);
				}
				return;
			}
			IL_023A:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel jsonSchemaModel5 = enumerator.Current;
					this.ValidateString(jsonSchemaModel5);
				}
				return;
			}
			IL_028C:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel jsonSchemaModel6 = enumerator.Current;
					this.ValidateBoolean(jsonSchemaModel6);
				}
				return;
			}
			IL_02DE:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel jsonSchemaModel7 = enumerator.Current;
					this.ValidateNull(jsonSchemaModel7);
				}
				return;
			}
			IL_0330:
			this.WriteToken(this.CurrentSchemas);
			foreach (JsonSchemaModel jsonSchemaModel8 in this.CurrentSchemas)
			{
				this.ValidateEndObject(jsonSchemaModel8);
			}
			this.Pop();
			return;
			IL_03F9:
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00012EAC File Offset: 0x000110AC
		private void WriteToken(IList<JsonSchemaModel> schemas)
		{
			foreach (JsonValidatingReader.SchemaScope schemaScope in this._stack)
			{
				bool flag = schemaScope.TokenType == JTokenType.Array && schemaScope.IsUniqueArray && schemaScope.ArrayItemCount > 0;
				if (!flag)
				{
					if (!schemas.Any((JsonSchemaModel s) => s.Enum != null))
					{
						continue;
					}
				}
				if (schemaScope.CurrentItemWriter == null)
				{
					if (JsonTokenUtils.IsEndToken(this._reader.TokenType))
					{
						continue;
					}
					schemaScope.CurrentItemWriter = new JTokenWriter();
				}
				schemaScope.CurrentItemWriter.WriteToken(this._reader, false);
				if (schemaScope.CurrentItemWriter.Top == 0 && this._reader.TokenType != JsonToken.PropertyName)
				{
					JToken token = schemaScope.CurrentItemWriter.Token;
					schemaScope.CurrentItemWriter = null;
					if (flag)
					{
						if (schemaScope.UniqueArrayItems.Contains(token, JToken.EqualityComparer))
						{
							this.RaiseError("Non-unique array item at index {0}.".FormatWith(CultureInfo.InvariantCulture, schemaScope.ArrayItemCount - 1), schemaScope.Schemas.First((JsonSchemaModel s) => s.UniqueItems));
						}
						schemaScope.UniqueArrayItems.Add(token);
					}
					else if (schemas.Any((JsonSchemaModel s) => s.Enum != null))
					{
						foreach (JsonSchemaModel jsonSchemaModel in schemas)
						{
							if (jsonSchemaModel.Enum != null && !jsonSchemaModel.Enum.ContainsValue(token, JToken.EqualityComparer))
							{
								StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
								token.WriteTo(new JsonTextWriter(stringWriter), new JsonConverter[0]);
								this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, stringWriter.ToString()), jsonSchemaModel);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001312C File Offset: 0x0001132C
		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				if (requiredProperties.Values.Any((bool v) => !v))
				{
					IEnumerable<string> enumerable = from kv in requiredProperties
						where !kv.Value
						select kv.Key;
					this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", enumerable)), schema);
				}
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x000131FC File Offset: 0x000113FC
		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			int arrayItemCount = this._currentScope.ArrayItemCount;
			if (schema.MaximumItems != null)
			{
				int num = arrayItemCount;
				int? num2 = schema.MaximumItems;
				if ((num > num2.GetValueOrDefault()) & (num2 != null))
				{
					this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MaximumItems), schema);
				}
			}
			if (schema.MinimumItems != null)
			{
				int num3 = arrayItemCount;
				int? num2 = schema.MinimumItems;
				if ((num3 < num2.GetValueOrDefault()) & (num2 != null))
				{
					this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MinimumItems), schema);
				}
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000132D0 File Offset: 0x000114D0
		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Null))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000132F0 File Offset: 0x000114F0
		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Boolean))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00013310 File Offset: 0x00011510
		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			string text = this._reader.Value.ToString();
			if (schema.MaximumLength != null)
			{
				int length = text.Length;
				int? num = schema.MaximumLength;
				if ((length > num.GetValueOrDefault()) & (num != null))
				{
					this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, text, schema.MaximumLength), schema);
				}
			}
			if (schema.MinimumLength != null)
			{
				int length2 = text.Length;
				int? num = schema.MinimumLength;
				if ((length2 < num.GetValueOrDefault()) & (num != null))
				{
					this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, text, schema.MinimumLength), schema);
				}
			}
			if (schema.Patterns != null)
			{
				foreach (string text2 in schema.Patterns)
				{
					if (!Regex.IsMatch(text, text2))
					{
						this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, text, text2), schema);
					}
				}
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001346C File Offset: 0x0001166C
		private void ValidateInteger(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			object value = this._reader.Value;
			if (schema.Maximum != null)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Maximum) > 0)
				{
					this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && JValue.Compare(JTokenType.Integer, value, schema.Maximum) == 0)
				{
					this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
			}
			if (schema.Minimum != null)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Minimum) < 0)
				{
					this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && JValue.Compare(JTokenType.Integer, value, schema.Minimum) == 0)
				{
					this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy != null)
			{
				bool flag;
				if (value is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)value;
					if (!Math.Abs(schema.DivisibleBy.Value - Math.Truncate(schema.DivisibleBy.Value)).Equals(0.0))
					{
						flag = bigInteger != 0L;
					}
					else
					{
						flag = bigInteger % new BigInteger(schema.DivisibleBy.Value) != 0L;
					}
				}
				else
				{
					flag = !JsonValidatingReader.IsZero((double)Convert.ToInt64(value, CultureInfo.InvariantCulture) % schema.DivisibleBy.GetValueOrDefault());
				}
				if (flag)
				{
					this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.DivisibleBy), schema);
				}
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000136B8 File Offset: 0x000118B8
		private void ProcessValue()
		{
			if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
			{
				JsonValidatingReader.SchemaScope currentScope = this._currentScope;
				int arrayItemCount = currentScope.ArrayItemCount;
				currentScope.ArrayItemCount = arrayItemCount + 1;
				foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
				{
					if (jsonSchemaModel != null && jsonSchemaModel.PositionalItemsValidation && !jsonSchemaModel.AllowAdditionalItems && (jsonSchemaModel.Items == null || this._currentScope.ArrayItemCount - 1 >= jsonSchemaModel.Items.Count))
					{
						this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, this._currentScope.ArrayItemCount), jsonSchemaModel);
					}
				}
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000137A4 File Offset: 0x000119A4
		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			double num = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum != null)
			{
				double num2 = num;
				double? num3 = schema.Maximum;
				if ((num2 > num3.GetValueOrDefault()) & (num3 != null))
				{
					this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					double num4 = num;
					num3 = schema.Maximum;
					if ((num4 == num3.GetValueOrDefault()) & (num3 != null))
					{
						this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
					}
				}
			}
			if (schema.Minimum != null)
			{
				double num5 = num;
				double? num3 = schema.Minimum;
				if ((num5 < num3.GetValueOrDefault()) & (num3 != null))
				{
					this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					double num6 = num;
					num3 = schema.Minimum;
					if ((num6 == num3.GetValueOrDefault()) & (num3 != null))
					{
						this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
					}
				}
			}
			if (schema.DivisibleBy != null && !JsonValidatingReader.IsZero(JsonValidatingReader.FloatingPointRemainder(num, schema.DivisibleBy.GetValueOrDefault())))
			{
				this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.DivisibleBy), schema);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00013990 File Offset: 0x00011B90
		private static double FloatingPointRemainder(double dividend, double divisor)
		{
			return dividend - Math.Floor(dividend / divisor) * divisor;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000139A0 File Offset: 0x00011BA0
		private static bool IsZero(double value)
		{
			return Math.Abs(value) < 4.440892098500626E-15;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000139B4 File Offset: 0x00011BB4
		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			if (this._currentScope.RequiredProperties.ContainsKey(text))
			{
				this._currentScope.RequiredProperties[text] = true;
			}
			if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, text))
			{
				this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, text), schema);
			}
			this._currentScope.CurrentPropertyName = text;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00013A48 File Offset: 0x00011C48
		private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
		{
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				foreach (string text in schema.PatternProperties.Keys)
				{
					if (Regex.IsMatch(propertyName, text))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00013ADC File Offset: 0x00011CDC
		private bool ValidateArray(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Array);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00013AF0 File Offset: 0x00011CF0
		private bool ValidateObject(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Object);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00013B04 File Offset: 0x00011D04
		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (!JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
			{
				this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, currentSchema.Type, currentType), currentSchema);
				return false;
			}
			return true;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00013B58 File Offset: 0x00011D58
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00013B84 File Offset: 0x00011D84
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00013BB0 File Offset: 0x00011DB0
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x040001CC RID: 460
		private readonly JsonReader _reader;

		// Token: 0x040001CD RID: 461
		private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

		// Token: 0x040001CE RID: 462
		private JsonSchema _schema;

		// Token: 0x040001CF RID: 463
		private JsonSchemaModel _model;

		// Token: 0x040001D0 RID: 464
		private JsonValidatingReader.SchemaScope _currentScope;

		// Token: 0x040001D2 RID: 466
		private static readonly IList<JsonSchemaModel> EmptySchemaList = new List<JsonSchemaModel>();

		// Token: 0x0200021F RID: 543
		private class SchemaScope
		{
			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00065780 File Offset: 0x00063980
			// (set) Token: 0x060016C9 RID: 5833 RVA: 0x00065788 File Offset: 0x00063988
			public string CurrentPropertyName { get; set; }

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x060016CA RID: 5834 RVA: 0x00065794 File Offset: 0x00063994
			// (set) Token: 0x060016CB RID: 5835 RVA: 0x0006579C File Offset: 0x0006399C
			public int ArrayItemCount { get; set; }

			// Token: 0x170003C2 RID: 962
			// (get) Token: 0x060016CC RID: 5836 RVA: 0x000657A8 File Offset: 0x000639A8
			public bool IsUniqueArray { get; }

			// Token: 0x170003C3 RID: 963
			// (get) Token: 0x060016CD RID: 5837 RVA: 0x000657B0 File Offset: 0x000639B0
			public IList<JToken> UniqueArrayItems { get; }

			// Token: 0x170003C4 RID: 964
			// (get) Token: 0x060016CE RID: 5838 RVA: 0x000657B8 File Offset: 0x000639B8
			// (set) Token: 0x060016CF RID: 5839 RVA: 0x000657C0 File Offset: 0x000639C0
			public JTokenWriter CurrentItemWriter { get; set; }

			// Token: 0x170003C5 RID: 965
			// (get) Token: 0x060016D0 RID: 5840 RVA: 0x000657CC File Offset: 0x000639CC
			public IList<JsonSchemaModel> Schemas
			{
				get
				{
					return this._schemas;
				}
			}

			// Token: 0x170003C6 RID: 966
			// (get) Token: 0x060016D1 RID: 5841 RVA: 0x000657D4 File Offset: 0x000639D4
			public Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return this._requiredProperties;
				}
			}

			// Token: 0x170003C7 RID: 967
			// (get) Token: 0x060016D2 RID: 5842 RVA: 0x000657DC File Offset: 0x000639DC
			public JTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
			}

			// Token: 0x060016D3 RID: 5843 RVA: 0x000657E4 File Offset: 0x000639E4
			public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
			{
				this._tokenType = tokenType;
				this._schemas = schemas;
				this._requiredProperties = schemas.SelectMany(new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties)).Distinct<string>().ToDictionary((string p) => p, (string p) => false);
				if (tokenType == JTokenType.Array)
				{
					if (schemas.Any((JsonSchemaModel s) => s.UniqueItems))
					{
						this.IsUniqueArray = true;
						this.UniqueArrayItems = new List<JToken>();
					}
				}
			}

			// Token: 0x060016D4 RID: 5844 RVA: 0x000658B8 File Offset: 0x00063AB8
			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				if (((schema != null) ? schema.Properties : null) == null)
				{
					return Enumerable.Empty<string>();
				}
				return from p in schema.Properties
					where p.Value.Required
					select p.Key;
			}

			// Token: 0x040009B8 RID: 2488
			private readonly JTokenType _tokenType;

			// Token: 0x040009B9 RID: 2489
			private readonly IList<JsonSchemaModel> _schemas;

			// Token: 0x040009BA RID: 2490
			private readonly Dictionary<string, bool> _requiredProperties;
		}
	}
}
