using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class arcPixelPerfectMove : MonoBehaviour {
	
	public int pixelPreUnit = 100;
	public float step = 1;
	
	// Use this for initialization
	//public Vector3 outval;
	public bool isLocal = false;
	void Start () {
		
		step = 1.0f/pixelPreUnit;
	}
	
	Vector3 RoundVector3XY(Vector3 val)
	{
		//outval.z = val.z;
		//outval.x = (val.x/step) * step;
		//outval.y = (val.y/step) * step;
		val.x = (float)Math.Round((double)val.x, 2);
		val.y = (float)Math.Round((double)val.y, 2);
		
		
		return val;
	}
	
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (isLocal)
		{
			transform.localPosition = RoundVector3XY(transform.localPosition);
		}
		else
		{
			transform.position = RoundVector3XY(transform.position);
		}
	}
}
