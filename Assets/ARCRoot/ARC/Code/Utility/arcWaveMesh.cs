using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class arcWaveMesh : MonoBehaviour {
	
	//效能.
	public bool RecaluteNormal = false;

	//頂點.
	private Vector3[] meshVertices;
	private Vector3[] origVerticesPos; 

	//設定.
	public int[] MoveVertices; //for my mesh, I used 0 and 1 (upper two vertices)
	public Vector3 WaveSpeed = new Vector3(0, 0, 0);
	public Vector3 WaveAmplitude = new Vector3(0, 0, 0);

	private Vector3 WaveRandomize = new Vector3(0, 0, 0);	
	private Vector3 WaveMove = new Vector3(0, 0, 0);
	private Vector3 v3wave;
	private Mesh mesh = null;


	//scale.
	public float Scalespeed = 1.0f;
	public float ScaleAmplitude = 0;
	private float ScaleRandomize = 0;


	void Start () {
		
		MeshFilter myMF = this.GetComponent("MeshFilter") as MeshFilter;

		if (myMF == null)
		{
			return;
		}

		mesh = myMF.mesh;
		//not reference. copy.
		meshVertices = mesh.vertices;
		origVerticesPos = mesh.vertices;
		WaveRandomize.x = Random.Range(0, 359);
		WaveRandomize.y = Random.Range(0, 359);
		WaveRandomize.z = Random.Range(0, 359);
		ScaleRandomize = Random.Range(0, 359);
	}
	
	void Update ()
	{
		float totaltime = arcGamePlayTimeMgr.GetTotalTime("Game");
		if (mesh == null)
		{
			return;
		}

		//效能.
		if (!renderer.isVisible)
		{
			return;
		}
		//(-1 ~ 1).
		WaveMove.x = Mathf.Sin((totaltime * WaveSpeed.x) + WaveRandomize.x);
		WaveMove.y = Mathf.Sin((totaltime * WaveSpeed.y) + WaveRandomize.y);
		WaveMove.z = Mathf.Sin((totaltime * WaveSpeed.z) + WaveRandomize.z);
		//幅.
		v3wave = new Vector3(WaveMove.x * WaveAmplitude.x, WaveMove.y * WaveAmplitude.y, WaveMove.z * WaveAmplitude.z);

		//scale.
		float scaleMove = Mathf.Sin((totaltime * Scalespeed) + ScaleRandomize);

		//修改.
		for (int i = 0; i < MoveVertices.Length; i++)
		{
			int idx = MoveVertices[i];
			//scale. 以位置為中心.
			Vector3 leng = (transform.TransformPoint(origVerticesPos[idx]) - transform.position) * scaleMove * ScaleAmplitude;
			//wave.
			meshVertices[idx] = origVerticesPos[idx] + v3wave + leng;
		}

		//給值.
		mesh.vertices = meshVertices;

		if (RecaluteNormal)
		{
			mesh.RecalculateNormals();
		}
	}
}
