using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200004B RID: 75
	public interface IJsonLineInfo
	{
		// Token: 0x06000138 RID: 312
		bool HasLineInfo();

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000139 RID: 313
		int LineNumber { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600013A RID: 314
		int LinePosition { get; }
	}
}
