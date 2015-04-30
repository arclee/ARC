using UnityEngine;
using System.Collections;

public class testarcGameCamera : MonoBehaviour {

	public GameObject target;
	// Use this for initialization
	public Vector3 lastpos;
	public float distDontFollow = 10;

	public bool follow = false;
	public bool Follow
	{
		get	{return follow;}
		set {follow = value;}
	}
	public bool lookat = false;
	public bool Lookat
	{
		get	{return lookat;}
		set {lookat = value;}
	}
	void Start ()
	{
		if (target == null)
		{
			target = GameObject.Find("Player_S");
			
			if (target == null)
			{
				return;
			}
		}
		lastpos = target.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target == null)
		{
			target = GameObject.Find("Player_S");

			if (target == null)
			{
				return;
			}
			lastpos = target.transform.position;
		}
		
		Vector3 pos = target.transform.position - lastpos;
		lastpos = target.transform.position;
		if (follow && distDontFollow < (target.transform.position - transform.position).magnitude)
		{
			transform.Translate(pos, Space.World);

		}

		if (lookat)
		{
			transform.LookAt(target.transform);
		}

	}


}
