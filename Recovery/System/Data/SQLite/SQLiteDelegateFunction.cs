using System;
using System.Globalization;

namespace System.Data.SQLite
{
	// Token: 0x020001A0 RID: 416
	public class SQLiteDelegateFunction : SQLiteFunction
	{
		// Token: 0x0600123C RID: 4668 RVA: 0x00057644 File Offset: 0x00055844
		public SQLiteDelegateFunction()
			: this(null, null)
		{
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00057650 File Offset: 0x00055850
		public SQLiteDelegateFunction(Delegate callback1, Delegate callback2)
		{
			this.callback1 = callback1;
			this.callback2 = callback2;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00057668 File Offset: 0x00055868
		protected virtual object[] GetInvokeArgs(object[] args, bool earlyBound)
		{
			object[] array = new object[] { "Invoke", args };
			if (!earlyBound)
			{
				array = new object[] { array };
			}
			return array;
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x000576A4 File Offset: 0x000558A4
		protected virtual object[] GetStepArgs(object[] args, int stepNumber, object contextData, bool earlyBound)
		{
			object[] array = new object[] { "Step", args, stepNumber, contextData };
			if (!earlyBound)
			{
				array = new object[] { array };
			}
			return array;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x000576EC File Offset: 0x000558EC
		protected virtual void UpdateStepArgs(object[] args, ref object contextData, bool earlyBound)
		{
			object[] array;
			if (earlyBound)
			{
				array = args;
			}
			else
			{
				array = args[0] as object[];
			}
			if (array == null)
			{
				return;
			}
			contextData = array[array.Length - 1];
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00057724 File Offset: 0x00055924
		protected virtual object[] GetFinalArgs(object contextData, bool earlyBound)
		{
			object[] array = new object[] { "Final", contextData };
			if (!earlyBound)
			{
				array = new object[] { array };
			}
			return array;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00057760 File Offset: 0x00055960
		protected virtual object[] GetCompareArgs(string param1, string param2, bool earlyBound)
		{
			object[] array = new object[] { "Compare", param1, param2 };
			if (!earlyBound)
			{
				array = new object[] { array };
			}
			return array;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x000577A0 File Offset: 0x000559A0
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x000577A8 File Offset: 0x000559A8
		public virtual Delegate Callback1
		{
			get
			{
				return this.callback1;
			}
			set
			{
				this.callback1 = value;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x000577B4 File Offset: 0x000559B4
		// (set) Token: 0x06001246 RID: 4678 RVA: 0x000577BC File Offset: 0x000559BC
		public virtual Delegate Callback2
		{
			get
			{
				return this.callback2;
			}
			set
			{
				this.callback2 = value;
			}
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000577C8 File Offset: 0x000559C8
		public override object Invoke(object[] args)
		{
			if (this.callback1 == null)
			{
				throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", new object[] { "Invoke" }));
			}
			SQLiteInvokeDelegate sqliteInvokeDelegate = this.callback1 as SQLiteInvokeDelegate;
			if (sqliteInvokeDelegate != null)
			{
				return sqliteInvokeDelegate("Invoke", args);
			}
			return this.callback1.DynamicInvoke(this.GetInvokeArgs(args, false));
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0005783C File Offset: 0x00055A3C
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			if (this.callback1 == null)
			{
				throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", new object[] { "Step" }));
			}
			SQLiteStepDelegate sqliteStepDelegate = this.callback1 as SQLiteStepDelegate;
			if (sqliteStepDelegate != null)
			{
				sqliteStepDelegate("Step", args, stepNumber, ref contextData);
				return;
			}
			object[] stepArgs = this.GetStepArgs(args, stepNumber, contextData, false);
			this.callback1.DynamicInvoke(stepArgs);
			this.UpdateStepArgs(stepArgs, ref contextData, false);
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000578C0 File Offset: 0x00055AC0
		public override object Final(object contextData)
		{
			if (this.callback2 == null)
			{
				throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", new object[] { "Final" }));
			}
			SQLiteFinalDelegate sqliteFinalDelegate = this.callback2 as SQLiteFinalDelegate;
			if (sqliteFinalDelegate != null)
			{
				return sqliteFinalDelegate("Final", contextData);
			}
			return this.callback1.DynamicInvoke(this.GetFinalArgs(contextData, false));
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00057934 File Offset: 0x00055B34
		public override int Compare(string param1, string param2)
		{
			if (this.callback1 == null)
			{
				throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", new object[] { "Compare" }));
			}
			SQLiteCompareDelegate sqliteCompareDelegate = this.callback1 as SQLiteCompareDelegate;
			if (sqliteCompareDelegate != null)
			{
				return sqliteCompareDelegate("Compare", param1, param2);
			}
			object obj = this.callback1.DynamicInvoke(this.GetCompareArgs(param1, param2, false));
			if (obj is int)
			{
				return (int)obj;
			}
			throw new InvalidOperationException(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "\"{0}\" result must be Int32.", new object[] { "Compare" }));
		}

		// Token: 0x040007C2 RID: 1986
		private const string NoCallbackError = "No \"{0}\" callback is set.";

		// Token: 0x040007C3 RID: 1987
		private const string ResultInt32Error = "\"{0}\" result must be Int32.";

		// Token: 0x040007C4 RID: 1988
		private Delegate callback1;

		// Token: 0x040007C5 RID: 1989
		private Delegate callback2;
	}
}
