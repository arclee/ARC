using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class arcCrypto
{
	static readonly int KeyLength = 24;
	static readonly int IVLength = 8;
	static TripleDES algorithm = null;
	static TripleDESCryptoServiceProvider serviceProvider = null;
	
	public delegate void WriteTextCB(StreamWriter writer);
	public delegate void WriteBinCB(BinaryWriter writer, MemoryStream fileStream, CryptoStream cryptoStream);
	public delegate void ReadTextCB(StreamReader reader);
	public delegate void ReadBinCB(BinaryReader reader, FileStream fileStream, CryptoStream cryptoStream);
	
	//static arcCrypto()
	//{
	//	initCrypto();
	//}
	
	static byte[] MakeKey(string input)
	{
		string mod = input.PadRight(KeyLength, '0').Substring(0, KeyLength);         
		return Encoding.ASCII.GetBytes(mod);
	}
	
	static byte[] MakeIV(string input)
	{
		string mod = input.PadRight(IVLength, '0').Substring(0, IVLength);   
		return Encoding.ASCII.GetBytes(mod);
	}
	
	static public void initCrypto()
	{
		
		if (algorithm == null)
		{
			string uniqueID = SystemInfo.deviceUniqueIdentifier;
			algorithm = TripleDES.Create();
			algorithm.Key = MakeKey(uniqueID);
			algorithm.IV = MakeIV(uniqueID);
#if UNITY_EDITOR
            Debug.Log("algorithm.Key:" + algorithm.Key);
            Debug.Log("algorithm.IV:" + algorithm.IV);
#endif
        }
		if (serviceProvider == null)
		{
			serviceProvider = new TripleDESCryptoServiceProvider();
		}		
	}

	public static int getCRC(byte[] bts)
	{
		int crc = arcCRC8.ComputeChecksum(bts);

		//Debug.Log("crc:" + crc.ToString());

		return crc;
	}


	
	public static bool WriteBin(string filepathname, WriteBinCB writeCB, bool useCrypto, string Crcfilepathname)
	{
		using (MemoryStream ms = new MemoryStream())
		{
			CryptoStream cryptoStream = null;
			BinaryWriter writer = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					ms,
					serviceProvider.CreateEncryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Write);
				writer = new BinaryWriter(cryptoStream);
			}
			else
			{
				writer = new BinaryWriter(ms);
			}
			
			//處理.
			if (writeCB != null)
			{
				writeCB(writer, ms, cryptoStream);
			}
			
			writer.Close();
			writer = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}


			//write data to disk.
			using (FileStream fileStream = new FileStream(filepathname, FileMode.Create, FileAccess.Write))
			{
				BinaryWriter binwriter = new BinaryWriter(fileStream);
				binwriter.Write(ms.ToArray());
				binwriter.Close();
				binwriter = null;
				fileStream.Close();
			}

			//write crc to disk.
			int crc = getCRC(ms.ToArray());
			using (FileStream fileStream = new FileStream(Crcfilepathname, FileMode.Create, FileAccess.Write))
			{
				CryptoStream crccryptoStream = null;
				BinaryWriter crcwriter = null;
				if (useCrypto)
				{
					crccryptoStream = new CryptoStream(
						fileStream,
						serviceProvider.CreateEncryptor(algorithm.Key, algorithm.IV),
						CryptoStreamMode.Write);
					crcwriter = new BinaryWriter(crccryptoStream);
				}
				else
				{
					crcwriter = new BinaryWriter(fileStream);
				}
				
				crcwriter.Write(crc);
				crcwriter.Close();
				crcwriter = null;
				
				if (useCrypto)
				{
					crccryptoStream.Close();
					crccryptoStream = null;
				}				
			}
		}
		return true;
	}


	public static bool WriteText(string filepathname, WriteTextCB writeCB, bool useCrypto, string Crcfilepathname)
	{
		using (MemoryStream ms = new MemoryStream())
		{
			CryptoStream cryptoStream = null;
			StreamWriter writer = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					ms,
					serviceProvider.CreateEncryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Write);
				writer = new StreamWriter(cryptoStream);
			}
			else
			{
				writer = new StreamWriter(ms);
			}
			
			//處理.
			if (writeCB != null)
			{
				writeCB(writer);
			}
			
			//Debug.Log("memstream length:" + ms.Length.ToString());
			writer.Close();
			writer = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}

			
			//write data to disk.
			using (FileStream fileStream = new FileStream(filepathname, FileMode.Create, FileAccess.Write))
			{
				BinaryWriter binwriter = new BinaryWriter(fileStream);
				//stwriter.Write(Encoding.UTF8.GetString(ms.ToArray()));
				binwriter.Write(ms.ToArray());
				binwriter.Close();
				binwriter = null;
				fileStream.Close();
			}

			//write crc to disk.
			//crc.
			int crc = getCRC(ms.ToArray());
			using (FileStream fileStream = new FileStream(Crcfilepathname, FileMode.Create, FileAccess.Write))
			{
				CryptoStream crccryptoStream = null;
				StreamWriter crcwriter = null;
				if (useCrypto)
				{
					crccryptoStream = new CryptoStream(
						fileStream,
						serviceProvider.CreateEncryptor(algorithm.Key, algorithm.IV),
						CryptoStreamMode.Write);
					crcwriter = new StreamWriter(crccryptoStream);
				}
				else
				{
					crcwriter = new StreamWriter(fileStream);
				}
				
				crcwriter.Write(crc);
				crcwriter.Close();
				crcwriter = null;
				
				if (useCrypto)
				{
					crccryptoStream.Close();
					crccryptoStream = null;
				}				
			}
		}

		return true;
	}

	public static bool ReadTextFromResouce(string filepathname, ReadTextCB readCB, bool useCrypto, string Crcfilepathname)
	{
		TextAsset mTextAsset = Resources.Load(filepathname) as TextAsset;
		if (mTextAsset == null)
		{
			Debug.Log("no file:" + filepathname.ToString());
			return false;;
		}
		TextAsset mTextAssetc = Resources.Load(Crcfilepathname) as TextAsset;
		if (mTextAssetc == null)
		{
			Debug.Log("no file:" + Crcfilepathname.ToString());
			return false;
		}
		
		//取得 CRC.
		int crc = 0;
		using (MemoryStream ms = new MemoryStream(mTextAssetc.bytes))
		{
			CryptoStream cryptoStream = null;
			StreamReader reader = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					ms,
					serviceProvider.CreateDecryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Read);
				reader = new StreamReader(cryptoStream);
			}
			else
			{
				reader = new StreamReader(ms);
			}
			string val = reader.ReadToEnd();
			
			reader.Close();
			reader = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}
			
			int.TryParse(val, out crc);
			//Debug.Log("read crc:" + crc.ToString());
		}
		
		//計算CRC.
		int checkcrc = 0;		
		checkcrc = getCRC(mTextAsset.bytes);
		//Debug.Log("checkcrc:" + checkcrc.ToString());
		
		
		if (crc != checkcrc)
		{
			Debug.Log("File check error!");
			return false;
		}

		//讀取資料.
		using (MemoryStream ms = new MemoryStream(mTextAsset.bytes))
		{			
			CryptoStream cryptoStream = null;
			StreamReader reader = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					ms,
					serviceProvider.CreateDecryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Read);
				reader = new StreamReader(cryptoStream);
			}
			else
			{
				reader = new StreamReader(ms);
			}
			
			//處理.
			if (readCB != null)
			{
				readCB(reader);
			}
			
			
			reader.Close();
			reader = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}
		}

		
		
		return true;
	}

	public static bool ReadText(string filepathname, ReadTextCB readCB, bool useCrypto, string Crcfilepathname)
	{
		
		if (!File.Exists(filepathname) || !File.Exists(Crcfilepathname))
		{
			return false;
		}
				
		//取得 CRC.
		int crc = 0;
		using (FileStream fileStream = new FileStream(Crcfilepathname, FileMode.Open, FileAccess.Read))
		{		
			CryptoStream cryptoStream = null;
			StreamReader reader = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					fileStream,
					serviceProvider.CreateDecryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Read);
				reader = new StreamReader(cryptoStream);
			}
			else
			{
				reader = new StreamReader(fileStream);
			}

            string val = "";
            try
            {
                val = reader.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return false;
            }


			reader.Close();
			reader = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}
			
			int.TryParse(val, out crc);
			fileStream.Close();
			//Debug.Log("read crc:" + crc.ToString());
		}
		
		//計算CRC.
		int checkcrc = 0;
		using (FileStream fileStream = new FileStream(filepathname, FileMode.Open, FileAccess.Read))
		{
			//Debug.Log("file length:" + fileStream.Length.ToString());
			byte[] bytes = new byte[fileStream.Length]; 
			fileStream.Read(bytes, 0, (int)fileStream.Length);			
			checkcrc = getCRC(bytes);
			//Debug.Log("checkcrc:" + checkcrc.ToString());

			fileStream.Close();
		}
		
		if ((crc != checkcrc))
		{
			Debug.Log("File check error!");
			return false;
		}
		
		//讀取資料.
		using (FileStream fileStream = new FileStream(filepathname, FileMode.Open, FileAccess.Read))
		{			
			CryptoStream cryptoStream = null;
			StreamReader reader = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					fileStream,
					serviceProvider.CreateDecryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Read);
				reader = new StreamReader(cryptoStream);
			}
			else
			{
				reader = new StreamReader(fileStream);
			}
			
			//處理.
			if (readCB != null)
			{
				readCB(reader);
			}
			
			
			reader.Close();
			reader = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}
			fileStream.Close();
		}
		
		return true;
	}
	
	public static bool ReadBin(string filepathname, ReadBinCB readCB, bool useCrypto, string Crcfilepathname)
	{
		if (!File.Exists(filepathname) || !File.Exists(Crcfilepathname))
		{
			return false;
		}
				
		//取得 CRC.
		int crc = 0;
		using (FileStream fileStream = new FileStream(Crcfilepathname, FileMode.Open, FileAccess.Read))
		{		
			CryptoStream cryptoStream = null;
			BinaryReader reader = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					fileStream,
					serviceProvider.CreateDecryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Read);
				reader = new BinaryReader(cryptoStream);
			}
			else
			{
				reader = new BinaryReader(fileStream);
			}

			crc = reader.ReadInt16();
			
			reader.Close();
			reader = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}

			//Debug.Log("read crc:" + crc.ToString());
		}
		
		//計算CRC.
		int checkcrc = 0;
		using (FileStream fileStream = new FileStream(filepathname, FileMode.Open, FileAccess.Read))
		{			
			byte[] bytes = new byte[fileStream.Length]; 
			fileStream.Read(bytes, 0, (int)fileStream.Length);			
			checkcrc = getCRC(bytes);
			//Debug.Log("checkcrc:" + checkcrc.ToString());
		}
		
		if (useCrypto && (crc != checkcrc))
		{
			Debug.Log("File Check Error!");
			return false;
		}


		//讀取資料.
		using (FileStream fileStream = new FileStream(filepathname, FileMode.Open, FileAccess.Read))
		{			
			CryptoStream cryptoStream = null;
			BinaryReader reader = null;
			if (useCrypto)
			{
				cryptoStream = new CryptoStream(
					fileStream,
					serviceProvider.CreateDecryptor(algorithm.Key, algorithm.IV),
					CryptoStreamMode.Read);
				reader = new BinaryReader(cryptoStream);
			}
			else
			{
				reader = new BinaryReader(fileStream);
			}

			//處理.
			if (readCB != null)
			{
				readCB(reader, fileStream, cryptoStream);
			}
			
			
			reader.Close();
			reader = null;
			if (useCrypto)
			{
				cryptoStream.Close();
				cryptoStream = null;
			}
		}

		return true;
	}
}
