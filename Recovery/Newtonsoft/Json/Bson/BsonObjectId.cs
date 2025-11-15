using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000139 RID: 313
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0003E528 File Offset: 0x0003C728
		public byte[] Value { get; }

		// Token: 0x06000D3B RID: 3387 RVA: 0x0003E530 File Offset: 0x0003C730
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
