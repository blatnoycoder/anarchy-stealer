using System;
using System.IO;
using System.Text;

namespace Stealer
{
	// Token: 0x02000015 RID: 21
	internal class SQLite
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000045B0 File Offset: 0x000027B0
		public SQLite(string fileName)
		{
			this._fileBytes = File.ReadAllBytes(fileName);
			this._pageSize = this.ConvertToULong(16, 2);
			this._dbEncoding = this.ConvertToULong(56, 4);
			this.ReadMasterTable(100L);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004618 File Offset: 0x00002818
		public string GetValue(int rowNum, int field)
		{
			string text;
			try
			{
				bool flag = rowNum >= this._tableEntries.Length;
				if (flag)
				{
					text = null;
				}
				else
				{
					text = ((field >= this._tableEntries[rowNum].Content.Length) ? null : this._tableEntries[rowNum].Content[field]);
				}
			}
			catch
			{
				text = "";
			}
			return text;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000046A0 File Offset: 0x000028A0
		public int GetRowCount()
		{
			return this._tableEntries.Length;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000046C4 File Offset: 0x000028C4
		private bool ReadTableFromOffset(ulong offset)
		{
			bool flag11;
			try
			{
				bool flag = this._fileBytes[(int)(checked((IntPtr)offset))] == 13;
				if (flag)
				{
					uint num = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
					int num2 = 0;
					bool flag2 = this._tableEntries != null;
					if (flag2)
					{
						num2 = this._tableEntries.Length;
						Array.Resize<SQLite.TableEntry>(ref this._tableEntries, this._tableEntries.Length + (int)num + 1);
					}
					else
					{
						this._tableEntries = new SQLite.TableEntry[num + 1U];
					}
					for (uint num3 = 0U; num3 <= num; num3 += 1U)
					{
						ulong num4 = this.ConvertToULong((int)offset + 8 + (int)(num3 * 2U), 2);
						bool flag3 = offset != 100UL;
						if (flag3)
						{
							num4 += offset;
						}
						int num5 = this.Gvl((int)num4);
						this.Cvl((int)num4, num5);
						int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
						this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
						ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
						int num8 = this.Gvl((int)num7);
						int num9 = num8;
						long num10 = this.Cvl((int)num7, num8);
						SQLite.RecordHeaderField[] array = null;
						long num11 = (long)(num7 - (ulong)((long)num8) + 1UL);
						int num12 = 0;
						while (num11 < num10)
						{
							Array.Resize<SQLite.RecordHeaderField>(ref array, num12 + 1);
							int num13 = num9 + 1;
							num9 = this.Gvl(num13);
							array[num12].Type = this.Cvl(num13, num9);
							array[num12].Size = (long)((array[num12].Type <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[num12].Type))]) : ((ulong)((!SQLite.IsOdd(array[num12].Type)) ? ((array[num12].Type - 12L) / 2L) : ((array[num12].Type - 13L) / 2L))));
							num11 = num11 + (long)(num9 - num13) + 1L;
							num12++;
						}
						bool flag4 = array != null;
						if (flag4)
						{
							this._tableEntries[num2 + (int)num3].Content = new string[array.Length];
							int num14 = 0;
							for (int i = 0; i <= array.Length - 1; i++)
							{
								bool flag5 = array[i].Type > 9L;
								if (flag5)
								{
									bool flag6 = !SQLite.IsOdd(array[i].Type);
									if (flag6)
									{
										bool flag7 = this._dbEncoding == 1UL;
										if (flag7)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
										else
										{
											bool flag8 = this._dbEncoding == 2UL;
											if (flag8)
											{
												this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
											}
											else
											{
												bool flag9 = this._dbEncoding == 3UL;
												if (flag9)
												{
													this._tableEntries[num2 + (int)num3].Content[i] = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
												}
											}
										}
									}
									else
									{
										this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
									}
								}
								else
								{
									this._tableEntries[num2 + (int)num3].Content[i] = Convert.ToString(this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size));
								}
								num14 += (int)array[i].Size;
							}
						}
					}
				}
				else
				{
					bool flag10 = this._fileBytes[(int)(checked((IntPtr)offset))] == 5;
					if (flag10)
					{
						uint num15 = (uint)(this.ConvertToULong((int)(offset + 3UL), 2) - 1UL);
						for (uint num16 = 0U; num16 <= num15; num16 += 1U)
						{
							uint num17 = (uint)this.ConvertToULong((int)offset + 12 + (int)(num16 * 2U), 2);
							this.ReadTableFromOffset((this.ConvertToULong((int)(offset + (ulong)num17), 4) - 1UL) * this._pageSize);
						}
						this.ReadTableFromOffset((this.ConvertToULong((int)(offset + 8UL), 4) - 1UL) * this._pageSize);
					}
				}
				flag11 = true;
			}
			catch
			{
				flag11 = false;
			}
			return flag11;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004C04 File Offset: 0x00002E04
		private void ReadMasterTable(long offset)
		{
			try
			{
				byte b = this._fileBytes[(int)(checked((IntPtr)offset))];
				byte b2 = b;
				if (b2 != 5)
				{
					if (b2 == 13)
					{
						ulong num = this.ConvertToULong((int)offset + 3, 2) - 1UL;
						int num2 = 0;
						bool flag = this._masterTableEntries != null;
						if (flag)
						{
							num2 = this._masterTableEntries.Length;
							Array.Resize<SQLite.SqliteMasterEntry>(ref this._masterTableEntries, this._masterTableEntries.Length + (int)num + 1);
						}
						else
						{
							this._masterTableEntries = new SQLite.SqliteMasterEntry[num + 1UL];
						}
						for (ulong num3 = 0UL; num3 <= num; num3 += 1UL)
						{
							ulong num4 = this.ConvertToULong((int)offset + 8 + (int)num3 * 2, 2);
							bool flag2 = offset != 100L;
							if (flag2)
							{
								num4 += (ulong)offset;
							}
							int num5 = this.Gvl((int)num4);
							this.Cvl((int)num4, num5);
							int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
							this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
							ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
							int num8 = this.Gvl((int)num7);
							int num9 = num8;
							long num10 = this.Cvl((int)num7, num8);
							long[] array = new long[5];
							for (int i = 0; i <= 4; i++)
							{
								int num11 = num9 + 1;
								num9 = this.Gvl(num11);
								array[i] = this.Cvl(num11, num9);
								array[i] = (long)((array[i] <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[i]))]) : ((ulong)((!SQLite.IsOdd(array[i])) ? ((array[i] - 12L) / 2L) : ((array[i] - 13L) / 2L))));
							}
							bool flag3 = this._dbEncoding == 1UL || this._dbEncoding == 2UL;
							if (flag3)
							{
								bool flag4 = this._dbEncoding == 1UL;
								if (flag4)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
								else
								{
									bool flag5 = this._dbEncoding == 2UL;
									if (flag5)
									{
										this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
									}
									else
									{
										bool flag6 = this._dbEncoding == 3UL;
										if (flag6)
										{
											this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
										}
									}
								}
							}
							this._masterTableEntries[num2 + (int)num3].RootNum = (long)this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2]), (int)array[3]);
							bool flag7 = this._dbEncoding == 1UL;
							if (flag7)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
							else
							{
								bool flag8 = this._dbEncoding == 2UL;
								if (flag8)
								{
									this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
								}
								else
								{
									bool flag9 = this._dbEncoding == 3UL;
									if (flag9)
									{
										this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
									}
								}
							}
						}
					}
				}
				else
				{
					uint num12 = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
					for (int j = 0; j <= (int)num12; j++)
					{
						uint num13 = (uint)this.ConvertToULong((int)offset + 12 + j * 2, 2);
						bool flag10 = offset == 100L;
						if (flag10)
						{
							this.ReadMasterTable((long)((this.ConvertToULong((int)num13, 4) - 1UL) * this._pageSize));
						}
						else
						{
							this.ReadMasterTable((long)((this.ConvertToULong((int)(offset + (long)((ulong)num13)), 4) - 1UL) * this._pageSize));
						}
					}
					this.ReadMasterTable((long)((this.ConvertToULong((int)offset + 8, 4) - 1UL) * this._pageSize));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000511C File Offset: 0x0000331C
		public bool ReadTable(string tableName)
		{
			bool flag3;
			try
			{
				int num = -1;
				for (int i = 0; i <= this._masterTableEntries.Length; i++)
				{
					bool flag = string.Compare(this._masterTableEntries[i].ItemName.ToLower(), tableName.ToLower(), StringComparison.Ordinal) == 0;
					if (flag)
					{
						num = i;
						break;
					}
				}
				bool flag2 = num == -1;
				if (flag2)
				{
					flag3 = false;
				}
				else
				{
					string[] array = this._masterTableEntries[num].SqlStatement.Substring(this._masterTableEntries[num].SqlStatement.IndexOf("(", StringComparison.Ordinal) + 1).Split(new char[] { ',' });
					for (int j = 0; j <= array.Length - 1; j++)
					{
						array[j] = array[j].TrimStart(new char[0]);
						int num2 = array[j].IndexOf(' ');
						bool flag4 = num2 > 0;
						if (flag4)
						{
							array[j] = array[j].Substring(0, num2);
						}
						bool flag5 = array[j].IndexOf("UNIQUE", StringComparison.Ordinal) != 0;
						if (flag5)
						{
							Array.Resize<string>(ref this._fieldNames, j + 1);
							this._fieldNames[j] = array[j];
						}
					}
					flag3 = this.ReadTableFromOffset((ulong)((this._masterTableEntries[num].RootNum - 1L) * (long)this._pageSize));
				}
			}
			catch
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000052F0 File Offset: 0x000034F0
		private ulong ConvertToULong(int startIndex, int size)
		{
			ulong num;
			try
			{
				bool flag = (size > 8) | (size == 0);
				if (flag)
				{
					num = 0UL;
				}
				else
				{
					ulong num2 = 0UL;
					for (int i = 0; i <= size - 1; i++)
					{
						num2 = (num2 << 8) | (ulong)this._fileBytes[startIndex + i];
					}
					num = num2;
				}
			}
			catch
			{
				num = 0UL;
			}
			return num;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005368 File Offset: 0x00003568
		private int Gvl(int startIdx)
		{
			int num;
			try
			{
				bool flag = startIdx > this._fileBytes.Length;
				if (flag)
				{
					num = 0;
				}
				else
				{
					for (int i = startIdx; i <= startIdx + 8; i++)
					{
						bool flag2 = i > this._fileBytes.Length - 1;
						if (flag2)
						{
							return 0;
						}
						bool flag3 = (this._fileBytes[i] & 128) != 128;
						if (flag3)
						{
							return i;
						}
					}
					num = startIdx + 8;
				}
			}
			catch
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005414 File Offset: 0x00003614
		private long Cvl(int startIdx, int endIdx)
		{
			long num2;
			try
			{
				endIdx++;
				byte[] array = new byte[8];
				int num = endIdx - startIdx;
				bool flag = false;
				bool flag2 = (num == 0) | (num > 9);
				if (flag2)
				{
					num2 = 0L;
				}
				else
				{
					bool flag3 = num == 1;
					if (flag3)
					{
						array[0] = this._fileBytes[startIdx] & 127;
						num2 = BitConverter.ToInt64(array, 0);
					}
					else
					{
						bool flag4 = num == 9;
						if (flag4)
						{
							flag = true;
						}
						int num3 = 1;
						int num4 = 7;
						int num5 = 0;
						bool flag5 = flag;
						if (flag5)
						{
							array[0] = this._fileBytes[endIdx - 1];
							endIdx--;
							num5 = 1;
						}
						for (int i = endIdx - 1; i >= startIdx; i += -1)
						{
							bool flag6 = i - 1 >= startIdx;
							if (flag6)
							{
								array[num5] = (byte)(((this._fileBytes[i] >> num3 - 1) & (255 >> num3)) | ((int)this._fileBytes[i - 1] << num4));
								num3++;
								num5++;
								num4--;
							}
							else
							{
								bool flag7 = !flag;
								if (flag7)
								{
									array[num5] = (byte)((this._fileBytes[i] >> num3 - 1) & (255 >> num3));
								}
							}
						}
						num2 = BitConverter.ToInt64(array, 0);
					}
				}
			}
			catch
			{
				num2 = 0L;
			}
			return num2;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000055A0 File Offset: 0x000037A0
		private static bool IsOdd(long value)
		{
			return (value & 1L) == 1L;
		}

		// Token: 0x0400006F RID: 111
		private readonly byte[] _sqlDataTypeSize = new byte[] { 0, 1, 2, 3, 4, 6, 8, 8, 0, 0 };

		// Token: 0x04000070 RID: 112
		private readonly ulong _dbEncoding;

		// Token: 0x04000071 RID: 113
		private readonly byte[] _fileBytes;

		// Token: 0x04000072 RID: 114
		private readonly ulong _pageSize;

		// Token: 0x04000073 RID: 115
		private string[] _fieldNames;

		// Token: 0x04000074 RID: 116
		private SQLite.SqliteMasterEntry[] _masterTableEntries;

		// Token: 0x04000075 RID: 117
		private SQLite.TableEntry[] _tableEntries;

		// Token: 0x020001FA RID: 506
		private struct RecordHeaderField
		{
			// Token: 0x0400092E RID: 2350
			public long Size;

			// Token: 0x0400092F RID: 2351
			public long Type;
		}

		// Token: 0x020001FB RID: 507
		private struct TableEntry
		{
			// Token: 0x04000930 RID: 2352
			public string[] Content;
		}

		// Token: 0x020001FC RID: 508
		private struct SqliteMasterEntry
		{
			// Token: 0x04000931 RID: 2353
			public string ItemName;

			// Token: 0x04000932 RID: 2354
			public long RootNum;

			// Token: 0x04000933 RID: 2355
			public string SqlStatement;
		}
	}
}
