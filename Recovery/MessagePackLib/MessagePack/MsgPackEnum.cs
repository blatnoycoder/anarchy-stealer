using System;
using System.Collections;
using System.Collections.Generic;

namespace MessagePackLib.MessagePack
{
	// Token: 0x02000030 RID: 48
	public class MsgPackEnum : IEnumerator
	{
		// Token: 0x060000DE RID: 222 RVA: 0x0000826C File Offset: 0x0000646C
		public MsgPackEnum(List<MsgPack> obj)
		{
			this.children = obj;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00008284 File Offset: 0x00006484
		object IEnumerator.Current
		{
			get
			{
				return this.children[this.position];
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00008298 File Offset: 0x00006498
		bool IEnumerator.MoveNext()
		{
			this.position++;
			return this.position < this.children.Count;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000082BC File Offset: 0x000064BC
		void IEnumerator.Reset()
		{
			this.position = -1;
		}

		// Token: 0x040000B7 RID: 183
		private List<MsgPack> children;

		// Token: 0x040000B8 RID: 184
		private int position = -1;
	}
}
