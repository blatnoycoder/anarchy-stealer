using System;
using System.Collections.Generic;

namespace System.Data.SQLite
{
	// Token: 0x02000183 RID: 387
	internal static class SQLiteConnectionPool
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x00050F20 File Offset: 0x0004F120
		public static void GetCounts(string fileName, ref Dictionary<string, int> counts, ref int openCount, ref int closeCount, ref int totalCount)
		{
			ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
			if (connectionPool == null)
			{
				return;
			}
			connectionPool.GetCounts(fileName, ref counts, ref openCount, ref closeCount, ref totalCount);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00050F4C File Offset: 0x0004F14C
		public static void ClearPool(string fileName)
		{
			ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
			if (connectionPool == null)
			{
				return;
			}
			connectionPool.ClearPool(fileName);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00050F74 File Offset: 0x0004F174
		public static void ClearAllPools()
		{
			ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
			if (connectionPool == null)
			{
				return;
			}
			connectionPool.ClearAllPools();
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00050F98 File Offset: 0x0004F198
		public static void Add(string fileName, SQLiteConnectionHandle handle, int version)
		{
			ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
			if (connectionPool == null)
			{
				return;
			}
			connectionPool.Add(fileName, handle, version);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00050FC0 File Offset: 0x0004F1C0
		public static SQLiteConnectionHandle Remove(string fileName, int maxPoolSize, out int version)
		{
			ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
			if (connectionPool == null)
			{
				version = 0;
				return null;
			}
			return connectionPool.Remove(fileName, maxPoolSize, out version) as SQLiteConnectionHandle;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00050FF0 File Offset: 0x0004F1F0
		public static void Initialize(object argument)
		{
			ISQLiteConnectionPool2 isqliteConnectionPool = SQLiteConnectionPool.GetConnectionPool() as ISQLiteConnectionPool2;
			if (isqliteConnectionPool == null)
			{
				return;
			}
			isqliteConnectionPool.Initialize(argument);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0005101C File Offset: 0x0004F21C
		public static void Terminate(object argument)
		{
			ISQLiteConnectionPool2 isqliteConnectionPool = SQLiteConnectionPool.GetConnectionPool() as ISQLiteConnectionPool2;
			if (isqliteConnectionPool == null)
			{
				return;
			}
			isqliteConnectionPool.Terminate(argument);
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00051048 File Offset: 0x0004F248
		public static void GetCounts(ref int openCount, ref int closeCount)
		{
			ISQLiteConnectionPool2 isqliteConnectionPool = SQLiteConnectionPool.GetConnectionPool() as ISQLiteConnectionPool2;
			if (isqliteConnectionPool == null)
			{
				return;
			}
			isqliteConnectionPool.GetCounts(ref openCount, ref closeCount);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00051074 File Offset: 0x0004F274
		public static void ResetCounts()
		{
			ISQLiteConnectionPool2 isqliteConnectionPool = SQLiteConnectionPool.GetConnectionPool() as ISQLiteConnectionPool2;
			if (isqliteConnectionPool == null)
			{
				return;
			}
			isqliteConnectionPool.ResetCounts();
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000510A0 File Offset: 0x0004F2A0
		public static void CreateAndInitialize(object argument, bool strong, bool force)
		{
			lock (SQLiteConnectionPool._syncRoot)
			{
				if (force || SQLiteConnectionPool._connectionPool == null)
				{
					ISQLiteConnectionPool isqliteConnectionPool;
					if (strong)
					{
						isqliteConnectionPool = new StrongConnectionPool();
					}
					else
					{
						isqliteConnectionPool = new WeakConnectionPool();
					}
					ISQLiteConnectionPool2 isqliteConnectionPool2 = isqliteConnectionPool as ISQLiteConnectionPool2;
					if (isqliteConnectionPool2 != null)
					{
						isqliteConnectionPool2.Initialize(argument);
					}
					SQLiteConnectionPool._connectionPool = isqliteConnectionPool;
				}
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00051120 File Offset: 0x0004F320
		public static void TerminateAndReset(object argument)
		{
			lock (SQLiteConnectionPool._syncRoot)
			{
				if (SQLiteConnectionPool._connectionPool != null)
				{
					ISQLiteConnectionPool2 isqliteConnectionPool = SQLiteConnectionPool._connectionPool as ISQLiteConnectionPool2;
					if (isqliteConnectionPool != null)
					{
						isqliteConnectionPool.Terminate(argument);
					}
					SQLiteConnectionPool._connectionPool = null;
				}
			}
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00051188 File Offset: 0x0004F388
		public static ISQLiteConnectionPool GetConnectionPool()
		{
			ISQLiteConnectionPool connectionPool;
			lock (SQLiteConnectionPool._syncRoot)
			{
				connectionPool = SQLiteConnectionPool._connectionPool;
			}
			return connectionPool;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x000511D0 File Offset: 0x0004F3D0
		public static void SetConnectionPool(ISQLiteConnectionPool connectionPool)
		{
			lock (SQLiteConnectionPool._syncRoot)
			{
				SQLiteConnectionPool._connectionPool = connectionPool;
			}
		}

		// Token: 0x040006B3 RID: 1715
		private static readonly object _syncRoot = new object();

		// Token: 0x040006B4 RID: 1716
		private static ISQLiteConnectionPool _connectionPool = null;
	}
}
