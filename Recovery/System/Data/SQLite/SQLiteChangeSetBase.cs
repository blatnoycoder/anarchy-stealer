using System;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001ED RID: 493
	internal class SQLiteChangeSetBase : SQLiteConnectionLock
	{
		// Token: 0x06001636 RID: 5686 RVA: 0x000643A4 File Offset: 0x000625A4
		internal SQLiteChangeSetBase(SQLiteConnectionHandle handle, SQLiteConnectionFlags flags)
			: base(handle, flags, true)
		{
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000643B0 File Offset: 0x000625B0
		private ISQLiteChangeSetMetadataItem CreateMetadataItem(IntPtr iterator)
		{
			return new SQLiteChangeSetMetadataItem(SQLiteChangeSetIterator.Attach(iterator));
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000643C0 File Offset: 0x000625C0
		protected UnsafeNativeMethods.xSessionFilter GetDelegate(SessionTableFilterCallback tableFilterCallback, object clientData)
		{
			if (tableFilterCallback == null)
			{
				return null;
			}
			return delegate(IntPtr context, IntPtr pTblName)
			{
				try
				{
					string text = SQLiteString.StringFromUtf8IntPtr(pTblName);
					return tableFilterCallback(clientData, text) ? 1 : 0;
				}
				catch (Exception ex)
				{
					try
					{
						if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
						{
							SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "xSessionFilter", ex }));
						}
					}
					catch
					{
					}
				}
				return 0;
			};
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00064408 File Offset: 0x00062608
		protected UnsafeNativeMethods.xSessionConflict GetDelegate(SessionConflictCallback conflictCallback, object clientData)
		{
			if (conflictCallback == null)
			{
				return null;
			}
			return delegate(IntPtr context, SQLiteChangeSetConflictType type, IntPtr iterator)
			{
				try
				{
					ISQLiteChangeSetMetadataItem isqliteChangeSetMetadataItem = this.CreateMetadataItem(iterator);
					if (isqliteChangeSetMetadataItem == null)
					{
						throw new SQLiteException("could not create metadata item");
					}
					return conflictCallback(clientData, type, isqliteChangeSetMetadataItem);
				}
				catch (Exception ex)
				{
					try
					{
						if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
						{
							SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "xSessionConflict", ex }));
						}
					}
					catch
					{
					}
				}
				return SQLiteChangeSetConflictResult.Abort;
			};
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00064450 File Offset: 0x00062650
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteChangeSetBase).Name);
			}
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00064474 File Offset: 0x00062674
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed)
				{
					base.Unlock();
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x040008FA RID: 2298
		private bool disposed;
	}
}
