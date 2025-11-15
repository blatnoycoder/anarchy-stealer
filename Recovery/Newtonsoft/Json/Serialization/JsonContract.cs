using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000BF RID: 191
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonContract
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00022878 File Offset: 0x00020A78
		public Type UnderlyingType { get; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00022880 File Offset: 0x00020A80
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x00022888 File Offset: 0x00020A88
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = !this._createdType.IsInterface() && !this._createdType.IsAbstract();
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x000228E8 File Offset: 0x00020AE8
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x000228F0 File Offset: 0x00020AF0
		public bool? IsReference { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000228FC File Offset: 0x00020AFC
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00022904 File Offset: 0x00020B04
		[Nullable(2)]
		public JsonConverter Converter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00022910 File Offset: 0x00020B10
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00022918 File Offset: 0x00020B18
		[Nullable(2)]
		public JsonConverter InternalConverter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			internal set;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00022924 File Offset: 0x00020B24
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00022944 File Offset: 0x00020B44
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00022964 File Offset: 0x00020B64
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00022984 File Offset: 0x00020B84
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x000229A4 File Offset: 0x00020BA4
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x000229C4 File Offset: 0x00020BC4
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x000229CC File Offset: 0x00020BCC
		[Nullable(new byte[] { 2, 1 })]
		public Func<object> DefaultCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x000229D8 File Offset: 0x00020BD8
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x000229E0 File Offset: 0x00020BE0
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x060006C1 RID: 1729 RVA: 0x000229EC File Offset: 0x00020BEC
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((this.IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			this._createdType = (this.CreatedType = this.NonNullableUnderlyingType);
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00022A90 File Offset: 0x00020C90
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00022AF4 File Offset: 0x00020CF4
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00022B58 File Offset: 0x00020D58
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00022BBC File Offset: 0x00020DBC
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00022C20 File Offset: 0x00020E20
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00022C84 File Offset: 0x00020E84
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[] { context });
			};
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00022CA0 File Offset: 0x00020EA0
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[] { context, econtext });
			};
		}

		// Token: 0x04000316 RID: 790
		internal bool IsNullable;

		// Token: 0x04000317 RID: 791
		internal bool IsConvertable;

		// Token: 0x04000318 RID: 792
		internal bool IsEnum;

		// Token: 0x04000319 RID: 793
		internal Type NonNullableUnderlyingType;

		// Token: 0x0400031A RID: 794
		internal ReadType InternalReadType;

		// Token: 0x0400031B RID: 795
		internal JsonContractType ContractType;

		// Token: 0x0400031C RID: 796
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x0400031D RID: 797
		internal bool IsSealed;

		// Token: 0x0400031E RID: 798
		internal bool IsInstantiable;

		// Token: 0x0400031F RID: 799
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x04000320 RID: 800
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x04000321 RID: 801
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x04000322 RID: 802
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x04000323 RID: 803
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x04000324 RID: 804
		private Type _createdType;
	}
}
