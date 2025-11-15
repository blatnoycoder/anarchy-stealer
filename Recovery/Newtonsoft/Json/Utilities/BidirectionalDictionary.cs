using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000077 RID: 119
	[NullableContext(1)]
	[Nullable(0)]
	internal class BidirectionalDictionary<[Nullable(2)] TFirst, [Nullable(2)] TSecond>
	{
		// Token: 0x06000436 RID: 1078 RVA: 0x000157E0 File Offset: 0x000139E0
		public BidirectionalDictionary()
			: this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
		{
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000157F4 File Offset: 0x000139F4
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer)
			: this(firstEqualityComparer, secondEqualityComparer, "Duplicate item already exists for '{0}'.", "Duplicate item already exists for '{0}'.")
		{
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00015808 File Offset: 0x00013A08
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer, string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
			this._duplicateFirstErrorMessage = duplicateFirstErrorMessage;
			this._duplicateSecondErrorMessage = duplicateSecondErrorMessage;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00015838 File Offset: 0x00013A38
		public void Set(TFirst first, TSecond second)
		{
			TSecond tsecond;
			if (this._firstToSecond.TryGetValue(first, out tsecond) && !tsecond.Equals(second))
			{
				throw new ArgumentException(this._duplicateFirstErrorMessage.FormatWith(CultureInfo.InvariantCulture, first));
			}
			TFirst tfirst;
			if (this._secondToFirst.TryGetValue(second, out tfirst) && !tfirst.Equals(first))
			{
				throw new ArgumentException(this._duplicateSecondErrorMessage.FormatWith(CultureInfo.InvariantCulture, second));
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000158F4 File Offset: 0x00013AF4
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, out second);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00015904 File Offset: 0x00013B04
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, out first);
		}

		// Token: 0x0400021D RID: 541
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		// Token: 0x0400021E RID: 542
		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		// Token: 0x0400021F RID: 543
		private readonly string _duplicateFirstErrorMessage;

		// Token: 0x04000220 RID: 544
		private readonly string _duplicateSecondErrorMessage;
	}
}
