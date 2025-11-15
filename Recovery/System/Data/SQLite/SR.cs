using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Data.SQLite
{
	// Token: 0x020001BE RID: 446
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal sealed class SR
	{
		// Token: 0x06001431 RID: 5169 RVA: 0x0005D888 File Offset: 0x0005BA88
		internal SR()
		{
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0005D890 File Offset: 0x0005BA90
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(SR.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("System.Data.SQLite.SR", typeof(SR).Assembly);
					SR.resourceMan = resourceManager;
				}
				return SR.resourceMan;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0005D8D8 File Offset: 0x0005BAD8
		// (set) Token: 0x06001434 RID: 5172 RVA: 0x0005D8E0 File Offset: 0x0005BAE0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return SR.resourceCulture;
			}
			set
			{
				SR.resourceCulture = value;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0005D8E8 File Offset: 0x0005BAE8
		internal static string DataTypes
		{
			get
			{
				return SR.ResourceManager.GetString("DataTypes", SR.resourceCulture);
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0005D900 File Offset: 0x0005BB00
		internal static string Keywords
		{
			get
			{
				return SR.ResourceManager.GetString("Keywords", SR.resourceCulture);
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x0005D918 File Offset: 0x0005BB18
		internal static string MetaDataCollections
		{
			get
			{
				return SR.ResourceManager.GetString("MetaDataCollections", SR.resourceCulture);
			}
		}

		// Token: 0x04000849 RID: 2121
		private static ResourceManager resourceMan;

		// Token: 0x0400084A RID: 2122
		private static CultureInfo resourceCulture;
	}
}
