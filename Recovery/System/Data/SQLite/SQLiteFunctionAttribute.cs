using System;

namespace System.Data.SQLite
{
	// Token: 0x020001A9 RID: 425
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class SQLiteFunctionAttribute : Attribute
	{
		// Token: 0x0600125D RID: 4701 RVA: 0x00057AA8 File Offset: 0x00055CA8
		public SQLiteFunctionAttribute()
			: this(null, -1, FunctionType.Scalar)
		{
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00057AB4 File Offset: 0x00055CB4
		public SQLiteFunctionAttribute(string name, int argumentCount, FunctionType functionType)
		{
			this._name = name;
			this._argumentCount = argumentCount;
			this._functionType = functionType;
			this._instanceType = null;
			this._callback1 = null;
			this._callback2 = null;
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x00057AE8 File Offset: 0x00055CE8
		// (set) Token: 0x06001260 RID: 4704 RVA: 0x00057AF0 File Offset: 0x00055CF0
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00057AFC File Offset: 0x00055CFC
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x00057B04 File Offset: 0x00055D04
		public int Arguments
		{
			get
			{
				return this._argumentCount;
			}
			set
			{
				this._argumentCount = value;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x00057B10 File Offset: 0x00055D10
		// (set) Token: 0x06001264 RID: 4708 RVA: 0x00057B18 File Offset: 0x00055D18
		public FunctionType FuncType
		{
			get
			{
				return this._functionType;
			}
			set
			{
				this._functionType = value;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00057B24 File Offset: 0x00055D24
		// (set) Token: 0x06001266 RID: 4710 RVA: 0x00057B2C File Offset: 0x00055D2C
		internal Type InstanceType
		{
			get
			{
				return this._instanceType;
			}
			set
			{
				this._instanceType = value;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x00057B38 File Offset: 0x00055D38
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x00057B40 File Offset: 0x00055D40
		internal Delegate Callback1
		{
			get
			{
				return this._callback1;
			}
			set
			{
				this._callback1 = value;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x00057B4C File Offset: 0x00055D4C
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x00057B54 File Offset: 0x00055D54
		internal Delegate Callback2
		{
			get
			{
				return this._callback2;
			}
			set
			{
				this._callback2 = value;
			}
		}

		// Token: 0x040007D8 RID: 2008
		private string _name;

		// Token: 0x040007D9 RID: 2009
		private int _argumentCount;

		// Token: 0x040007DA RID: 2010
		private FunctionType _functionType;

		// Token: 0x040007DB RID: 2011
		private Type _instanceType;

		// Token: 0x040007DC RID: 2012
		private Delegate _callback1;

		// Token: 0x040007DD RID: 2013
		private Delegate _callback2;
	}
}
