using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(arcMeshCombine))]
public class arcMeshCombineEditor : Editor 
{
	public bool saveMesh = false;

	void Start ()
	{
		
	}
	
	override public void OnInspectorGUI()
	{

		DrawDefaultInspector();

		saveMesh = EditorGUILayout.Toggle("Save Mesh", saveMesh);

		if (GUILayout.Button("Combine"))
		{
			Combine();
		}


	}
		
	void Combine()
	{
		arcMeshCombine tar = (arcMeshCombine)target;
		
		List<MeshRenderer> renders = new List<MeshRenderer>();
		MeshFilter[] meshFilters = tar.GetComponentsInChildren<MeshFilter>();


		
		// figure out array sizes
		int vertCount = 0;
		int normCount = 0;
		int triCount = 0;
		int uvCount = 0;
		
		for (int i = 0; i < meshFilters.Length; i++)
		{
			MeshRenderer mrd = meshFilters[i].gameObject.GetComponent<MeshRenderer>();
			renders.Add(mrd);

			vertCount += meshFilters[i].sharedMesh.vertices.Length; 
			normCount += meshFilters[i].sharedMesh.normals.Length;
			triCount += meshFilters[i].sharedMesh.triangles.Length; 
			uvCount += meshFilters[i].sharedMesh.uv.Length;
		}

		//material.
		int matcount = 0;
		for (int i = 0; i < renders.Count; i++)
		{
			matcount += renders[i].sharedMaterials.Length;
		}
		Material[] setmats = new Material[matcount];
		int matidx = 0;
		for (int i = 0; i < renders.Count; i++)
		{
			for (int j = 0; j < renders[i].sharedMaterials.Length; j++)
			{
				setmats[matidx++] = renders[i].sharedMaterials[j];
			}
		}

		// allocate arrays
		Vector3[] verts = new Vector3[vertCount];
		Vector3[] norms = new Vector3[normCount];
		Transform[] aBones = new Transform[meshFilters.Length];
		Matrix4x4[] bindPoses = new Matrix4x4[meshFilters.Length];
		BoneWeight[] weights = new BoneWeight[vertCount];
		int[] tris  = new int[triCount];
		Vector2[] uvs = new Vector2[uvCount];
		
		int vertOffset = 0;
		int normOffset = 0;
		int triOffset = 0;
		int uvOffset = 0;
		int meshOffset = 0;
		
		// merge the meshes and set up bones
		List<List<int>> subtris = new List<List<int>>();
		foreach(MeshFilter mf in meshFilters)
		{
			List<int> subtri = new List<int>();

			foreach(int i in mf.sharedMesh.triangles)
			{
				tris[triOffset] = i + vertOffset;
				subtri.Add(tris[triOffset]);
				triOffset++;
			}
			
			aBones[meshOffset] = mf.transform;
			bindPoses[meshOffset] = Matrix4x4.identity;
			
			foreach(Vector3 v in mf.sharedMesh.vertices)
			{
				weights[vertOffset].weight0 = 1.0f;
				weights[vertOffset].boneIndex0 = meshOffset;
				verts[vertOffset++] = v;
			}
			
			foreach(Vector3 n in mf.sharedMesh.normals)
				norms[normOffset++] = n;
			
			foreach(Vector2 uv in mf.sharedMesh.uv)
				uvs[uvOffset++] = uv;
			
			meshOffset++;
			
			MeshRenderer mr = 
				mf.gameObject.GetComponent(typeof(MeshRenderer)) 
					as MeshRenderer;
			
			if(mr)
			{
				mr.enabled = false;
			}

			subtris.Add(subtri);
		}
		
		// hook up the mesh
		Mesh me = new Mesh();       
		me.name = tar.gameObject.name;
		me.vertices = verts;
		me.normals = norms;
		me.boneWeights = weights;
		me.uv = uvs;
		me.triangles = tris;
		me.bindposes = bindPoses;
		
		// hook up the mesh renderer        
		SkinnedMeshRenderer smr = 
			tar.gameObject.AddComponent(typeof(SkinnedMeshRenderer)) 
				as SkinnedMeshRenderer;

		me.subMeshCount = subtris.Count;
		for (int i = 0; i < subtris.Count; i++)
		{
			me.SetTriangles(subtris[i].ToArray(), i);
		}
		me.RecalculateBounds();

		smr.sharedMesh = me;
		smr.bones = aBones;
		smr.sharedMaterials = setmats;
	}

}
