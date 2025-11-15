using System;

namespace System.Data.SQLite
{
	// Token: 0x02000199 RID: 409
	public enum SQLiteErrorCode
	{
		// Token: 0x04000744 RID: 1860
		Unknown = -1,
		// Token: 0x04000745 RID: 1861
		Ok,
		// Token: 0x04000746 RID: 1862
		Error,
		// Token: 0x04000747 RID: 1863
		Internal,
		// Token: 0x04000748 RID: 1864
		Perm,
		// Token: 0x04000749 RID: 1865
		Abort,
		// Token: 0x0400074A RID: 1866
		Busy,
		// Token: 0x0400074B RID: 1867
		Locked,
		// Token: 0x0400074C RID: 1868
		NoMem,
		// Token: 0x0400074D RID: 1869
		ReadOnly,
		// Token: 0x0400074E RID: 1870
		Interrupt,
		// Token: 0x0400074F RID: 1871
		IoErr,
		// Token: 0x04000750 RID: 1872
		Corrupt,
		// Token: 0x04000751 RID: 1873
		NotFound,
		// Token: 0x04000752 RID: 1874
		Full,
		// Token: 0x04000753 RID: 1875
		CantOpen,
		// Token: 0x04000754 RID: 1876
		Protocol,
		// Token: 0x04000755 RID: 1877
		Empty,
		// Token: 0x04000756 RID: 1878
		Schema,
		// Token: 0x04000757 RID: 1879
		TooBig,
		// Token: 0x04000758 RID: 1880
		Constraint,
		// Token: 0x04000759 RID: 1881
		Mismatch,
		// Token: 0x0400075A RID: 1882
		Misuse,
		// Token: 0x0400075B RID: 1883
		NoLfs,
		// Token: 0x0400075C RID: 1884
		Auth,
		// Token: 0x0400075D RID: 1885
		Format,
		// Token: 0x0400075E RID: 1886
		Range,
		// Token: 0x0400075F RID: 1887
		NotADb,
		// Token: 0x04000760 RID: 1888
		Notice,
		// Token: 0x04000761 RID: 1889
		Warning,
		// Token: 0x04000762 RID: 1890
		Row = 100,
		// Token: 0x04000763 RID: 1891
		Done,
		// Token: 0x04000764 RID: 1892
		NonExtendedMask = 255,
		// Token: 0x04000765 RID: 1893
		Error_Missing_CollSeq = 257,
		// Token: 0x04000766 RID: 1894
		Error_Retry = 513,
		// Token: 0x04000767 RID: 1895
		Error_Snapshot = 769,
		// Token: 0x04000768 RID: 1896
		IoErr_Read = 266,
		// Token: 0x04000769 RID: 1897
		IoErr_Short_Read = 522,
		// Token: 0x0400076A RID: 1898
		IoErr_Write = 778,
		// Token: 0x0400076B RID: 1899
		IoErr_Fsync = 1034,
		// Token: 0x0400076C RID: 1900
		IoErr_Dir_Fsync = 1290,
		// Token: 0x0400076D RID: 1901
		IoErr_Truncate = 1546,
		// Token: 0x0400076E RID: 1902
		IoErr_Fstat = 1802,
		// Token: 0x0400076F RID: 1903
		IoErr_Unlock = 2058,
		// Token: 0x04000770 RID: 1904
		IoErr_RdLock = 2314,
		// Token: 0x04000771 RID: 1905
		IoErr_Delete = 2570,
		// Token: 0x04000772 RID: 1906
		IoErr_Blocked = 2826,
		// Token: 0x04000773 RID: 1907
		IoErr_NoMem = 3082,
		// Token: 0x04000774 RID: 1908
		IoErr_Access = 3338,
		// Token: 0x04000775 RID: 1909
		IoErr_CheckReservedLock = 3594,
		// Token: 0x04000776 RID: 1910
		IoErr_Lock = 3850,
		// Token: 0x04000777 RID: 1911
		IoErr_Close = 4106,
		// Token: 0x04000778 RID: 1912
		IoErr_Dir_Close = 4362,
		// Token: 0x04000779 RID: 1913
		IoErr_ShmOpen = 4618,
		// Token: 0x0400077A RID: 1914
		IoErr_ShmSize = 4874,
		// Token: 0x0400077B RID: 1915
		IoErr_ShmLock = 5130,
		// Token: 0x0400077C RID: 1916
		IoErr_ShmMap = 5386,
		// Token: 0x0400077D RID: 1917
		IoErr_Seek = 5642,
		// Token: 0x0400077E RID: 1918
		IoErr_Delete_NoEnt = 5898,
		// Token: 0x0400077F RID: 1919
		IoErr_Mmap = 6154,
		// Token: 0x04000780 RID: 1920
		IoErr_GetTempPath = 6410,
		// Token: 0x04000781 RID: 1921
		IoErr_ConvPath = 6666,
		// Token: 0x04000782 RID: 1922
		IoErr_VNode = 6922,
		// Token: 0x04000783 RID: 1923
		IoErr_Auth = 7178,
		// Token: 0x04000784 RID: 1924
		IoErr_Begin_Atomic = 7434,
		// Token: 0x04000785 RID: 1925
		IoErr_Commit_Atomic = 7690,
		// Token: 0x04000786 RID: 1926
		IoErr_Rollback_Atomic = 7946,
		// Token: 0x04000787 RID: 1927
		IoErr_Data = 8202,
		// Token: 0x04000788 RID: 1928
		IoErr_CorruptFs = 8458,
		// Token: 0x04000789 RID: 1929
		Locked_SharedCache = 262,
		// Token: 0x0400078A RID: 1930
		Locked_Vtab = 518,
		// Token: 0x0400078B RID: 1931
		Busy_Recovery = 261,
		// Token: 0x0400078C RID: 1932
		Busy_Snapshot = 517,
		// Token: 0x0400078D RID: 1933
		Busy_Timeout = 773,
		// Token: 0x0400078E RID: 1934
		CantOpen_NoTempDir = 270,
		// Token: 0x0400078F RID: 1935
		CantOpen_IsDir = 526,
		// Token: 0x04000790 RID: 1936
		CantOpen_FullPath = 782,
		// Token: 0x04000791 RID: 1937
		CantOpen_ConvPath = 1038,
		// Token: 0x04000792 RID: 1938
		CantOpen_DirtyWal = 1294,
		// Token: 0x04000793 RID: 1939
		CantOpen_SymLink = 1550,
		// Token: 0x04000794 RID: 1940
		Corrupt_Vtab = 267,
		// Token: 0x04000795 RID: 1941
		Corrupt_Sequence = 523,
		// Token: 0x04000796 RID: 1942
		Corrupt_Index = 779,
		// Token: 0x04000797 RID: 1943
		ReadOnly_Recovery = 264,
		// Token: 0x04000798 RID: 1944
		ReadOnly_CantLock = 520,
		// Token: 0x04000799 RID: 1945
		ReadOnly_Rollback = 776,
		// Token: 0x0400079A RID: 1946
		ReadOnly_DbMoved = 1032,
		// Token: 0x0400079B RID: 1947
		ReadOnly_CantInit = 1288,
		// Token: 0x0400079C RID: 1948
		ReadOnly_Directory = 1544,
		// Token: 0x0400079D RID: 1949
		Abort_Rollback = 516,
		// Token: 0x0400079E RID: 1950
		Constraint_Check = 275,
		// Token: 0x0400079F RID: 1951
		Constraint_CommitHook = 531,
		// Token: 0x040007A0 RID: 1952
		Constraint_ForeignKey = 787,
		// Token: 0x040007A1 RID: 1953
		Constraint_Function = 1043,
		// Token: 0x040007A2 RID: 1954
		Constraint_NotNull = 1299,
		// Token: 0x040007A3 RID: 1955
		Constraint_PrimaryKey = 1555,
		// Token: 0x040007A4 RID: 1956
		Constraint_Trigger = 1811,
		// Token: 0x040007A5 RID: 1957
		Constraint_Unique = 2067,
		// Token: 0x040007A6 RID: 1958
		Constraint_Vtab = 2323,
		// Token: 0x040007A7 RID: 1959
		Constraint_RowId = 2579,
		// Token: 0x040007A8 RID: 1960
		Constraint_Pinned = 2835,
		// Token: 0x040007A9 RID: 1961
		Constraint_DataType = 3091,
		// Token: 0x040007AA RID: 1962
		Misuse_No_License = 277,
		// Token: 0x040007AB RID: 1963
		Notice_Recover_Wal = 283,
		// Token: 0x040007AC RID: 1964
		Notice_Recover_Rollback = 539,
		// Token: 0x040007AD RID: 1965
		Warning_AutoIndex = 284,
		// Token: 0x040007AE RID: 1966
		Auth_User = 279,
		// Token: 0x040007AF RID: 1967
		Ok_Load_Permanently = 256,
		// Token: 0x040007B0 RID: 1968
		Ok_SymLink = 512
	}
}
