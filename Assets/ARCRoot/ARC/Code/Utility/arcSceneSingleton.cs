using UnityEngine;
using System.Collections;

public class arcSceneSingleton <T> : MonoBehaviour where T : MonoBehaviour
{ 
	
	public static T instance;

	
	void Awake()
	{

		if (instance != null)
		{
			Debug.Log(this.ToString() + "multi instace!", this.gameObject);
		}
		else
		{
			instance = (T) FindObjectOfType(typeof(T));
		}

		DerivedAwake();
	}
	
	public virtual void DerivedAwake()
	{
		
	}
	
	public virtual void DerivedStart()
	{
		
	}
	public virtual void DerivedUpdate()
	{
		
	}

	// Use this for initialization
	void Start ()
	{
		DerivedStart();
	}
	
	// Update is called once per frame
	void Update () {
		DerivedUpdate();
	}
}
