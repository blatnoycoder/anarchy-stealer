using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data.SQLite
{
	// Token: 0x02000198 RID: 408
	[Serializable]
	public sealed class SQLiteException : DbException, ISerializable
	{
		// Token: 0x060011F1 RID: 4593 RVA: 0x00055FB0 File Offset: 0x000541B0
		private SQLiteException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._errorCode = (SQLiteErrorCode)info.GetInt32("errorCode");
			this.Initialize();
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00055FD4 File Offset: 0x000541D4
		public SQLiteException(SQLiteErrorCode errorCode, string message)
			: base(SQLiteException.GetStockErrorMessage(errorCode, message))
		{
			this._errorCode = errorCode;
			this.Initialize();
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00055FF0 File Offset: 0x000541F0
		public SQLiteException(string message)
			: this(SQLiteErrorCode.Unknown, message)
		{
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00055FFC File Offset: 0x000541FC
		public SQLiteException()
		{
			this.Initialize();
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0005600C File Offset: 0x0005420C
		public SQLiteException(string message, Exception innerException)
			: base(message, innerException)
		{
			this.Initialize();
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0005601C File Offset: 0x0005421C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				info.AddValue("errorCode", this._errorCode);
			}
			base.GetObjectData(info, context);
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00056044 File Offset: 0x00054244
		public SQLiteErrorCode ResultCode
		{
			get
			{
				return this._errorCode;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0005604C File Offset: 0x0005424C
		public override int ErrorCode
		{
			get
			{
				return (int)this._errorCode;
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00056054 File Offset: 0x00054254
		private void Initialize()
		{
			if (base.HResult == -2147467259)
			{
				int? hresultForErrorCode = SQLiteException.GetHResultForErrorCode(this.ResultCode);
				if (hresultForErrorCode != null)
				{
					base.HResult = hresultForErrorCode.Value;
				}
			}
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0005609C File Offset: 0x0005429C
		private static int MakeHResult(int errorCode, bool success)
		{
			return (errorCode & 65535) | 1967 | (success ? 0 : int.MinValue);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000560C0 File Offset: 0x000542C0
		private static int? GetHResultForErrorCode(SQLiteErrorCode errorCode)
		{
			SQLiteErrorCode sqliteErrorCode = errorCode & SQLiteErrorCode.NonExtendedMask;
			switch (sqliteErrorCode)
			{
			case SQLiteErrorCode.Ok:
				return new int?(0);
			case SQLiteErrorCode.Error:
				return new int?(SQLiteException.MakeHResult(31, false));
			case SQLiteErrorCode.Internal:
				return new int?(-2147418113);
			case SQLiteErrorCode.Perm:
				return new int?(SQLiteException.MakeHResult(5, false));
			case SQLiteErrorCode.Abort:
				return new int?(-2147467260);
			case SQLiteErrorCode.Busy:
				return new int?(SQLiteException.MakeHResult(170, false));
			case SQLiteErrorCode.Locked:
				return new int?(SQLiteException.MakeHResult(212, false));
			case SQLiteErrorCode.NoMem:
				return new int?(SQLiteException.MakeHResult(14, false));
			case SQLiteErrorCode.ReadOnly:
				return new int?(SQLiteException.MakeHResult(6009, false));
			case SQLiteErrorCode.Interrupt:
				return new int?(SQLiteException.MakeHResult(1223, false));
			case SQLiteErrorCode.IoErr:
				return new int?(SQLiteException.MakeHResult(1117, false));
			case SQLiteErrorCode.Corrupt:
				return new int?(SQLiteException.MakeHResult(1358, false));
			case SQLiteErrorCode.NotFound:
				return new int?(SQLiteException.MakeHResult(50, false));
			case SQLiteErrorCode.Full:
				return new int?(SQLiteException.MakeHResult(112, false));
			case SQLiteErrorCode.CantOpen:
				return new int?(SQLiteException.MakeHResult(1011, false));
			case SQLiteErrorCode.Protocol:
				return new int?(SQLiteException.MakeHResult(1460, false));
			case SQLiteErrorCode.Empty:
				return new int?(SQLiteException.MakeHResult(4306, false));
			case SQLiteErrorCode.Schema:
				return new int?(SQLiteException.MakeHResult(1931, false));
			case SQLiteErrorCode.TooBig:
				return new int?(-2147317563);
			case SQLiteErrorCode.Constraint:
				return new int?(SQLiteException.MakeHResult(8239, false));
			case SQLiteErrorCode.Mismatch:
				return new int?(SQLiteException.MakeHResult(1629, false));
			case SQLiteErrorCode.Misuse:
				return new int?(SQLiteException.MakeHResult(1609, false));
			case SQLiteErrorCode.NoLfs:
				return new int?(SQLiteException.MakeHResult(1606, false));
			case SQLiteErrorCode.Auth:
				return new int?(SQLiteException.MakeHResult(1935, false));
			case SQLiteErrorCode.Format:
				return new int?(SQLiteException.MakeHResult(11, false));
			case SQLiteErrorCode.Range:
				return new int?(-2147316575);
			case SQLiteErrorCode.NotADb:
				return new int?(SQLiteException.MakeHResult(1392, false));
			case SQLiteErrorCode.Notice:
			case SQLiteErrorCode.Warning:
				break;
			default:
				switch (sqliteErrorCode)
				{
				case SQLiteErrorCode.Row:
				case SQLiteErrorCode.Done:
					break;
				default:
					return null;
				}
				break;
			}
			return new int?(SQLiteException.MakeHResult((int)errorCode, true));
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00056318 File Offset: 0x00054518
		private static string GetErrorString(SQLiteErrorCode errorCode)
		{
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
			return typeof(SQLite3).InvokeMember("GetErrorString", bindingFlags, null, null, new object[] { errorCode }) as string;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0005635C File Offset: 0x0005455C
		private static string GetStockErrorMessage(SQLiteErrorCode errorCode, string message)
		{
			return HelperMethods.StringFormat(CultureInfo.CurrentCulture, "{0}{1}{2}", new object[]
			{
				SQLiteException.GetErrorString(errorCode),
				Environment.NewLine,
				message
			}).Trim();
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000563A0 File Offset: 0x000545A0
		public override string ToString()
		{
			return HelperMethods.StringFormat(CultureInfo.CurrentCulture, "code = {0} ({1}), message = {2}", new object[]
			{
				this._errorCode,
				(int)this._errorCode,
				base.ToString()
			});
		}

		// Token: 0x04000741 RID: 1857
		private const int FACILITY_SQLITE = 1967;

		// Token: 0x04000742 RID: 1858
		private SQLiteErrorCode _errorCode;
	}
}
