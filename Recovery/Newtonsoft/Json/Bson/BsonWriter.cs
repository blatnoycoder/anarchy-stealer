using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000146 RID: 326
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonWriter : JsonWriter
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0003F4F4 File Offset: 0x0003D6F4
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x0003F504 File Offset: 0x0003D704
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0003F514 File Offset: 0x0003D714
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003F538 File Offset: 0x0003D738
		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0003F558 File Offset: 0x0003D758
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0003F568 File Offset: 0x0003D768
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003F594 File Offset: 0x0003D794
		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0003F5A4 File Offset: 0x0003D7A4
		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0003F5B4 File Offset: 0x0003D7B4
		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0003F5C4 File Offset: 0x0003D7C4
		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0003F5D4 File Offset: 0x0003D7D4
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0003F5E8 File Offset: 0x0003D7E8
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0003F5FC File Offset: 0x0003D7FC
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0003F60C File Offset: 0x0003D80C
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput)
			{
				BsonBinaryWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0003F634 File Offset: 0x0003D834
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0003F644 File Offset: 0x0003D844
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0003F658 File Offset: 0x0003D858
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0003F668 File Offset: 0x0003D868
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				BsonObject bsonObject = this._parent as BsonObject;
				if (bsonObject != null)
				{
					bsonObject.Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0003F708 File Offset: 0x0003D908
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value;
				base.SetWriteState(JsonToken.Integer, null);
				this.AddToken(new BsonBinary(bigInteger.ToByteArray(), BsonBinaryType.Binary));
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0003F750 File Offset: 0x0003D950
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddToken(BsonEmpty.Null);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0003F764 File Offset: 0x0003D964
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddToken(BsonEmpty.Undefined);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003F778 File Offset: 0x0003D978
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddToken((value == null) ? BsonEmpty.Null : new BsonString(value, true));
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003F7A0 File Offset: 0x0003D9A0
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0003F7B8 File Offset: 0x0003D9B8
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0003F7E8 File Offset: 0x0003D9E8
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0003F800 File Offset: 0x0003DA00
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0003F834 File Offset: 0x0003DA34
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0003F84C File Offset: 0x0003DA4C
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0003F864 File Offset: 0x0003DA64
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddToken(value ? BsonBoolean.True : BsonBoolean.False);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0003F888 File Offset: 0x0003DA88
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0003F8A0 File Offset: 0x0003DAA0
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0003F8B8 File Offset: 0x0003DAB8
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(text, true));
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0003F8EC File Offset: 0x0003DAEC
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0003F904 File Offset: 0x0003DB04
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0003F91C File Offset: 0x0003DB1C
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0003F934 File Offset: 0x0003DB34
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0003F95C File Offset: 0x0003DB5C
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0003F974 File Offset: 0x0003DB74
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0003F998 File Offset: 0x0003DB98
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0003F9B4 File Offset: 0x0003DBB4
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0003F9D8 File Offset: 0x0003DBD8
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0003FA18 File Offset: 0x0003DC18
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003FA50 File Offset: 0x0003DC50
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x04000509 RID: 1289
		private readonly BsonBinaryWriter _writer;

		// Token: 0x0400050A RID: 1290
		private BsonToken _root;

		// Token: 0x0400050B RID: 1291
		private BsonToken _parent;

		// Token: 0x0400050C RID: 1292
		private string _propertyName;
	}
}
