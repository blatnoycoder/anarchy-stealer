using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001BA RID: 442
	internal sealed class SQLiteConnectionHandle : CriticalHandle
	{
		// Token: 0x0600141C RID: 5148 RVA: 0x0005D578 File Offset: 0x0005B778
		public static implicit operator IntPtr(SQLiteConnectionHandle db)
		{
			if (db != null)
			{
				return db.handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0005D58C File Offset: 0x0005B78C
		internal SQLiteConnectionHandle(IntPtr db, bool ownHandle)
			: this(ownHandle)
		{
			this.ownHandle = ownHandle;
			base.SetHandle(db);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0005D5A4 File Offset: 0x0005B7A4
		private SQLiteConnectionHandle(bool ownHandle)
			: base(IntPtr.Zero)
		{
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005D5B4 File Offset: 0x0005B7B4
		protected override bool ReleaseHandle()
		{
			if (!this.ownHandle)
			{
				return true;
			}
			try
			{
				IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					SQLiteBase.CloseConnection(this, intPtr);
				}
			}
			catch (SQLiteException)
			{
			}
			finally
			{
				base.SetHandleAsInvalid();
			}
			return true;
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0005D62C File Offset: 0x0005B82C
		public bool OwnHandle
		{
			get
			{
				return this.ownHandle;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0005D634 File Offset: 0x0005B834
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x04000845 RID: 2117
		private bool ownHandle;
	}
}
