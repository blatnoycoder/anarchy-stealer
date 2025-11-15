using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x02000184 RID: 388
	internal static class StaticWeakConnectionPool<T> where T : WeakReference
	{
		// Token: 0x06001127 RID: 4391 RVA: 0x0005122C File Offset: 0x0004F42C
		public static void GetCounts(string fileName, ref Dictionary<string, int> counts, ref int openCount, ref int closeCount, ref int totalCount)
		{
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				openCount = StaticWeakConnectionPool<T>._poolOpened;
				closeCount = StaticWeakConnectionPool<T>._poolClosed;
				if (counts == null)
				{
					counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
				}
				if (fileName != null)
				{
					PoolQueue<T> poolQueue;
					if (StaticWeakConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue))
					{
						Queue<T> queue = poolQueue.Queue;
						int num = ((queue != null) ? queue.Count : 0);
						counts.Add(fileName, num);
						totalCount += num;
					}
				}
				else
				{
					foreach (KeyValuePair<string, PoolQueue<T>> keyValuePair in StaticWeakConnectionPool<T>._queueList)
					{
						if (keyValuePair.Value != null)
						{
							Queue<T> queue2 = keyValuePair.Value.Queue;
							int num2 = ((queue2 != null) ? queue2.Count : 0);
							counts.Add(keyValuePair.Key, num2);
							totalCount += num2;
						}
					}
				}
			}
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00051364 File Offset: 0x0004F564
		public static void ClearPool(string fileName)
		{
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				PoolQueue<T> poolQueue;
				if (StaticWeakConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue))
				{
					poolQueue.PoolVersion++;
					Queue<T> queue = poolQueue.Queue;
					if (queue != null)
					{
						while (queue.Count > 0)
						{
							T t = queue.Dequeue();
							if (t != null)
							{
								SQLiteConnectionHandle sqliteConnectionHandle = t.Target as SQLiteConnectionHandle;
								if (sqliteConnectionHandle != null)
								{
									sqliteConnectionHandle.Dispose();
								}
								GC.KeepAlive(sqliteConnectionHandle);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0005141C File Offset: 0x0004F61C
		public static void ClearAllPools()
		{
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				foreach (KeyValuePair<string, PoolQueue<T>> keyValuePair in StaticWeakConnectionPool<T>._queueList)
				{
					if (keyValuePair.Value != null)
					{
						Queue<T> queue = keyValuePair.Value.Queue;
						while (queue.Count > 0)
						{
							T t = queue.Dequeue();
							if (t != null)
							{
								SQLiteConnectionHandle sqliteConnectionHandle = t.Target as SQLiteConnectionHandle;
								if (sqliteConnectionHandle != null)
								{
									sqliteConnectionHandle.Dispose();
								}
								GC.KeepAlive(sqliteConnectionHandle);
							}
						}
						if (StaticWeakConnectionPool<T>._poolVersion <= keyValuePair.Value.PoolVersion)
						{
							StaticWeakConnectionPool<T>._poolVersion = keyValuePair.Value.PoolVersion + 1;
						}
					}
				}
				StaticWeakConnectionPool<T>._queueList.Clear();
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00051538 File Offset: 0x0004F738
		public static void Add(string fileName, SQLiteConnectionHandle handle, int version)
		{
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				PoolQueue<T> poolQueue;
				if (StaticWeakConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue) && version == poolQueue.PoolVersion)
				{
					StaticWeakConnectionPool<T>.ResizePool(poolQueue, true);
					Queue<T> queue = poolQueue.Queue;
					if (queue == null)
					{
						return;
					}
					queue.Enqueue((T)((object)new WeakReference(handle, false)));
					Interlocked.Increment(ref StaticWeakConnectionPool<T>._poolClosed);
				}
				else
				{
					handle.Close();
				}
				GC.KeepAlive(handle);
			}
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000515DC File Offset: 0x0004F7DC
		public static SQLiteConnectionHandle Remove(string fileName, int maxPoolSize, out int version)
		{
			int num;
			Queue<T> queue;
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				version = StaticWeakConnectionPool<T>._poolVersion;
				PoolQueue<T> poolQueue;
				if (!StaticWeakConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue))
				{
					poolQueue = new PoolQueue<T>(StaticWeakConnectionPool<T>._poolVersion, maxPoolSize);
					StaticWeakConnectionPool<T>._queueList.Add(fileName, poolQueue);
					return null;
				}
				num = (version = poolQueue.PoolVersion);
				poolQueue.MaxPoolSize = maxPoolSize;
				StaticWeakConnectionPool<T>.ResizePool(poolQueue, false);
				queue = poolQueue.Queue;
				if (queue == null)
				{
					return null;
				}
				StaticWeakConnectionPool<T>._queueList.Remove(fileName);
				queue = new Queue<T>(queue);
			}
			try
			{
				while (queue.Count > 0)
				{
					T t = queue.Dequeue();
					if (t != null)
					{
						SQLiteConnectionHandle sqliteConnectionHandle = t.Target as SQLiteConnectionHandle;
						if (sqliteConnectionHandle != null)
						{
							GC.SuppressFinalize(sqliteConnectionHandle);
							try
							{
								GC.WaitForPendingFinalizers();
								if (!sqliteConnectionHandle.IsInvalid && !sqliteConnectionHandle.IsClosed)
								{
									Interlocked.Increment(ref StaticWeakConnectionPool<T>._poolOpened);
									return sqliteConnectionHandle;
								}
							}
							finally
							{
								GC.ReRegisterForFinalize(sqliteConnectionHandle);
							}
							GC.KeepAlive(sqliteConnectionHandle);
						}
					}
				}
			}
			finally
			{
				lock (StaticWeakConnectionPool<T>._syncRoot)
				{
					PoolQueue<T> poolQueue2;
					bool flag3;
					if (StaticWeakConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue2))
					{
						flag3 = false;
					}
					else
					{
						flag3 = true;
						poolQueue2 = new PoolQueue<T>(num, maxPoolSize);
					}
					Queue<T> queue2 = poolQueue2.Queue;
					while (queue.Count > 0)
					{
						queue2.Enqueue(queue.Dequeue());
					}
					StaticWeakConnectionPool<T>.ResizePool(poolQueue2, false);
					if (flag3)
					{
						StaticWeakConnectionPool<T>._queueList.Add(fileName, poolQueue2);
					}
				}
			}
			return null;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000517E0 File Offset: 0x0004F9E0
		public static void ResetCounts()
		{
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				StaticWeakConnectionPool<T>._poolOpened = 0;
				StaticWeakConnectionPool<T>._poolClosed = 0;
			}
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0005182C File Offset: 0x0004FA2C
		public static void GetCounts(ref int openCount, ref int closeCount)
		{
			lock (StaticWeakConnectionPool<T>._syncRoot)
			{
				openCount = StaticWeakConnectionPool<T>._poolOpened;
				closeCount = StaticWeakConnectionPool<T>._poolClosed;
			}
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0005187C File Offset: 0x0004FA7C
		private static void ResizePool(PoolQueue<T> queue, bool add)
		{
			int num = queue.MaxPoolSize;
			if (add && num > 0)
			{
				num--;
			}
			Queue<T> queue2 = queue.Queue;
			if (queue2 == null)
			{
				return;
			}
			while (queue2.Count > num)
			{
				T t = queue2.Dequeue();
				if (t != null)
				{
					SQLiteConnectionHandle sqliteConnectionHandle = t.Target as SQLiteConnectionHandle;
					if (sqliteConnectionHandle != null)
					{
						sqliteConnectionHandle.Dispose();
					}
					GC.KeepAlive(sqliteConnectionHandle);
				}
			}
		}

		// Token: 0x040006B5 RID: 1717
		private static readonly object _syncRoot = new object();

		// Token: 0x040006B6 RID: 1718
		private static SortedList<string, PoolQueue<T>> _queueList = new SortedList<string, PoolQueue<T>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040006B7 RID: 1719
		private static int _poolVersion = 1;

		// Token: 0x040006B8 RID: 1720
		private static int _poolOpened = 0;

		// Token: 0x040006B9 RID: 1721
		private static int _poolClosed = 0;
	}
}
