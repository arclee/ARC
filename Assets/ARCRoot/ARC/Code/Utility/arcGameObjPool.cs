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

	public void Inital(GameObject parent = null)
	{
		if (gameObject == null)
		{
			gameObject = new GameObject(prefabName);
			gameObject.transform.SetParent(parent.transform);
		}
		else
		{
			gameObject.transform.SetParent(parent.transform);
		}
	}

	public void PreCreate (GameObject parent)
	{
		Inital(parent);
		gameObject.transform.SetParent(parent.transform);
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
		Inital(parent);
		gameObject.transform.SetParent(parent.transform);
		templatePrefabObj = prefab;
		if (!templatePrefabObj)
		{
			Debug.Log(this.GetType().ToString() + " PreCreate fail " + prefabName);
			return;
		}
		
		int need = maxobj - pool.Count;
		if (need > 0)
		{
			for (int i = 0; i < need; i++)
			{
				CreateOneObj();
			}
		}
	}

	
	public void PreCreatePrefabCont(GameObject prefab, GameObject parent, int count)
	{
		Inital(parent);
		gameObject.transform.SetParent(parent.transform);
		templatePrefabObj = prefab;
		if (!templatePrefabObj)
		{
			Debug.Log(this.GetType().ToString() + " PreCreate fail " + prefabName);
			return;
		}
		
		int need = count - pool.Count;
		if (need > 0)
		{
			for (int i = 0; i < need; i++)
			{
				CreateOneObj();
			}
		}
	}

	GameObject CreateOneObj(GameObject parent = null)
	{
        GameObject newob = null;
        if (parent != null)
        {
            newob = (GameObject)GameObject.Instantiate(templatePrefabObj, parent.transform);
        }
        else
        {
            newob = (GameObject)GameObject.Instantiate(templatePrefabObj, gameObject.transform);
            //newob.transform.SetParent(gameObject.transform);
        }

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

	public GameObject GetObject(GameObject parent = null)
	{
		int poolcount = pool.Count;
		for (int i = 0; i < poolcount; i++)
		{
			poolidx = (poolidx + 1) % poolcount;
			if (!pool[poolidx].activeSelf)
			{
				pool[poolidx].SetActive(true);

                if (parent != null)
                {
                    pool[poolidx].transform.SetParent(parent.transform);
                }
                return pool[poolidx];
			}			
		}

		if (grawable)
		{
			GameObject newob = CreateOneObj(parent);
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
	
	public void DestoryAll()
	{
		int poolcount = pool.Count;
		for (int i = 0; i < poolcount; i++)
		{
			GameObject.Destroy(pool[i]);
		}
		pool.Clear();
		GameObject.Destroy(gameObject);
	}
}
