using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x020001E0 RID: 480
	public interface ISQLiteChangeSet : IEnumerable<ISQLiteChangeSetMetadataItem>, IEnumerable, IDisposable
	{
		// Token: 0x060015C1 RID: 5569
		ISQLiteChangeSet Invert();

		// Token: 0x060015C2 RID: 5570
		ISQLiteChangeSet CombineWith(ISQLiteChangeSet changeSet);

		// Token: 0x060015C3 RID: 5571
		void Apply(SessionConflictCallback conflictCallback, object clientData);

		// Token: 0x060015C4 RID: 5572
		void Apply(SessionConflictCallback conflictCallback, SessionTableFilterCallback tableFilterCallback, object clientData);
	}
}
