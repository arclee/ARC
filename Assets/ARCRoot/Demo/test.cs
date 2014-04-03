using UnityEngine;
using System.Collections;
public class test : MonoBehaviour {
	
	arcCSVReader cr = new arcCSVReader();
	// Use this for initialization
	void Start () {
		
		//Rigidbody2D ant = gameObject.GetSafeComponent<Rigidbody2D>();
		//Animator aaa =gameObject.GetInterfaceComponent<Animator>();
		//Vector3 v3 = new Vector3(1,2,3);
		
		//Vector3 cv3 = arcUtility.Clone(v3);
		//Vector3 cv43 = arcUtility.Clone(cv3);

	}
	
	// Update is called once per frame
	void Update () {
		

	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(0,0,100,100), "load csv"))
		{
			cr.LoadFromResource("story");

			//cr.DebugOutput();

			//TextMesh tm = GetComponent<TextMesh>();
			//tm.text = cr.mData[0][1];
		}

	}

}
