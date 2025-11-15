using System;
using System.IO;

namespace Utils
{
	// Token: 0x02000002 RID: 2
	internal class FileUtils
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static string CreateTempDuplicateFile(string filePath)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("LOCALAPPDATA");
			string randomFileName = Path.GetRandomFileName();
			string text = environmentVariable + "\\Temp\\" + randomFileName;
			File.Copy(filePath, text);
			return text;
		}
	}
}
