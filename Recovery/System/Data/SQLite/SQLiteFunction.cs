using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Data.SQLite
{
	// Token: 0x0200019B RID: 411
	public abstract class SQLiteFunction : IDisposable
	{
		// Token: 0x06001211 RID: 4625 RVA: 0x0005672C File Offset: 0x0005492C
		protected SQLiteFunction()
		{
			this._contextDataList = new Dictionary<IntPtr, SQLiteFunction.AggregateData>();
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00056740 File Offset: 0x00054940
		protected SQLiteFunction(SQLiteDateFormats format, DateTimeKind kind, string formatString, bool utf16)
			: this()
		{
			if (utf16)
			{
				this._base = new SQLite3_UTF16(format, kind, formatString, IntPtr.Zero, null, false);
				return;
			}
			this._base = new SQLite3(format, kind, formatString, IntPtr.Zero, null, false);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0005677C File Offset: 0x0005497C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0005678C File Offset: 0x0005498C
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteFunction).Name);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x000567B0 File Offset: 0x000549B0
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					foreach (KeyValuePair<IntPtr, SQLiteFunction.AggregateData> keyValuePair in this._contextDataList)
					{
						IDisposable disposable = keyValuePair.Value._data as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					this._contextDataList.Clear();
					this._contextDataList = null;
					this._flags = SQLiteConnectionFlags.None;
					this._InvokeFunc = null;
					this._StepFunc = null;
					this._FinalFunc = null;
					this._CompareFunc = null;
					this._base = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00056878 File Offset: 0x00054A78
		~SQLiteFunction()
		{
			this.Dispose(false);
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x000568A8 File Offset: 0x00054AA8
		public SQLiteConvert SQLiteConvert
		{
			get
			{
				this.CheckDisposed();
				return this._base;
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000568B8 File Offset: 0x00054AB8
		public virtual object Invoke(object[] args)
		{
			this.CheckDisposed();
			return null;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x000568C4 File Offset: 0x00054AC4
		public virtual void Step(object[] args, int stepNumber, ref object contextData)
		{
			this.CheckDisposed();
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x000568CC File Offset: 0x00054ACC
		public virtual object Final(object contextData)
		{
			this.CheckDisposed();
			return null;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x000568D8 File Offset: 0x00054AD8
		public virtual int Compare(string param1, string param2)
		{
			this.CheckDisposed();
			return 0;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000568E4 File Offset: 0x00054AE4
		internal object[] ConvertParams(int nArgs, IntPtr argsptr)
		{
			object[] array = new object[nArgs];
			IntPtr[] array2 = new IntPtr[nArgs];
			Marshal.Copy(argsptr, array2, 0, nArgs);
			for (int i = 0; i < nArgs; i++)
			{
				switch (this._base.GetParamValueType(array2[i]))
				{
				case TypeAffinity.Int64:
					array[i] = this._base.GetParamValueInt64(array2[i]);
					break;
				case TypeAffinity.Double:
					array[i] = this._base.GetParamValueDouble(array2[i]);
					break;
				case TypeAffinity.Text:
					array[i] = this._base.GetParamValueText(array2[i]);
					break;
				case TypeAffinity.Blob:
				{
					int num = (int)this._base.GetParamValueBytes(array2[i], 0, null, 0, 0);
					byte[] array3 = new byte[num];
					this._base.GetParamValueBytes(array2[i], 0, array3, 0, num);
					array[i] = array3;
					break;
				}
				case TypeAffinity.Null:
					array[i] = DBNull.Value;
					break;
				case TypeAffinity.DateTime:
					array[i] = this._base.ToDateTime(this._base.GetParamValueText(array2[i]));
					break;
				}
			}
			return array;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00056A5C File Offset: 0x00054C5C
		private void SetReturnValue(IntPtr context, object returnValue)
		{
			if (returnValue == null || returnValue == DBNull.Value)
			{
				this._base.ReturnNull(context);
				return;
			}
			Type type = returnValue.GetType();
			if (type == typeof(DateTime))
			{
				this._base.ReturnText(context, this._base.ToString((DateTime)returnValue));
				return;
			}
			Exception ex = returnValue as Exception;
			if (ex != null)
			{
				this._base.ReturnError(context, ex.Message);
				return;
			}
			switch (SQLiteConvert.TypeToAffinity(type, this._flags))
			{
			case TypeAffinity.Int64:
				this._base.ReturnInt64(context, Convert.ToInt64(returnValue, CultureInfo.CurrentCulture));
				return;
			case TypeAffinity.Double:
				this._base.ReturnDouble(context, Convert.ToDouble(returnValue, CultureInfo.CurrentCulture));
				return;
			case TypeAffinity.Text:
				this._base.ReturnText(context, returnValue.ToString());
				return;
			case TypeAffinity.Blob:
				this._base.ReturnBlob(context, (byte[])returnValue);
				return;
			case TypeAffinity.Null:
				this._base.ReturnNull(context);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00056B74 File Offset: 0x00054D74
		internal void ScalarCallback(IntPtr context, int nArgs, IntPtr argsptr)
		{
			try
			{
				this._context = context;
				this.SetReturnValue(context, this.Invoke(this.ConvertParams(nArgs, argsptr)));
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Invoke", ex }));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00056C0C File Offset: 0x00054E0C
		internal int CompareCallback(IntPtr ptr, int len1, IntPtr ptr1, int len2, IntPtr ptr2)
		{
			try
			{
				return this.Compare(SQLiteConvert.UTF8ToString(ptr1, len1), SQLiteConvert.UTF8ToString(ptr2, len2));
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Compare", ex }));
					}
				}
				catch
				{
				}
			}
			if (this._base != null && this._base.IsOpen())
			{
				this._base.Cancel();
			}
			return 0;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00056CC8 File Offset: 0x00054EC8
		internal int CompareCallback16(IntPtr ptr, int len1, IntPtr ptr1, int len2, IntPtr ptr2)
		{
			try
			{
				return this.Compare(SQLite3_UTF16.UTF16ToString(ptr1, len1), SQLite3_UTF16.UTF16ToString(ptr2, len2));
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Compare (UTF16)", ex }));
					}
				}
				catch
				{
				}
			}
			if (this._base != null && this._base.IsOpen())
			{
				this._base.Cancel();
			}
			return 0;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00056D84 File Offset: 0x00054F84
		internal void StepCallback(IntPtr context, int nArgs, IntPtr argsptr)
		{
			try
			{
				SQLiteFunction.AggregateData aggregateData = null;
				if (this._base != null)
				{
					IntPtr intPtr = this._base.AggregateContext(context);
					if (this._contextDataList != null && !this._contextDataList.TryGetValue(intPtr, out aggregateData))
					{
						aggregateData = new SQLiteFunction.AggregateData();
						this._contextDataList[intPtr] = aggregateData;
					}
				}
				if (aggregateData == null)
				{
					aggregateData = new SQLiteFunction.AggregateData();
				}
				try
				{
					this._context = context;
					this.Step(this.ConvertParams(nArgs, argsptr), aggregateData._count, ref aggregateData._data);
				}
				finally
				{
					aggregateData._count++;
				}
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Step", ex }));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00056E98 File Offset: 0x00055098
		internal void FinalCallback(IntPtr context)
		{
			try
			{
				object obj = null;
				if (this._base != null)
				{
					IntPtr intPtr = this._base.AggregateContext(context);
					SQLiteFunction.AggregateData aggregateData;
					if (this._contextDataList != null && this._contextDataList.TryGetValue(intPtr, out aggregateData))
					{
						obj = aggregateData._data;
						this._contextDataList.Remove(intPtr);
					}
				}
				try
				{
					this._context = context;
					this.SetReturnValue(context, this.Final(obj));
				}
				finally
				{
					IDisposable disposable = obj as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				try
				{
					if (HelperMethods.LogCallbackExceptions(this._flags))
					{
						SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", new object[] { "Final", ex }));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00056FA0 File Offset: 0x000551A0
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		static SQLiteFunction()
		{
			try
			{
				if (UnsafeNativeMethods.GetSettingValue("No_SQLiteFunctions", null) == null)
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					int num = assemblies.Length;
					AssemblyName name = Assembly.GetExecutingAssembly().GetName();
					int i = 0;
					while (i < num)
					{
						bool flag = false;
						Type[] array;
						try
						{
							AssemblyName[] referencedAssemblies = assemblies[i].GetReferencedAssemblies();
							int num2 = referencedAssemblies.Length;
							for (int j = 0; j < num2; j++)
							{
								if (referencedAssemblies[j].Name == name.Name)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_0152;
							}
							array = assemblies[i].GetTypes();
						}
						catch (ReflectionTypeLoadException ex)
						{
							array = ex.Types;
						}
						goto IL_00C0;
						IL_0152:
						i++;
						continue;
						IL_00C0:
						int num3 = array.Length;
						for (int k = 0; k < num3; k++)
						{
							if (!(array[k] == null))
							{
								object[] customAttributes = array[k].GetCustomAttributes(typeof(SQLiteFunctionAttribute), false);
								int num4 = customAttributes.Length;
								for (int l = 0; l < num4; l++)
								{
									SQLiteFunctionAttribute sqliteFunctionAttribute = customAttributes[l] as SQLiteFunctionAttribute;
									if (sqliteFunctionAttribute != null)
									{
										sqliteFunctionAttribute.InstanceType = array[k];
										SQLiteFunction.ReplaceFunction(sqliteFunctionAttribute, null);
									}
								}
							}
						}
						goto IL_0152;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0005714C File Offset: 0x0005534C
		public static void RegisterFunction(Type typ)
		{
			object[] customAttributes = typ.GetCustomAttributes(typeof(SQLiteFunctionAttribute), false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				SQLiteFunctionAttribute sqliteFunctionAttribute = customAttributes[i] as SQLiteFunctionAttribute;
				if (sqliteFunctionAttribute != null)
				{
					SQLiteFunction.RegisterFunction(sqliteFunctionAttribute.Name, sqliteFunctionAttribute.Arguments, sqliteFunctionAttribute.FuncType, typ, sqliteFunctionAttribute.Callback1, sqliteFunctionAttribute.Callback2);
				}
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x000571B4 File Offset: 0x000553B4
		public static void RegisterFunction(string name, int argumentCount, FunctionType functionType, Type instanceType, Delegate callback1, Delegate callback2)
		{
			SQLiteFunction.ReplaceFunction(new SQLiteFunctionAttribute(name, argumentCount, functionType)
			{
				InstanceType = instanceType,
				Callback1 = callback1,
				Callback2 = callback2
			}, null);
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x000571F0 File Offset: 0x000553F0
		private static bool ReplaceFunction(SQLiteFunctionAttribute at, object newValue)
		{
			object obj;
			if (SQLiteFunction._registeredFunctions.TryGetValue(at, out obj))
			{
				IDisposable disposable = obj as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				SQLiteFunction._registeredFunctions[at] = newValue;
				return true;
			}
			SQLiteFunction._registeredFunctions.Add(at, newValue);
			return false;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00057244 File Offset: 0x00055444
		private static bool CreateFunction(SQLiteFunctionAttribute functionAttribute, out SQLiteFunction function)
		{
			if (functionAttribute == null)
			{
				function = null;
				return false;
			}
			if (functionAttribute.Callback1 != null || functionAttribute.Callback2 != null)
			{
				function = new SQLiteDelegateFunction(functionAttribute.Callback1, functionAttribute.Callback2);
				return true;
			}
			if (functionAttribute.InstanceType != null)
			{
				function = (SQLiteFunction)Activator.CreateInstance(functionAttribute.InstanceType);
				return true;
			}
			function = null;
			return false;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000572B4 File Offset: 0x000554B4
		internal static IDictionary<SQLiteFunctionAttribute, SQLiteFunction> BindFunctions(SQLiteBase sqlbase, SQLiteConnectionFlags flags)
		{
			IDictionary<SQLiteFunctionAttribute, SQLiteFunction> dictionary = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
			foreach (KeyValuePair<SQLiteFunctionAttribute, object> keyValuePair in SQLiteFunction._registeredFunctions)
			{
				SQLiteFunctionAttribute key = keyValuePair.Key;
				if (key != null)
				{
					SQLiteFunction sqliteFunction;
					if (SQLiteFunction.CreateFunction(key, out sqliteFunction))
					{
						SQLiteFunction.BindFunction(sqlbase, key, sqliteFunction, flags);
						dictionary[key] = sqliteFunction;
					}
					else
					{
						dictionary[key] = null;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00057344 File Offset: 0x00055544
		internal static bool UnbindAllFunctions(SQLiteBase sqlbase, SQLiteConnectionFlags flags, bool registered)
		{
			if (sqlbase == null)
			{
				return false;
			}
			IDictionary<SQLiteFunctionAttribute, SQLiteFunction> dictionary = sqlbase.Functions;
			if (dictionary == null)
			{
				return false;
			}
			bool flag = true;
			if (registered)
			{
				using (IEnumerator<KeyValuePair<SQLiteFunctionAttribute, object>> enumerator = SQLiteFunction._registeredFunctions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<SQLiteFunctionAttribute, object> keyValuePair = enumerator.Current;
						SQLiteFunctionAttribute key = keyValuePair.Key;
						SQLiteFunction sqliteFunction;
						if (key != null && (!dictionary.TryGetValue(key, out sqliteFunction) || sqliteFunction == null || !SQLiteFunction.UnbindFunction(sqlbase, key, sqliteFunction, flags)))
						{
							flag = false;
						}
					}
					return flag;
				}
			}
			dictionary = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>(dictionary);
			foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> keyValuePair2 in dictionary)
			{
				SQLiteFunctionAttribute key2 = keyValuePair2.Key;
				if (key2 != null)
				{
					SQLiteFunction value = keyValuePair2.Value;
					if (value != null && SQLiteFunction.UnbindFunction(sqlbase, key2, value, flags))
					{
						sqlbase.Functions.Remove(key2);
					}
					else
					{
						flag = false;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00057478 File Offset: 0x00055678
		internal static void BindFunction(SQLiteBase sqliteBase, SQLiteFunctionAttribute functionAttribute, SQLiteFunction function, SQLiteConnectionFlags flags)
		{
			if (sqliteBase == null)
			{
				throw new ArgumentNullException("sqliteBase");
			}
			if (functionAttribute == null)
			{
				throw new ArgumentNullException("functionAttribute");
			}
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			FunctionType funcType = functionAttribute.FuncType;
			function._base = sqliteBase;
			function._flags = flags;
			function._InvokeFunc = ((funcType == FunctionType.Scalar) ? new SQLiteCallback(function.ScalarCallback) : null);
			function._StepFunc = ((funcType == FunctionType.Aggregate) ? new SQLiteCallback(function.StepCallback) : null);
			function._FinalFunc = ((funcType == FunctionType.Aggregate) ? new SQLiteFinalCallback(function.FinalCallback) : null);
			function._CompareFunc = ((funcType == FunctionType.Collation) ? new SQLiteCollation(function.CompareCallback) : null);
			function._CompareFunc16 = ((funcType == FunctionType.Collation) ? new SQLiteCollation(function.CompareCallback16) : null);
			string name = functionAttribute.Name;
			if (funcType != FunctionType.Collation)
			{
				bool flag = function is SQLiteFunctionEx;
				sqliteBase.CreateFunction(name, functionAttribute.Arguments, flag, function._InvokeFunc, function._StepFunc, function._FinalFunc, true);
				return;
			}
			sqliteBase.CreateCollation(name, function._CompareFunc, function._CompareFunc16, true);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000575BC File Offset: 0x000557BC
		internal static bool UnbindFunction(SQLiteBase sqliteBase, SQLiteFunctionAttribute functionAttribute, SQLiteFunction function, SQLiteConnectionFlags flags)
		{
			if (sqliteBase == null)
			{
				throw new ArgumentNullException("sqliteBase");
			}
			if (functionAttribute == null)
			{
				throw new ArgumentNullException("functionAttribute");
			}
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			FunctionType funcType = functionAttribute.FuncType;
			string name = functionAttribute.Name;
			if (funcType != FunctionType.Collation)
			{
				bool flag = function is SQLiteFunctionEx;
				return sqliteBase.CreateFunction(name, functionAttribute.Arguments, flag, null, null, null, false) == SQLiteErrorCode.Ok;
			}
			return sqliteBase.CreateCollation(name, null, null, false) == SQLiteErrorCode.Ok;
		}

		// Token: 0x040007B7 RID: 1975
		internal SQLiteBase _base;

		// Token: 0x040007B8 RID: 1976
		private Dictionary<IntPtr, SQLiteFunction.AggregateData> _contextDataList;

		// Token: 0x040007B9 RID: 1977
		private SQLiteConnectionFlags _flags;

		// Token: 0x040007BA RID: 1978
		private SQLiteCallback _InvokeFunc;

		// Token: 0x040007BB RID: 1979
		private SQLiteCallback _StepFunc;

		// Token: 0x040007BC RID: 1980
		private SQLiteFinalCallback _FinalFunc;

		// Token: 0x040007BD RID: 1981
		private SQLiteCollation _CompareFunc;

		// Token: 0x040007BE RID: 1982
		private SQLiteCollation _CompareFunc16;

		// Token: 0x040007BF RID: 1983
		internal IntPtr _context;

		// Token: 0x040007C0 RID: 1984
		private static IDictionary<SQLiteFunctionAttribute, object> _registeredFunctions = new Dictionary<SQLiteFunctionAttribute, object>();

		// Token: 0x040007C1 RID: 1985
		private bool disposed;

		// Token: 0x02000299 RID: 665
		private class AggregateData
		{
			// Token: 0x04000B3E RID: 2878
			internal int _count = 1;

			// Token: 0x04000B3F RID: 2879
			internal object _data;
		}
	}
}
