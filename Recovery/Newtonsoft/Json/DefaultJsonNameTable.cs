using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	public class DefaultJsonNameTable : JsonNameTable
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00009A4C File Offset: 0x00007C4C
		public DefaultJsonNameTable()
		{
			this._entries = new DefaultJsonNameTable.Entry[this._mask + 1];
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009A70 File Offset: 0x00007C70
		[return: Nullable(2)]
		public override string Get(char[] key, int start, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + DefaultJsonNameTable.HashCodeRandomizer;
			num += (num << 7) ^ (int)key[start];
			int num2 = start + length;
			for (int i = start + 1; i < num2; i++)
			{
				num += (num << 7) ^ (int)key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			int num3 = num & this._mask;
			for (DefaultJsonNameTable.Entry entry = this._entries[num3]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && DefaultJsonNameTable.TextEquals(entry.Value, key, start, length))
				{
					return entry.Value;
				}
			}
			return null;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00009B28 File Offset: 0x00007D28
		public string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int length = key.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + DefaultJsonNameTable.HashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7) ^ (int)key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			for (DefaultJsonNameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && entry.Value.Equals(key, StringComparison.Ordinal))
				{
					return entry.Value;
				}
			}
			return this.AddEntry(key, num);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00009BEC File Offset: 0x00007DEC
		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & this._mask;
			DefaultJsonNameTable.Entry entry = new DefaultJsonNameTable.Entry(str, hashCode, this._entries[num]);
			this._entries[num] = entry;
			int count = this._count;
			this._count = count + 1;
			if (count == this._mask)
			{
				this.Grow();
			}
			return entry.Value;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009C50 File Offset: 0x00007E50
		private void Grow()
		{
			DefaultJsonNameTable.Entry[] entries = this._entries;
			int num = this._mask * 2 + 1;
			DefaultJsonNameTable.Entry[] array = new DefaultJsonNameTable.Entry[num + 1];
			foreach (DefaultJsonNameTable.Entry entry in entries)
			{
				while (entry != null)
				{
					int num2 = entry.HashCode & num;
					DefaultJsonNameTable.Entry next = entry.Next;
					entry.Next = array[num2];
					array[num2] = entry;
					entry = next;
				}
			}
			this._entries = array;
			this._mask = num;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00009CE0 File Offset: 0x00007EE0
		private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
		{
			if (str1.Length != str2Length)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[str2Start + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000E3 RID: 227
		private static readonly int HashCodeRandomizer = Environment.TickCount;

		// Token: 0x040000E4 RID: 228
		private int _count;

		// Token: 0x040000E5 RID: 229
		private DefaultJsonNameTable.Entry[] _entries;

		// Token: 0x040000E6 RID: 230
		private int _mask = 31;

		// Token: 0x0200021C RID: 540
		[Nullable(0)]
		private class Entry
		{
			// Token: 0x060016C5 RID: 5829 RVA: 0x00065750 File Offset: 0x00063950
			internal Entry(string value, int hashCode, DefaultJsonNameTable.Entry next)
			{
				this.Value = value;
				this.HashCode = hashCode;
				this.Next = next;
			}

			// Token: 0x040009A6 RID: 2470
			internal readonly string Value;

			// Token: 0x040009A7 RID: 2471
			internal readonly int HashCode;

			// Token: 0x040009A8 RID: 2472
			internal DefaultJsonNameTable.Entry Next;
		}
	}
}
