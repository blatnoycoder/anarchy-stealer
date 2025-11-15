using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200009E RID: 158
	[NullableContext(2)]
	[Nullable(0)]
	internal struct StringBuffer
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001EA64 File Offset: 0x0001CC64
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x0001EA6C File Offset: 0x0001CC6C
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001EA78 File Offset: 0x0001CC78
		public bool IsEmpty
		{
			get
			{
				return this._buffer == null;
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001EA84 File Offset: 0x0001CC84
		public StringBuffer(IArrayPool<char> bufferPool, int initalSize)
		{
			this = new StringBuffer(BufferUtils.RentBuffer(bufferPool, initalSize));
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001EA94 File Offset: 0x0001CC94
		[NullableContext(1)]
		private StringBuffer(char[] buffer)
		{
			this._buffer = buffer;
			this._position = 0;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
		public void Append(IArrayPool<char> bufferPool, char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(bufferPool, 1);
			}
			char[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			buffer[position] = value;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001EAEC File Offset: 0x0001CCEC
		[NullableContext(1)]
		public void Append([Nullable(2)] IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
		{
			if (this._position + count >= this._buffer.Length)
			{
				this.EnsureSize(bufferPool, count);
			}
			Array.Copy(buffer, startIndex, this._buffer, this._position, count);
			this._position += count;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001EB40 File Offset: 0x0001CD40
		public void Clear(IArrayPool<char> bufferPool)
		{
			if (this._buffer != null)
			{
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
				this._buffer = null;
			}
			this._position = 0;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001EB68 File Offset: 0x0001CD68
		private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
		{
			char[] array = BufferUtils.RentBuffer(bufferPool, (this._position + appendLength) * 2);
			if (this._buffer != null)
			{
				Array.Copy(this._buffer, array, this._position);
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
			}
			this._buffer = array;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001EBBC File Offset: 0x0001CDBC
		[NullableContext(1)]
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001EBCC File Offset: 0x0001CDCC
		[NullableContext(1)]
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001EBDC File Offset: 0x0001CDDC
		public char[] InternalBuffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x040002C9 RID: 713
		private char[] _buffer;

		// Token: 0x040002CA RID: 714
		private int _position;
	}
}
