using UnityEngine;
using System.Collections;

public class gameEffMgr : MonoBehaviour {
	
	public static gameEffMgr instance;
	public GameObject[] effGameObjects;
    public bool useGameObjPoolMgr = true;
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
        GameObject igo = null;

        if (useGameObjPoolMgr)
        {
            igo = gameObjPoolMgr.Instance.GetObjPrefab(effGameObjects[idx]);
        }
        else
        {
            igo = sceneObjPoolMgr.Instance.GetObjPrefab(effGameObjects[idx]);
        }

		igo.transform.position = pos;
		return igo;

	}

	public GameObject ShowEffect(int idx, Vector3 pos, string func, object parm)
	{
        GameObject igo = null;

        if (useGameObjPoolMgr)
        {
            igo = gameObjPoolMgr.Instance.GetObjPrefab(effGameObjects[idx]);
        }
        else
        {
            igo = sceneObjPoolMgr.Instance.GetObjPrefab(effGameObjects[idx]);
        }

		igo.transform.position = pos;
		igo.SendMessage(func, parm);
		return igo;
	}

    public void RestoreEffect(GameObject go)
    {
        if (useGameObjPoolMgr)
        {
            gameObjPoolMgr.Instance.RestoreObj(go);
        }
        else
        {
            sceneObjPoolMgr.Instance.RestoreObj(go);
        }
    }

	public void DestoryAll()
    {
        if (useGameObjPoolMgr)
        {
            gameObjPoolMgr.Instance.DestoryAll();
        }
        else
        {
            sceneObjPoolMgr.Instance.DestoryAll();
        }
	}
}
