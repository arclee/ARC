using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;
using  System.Runtime.Serialization.Formatters.Binary;
public class arcGamePlayTimeMgr : arcSingleton<arcGamePlayTimeMgr>
{
	
	[Serializable]
	public class TimeData
	{
		public bool Enable;
		public string Name;
		public float DeltaTime;
		public float TotalTime;
		//多人操作暫停時, 以最後一個人解除為主.
		public int pauseCount;
		public float scale;
		public TimeData(string name)
		{
			Name = name;
			Reset();
		}

		public void Reset()
		{
			Enable = true;
			DeltaTime = 0;
			TotalTime = 0;
			pauseCount = 0;
			scale = 1;
		}

		public void PauseWithCount()
		{
			DeltaTime = 0;
			pauseCount++;
		}

		public void PlayWithCount()
		{
			pauseCount--;
			pauseCount = Math.Max(pauseCount, 0);
		}

		public bool isPlaying()
		{
			return pauseCount == 0;
		}
	}
	
	static TextAsset mTextAsset = null;
	static public string SaveFileName = "ARC/timemgrtext";

	static int version = 1;
	public static Dictionary<string, TimeData> TimeList = new Dictionary<string, TimeData>();
	
	private arcGamePlayTimeMgr()
	{
		
	}

	static public void Load()
	{
		LoadXML();
	}

	static void LoadBin()
	{
		//not work on web.
//		mTextAsset = Resources.Load(SaveFileName) as TextAsset;
//		if (mTextAsset == null)
//		{
//			return;
//		}
//		BinaryFormatter bf = new BinaryFormatter();
//		MemoryStream ms = new MemoryStream(mTextAsset.bytes);
//		arcGamePlayTimeMgr.TimeList = bf.Deserialize(ms) as Dictionary<string, arcGamePlayTimeMgr.TimeData>;

	}

	static public void Save(string SaveFilePathName)
	{
		arcCrypto.WriteText(SaveFilePathName + ".bytes", SaveXMLCB, false, SaveFilePathName + "c.bytes");
	}

	static void SaveXMLCB(StreamWriter writer)
	{

		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.Encoding = System.Text.Encoding.UTF8;
		
		
		//寫檔.		
		XmlWriter xmlwriter = XmlWriter.Create(writer, settings);
		xmlwriter.WriteStartDocument();		
		xmlwriter.WriteStartElement("arcGamePlayTimeMgr");
		xmlwriter.WriteAttributeString("version", version.ToString());		

		
		foreach(KeyValuePair<string, TimeData>  kp in TimeList)
		{
			xmlwriter.WriteStartElement("Data");
			xmlwriter.WriteAttributeString("name", kp.Value.Name);
			xmlwriter.WriteAttributeString("scale", kp.Value.scale.ToString());
			xmlwriter.WriteEndElement();//Data.

		}	

		
		
		xmlwriter.WriteEndElement();//arcGamePlayTimeMgr.		
		xmlwriter.WriteEndDocument();
		
		xmlwriter.Close();
		xmlwriter = null;
		
		writer.Flush();
		
		Debug.Log("XML write finish");

	}
	
	public static void LoadXML()
	{
		arcCrypto.ReadTextFromResouce(SaveFileName, LoadXMLCB, false, SaveFileName + "c");


	}

	static void LoadXMLCB(StreamReader reader)
	{
		TimeList.Clear();
		XmlReader xr = XmlReader.Create(reader);		
		XmlParseTimeMgr(xr);
		xr.Close();
		//Debug.Log("XML Read finish");
	}
	
	static void XmlParseTimeMgr(XmlReader xr)
	{
		while(xr.Read())
		{
			if (xr.IsStartElement())
			{
				//Debug.Log(xr.Name + " " + xr.Value);
				if (xr.NodeType == XmlNodeType.Element)
				{
					if (xr.Name == "arcGamePlayTimeMgr")
					{
						//Debug.Log("Element:" + xr.NodeType + " name:" + xr.Name);
						while (xr.MoveToNextAttribute())
						{
//							if (xr.Name == "version")
//							{
//								Debug.Log(xr.Name + ":" + xr.Value);
//							}
						}

					}
					
					xr.MoveToElement();		

					XmlReader xrchild = xr.ReadSubtree();
					xrchild.Read();
					XmlParseData(xrchild);
						
				}
			}
		}		
	}
	
	static void XmlParseData(XmlReader xr)
	{
		while(xr.Read())
		{
			if (xr.IsStartElement())
			{
				//Debug.Log(xr.Name + " " + xr.Value);
				if (xr.NodeType == XmlNodeType.Element)
				{
					if (xr.Name == "Data")
					{
						TimeData td = new TimeData("");

						//Debug.Log("Element:" + xr.NodeType + " name:" + xr.Name);
						while (xr.MoveToNextAttribute())
						{
							if (xr.Name == "name")
							{
								td.Name = xr.Value;
							}

							if (xr.Name == "scale")
							{
								float.TryParse(xr.Value, out td.scale);
							}
						}

						TimeList.Add(td.Name, td);
					}
				}
			}
		}

	}

	void Awake()
	{
		Load();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach(KeyValuePair<string, TimeData>  kp in TimeList)
		{
			if (kp.Value.Enable && kp.Value.isPlaying())
			{
				kp.Value.DeltaTime = Time.deltaTime * kp.Value.scale;
				kp.Value.TotalTime += kp.Value.DeltaTime;
			}
			else
			{
				kp.Value.DeltaTime = 0;
			}
		}
	}

	static public TimeData CrateTime(string name)
	{
		TimeData td = GetTimeData(name);

		if (td != null)
		{
			return td;
		}

		TimeData timedata = new TimeData(name);

		TimeList.Add(name, timedata);

		return timedata;
	}

	static public TimeData GetTimeData(string name)
	{
		if (TimeList.ContainsKey(name))
		{
			return TimeList[name];
		}

		return null;
	}
	
	static public float GetDeltaTime(string name)
	{
		TimeData td = GetTimeData(name);
		
		if (td != null)
		{
			return td.DeltaTime;
		}
		
		Debug.Log("Time not found:" + name);

		return 0;
	}

	
	static public float GetTotalTime(string name)
	{
		TimeData td = GetTimeData(name);
		
		if (td != null)
		{
			return td.TotalTime;
		}
		
		Debug.Log("Time not found:" + name);

		return 0;
	}

	static public void SetEnable(string name, bool enable)
	{
		TimeData td = GetTimeData(name);
		if (td != null)
		{
			td.Enable = enable;
		}
	}

	static public void SetScale(string name, float scale)
	{
		TimeData td = GetTimeData(name);

		if (td != null)
		{
			td.scale = scale;
		}
	}

	static public void GetBackupEnable(ref Dictionary<string, bool> backuplist)
	{
		backuplist.Clear();

		foreach (KeyValuePair<string, TimeData> kp in TimeList)
		{
			backuplist.Add(kp.Key, kp.Value.Enable);
		}
	}

	static public void SetRestoreEnable(ref Dictionary<string, bool> backuplist)
	{
		foreach (KeyValuePair<string, bool> kp in backuplist)
		{
			SetEnable(kp.Key, kp.Value);
		}

	}

}
