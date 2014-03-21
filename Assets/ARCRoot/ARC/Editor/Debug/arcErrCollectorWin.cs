using UnityEngine;
using UnityEditor;
using System.Collections;

//開一個 window 顯示 log 資料.
public class arcErrCollectorWin : EditorWindow
{
	Vector2 scrollpos = new Vector2(0, 0);

	int scrollViewHeight = 300;
	int scrollViewItemHeight = 20;
	int m_ViewCount = 15;

	[MenuItem (arcMenu.WindowRoot + "Debug/ErrCollection")]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		arcErrCollectorWin window = (arcErrCollectorWin)EditorWindow.GetWindow (typeof (arcErrCollectorWin));
		window.Show();
	}

	void OnGUI (){
		GUILayout.Label ("Error Logs", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal();
		//enable.
		arcErrCollector.mEnable = GUILayout.Toggle(arcErrCollector.mEnable, "Enable");
		
		//清 log.
		if (GUILayout.Button("Clear log"))
		{
			arcErrCollector.Clear();
		}
		GUILayout.EndHorizontal();
		
		//log.
		GUILayout.BeginHorizontal();
		
//		scrollpos = GUILayout.BeginScrollView(scrollpos, GUILayout.Height(scrollViewHeight));		
//		for (int i = 0; i < arcErrCollector.mNullErrorObjs.Count; i++)
//		{
		
		scrollpos = GUILayout.BeginScrollView(scrollpos, true, true);
		//總數.
		int itemcount = arcErrCollector.mNullErrorObjs.Count;
		//依 scroll 值置算出該印出第幾個.
		int FirstIndex = (int)(scrollpos.y / scrollViewItemHeight);
		//防止爆表.
		FirstIndex = Mathf.Clamp(FirstIndex, 0, Mathf.Max(0, itemcount - m_ViewCount));
		//scroll view 位置前補空白(不印資料加快效能).
		GUILayout.Space(FirstIndex * scrollViewItemHeight);
		//再接著才印資料.
		for(int i = FirstIndex; i < Mathf.Min(itemcount, FirstIndex + m_ViewCount); i++)
		{
			arcErrCollector.ErrorData ed = arcErrCollector.mNullErrorObjs[i];

			//GUILayout.BeginVertical("box", GUILayout.Height(scrollViewItemHeight));			
			GUILayout.BeginHorizontal();
			//hight light 物件.
			if (GUILayout.Button("<", GUILayout.Width(scrollViewItemHeight), GUILayout.Height(scrollViewItemHeight)))
			{
				if (ed.obj != null)
				{
					EditorGUIUtility.PingObject(ed.obj);
				}
				if (ed.filepathname != null)
				{
					//string asp = Application.dataPath;
					//string uuu = asp.Replace('/', '\\');
					int idx = ed.filepathname.LastIndexOf(@"\Assets\");
					//string bb = ed.filepathname.Replace(uuu, "");
					string ass = ed.filepathname.Substring(idx + 1);
					//int ed.filepathname
					//System.String asspath = ed.filepathname.last;
					//Object ob = AssetDatabase.LoadAssetAtPath(ed.filepathname, (typeof(Object))) as Object;
					//載入 asset.
					Object ob = AssetDatabase.LoadAssetAtPath(ass, (typeof(Object))) as Object;
					Selection.activeObject = ob;
					//ping 物件.
					EditorGUIUtility.PingObject(ob);
					ob = null;
					
				}
			}
			GUILayout.TextField(ed.msg);
			GUILayout.EndHorizontal();
			//GUILayout.EndVertical();



		}
		GUILayout.Space(Mathf.Max(0,(itemcount-FirstIndex-m_ViewCount) * scrollViewItemHeight));
		GUILayout.EndScrollView();

		GUILayout.EndHorizontal();
	}
}
