using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000DD RID: 221
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal static class JsonSchemaConstants
	{
		// Token: 0x040003C3 RID: 963
		public const string TypePropertyName = "type";

		// Token: 0x040003C4 RID: 964
		public const string PropertiesPropertyName = "properties";

		// Token: 0x040003C5 RID: 965
		public const string ItemsPropertyName = "items";

		// Token: 0x040003C6 RID: 966
		public const string AdditionalItemsPropertyName = "additionalItems";

		// Token: 0x040003C7 RID: 967
		public const string RequiredPropertyName = "required";

		// Token: 0x040003C8 RID: 968
		public const string PatternPropertiesPropertyName = "patternProperties";

		// Token: 0x040003C9 RID: 969
		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		// Token: 0x040003CA RID: 970
		public const string RequiresPropertyName = "requires";

		// Token: 0x040003CB RID: 971
		public const string MinimumPropertyName = "minimum";

		// Token: 0x040003CC RID: 972
		public const string MaximumPropertyName = "maximum";

		// Token: 0x040003CD RID: 973
		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		// Token: 0x040003CE RID: 974
		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		// Token: 0x040003CF RID: 975
		public const string MinimumItemsPropertyName = "minItems";

		// Token: 0x040003D0 RID: 976
		public const string MaximumItemsPropertyName = "maxItems";

		// Token: 0x040003D1 RID: 977
		public const string PatternPropertyName = "pattern";

		// Token: 0x040003D2 RID: 978
		public const string MaximumLengthPropertyName = "maxLength";

		// Token: 0x040003D3 RID: 979
		public const string MinimumLengthPropertyName = "minLength";

		// Token: 0x040003D4 RID: 980
		public const string EnumPropertyName = "enum";

		// Token: 0x040003D5 RID: 981
		public const string ReadOnlyPropertyName = "readonly";

		// Token: 0x040003D6 RID: 982
		public const string TitlePropertyName = "title";

		// Token: 0x040003D7 RID: 983
		public const string DescriptionPropertyName = "description";

		// Token: 0x040003D8 RID: 984
		public const string FormatPropertyName = "format";

		// Token: 0x040003D9 RID: 985
		public const string DefaultPropertyName = "default";

		// Token: 0x040003DA RID: 986
		public const string TransientPropertyName = "transient";

		// Token: 0x040003DB RID: 987
		public const string DivisibleByPropertyName = "divisibleBy";

		// Token: 0x040003DC RID: 988
		public const string HiddenPropertyName = "hidden";

		// Token: 0x040003DD RID: 989
		public const string DisallowPropertyName = "disallow";

		// Token: 0x040003DE RID: 990
		public const string ExtendsPropertyName = "extends";

		// Token: 0x040003DF RID: 991
		public const string IdPropertyName = "id";

		// Token: 0x040003E0 RID: 992
		public const string UniqueItemsPropertyName = "uniqueItems";

		// Token: 0x040003E1 RID: 993
		public const string OptionValuePropertyName = "value";

		// Token: 0x040003E2 RID: 994
		public const string OptionLabelPropertyName = "label";

		// Token: 0x040003E3 RID: 995
		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping = new Dictionary<string, JsonSchemaType>
		{
			{
				"string",
				JsonSchemaType.String
			},
			{
				"object",
				JsonSchemaType.Object
			},
			{
				"integer",
				JsonSchemaType.Integer
			},
			{
				"number",
				JsonSchemaType.Float
			},
			{
				"null",
				JsonSchemaType.Null
			},
			{
				"boolean",
				JsonSchemaType.Boolean
			},
			{
				"array",
				JsonSchemaType.Array
			},
			{
				"any",
				JsonSchemaType.Any
			}
		};
	}
}
