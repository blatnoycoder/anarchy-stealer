using System;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001E3 RID: 483
	public interface ISQLiteSession : IDisposable
	{
		// Token: 0x060015D2 RID: 5586
		bool IsEnabled();

		// Token: 0x060015D3 RID: 5587
		void SetToEnabled();

		// Token: 0x060015D4 RID: 5588
		void SetToDisabled();

		// Token: 0x060015D5 RID: 5589
		bool IsIndirect();

		// Token: 0x060015D6 RID: 5590
		void SetToIndirect();

		// Token: 0x060015D7 RID: 5591
		void SetToDirect();

		// Token: 0x060015D8 RID: 5592
		bool IsEmpty();

		// Token: 0x060015D9 RID: 5593
		long GetMemoryBytesInUse();

		// Token: 0x060015DA RID: 5594
		void AttachTable(string name);

		// Token: 0x060015DB RID: 5595
		void SetTableFilter(SessionTableFilterCallback callback, object clientData);

		// Token: 0x060015DC RID: 5596
		void CreateChangeSet(ref byte[] rawData);

		// Token: 0x060015DD RID: 5597
		void CreateChangeSet(Stream stream);

		// Token: 0x060015DE RID: 5598
		void CreatePatchSet(ref byte[] rawData);

		// Token: 0x060015DF RID: 5599
		void CreatePatchSet(Stream stream);

		// Token: 0x060015E0 RID: 5600
		void LoadDifferencesFromTable(string fromDatabaseName, string tableName);
	}
}
