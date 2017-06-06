using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameDBMgr : MonoBehaviour {

	public static gameDBMgr instance;

	public Dictionary<string, arcCSVReader> dbs = new Dictionary<string, arcCSVReader>();

	public string dbpath;
	public string[] preloadDbsName;
	
	public Dictionary<string, gameDBData> dbdatas = new Dictionary<string, gameDBData>();

	
	public static gameDBMgr EditorInstance
	{
		get
		{
			if(instance == null)
			{
				instance = (gameDBMgr) FindObjectOfType(typeof(gameDBMgr));
				if (instance == null)
				{
					Debug.LogError("An instance of " + typeof(gameDBMgr) + 
					               " is needed in the scene, but there is none.");
				}

			}
			
			instance.PreLoadDBs();
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyObject(gameObject);
			return;
		}

		PreLoadDBs();
	}

	public void PreLoadDBs()
	{
		
		dbs.Clear();
		for (int i = 0; i < preloadDbsName.Length; i++)
		{
			arcCSVReader csvr = new arcCSVReader();
			if (!csvr.LoadFromResource(dbpath + preloadDbsName[i]))
			{
				Debug.Log("Load db error:" + dbpath + preloadDbsName[i]);
			}
			else
			{
				Debug.Log("Load db success:" + dbpath + preloadDbsName[i]);
				
			}
			dbs.Add(preloadDbsName[i], csvr);
			csvr.DebugOutput();
		}

		dbdatas.Clear();
		for (int i = 0; i < preloadDbsName.Length; i++)
		{
			arcCSVReader csvr = new arcCSVReader();
			if (!csvr.LoadFromResource(dbpath + preloadDbsName[i]))
			{
				Debug.Log("Load db error:" + dbpath + preloadDbsName[i]);
			}
			else
			{
				Debug.Log("Load db success:" + dbpath + preloadDbsName[i]);
				
				gameDBData dbda = new gameDBData(csvr);
				dbdatas.Add(preloadDbsName[i], dbda);
				//dbda.DebugOutput();
			}
		}

	}

	public arcCSVReader GetDB(string dbname)
	{
		if (dbs.ContainsKey(dbname))
		{
			return dbs[dbname];
		}

		return null;
	}	
	
	public gameDBData GetDBData(string dbname)
	{
		if (dbdatas.ContainsKey(dbname))
		{
			return dbdatas[dbname];
		}
		
		return null;
	}	


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
