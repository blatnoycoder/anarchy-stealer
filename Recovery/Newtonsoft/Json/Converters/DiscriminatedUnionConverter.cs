using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000118 RID: 280
	[NullableContext(1)]
	[Nullable(0)]
	public class DiscriminatedUnionConverter : JsonConverter
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x00039528 File Offset: 0x00037728
		private static Type CreateUnionTypeLookup(Type t)
		{
			MethodCall<object, object> getUnionCases = FSharpUtils.Instance.GetUnionCases;
			object obj = null;
			object[] array = new object[2];
			array[0] = t;
			object obj2 = ((object[])getUnionCases(obj, array)).First<object>();
			return (Type)FSharpUtils.Instance.GetUnionCaseInfoDeclaringType(obj2);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00039574 File Offset: 0x00037774
		private static DiscriminatedUnionConverter.Union CreateUnion(Type t)
		{
			MethodCall<object, object> preComputeUnionTagReader = FSharpUtils.Instance.PreComputeUnionTagReader;
			object obj = null;
			object[] array = new object[2];
			array[0] = t;
			DiscriminatedUnionConverter.Union union = new DiscriminatedUnionConverter.Union((FSharpFunction)preComputeUnionTagReader(obj, array), new List<DiscriminatedUnionConverter.UnionCase>());
			MethodCall<object, object> getUnionCases = FSharpUtils.Instance.GetUnionCases;
			object obj2 = null;
			object[] array2 = new object[2];
			array2[0] = t;
			foreach (object obj3 in (object[])getUnionCases(obj2, array2))
			{
				int num = (int)FSharpUtils.Instance.GetUnionCaseInfoTag(obj3);
				string text = (string)FSharpUtils.Instance.GetUnionCaseInfoName(obj3);
				PropertyInfo[] array4 = (PropertyInfo[])FSharpUtils.Instance.GetUnionCaseInfoFields(obj3, new object[0]);
				MethodCall<object, object> preComputeUnionReader = FSharpUtils.Instance.PreComputeUnionReader;
				object obj4 = null;
				object[] array5 = new object[2];
				array5[0] = obj3;
				FSharpFunction fsharpFunction = (FSharpFunction)preComputeUnionReader(obj4, array5);
				MethodCall<object, object> preComputeUnionConstructor = FSharpUtils.Instance.PreComputeUnionConstructor;
				object obj5 = null;
				object[] array6 = new object[2];
				array6[0] = obj3;
				DiscriminatedUnionConverter.UnionCase unionCase = new DiscriminatedUnionConverter.UnionCase(num, text, array4, fsharpFunction, (FSharpFunction)preComputeUnionConstructor(obj5, array6));
				union.Cases.Add(unionCase);
			}
			return union;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00039680 File Offset: 0x00037880
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type type = DiscriminatedUnionConverter.UnionTypeLookupCache.Get(value.GetType());
			DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(type);
			int tag = (int)union.TagReader.Invoke(new object[] { value });
			DiscriminatedUnionConverter.UnionCase unionCase = union.Cases.Single((DiscriminatedUnionConverter.UnionCase c) => c.Tag == tag);
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue(unionCase.Name);
			if (unionCase.Fields != null && unionCase.Fields.Length != 0)
			{
				object[] array = (object[])unionCase.FieldReader.Invoke(new object[] { value });
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				writer.WriteStartArray();
				foreach (object obj in array)
				{
					serializer.Serialize(writer, obj);
				}
				writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000397CC File Offset: 0x000379CC
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DiscriminatedUnionConverter.UnionCase unionCase = null;
			string caseName = null;
			JArray jarray = null;
			reader.ReadAndAssert();
			Func<DiscriminatedUnionConverter.UnionCase, bool> <>9__0;
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = reader.Value.ToString();
				if (string.Equals(text, "Case", StringComparison.OrdinalIgnoreCase))
				{
					reader.ReadAndAssert();
					DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(objectType);
					caseName = reader.Value.ToString();
					IEnumerable<DiscriminatedUnionConverter.UnionCase> cases = union.Cases;
					Func<DiscriminatedUnionConverter.UnionCase, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (DiscriminatedUnionConverter.UnionCase c) => c.Name == caseName);
					}
					unionCase = cases.SingleOrDefault(func);
					if (unionCase == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
					}
				}
				else
				{
					if (!string.Equals(text, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text));
					}
					reader.ReadAndAssert();
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jarray = (JArray)JToken.ReadFrom(reader);
				}
				reader.ReadAndAssert();
			}
			if (unionCase == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			object[] array = new object[unionCase.Fields.Length];
			if (unionCase.Fields.Length != 0 && jarray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jarray != null)
			{
				if (unionCase.Fields.Length != jarray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties defined by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
				}
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					PropertyInfo propertyInfo = unionCase.Fields[i];
					array[i] = jtoken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			object[] array2 = new object[] { array };
			return unionCase.Constructor.Invoke(array2);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000399F4 File Offset: 0x00037BF4
		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(true);
			bool flag = false;
			object[] array = customAttributes;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i].GetType();
				if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			MethodCall<object, object> isUnion = FSharpUtils.Instance.IsUnion;
			object obj = null;
			object[] array2 = new object[2];
			array2[0] = objectType;
			return (bool)isUnion(obj, array2);
		}

		// Token: 0x04000499 RID: 1177
		private const string CasePropertyName = "Case";

		// Token: 0x0400049A RID: 1178
		private const string FieldsPropertyName = "Fields";

		// Token: 0x0400049B RID: 1179
		private static readonly ThreadSafeStore<Type, DiscriminatedUnionConverter.Union> UnionCache = new ThreadSafeStore<Type, DiscriminatedUnionConverter.Union>(new Func<Type, DiscriminatedUnionConverter.Union>(DiscriminatedUnionConverter.CreateUnion));

		// Token: 0x0400049C RID: 1180
		private static readonly ThreadSafeStore<Type, Type> UnionTypeLookupCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(DiscriminatedUnionConverter.CreateUnionTypeLookup));

		// Token: 0x0200028D RID: 653
		[Nullable(0)]
		internal class Union
		{
			// Token: 0x06001874 RID: 6260 RVA: 0x00069CA4 File Offset: 0x00067EA4
			public Union(FSharpFunction tagReader, List<DiscriminatedUnionConverter.UnionCase> cases)
			{
				this.TagReader = tagReader;
				this.Cases = cases;
			}

			// Token: 0x04000B24 RID: 2852
			public readonly FSharpFunction TagReader;

			// Token: 0x04000B25 RID: 2853
			public readonly List<DiscriminatedUnionConverter.UnionCase> Cases;
		}

		// Token: 0x0200028E RID: 654
		[Nullable(0)]
		internal class UnionCase
		{
			// Token: 0x06001875 RID: 6261 RVA: 0x00069CBC File Offset: 0x00067EBC
			public UnionCase(int tag, string name, PropertyInfo[] fields, FSharpFunction fieldReader, FSharpFunction constructor)
			{
				this.Tag = tag;
				this.Name = name;
				this.Fields = fields;
				this.FieldReader = fieldReader;
				this.Constructor = constructor;
			}

			// Token: 0x04000B26 RID: 2854
			public readonly int Tag;

			// Token: 0x04000B27 RID: 2855
			public readonly string Name;

			// Token: 0x04000B28 RID: 2856
			public readonly PropertyInfo[] Fields;

			// Token: 0x04000B29 RID: 2857
			public readonly FSharpFunction FieldReader;

			// Token: 0x04000B2A RID: 2858
			public readonly FSharpFunction Constructor;
		}
	}
}
