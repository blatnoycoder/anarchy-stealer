using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000094 RID: 148
	internal static class JsonTokenUtils
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x0001CC30 File Offset: 0x0001AE30
		internal static bool IsEndToken(JsonToken token)
		{
			return token - JsonToken.EndObject <= 2;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001CC40 File Offset: 0x0001AE40
		internal static bool IsStartToken(JsonToken token)
		{
			return token - JsonToken.StartObject <= 2;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001CC50 File Offset: 0x0001AE50
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			return token - JsonToken.Integer <= 5 || token - JsonToken.Date <= 1;
		}
	}
}
