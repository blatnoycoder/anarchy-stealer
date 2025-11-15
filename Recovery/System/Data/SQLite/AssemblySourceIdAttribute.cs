using System;

namespace System.Data.SQLite
{
	// Token: 0x02000148 RID: 328
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblySourceIdAttribute : Attribute
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x0003FAB8 File Offset: 0x0003DCB8
		public AssemblySourceIdAttribute(string value)
		{
			this.sourceId = value;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0003FAC8 File Offset: 0x0003DCC8
		public string SourceId
		{
			get
			{
				return this.sourceId;
			}
		}

		// Token: 0x04000513 RID: 1299
		private string sourceId;
	}
}
