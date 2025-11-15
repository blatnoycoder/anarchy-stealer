using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x02000059 RID: 89
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000AB70 File Offset: 0x00008D70
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000AB78 File Offset: 0x00008D78
		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000AB84 File Offset: 0x00008D84
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0000AB94 File Offset: 0x00008D94
		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling.GetValueOrDefault();
			}
			set
			{
				this._missingMemberHandling = new MissingMemberHandling?(value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000ABA4 File Offset: 0x00008DA4
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000ABB4 File Offset: 0x00008DB4
		public NullValueHandling ItemNullValueHandling
		{
			get
			{
				return this._itemNullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._itemNullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		// (set) Token: 0x060001CA RID: 458 RVA: 0x0000ABD4 File Offset: 0x00008DD4
		public Required ItemRequired
		{
			get
			{
				return this._itemRequired.GetValueOrDefault();
			}
			set
			{
				this._itemRequired = new Required?(value);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public JsonObjectAttribute()
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000ABEC File Offset: 0x00008DEC
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000ABFC File Offset: 0x00008DFC
		[NullableContext(1)]
		public JsonObjectAttribute(string id)
			: base(id)
		{
		}

		// Token: 0x0400010F RID: 271
		private MemberSerialization _memberSerialization;

		// Token: 0x04000110 RID: 272
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x04000111 RID: 273
		internal Required? _itemRequired;

		// Token: 0x04000112 RID: 274
		internal NullValueHandling? _itemNullValueHandling;
	}
}
