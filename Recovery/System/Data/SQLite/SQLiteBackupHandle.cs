using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001BC RID: 444
	internal sealed class SQLiteBackupHandle : CriticalHandle
	{
		// Token: 0x06001427 RID: 5159 RVA: 0x0005D708 File Offset: 0x0005B908
		public static implicit operator IntPtr(SQLiteBackupHandle backup)
		{
			if (backup != null)
			{
				return backup.handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0005D71C File Offset: 0x0005B91C
		internal SQLiteBackupHandle(SQLiteConnectionHandle cnn, IntPtr backup)
			: this()
		{
			this.cnn = cnn;
			base.SetHandle(backup);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0005D734 File Offset: 0x0005B934
		private SQLiteBackupHandle()
			: base(IntPtr.Zero)
		{
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0005D744 File Offset: 0x0005B944
		protected override bool ReleaseHandle()
		{
			try
			{
				IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					SQLiteBase.FinishBackup(this.cnn, intPtr);
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

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0005D7B4 File Offset: 0x0005B9B4
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x04000847 RID: 2119
		private SQLiteConnectionHandle cnn;
	}
}
