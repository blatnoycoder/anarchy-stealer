using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x0200004C RID: 76
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00009D28 File Offset: 0x00007F28
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00009D30 File Offset: 0x00007F30
		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009D3C File Offset: 0x00007F3C
		public JsonArrayAttribute()
		{
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009D44 File Offset: 0x00007F44
		public JsonArrayAttribute(bool allowNullItems)
		{
			this._allowNullItems = allowNullItems;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009D54 File Offset: 0x00007F54
		[NullableContext(1)]
		public JsonArrayAttribute(string id)
			: base(id)
		{
		}

		// Token: 0x040000F6 RID: 246
		private bool _allowNullItems;
	}
}
