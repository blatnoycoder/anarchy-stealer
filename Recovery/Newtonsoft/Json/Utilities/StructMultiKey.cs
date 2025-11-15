using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StructMultiKey<[Nullable(2)] T1, [Nullable(2)] T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		// Token: 0x060005EC RID: 1516 RVA: 0x0001F218 File Offset: 0x0001D418
		public StructMultiKey(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001F228 File Offset: 0x0001D428
		public override int GetHashCode()
		{
			T1 value = this.Value1;
			ref T1 ptr = ref value;
			T1 t = default(T1);
			int num;
			if (t == null)
			{
				t = value;
				ptr = ref t;
				if (t == null)
				{
					num = 0;
					goto IL_0041;
				}
			}
			num = ptr.GetHashCode();
			IL_0041:
			T2 value2 = this.Value2;
			ref T2 ptr2 = ref value2;
			T2 t2 = default(T2);
			int num2;
			if (t2 == null)
			{
				t2 = value2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					num2 = 0;
					goto IL_0082;
				}
			}
			num2 = ptr2.GetHashCode();
			IL_0082:
			return num ^ num2;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001F2BC File Offset: 0x0001D4BC
		public override bool Equals(object obj)
		{
			if (obj is StructMultiKey<T1, T2>)
			{
				StructMultiKey<T1, T2> structMultiKey = (StructMultiKey<T1, T2>)obj;
				return this.Equals(structMultiKey);
			}
			return false;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
		public bool Equals([Nullable(new byte[] { 0, 1, 1 })] StructMultiKey<T1, T2> other)
		{
			return object.Equals(this.Value1, other.Value1) && object.Equals(this.Value2, other.Value2);
		}

		// Token: 0x040002D3 RID: 723
		public readonly T1 Value1;

		// Token: 0x040002D4 RID: 724
		public readonly T2 Value2;
	}
}
