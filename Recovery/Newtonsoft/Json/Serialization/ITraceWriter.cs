using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	public interface ITraceWriter
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600067F RID: 1663
		TraceLevel LevelFilter { get; }

		// Token: 0x06000680 RID: 1664
		void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex);
	}
}
