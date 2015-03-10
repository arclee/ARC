using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		public int PauseCount;
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
			PauseCount = 0;
		}

		public void PauseWithCount()
		{
			DeltaTime = 0;
			PauseCount++;
		}

		public void PlayWithCount()
		{
			PauseCount--;
			PauseCount = Math.Max(PauseCount, 0);
		}

		public bool isPlaying()
		{
			return PauseCount == 0;
		}
	}
	
	static TextAsset mTextAsset = null;
	static public string SaveFileName = "ARC/timemgr";
	public static Dictionary<string, TimeData> TimeList = new Dictionary<string, TimeData>();
	
	private arcGamePlayTimeMgr()
	{
		
	}

	static public void Load()
	{
		mTextAsset = Resources.Load(SaveFileName) as TextAsset;
		if (mTextAsset == null)
		{
			return;
		}
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream(mTextAsset.bytes);
		arcGamePlayTimeMgr.TimeList = bf.Deserialize(ms) as Dictionary<string, arcGamePlayTimeMgr.TimeData>;

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
				kp.Value.DeltaTime = Time.deltaTime;
				kp.Value.TotalTime += Time.deltaTime;
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
	

}
