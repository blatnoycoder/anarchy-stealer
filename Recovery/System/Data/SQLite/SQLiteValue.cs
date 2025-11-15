using System;

namespace System.Data.SQLite
{
	// Token: 0x020001C4 RID: 452
	public sealed class SQLiteValue : ISQLiteNativeHandle
	{
		// Token: 0x06001477 RID: 5239 RVA: 0x0005EBA4 File Offset: 0x0005CDA4
		private SQLiteValue(IntPtr pValue)
		{
			this.pValue = pValue;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0005EBB4 File Offset: 0x0005CDB4
		private void PreventNativeAccess()
		{
			this.pValue = IntPtr.Zero;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0005EBC4 File Offset: 0x0005CDC4
		internal static SQLiteValue FromIntPtr(IntPtr pValue)
		{
			if (pValue == IntPtr.Zero)
			{
				return null;
			}
			return new SQLiteValue(pValue);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0005EBE0 File Offset: 0x0005CDE0
		internal static SQLiteValue[] ArrayFromSizeAndIntPtr(int argc, IntPtr argv)
		{
			if (argc < 0)
			{
				return null;
			}
			if (argv == IntPtr.Zero)
			{
				return null;
			}
			SQLiteValue[] array = new SQLiteValue[argc];
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				IntPtr intPtr = SQLiteMarshal.ReadIntPtr(argv, num);
				array[i] = ((intPtr != IntPtr.Zero) ? new SQLiteValue(intPtr) : null);
				i++;
				num += IntPtr.Size;
			}
			return array;
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0005EC58 File Offset: 0x0005CE58
		public IntPtr NativeHandle
		{
			get
			{
				return this.pValue;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0005EC60 File Offset: 0x0005CE60
		public bool Persisted
		{
			get
			{
				return this.persisted;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x0005EC68 File Offset: 0x0005CE68
		public object Value
		{
			get
			{
				if (!this.persisted)
				{
					throw new InvalidOperationException("value was not persisted");
				}
				return this.value;
			}
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0005EC88 File Offset: 0x0005CE88
		public TypeAffinity GetTypeAffinity()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return TypeAffinity.None;
			}
			return UnsafeNativeMethods.sqlite3_value_type(this.pValue);
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0005ECB0 File Offset: 0x0005CEB0
		public int GetBytes()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return 0;
			}
			return UnsafeNativeMethods.sqlite3_value_bytes(this.pValue);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0005ECD4 File Offset: 0x0005CED4
		public int GetInt()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return 0;
			}
			return UnsafeNativeMethods.sqlite3_value_int(this.pValue);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0005ECF8 File Offset: 0x0005CEF8
		public long GetInt64()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return 0L;
			}
			return UnsafeNativeMethods.sqlite3_value_int64(this.pValue);
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0005ED20 File Offset: 0x0005CF20
		public double GetDouble()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return 0.0;
			}
			return UnsafeNativeMethods.sqlite3_value_double(this.pValue);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0005ED4C File Offset: 0x0005CF4C
		public string GetString()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return null;
			}
			int num = 0;
			IntPtr intPtr = UnsafeNativeMethods.sqlite3_value_text_interop(this.pValue, ref num);
			return SQLiteString.StringFromUtf8IntPtr(intPtr, num);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0005ED8C File Offset: 0x0005CF8C
		public byte[] GetBlob()
		{
			if (this.pValue == IntPtr.Zero)
			{
				return null;
			}
			return SQLiteBytes.FromIntPtr(UnsafeNativeMethods.sqlite3_value_blob(this.pValue), this.GetBytes());
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0005EDBC File Offset: 0x0005CFBC
		public object GetObject()
		{
			switch (this.GetTypeAffinity())
			{
			case TypeAffinity.Uninitialized:
				return null;
			case TypeAffinity.Int64:
				return this.GetInt64();
			case TypeAffinity.Double:
				return this.GetDouble();
			case TypeAffinity.Text:
				return this.GetString();
			case TypeAffinity.Blob:
				return this.GetBytes();
			case TypeAffinity.Null:
				return DBNull.Value;
			default:
				return null;
			}
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0005EE2C File Offset: 0x0005D02C
		public bool Persist()
		{
			switch (this.GetTypeAffinity())
			{
			case TypeAffinity.Uninitialized:
				this.value = null;
				this.PreventNativeAccess();
				return this.persisted = true;
			case TypeAffinity.Int64:
				this.value = this.GetInt64();
				this.PreventNativeAccess();
				return this.persisted = true;
			case TypeAffinity.Double:
				this.value = this.GetDouble();
				this.PreventNativeAccess();
				return this.persisted = true;
			case TypeAffinity.Text:
				this.value = this.GetString();
				this.PreventNativeAccess();
				return this.persisted = true;
			case TypeAffinity.Blob:
				this.value = this.GetBytes();
				this.PreventNativeAccess();
				return this.persisted = true;
			case TypeAffinity.Null:
				this.value = DBNull.Value;
				this.PreventNativeAccess();
				return this.persisted = true;
			default:
				return false;
			}
		}

		// Token: 0x04000877 RID: 2167
		private IntPtr pValue;

		// Token: 0x04000878 RID: 2168
		private bool persisted;

		// Token: 0x04000879 RID: 2169
		private object value;
	}
}
