using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class arcFindObjectWin : EditorWindow
{
	//scroll.
	Vector2 mScrollpos = new Vector2(0, 0);
	int mScrollViewHeight = 300;
	int mScrollViewItemHeight = 20;

	//find.
	class FindData
	{
		public bool enable;
		public string componentName;
		public int guiid;
	}

	static List<FindData> mFindDataList = new List<FindData>();
	
	List<GameObject> mFindObjsH = new List<GameObject>();
	List<GameObject> mFindObjsA = new List<GameObject>();

	[MenuItem (arcMenu.WindowRoot + "Find")]
	static void Init ()
	{

		// Get existing open window or if none, make a new one:
		arcFindObjectWin window = (arcFindObjectWin)EditorWindow.GetWindow (typeof (arcFindObjectWin));


	}

	void AddFindData(string name)
	{
		FindData data = new FindData();
		data.enable = true;
		data.componentName = name;
		mFindDataList.Add(data);
	}

	void RemoveFindDataAt(int idx)
	{
		mFindDataList.RemoveAt(idx);
	}

	void OnGUI ()
	{
		GUILayout.Label ("Find Object With Component", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add component name", GUILayout.Width(200)))
		{
			AddFindData("");
		}
		GUILayout.EndHorizontal();

		//畫出條件.
		int guiidx = 0;
		for (int i = 0; i < mFindDataList.Count; i++)
		{
			GUILayout.BeginHorizontal();
			FindData data = mFindDataList[i];
			data.guiid = guiidx;
			data.enable = GUILayout.Toggle(data.enable, "", GUILayout.Width(20));
			data.componentName = GUILayout.TextArea(data.componentName);
			if (GUILayout.Button("X", GUILayout.Width(20)))
			{
				RemoveFindDataAt(data.guiid);
			}

			guiidx++;
			GUILayout.EndHorizontal();
		}

		//找.
		GUILayout.BeginHorizontal();

		//左.
		GUILayout.BeginVertical();
		if (GUILayout.Button("FindHierachy", GUILayout.Width(300)))
		{
			DoFindObjWithComponent();
		}
		//印出.
		
		mScrollpos = GUILayout.BeginScrollView(mScrollpos, GUILayout.Width(300), GUILayout.Height(mScrollViewHeight));
		
		foreach(GameObject obj in mFindObjsH)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("<", GUILayout.Width(mScrollViewItemHeight), GUILayout.Height(mScrollViewItemHeight)))
			{
				EditorGUIUtility.PingObject(obj);
			}
			
			GUILayout.TextField(obj.name, GUILayout.Width(250));
			GUILayout.EndHorizontal();			
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
		//右.
		GUILayout.BeginVertical();
		if (GUILayout.Button("FindAsset", GUILayout.Width(300)))
		{
			DoFindAssetWithComponent();
		}
		//印出.		
		mScrollpos = GUILayout.BeginScrollView(mScrollpos, GUILayout.Width(300), GUILayout.Height(mScrollViewHeight));
		
		foreach(GameObject obj in mFindObjsA)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("<", GUILayout.Width(mScrollViewItemHeight), GUILayout.Height(mScrollViewItemHeight)))
			{
				EditorGUIUtility.PingObject(obj);
			}
			
			GUILayout.TextField(obj.name, GUILayout.Width(250));
			GUILayout.EndHorizontal();			
		}
		GUILayout.EndScrollView();
		//GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

	}

	void DoFindObjWithComponent()
	{
		mFindObjsH.Clear();
		//所有物件.
		GameObject[] finds = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject obj in finds)
		{
			//所有條件.
			int oks = 0;
			int conds = 0;
			foreach(FindData da in mFindDataList)
			{
				if (da.enable 
				    && (da.componentName != null)
				    && (da.componentName.Length > 0)
				    )
				{
					conds++;
					Component cp = obj.GetComponent(da.componentName);
					if (cp != null)
					{
						oks++;
					}
					else
					{
						break;
					}
				}
			}

			if ((conds > 0) && (oks >= conds))
			{
				mFindObjsH.Add(obj);
			}

		}
	}

	void DoFindAssetWithComponent()
	{
		mFindObjsA.Clear();
		string[] phs = AssetDatabase.GetAllAssetPaths();
		foreach(string ph in phs)
		{
			GameObject obj = AssetDatabase.LoadAssetAtPath(ph, (typeof(GameObject))) as GameObject;
			if (obj != null)
			{
				
				//所有條件.
				int oks = 0;
				int conds = 0;
				foreach(FindData da in mFindDataList)
				{
					if (da.enable 
					    && (da.componentName != null)
					    && (da.componentName.Length > 0)
					    )
					{
						conds++;
						Component cp = obj.GetComponent(da.componentName);
						if (cp != null)
						{
							oks++;
						}
						else
						{
							break;
						}
					}
				}
				
				if ((conds > 0) && (oks >= conds))
				{
					mFindObjsA.Add(obj);
				}
			}
			obj = null;
		}

	}
}
