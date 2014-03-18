using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(arcErrCollector))]
public class arcErrCollectorEditor : Editor
{

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
		arcErrCollector.mEnable = GUILayout.Toggle(arcErrCollector.mEnable, "Enable");

		if (GUILayout.Button("Clear log"))
		{
			arcErrCollector.Clear();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		scrollpos = GUILayout.BeginScrollView(scrollpos);
	
		for (int i = 0; i < arcErrCollector.mNullErrorObjs.Count; i++)
		{			
			if (GUILayout.Button(arcErrCollector.mNullErrorObjs[i].msg))
			{
				EditorGUIUtility.PingObject(arcErrCollector.mNullErrorObjs[i].obj);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();

		 
		//update and redraw:
//		if(GUI.changed){
//			EditorUtility.SetDirty(mtarget);			
//		}
	}

	
	[MenuItem(arcMenu.GameObjectRoot + "Debug/arcErrCollector", false, 13000)]
	static void DoCreateSpriteObject()
	{
		
		GameObject go = new GameObject("arcErrCollector");
		go.AddComponent<arcErrCollector>();
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create arcErrCollector");
		
	}
}
