using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.SQLite
{
	// Token: 0x020001BB RID: 443
	internal sealed class SQLiteStatementHandle : CriticalHandle
	{
		// Token: 0x06001422 RID: 5154 RVA: 0x0005D648 File Offset: 0x0005B848
		public static implicit operator IntPtr(SQLiteStatementHandle stmt)
		{
			if (stmt != null)
			{
				return stmt.handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0005D65C File Offset: 0x0005B85C
		internal SQLiteStatementHandle(SQLiteConnectionHandle cnn, IntPtr stmt)
			: this()
		{
			this.cnn = cnn;
			base.SetHandle(stmt);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0005D674 File Offset: 0x0005B874
		private SQLiteStatementHandle()
			: base(IntPtr.Zero)
		{
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0005D684 File Offset: 0x0005B884
		protected override bool ReleaseHandle()
		{
			try
			{
				IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					SQLiteBase.FinalizeStatement(this.cnn, intPtr);
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

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0005D6F4 File Offset: 0x0005B8F4
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x04000846 RID: 2118
		private SQLiteConnectionHandle cnn;
	}
}
