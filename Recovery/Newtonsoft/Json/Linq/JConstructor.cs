using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000EF RID: 239
	[NullableContext(1)]
	[Nullable(0)]
	public class JConstructor : JContainer
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0002F9F4 File Offset: 0x0002DBF4
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0002F9FC File Offset: 0x0002DBFC
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0002FA14 File Offset: 0x0002DC14
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JConstructor jconstructor = content as JConstructor;
			if (jconstructor == null)
			{
				return;
			}
			if (jconstructor.Name != null)
			{
				this.Name = jconstructor.Name;
			}
			JContainer.MergeEnumerableContent(this, jconstructor, settings);
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0002FA54 File Offset: 0x0002DC54
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0002FA5C File Offset: 0x0002DC5C
		[Nullable(2)]
		public string Name
		{
			[NullableContext(2)]
			get
			{
				return this._name;
			}
			[NullableContext(2)]
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0002FA68 File Offset: 0x0002DC68
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0002FA6C File Offset: 0x0002DC6C
		public JConstructor()
		{
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0002FA80 File Offset: 0x0002DC80
		public JConstructor(JConstructor other)
			: base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002FAA0 File Offset: 0x0002DCA0
		public JConstructor(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002FAAC File Offset: 0x0002DCAC
		public JConstructor(string name, object content)
			: this(name)
		{
			this.Add(content);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0002FABC File Offset: 0x0002DCBC
		public JConstructor(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Constructor name cannot be empty.", "name");
			}
			this._name = name;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002FB14 File Offset: 0x0002DD14
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002FB54 File Offset: 0x0002DD54
		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002FB5C File Offset: 0x0002DD5C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			int count = this._values.Count;
			for (int i = 0; i < count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x170001E4 RID: 484
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int num = (int)key;
					return this.GetItem(num);
				}
				throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int num = (int)key;
					this.SetItem(num, value);
					return;
				}
				throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002FC60 File Offset: 0x0002DE60
		internal override int GetDeepHashCode()
		{
			string name = this._name;
			return ((name != null) ? name.GetHashCode() : 0) ^ base.ContentsHashCode();
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002FC84 File Offset: 0x0002DE84
		public new static JConstructor Load(JsonReader reader)
		{
			return JConstructor.Load(reader, null);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002FC90 File Offset: 0x0002DE90
		public new static JConstructor Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			jconstructor.ReadTokenFrom(reader, settings);
			return jconstructor;
		}

		// Token: 0x04000427 RID: 1063
		[Nullable(2)]
		private string _name;

		// Token: 0x04000428 RID: 1064
		private readonly List<JToken> _values = new List<JToken>();
	}
}
