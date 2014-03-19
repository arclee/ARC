using UnityEditor;
using UnityEngine;
using System.Collections;

[InitializeOnLoad]
public static class arcEditorUtility
{
	
	[MenuItem(arcMenu.WindowRoot + "About", false, 14000)]
	public static void About2DToolkit()
	{
		EditorUtility.DisplayDialog("About ARC",
		                            "Arc's package for game!",
		                            "Ok");
	}
}
