using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(0)]
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0002F658 File Offset: 0x0002D858
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0002F660 File Offset: 0x0002D860
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0002F664 File Offset: 0x0002D864
		public JArray()
		{
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002F678 File Offset: 0x0002D878
		public JArray(JArray other)
			: base(other)
		{
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002F68C File Offset: 0x0002D88C
		public JArray(params object[] content)
			: this(content)
		{
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002F698 File Offset: 0x0002D898
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002F6B4 File Offset: 0x0002D8B4
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002F6DC File Offset: 0x0002D8DC
		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002F6E4 File Offset: 0x0002D8E4
		public new static JArray Load(JsonReader reader)
		{
			return JArray.Load(reader, null);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002F6F0 File Offset: 0x0002D8F0
		public new static JArray Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo, settings);
			jarray.ReadTokenFrom(reader, settings);
			return jarray;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002F774 File Offset: 0x0002D974
		public new static JArray Parse(string json)
		{
			return JArray.Parse(json, null);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002F780 File Offset: 0x0002D980
		public new static JArray Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JArray jarray2;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JArray jarray = JArray.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				jarray2 = jarray;
			}
			return jarray2;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002F7D0 File Offset: 0x0002D9D0
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0002F7E0 File Offset: 0x0002D9E0
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JArray)jtoken;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0002F82C File Offset: 0x0002DA2C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < this._values.Count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x170001DE RID: 478
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x170001DF RID: 479
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002F910 File Offset: 0x0002DB10
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002F928 File Offset: 0x0002DB28
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			IEnumerable enumerable = ((base.IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null);
			if (enumerable == null)
			{
				return;
			}
			JContainer.MergeEnumerableContent(this, enumerable, settings);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0002F96C File Offset: 0x0002DB6C
		public int IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0002F978 File Offset: 0x0002DB78
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002F984 File Offset: 0x0002DB84
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002F990 File Offset: 0x0002DB90
		public IEnumerator<JToken> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002F9B0 File Offset: 0x0002DBB0
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002F9BC File Offset: 0x0002DBBC
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002F9D0 File Offset: 0x0002DBD0
		public void CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0002F9DC File Offset: 0x0002DBDC
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002F9E0 File Offset: 0x0002DBE0
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002F9EC File Offset: 0x0002DBEC
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x04000426 RID: 1062
		private readonly List<JToken> _values = new List<JToken>();
	}
}
