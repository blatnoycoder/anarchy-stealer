using System;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
	// Token: 0x020001CC RID: 460
	public sealed class SQLiteIndex
	{
		// Token: 0x060014A6 RID: 5286 RVA: 0x0005F134 File Offset: 0x0005D334
		internal SQLiteIndex(int nConstraint, int nOrderBy)
		{
			this.inputs = new SQLiteIndexInputs(nConstraint, nOrderBy);
			this.outputs = new SQLiteIndexOutputs(nConstraint);
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0005F158 File Offset: 0x0005D358
		private static void SizeOfNative(out int sizeOfInfoType, out int sizeOfConstraintType, out int sizeOfOrderByType, out int sizeOfConstraintUsageType)
		{
			sizeOfInfoType = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_info));
			sizeOfConstraintType = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_constraint));
			sizeOfOrderByType = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_orderby));
			sizeOfConstraintUsageType = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_constraint_usage));
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0005F1B0 File Offset: 0x0005D3B0
		private static IntPtr AllocateAndInitializeNative(int nConstraint, int nOrderBy)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			IntPtr intPtr5 = IntPtr.Zero;
			try
			{
				int num;
				int num2;
				int num3;
				int num4;
				SQLiteIndex.SizeOfNative(out num, out num2, out num3, out num4);
				if (num > 0 && num2 > 0 && num3 > 0 && num4 > 0)
				{
					intPtr2 = SQLiteMemory.Allocate(num);
					intPtr3 = SQLiteMemory.Allocate(num2 * nConstraint);
					intPtr4 = SQLiteMemory.Allocate(num3 * nOrderBy);
					intPtr5 = SQLiteMemory.Allocate(num4 * nConstraint);
					if (intPtr2 != IntPtr.Zero && intPtr3 != IntPtr.Zero && intPtr4 != IntPtr.Zero && intPtr5 != IntPtr.Zero)
					{
						int num5 = 0;
						SQLiteMarshal.WriteInt32(intPtr2, num5, nConstraint);
						num5 = SQLiteMarshal.NextOffsetOf(num5, 4, IntPtr.Size);
						SQLiteMarshal.WriteIntPtr(intPtr2, num5, intPtr3);
						num5 = SQLiteMarshal.NextOffsetOf(num5, IntPtr.Size, 4);
						SQLiteMarshal.WriteInt32(intPtr2, num5, nOrderBy);
						num5 = SQLiteMarshal.NextOffsetOf(num5, 4, IntPtr.Size);
						SQLiteMarshal.WriteIntPtr(intPtr2, num5, intPtr4);
						num5 = SQLiteMarshal.NextOffsetOf(num5, IntPtr.Size, IntPtr.Size);
						SQLiteMarshal.WriteIntPtr(intPtr2, num5, intPtr5);
						intPtr = intPtr2;
					}
				}
			}
			finally
			{
				if (intPtr == IntPtr.Zero)
				{
					if (intPtr5 != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr5);
						intPtr5 = IntPtr.Zero;
					}
					if (intPtr4 != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr4);
						intPtr4 = IntPtr.Zero;
					}
					if (intPtr3 != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr3);
						intPtr3 = IntPtr.Zero;
					}
					if (intPtr2 != IntPtr.Zero)
					{
						SQLiteMemory.Free(intPtr2);
						intPtr2 = IntPtr.Zero;
					}
				}
			}
			return intPtr;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0005F38C File Offset: 0x0005D58C
		private static void FreeNative(IntPtr pIndex)
		{
			if (pIndex == IntPtr.Zero)
			{
				return;
			}
			int num = 0;
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			IntPtr intPtr = SQLiteMarshal.ReadIntPtr(pIndex, num);
			int num2 = num;
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			IntPtr intPtr2 = SQLiteMarshal.ReadIntPtr(pIndex, num);
			int num3 = num;
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, IntPtr.Size);
			IntPtr intPtr3 = SQLiteMarshal.ReadIntPtr(pIndex, num);
			int num4 = num;
			if (intPtr3 != IntPtr.Zero)
			{
				SQLiteMemory.Free(intPtr3);
				intPtr3 = IntPtr.Zero;
				SQLiteMarshal.WriteIntPtr(pIndex, num4, intPtr3);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				SQLiteMemory.Free(intPtr2);
				intPtr2 = IntPtr.Zero;
				SQLiteMarshal.WriteIntPtr(pIndex, num3, intPtr2);
			}
			if (intPtr != IntPtr.Zero)
			{
				SQLiteMemory.Free(intPtr);
				intPtr = IntPtr.Zero;
				SQLiteMarshal.WriteIntPtr(pIndex, num2, intPtr);
			}
			if (pIndex != IntPtr.Zero)
			{
				SQLiteMemory.Free(pIndex);
				pIndex = IntPtr.Zero;
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0005F498 File Offset: 0x0005D698
		internal static void FromIntPtr(IntPtr pIndex, bool includeOutput, ref SQLiteIndex index)
		{
			if (pIndex == IntPtr.Zero)
			{
				return;
			}
			int num = 0;
			int num2 = SQLiteMarshal.ReadInt32(pIndex, num);
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			IntPtr intPtr = SQLiteMarshal.ReadIntPtr(pIndex, num);
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
			int num3 = SQLiteMarshal.ReadInt32(pIndex, num);
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			IntPtr intPtr2 = SQLiteMarshal.ReadIntPtr(pIndex, num);
			IntPtr intPtr3 = IntPtr.Zero;
			if (includeOutput)
			{
				num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, IntPtr.Size);
				intPtr3 = SQLiteMarshal.ReadIntPtr(pIndex, num);
			}
			index = new SQLiteIndex(num2, num3);
			SQLiteIndexInputs sqliteIndexInputs = index.Inputs;
			if (sqliteIndexInputs == null)
			{
				return;
			}
			SQLiteIndexConstraint[] constraints = sqliteIndexInputs.Constraints;
			if (constraints == null)
			{
				return;
			}
			SQLiteIndexOrderBy[] orderBys = sqliteIndexInputs.OrderBys;
			if (orderBys == null)
			{
				return;
			}
			Type typeFromHandle = typeof(UnsafeNativeMethods.sqlite3_index_constraint);
			int num4 = Marshal.SizeOf(typeFromHandle);
			for (int i = 0; i < num2; i++)
			{
				IntPtr intPtr4 = SQLiteMarshal.IntPtrForOffset(intPtr, i * num4);
				UnsafeNativeMethods.sqlite3_index_constraint sqlite3_index_constraint = (UnsafeNativeMethods.sqlite3_index_constraint)Marshal.PtrToStructure(intPtr4, typeFromHandle);
				constraints[i] = new SQLiteIndexConstraint(sqlite3_index_constraint);
			}
			Type typeFromHandle2 = typeof(UnsafeNativeMethods.sqlite3_index_orderby);
			int num5 = Marshal.SizeOf(typeFromHandle2);
			for (int j = 0; j < num3; j++)
			{
				IntPtr intPtr5 = SQLiteMarshal.IntPtrForOffset(intPtr2, j * num5);
				UnsafeNativeMethods.sqlite3_index_orderby sqlite3_index_orderby = (UnsafeNativeMethods.sqlite3_index_orderby)Marshal.PtrToStructure(intPtr5, typeFromHandle2);
				orderBys[j] = new SQLiteIndexOrderBy(sqlite3_index_orderby);
			}
			if (includeOutput)
			{
				SQLiteIndexOutputs sqliteIndexOutputs = index.Outputs;
				if (sqliteIndexOutputs == null)
				{
					return;
				}
				SQLiteIndexConstraintUsage[] constraintUsages = sqliteIndexOutputs.ConstraintUsages;
				if (constraintUsages == null)
				{
					return;
				}
				Type typeFromHandle3 = typeof(UnsafeNativeMethods.sqlite3_index_constraint_usage);
				int num6 = Marshal.SizeOf(typeFromHandle3);
				for (int k = 0; k < num2; k++)
				{
					IntPtr intPtr6 = SQLiteMarshal.IntPtrForOffset(intPtr3, k * num6);
					UnsafeNativeMethods.sqlite3_index_constraint_usage sqlite3_index_constraint_usage = (UnsafeNativeMethods.sqlite3_index_constraint_usage)Marshal.PtrToStructure(intPtr6, typeFromHandle3);
					constraintUsages[k] = new SQLiteIndexConstraintUsage(sqlite3_index_constraint_usage);
				}
				num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
				sqliteIndexOutputs.IndexNumber = SQLiteMarshal.ReadInt32(pIndex, num);
				num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
				sqliteIndexOutputs.IndexString = SQLiteString.StringFromUtf8IntPtr(SQLiteMarshal.ReadIntPtr(pIndex, num));
				num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
				sqliteIndexOutputs.NeedToFreeIndexString = SQLiteMarshal.ReadInt32(pIndex, num);
				num = SQLiteMarshal.NextOffsetOf(num, 4, 4);
				sqliteIndexOutputs.OrderByConsumed = SQLiteMarshal.ReadInt32(pIndex, num);
				num = SQLiteMarshal.NextOffsetOf(num, 4, 8);
				sqliteIndexOutputs.EstimatedCost = new double?(SQLiteMarshal.ReadDouble(pIndex, num));
				num = SQLiteMarshal.NextOffsetOf(num, 8, 8);
				if (sqliteIndexOutputs.CanUseEstimatedRows())
				{
					sqliteIndexOutputs.EstimatedRows = new long?(SQLiteMarshal.ReadInt64(pIndex, num));
				}
				num = SQLiteMarshal.NextOffsetOf(num, 8, 4);
				if (sqliteIndexOutputs.CanUseIndexFlags())
				{
					sqliteIndexOutputs.IndexFlags = new SQLiteIndexFlags?((SQLiteIndexFlags)SQLiteMarshal.ReadInt32(pIndex, num));
				}
				num = SQLiteMarshal.NextOffsetOf(num, 4, 8);
				if (sqliteIndexOutputs.CanUseColumnsUsed())
				{
					sqliteIndexOutputs.ColumnsUsed = new long?(SQLiteMarshal.ReadInt64(pIndex, num));
				}
			}
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0005F794 File Offset: 0x0005D994
		internal static void ToIntPtr(SQLiteIndex index, IntPtr pIndex, bool includeInput)
		{
			if (index == null)
			{
				return;
			}
			SQLiteIndexOutputs sqliteIndexOutputs = index.Outputs;
			if (sqliteIndexOutputs == null)
			{
				return;
			}
			SQLiteIndexConstraintUsage[] constraintUsages = sqliteIndexOutputs.ConstraintUsages;
			if (constraintUsages == null)
			{
				return;
			}
			SQLiteIndexConstraint[] array = null;
			SQLiteIndexOrderBy[] array2 = null;
			if (includeInput)
			{
				SQLiteIndexInputs sqliteIndexInputs = index.Inputs;
				if (sqliteIndexInputs == null)
				{
					return;
				}
				array = sqliteIndexInputs.Constraints;
				if (array == null)
				{
					return;
				}
				array2 = sqliteIndexInputs.OrderBys;
				if (array2 == null)
				{
					return;
				}
			}
			if (pIndex == IntPtr.Zero)
			{
				return;
			}
			int num = 0;
			int num2 = SQLiteMarshal.ReadInt32(pIndex, num);
			if (includeInput && num2 != array.Length)
			{
				return;
			}
			if (num2 != constraintUsages.Length)
			{
				return;
			}
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			if (includeInput)
			{
				IntPtr intPtr = SQLiteMarshal.ReadIntPtr(pIndex, num);
				int num3 = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_constraint));
				for (int i = 0; i < num2; i++)
				{
					UnsafeNativeMethods.sqlite3_index_constraint sqlite3_index_constraint = new UnsafeNativeMethods.sqlite3_index_constraint(array[i]);
					Marshal.StructureToPtr(sqlite3_index_constraint, SQLiteMarshal.IntPtrForOffset(intPtr, i * num3), false);
				}
			}
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
			int num4 = (includeInput ? SQLiteMarshal.ReadInt32(pIndex, num) : 0);
			if (includeInput && num4 != array2.Length)
			{
				return;
			}
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			if (includeInput)
			{
				IntPtr intPtr2 = SQLiteMarshal.ReadIntPtr(pIndex, num);
				int num5 = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_orderby));
				for (int j = 0; j < num4; j++)
				{
					UnsafeNativeMethods.sqlite3_index_orderby sqlite3_index_orderby = new UnsafeNativeMethods.sqlite3_index_orderby(array2[j]);
					Marshal.StructureToPtr(sqlite3_index_orderby, SQLiteMarshal.IntPtrForOffset(intPtr2, j * num5), false);
				}
			}
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, IntPtr.Size);
			IntPtr intPtr3 = SQLiteMarshal.ReadIntPtr(pIndex, num);
			int num6 = Marshal.SizeOf(typeof(UnsafeNativeMethods.sqlite3_index_constraint_usage));
			for (int k = 0; k < num2; k++)
			{
				UnsafeNativeMethods.sqlite3_index_constraint_usage sqlite3_index_constraint_usage = new UnsafeNativeMethods.sqlite3_index_constraint_usage(constraintUsages[k]);
				Marshal.StructureToPtr(sqlite3_index_constraint_usage, SQLiteMarshal.IntPtrForOffset(intPtr3, k * num6), false);
			}
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
			SQLiteMarshal.WriteInt32(pIndex, num, sqliteIndexOutputs.IndexNumber);
			num = SQLiteMarshal.NextOffsetOf(num, 4, IntPtr.Size);
			SQLiteMarshal.WriteIntPtr(pIndex, num, SQLiteString.Utf8IntPtrFromString(sqliteIndexOutputs.IndexString, false));
			num = SQLiteMarshal.NextOffsetOf(num, IntPtr.Size, 4);
			int num7 = ((sqliteIndexOutputs.NeedToFreeIndexString != 0) ? sqliteIndexOutputs.NeedToFreeIndexString : 1);
			SQLiteMarshal.WriteInt32(pIndex, num, num7);
			num = SQLiteMarshal.NextOffsetOf(num, 4, 4);
			SQLiteMarshal.WriteInt32(pIndex, num, sqliteIndexOutputs.OrderByConsumed);
			num = SQLiteMarshal.NextOffsetOf(num, 4, 8);
			if (sqliteIndexOutputs.EstimatedCost != null)
			{
				SQLiteMarshal.WriteDouble(pIndex, num, sqliteIndexOutputs.EstimatedCost.GetValueOrDefault());
			}
			num = SQLiteMarshal.NextOffsetOf(num, 8, 8);
			if (sqliteIndexOutputs.CanUseEstimatedRows() && sqliteIndexOutputs.EstimatedRows != null)
			{
				SQLiteMarshal.WriteInt64(pIndex, num, sqliteIndexOutputs.EstimatedRows.GetValueOrDefault());
			}
			num = SQLiteMarshal.NextOffsetOf(num, 8, 4);
			if (sqliteIndexOutputs.CanUseIndexFlags() && sqliteIndexOutputs.IndexFlags != null)
			{
				SQLiteMarshal.WriteInt32(pIndex, num, (int)sqliteIndexOutputs.IndexFlags.GetValueOrDefault());
			}
			num = SQLiteMarshal.NextOffsetOf(num, 4, 8);
			if (sqliteIndexOutputs.CanUseColumnsUsed() && sqliteIndexOutputs.ColumnsUsed != null)
			{
				SQLiteMarshal.WriteInt64(pIndex, num, sqliteIndexOutputs.ColumnsUsed.GetValueOrDefault());
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x0005FB3C File Offset: 0x0005DD3C
		public SQLiteIndexInputs Inputs
		{
			get
			{
				return this.inputs;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0005FB44 File Offset: 0x0005DD44
		public SQLiteIndexOutputs Outputs
		{
			get
			{
				return this.outputs;
			}
		}

		// Token: 0x0400089F RID: 2207
		private SQLiteIndexInputs inputs;

		// Token: 0x040008A0 RID: 2208
		private SQLiteIndexOutputs outputs;
	}
}
