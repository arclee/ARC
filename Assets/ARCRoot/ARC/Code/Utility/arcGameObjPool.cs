using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjPool
{

	public string prefabName = "";
	public GameObject templatePrefabObj;
	public bool grawable = true;
	public int maxobj = 10;	
	public List<GameObject> pool = new List<GameObject>();
	int poolidx = 0;
	
	GameObject gameObject;
	//GameObject parentGameObject;
	// Use this for initialization

	void Inital()
	{
		if (gameObject == null)
		{
			gameObject = new GameObject(prefabName);
		}
	}

	public void PreCreate (GameObject parent)
	{
		Inital();
		gameObject.transform.parent = parent.transform;
		templatePrefabObj = (GameObject)Resources.Load(prefabName);
		if (!templatePrefabObj)
		{
			//Debug.Log(this.GetType().ToString() + " PreCreate fail " + prefabName);
			return;
		}

	
		for (int i = 0; i < maxobj; i++)
		{
			CreateOneObj();
		}
	}
	
	public void PreCreatePrefab (GameObject prefab, GameObject parent)
	{
		Inital();
		gameObject.transform.parent = parent.transform;
		templatePrefabObj = prefab;
		if (!templatePrefabObj)
		{
			Debug.Log(this.GetType().ToString() + " PreCreate fail " + prefabName);
			return;
		}
		
		
		for (int i = 0; i < maxobj; i++)
		{
			CreateOneObj();
		}
	}

	GameObject CreateOneObj()
	{
		GameObject newob = (GameObject)GameObject.Instantiate(templatePrefabObj);
		
		newob.transform.parent = gameObject.transform;

		newob.name = newob.name + pool.Count.ToString();
		newob.SetActive(false);
		pool.Add(newob);
		return newob;
	}

	public void Restore(GameObject obj)
	{
//		if (pool.Contains(obj))
//		{
//			obj.SetActive(false);
//		}
	}

	public GameObject GetObject()
	{
		int poolcount = pool.Count;
		for (int i = 0; i < maxobj; i++)
		{
			poolidx = (poolidx + 1) % poolcount;
			if (!pool[poolidx].activeSelf)
			{
				pool[poolidx].SetActive(true);
				return pool[poolidx];
			}			
		}

		if (grawable)
		{
			GameObject newob = CreateOneObj();
			newob.SetActive(true);
			return newob;
		}

		return null;
	}

	public void DisableAll()
	{
		int poolcount = pool.Count;
		for (int i = 0; i < poolcount; i++)
		{
			pool[i].SetActive(false);		
		}

	}
}
