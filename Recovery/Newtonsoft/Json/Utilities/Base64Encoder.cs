using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000076 RID: 118
	[NullableContext(1)]
	[Nullable(0)]
	internal class Base64Encoder
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x000155A0 File Offset: 0x000137A0
		public Base64Encoder(TextWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000155C8 File Offset: 0x000137C8
		private void ValidateEncode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00015624 File Offset: 0x00013824
		public void Encode(byte[] buffer, int index, int count)
		{
			this.ValidateEncode(buffer, index, count);
			if (this._leftOverBytesCount > 0)
			{
				if (this.FulfillFromLeftover(buffer, index, ref count))
				{
					return;
				}
				int num = Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, num);
			}
			this.StoreLeftOverBytes(buffer, index, ref count);
			int num2 = index + count;
			int num3 = 57;
			while (index < num2)
			{
				if (index + num3 > num2)
				{
					num3 = num2 - index;
				}
				int num4 = Convert.ToBase64CharArray(buffer, index, num3, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, num4);
				index += num3;
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000156C8 File Offset: 0x000138C8
		private void StoreLeftOverBytes(byte[] buffer, int index, ref int count)
		{
			int num = count % 3;
			if (num > 0)
			{
				count -= num;
				if (this._leftOverBytes == null)
				{
					this._leftOverBytes = new byte[3];
				}
				for (int i = 0; i < num; i++)
				{
					this._leftOverBytes[i] = buffer[index + count + i];
				}
			}
			this._leftOverBytesCount = num;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00015728 File Offset: 0x00013928
		private bool FulfillFromLeftover(byte[] buffer, int index, ref int count)
		{
			int leftOverBytesCount = this._leftOverBytesCount;
			while (leftOverBytesCount < 3 && count > 0)
			{
				this._leftOverBytes[leftOverBytesCount++] = buffer[index++];
				count--;
			}
			if (count == 0 && leftOverBytesCount < 3)
			{
				this._leftOverBytesCount = leftOverBytesCount;
				return true;
			}
			return false;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00015784 File Offset: 0x00013984
		public void Flush()
		{
			if (this._leftOverBytesCount > 0)
			{
				int num = Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, num);
				this._leftOverBytesCount = 0;
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000157D0 File Offset: 0x000139D0
		private void WriteChars(char[] chars, int index, int count)
		{
			this._writer.Write(chars, index, count);
		}

		// Token: 0x04000217 RID: 535
		private const int Base64LineSize = 76;

		// Token: 0x04000218 RID: 536
		private const int LineSizeInBytes = 57;

		// Token: 0x04000219 RID: 537
		private readonly char[] _charsLine = new char[76];

		// Token: 0x0400021A RID: 538
		private readonly TextWriter _writer;

		// Token: 0x0400021B RID: 539
		[Nullable(2)]
		private byte[] _leftOverBytes;

		// Token: 0x0400021C RID: 540
		private int _leftOverBytesCount;
	}
}
