using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class arcGameBillBoard : MonoBehaviour {

	public enum LookMode
	{
		LookCam = 0,
		LookCamFoward
	}

	public LookMode lookmode = LookMode.LookCam;
	public bool keepYup = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lookmode == LookMode.LookCam)
		{
			Vector3 cam = Camera.main.transform.position;
			if (keepYup)
			{
				cam.y = transform.position.y;
			}
			transform.LookAt(transform.position - cam + transform.position);
		}
		else if (lookmode == LookMode.LookCamFoward)
		{
			Vector3 cam = Camera.main.transform.forward;
			if (keepYup)
			{
				cam.y = 0;
				cam.Normalize();
			}
			
			transform.LookAt(transform.position + cam);
		}

	}
}
