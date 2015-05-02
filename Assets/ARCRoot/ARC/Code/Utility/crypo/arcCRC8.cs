using UnityEngine;
using System.Collections;

public static class arcCRC8
{
	const byte polynomial = 0xA1;
	static readonly byte[] table = new byte[256];
	
	public static byte ComputeChecksum(byte[] bytes)
	{
		byte crc = 0;
		for (int i = 0; i < bytes.Length; ++i)
		{
			byte index = (byte)(crc ^ bytes[i]);
			crc = (byte)((crc >> 4) ^ table[index]);
		}
		return crc;
	}
	
	static arcCRC8()
	{
		byte value;
		byte temp;
		for (ushort i = 0; i < table.Length; i++)
		{
			value = 0;
			temp = (byte)i;
			for (byte j = 0; j < 4; j++)
			{
				if (((value ^ temp) & 0x01) != 0)
				{
					value = (byte)((value >> 1) ^ polynomial);
				}
				else
				{
					value >>= 1;
				}
				temp >>= 1;
			}
			table[i] = value;
		}
	}
}