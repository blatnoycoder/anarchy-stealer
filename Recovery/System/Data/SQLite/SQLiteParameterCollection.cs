using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001B0 RID: 432
	[ListBindable(false)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class SQLiteParameterCollection : DbParameterCollection
	{
		// Token: 0x060012DA RID: 4826 RVA: 0x00059FF8 File Offset: 0x000581F8
		internal SQLiteParameterCollection(SQLiteCommand cmd)
		{
			this._command = cmd;
			this._parameterList = new List<SQLiteParameter>();
			this._unboundFlag = true;
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x0005A01C File Offset: 0x0005821C
		public override bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0005A020 File Offset: 0x00058220
		public override bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0005A024 File Offset: 0x00058224
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0005A028 File Offset: 0x00058228
		public override object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0005A02C File Offset: 0x0005822C
		public override IEnumerator GetEnumerator()
		{
			return this._parameterList.GetEnumerator();
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0005A040 File Offset: 0x00058240
		public SQLiteParameter Add(string parameterName, DbType parameterType, int parameterSize, string sourceColumn)
		{
			SQLiteParameter sqliteParameter = new SQLiteParameter(parameterName, parameterType, parameterSize, sourceColumn);
			this.Add(sqliteParameter);
			return sqliteParameter;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0005A068 File Offset: 0x00058268
		public SQLiteParameter Add(string parameterName, DbType parameterType, int parameterSize)
		{
			SQLiteParameter sqliteParameter = new SQLiteParameter(parameterName, parameterType, parameterSize);
			this.Add(sqliteParameter);
			return sqliteParameter;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0005A08C File Offset: 0x0005828C
		public SQLiteParameter Add(string parameterName, DbType parameterType)
		{
			SQLiteParameter sqliteParameter = new SQLiteParameter(parameterName, parameterType);
			this.Add(sqliteParameter);
			return sqliteParameter;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0005A0B0 File Offset: 0x000582B0
		public int Add(SQLiteParameter parameter)
		{
			int num = -1;
			if (!string.IsNullOrEmpty(parameter.ParameterName))
			{
				num = this.IndexOf(parameter.ParameterName);
			}
			if (num == -1)
			{
				num = this._parameterList.Count;
				this._parameterList.Add(parameter);
			}
			this.SetParameter(num, parameter);
			return num;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0005A108 File Offset: 0x00058308
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int Add(object value)
		{
			return this.Add((SQLiteParameter)value);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0005A118 File Offset: 0x00058318
		public SQLiteParameter AddWithValue(string parameterName, object value)
		{
			SQLiteParameter sqliteParameter = new SQLiteParameter(parameterName, value);
			this.Add(sqliteParameter);
			return sqliteParameter;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0005A13C File Offset: 0x0005833C
		public void AddRange(SQLiteParameter[] values)
		{
			int num = values.Length;
			for (int i = 0; i < num; i++)
			{
				this.Add(values[i]);
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0005A170 File Offset: 0x00058370
		public override void AddRange(Array values)
		{
			int length = values.Length;
			for (int i = 0; i < length; i++)
			{
				this.Add((SQLiteParameter)values.GetValue(i));
			}
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0005A1AC File Offset: 0x000583AC
		public override void Clear()
		{
			this._unboundFlag = true;
			this._parameterList.Clear();
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0005A1C0 File Offset: 0x000583C0
		public override bool Contains(string parameterName)
		{
			return this.IndexOf(parameterName) != -1;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0005A1D0 File Offset: 0x000583D0
		public override bool Contains(object value)
		{
			return this._parameterList.Contains((SQLiteParameter)value);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0005A1E4 File Offset: 0x000583E4
		public override void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x0005A1EC File Offset: 0x000583EC
		public override int Count
		{
			get
			{
				return this._parameterList.Count;
			}
		}

		// Token: 0x17000364 RID: 868
		public SQLiteParameter this[string parameterName]
		{
			get
			{
				return (SQLiteParameter)this.GetParameter(parameterName);
			}
			set
			{
				this.SetParameter(parameterName, value);
			}
		}

		// Token: 0x17000365 RID: 869
		public SQLiteParameter this[int index]
		{
			get
			{
				return (SQLiteParameter)this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, value);
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0005A234 File Offset: 0x00058434
		protected override DbParameter GetParameter(string parameterName)
		{
			return this.GetParameter(this.IndexOf(parameterName));
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0005A244 File Offset: 0x00058444
		protected override DbParameter GetParameter(int index)
		{
			return this._parameterList[index];
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0005A254 File Offset: 0x00058454
		public override int IndexOf(string parameterName)
		{
			int count = this._parameterList.Count;
			for (int i = 0; i < count; i++)
			{
				if (string.Compare(parameterName, this._parameterList[i].ParameterName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0005A2A0 File Offset: 0x000584A0
		public override int IndexOf(object value)
		{
			return this._parameterList.IndexOf((SQLiteParameter)value);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0005A2B4 File Offset: 0x000584B4
		public override void Insert(int index, object value)
		{
			this._unboundFlag = true;
			this._parameterList.Insert(index, (SQLiteParameter)value);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0005A2D0 File Offset: 0x000584D0
		public override void Remove(object value)
		{
			this._unboundFlag = true;
			this._parameterList.Remove((SQLiteParameter)value);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0005A2EC File Offset: 0x000584EC
		public override void RemoveAt(string parameterName)
		{
			this.RemoveAt(this.IndexOf(parameterName));
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0005A2FC File Offset: 0x000584FC
		public override void RemoveAt(int index)
		{
			this._unboundFlag = true;
			this._parameterList.RemoveAt(index);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0005A314 File Offset: 0x00058514
		protected override void SetParameter(string parameterName, DbParameter value)
		{
			this.SetParameter(this.IndexOf(parameterName), value);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0005A324 File Offset: 0x00058524
		protected override void SetParameter(int index, DbParameter value)
		{
			this._unboundFlag = true;
			this._parameterList[index] = (SQLiteParameter)value;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0005A340 File Offset: 0x00058540
		internal void Unbind()
		{
			this._unboundFlag = true;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0005A34C File Offset: 0x0005854C
		internal void MapParameters(SQLiteStatement activeStatement)
		{
			if (!this._unboundFlag || this._parameterList.Count == 0 || this._command._statementList == null)
			{
				return;
			}
			int num = 0;
			int num2 = -1;
			foreach (SQLiteParameter sqliteParameter in this._parameterList)
			{
				num2++;
				string text = sqliteParameter.ParameterName;
				if (text == null)
				{
					text = HelperMethods.StringFormat(CultureInfo.InvariantCulture, ";{0}", new object[] { num });
					num++;
				}
				bool flag = false;
				int num3;
				if (activeStatement == null)
				{
					num3 = this._command._statementList.Count;
				}
				else
				{
					num3 = 1;
				}
				SQLiteStatement sqliteStatement = activeStatement;
				for (int i = 0; i < num3; i++)
				{
					flag = false;
					if (sqliteStatement == null)
					{
						sqliteStatement = this._command._statementList[i];
					}
					if (sqliteStatement._paramNames != null && sqliteStatement.MapParameter(text, sqliteParameter))
					{
						flag = true;
					}
					sqliteStatement = null;
				}
				if (!flag)
				{
					text = HelperMethods.StringFormat(CultureInfo.InvariantCulture, ";{0}", new object[] { num2 });
					sqliteStatement = activeStatement;
					for (int i = 0; i < num3; i++)
					{
						if (sqliteStatement == null)
						{
							sqliteStatement = this._command._statementList[i];
						}
						if (sqliteStatement._paramNames == null || sqliteStatement.MapParameter(text, sqliteParameter))
						{
						}
						sqliteStatement = null;
					}
				}
			}
			if (activeStatement == null)
			{
				this._unboundFlag = false;
			}
		}

		// Token: 0x04000807 RID: 2055
		private SQLiteCommand _command;

		// Token: 0x04000808 RID: 2056
		private List<SQLiteParameter> _parameterList;

		// Token: 0x04000809 RID: 2057
		private bool _unboundFlag;
	}
}
