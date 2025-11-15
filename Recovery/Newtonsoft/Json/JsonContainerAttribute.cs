using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200004E RID: 78
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00009D68 File Offset: 0x00007F68
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00009D70 File Offset: 0x00007F70
		public string Id { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00009D7C File Offset: 0x00007F7C
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00009D84 File Offset: 0x00007F84
		public string Title { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00009D90 File Offset: 0x00007F90
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00009D98 File Offset: 0x00007F98
		public string Description { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00009DA4 File Offset: 0x00007FA4
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00009DAC File Offset: 0x00007FAC
		public Type ItemConverterType { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00009DB8 File Offset: 0x00007FB8
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00009DC0 File Offset: 0x00007FC0
		[Nullable(new byte[] { 2, 1 })]
		public object[] ItemConverterParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00009DCC File Offset: 0x00007FCC
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00009DD4 File Offset: 0x00007FD4
		public Type NamingStrategyType
		{
			get
			{
				return this._namingStrategyType;
			}
			set
			{
				this._namingStrategyType = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00009DE4 File Offset: 0x00007FE4
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00009DEC File Offset: 0x00007FEC
		[Nullable(new byte[] { 2, 1 })]
		public object[] NamingStrategyParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._namingStrategyParameters;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._namingStrategyParameters = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00009DFC File Offset: 0x00007FFC
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00009E04 File Offset: 0x00008004
		internal NamingStrategy NamingStrategyInstance { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00009E10 File Offset: 0x00008010
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00009E20 File Offset: 0x00008020
		public bool IsReference
		{
			get
			{
				return this._isReference.GetValueOrDefault();
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00009E30 File Offset: 0x00008030
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00009E40 File Offset: 0x00008040
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference.GetValueOrDefault();
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00009E50 File Offset: 0x00008050
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00009E60 File Offset: 0x00008060
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				return this._itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00009E70 File Offset: 0x00008070
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00009E80 File Offset: 0x00008080
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				return this._itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009E90 File Offset: 0x00008090
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00009E98 File Offset: 0x00008098
		[NullableContext(1)]
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x040000FD RID: 253
		internal bool? _isReference;

		// Token: 0x040000FE RID: 254
		internal bool? _itemIsReference;

		// Token: 0x040000FF RID: 255
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x04000100 RID: 256
		internal TypeNameHandling? _itemTypeNameHandling;

		// Token: 0x04000101 RID: 257
		private Type _namingStrategyType;

		// Token: 0x04000102 RID: 258
		[Nullable(new byte[] { 2, 1 })]
		private object[] _namingStrategyParameters;
	}
}
