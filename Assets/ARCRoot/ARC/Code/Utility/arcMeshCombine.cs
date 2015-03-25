using UnityEngine;
using System;
using System.Collections.Generic;

using UnityEditor;
// 修改自 http://wiki.unity3d.com/index.php?title=MeshMerger

//==============================================================================
[ExecuteInEditMode]
public class arcMeshCombine : MonoBehaviour 
{ 
	void Start () 
	{ 
	}

	/* unity 的 CombineMeshes 方法 skin 有問題.
	public void CombineSubMesh()
	{

		List<MeshRenderer> renders = new List<MeshRenderer>();
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length)
		{
			MeshRenderer mrd = meshFilters[i].gameObject.GetComponent<MeshRenderer>();
			renders.Add(mrd);
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.active = false;
			i++;
		}

		int matcount = 0;
		for (i = 0; i < renders.Count; i++)
		{
			matcount += renders[i].sharedMaterials.Length;
		}
		Material[] setmats = new Material[matcount];
		int matidx = 0;
		for (i = 0; i < renders.Count; i++)
		{
			for (int j = 0; j < renders[i].sharedMaterials.Length; j++)
			{
				setmats[matidx++] = renders[i].sharedMaterials[j];		
			}
		}


		gameObject.AddComponent(typeof(MeshFilter));
		gameObject.AddComponent(typeof(MeshRenderer));
		transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
		transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine, false);
		transform.GetComponent<MeshRenderer>().sharedMaterials = setmats;

		transform.gameObject.active = true;

		Debug.Log(transform.GetComponent<MeshFilter>().sharedMesh.subMeshCount);
		AssetDatabase.CreateAsset(transform.GetComponent<MeshFilter>().sharedMesh, "Assets/cobmmeshUnity.asset");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}*/
}