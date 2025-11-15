using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TypeExtensions
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x0001F36C File Offset: 0x0001D56C
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001F374 File Offset: 0x0001D574
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001F37C File Offset: 0x0001D57C
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001F384 File Offset: 0x0001D584
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001F38C File Offset: 0x0001D58C
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001F394 File Offset: 0x0001D594
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001F39C File Offset: 0x0001D59C
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001F3A4 File Offset: 0x0001D5A4
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001F3AC File Offset: 0x0001D5AC
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001F3B4 File Offset: 0x0001D5B4
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001F3BC File Offset: 0x0001D5BC
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001F3CC File Offset: 0x0001D5CC
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001F3D4 File Offset: 0x0001D5D4
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001F3DC File Offset: 0x0001D5DC
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001F3E4 File Offset: 0x0001D5E4
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces, [Nullable(2)] [NotNullWhen(true)] out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			if (searchInterfaces)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (string.Equals(interfaces[i].Name, fullTypeName, StringComparison.Ordinal))
					{
						match = type;
						return true;
					}
				}
			}
			match = null;
			return false;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001F464 File Offset: 0x0001D664
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, searchInterfaces, out type2);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001F480 File Offset: 0x0001D680
		public static bool ImplementInterface(this Type type, Type interfaceType)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (Type type3 in ((IEnumerable<Type>)type2.GetInterfaces()))
				{
					if (type3 == interfaceType || (type3 != null && type3.ImplementInterface(interfaceType)))
					{
						return true;
					}
				}
				type2 = type2.BaseType();
			}
			return false;
		}
	}
}
