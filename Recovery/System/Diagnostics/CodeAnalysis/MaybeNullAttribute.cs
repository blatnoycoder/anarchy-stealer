using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x0200003E RID: 62
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	internal sealed class MaybeNullAttribute : Attribute
	{
	}
}
