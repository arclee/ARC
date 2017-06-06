using UnityEngine;
using System.Collections;

public class gameEffMgr : MonoBehaviour {
	
	public static gameEffMgr instance;
	public GameObject[] effGameObjects;
	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public GameObject ShowEffect(int idx, Vector3 pos)
	{
		GameObject igo = gameObjPoolMgr.Instance.GetObjPrefab(effGameObjects[idx]);
		igo.transform.position = pos;
		return igo;

	}

	public GameObject ShowEffect(int idx, Vector3 pos, string func, object parm)
	{
		GameObject igo = gameObjPoolMgr.Instance.GetObjPrefab(effGameObjects[idx]);
		igo.transform.position = pos;
		igo.SendMessage(func, parm);
		return igo;
	}

	public void DestoryAll()
	{
		gameObjPoolMgr.Instance.DestoryAll();
	}
}
