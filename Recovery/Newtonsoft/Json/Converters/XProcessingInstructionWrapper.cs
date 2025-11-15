using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000131 RID: 305
	[NullableContext(2)]
	[Nullable(0)]
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0003B8FC File Offset: 0x00039AFC
		[Nullable(1)]
		private XProcessingInstruction ProcessingInstruction
		{
			[NullableContext(1)]
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0003B90C File Offset: 0x00039B0C
		[NullableContext(1)]
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
			: base(processingInstruction)
		{
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0003B918 File Offset: 0x00039B18
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0003B928 File Offset: 0x00039B28
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x0003B938 File Offset: 0x00039B38
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value;
			}
		}
	}
}
