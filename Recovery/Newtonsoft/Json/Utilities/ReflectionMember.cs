using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200009B RID: 155
	[NullableContext(2)]
	[Nullable(0)]
	internal class ReflectionMember
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001D444 File Offset: 0x0001B644
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x0001D44C File Offset: 0x0001B64C
		public Type MemberType { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001D458 File Offset: 0x0001B658
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0001D460 File Offset: 0x0001B660
		[Nullable(new byte[] { 2, 1, 2 })]
		public Func<object, object> Getter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001D46C File Offset: 0x0001B66C
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x0001D474 File Offset: 0x0001B674
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> Setter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}
	}
}
