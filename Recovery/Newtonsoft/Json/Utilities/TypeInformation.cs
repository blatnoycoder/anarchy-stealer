using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	[Nullable(0)]
	internal class TypeInformation
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000162AC File Offset: 0x000144AC
		public Type Type { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000162B4 File Offset: 0x000144B4
		public PrimitiveTypeCode TypeCode { get; }

		// Token: 0x0600046B RID: 1131 RVA: 0x000162BC File Offset: 0x000144BC
		public TypeInformation(Type type, PrimitiveTypeCode typeCode)
		{
			this.Type = type;
			this.TypeCode = typeCode;
		}
	}
}
