using System;

namespace System.Data.SQLite
{
	// Token: 0x0200015E RID: 350
	public class SQLiteReadArrayEventArgs : SQLiteReadEventArgs
	{
		// Token: 0x06000FB6 RID: 4022 RVA: 0x00048DD4 File Offset: 0x00046FD4
		internal SQLiteReadArrayEventArgs(long dataOffset, byte[] byteBuffer, int bufferOffset, int length)
		{
			this.dataOffset = dataOffset;
			this.byteBuffer = byteBuffer;
			this.bufferOffset = bufferOffset;
			this.length = length;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00048DFC File Offset: 0x00046FFC
		internal SQLiteReadArrayEventArgs(long dataOffset, char[] charBuffer, int bufferOffset, int length)
		{
			this.dataOffset = dataOffset;
			this.charBuffer = charBuffer;
			this.bufferOffset = bufferOffset;
			this.length = length;
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x00048E24 File Offset: 0x00047024
		// (set) Token: 0x06000FB9 RID: 4025 RVA: 0x00048E2C File Offset: 0x0004702C
		public long DataOffset
		{
			get
			{
				return this.dataOffset;
			}
			set
			{
				this.dataOffset = value;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x00048E38 File Offset: 0x00047038
		public byte[] ByteBuffer
		{
			get
			{
				return this.byteBuffer;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00048E40 File Offset: 0x00047040
		public char[] CharBuffer
		{
			get
			{
				return this.charBuffer;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00048E48 File Offset: 0x00047048
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x00048E50 File Offset: 0x00047050
		public int BufferOffset
		{
			get
			{
				return this.bufferOffset;
			}
			set
			{
				this.bufferOffset = value;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00048E5C File Offset: 0x0004705C
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x00048E64 File Offset: 0x00047064
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		// Token: 0x0400061C RID: 1564
		private long dataOffset;

		// Token: 0x0400061D RID: 1565
		private byte[] byteBuffer;

		// Token: 0x0400061E RID: 1566
		private char[] charBuffer;

		// Token: 0x0400061F RID: 1567
		private int bufferOffset;

		// Token: 0x04000620 RID: 1568
		private int length;
	}
}
