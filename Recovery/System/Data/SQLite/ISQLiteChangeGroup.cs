using System;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001E1 RID: 481
	public interface ISQLiteChangeGroup : IDisposable
	{
		// Token: 0x060015C5 RID: 5573
		void AddChangeSet(byte[] rawData);

		// Token: 0x060015C6 RID: 5574
		void AddChangeSet(Stream stream);

		// Token: 0x060015C7 RID: 5575
		void CreateChangeSet(ref byte[] rawData);

		// Token: 0x060015C8 RID: 5576
		void CreateChangeSet(Stream stream);
	}
}
