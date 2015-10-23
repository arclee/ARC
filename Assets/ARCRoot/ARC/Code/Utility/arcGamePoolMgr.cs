using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class arcGamePoolMgr<T> : MonoBehaviour where T : MonoBehaviour
{
	
	static  public arcGamePoolMgr<T> Instance;

	public bool Grawable = false;
	public int MaxObj = 10;
	public int debugid = 0;
	
		
	protected Dictionary<string, GameObjPool> pooldict = new Dictionary<string, GameObjPool>();
	
	public string[] prefabNames;
	
	void Awake()
	{

		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
		
	}
	
	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < prefabNames.Length; i++)
		{
			if (!pooldict.ContainsKey(prefabNames[i]))
			{
				GameObjPool objpool = new GameObjPool();
				objpool.grawable = Grawable;
				objpool.maxobj = MaxObj;
				objpool.prefabName = prefabNames[i];
				objpool.PreCreate(gameObject);
				pooldict.Add(prefabNames[i], objpool);
			}
		}
		
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{	
	}
	
	public GameObject GetObjRes(string prefabname)
	{
		if (pooldict.ContainsKey(prefabname))
		{
			return pooldict[prefabname].GetObject();
		}
		else
		{
			//Debug.Log("Prefab pool not found! Create New: " + prefabname);
			
			GameObjPool objpool = new GameObjPool();
			objpool.grawable = Grawable;
			objpool.maxobj = MaxObj;
			objpool.prefabName = prefabname;
			objpool.PreCreate(gameObject);
			pooldict.Add(prefabname, objpool);
			return pooldict[prefabname].GetObject();
		}
		
		//return null;
	}

	public GameObject GetObjPrefab(GameObject prefab)
	{
		if (pooldict.ContainsKey(prefab.GetHashCode().ToString()))
		{
			return pooldict[prefab.GetHashCode().ToString()].GetObject();
		}
		else
		{
			Debug.Log("Prefab pool not found! Create New: " + prefab.name);
			
			GameObjPool objpool = new GameObjPool();
			objpool.grawable = Grawable;
			objpool.maxobj = MaxObj;
			objpool.prefabName = prefab.name;
			objpool.PreCreatePrefab(prefab, gameObject);
			pooldict.Add(prefab.GetHashCode().ToString(), objpool);
			return pooldict[prefab.GetHashCode().ToString()].GetObject();
		}

	}

	public void RestoreObj(GameObject obj)
	{
		obj.SetActive(false);
	}
	
	public void DisableAll()
	{
		foreach(KeyValuePair<string, GameObjPool> pa in pooldict)
		{
			pa.Value.DisableAll();
		}
	}



}
