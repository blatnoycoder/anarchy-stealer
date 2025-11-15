using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x02000185 RID: 389
	internal static class StaticStrongConnectionPool<T> where T : class
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x00051924 File Offset: 0x0004FB24
		public static void GetCounts(string fileName, ref Dictionary<string, int> counts, ref int openCount, ref int closeCount, ref int totalCount)
		{
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				openCount = StaticStrongConnectionPool<T>._poolOpened;
				closeCount = StaticStrongConnectionPool<T>._poolClosed;
				if (counts == null)
				{
					counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
				}
				if (fileName != null)
				{
					PoolQueue<T> poolQueue;
					if (StaticStrongConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue))
					{
						Queue<T> queue = poolQueue.Queue;
						int num = ((queue != null) ? queue.Count : 0);
						counts.Add(fileName, num);
						totalCount += num;
					}
				}
				else
				{
					foreach (KeyValuePair<string, PoolQueue<T>> keyValuePair in StaticStrongConnectionPool<T>._queueList)
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

		// Token: 0x06001131 RID: 4401 RVA: 0x00051A5C File Offset: 0x0004FC5C
		public static void ClearPool(string fileName)
		{
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				PoolQueue<T> poolQueue;
				if (StaticStrongConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue))
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
								SQLiteConnectionHandle sqliteConnectionHandle = t as SQLiteConnectionHandle;
								if (sqliteConnectionHandle != null)
								{
									sqliteConnectionHandle.Dispose();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00051B08 File Offset: 0x0004FD08
		public static void ClearAllPools()
		{
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				foreach (KeyValuePair<string, PoolQueue<T>> keyValuePair in StaticStrongConnectionPool<T>._queueList)
				{
					if (keyValuePair.Value != null)
					{
						Queue<T> queue = keyValuePair.Value.Queue;
						while (queue.Count > 0)
						{
							object obj = queue.Dequeue();
							if (obj != null)
							{
								SQLiteConnectionHandle sqliteConnectionHandle = obj as SQLiteConnectionHandle;
								if (sqliteConnectionHandle != null)
								{
									sqliteConnectionHandle.Dispose();
								}
							}
						}
						if (StaticStrongConnectionPool<T>._poolVersion <= keyValuePair.Value.PoolVersion)
						{
							StaticStrongConnectionPool<T>._poolVersion = keyValuePair.Value.PoolVersion + 1;
						}
					}
				}
				StaticStrongConnectionPool<T>._queueList.Clear();
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00051C10 File Offset: 0x0004FE10
		public static void Add(string fileName, SQLiteConnectionHandle handle, int version)
		{
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				PoolQueue<T> poolQueue;
				if (StaticStrongConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue) && version == poolQueue.PoolVersion)
				{
					StaticStrongConnectionPool<T>.ResizePool(poolQueue, true);
					Queue<T> queue = poolQueue.Queue;
					if (queue != null)
					{
						queue.Enqueue(handle as T);
						Interlocked.Increment(ref StaticStrongConnectionPool<T>._poolClosed);
					}
				}
				else
				{
					handle.Close();
				}
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00051CB0 File Offset: 0x0004FEB0
		public static SQLiteConnectionHandle Remove(string fileName, int maxPoolSize, out int version)
		{
			int num;
			Queue<T> queue;
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				version = StaticStrongConnectionPool<T>._poolVersion;
				PoolQueue<T> poolQueue;
				if (!StaticStrongConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue))
				{
					poolQueue = new PoolQueue<T>(StaticStrongConnectionPool<T>._poolVersion, maxPoolSize);
					StaticStrongConnectionPool<T>._queueList.Add(fileName, poolQueue);
					return null;
				}
				num = (version = poolQueue.PoolVersion);
				poolQueue.MaxPoolSize = maxPoolSize;
				StaticStrongConnectionPool<T>.ResizePool(poolQueue, false);
				queue = poolQueue.Queue;
				if (queue == null)
				{
					return null;
				}
				StaticStrongConnectionPool<T>._queueList.Remove(fileName);
				queue = new Queue<T>(queue);
			}
			try
			{
				while (queue.Count > 0)
				{
					object obj = queue.Dequeue();
					if (obj != null)
					{
						SQLiteConnectionHandle sqliteConnectionHandle = obj as SQLiteConnectionHandle;
						if (sqliteConnectionHandle != null && !sqliteConnectionHandle.IsInvalid && !sqliteConnectionHandle.IsClosed)
						{
							Interlocked.Increment(ref StaticStrongConnectionPool<T>._poolOpened);
							return sqliteConnectionHandle;
						}
					}
				}
			}
			finally
			{
				lock (StaticStrongConnectionPool<T>._syncRoot)
				{
					PoolQueue<T> poolQueue2;
					bool flag3;
					if (StaticStrongConnectionPool<T>._queueList.TryGetValue(fileName, out poolQueue2))
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
					StaticStrongConnectionPool<T>.ResizePool(poolQueue2, false);
					if (flag3)
					{
						StaticStrongConnectionPool<T>._queueList.Add(fileName, poolQueue2);
					}
				}
			}
			return null;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00051E7C File Offset: 0x0005007C
		public static void ResetCounts()
		{
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				StaticStrongConnectionPool<T>._poolOpened = 0;
				StaticStrongConnectionPool<T>._poolClosed = 0;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00051EC8 File Offset: 0x000500C8
		public static void GetCounts(ref int openCount, ref int closeCount)
		{
			lock (StaticStrongConnectionPool<T>._syncRoot)
			{
				openCount = StaticStrongConnectionPool<T>._poolOpened;
				closeCount = StaticStrongConnectionPool<T>._poolClosed;
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00051F18 File Offset: 0x00050118
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
				object obj = queue2.Dequeue();
				if (obj != null)
				{
					SQLiteConnectionHandle sqliteConnectionHandle = obj as SQLiteConnectionHandle;
					if (sqliteConnectionHandle != null)
					{
						sqliteConnectionHandle.Dispose();
					}
				}
			}
		}

		// Token: 0x040006BA RID: 1722
		private static readonly object _syncRoot = new object();

		// Token: 0x040006BB RID: 1723
		private static SortedList<string, PoolQueue<T>> _queueList = new SortedList<string, PoolQueue<T>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040006BC RID: 1724
		private static int _poolVersion = 1;

		// Token: 0x040006BD RID: 1725
		private static int _poolOpened = 0;

		// Token: 0x040006BE RID: 1726
		private static int _poolClosed = 0;
	}
}
