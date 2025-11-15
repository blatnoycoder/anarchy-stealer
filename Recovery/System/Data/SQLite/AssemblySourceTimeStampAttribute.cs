using System;

namespace System.Data.SQLite
{
	// Token: 0x02000149 RID: 329
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblySourceTimeStampAttribute : Attribute
	{
		// Token: 0x06000DB6 RID: 3510 RVA: 0x0003FAD0 File Offset: 0x0003DCD0
		public AssemblySourceTimeStampAttribute(string value)
		{
			this.sourceTimeStamp = value;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0003FAE0 File Offset: 0x0003DCE0
		public string SourceTimeStamp
		{
			get
			{
				return this.sourceTimeStamp;
			}
		}

		// Token: 0x04000514 RID: 1300
		private string sourceTimeStamp;
	}
}
