using UnityEngine;
using System.Collections;

public class arcSceneSingleton <T> : MonoBehaviour where T : MonoBehaviour
{ 
	
	public static T Instance;

	
	void Awake()
	{

		if (Instance != null)
		{
			Debug.Log(this.ToString() + "multi instace!", this.gameObject);
		}
		else
		{
			Instance = (T) FindObjectOfType(typeof(T));
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
