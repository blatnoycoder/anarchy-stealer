using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.SQLite
{
	// Token: 0x02000186 RID: 390
	[DefaultMember("Item")]
	[DefaultProperty("DataSource")]
	public sealed class SQLiteConnectionStringBuilder : DbConnectionStringBuilder
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x00051FB0 File Offset: 0x000501B0
		public SQLiteConnectionStringBuilder()
		{
			this.Initialize(null);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00051FC0 File Offset: 0x000501C0
		public SQLiteConnectionStringBuilder(string connectionString)
		{
			this.Initialize(connectionString);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00051FD0 File Offset: 0x000501D0
		private void Initialize(string cnnString)
		{
			this._properties = new Hashtable(StringComparer.OrdinalIgnoreCase);
			try
			{
				base.GetProperties(this._properties);
			}
			catch (NotImplementedException)
			{
				this.FallbackGetProperties(this._properties);
			}
			if (!string.IsNullOrEmpty(cnnString))
			{
				base.ConnectionString = cnnString;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00052034 File Offset: 0x00050234
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x00052060 File Offset: 0x00050260
		[DefaultValue(3)]
		[Browsable(true)]
		public int Version
		{
			get
			{
				object obj;
				this.TryGetValue("version", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				if (value != 3)
				{
					throw new NotSupportedException();
				}
				this["version"] = value;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x00052080 File Offset: 0x00050280
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x000520CC File Offset: 0x000502CC
		[DefaultValue(SynchronizationModes.Normal)]
		[Browsable(true)]
		[DisplayName("Synchronous")]
		public SynchronizationModes SyncMode
		{
			get
			{
				object obj;
				this.TryGetValue("synchronous", out obj);
				if (obj is string)
				{
					return (SynchronizationModes)TypeDescriptor.GetConverter(typeof(SynchronizationModes)).ConvertFrom(obj);
				}
				return (SynchronizationModes)obj;
			}
			set
			{
				this["synchronous"] = value;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x000520E0 File Offset: 0x000502E0
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x00052108 File Offset: 0x00050308
		[DefaultValue(false)]
		[Browsable(true)]
		[DisplayName("Use UTF-16 Encoding")]
		public bool UseUTF16Encoding
		{
			get
			{
				object obj;
				this.TryGetValue("useutf16encoding", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["useutf16encoding"] = value;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0005211C File Offset: 0x0005031C
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x00052144 File Offset: 0x00050344
		[DefaultValue(false)]
		[Browsable(true)]
		public bool Pooling
		{
			get
			{
				object obj;
				this.TryGetValue("pooling", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["pooling"] = value;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00052158 File Offset: 0x00050358
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x00052180 File Offset: 0x00050380
		[DisplayName("Binary GUID")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool BinaryGUID
		{
			get
			{
				object obj;
				this.TryGetValue("binaryguid", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["binaryguid"] = value;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00052194 File Offset: 0x00050394
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x000521C4 File Offset: 0x000503C4
		[DisplayName("Data Source")]
		[Browsable(true)]
		[DefaultValue("")]
		public string DataSource
		{
			get
			{
				object obj;
				this.TryGetValue("data source", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["data source"] = value;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000521D4 File Offset: 0x000503D4
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x00052204 File Offset: 0x00050404
		[DisplayName("URI")]
		[Browsable(true)]
		[DefaultValue(null)]
		public string Uri
		{
			get
			{
				object obj;
				this.TryGetValue("uri", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["uri"] = value;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00052214 File Offset: 0x00050414
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x00052244 File Offset: 0x00050444
		[Browsable(true)]
		[DefaultValue(null)]
		[DisplayName("Full URI")]
		public string FullUri
		{
			get
			{
				object obj;
				this.TryGetValue("fulluri", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["fulluri"] = value;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00052254 File Offset: 0x00050454
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x00052280 File Offset: 0x00050480
		[DisplayName("Default Timeout")]
		[DefaultValue(30)]
		[Browsable(true)]
		public int DefaultTimeout
		{
			get
			{
				object obj;
				this.TryGetValue("default timeout", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["default timeout"] = value;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00052294 File Offset: 0x00050494
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x000522C0 File Offset: 0x000504C0
		[DefaultValue(0)]
		[DisplayName("Busy Timeout")]
		[Browsable(true)]
		public int BusyTimeout
		{
			get
			{
				object obj;
				this.TryGetValue("busytimeout", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["busytimeout"] = value;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x000522D4 File Offset: 0x000504D4
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x00052300 File Offset: 0x00050500
		[DefaultValue(30000)]
		[Browsable(true)]
		[DisplayName("Wait Timeout")]
		public int WaitTimeout
		{
			get
			{
				object obj;
				this.TryGetValue("waittimeout", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["waittimeout"] = value;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x00052314 File Offset: 0x00050514
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x00052340 File Offset: 0x00050540
		[DisplayName("Prepare Retries")]
		[DefaultValue(3)]
		[Browsable(true)]
		public int PrepareRetries
		{
			get
			{
				object obj;
				this.TryGetValue("prepareretries", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["prepareretries"] = value;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x00052354 File Offset: 0x00050554
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x00052380 File Offset: 0x00050580
		[DefaultValue(0)]
		[DisplayName("Progress Ops")]
		[Browsable(true)]
		public int ProgressOps
		{
			get
			{
				object obj;
				this.TryGetValue("progressops", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["progressops"] = value;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x00052394 File Offset: 0x00050594
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x000523BC File Offset: 0x000505BC
		[Browsable(true)]
		[DefaultValue(true)]
		public bool Enlist
		{
			get
			{
				object obj;
				this.TryGetValue("enlist", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["enlist"] = value;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x000523D0 File Offset: 0x000505D0
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x000523F8 File Offset: 0x000505F8
		[Browsable(true)]
		[DefaultValue(false)]
		[DisplayName("Fail If Missing")]
		public bool FailIfMissing
		{
			get
			{
				object obj;
				this.TryGetValue("failifmissing", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["failifmissing"] = value;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x0005240C File Offset: 0x0005060C
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x00052434 File Offset: 0x00050634
		[DefaultValue(false)]
		[DisplayName("Legacy Format")]
		[Browsable(true)]
		public bool LegacyFormat
		{
			get
			{
				object obj;
				this.TryGetValue("legacy format", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["legacy format"] = value;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00052448 File Offset: 0x00050648
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x00052470 File Offset: 0x00050670
		[Browsable(true)]
		[DefaultValue(false)]
		[DisplayName("Read Only")]
		public bool ReadOnly
		{
			get
			{
				object obj;
				this.TryGetValue("read only", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["read only"] = value;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00052484 File Offset: 0x00050684
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x000524B4 File Offset: 0x000506B4
		[PasswordPropertyText(true)]
		[Browsable(true)]
		[DefaultValue("")]
		public string Password
		{
			get
			{
				object obj;
				this.TryGetValue("password", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["password"] = value;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x000524C4 File Offset: 0x000506C4
		// (set) Token: 0x06001161 RID: 4449 RVA: 0x0005250C File Offset: 0x0005070C
		[DefaultValue(null)]
		[Browsable(true)]
		[DisplayName("Hexadecimal Password")]
		[PasswordPropertyText(true)]
		public byte[] HexPassword
		{
			get
			{
				object obj;
				if (this.TryGetValue("hexpassword", out obj))
				{
					if (obj is string)
					{
						return SQLiteConnection.FromHexString((string)obj);
					}
					if (obj != null)
					{
						return (byte[])obj;
					}
				}
				return null;
			}
			set
			{
				this["hexpassword"] = SQLiteConnection.ToHexString(value);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x00052520 File Offset: 0x00050720
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x00052554 File Offset: 0x00050754
		[PasswordPropertyText(true)]
		[Browsable(true)]
		[DisplayName("Textual Password")]
		[DefaultValue(null)]
		public string TextPassword
		{
			get
			{
				object obj;
				if (!this.TryGetValue("textpassword", out obj))
				{
					return null;
				}
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["textpassword"] = value;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x00052564 File Offset: 0x00050764
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x00052590 File Offset: 0x00050790
		[DefaultValue(4096)]
		[DisplayName("Page Size")]
		[Browsable(true)]
		public int PageSize
		{
			get
			{
				object obj;
				this.TryGetValue("page size", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["page size"] = value;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x000525A4 File Offset: 0x000507A4
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x000525D0 File Offset: 0x000507D0
		[DefaultValue(0)]
		[Browsable(true)]
		[DisplayName("Maximum Page Count")]
		public int MaxPageCount
		{
			get
			{
				object obj;
				this.TryGetValue("max page count", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["max page count"] = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x000525E4 File Offset: 0x000507E4
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x00052610 File Offset: 0x00050810
		[DefaultValue(-2000)]
		[DisplayName("Cache Size")]
		[Browsable(true)]
		public int CacheSize
		{
			get
			{
				object obj;
				this.TryGetValue("cache size", out obj);
				return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
			}
			set
			{
				this["cache size"] = value;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x00052624 File Offset: 0x00050824
		// (set) Token: 0x0600116B RID: 4459 RVA: 0x0005267C File Offset: 0x0005087C
		[DefaultValue(SQLiteDateFormats.ISO8601)]
		[DisplayName("DateTime Format")]
		[Browsable(true)]
		public SQLiteDateFormats DateTimeFormat
		{
			get
			{
				object obj;
				if (this.TryGetValue("datetimeformat", out obj))
				{
					if (obj is SQLiteDateFormats)
					{
						return (SQLiteDateFormats)obj;
					}
					if (obj != null)
					{
						return (SQLiteDateFormats)TypeDescriptor.GetConverter(typeof(SQLiteDateFormats)).ConvertFrom(obj);
					}
				}
				return SQLiteDateFormats.ISO8601;
			}
			set
			{
				this["datetimeformat"] = value;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x00052690 File Offset: 0x00050890
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x000526E8 File Offset: 0x000508E8
		[Browsable(true)]
		[DefaultValue(DateTimeKind.Unspecified)]
		[DisplayName("DateTime Kind")]
		public DateTimeKind DateTimeKind
		{
			get
			{
				object obj;
				if (this.TryGetValue("datetimekind", out obj))
				{
					if (obj is DateTimeKind)
					{
						return (DateTimeKind)obj;
					}
					if (obj != null)
					{
						return (DateTimeKind)TypeDescriptor.GetConverter(typeof(DateTimeKind)).ConvertFrom(obj);
					}
				}
				return DateTimeKind.Unspecified;
			}
			set
			{
				this["datetimekind"] = value;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x000526FC File Offset: 0x000508FC
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x00052740 File Offset: 0x00050940
		[DisplayName("DateTime Format String")]
		[DefaultValue(null)]
		[Browsable(true)]
		public string DateTimeFormatString
		{
			get
			{
				object obj;
				if (this.TryGetValue("datetimeformatstring", out obj))
				{
					if (obj is string)
					{
						return (string)obj;
					}
					if (obj != null)
					{
						return obj.ToString();
					}
				}
				return null;
			}
			set
			{
				this["datetimeformatstring"] = value;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x00052750 File Offset: 0x00050950
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x00052794 File Offset: 0x00050994
		[DefaultValue("sqlite_default_schema")]
		[DisplayName("Base Schema Name")]
		[Browsable(true)]
		public string BaseSchemaName
		{
			get
			{
				object obj;
				if (this.TryGetValue("baseschemaname", out obj))
				{
					if (obj is string)
					{
						return (string)obj;
					}
					if (obj != null)
					{
						return obj.ToString();
					}
				}
				return null;
			}
			set
			{
				this["baseschemaname"] = value;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x000527A4 File Offset: 0x000509A4
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x000527F0 File Offset: 0x000509F0
		[DefaultValue(SQLiteJournalModeEnum.Default)]
		[DisplayName("Journal Mode")]
		[Browsable(true)]
		public SQLiteJournalModeEnum JournalMode
		{
			get
			{
				object obj;
				this.TryGetValue("journal mode", out obj);
				if (obj is string)
				{
					return (SQLiteJournalModeEnum)TypeDescriptor.GetConverter(typeof(SQLiteJournalModeEnum)).ConvertFrom(obj);
				}
				return (SQLiteJournalModeEnum)obj;
			}
			set
			{
				this["journal mode"] = value;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00052804 File Offset: 0x00050A04
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00052850 File Offset: 0x00050A50
		[Browsable(true)]
		[DisplayName("Default Isolation Level")]
		[DefaultValue(IsolationLevel.Serializable)]
		public IsolationLevel DefaultIsolationLevel
		{
			get
			{
				object obj;
				this.TryGetValue("default isolationlevel", out obj);
				if (obj is string)
				{
					return (IsolationLevel)TypeDescriptor.GetConverter(typeof(IsolationLevel)).ConvertFrom(obj);
				}
				return (IsolationLevel)obj;
			}
			set
			{
				this["default isolationlevel"] = value;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00052864 File Offset: 0x00050A64
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x000528BC File Offset: 0x00050ABC
		[DefaultValue((DbType)(-1))]
		[DisplayName("Default Database Type")]
		[Browsable(true)]
		public DbType DefaultDbType
		{
			get
			{
				object obj;
				if (this.TryGetValue("defaultdbtype", out obj))
				{
					if (obj is string)
					{
						return (DbType)TypeDescriptor.GetConverter(typeof(DbType)).ConvertFrom(obj);
					}
					if (obj != null)
					{
						return (DbType)obj;
					}
				}
				return (DbType)(-1);
			}
			set
			{
				this["defaultdbtype"] = value;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x000528D0 File Offset: 0x00050AD0
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x00052900 File Offset: 0x00050B00
		[DefaultValue(null)]
		[DisplayName("Default Type Name")]
		[Browsable(true)]
		public string DefaultTypeName
		{
			get
			{
				object obj;
				this.TryGetValue("defaulttypename", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["defaulttypename"] = value;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x00052910 File Offset: 0x00050B10
		// (set) Token: 0x0600117B RID: 4475 RVA: 0x00052940 File Offset: 0x00050B40
		[DefaultValue(null)]
		[DisplayName("VFS Name")]
		[Browsable(true)]
		public string VfsName
		{
			get
			{
				object obj;
				this.TryGetValue("vfsname", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["vfsname"] = value;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00052950 File Offset: 0x00050B50
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x00052978 File Offset: 0x00050B78
		[Browsable(true)]
		[DefaultValue(false)]
		[DisplayName("Foreign Keys")]
		public bool ForeignKeys
		{
			get
			{
				object obj;
				this.TryGetValue("foreign keys", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["foreign keys"] = value;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x0005298C File Offset: 0x00050B8C
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x000529B4 File Offset: 0x00050BB4
		[DefaultValue(false)]
		[Browsable(true)]
		[DisplayName("Recursive Triggers")]
		public bool RecursiveTriggers
		{
			get
			{
				object obj;
				this.TryGetValue("recursive triggers", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["recursive triggers"] = value;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x000529C8 File Offset: 0x00050BC8
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x000529F8 File Offset: 0x00050BF8
		[DefaultValue(null)]
		[Browsable(true)]
		[DisplayName("ZipVFS Version")]
		public string ZipVfsVersion
		{
			get
			{
				object obj;
				this.TryGetValue("zipvfsversion", out obj);
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
			set
			{
				this["zipvfsversion"] = value;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00052A08 File Offset: 0x00050C08
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x00052A68 File Offset: 0x00050C68
		[Browsable(true)]
		[DefaultValue(SQLiteConnectionFlags.Default)]
		public SQLiteConnectionFlags Flags
		{
			get
			{
				object obj;
				if (this.TryGetValue("flags", out obj))
				{
					if (obj is SQLiteConnectionFlags)
					{
						return (SQLiteConnectionFlags)obj;
					}
					if (obj != null)
					{
						return (SQLiteConnectionFlags)TypeDescriptor.GetConverter(typeof(SQLiteConnectionFlags)).ConvertFrom(obj);
					}
				}
				return SQLiteConnectionFlags.Default;
			}
			set
			{
				this["flags"] = value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x00052A7C File Offset: 0x00050C7C
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x00052AA4 File Offset: 0x00050CA4
		[Browsable(true)]
		[DisplayName("Set Defaults")]
		[DefaultValue(true)]
		public bool SetDefaults
		{
			get
			{
				object obj;
				this.TryGetValue("setdefaults", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["setdefaults"] = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00052AB8 File Offset: 0x00050CB8
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x00052AE0 File Offset: 0x00050CE0
		[DefaultValue(true)]
		[Browsable(true)]
		[DisplayName("To Full Path")]
		public bool ToFullPath
		{
			get
			{
				object obj;
				this.TryGetValue("tofullpath", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["tofullpath"] = value;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00052AF4 File Offset: 0x00050CF4
		// (set) Token: 0x06001189 RID: 4489 RVA: 0x00052B1C File Offset: 0x00050D1C
		[Browsable(true)]
		[DisplayName("No Default Flags")]
		[DefaultValue(false)]
		public bool NoDefaultFlags
		{
			get
			{
				object obj;
				this.TryGetValue("nodefaultflags", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["nodefaultflags"] = value;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00052B30 File Offset: 0x00050D30
		// (set) Token: 0x0600118B RID: 4491 RVA: 0x00052B58 File Offset: 0x00050D58
		[DefaultValue(false)]
		[Browsable(true)]
		[DisplayName("No Shared Flags")]
		public bool NoSharedFlags
		{
			get
			{
				object obj;
				this.TryGetValue("nosharedflags", out obj);
				return SQLiteConvert.ToBoolean(obj);
			}
			set
			{
				this["nosharedflags"] = value;
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00052B6C File Offset: 0x00050D6C
		public override bool TryGetValue(string keyword, out object value)
		{
			bool flag = base.TryGetValue(keyword, out value);
			if (!this._properties.ContainsKey(keyword))
			{
				return flag;
			}
			PropertyDescriptor propertyDescriptor = this._properties[keyword] as PropertyDescriptor;
			if (propertyDescriptor == null)
			{
				return flag;
			}
			if (flag)
			{
				if (propertyDescriptor.PropertyType == typeof(bool))
				{
					value = SQLiteConvert.ToBoolean(value);
				}
				else if (propertyDescriptor.PropertyType != typeof(byte[]))
				{
					value = TypeDescriptor.GetConverter(propertyDescriptor.PropertyType).ConvertFrom(value);
				}
			}
			else
			{
				DefaultValueAttribute defaultValueAttribute = propertyDescriptor.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
				if (defaultValueAttribute != null)
				{
					value = defaultValueAttribute.Value;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00052C48 File Offset: 0x00050E48
		private void FallbackGetProperties(Hashtable propertyList)
		{
			foreach (object obj in TypeDescriptor.GetProperties(this, true))
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				if (propertyDescriptor.Name != "ConnectionString" && !propertyList.ContainsKey(propertyDescriptor.DisplayName))
				{
					propertyList.Add(propertyDescriptor.DisplayName, propertyDescriptor);
				}
			}
		}

		// Token: 0x040006BF RID: 1727
		private Hashtable _properties;
	}
}
