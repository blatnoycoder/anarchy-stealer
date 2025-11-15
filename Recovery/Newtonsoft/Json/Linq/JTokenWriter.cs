using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000FD RID: 253
	[NullableContext(2)]
	[Nullable(0)]
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00035570 File Offset: 0x00033770
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00035578 File Offset: 0x00033778
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00035594 File Offset: 0x00033794
		[NullableContext(1)]
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000355B8 File Offset: 0x000337B8
		public JTokenWriter()
		{
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000355C0 File Offset: 0x000337C0
		public override void Flush()
		{
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x000355C4 File Offset: 0x000337C4
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x000355CC File Offset: 0x000337CC
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x000355E0 File Offset: 0x000337E0
		[NullableContext(1)]
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.AddAndSkipParentCheck(container);
			}
			this._parent = container;
			this._current = container;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00035614 File Offset: 0x00033814
		private void RemoveParent()
		{
			this._current = this._parent;
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00035670 File Offset: 0x00033870
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00035684 File Offset: 0x00033884
		[NullableContext(1)]
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0003569C File Offset: 0x0003389C
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000356A4 File Offset: 0x000338A4
		[NullableContext(1)]
		public override void WritePropertyName(string name)
		{
			JObject jobject = this._parent as JObject;
			if (jobject != null)
			{
				jobject.Remove(name);
			}
			this.AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000356D8 File Offset: 0x000338D8
		private void AddValue(object value, JsonToken token)
		{
			this.AddValue(new JValue(value), token);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x000356E8 File Offset: 0x000338E8
		internal void AddValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				this._parent.Add(value);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					return;
				}
			}
			else
			{
				this._value = value ?? JValue.CreateNull();
				this._current = this._value;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00035764 File Offset: 0x00033964
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddValue(value, JsonToken.Integer);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00035788 File Offset: 0x00033988
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, JsonToken.Null);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003579C File Offset: 0x0003399C
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, JsonToken.Undefined);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x000357B0 File Offset: 0x000339B0
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000357C8 File Offset: 0x000339C8
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000357E0 File Offset: 0x000339E0
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000357F4 File Offset: 0x000339F4
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0003580C File Offset: 0x00033A0C
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00035824 File Offset: 0x00033A24
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0003583C File Offset: 0x00033A3C
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00035854 File Offset: 0x00033A54
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0003586C File Offset: 0x00033A6C
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00035884 File Offset: 0x00033A84
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Boolean);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003589C File Offset: 0x00033A9C
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000358B4 File Offset: 0x00033AB4
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x000358CC File Offset: 0x00033ACC
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = value.ToString(CultureInfo.InvariantCulture);
			this.AddValue(text, JsonToken.String);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000358FC File Offset: 0x00033AFC
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00035914 File Offset: 0x00033B14
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0003592C File Offset: 0x00033B2C
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00035944 File Offset: 0x00033B44
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0003596C File Offset: 0x00033B6C
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00035984 File Offset: 0x00033B84
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Bytes);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00035998 File Offset: 0x00033B98
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000359B0 File Offset: 0x00033BB0
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000359C8 File Offset: 0x00033BC8
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000359DC File Offset: 0x00033BDC
		[NullableContext(1)]
		internal override void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			JTokenReader jtokenReader = reader as JTokenReader;
			if (jtokenReader == null || !writeChildren || !writeDateConstructorAsDate || !writeComments)
			{
				base.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return;
			}
			if (jtokenReader.TokenType == JsonToken.None && !jtokenReader.Read())
			{
				return;
			}
			JToken jtoken = jtokenReader.CurrentToken.CloneToken();
			if (this._parent != null)
			{
				this._parent.Add(jtoken);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					base.InternalWriteValue(JsonToken.Null);
				}
			}
			else
			{
				this._current = jtoken;
				if (this._token == null && this._value == null)
				{
					this._token = jtoken as JContainer;
					this._value = jtoken as JValue;
				}
			}
			jtokenReader.Skip();
		}

		// Token: 0x04000463 RID: 1123
		private JContainer _token;

		// Token: 0x04000464 RID: 1124
		private JContainer _parent;

		// Token: 0x04000465 RID: 1125
		private JValue _value;

		// Token: 0x04000466 RID: 1126
		private JToken _current;
	}
}
