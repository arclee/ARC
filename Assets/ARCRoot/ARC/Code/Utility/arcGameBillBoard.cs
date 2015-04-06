using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class arcGameBillBoard : MonoBehaviour {

	public enum LookMode
	{
		LookCam = 0,//看著 cam.
		LookCamFoward//看著 cam 看的方向.
	}

	public LookMode lookmode = LookMode.LookCam;
	//保持 y 朝正上.
	public bool keepYUp = false;
	//使用 -forward 當面向. 一般 sprite 的圖片正面會是 -forward, 這時就要勾選.
	public bool useBackwardLookCam = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Camera.main == null) 
		{
			return;
		}
		if (lookmode == LookMode.LookCam)
		{
			Vector3 cam = Camera.main.transform.position;
			if (keepYUp)
			{
				cam.y = transform.position.y;
			}

			if (useBackwardLookCam)
			{
				transform.LookAt(transform.position - cam + transform.position);
			}
			else
			{
				transform.LookAt(cam);
			}
		}
		else if (lookmode == LookMode.LookCamFoward)
		{
			Vector3 cam = Camera.main.transform.forward;
			if (keepYUp)
			{
				cam.y = 0;
				cam.Normalize();
			}
			
			if (useBackwardLookCam)
			{
				transform.LookAt(transform.position + cam);
			}
			else
			{
				transform.LookAt(transform.position - cam);
			}
		}

	}
}
