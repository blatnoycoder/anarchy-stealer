using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000F6 RID: 246
	[NullableContext(1)]
	[Nullable(0)]
	public class JRaw : JValue
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x0003242C File Offset: 0x0003062C
		public JRaw(JRaw other)
			: base(other)
		{
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00032438 File Offset: 0x00030638
		[NullableContext(2)]
		public JRaw(object rawJson)
			: base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00032444 File Offset: 0x00030644
		public static JRaw Create(JsonReader reader)
		{
			JRaw jraw;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					jraw = new JRaw(stringWriter.ToString());
				}
			}
			return jraw;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000324B4 File Offset: 0x000306B4
		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
