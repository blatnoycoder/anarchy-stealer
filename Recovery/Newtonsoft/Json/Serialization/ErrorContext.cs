using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AF RID: 175
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorContext
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x00021EFC File Offset: 0x000200FC
		internal ErrorContext([Nullable(2)] object originalObject, [Nullable(2)] object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00021F24 File Offset: 0x00020124
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00021F2C File Offset: 0x0002012C
		internal bool Traced { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00021F38 File Offset: 0x00020138
		public Exception Error { get; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00021F40 File Offset: 0x00020140
		[Nullable(2)]
		public object OriginalObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00021F48 File Offset: 0x00020148
		[Nullable(2)]
		public object Member
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00021F50 File Offset: 0x00020150
		public string Path { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00021F58 File Offset: 0x00020158
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x00021F60 File Offset: 0x00020160
		public bool Handled { get; set; }
	}
}
