using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000192 RID: 402
	internal sealed class SQLiteDbTypeMap : Dictionary<string, SQLiteDbTypeMapping>
	{
		// Token: 0x06001190 RID: 4496 RVA: 0x00052CF8 File Offset: 0x00050EF8
		public SQLiteDbTypeMap()
			: base(new TypeNameStringComparer())
		{
			this.reverse = new Dictionary<DbType, SQLiteDbTypeMapping>();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00052D10 File Offset: 0x00050F10
		public SQLiteDbTypeMap(IEnumerable<SQLiteDbTypeMapping> collection)
			: this()
		{
			this.Add(collection);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00052D20 File Offset: 0x00050F20
		public new int Clear()
		{
			int num = 0;
			if (this.reverse != null)
			{
				num += this.reverse.Count;
				this.reverse.Clear();
			}
			num += base.Count;
			base.Clear();
			return num;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00052D68 File Offset: 0x00050F68
		public void Add(IEnumerable<SQLiteDbTypeMapping> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (SQLiteDbTypeMapping sqliteDbTypeMapping in collection)
			{
				this.Add(sqliteDbTypeMapping);
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00052DCC File Offset: 0x00050FCC
		public void Add(SQLiteDbTypeMapping item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.typeName == null)
			{
				throw new ArgumentException("item type name cannot be null");
			}
			base.Add(item.typeName, item);
			if (item.primary)
			{
				this.reverse.Add(item.dataType, item);
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00052E30 File Offset: 0x00051030
		public bool ContainsKey(DbType key)
		{
			return this.reverse != null && this.reverse.ContainsKey(key);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00052E4C File Offset: 0x0005104C
		public bool TryGetValue(DbType key, out SQLiteDbTypeMapping value)
		{
			if (this.reverse == null)
			{
				value = null;
				return false;
			}
			return this.reverse.TryGetValue(key, out value);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00052E6C File Offset: 0x0005106C
		public bool Remove(DbType key)
		{
			return this.reverse != null && this.reverse.Remove(key);
		}

		// Token: 0x04000727 RID: 1831
		private Dictionary<DbType, SQLiteDbTypeMapping> reverse;
	}
}
