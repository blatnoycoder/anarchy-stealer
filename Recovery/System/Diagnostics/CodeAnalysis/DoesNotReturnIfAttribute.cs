using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000040 RID: 64
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal class DoesNotReturnIfAttribute : Attribute
	{
		// Token: 0x0600012D RID: 301 RVA: 0x00009A28 File Offset: 0x00007C28
		public DoesNotReturnIfAttribute(bool parameterValue)
		{
			this.ParameterValue = parameterValue;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00009A38 File Offset: 0x00007C38
		public bool ParameterValue { get; }
	}
}
