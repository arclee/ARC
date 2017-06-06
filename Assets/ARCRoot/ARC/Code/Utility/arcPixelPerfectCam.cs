using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class arcPixelPerfectCam : MonoBehaviour {
	
	public int targetHeightPixel = 480;
	//public int targetWidthPixel = 320;
	public int pixelPreUnit = 100;
	
	
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Camera cam = GetComponent<Camera>();
		cam.orthographicSize = targetHeightPixel/2.0f/pixelPreUnit;
		//cam.orthographicSize = targetWidthPixel/ (((targetWidthPixel/targetHeightPixel)*2.0f)*pixelPreUnit);
	}
}
