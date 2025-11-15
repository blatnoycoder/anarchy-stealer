using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000D0 RID: 208
	[NullableContext(1)]
	[Nullable(0)]
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0002A9CC File Offset: 0x00028BCC
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0002A9D4 File Offset: 0x00028BD4
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000819 RID: 2073 RVA: 0x0002A9E0 File Offset: 0x00028BE0
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
			this._lock = new object();
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002AA08 File Offset: 0x00028C08
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture));
			stringBuilder.Append(" ");
			stringBuilder.Append(level.ToString("g"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			string text = stringBuilder.ToString();
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._traceMessages.Count >= 1000)
				{
					this._traceMessages.Dequeue();
				}
				this._traceMessages.Enqueue(text);
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002AAD8 File Offset: 0x00028CD8
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002AAE0 File Offset: 0x00028CE0
		public override string ToString()
		{
			object @lock = this._lock;
			string text2;
			lock (@lock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this._traceMessages)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
				text2 = stringBuilder.ToString();
			}
			return text2;
		}

		// Token: 0x0400038A RID: 906
		private readonly Queue<string> _traceMessages;

		// Token: 0x0400038B RID: 907
		private readonly object _lock;
	}
}
