using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StringReference
	{
		// Token: 0x170000F4 RID: 244
		public char this[int i]
		{
			get
			{
				return this._chars[i];
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001EBF8 File Offset: 0x0001CDF8
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001EC00 File Offset: 0x0001CE00
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001EC08 File Offset: 0x0001CE08
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001EC20 File Offset: 0x0001CE20
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x040002CB RID: 715
		private readonly char[] _chars;

		// Token: 0x040002CC RID: 716
		private readonly int _startIndex;

		// Token: 0x040002CD RID: 717
		private readonly int _length;
	}
}
