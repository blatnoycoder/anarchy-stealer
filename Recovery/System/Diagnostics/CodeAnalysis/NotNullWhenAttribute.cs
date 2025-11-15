using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x0200003D RID: 61
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	internal sealed class NotNullWhenAttribute : Attribute
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00009A00 File Offset: 0x00007C00
		public NotNullWhenAttribute(bool returnValue)
		{
			this.ReturnValue = returnValue;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00009A10 File Offset: 0x00007C10
		public bool ReturnValue { get; }
	}
}
