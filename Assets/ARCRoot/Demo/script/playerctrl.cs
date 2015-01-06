﻿using UnityEngine;
using System.Collections;

public class playerctrl : MonoBehaviour {

	public float speed = 1.0f;
	public Vector3 movedir = Vector3.zero;

	public Rigidbody ball;
	public float shootforce = 10;

	public float maxcharge = 5.0f;
	
	public float minchargetime = 0.1f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	void OnEnable ()
	{
		GameVirtualInput.instance.RegStickInputEvent(0, GameVirtualInput.StickInputEventType.MOVE, VIMove);
		GameVirtualInput.instance.RegStickInputEvent(0, GameVirtualInput.StickInputEventType.UP, VIMoveStop);

		GameVirtualInput.instance.RegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.SHORT_UP, VIShoot);
		GameVirtualInput.instance.RegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.HOLD, VICharge);
		GameVirtualInput.instance.RegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.UP, VIChargeUP);


	}


	void OnDisable ()
	{
		GameVirtualInput.instance.UnRegStickInputEvent(0, GameVirtualInput.StickInputEventType.MOVE, VIMove);
		GameVirtualInput.instance.UnRegStickInputEvent(0, GameVirtualInput.StickInputEventType.UP, VIMoveStop);
		
		GameVirtualInput.instance.UnRegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.SHORT_UP, VIShoot);
		GameVirtualInput.instance.UnRegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.HOLD, VICharge);
		GameVirtualInput.instance.UnRegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.UP, VIChargeUP);
		
	}


	void VIMove(Vector2 move)
	{
		movedir = new Vector3(move.x, 0, move.y) * speed * Time.deltaTime;
		
		if ((movedir.magnitude > 0))
		{
			transform.rotation = Quaternion.LookRotation(movedir);
		}
	}
	
	void VIMoveStop(Vector2 move)
	{		
		movedir = Vector3.zero;
	}
	
	void VIShoot(Vector2 move)
	{
		ball.gameObject.transform.position = transform.position;
		ball.velocity = Vector3.zero;
		ball.AddForce(transform.forward * shootforce);
	}

	void VICharge(Vector2 move)
	{
		if (move.x <= maxcharge)
		{
			transform.localScale = new Vector3(1 + move.x, 1 + move.x, 1 + move.x);
		}
	}

	void VIChargeUP(Vector2 move)
	{
		transform.localScale = new Vector3(1, 1, 1);

		if (move.x > minchargetime)
		{
			ball.gameObject.transform.position = transform.position;
			ball.velocity = Vector3.zero;
			ball.AddForce(transform.forward * shootforce *  (1 + move.x));
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//if (movedir.magnitude > 0)
		{
			transform.Translate(movedir, Space.World);
		}
	
	}
}
