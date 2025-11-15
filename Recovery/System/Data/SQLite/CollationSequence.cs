using System;

namespace System.Data.SQLite
{
	// Token: 0x020001A8 RID: 424
	public struct CollationSequence
	{
		// Token: 0x0600125B RID: 4699 RVA: 0x00057A58 File Offset: 0x00055C58
		public int Compare(string s1, string s2)
		{
			return this._func._base.ContextCollateCompare(this.Encoding, this._func._context, s1, s2);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00057A80 File Offset: 0x00055C80
		public int Compare(char[] c1, char[] c2)
		{
			return this._func._base.ContextCollateCompare(this.Encoding, this._func._context, c1, c2);
		}

		// Token: 0x040007D4 RID: 2004
		public string Name;

		// Token: 0x040007D5 RID: 2005
		public CollationTypeEnum Type;

		// Token: 0x040007D6 RID: 2006
		public CollationEncodingEnum Encoding;

		// Token: 0x040007D7 RID: 2007
		internal SQLiteFunction _func;
	}
}
