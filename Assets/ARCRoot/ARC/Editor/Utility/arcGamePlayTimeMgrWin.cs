using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class arcGamePlayTimeMgrWin : EditorWindow
{

	static string TimeName = "";
	static TextAsset mTextAsset = null;
	static List<string> RemoveList = new List<string>();
	static string SaveFilePathName = Application.dataPath + "/" + arcMenu.ResourceRoot + arcMenu.ResourceDir +"timemgr.bytes";

	[MenuItem (arcMenu.WindowRoot + "Game/TimeManager")]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		arcGamePlayTimeMgrWin window = (arcGamePlayTimeMgrWin)EditorWindow.GetWindow (typeof (arcGamePlayTimeMgrWin));
		window.Show();
	}
	
	static public void Save()
	{
		//create dir.
		string arcdir = Application.dataPath + "/" + arcMenu.ResourceRoot + arcMenu.ResourceDir;
		if (!System.IO.Directory.Exists(arcdir))
		{
			System.IO.Directory.CreateDirectory(arcdir);
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(SaveFilePathName);
		bf.Serialize(file, arcGamePlayTimeMgr.TimeList);
		file.Close();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

	}
	
	static public void Load()
	{

		mTextAsset = Resources.Load(arcGamePlayTimeMgr.SaveFileName) as TextAsset;
		if (mTextAsset == null)
		{
			Debug.Log("Time data not found: " + arcMenu.ResourceRoot + arcGamePlayTimeMgr.SaveFileName);
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream(mTextAsset.bytes);
		arcGamePlayTimeMgr.TimeList = bf.Deserialize(ms) as Dictionary<string, arcGamePlayTimeMgr.TimeData>;
	}

	
	void OnGUI ()
	{
		GUILayout.Label ("GamePlayTimeMgr", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal();

				
		if (GUILayout.Button("save"))
		{
			Save();
		}
		
		if (GUILayout.Button("load"))
		{
			Load();
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		TimeName = GUILayout.TextField(TimeName);
		if (GUILayout.Button("Add"))
		{
			arcGamePlayTimeMgr.CrateTime(TimeName);
		}
		GUILayout.EndHorizontal();


		//印出.
		GUILayout.BeginVertical();
		Dictionary<string, arcGamePlayTimeMgr.TimeData> TimeList = arcGamePlayTimeMgr.TimeList;
		RemoveList.Clear();
		foreach(KeyValuePair<string, arcGamePlayTimeMgr.TimeData>  kp in TimeList)
		{
			GUILayout.BeginHorizontal();
			kp.Value.Enable = GUILayout.Toggle(kp.Value.Enable, "Enable");
			GUILayout.Label(kp.Value.Name);

			if (GUILayout.Button("X"))
			{
				RemoveList.Add(kp.Value.Name);
			}

			GUILayout.EndHorizontal();
		}		

		for (int i = 0; i < RemoveList.Count; i++)
		{
			TimeList.Remove(RemoveList[i]);
		}
		
		GUILayout.EndVertical();

	}
}
