using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AD RID: 173
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00021CC0 File Offset: 0x0001FEC0
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00021CC8 File Offset: 0x0001FEC8
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000661 RID: 1633 RVA: 0x00021CD4 File Offset: 0x0001FED4
		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00021D08 File Offset: 0x0001FF08
		[NullableContext(1)]
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache traceEventCache = new TraceEventCache();
			TraceEventType traceEventType = this.GetTraceEventType(level);
			foreach (object obj in global::System.Diagnostics.Trace.Listeners)
			{
				TraceListener traceListener = (TraceListener)obj;
				if (!traceListener.IsThreadSafe)
				{
					TraceListener traceListener2 = traceListener;
					lock (traceListener2)
					{
						traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
						goto IL_007D;
					}
					goto IL_006E;
				}
				goto IL_006E;
				IL_007D:
				if (global::System.Diagnostics.Trace.AutoFlush)
				{
					traceListener.Flush();
					continue;
				}
				continue;
				IL_006E:
				traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
				goto IL_007D;
			}
		}
	}
}
