using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001E9 RID: 489
	internal sealed class SQLiteStreamAdapter : IDisposable
	{
		// Token: 0x060015FF RID: 5631 RVA: 0x00063490 File Offset: 0x00061690
		public SQLiteStreamAdapter(Stream stream, SQLiteConnectionFlags flags)
		{
			this.stream = stream;
			this.flags = flags;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x000634A8 File Offset: 0x000616A8
		private SQLiteConnectionFlags GetFlags()
		{
			return this.flags;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x000634B0 File Offset: 0x000616B0
		public UnsafeNativeMethods.xSessionInput GetInputDelegate()
		{
			this.CheckDisposed();
			if (this.xInput == null)
			{
				this.xInput = new UnsafeNativeMethods.xSessionInput(this.Input);
			}
			return this.xInput;
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x000634DC File Offset: 0x000616DC
		public UnsafeNativeMethods.xSessionOutput GetOutputDelegate()
		{
			this.CheckDisposed();
			if (this.xOutput == null)
			{
				this.xOutput = new UnsafeNativeMethods.xSessionOutput(this.Output);
			}
			return this.xOutput;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00063508 File Offset: 0x00061708
		private SQLiteErrorCode Input(IntPtr context, IntPtr pData, ref int nData)
		{
			try
			{
				Stream stream = this.stream;
				if (stream == null)
				{
					return SQLiteErrorCode.Misuse;
				}
				if (nData > 0)
				{
					byte[] array = new byte[nData];
					int num = stream.Read(array, 0, nData);
					if (num > 0 && pData != IntPtr.Zero)
					{
						Marshal.Copy(array, 0, pData, num);
					}
					nData = num;
				}
				return SQLiteErrorCode.Ok;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "xSessionInput", ex }));
					}
				}
				catch
				{
				}
			}
			return SQLiteErrorCode.IoErr_Read;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000635E8 File Offset: 0x000617E8
		private SQLiteErrorCode Output(IntPtr context, IntPtr pData, int nData)
		{
			try
			{
				Stream stream = this.stream;
				if (stream == null)
				{
					return SQLiteErrorCode.Misuse;
				}
				if (nData > 0)
				{
					byte[] array = new byte[nData];
					if (pData != IntPtr.Zero)
					{
						Marshal.Copy(pData, array, 0, nData);
					}
					stream.Write(array, 0, nData);
				}
				stream.Flush();
				return SQLiteErrorCode.Ok;
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "xSessionOutput", ex }));
					}
				}
				catch
				{
				}
			}
			return SQLiteErrorCode.IoErr_Write;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x000636BC File Offset: 0x000618BC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x000636CC File Offset: 0x000618CC
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteStreamAdapter).Name);
			}
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x000636F0 File Offset: 0x000618F0
		private void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing)
				{
					if (this.xInput != null)
					{
						this.xInput = null;
					}
					if (this.xOutput != null)
					{
						this.xOutput = null;
					}
					if (this.stream != null)
					{
						this.stream = null;
					}
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00063764 File Offset: 0x00061964
		~SQLiteStreamAdapter()
		{
			this.Dispose(false);
		}

		// Token: 0x040008E7 RID: 2279
		private Stream stream;

		// Token: 0x040008E8 RID: 2280
		private SQLiteConnectionFlags flags;

		// Token: 0x040008E9 RID: 2281
		private UnsafeNativeMethods.xSessionInput xInput;

		// Token: 0x040008EA RID: 2282
		private UnsafeNativeMethods.xSessionOutput xOutput;

		// Token: 0x040008EB RID: 2283
		private bool disposed;
	}
}
