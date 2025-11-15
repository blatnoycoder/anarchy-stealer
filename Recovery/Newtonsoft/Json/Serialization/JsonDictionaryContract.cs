using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonDictionaryContract : JsonContainerContract
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00022CBC File Offset: 0x00020EBC
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00022CC4 File Offset: 0x00020EC4
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> DictionaryKeyResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00022CD0 File Offset: 0x00020ED0
		public Type DictionaryKeyType { get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00022CD8 File Offset: 0x00020ED8
		public Type DictionaryValueType { get; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00022CE0 File Offset: 0x00020EE0
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00022CE8 File Offset: 0x00020EE8
		internal JsonContract KeyContract { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00022CF4 File Offset: 0x00020EF4
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00022CFC File Offset: 0x00020EFC
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				if (this._parameterizedCreator == null && this._parameterizedConstructor != null)
				{
					this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(this._parameterizedConstructor);
				}
				return this._parameterizedCreator;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00022D38 File Offset: 0x00020F38
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00022D40 File Offset: 0x00020F40
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00022D4C File Offset: 0x00020F4C
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00022D54 File Offset: 0x00020F54
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00022D60 File Offset: 0x00020F60
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00022D88 File Offset: 0x00020F88
		[NullableContext(1)]
		public JsonDictionaryContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Dictionary;
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IDictionary<, >)))
				{
					base.CreatedType = typeof(Dictionary<, >).MakeGenericType(new Type[] { type, type2 });
				}
				else if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.ConcurrentDictionary`2")
				{
					this.ShouldCreateWrapper = true;
				}
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
				if (base.UnderlyingType == typeof(IDictionary))
				{
					base.CreatedType = typeof(Dictionary<object, object>);
				}
			}
			if (type != null && type2 != null)
			{
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, typeof(KeyValuePair<, >).MakeGenericType(new Type[] { type, type2 }), typeof(IDictionary<, >).MakeGenericType(new Type[] { type, type2 }));
				if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpMap`2")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					this._parameterizedCreator = FSharpUtils.Instance.CreateMap(type, type2);
				}
			}
			if (!typeof(IDictionary).IsAssignableFrom(base.CreatedType))
			{
				this.ShouldCreateWrapper = true;
			}
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			Type type3;
			ObjectConstructor<object> objectConstructor;
			if (this.DictionaryKeyType != null && this.DictionaryValueType != null && ImmutableCollectionsUtils.TryBuildImmutableForDictionaryContract(underlyingType, this.DictionaryKeyType, this.DictionaryValueType, out type3, out objectConstructor))
			{
				base.CreatedType = type3;
				this._parameterizedCreator = objectConstructor;
				this.IsReadOnlyOrFixedSize = true;
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00022FB4 File Offset: 0x000211B4
		[NullableContext(1)]
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(DictionaryWrapper<, >).MakeGenericType(new Type[] { this.DictionaryKeyType, this.DictionaryValueType });
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { this._genericCollectionDefinitionType });
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedDictionary)this._genericWrapperCreator(new object[] { dictionary });
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00023044 File Offset: 0x00021244
		[NullableContext(1)]
		internal IDictionary CreateTemporaryDictionary()
		{
			if (this._genericTemporaryDictionaryCreator == null)
			{
				Type type = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType ?? typeof(object),
					this.DictionaryValueType ?? typeof(object)
				});
				this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			return (IDictionary)this._genericTemporaryDictionaryCreator();
		}

		// Token: 0x0400032F RID: 815
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x04000330 RID: 816
		private Type _genericWrapperType;

		// Token: 0x04000331 RID: 817
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x04000332 RID: 818
		[Nullable(new byte[] { 2, 1 })]
		private Func<object> _genericTemporaryDictionaryCreator;

		// Token: 0x04000334 RID: 820
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x04000335 RID: 821
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000336 RID: 822
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;
	}
}
