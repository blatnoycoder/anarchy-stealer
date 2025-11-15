using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C3 RID: 451
	public sealed class SQLiteContext : ISQLiteNativeHandle
	{
		// Token: 0x06001469 RID: 5225 RVA: 0x0005E8F4 File Offset: 0x0005CAF4
		internal SQLiteContext(IntPtr pContext)
		{
			this.pContext = pContext;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x0005E904 File Offset: 0x0005CB04
		public IntPtr NativeHandle
		{
			get
			{
				return this.pContext;
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0005E90C File Offset: 0x0005CB0C
		public void SetNull()
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_null(this.pContext);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0005E934 File Offset: 0x0005CB34
		public void SetDouble(double value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_double(this.pContext, value);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0005E960 File Offset: 0x0005CB60
		public void SetInt(int value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_int(this.pContext, value);
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0005E98C File Offset: 0x0005CB8C
		public void SetInt64(long value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_int64(this.pContext, value);
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0005E9B8 File Offset: 0x0005CBB8
		public void SetString(string value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
			if (utf8BytesFromString == null)
			{
				throw new ArgumentNullException("value");
			}
			UnsafeNativeMethods.sqlite3_result_text(this.pContext, utf8BytesFromString, utf8BytesFromString.Length, (IntPtr)(-1));
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0005EA14 File Offset: 0x0005CC14
		public void SetError(string value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
			if (utf8BytesFromString == null)
			{
				throw new ArgumentNullException("value");
			}
			UnsafeNativeMethods.sqlite3_result_error(this.pContext, utf8BytesFromString, utf8BytesFromString.Length);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0005EA68 File Offset: 0x0005CC68
		public void SetErrorCode(SQLiteErrorCode value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_error_code(this.pContext, value);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0005EA94 File Offset: 0x0005CC94
		public void SetErrorTooBig()
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_error_toobig(this.pContext);
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0005EABC File Offset: 0x0005CCBC
		public void SetErrorNoMemory()
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_error_nomem(this.pContext);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0005EAE4 File Offset: 0x0005CCE4
		public void SetBlob(byte[] value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			UnsafeNativeMethods.sqlite3_result_blob(this.pContext, value, value.Length, (IntPtr)(-1));
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0005EB38 File Offset: 0x0005CD38
		public void SetZeroBlob(int value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			UnsafeNativeMethods.sqlite3_result_zeroblob(this.pContext, value);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0005EB64 File Offset: 0x0005CD64
		public void SetValue(SQLiteValue value)
		{
			if (this.pContext == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			UnsafeNativeMethods.sqlite3_result_value(this.pContext, value.NativeHandle);
		}

		// Token: 0x04000876 RID: 2166
		private IntPtr pContext;
	}
}
