using System;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.SQLite
{
	// Token: 0x02000195 RID: 405
	[ToolboxItem("SQLite.Designer.SQLiteDataAdapterToolboxItem, SQLite.Designer, Version=1.0.116.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
	[DefaultEvent("RowUpdated")]
	[Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class SQLiteDataAdapter : DbDataAdapter
	{
		// Token: 0x0600119D RID: 4509 RVA: 0x00052F04 File Offset: 0x00051104
		public SQLiteDataAdapter()
		{
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00052F14 File Offset: 0x00051114
		public SQLiteDataAdapter(SQLiteCommand cmd)
		{
			this.SelectCommand = cmd;
			this.disposeSelect = false;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00052F34 File Offset: 0x00051134
		public SQLiteDataAdapter(string commandText, SQLiteConnection connection)
		{
			this.SelectCommand = new SQLiteCommand(commandText, connection);
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00052F50 File Offset: 0x00051150
		public SQLiteDataAdapter(string commandText, string connectionString)
			: this(commandText, connectionString, false)
		{
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00052F5C File Offset: 0x0005115C
		public SQLiteDataAdapter(string commandText, string connectionString, bool parseViaFramework)
		{
			SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString, parseViaFramework);
			this.SelectCommand = new SQLiteCommand(commandText, sqliteConnection);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00052F90 File Offset: 0x00051190
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(SQLiteDataAdapter).Name);
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00052FB4 File Offset: 0x000511B4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.disposed && disposing)
				{
					if (this.disposeSelect && this.SelectCommand != null)
					{
						this.SelectCommand.Dispose();
						this.SelectCommand = null;
					}
					if (this.InsertCommand != null)
					{
						this.InsertCommand.Dispose();
						this.InsertCommand = null;
					}
					if (this.UpdateCommand != null)
					{
						this.UpdateCommand.Dispose();
						this.UpdateCommand = null;
					}
					if (this.DeleteCommand != null)
					{
						this.DeleteCommand.Dispose();
						this.DeleteCommand = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060011A4 RID: 4516 RVA: 0x00053078 File Offset: 0x00051278
		// (remove) Token: 0x060011A5 RID: 4517 RVA: 0x000530F0 File Offset: 0x000512F0
		public event EventHandler<RowUpdatingEventArgs> RowUpdating
		{
			add
			{
				this.CheckDisposed();
				EventHandler<RowUpdatingEventArgs> eventHandler = (EventHandler<RowUpdatingEventArgs>)base.Events[SQLiteDataAdapter._updatingEventPH];
				if (eventHandler != null && value.Target is DbCommandBuilder)
				{
					EventHandler<RowUpdatingEventArgs> eventHandler2 = (EventHandler<RowUpdatingEventArgs>)SQLiteDataAdapter.FindBuilder(eventHandler);
					if (eventHandler2 != null)
					{
						base.Events.RemoveHandler(SQLiteDataAdapter._updatingEventPH, eventHandler2);
					}
				}
				base.Events.AddHandler(SQLiteDataAdapter._updatingEventPH, value);
			}
			remove
			{
				this.CheckDisposed();
				base.Events.RemoveHandler(SQLiteDataAdapter._updatingEventPH, value);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0005310C File Offset: 0x0005130C
		internal static Delegate FindBuilder(MulticastDelegate mcd)
		{
			if (mcd != null)
			{
				Delegate[] invocationList = mcd.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					if (invocationList[i].Target is DbCommandBuilder)
					{
						return invocationList[i];
					}
				}
			}
			return null;
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060011A7 RID: 4519 RVA: 0x0005315C File Offset: 0x0005135C
		// (remove) Token: 0x060011A8 RID: 4520 RVA: 0x00053178 File Offset: 0x00051378
		public event EventHandler<RowUpdatedEventArgs> RowUpdated
		{
			add
			{
				this.CheckDisposed();
				base.Events.AddHandler(SQLiteDataAdapter._updatedEventPH, value);
			}
			remove
			{
				this.CheckDisposed();
				base.Events.RemoveHandler(SQLiteDataAdapter._updatedEventPH, value);
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00053194 File Offset: 0x00051394
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			EventHandler<RowUpdatingEventArgs> eventHandler = base.Events[SQLiteDataAdapter._updatingEventPH] as EventHandler<RowUpdatingEventArgs>;
			if (eventHandler != null)
			{
				eventHandler(this, value);
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000531CC File Offset: 0x000513CC
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			EventHandler<RowUpdatedEventArgs> eventHandler = base.Events[SQLiteDataAdapter._updatedEventPH] as EventHandler<RowUpdatedEventArgs>;
			if (eventHandler != null)
			{
				eventHandler(this, value);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x00053204 File Offset: 0x00051404
		// (set) Token: 0x060011AC RID: 4524 RVA: 0x00053218 File Offset: 0x00051418
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue(null)]
		public new SQLiteCommand SelectCommand
		{
			get
			{
				this.CheckDisposed();
				return (SQLiteCommand)base.SelectCommand;
			}
			set
			{
				this.CheckDisposed();
				base.SelectCommand = value;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x00053228 File Offset: 0x00051428
		// (set) Token: 0x060011AE RID: 4526 RVA: 0x0005323C File Offset: 0x0005143C
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue(null)]
		public new SQLiteCommand InsertCommand
		{
			get
			{
				this.CheckDisposed();
				return (SQLiteCommand)base.InsertCommand;
			}
			set
			{
				this.CheckDisposed();
				base.InsertCommand = value;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x0005324C File Offset: 0x0005144C
		// (set) Token: 0x060011B0 RID: 4528 RVA: 0x00053260 File Offset: 0x00051460
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public new SQLiteCommand UpdateCommand
		{
			get
			{
				this.CheckDisposed();
				return (SQLiteCommand)base.UpdateCommand;
			}
			set
			{
				this.CheckDisposed();
				base.UpdateCommand = value;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00053270 File Offset: 0x00051470
		// (set) Token: 0x060011B2 RID: 4530 RVA: 0x00053284 File Offset: 0x00051484
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public new SQLiteCommand DeleteCommand
		{
			get
			{
				this.CheckDisposed();
				return (SQLiteCommand)base.DeleteCommand;
			}
			set
			{
				this.CheckDisposed();
				base.DeleteCommand = value;
			}
		}

		// Token: 0x0400072B RID: 1835
		private bool disposeSelect = true;

		// Token: 0x0400072C RID: 1836
		private static object _updatingEventPH = new object();

		// Token: 0x0400072D RID: 1837
		private static object _updatedEventPH = new object();

		// Token: 0x0400072E RID: 1838
		private bool disposed;
	}
}
