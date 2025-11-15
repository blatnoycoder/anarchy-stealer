using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000AA7C File Offset: 0x00008C7C
		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000AA84 File Offset: 0x00008C84
		[Nullable(new byte[] { 2, 1 })]
		public object[] ConverterParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000AA8C File Offset: 0x00008C8C
		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		public JsonConverterAttribute(Type converterType, params object[] converterParameters)
			: this(converterType)
		{
			this.ConverterParameters = converterParameters;
		}

		// Token: 0x0400010B RID: 267
		private readonly Type _converterType;
	}
}
