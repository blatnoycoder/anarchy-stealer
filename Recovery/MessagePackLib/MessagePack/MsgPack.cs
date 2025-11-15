using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MessagePackLib.MessagePack
{
	// Token: 0x02000032 RID: 50
	public class MsgPack : IEnumerable
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0000834C File Offset: 0x0000654C
		private void SetName(string value)
		{
			this.name = value;
			this.lowerName = this.name.ToLower();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008368 File Offset: 0x00006568
		private void Clear()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].Clear();
			}
			this.children.Clear();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000083B0 File Offset: 0x000065B0
		private MsgPack InnerAdd()
		{
			MsgPack msgPack = new MsgPack();
			msgPack.parent = this;
			this.children.Add(msgPack);
			return msgPack;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000083DC File Offset: 0x000065DC
		private int IndexOf(string name)
		{
			int num = -1;
			int num2 = -1;
			string text = name.ToLower();
			foreach (MsgPack msgPack in this.children)
			{
				num++;
				if (text.Equals(msgPack.lowerName))
				{
					num2 = num;
					break;
				}
			}
			return num2;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008458 File Offset: 0x00006658
		public MsgPack FindObject(string name)
		{
			int num = this.IndexOf(name);
			if (num == -1)
			{
				return null;
			}
			return this.children[num];
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00008488 File Offset: 0x00006688
		private MsgPack InnerAddMapChild()
		{
			if (this.valueType != MsgPackType.Map)
			{
				this.Clear();
				this.valueType = MsgPackType.Map;
			}
			return this.InnerAdd();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000084AC File Offset: 0x000066AC
		private MsgPack InnerAddArrayChild()
		{
			if (this.valueType != MsgPackType.Array)
			{
				this.Clear();
				this.valueType = MsgPackType.Array;
			}
			return this.InnerAdd();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000084D0 File Offset: 0x000066D0
		public MsgPack AddArrayChild()
		{
			return this.InnerAddArrayChild();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000084D8 File Offset: 0x000066D8
		private void WriteMap(Stream ms)
		{
			int count = this.children.Count;
			if (count <= 15)
			{
				byte b = 128 + (byte)count;
				ms.WriteByte(b);
			}
			else if (count <= 65535)
			{
				byte b = 222;
				ms.WriteByte(b);
				byte[] array = BytesTools.SwapBytes(BitConverter.GetBytes((short)count));
				ms.Write(array, 0, array.Length);
			}
			else
			{
				byte b = 223;
				ms.WriteByte(b);
				byte[] array = BytesTools.SwapBytes(BitConverter.GetBytes(count));
				ms.Write(array, 0, array.Length);
			}
			for (int i = 0; i < count; i++)
			{
				WriteTools.WriteString(ms, this.children[i].name);
				this.children[i].Encode2Stream(ms);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000085A4 File Offset: 0x000067A4
		private void WirteArray(Stream ms)
		{
			int count = this.children.Count;
			if (count <= 15)
			{
				byte b = 144 + (byte)count;
				ms.WriteByte(b);
			}
			else if (count <= 65535)
			{
				byte b = 220;
				ms.WriteByte(b);
				byte[] array = BytesTools.SwapBytes(BitConverter.GetBytes((short)count));
				ms.Write(array, 0, array.Length);
			}
			else
			{
				byte b = 221;
				ms.WriteByte(b);
				byte[] array = BytesTools.SwapBytes(BitConverter.GetBytes(count));
				ms.Write(array, 0, array.Length);
			}
			for (int i = 0; i < count; i++)
			{
				this.children[i].Encode2Stream(ms);
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000865C File Offset: 0x0000685C
		public void SetAsInteger(long value)
		{
			this.innerValue = value;
			this.valueType = MsgPackType.Integer;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00008674 File Offset: 0x00006874
		public void SetAsUInt64(ulong value)
		{
			this.innerValue = value;
			this.valueType = MsgPackType.UInt64;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000868C File Offset: 0x0000688C
		public ulong GetAsUInt64()
		{
			switch (this.valueType)
			{
			case MsgPackType.String:
				return ulong.Parse(this.innerValue.ToString().Trim());
			case MsgPackType.Integer:
				return Convert.ToUInt64((long)this.innerValue);
			case MsgPackType.UInt64:
				return (ulong)this.innerValue;
			case MsgPackType.Float:
				return Convert.ToUInt64((double)this.innerValue);
			case MsgPackType.Single:
				return Convert.ToUInt64((float)this.innerValue);
			case MsgPackType.DateTime:
				return Convert.ToUInt64((DateTime)this.innerValue);
			}
			return 0UL;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00008738 File Offset: 0x00006938
		public long GetAsInteger()
		{
			switch (this.valueType)
			{
			case MsgPackType.String:
				return long.Parse(this.innerValue.ToString().Trim());
			case MsgPackType.Integer:
				return (long)this.innerValue;
			case MsgPackType.UInt64:
				return Convert.ToInt64((long)this.innerValue);
			case MsgPackType.Float:
				return Convert.ToInt64((double)this.innerValue);
			case MsgPackType.Single:
				return Convert.ToInt64((float)this.innerValue);
			case MsgPackType.DateTime:
				return Convert.ToInt64((DateTime)this.innerValue);
			}
			return 0L;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000087E4 File Offset: 0x000069E4
		public double GetAsFloat()
		{
			switch (this.valueType)
			{
			case MsgPackType.String:
				return double.Parse((string)this.innerValue);
			case MsgPackType.Integer:
				return Convert.ToDouble((long)this.innerValue);
			case MsgPackType.Float:
				return (double)this.innerValue;
			case MsgPackType.Single:
				return (double)((float)this.innerValue);
			case MsgPackType.DateTime:
				return (double)Convert.ToInt64((DateTime)this.innerValue);
			}
			return 0.0;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000887C File Offset: 0x00006A7C
		public void SetAsBytes(byte[] value)
		{
			this.innerValue = value;
			this.valueType = MsgPackType.Binary;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00008890 File Offset: 0x00006A90
		public byte[] GetAsBytes()
		{
			switch (this.valueType)
			{
			case MsgPackType.String:
				return BytesTools.GetUtf8Bytes(this.innerValue.ToString());
			case MsgPackType.Integer:
				return BitConverter.GetBytes((long)this.innerValue);
			case MsgPackType.Float:
				return BitConverter.GetBytes((double)this.innerValue);
			case MsgPackType.Single:
				return BitConverter.GetBytes((float)this.innerValue);
			case MsgPackType.DateTime:
				return BitConverter.GetBytes(((DateTime)this.innerValue).ToBinary());
			case MsgPackType.Binary:
				return (byte[])this.innerValue;
			}
			return new byte[0];
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008944 File Offset: 0x00006B44
		public void Add(string key, string value)
		{
			MsgPack msgPack = this.InnerAddArrayChild();
			msgPack.name = key;
			msgPack.SetAsString(value);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000895C File Offset: 0x00006B5C
		public void Add(string key, int value)
		{
			MsgPack msgPack = this.InnerAddArrayChild();
			msgPack.name = key;
			msgPack.SetAsInteger((long)value);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00008974 File Offset: 0x00006B74
		public bool LoadFileAsBytes(string fileName)
		{
			if (File.Exists(fileName))
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				fileStream.Close();
				fileStream.Dispose();
				this.SetAsBytes(array);
				return true;
			}
			return false;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000089D0 File Offset: 0x00006BD0
		public bool SaveBytesToFile(string fileName)
		{
			if (this.innerValue != null)
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Append);
				fileStream.Write((byte[])this.innerValue, 0, ((byte[])this.innerValue).Length);
				fileStream.Close();
				fileStream.Dispose();
				return true;
			}
			return false;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008A20 File Offset: 0x00006C20
		public MsgPack ForcePathObject(string path)
		{
			MsgPack msgPack = this;
			string[] array = path.Trim().Split(new char[] { '.', '/', '\\' });
			if (array.Length == 0)
			{
				return null;
			}
			string text;
			if (array.Length > 1)
			{
				for (int i = 0; i < array.Length - 1; i++)
				{
					text = array[i];
					MsgPack msgPack2 = msgPack.FindObject(text);
					if (msgPack2 == null)
					{
						msgPack = msgPack.InnerAddMapChild();
						msgPack.SetName(text);
					}
					else
					{
						msgPack = msgPack2;
					}
				}
			}
			text = array[array.Length - 1];
			int num = msgPack.IndexOf(text);
			if (num > -1)
			{
				return msgPack.children[num];
			}
			msgPack = msgPack.InnerAddMapChild();
			msgPack.SetName(text);
			return msgPack;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008AE0 File Offset: 0x00006CE0
		public void SetAsNull()
		{
			this.Clear();
			this.innerValue = null;
			this.valueType = MsgPackType.Null;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00008AF8 File Offset: 0x00006CF8
		public void SetAsString(string value)
		{
			this.innerValue = value;
			this.valueType = MsgPackType.String;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008B08 File Offset: 0x00006D08
		public string GetAsString()
		{
			if (this.innerValue == null)
			{
				return "";
			}
			return this.innerValue.ToString();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008B28 File Offset: 0x00006D28
		public void SetAsBoolean(bool bVal)
		{
			this.valueType = MsgPackType.Boolean;
			this.innerValue = bVal;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008B40 File Offset: 0x00006D40
		public void SetAsSingle(float fVal)
		{
			this.valueType = MsgPackType.Single;
			this.innerValue = fVal;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008B58 File Offset: 0x00006D58
		public void SetAsFloat(double fVal)
		{
			this.valueType = MsgPackType.Float;
			this.innerValue = fVal;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008B70 File Offset: 0x00006D70
		public void DecodeFromBytes(byte[] bytes)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bytes = Zip.Decompress(bytes);
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Position = 0L;
				this.DecodeFromStream(memoryStream);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008BC8 File Offset: 0x00006DC8
		public void DecodeFromFile(string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open);
			this.DecodeFromStream(fileStream);
			fileStream.Dispose();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008BF0 File Offset: 0x00006DF0
		public void DecodeFromStream(Stream ms)
		{
			byte b = (byte)ms.ReadByte();
			if (b <= 127)
			{
				this.SetAsInteger((long)((ulong)b));
				return;
			}
			if (b >= 128 && b <= 143)
			{
				this.Clear();
				this.valueType = MsgPackType.Map;
				int num = (int)(b - 128);
				for (int i = 0; i < num; i++)
				{
					MsgPack msgPack = this.InnerAdd();
					msgPack.SetName(ReadTools.ReadString(ms));
					msgPack.DecodeFromStream(ms);
				}
				return;
			}
			if (b >= 144 && b <= 159)
			{
				this.Clear();
				this.valueType = MsgPackType.Array;
				int num = (int)(b - 144);
				for (int i = 0; i < num; i++)
				{
					this.InnerAdd().DecodeFromStream(ms);
				}
				return;
			}
			if (b >= 160 && b <= 191)
			{
				int num = (int)(b - 160);
				this.SetAsString(ReadTools.ReadString(ms, num));
				return;
			}
			if (b >= 224 && b <= 255)
			{
				this.SetAsInteger((long)((sbyte)b));
				return;
			}
			if (b == 192)
			{
				this.SetAsNull();
				return;
			}
			if (b == 193)
			{
				throw new Exception("(never used) type $c1");
			}
			if (b == 194)
			{
				this.SetAsBoolean(false);
				return;
			}
			if (b == 195)
			{
				this.SetAsBoolean(true);
				return;
			}
			if (b == 196)
			{
				int num = ms.ReadByte();
				byte[] array = new byte[num];
				ms.Read(array, 0, num);
				this.SetAsBytes(array);
				return;
			}
			if (b == 197)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				array = BytesTools.SwapBytes(array);
				int num = (int)BitConverter.ToUInt16(array, 0);
				array = new byte[num];
				ms.Read(array, 0, num);
				this.SetAsBytes(array);
				return;
			}
			if (b == 198)
			{
				byte[] array = new byte[4];
				ms.Read(array, 0, 4);
				array = BytesTools.SwapBytes(array);
				int num = BitConverter.ToInt32(array, 0);
				array = new byte[num];
				ms.Read(array, 0, num);
				this.SetAsBytes(array);
				return;
			}
			if (b == 199 || b == 200 || b == 201)
			{
				throw new Exception("(ext8,ext16,ex32) type $c7,$c8,$c9");
			}
			if (b == 202)
			{
				byte[] array = new byte[4];
				ms.Read(array, 0, 4);
				array = BytesTools.SwapBytes(array);
				this.SetAsSingle(BitConverter.ToSingle(array, 0));
				return;
			}
			if (b == 203)
			{
				byte[] array = new byte[8];
				ms.Read(array, 0, 8);
				array = BytesTools.SwapBytes(array);
				this.SetAsFloat(BitConverter.ToDouble(array, 0));
				return;
			}
			if (b == 204)
			{
				b = (byte)ms.ReadByte();
				this.SetAsInteger((long)((ulong)b));
				return;
			}
			if (b == 205)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				array = BytesTools.SwapBytes(array);
				this.SetAsInteger((long)((ulong)BitConverter.ToUInt16(array, 0)));
				return;
			}
			if (b == 206)
			{
				byte[] array = new byte[4];
				ms.Read(array, 0, 4);
				array = BytesTools.SwapBytes(array);
				this.SetAsInteger((long)((ulong)BitConverter.ToUInt32(array, 0)));
				return;
			}
			if (b == 207)
			{
				byte[] array = new byte[8];
				ms.Read(array, 0, 8);
				array = BytesTools.SwapBytes(array);
				this.SetAsUInt64(BitConverter.ToUInt64(array, 0));
				return;
			}
			if (b == 220)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				array = BytesTools.SwapBytes(array);
				int num = (int)BitConverter.ToInt16(array, 0);
				this.Clear();
				this.valueType = MsgPackType.Array;
				for (int i = 0; i < num; i++)
				{
					this.InnerAdd().DecodeFromStream(ms);
				}
				return;
			}
			if (b == 221)
			{
				byte[] array = new byte[4];
				ms.Read(array, 0, 4);
				array = BytesTools.SwapBytes(array);
				int num = (int)BitConverter.ToInt16(array, 0);
				this.Clear();
				this.valueType = MsgPackType.Array;
				for (int i = 0; i < num; i++)
				{
					this.InnerAdd().DecodeFromStream(ms);
				}
				return;
			}
			if (b == 217)
			{
				this.SetAsString(ReadTools.ReadString(b, ms));
				return;
			}
			if (b == 222)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				array = BytesTools.SwapBytes(array);
				int num = (int)BitConverter.ToInt16(array, 0);
				this.Clear();
				this.valueType = MsgPackType.Map;
				for (int i = 0; i < num; i++)
				{
					MsgPack msgPack2 = this.InnerAdd();
					msgPack2.SetName(ReadTools.ReadString(ms));
					msgPack2.DecodeFromStream(ms);
				}
				return;
			}
			if (b == 222)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				array = BytesTools.SwapBytes(array);
				int num = (int)BitConverter.ToInt16(array, 0);
				this.Clear();
				this.valueType = MsgPackType.Map;
				for (int i = 0; i < num; i++)
				{
					MsgPack msgPack3 = this.InnerAdd();
					msgPack3.SetName(ReadTools.ReadString(ms));
					msgPack3.DecodeFromStream(ms);
				}
				return;
			}
			if (b == 223)
			{
				byte[] array = new byte[4];
				ms.Read(array, 0, 4);
				array = BytesTools.SwapBytes(array);
				int num = BitConverter.ToInt32(array, 0);
				this.Clear();
				this.valueType = MsgPackType.Map;
				for (int i = 0; i < num; i++)
				{
					MsgPack msgPack4 = this.InnerAdd();
					msgPack4.SetName(ReadTools.ReadString(ms));
					msgPack4.DecodeFromStream(ms);
				}
				return;
			}
			if (b == 218)
			{
				this.SetAsString(ReadTools.ReadString(b, ms));
				return;
			}
			if (b == 219)
			{
				this.SetAsString(ReadTools.ReadString(b, ms));
				return;
			}
			if (b == 208)
			{
				this.SetAsInteger((long)((sbyte)ms.ReadByte()));
				return;
			}
			if (b == 209)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				array = BytesTools.SwapBytes(array);
				this.SetAsInteger((long)BitConverter.ToInt16(array, 0));
				return;
			}
			if (b == 210)
			{
				byte[] array = new byte[4];
				ms.Read(array, 0, 4);
				array = BytesTools.SwapBytes(array);
				this.SetAsInteger((long)BitConverter.ToInt32(array, 0));
				return;
			}
			if (b == 211)
			{
				byte[] array = new byte[8];
				ms.Read(array, 0, 8);
				array = BytesTools.SwapBytes(array);
				this.SetAsInteger(BitConverter.ToInt64(array, 0));
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00009208 File Offset: 0x00007408
		public byte[] Encode2Bytes()
		{
			byte[] array2;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.Encode2Stream(memoryStream);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Position = 0L;
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				array2 = Zip.Compress(array);
			}
			return array2;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00009274 File Offset: 0x00007474
		public void Encode2Stream(Stream ms)
		{
			switch (this.valueType)
			{
			case MsgPackType.Unknown:
			case MsgPackType.Null:
				WriteTools.WriteNull(ms);
				return;
			case MsgPackType.Map:
				this.WriteMap(ms);
				return;
			case MsgPackType.Array:
				this.WirteArray(ms);
				return;
			case MsgPackType.String:
				WriteTools.WriteString(ms, (string)this.innerValue);
				return;
			case MsgPackType.Integer:
				WriteTools.WriteInteger(ms, (long)this.innerValue);
				return;
			case MsgPackType.UInt64:
				WriteTools.WriteUInt64(ms, (ulong)this.innerValue);
				return;
			case MsgPackType.Boolean:
				WriteTools.WriteBoolean(ms, (bool)this.innerValue);
				return;
			case MsgPackType.Float:
				WriteTools.WriteFloat(ms, (double)this.innerValue);
				return;
			case MsgPackType.Single:
				WriteTools.WriteFloat(ms, (double)((float)this.innerValue));
				return;
			case MsgPackType.DateTime:
				WriteTools.WriteInteger(ms, this.GetAsInteger());
				return;
			case MsgPackType.Binary:
				WriteTools.WriteBinary(ms, (byte[])this.innerValue);
				return;
			default:
				WriteTools.WriteNull(ms);
				return;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00009370 File Offset: 0x00007570
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00009378 File Offset: 0x00007578
		public string AsString
		{
			get
			{
				return this.GetAsString();
			}
			set
			{
				this.SetAsString(value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00009384 File Offset: 0x00007584
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000938C File Offset: 0x0000758C
		public long AsInteger
		{
			get
			{
				return this.GetAsInteger();
			}
			set
			{
				this.SetAsInteger(value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00009398 File Offset: 0x00007598
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000093A0 File Offset: 0x000075A0
		public double AsFloat
		{
			get
			{
				return this.GetAsFloat();
			}
			set
			{
				this.SetAsFloat(value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000093AC File Offset: 0x000075AC
		public MsgPackArray AsArray
		{
			get
			{
				lock (this)
				{
					if (this.refAsArray == null)
					{
						this.refAsArray = new MsgPackArray(this, this.children);
					}
				}
				return this.refAsArray;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000940C File Offset: 0x0000760C
		public MsgPackType ValueType
		{
			get
			{
				return this.valueType;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00009414 File Offset: 0x00007614
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new MsgPackEnum(this.children);
		}

		// Token: 0x040000BB RID: 187
		private string name;

		// Token: 0x040000BC RID: 188
		private string lowerName;

		// Token: 0x040000BD RID: 189
		private object innerValue;

		// Token: 0x040000BE RID: 190
		private MsgPackType valueType;

		// Token: 0x040000BF RID: 191
		private MsgPack parent;

		// Token: 0x040000C0 RID: 192
		private List<MsgPack> children = new List<MsgPack>();

		// Token: 0x040000C1 RID: 193
		private MsgPackArray refAsArray;
	}
}
