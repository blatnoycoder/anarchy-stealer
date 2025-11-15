using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000F4 RID: 244
	[NullableContext(1)]
	[Nullable(0)]
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x00031F80 File Offset: 0x00030180
		public JPropertyDescriptor(string name)
			: base(name, null)
		{
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00031F8C File Offset: 0x0003018C
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00031F94 File Offset: 0x00030194
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00031F98 File Offset: 0x00030198
		[return: Nullable(2)]
		public override object GetValue(object component)
		{
			JObject jobject = component as JObject;
			if (jobject == null)
			{
				return null;
			}
			return jobject[this.Name];
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00031FB4 File Offset: 0x000301B4
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00031FB8 File Offset: 0x000301B8
		public override void SetValue(object component, object value)
		{
			JObject jobject = component as JObject;
			if (jobject != null)
			{
				JToken jtoken = (value as JToken) ?? new JValue(value);
				jobject[this.Name] = jtoken;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00031FF8 File Offset: 0x000301F8
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00031FFC File Offset: 0x000301FC
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00032008 File Offset: 0x00030208
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0003200C File Offset: 0x0003020C
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00032018 File Offset: 0x00030218
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
