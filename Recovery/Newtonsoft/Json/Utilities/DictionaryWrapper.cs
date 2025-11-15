using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000083 RID: 131
	[NullableContext(1)]
	[Nullable(0)]
	internal class DictionaryWrapper<[Nullable(2)] TKey, [Nullable(2)] TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IWrappedDictionary, IDictionary, ICollection
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x00018F54 File Offset: 0x00017154
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00018F70 File Offset: 0x00017170
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00018F8C File Offset: 0x0001718C
		internal IDictionary<TKey, TValue> GenericDictionary
		{
			get
			{
				return this._genericDictionary;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00018F94 File Offset: 0x00017194
		public void Add(TKey key, TValue value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00018FE8 File Offset: 0x000171E8
		public bool ContainsKey(TKey key)
		{
			if (this._dictionary != null)
			{
				return this._dictionary.Contains(key);
			}
			return this.GenericDictionary.ContainsKey(key);
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00019014 File Offset: 0x00017214
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
				}
				return this.GenericDictionary.Keys;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00019044 File Offset: 0x00017244
		public bool Remove(TKey key)
		{
			if (this._dictionary == null)
			{
				return this.GenericDictionary.Remove(key);
			}
			if (this._dictionary.Contains(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00019098 File Offset: 0x00017298
		public bool TryGetValue(TKey key, [MaybeNull] out TValue value)
		{
			if (this._dictionary == null)
			{
				return this.GenericDictionary.TryGetValue(key, out value);
			}
			if (!this._dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)((object)this._dictionary[key]);
			return true;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00019100 File Offset: 0x00017300
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Values.Cast<TValue>().ToList<TValue>();
				}
				return this.GenericDictionary.Values;
			}
		}

		// Token: 0x170000D3 RID: 211
		public TValue this[TKey key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return (TValue)((object)this._dictionary[key]);
				}
				return this.GenericDictionary[key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				this.GenericDictionary[key] = value;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00019194 File Offset: 0x00017394
		public void Add([Nullable(new byte[] { 0, 1, 1 })] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				((IList)this._dictionary).Add(item);
				return;
			}
			IDictionary<TKey, TValue> genericDictionary = this._genericDictionary;
			if (genericDictionary == null)
			{
				return;
			}
			genericDictionary.Add(item);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000191D0 File Offset: 0x000173D0
		public void Clear()
		{
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
				return;
			}
			this.GenericDictionary.Clear();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000191F4 File Offset: 0x000173F4
		public bool Contains([Nullable(new byte[] { 0, 1, 1 })] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				return ((IList)this._dictionary).Contains(item);
			}
			return this.GenericDictionary.Contains(item);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00019224 File Offset: 0x00017424
		public void CopyTo([Nullable(new byte[] { 1, 0, 1, 1 })] KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dictionary != null)
			{
				using (IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry entry = enumerator.Entry;
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)entry.Key), (TValue)((object)entry.Value));
					}
					return;
				}
			}
			this.GenericDictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000192C0 File Offset: 0x000174C0
		public int Count
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Count;
				}
				return this.GenericDictionary.Count;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x000192E4 File Offset: 0x000174E4
		public bool IsReadOnly
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.IsReadOnly;
				}
				return this.GenericDictionary.IsReadOnly;
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00019308 File Offset: 0x00017508
		public bool Remove([Nullable(new byte[] { 0, 1, 1 })] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary == null)
			{
				return this.GenericDictionary.Remove(item);
			}
			if (!this._dictionary.Contains(item.Key))
			{
				return true;
			}
			if (object.Equals(this._dictionary[item.Key], item.Value))
			{
				this._dictionary.Remove(item.Key);
				return true;
			}
			return false;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00019398 File Offset: 0x00017598
		[return: Nullable(new byte[] { 1, 0, 1, 1 })]
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return (from DictionaryEntry de in this._dictionary
					select new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
			}
			return this.GenericDictionary.GetEnumerator();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000193F8 File Offset: 0x000175F8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00019400 File Offset: 0x00017600
		void IDictionary.Add(object key, object value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			this.GenericDictionary.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x170000D6 RID: 214
		[Nullable(2)]
		object IDictionary.this[object key]
		{
			[return: Nullable(2)]
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				return this.GenericDictionary[(TKey)((object)key)];
			}
			[param: Nullable(2)]
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				this.GenericDictionary[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00019498 File Offset: 0x00017698
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return this._dictionary.GetEnumerator();
			}
			return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this.GenericDictionary.GetEnumerator());
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000194C8 File Offset: 0x000176C8
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x000194F4 File Offset: 0x000176F4
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.IsFixedSize;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00019510 File Offset: 0x00017710
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys.ToList<TKey>();
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001953C File Offset: 0x0001773C
		public void Remove(object key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
				return;
			}
			this.GenericDictionary.Remove((TKey)((object)key));
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00019568 File Offset: 0x00017768
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values.ToList<TValue>();
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00019594 File Offset: 0x00017794
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._dictionary != null)
			{
				this._dictionary.CopyTo(array, index);
				return;
			}
			this.GenericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000195C4 File Offset: 0x000177C4
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._dictionary != null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x000195E0 File Offset: 0x000177E0
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00019608 File Offset: 0x00017808
		public object UnderlyingDictionary
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary;
				}
				return this.GenericDictionary;
			}
		}

		// Token: 0x04000282 RID: 642
		[Nullable(2)]
		private readonly IDictionary _dictionary;

		// Token: 0x04000283 RID: 643
		[Nullable(new byte[] { 2, 1, 1 })]
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x04000284 RID: 644
		[Nullable(2)]
		private object _syncRoot;

		// Token: 0x02000225 RID: 549
		[Nullable(0)]
		private readonly struct DictionaryEnumerator<[Nullable(2)] TEnumeratorKey, [Nullable(2)] TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060016E0 RID: 5856 RVA: 0x000659C0 File Offset: 0x00063BC0
			public DictionaryEnumerator([Nullable(new byte[] { 1, 0, 1, 1 })] IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x060016E1 RID: 5857 RVA: 0x000659D4 File Offset: 0x00063BD4
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x060016E2 RID: 5858 RVA: 0x000659E4 File Offset: 0x00063BE4
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x170003CA RID: 970
			// (get) Token: 0x060016E3 RID: 5859 RVA: 0x00065A04 File Offset: 0x00063C04
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x170003CB RID: 971
			// (get) Token: 0x060016E4 RID: 5860 RVA: 0x00065A24 File Offset: 0x00063C24
			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object obj = keyValuePair.Key;
					keyValuePair = this._e.Current;
					return new DictionaryEntry(obj, keyValuePair.Value);
				}
			}

			// Token: 0x060016E5 RID: 5861 RVA: 0x00065A70 File Offset: 0x00063C70
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x060016E6 RID: 5862 RVA: 0x00065A80 File Offset: 0x00063C80
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x040009D9 RID: 2521
			[Nullable(new byte[] { 1, 0, 1, 1 })]
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
