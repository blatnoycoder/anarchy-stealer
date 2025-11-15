using System;

namespace System.Data.SQLite
{
	// Token: 0x02000152 RID: 338
	[Flags]
	public enum SQLiteConnectionFlags : long
	{
		// Token: 0x04000568 RID: 1384
		None = 0L,
		// Token: 0x04000569 RID: 1385
		LogPrepare = 1L,
		// Token: 0x0400056A RID: 1386
		LogPreBind = 2L,
		// Token: 0x0400056B RID: 1387
		LogBind = 4L,
		// Token: 0x0400056C RID: 1388
		LogCallbackException = 8L,
		// Token: 0x0400056D RID: 1389
		LogBackup = 16L,
		// Token: 0x0400056E RID: 1390
		NoExtensionFunctions = 32L,
		// Token: 0x0400056F RID: 1391
		BindUInt32AsInt64 = 64L,
		// Token: 0x04000570 RID: 1392
		BindAllAsText = 128L,
		// Token: 0x04000571 RID: 1393
		GetAllAsText = 256L,
		// Token: 0x04000572 RID: 1394
		NoLoadExtension = 512L,
		// Token: 0x04000573 RID: 1395
		NoCreateModule = 1024L,
		// Token: 0x04000574 RID: 1396
		NoBindFunctions = 2048L,
		// Token: 0x04000575 RID: 1397
		NoLogModule = 4096L,
		// Token: 0x04000576 RID: 1398
		LogModuleError = 8192L,
		// Token: 0x04000577 RID: 1399
		LogModuleException = 16384L,
		// Token: 0x04000578 RID: 1400
		TraceWarning = 32768L,
		// Token: 0x04000579 RID: 1401
		ConvertInvariantText = 65536L,
		// Token: 0x0400057A RID: 1402
		BindInvariantText = 131072L,
		// Token: 0x0400057B RID: 1403
		NoConnectionPool = 262144L,
		// Token: 0x0400057C RID: 1404
		UseConnectionPool = 524288L,
		// Token: 0x0400057D RID: 1405
		UseConnectionTypes = 1048576L,
		// Token: 0x0400057E RID: 1406
		NoGlobalTypes = 2097152L,
		// Token: 0x0400057F RID: 1407
		StickyHasRows = 4194304L,
		// Token: 0x04000580 RID: 1408
		StrictEnlistment = 8388608L,
		// Token: 0x04000581 RID: 1409
		MapIsolationLevels = 16777216L,
		// Token: 0x04000582 RID: 1410
		DetectTextAffinity = 33554432L,
		// Token: 0x04000583 RID: 1411
		DetectStringType = 67108864L,
		// Token: 0x04000584 RID: 1412
		NoConvertSettings = 134217728L,
		// Token: 0x04000585 RID: 1413
		BindDateTimeWithKind = 268435456L,
		// Token: 0x04000586 RID: 1414
		RollbackOnException = 536870912L,
		// Token: 0x04000587 RID: 1415
		DenyOnException = 1073741824L,
		// Token: 0x04000588 RID: 1416
		InterruptOnException = 2147483648L,
		// Token: 0x04000589 RID: 1417
		UnbindFunctionsOnClose = 4294967296L,
		// Token: 0x0400058A RID: 1418
		NoVerifyTextAffinity = 8589934592L,
		// Token: 0x0400058B RID: 1419
		UseConnectionBindValueCallbacks = 17179869184L,
		// Token: 0x0400058C RID: 1420
		UseConnectionReadValueCallbacks = 34359738368L,
		// Token: 0x0400058D RID: 1421
		UseParameterNameForTypeName = 68719476736L,
		// Token: 0x0400058E RID: 1422
		UseParameterDbTypeForTypeName = 137438953472L,
		// Token: 0x0400058F RID: 1423
		NoVerifyTypeAffinity = 274877906944L,
		// Token: 0x04000590 RID: 1424
		AllowNestedTransactions = 549755813888L,
		// Token: 0x04000591 RID: 1425
		BindDecimalAsText = 1099511627776L,
		// Token: 0x04000592 RID: 1426
		GetDecimalAsText = 2199023255552L,
		// Token: 0x04000593 RID: 1427
		BindInvariantDecimal = 4398046511104L,
		// Token: 0x04000594 RID: 1428
		GetInvariantDecimal = 8796093022208L,
		// Token: 0x04000595 RID: 1429
		WaitForEnlistmentReset = 17592186044416L,
		// Token: 0x04000596 RID: 1430
		GetInvariantInt64 = 35184372088832L,
		// Token: 0x04000597 RID: 1431
		GetInvariantDouble = 70368744177664L,
		// Token: 0x04000598 RID: 1432
		StrictConformance = 140737488355328L,
		// Token: 0x04000599 RID: 1433
		HidePassword = 281474976710656L,
		// Token: 0x0400059A RID: 1434
		NoCoreFunctions = 562949953421312L,
		// Token: 0x0400059B RID: 1435
		StopOnException = 1125899906842624L,
		// Token: 0x0400059C RID: 1436
		BindAndGetAllAsText = 384L,
		// Token: 0x0400059D RID: 1437
		ConvertAndBindInvariantText = 196608L,
		// Token: 0x0400059E RID: 1438
		BindAndGetAllAsInvariantText = 131456L,
		// Token: 0x0400059F RID: 1439
		ConvertAndBindAndGetAllAsInvariantText = 196992L,
		// Token: 0x040005A0 RID: 1440
		UseConnectionAllValueCallbacks = 51539607552L,
		// Token: 0x040005A1 RID: 1441
		UseParameterAnythingForTypeName = 206158430208L,
		// Token: 0x040005A2 RID: 1442
		LogAll = 24607L,
		// Token: 0x040005A3 RID: 1443
		LogDefault = 16392L,
		// Token: 0x040005A4 RID: 1444
		Default = 13194139549704L,
		// Token: 0x040005A5 RID: 1445
		DefaultAndLogAll = 13194139557919L
	}
}
