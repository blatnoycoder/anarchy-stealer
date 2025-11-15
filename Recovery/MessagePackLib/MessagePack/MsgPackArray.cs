using System;
using System.Collections.Generic;

namespace MessagePackLib.MessagePack
{
	// Token: 0x02000031 RID: 49
	public class MsgPackArray
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x000082C8 File Offset: 0x000064C8
		public MsgPackArray(MsgPack msgpackObj, List<MsgPack> listObj)
		{
			this.owner = msgpackObj;
			this.children = listObj;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000082E0 File Offset: 0x000064E0
		public MsgPack Add()
		{
			return this.owner.AddArrayChild();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000082F0 File Offset: 0x000064F0
		public MsgPack Add(string value)
		{
			MsgPack msgPack = this.owner.AddArrayChild();
			msgPack.AsString = value;
			return msgPack;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008304 File Offset: 0x00006504
		public MsgPack Add(long value)
		{
			MsgPack msgPack = this.owner.AddArrayChild();
			msgPack.SetAsInteger(value);
			return msgPack;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008318 File Offset: 0x00006518
		public MsgPack Add(double value)
		{
			MsgPack msgPack = this.owner.AddArrayChild();
			msgPack.SetAsFloat(value);
			return msgPack;
		}

		// Token: 0x1700001B RID: 27
		public MsgPack this[int index]
		{
			get
			{
				return this.children[index];
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000833C File Offset: 0x0000653C
		public int Length
		{
			get
			{
				return this.children.Count;
			}
		}

		// Token: 0x040000B9 RID: 185
		private List<MsgPack> children;

		// Token: 0x040000BA RID: 186
		private MsgPack owner;
	}
}
