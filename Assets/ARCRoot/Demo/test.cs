using UnityEngine;
using System.Collections;
public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		Rigidbody2D ant = gameObject.GetSafeComponent<Rigidbody2D>();
		Animator aaa =gameObject.GetInterfaceComponent<Animator>();
		Vector3 v3 = new Vector3(1,2,3);
		
		Vector3 cv3 = arcUtility.Clone(v3);
		Vector3 cv43 = arcUtility.Clone(cv3);

	}
	
	// Update is called once per frame
	void Update () {
		
		Rigidbody2D ant = gameObject.GetSafeComponent<Rigidbody2D>();
		Animator aaa =gameObject.GetInterfaceComponent<Animator>();
		Vector3 v3 = new Vector3(1,2,3);
		
		Vector3 cv3 = arcUtility.Clone(v3);
		Vector3 cv43 = arcUtility.Clone(cv3);
	}
}
