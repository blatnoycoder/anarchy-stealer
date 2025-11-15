using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Data.SQLite
{
	// Token: 0x0200019A RID: 410
	public sealed class SQLiteFactory : DbProviderFactory, IDisposable, IServiceProvider
	{
		// Token: 0x06001200 RID: 4608 RVA: 0x000563F8 File Offset: 0x000545F8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00056408 File Offset: 0x00054608
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteFactory).Name);
			}
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0005642C File Offset: 0x0005462C
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00056440 File Offset: 0x00054640
		~SQLiteFactory()
		{
			this.Dispose(false);
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001204 RID: 4612 RVA: 0x00056470 File Offset: 0x00054670
		// (remove) Token: 0x06001205 RID: 4613 RVA: 0x00056480 File Offset: 0x00054680
		public event SQLiteLogEventHandler Log
		{
			add
			{
				this.CheckDisposed();
				SQLiteLog.Log += value;
			}
			remove
			{
				this.CheckDisposed();
				SQLiteLog.Log -= value;
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00056490 File Offset: 0x00054690
		public override DbCommand CreateCommand()
		{
			this.CheckDisposed();
			return new SQLiteCommand();
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000564A0 File Offset: 0x000546A0
		public override DbCommandBuilder CreateCommandBuilder()
		{
			this.CheckDisposed();
			return new SQLiteCommandBuilder();
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000564B0 File Offset: 0x000546B0
		public override DbConnection CreateConnection()
		{
			this.CheckDisposed();
			return new SQLiteConnection();
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x000564C0 File Offset: 0x000546C0
		public override DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			this.CheckDisposed();
			return new SQLiteConnectionStringBuilder();
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x000564D0 File Offset: 0x000546D0
		public override DbDataAdapter CreateDataAdapter()
		{
			this.CheckDisposed();
			return new SQLiteDataAdapter();
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000564E0 File Offset: 0x000546E0
		public override DbParameter CreateParameter()
		{
			this.CheckDisposed();
			return new SQLiteParameter();
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x000564F0 File Offset: 0x000546F0
		static SQLiteFactory()
		{
			SQLiteFactory.InitializeDbProviderServices();
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00056514 File Offset: 0x00054714
		internal static void PreInitialize()
		{
			UnsafeNativeMethods.Initialize();
			SQLiteLog.Initialize(typeof(SQLiteFactory).Name);
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00056530 File Offset: 0x00054730
		private static void InitializeDbProviderServices()
		{
			SQLiteFactory.PreInitialize();
			string text = "4.0.0.0";
			SQLiteFactory._dbProviderServicesType = Type.GetType(HelperMethods.StringFormat(CultureInfo.InvariantCulture, "System.Data.Common.DbProviderServices, System.Data.Entity, Version={0}, Culture=neutral, PublicKeyToken=b77a5c561934e089", new object[] { text }), false);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00056574 File Offset: 0x00054774
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(ISQLiteSchemaExtensions) || (SQLiteFactory._dbProviderServicesType != null && serviceType == SQLiteFactory._dbProviderServicesType))
			{
				object sqliteProviderServicesInstance = this.GetSQLiteProviderServicesInstance();
				if (SQLite3.ForceLogLifecycle())
				{
					SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Success of \"{0}\" from SQLiteFactory.GetService(\"{1}\")...", new object[]
					{
						(sqliteProviderServicesInstance != null) ? sqliteProviderServicesInstance.ToString() : "<null>",
						(serviceType != null) ? serviceType.ToString() : "<null>"
					}));
				}
				return sqliteProviderServicesInstance;
			}
			if (SQLite3.ForceLogLifecycle())
			{
				SQLiteLog.LogMessage(HelperMethods.StringFormat(CultureInfo.CurrentCulture, "Failure of SQLiteFactory.GetService(\"{0}\")...", new object[] { (serviceType != null) ? serviceType.ToString() : "<null>" }));
			}
			return null;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00056668 File Offset: 0x00054868
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private object GetSQLiteProviderServicesInstance()
		{
			if (SQLiteFactory._sqliteServices == null)
			{
				string text = UnsafeNativeMethods.GetSettingValue("TypeName_SQLiteProviderServices", null);
				Version version = base.GetType().Assembly.GetName().Version;
				if (text != null)
				{
					text = HelperMethods.StringFormat(CultureInfo.InvariantCulture, text, new object[] { version });
				}
				else
				{
					text = HelperMethods.StringFormat(CultureInfo.InvariantCulture, SQLiteFactory.DefaultTypeName, new object[] { version });
				}
				Type type = Type.GetType(text, false);
				if (type != null)
				{
					FieldInfo field = type.GetField("Instance", SQLiteFactory.DefaultBindingFlags);
					if (field != null)
					{
						SQLiteFactory._sqliteServices = field.GetValue(null);
					}
				}
			}
			return SQLiteFactory._sqliteServices;
		}

		// Token: 0x040007B1 RID: 1969
		private bool disposed;

		// Token: 0x040007B2 RID: 1970
		public static readonly SQLiteFactory Instance = new SQLiteFactory();

		// Token: 0x040007B3 RID: 1971
		private static readonly string DefaultTypeName = "System.Data.SQLite.Linq.SQLiteProviderServices, System.Data.SQLite.Linq, Version={0}, Culture=neutral, PublicKeyToken=db937bc2d44ff139";

		// Token: 0x040007B4 RID: 1972
		private static readonly BindingFlags DefaultBindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

		// Token: 0x040007B5 RID: 1973
		private static Type _dbProviderServicesType;

		// Token: 0x040007B6 RID: 1974
		private static object _sqliteServices;
	}
}
