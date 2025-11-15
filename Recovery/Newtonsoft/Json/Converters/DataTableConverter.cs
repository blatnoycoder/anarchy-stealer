using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000116 RID: 278
	[NullableContext(1)]
	[Nullable(0)]
	public class DataTableConverter : JsonConverter
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x00039004 File Offset: 0x00037204
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DataTable dataTable = (DataTable)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartArray();
			foreach (object obj in dataTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				writer.WriteStartObject();
				foreach (object obj2 in dataRow.Table.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj2;
					object obj3 = dataRow[dataColumn];
					if (serializer.NullValueHandling != NullValueHandling.Ignore || (obj3 != null && obj3 != DBNull.Value))
					{
						writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(dataColumn.ColumnName) : dataColumn.ColumnName);
						serializer.Serialize(writer, obj3);
					}
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00039148 File Offset: 0x00037348
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DataTable dataTable = existingValue as DataTable;
			if (dataTable == null)
			{
				dataTable = ((objectType == typeof(DataTable)) ? new DataTable() : ((DataTable)Activator.CreateInstance(objectType)));
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				dataTable.TableName = (string)reader.Value;
				reader.ReadAndAssert();
				if (reader.TokenType == JsonToken.Null)
				{
					return dataTable;
				}
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			reader.ReadAndAssert();
			while (reader.TokenType != JsonToken.EndArray)
			{
				DataTableConverter.CreateRow(reader, dataTable, serializer);
				reader.ReadAndAssert();
			}
			return dataTable;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00039224 File Offset: 0x00037424
		private static void CreateRow(JsonReader reader, DataTable dt, JsonSerializer serializer)
		{
			DataRow dataRow = dt.NewRow();
			reader.ReadAndAssert();
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = (string)reader.Value;
				reader.ReadAndAssert();
				DataColumn dataColumn = dt.Columns[text];
				if (dataColumn == null)
				{
					Type columnDataType = DataTableConverter.GetColumnDataType(reader);
					dataColumn = new DataColumn(text, columnDataType);
					dt.Columns.Add(dataColumn);
				}
				if (dataColumn.DataType == typeof(DataTable))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						reader.ReadAndAssert();
					}
					DataTable dataTable = new DataTable();
					while (reader.TokenType != JsonToken.EndArray)
					{
						DataTableConverter.CreateRow(reader, dataTable, serializer);
						reader.ReadAndAssert();
					}
					dataRow[text] = dataTable;
				}
				else if (dataColumn.DataType.IsArray && dataColumn.DataType != typeof(byte[]))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						reader.ReadAndAssert();
					}
					List<object> list = new List<object>();
					while (reader.TokenType != JsonToken.EndArray)
					{
						list.Add(reader.Value);
						reader.ReadAndAssert();
					}
					Array array = Array.CreateInstance(dataColumn.DataType.GetElementType(), list.Count);
					((ICollection)list).CopyTo(array, 0);
					dataRow[text] = array;
				}
				else
				{
					object obj = ((reader.Value != null) ? (serializer.Deserialize(reader, dataColumn.DataType) ?? DBNull.Value) : DBNull.Value);
					dataRow[text] = obj;
				}
				reader.ReadAndAssert();
			}
			dataRow.EndEdit();
			dt.Rows.Add(dataRow);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000393DC File Offset: 0x000375DC
		private static Type GetColumnDataType(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			switch (tokenType)
			{
			case JsonToken.StartArray:
				reader.ReadAndAssert();
				if (reader.TokenType == JsonToken.StartObject)
				{
					return typeof(DataTable);
				}
				return DataTableConverter.GetColumnDataType(reader).MakeArrayType();
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return reader.ValueType;
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.EndArray:
				return typeof(string);
			}
			throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith(CultureInfo.InvariantCulture, tokenType));
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00039498 File Offset: 0x00037698
		public override bool CanConvert(Type valueType)
		{
			return typeof(DataTable).IsAssignableFrom(valueType);
		}
	}
}
