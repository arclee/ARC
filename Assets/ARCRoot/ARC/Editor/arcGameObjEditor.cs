using UnityEditor;
using UnityEngine;
using System.Collections;

public class arcGameObjEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	[MenuItem("GameObject/Create Other/ARC/GameObject", false, 13000)]
	static void DoCreateSpriteObject()
	{
		
		GameObject go = new GameObject("arcGameObject");
		arcGameObject script = go.AddComponent<arcGameObject>();
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create arcGameObject");

	}
}
