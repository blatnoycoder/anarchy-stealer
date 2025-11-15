using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001BD RID: 445
	internal sealed class SQLiteBlobHandle : CriticalHandle
	{
		// Token: 0x0600142C RID: 5164 RVA: 0x0005D7C8 File Offset: 0x0005B9C8
		public static implicit operator IntPtr(SQLiteBlobHandle blob)
		{
			if (blob != null)
			{
				return blob.handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0005D7DC File Offset: 0x0005B9DC
		internal SQLiteBlobHandle(SQLiteConnectionHandle cnn, IntPtr blob)
			: this()
		{
			this.cnn = cnn;
			base.SetHandle(blob);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0005D7F4 File Offset: 0x0005B9F4
		private SQLiteBlobHandle()
			: base(IntPtr.Zero)
		{
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0005D804 File Offset: 0x0005BA04
		protected override bool ReleaseHandle()
		{
			try
			{
				IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					SQLiteBase.CloseBlob(this.cnn, intPtr);
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

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0005D874 File Offset: 0x0005BA74
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x04000848 RID: 2120
		private SQLiteConnectionHandle cnn;
	}
}
