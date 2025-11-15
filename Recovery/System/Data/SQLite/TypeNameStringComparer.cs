using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000194 RID: 404
	internal sealed class TypeNameStringComparer : IEqualityComparer<string>, IComparer<string>
	{
		// Token: 0x06001199 RID: 4505 RVA: 0x00052EA8 File Offset: 0x000510A8
		public bool Equals(string left, string right)
		{
			return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00052EB4 File Offset: 0x000510B4
		public int GetHashCode(string value)
		{
			if (value != null)
			{
				return StringComparer.OrdinalIgnoreCase.GetHashCode(value);
			}
			throw new ArgumentNullException("value");
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00052ED4 File Offset: 0x000510D4
		public int Compare(string x, string y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return x.CompareTo(y);
		}
	}
}
