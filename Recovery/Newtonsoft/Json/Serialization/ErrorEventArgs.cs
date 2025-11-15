using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00021F6C File Offset: 0x0002016C
		[Nullable(2)]
		public object CurrentObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00021F74 File Offset: 0x00020174
		public ErrorContext ErrorContext { get; }

		// Token: 0x06000672 RID: 1650 RVA: 0x00021F7C File Offset: 0x0002017C
		public ErrorEventArgs([Nullable(2)] object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
