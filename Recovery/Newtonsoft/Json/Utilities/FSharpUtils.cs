using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200008F RID: 143
	[NullableContext(1)]
	[Nullable(0)]
	internal class FSharpUtils
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x0001BB94 File Offset: 0x00019D94
		private FSharpUtils(Assembly fsharpCoreAssembly)
		{
			this.FSharpCoreAssembly = fsharpCoreAssembly;
			Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, "IsUnion", BindingFlags.Static | BindingFlags.Public);
			this.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodInfo methodWithNonPublicFallback2 = FSharpUtils.GetMethodWithNonPublicFallback(type, "GetUnionCases", BindingFlags.Static | BindingFlags.Public);
			this.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback2);
			Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
			this.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionTagReader");
			this.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionReader");
			this.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionConstructor");
			Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
			this.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
			this.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Tag"));
			this.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("DeclaringType"));
			this.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
			Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
			this._ofSeq = type4.GetMethod("OfSeq");
			this._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001BCE4 File Offset: 0x00019EE4
		public static FSharpUtils Instance
		{
			get
			{
				return FSharpUtils._instance;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0001BCEC File Offset: 0x00019EEC
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x0001BCF4 File Offset: 0x00019EF4
		public Assembly FSharpCoreAssembly { get; private set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001BD00 File Offset: 0x00019F00
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x0001BD08 File Offset: 0x00019F08
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> IsUnion
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001BD14 File Offset: 0x00019F14
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x0001BD1C File Offset: 0x00019F1C
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> GetUnionCases
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001BD28 File Offset: 0x00019F28
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x0001BD30 File Offset: 0x00019F30
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionTagReader
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001BD3C File Offset: 0x00019F3C
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x0001BD44 File Offset: 0x00019F44
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionReader
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001BD50 File Offset: 0x00019F50
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x0001BD58 File Offset: 0x00019F58
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionConstructor
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001BD64 File Offset: 0x00019F64
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x0001BD6C File Offset: 0x00019F6C
		public Func<object, object> GetUnionCaseInfoDeclaringType { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001BD78 File Offset: 0x00019F78
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x0001BD80 File Offset: 0x00019F80
		public Func<object, object> GetUnionCaseInfoName { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001BD8C File Offset: 0x00019F8C
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x0001BD94 File Offset: 0x00019F94
		public Func<object, object> GetUnionCaseInfoTag { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001BDA0 File Offset: 0x00019FA0
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x0001BDA8 File Offset: 0x00019FA8
		[Nullable(new byte[] { 1, 1, 2 })]
		public MethodCall<object, object> GetUnionCaseInfoFields
		{
			[return: Nullable(new byte[] { 1, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 1, 1, 2 })]
			private set;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (FSharpUtils._instance == null)
			{
				object @lock = FSharpUtils.Lock;
				lock (@lock)
				{
					if (FSharpUtils._instance == null)
					{
						FSharpUtils._instance = new FSharpUtils(fsharpCoreAssembly);
					}
				}
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001BE14 File Offset: 0x0001A014
		private static MethodInfo GetMethodWithNonPublicFallback(Type type, string methodName, BindingFlags bindingFlags)
		{
			MethodInfo methodInfo = type.GetMethod(methodName, bindingFlags);
			if (methodInfo == null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
			{
				methodInfo = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
			}
			return methodInfo;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001BE54 File Offset: 0x0001A054
		[return: Nullable(new byte[] { 1, 2, 1 })]
		private static MethodCall<object, object> CreateFSharpFuncCall(Type type, string methodName)
		{
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
			MethodInfo method = methodWithNonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodCall<object, object> invoke = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return ([Nullable(2)] object target, [Nullable(new byte[] { 1, 2 })] object[] args) => new FSharpFunction(call(target, args), invoke);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
		public ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo methodInfo = this._ofSeq.MakeGenericMethod(new Type[] { t });
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001BEE8 File Offset: 0x0001A0E8
		public ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			return (ObjectConstructor<object>)typeof(FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(new Type[] { keyType, valueType }).Invoke(this, null);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001BF2C File Offset: 0x0001A12C
		[NullableContext(2)]
		[return: Nullable(1)]
		public ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			ConstructorInfo constructor = this._mapType.MakeGenericType(new Type[]
			{
				typeof(TKey),
				typeof(TValue)
			}).GetConstructor(new Type[] { typeof(IEnumerable<Tuple<TKey, TValue>>) });
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			return delegate([Nullable(new byte[] { 1, 2 })] object[] args)
			{
				IEnumerable<Tuple<TKey, TValue>> enumerable = ((IEnumerable<KeyValuePair<TKey, TValue>>)args[0]).Select((KeyValuePair<TKey, TValue> kv) => new Tuple<TKey, TValue>(kv.Key, kv.Value));
				return ctorDelegate(new object[] { enumerable });
			};
		}

		// Token: 0x04000295 RID: 661
		private static readonly object Lock = new object();

		// Token: 0x04000296 RID: 662
		[Nullable(2)]
		private static FSharpUtils _instance;

		// Token: 0x04000297 RID: 663
		private MethodInfo _ofSeq;

		// Token: 0x04000298 RID: 664
		private Type _mapType;

		// Token: 0x040002A3 RID: 675
		public const string FSharpSetTypeName = "FSharpSet`1";

		// Token: 0x040002A4 RID: 676
		public const string FSharpListTypeName = "FSharpList`1";

		// Token: 0x040002A5 RID: 677
		public const string FSharpMapTypeName = "FSharpMap`2";
	}
}
