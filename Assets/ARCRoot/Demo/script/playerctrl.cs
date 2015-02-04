using UnityEngine;
using System.Collections;

public class playerctrl : MonoBehaviour {

	public float speed = 1.0f;
	public Vector3 movedir = Vector3.zero;

	public Rigidbody ball;
	public GameObject Gunobject;
	public float shootforce = 10;

	public float maxcharge = 5.0f;
	
	public float minchargetime = 0.1f;

	public float shiftforce = 10;

	//network.
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private Quaternion syncStartRot = Quaternion.identity;

	void Awake()
	{
		lastSynchronizationTime = Time.time;
	}


	// Use this for initialization
	void Start ()
	{
		
	}
	
	void OnEnable ()
	{
		if (networkView.isMine)
		{
			GameVirtualInput.instance.RegStickInputEvent(0, GameVirtualInput.StickInputEventType.MOVE, VIMove);
			GameVirtualInput.instance.RegStickInputEvent(0, GameVirtualInput.StickInputEventType.UP, VIMoveStop);

			GameVirtualInput.instance.RegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.SHORT_UP, VIShoot);
			GameVirtualInput.instance.RegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.HOLD, VICharge);
			GameVirtualInput.instance.RegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.UP, VIChargeUP);
			
			GameVirtualInput.instance.RegButtonInputEvent(1, GameVirtualInput.ButtonInputEventType.DOWN, VIChargeShiftDown);
			GameVirtualInput.instance.RegButtonInputEvent(2, GameVirtualInput.ButtonInputEventType.UP, VIChargeShiftUp);
		}
	}


	void OnDisable ()
	{
		if (networkView.isMine)
		{
			GameVirtualInput.instance.UnRegStickInputEvent(0, GameVirtualInput.StickInputEventType.MOVE, VIMove);
			GameVirtualInput.instance.UnRegStickInputEvent(0, GameVirtualInput.StickInputEventType.UP, VIMoveStop);
			
			GameVirtualInput.instance.UnRegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.SHORT_UP, VIShoot);
			GameVirtualInput.instance.UnRegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.HOLD, VICharge);
			GameVirtualInput.instance.UnRegButtonInputEvent(0, GameVirtualInput.ButtonInputEventType.UP, VIChargeUP);
			
			GameVirtualInput.instance.UnRegButtonInputEvent(1, GameVirtualInput.ButtonInputEventType.DOWN, VIChargeShiftDown);
			GameVirtualInput.instance.UnRegButtonInputEvent(2, GameVirtualInput.ButtonInputEventType.UP, VIChargeShiftUp);
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRot = Quaternion.identity;

		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncPosition = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);

			syncRot = rigidbody.rotation;
			stream.Serialize(ref syncRot);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncRot);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody.position;

			syncStartRot = syncRot;
		}
	}

	void VIMove(Vector2 move)
	{
		movedir = new Vector3(move.x, 0, move.y) * speed * Time.deltaTime;

		//鏡頭座標轉換.
		movedir = Camera.main.transform.TransformPoint(movedir) - Camera.main.transform.position;
		movedir.y = 0;


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
		ball.gameObject.transform.position = Gunobject.transform.position;
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
			ball.gameObject.transform.position = Gunobject.transform.position;
			ball.velocity = Vector3.zero;
			ball.AddForce(transform.forward * shootforce *  (1 + move.x));
		}
	}
	
	void VIChargeShiftDown(Vector2 move)
	{
		rigidbody.AddForce(transform.forward * shiftforce);
	}

	void VIChargeShiftUp(Vector2 move)
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if (networkView.isMine)
		{
#if UNITY_STANDALONE
			InputMovement();
#endif
			UpdatePos();
		}
		else
		{
			SyncedMovement();
		}
	}

	void UpdatePos()
	{
		//if (movedir.magnitude > 0)
		{
			//transform.Translate(movedir, Space.World);
			rigidbody.MovePosition(rigidbody.position + movedir);
		}

	}
	
	private void InputMovement()
	{
		if (Input.GetKeyUp(KeyCode.W))
		{
			movedir.y = 0;
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			movedir.y = 0;
		}
		if (Input.GetKeyUp(KeyCode.D))
		{
			movedir.x = 0;
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			movedir.x = 0;
		}


		if (Input.GetKey(KeyCode.W))
		{
			movedir.y = 1;
		}
		
		if (Input.GetKey(KeyCode.S))
		{
			movedir.y = -1;
		}

		if (Input.GetKey(KeyCode.D))
		{
			movedir.x = 1;
		}
		
		if (Input.GetKey(KeyCode.A))
		{
			movedir.x = -1;
		}
		
		VIMove(movedir);

	}
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);

		rigidbody.rotation = syncStartRot;
	}
}
