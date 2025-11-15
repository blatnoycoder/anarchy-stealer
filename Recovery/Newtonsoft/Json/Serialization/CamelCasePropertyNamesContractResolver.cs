using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(1)]
	[Nullable(0)]
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		// Token: 0x0600060B RID: 1547 RVA: 0x0001F588 File Offset: 0x0001D788
		public CamelCasePropertyNamesContractResolver()
		{
			base.NamingStrategy = new CamelCaseNamingStrategy
			{
				ProcessDictionaryKeys = true,
				OverrideSpecifiedNames = true
			};
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		public override JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			StructMultiKey<Type, Type> structMultiKey = new StructMultiKey<Type, Type>(base.GetType(), type);
			Dictionary<StructMultiKey<Type, Type>, JsonContract> dictionary = CamelCasePropertyNamesContractResolver._contractCache;
			JsonContract jsonContract;
			if (dictionary == null || !dictionary.TryGetValue(structMultiKey, out jsonContract))
			{
				jsonContract = this.CreateContract(type);
				object typeContractCacheLock = CamelCasePropertyNamesContractResolver.TypeContractCacheLock;
				lock (typeContractCacheLock)
				{
					dictionary = CamelCasePropertyNamesContractResolver._contractCache;
					Dictionary<StructMultiKey<Type, Type>, JsonContract> dictionary2 = ((dictionary != null) ? new Dictionary<StructMultiKey<Type, Type>, JsonContract>(dictionary) : new Dictionary<StructMultiKey<Type, Type>, JsonContract>());
					dictionary2[structMultiKey] = jsonContract;
					CamelCasePropertyNamesContractResolver._contractCache = dictionary2;
				}
			}
			return jsonContract;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001F66C File Offset: 0x0001D86C
		internal override DefaultJsonNameTable GetNameTable()
		{
			return CamelCasePropertyNamesContractResolver.NameTable;
		}

		// Token: 0x040002D8 RID: 728
		private static readonly object TypeContractCacheLock = new object();

		// Token: 0x040002D9 RID: 729
		private static readonly DefaultJsonNameTable NameTable = new DefaultJsonNameTable();

		// Token: 0x040002DA RID: 730
		[Nullable(new byte[] { 2, 0, 1, 1, 1 })]
		private static Dictionary<StructMultiKey<Type, Type>, JsonContract> _contractCache;
	}
}
