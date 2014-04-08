using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class arcFindObjectWin : EditorWindow
{
	//progres bar.
	arcCancelableProgressBar mPorgBar = new arcCancelableProgressBar("Finding", "Click \"Cancel\" to stop");

	//scroll.
	Vector2 mScrollpos1 = new Vector2(0, 0);
	Vector2 mScrollpos2 = new Vector2(0, 0);
//	int mScrollViewHeight = 300;
	int mScrollViewItemHeight = 20;
	int m_ViewCount = 15;
	
	//find.
	string [] mFindTypeStrs = { "Component", "Tag", "Layer", "SortingLayer"};

	class FindData
	{
		public int typeidx;
		public bool enable;
		public string name;
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
		window.Show();

	}

	void AddFindData(string name)
	{
		FindData data = new FindData();
		data.enable = true;
		data.name = name;
		data.typeidx = 0;
		mFindDataList.Add(data);
	}

	void RemoveFindDataAt(int idx)
	{
		mFindDataList.RemoveAt(idx);
	}

	void OnGUI ()
	{
		GUILayout.Label ("Find Object With ...", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Condition", GUILayout.Width(200)))
		{
			AddFindData("");
		}
		if (GUILayout.Button("Clear", GUILayout.Width(200)))
		{
			mFindDataList.Clear();			
			mFindObjsH.Clear();
			mFindObjsA.Clear();

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
			data.name = EditorGUILayout.TextField(data.name);

			data.typeidx = EditorGUILayout.Popup(data.typeidx, mFindTypeStrs, GUILayout.Width(100));

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
			DoFindObjInHierarchy();
		}
		//印出.
		
//		mScrollpos1 = GUILayout.BeginScrollView(mScrollpos1, GUILayout.Width(300), GUILayout.Height(mScrollViewHeight));
//		Debug.Log (mScrollpos1);
//		foreach(GameObject obj in mFindObjsH)
//		{
//			GUILayout.BeginHorizontal();
//			if (GUILayout.Button("<", GUILayout.Width(mScrollViewItemHeight), GUILayout.Height(mScrollViewItemHeight)))
//			{
//				EditorGUIUtility.PingObject(obj);
//			}
//			
//			EditorGUILayout.TextField(obj.name, GUILayout.Width(250));
//			GUILayout.EndHorizontal();			
//		}
//		GUILayout.EndScrollView();

		mScrollpos1 = GUILayout.BeginScrollView(mScrollpos1, GUILayout.Width(300));
		int FirstIndex = (int)(mScrollpos1.y / mScrollViewItemHeight);
		int itemcount = mFindObjsH.Count;
		FirstIndex = Mathf.Clamp(FirstIndex, 0, Mathf.Max(0, itemcount - m_ViewCount));
		GUILayout.Space(FirstIndex * mScrollViewItemHeight);
		for(int i = FirstIndex; i < Mathf.Min(itemcount, FirstIndex + m_ViewCount); i++)
		{
			GameObject obj = mFindObjsH[i];
			if (obj == null)
			{
				continue;
			}
			GUILayout.BeginHorizontal();
			
			if (GUILayout.Button("<", GUILayout.Width(mScrollViewItemHeight), GUILayout.Height(mScrollViewItemHeight)))
			{
				EditorGUIUtility.PingObject(obj);
			}			
			EditorGUILayout.TextField(obj.name, GUILayout.Width(250));
			GUILayout.EndHorizontal();
		}		
		GUILayout.Space(Mathf.Max(0,(itemcount - FirstIndex - m_ViewCount) * mScrollViewItemHeight));
		GUILayout.EndScrollView();


		GUILayout.EndVertical();
		//右.
		GUILayout.BeginVertical();
		if (GUILayout.Button("FindAsset", GUILayout.Width(300)))
		{
			DoFindObjInAsset();
		}
		//印出.
//		mScrollpos2 = GUILayout.BeginScrollView(mScrollpos2, GUILayout.Width(300), GUILayout.Height(mScrollViewHeight));		
//		foreach(GameObject obj in mFindObjsA)		
		mScrollpos2 = GUILayout.BeginScrollView(mScrollpos2, GUILayout.Width(300));
		FirstIndex = (int)(mScrollpos2.y / mScrollViewItemHeight);
		itemcount = mFindObjsH.Count;
		FirstIndex = Mathf.Clamp(FirstIndex, 0, Mathf.Max(0, itemcount - m_ViewCount));
		GUILayout.Space(FirstIndex * mScrollViewItemHeight);
		for(int i = FirstIndex; i < Mathf.Min(mFindObjsA.Count, FirstIndex + m_ViewCount); i++)
		{
			GameObject obj = mFindObjsA[i];
			if (obj == null)
			{
				continue;
			}

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("<", GUILayout.Width(mScrollViewItemHeight), GUILayout.Height(mScrollViewItemHeight)))
			{
				Transform ts = obj.transform.root.transform.Find(obj.name);

				//找後到就接PING.因為 Asset 視窗只看得到一層 child 而已.
				if (ts != null && ts.gameObject == obj)
				{
					EditorGUIUtility.PingObject(obj);
				}
				else
				{
					//在很裡面就 Ping root.
					EditorGUIUtility.PingObject(obj.transform.root.gameObject);
				}
			}



			EditorGUILayout.TextField(obj.name, GUILayout.Width(250));
			GUILayout.EndHorizontal();			
		}
		GUILayout.Space(Mathf.Max(0,(itemcount - FirstIndex - m_ViewCount) * mScrollViewItemHeight));
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

	}


	bool CheckFindData(FindData da, GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		// { "Component", "Tag", "Layer"};

		switch (da.typeidx)
		{
		case 0 :
		{
			Component cp = obj.GetComponent(da.name);
			if (cp != null)
			{
				return true;
			}
			break;
		}
		case 1 :
		{
			if (obj.tag == da.name)
			{
				return true;
			}
			break;
		}
		case 2 :
		{
			int layermask = LayerMask.NameToLayer(da.name);
			if (obj.IsInLayerMask(1 << layermask))
			{
				return true;
			}
			break;
		}
		case 3 :
		{
			Renderer rd = obj.GetComponent(typeof(Renderer)) as Renderer;
			if (rd != null)
			{
				if (rd.sortingLayerName == da.name)
				{
					return true;
				}
			}
			break;
		}

		}

		return false;

	}
	
	void CheckFindDatasRecursive(GameObject obj, List<GameObject> refList)
	{
		if (obj == null)
		{
			return;
		}

		CheckFindDatas(obj, refList);
		foreach (Transform ts in obj.transform)
		{
			CheckFindDatasRecursive(ts.gameObject, refList);
		}
	}

	void CheckFindDatas(GameObject obj, List<GameObject> refList)
	{
		if (obj == null)
		{
			return;
		}

		//所有條件.
		int oks = 0;
		int conds = 0;
		foreach(FindData da in mFindDataList)
		{
			if (da.enable 
			    && (da.name != null)
			    && (da.name.Length > 0)
			    )
			{
				conds++;
				
				if (CheckFindData(da, obj))
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
			refList.Add(obj);
		}

	}

	void DoFindObjInHierarchy()
	{
		mFindObjsH.Clear();
		//所有物件.
		GameObject[] finds = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int totalcount = finds.Length;
		int porced = 0;
		mPorgBar.Show();
		foreach(GameObject obj in finds)
		{
			CheckFindDatas(obj, mFindObjsH);
			porced++;
			if (mPorgBar.Update(porced, totalcount))
			{
				break;
			}
		}

		mPorgBar.Close();
	}

	void DoFindObjInAsset()
	{
		mFindObjsA.Clear();
		string[] phs = AssetDatabase.GetAllAssetPaths();
		int totalcount = phs.Length;
		int porced = 0;
		mPorgBar.Show();
		foreach(string ph in phs)
		{
			porced++;
			GameObject obj = AssetDatabase.LoadAssetAtPath(ph, (typeof(GameObject))) as GameObject;
			if (obj != null)
			{
				//所有GameObj.
				CheckFindDatasRecursive(obj, mFindObjsA);
			}
			if (mPorgBar.Update(porced, totalcount))
			{
				break;
			}
			obj = null;
		}
		
		mPorgBar.Close();
	}

}
