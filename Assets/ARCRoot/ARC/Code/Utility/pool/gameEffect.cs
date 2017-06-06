using UnityEngine;
using System.Collections;

public class gameEffect : MonoBehaviour {

	public float showtime = 0.1f;
	public float currenttime = 0;
	
	// Use this for initialization
	void Start ()
	{
	
	}

	void OnEnable()
	{
		#if USE_TK2D
		currenttime = 0;
		tk2dSpriteAnimator tam = GetComponentInChildren<tk2dSpriteAnimator>();
		if (tam)
		{
			tam.Play();
		}
		#endif
	}

	// Update is called once per frame
	void Update () {
	
		currenttime += Time.deltaTime;
		if (currenttime >= showtime)
		{
			gameObjPoolMgr.Instance.RestoreObj(gameObject);
		}
	}
}
