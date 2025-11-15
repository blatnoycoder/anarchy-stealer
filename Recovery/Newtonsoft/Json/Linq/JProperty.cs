using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000F3 RID: 243
	[NullableContext(1)]
	[Nullable(0)]
	public class JProperty : JContainer
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00031B70 File Offset: 0x0002FD70
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00031B78 File Offset: 0x0002FD78
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00031B80 File Offset: 0x0002FD80
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x00031B90 File Offset: 0x0002FD90
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken jtoken = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, jtoken, false);
					return;
				}
				this.SetItem(0, jtoken);
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00031BD8 File Offset: 0x0002FDD8
		public JProperty(JProperty other)
			: base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00031BF8 File Offset: 0x0002FDF8
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00031C0C File Offset: 0x0002FE0C
		[NullableContext(2)]
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject jobject = (JObject)base.Parent;
			if (jobject != null)
			{
				jobject.InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			JObject jobject2 = (JObject)base.Parent;
			if (jobject2 == null)
			{
				return;
			}
			jobject2.InternalPropertyChanged(this);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00031C7C File Offset: 0x0002FE7C
		[NullableContext(2)]
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00031C9C File Offset: 0x0002FE9C
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00031CBC File Offset: 0x0002FEBC
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._content.IndexOf(item);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00031CD4 File Offset: 0x0002FED4
		[NullableContext(2)]
		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			base.InsertItem(0, item, false);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00031D2C File Offset: 0x0002FF2C
		[NullableContext(2)]
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00031D38 File Offset: 0x0002FF38
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			JToken jtoken = ((jproperty != null) ? jproperty.Value : null);
			if (jtoken != null && jtoken.Type != JTokenType.Null)
			{
				this.Value = jtoken;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00031D7C File Offset: 0x0002FF7C
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00031D9C File Offset: 0x0002FF9C
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00031DDC File Offset: 0x0002FFDC
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00031DE4 File Offset: 0x0002FFE4
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00031DE8 File Offset: 0x0002FFE8
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00031E10 File Offset: 0x00030010
		public JProperty(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00031E1C File Offset: 0x0003001C
		public JProperty(string name, [Nullable(2)] object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00031E74 File Offset: 0x00030074
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00031EB0 File Offset: 0x000300B0
		internal override int GetDeepHashCode()
		{
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			return hashCode ^ ((value != null) ? value.GetDeepHashCode() : 0);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00031EE8 File Offset: 0x000300E8
		public new static JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00031EF4 File Offset: 0x000300F4
		public new static JProperty Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jproperty.ReadTokenFrom(reader, settings);
			return jproperty;
		}

		// Token: 0x04000433 RID: 1075
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x04000434 RID: 1076
		private readonly string _name;

		// Token: 0x0200027B RID: 635
		[Nullable(0)]
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			// Token: 0x060017DE RID: 6110 RVA: 0x00067640 File Offset: 0x00065840
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x060017DF RID: 6111 RVA: 0x00067650 File Offset: 0x00065850
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060017E0 RID: 6112 RVA: 0x00067658 File Offset: 0x00065858
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x060017E1 RID: 6113 RVA: 0x00067664 File Offset: 0x00065864
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x060017E2 RID: 6114 RVA: 0x00067670 File Offset: 0x00065870
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x060017E3 RID: 6115 RVA: 0x0006767C File Offset: 0x0006587C
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x060017E4 RID: 6116 RVA: 0x00067698 File Offset: 0x00065898
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x170003D9 RID: 985
			// (get) Token: 0x060017E5 RID: 6117 RVA: 0x000676B0 File Offset: 0x000658B0
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x170003DA RID: 986
			// (get) Token: 0x060017E6 RID: 6118 RVA: 0x000676C0 File Offset: 0x000658C0
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060017E7 RID: 6119 RVA: 0x000676C4 File Offset: 0x000658C4
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x060017E8 RID: 6120 RVA: 0x000676D8 File Offset: 0x000658D8
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x060017E9 RID: 6121 RVA: 0x000676E8 File Offset: 0x000658E8
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x170003DB RID: 987
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					return this._token;
				}
				set
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					this._token = value;
				}
			}

			// Token: 0x04000A9F RID: 2719
			[Nullable(2)]
			internal JToken _token;
		}
	}
}
