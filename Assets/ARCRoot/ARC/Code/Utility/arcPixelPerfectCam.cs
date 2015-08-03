using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class arcPixelPerfectCam : MonoBehaviour {

	public int pixelPreUnit = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		AutoResize();
	}

	void AutoResize()
	{
		Camera cam = GetComponent<Camera>();

		cam.orthographicSize = Screen.height / 2.0f / pixelPreUnit;
	}
}
