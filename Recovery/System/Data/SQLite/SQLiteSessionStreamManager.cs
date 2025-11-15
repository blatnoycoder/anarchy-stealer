using System;
using System.Collections.Generic;
using System.IO;

namespace System.Data.SQLite
{
	// Token: 0x020001EA RID: 490
	internal sealed class SQLiteSessionStreamManager : IDisposable
	{
		// Token: 0x06001609 RID: 5641 RVA: 0x00063794 File Offset: 0x00061994
		public SQLiteSessionStreamManager(SQLiteConnectionFlags flags)
		{
			this.flags = flags;
			this.InitializeStreamAdapters();
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x000637AC File Offset: 0x000619AC
		private void InitializeStreamAdapters()
		{
			if (this.streamAdapters != null)
			{
				return;
			}
			this.streamAdapters = new Dictionary<Stream, SQLiteStreamAdapter>();
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x000637C8 File Offset: 0x000619C8
		private void DisposeStreamAdapters()
		{
			if (this.streamAdapters == null)
			{
				return;
			}
			foreach (KeyValuePair<Stream, SQLiteStreamAdapter> keyValuePair in this.streamAdapters)
			{
				SQLiteStreamAdapter value = keyValuePair.Value;
				if (value != null)
				{
					value.Dispose();
				}
			}
			this.streamAdapters.Clear();
			this.streamAdapters = null;
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0006384C File Offset: 0x00061A4C
		public SQLiteStreamAdapter GetAdapter(Stream stream)
		{
			this.CheckDisposed();
			if (stream == null)
			{
				return null;
			}
			SQLiteStreamAdapter sqliteStreamAdapter;
			if (this.streamAdapters.TryGetValue(stream, out sqliteStreamAdapter))
			{
				return sqliteStreamAdapter;
			}
			sqliteStreamAdapter = new SQLiteStreamAdapter(stream, this.flags);
			this.streamAdapters.Add(stream, sqliteStreamAdapter);
			return sqliteStreamAdapter;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0006389C File Offset: 0x00061A9C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x000638AC File Offset: 0x00061AAC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteSessionStreamManager).Name);
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x000638D0 File Offset: 0x00061AD0
		private void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing)
				{
					this.DisposeStreamAdapters();
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00063914 File Offset: 0x00061B14
		~SQLiteSessionStreamManager()
		{
			this.Dispose(false);
		}

		// Token: 0x040008EC RID: 2284
		private Dictionary<Stream, SQLiteStreamAdapter> streamAdapters;

		// Token: 0x040008ED RID: 2285
		private SQLiteConnectionFlags flags;

		// Token: 0x040008EE RID: 2286
		private bool disposed;
	}
}
