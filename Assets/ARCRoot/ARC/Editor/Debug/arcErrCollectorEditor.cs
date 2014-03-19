using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(arcErrCollector))]
public class arcErrCollectorEditor : Editor
{
	//只顯示 enable.
#if null
	Vector2 scrollpos;
	//arcNullErrCollector mtarget = null;

	public void OnEnable()
	{
		scrollpos = Vector2.zero;
		//mtarget = (arcNullErrCollector)target;

	}

	public override void OnInspectorGUI ()
	{

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
		scrollpos = GUILayout.BeginScrollView(scrollpos);
	
		for (int i = 0; i < arcErrCollector.mNullErrorObjs.Count; i++)
		{
			arcErrCollector.ErrorData ed = arcErrCollector.mNullErrorObjs[i];
			if (GUILayout.Button(ed.msg) && (ed.obj != null))
			{
				EditorGUIUtility.PingObject(ed.obj);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();

		 
		//update and redraw:
//		if(GUI.changed){
//			EditorUtility.SetDirty(mtarget);			
//		}
	}


#else

	public override void OnInspectorGUI ()
	{
		//enable.
		arcErrCollector.mEnable = GUILayout.Toggle(arcErrCollector.mEnable, "Enable");

	}
#endif

	
	[MenuItem(arcMenu.GameObjectRoot + "Debug/arcErrCollector", false, 13000)]
	static void DoCreateSpriteObject()
	{
		
		GameObject go = new GameObject("arcErrCollector");
		go.AddComponent<arcErrCollector>();
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create arcErrCollector");
		
	}
}
