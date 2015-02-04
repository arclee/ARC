using UnityEngine;
using System.Collections;
using System;



public class GameVirtualInputStick : GameVirtualInputBase
{

	//out value 0~1.
	//public Vector2 Axis = new Vector2(0, 0);


	//event.
	public event Action<Vector2> onDownEvent;
	public event Action<Vector2> onShortUpEvent;
	public event Action<Vector2> onLongUpEvent;
	public event Action<Vector2> onUpEvent;
	public event Action<Vector2> onMoveEvent;
	public event Action<Vector2> onHoldEvent;


	//Objects.
	public Camera tartegCamera;
	public GameObject stickHead;
	public CircleCollider2D circleC2d;
	
	//GUISettings.
	public float minToMove = 0.2f;
	public float minLongUpTime = 0.1f;
	public bool moveStick = false;
	public float minStickTomovek = 0.95f;
	public float colliderTouchScale = 3.0f;

	float oldCircleR;
	
	public bool isUseStickAppearArea = true;
	public BoxCollider2D AppearArea;
	//info.
	public float holdTime = 0;


	//public GUIText debugtext;

	// Use this for initialization
	void Start ()
	{
		oldCircleR = circleC2d.radius;
	}
	
	virtual public void SetUnTouched()
	{
		circleC2d.radius = oldCircleR;
		circleC2d.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, circleC2d.gameObject.transform.position.z);
		
		stickHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, circleC2d.gameObject.transform.position.z);

		touchID = -1;		
		holdTime = 0;
	}


	override public int InputUpdate()
	{

		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch t = Input.GetTouch(i);

				//stickpos z = 0;
				Vector3 stickPosZ0 = gameObject.transform.position;
				stickPosZ0.z = 0;

				//touch pos.
				Vector3 touchWorldPos = tartegCamera.ScreenToWorldPoint(new Vector3 (t.position.x, t.position.y, 0));
				touchWorldPos.z = 0;
				
				//touch delta pos.
				Vector3 touchdeltaWorldPos = tartegCamera.ScreenToWorldPoint(new Vector3 (t.deltaPosition.x, t.deltaPosition.y, 0));
				touchdeltaWorldPos.z = 0;

				//可出現stick的範圍.
				if (isUseStickAppearArea && AppearArea != null)
				{
					if (AppearArea.OverlapPoint(touchWorldPos) && (t.phase == TouchPhase.Began))
					{
						transform.position = new Vector3(touchWorldPos.x, touchWorldPos.y, transform.position.z);
					}
				}


				//是否點中.
				if (circleC2d.OverlapPoint(touchWorldPos))
				{
					//是否別人用了.
					GameVirtualInputBase touchedInput = GameVirtualInput.instance.isTouched(i);
					//別的 input 用了.
					if (touchedInput != null)
					{
						continue;
					}

				}
				else
				{
					continue;
				}

				//暫時記下 touch id;
				touchID = i;
				
				//Debug.Log("in " + i.ToString() + " " + t.phase.ToString());
				//開始.
				if (t.phase == TouchPhase.Began)
				{
					circleC2d.radius = oldCircleR * colliderTouchScale;
					holdTime = 0;
					if (onDownEvent != null)
					{
						onDownEvent(Vector2.zero);
					}

				}

				//按住.
				if (t.phase == TouchPhase.Stationary)
				{
					holdTime += Time.deltaTime;
				}

				//移動.
				if (t.phase == TouchPhase.Moved)
				{
					//direction.
					Vector3 directionWorld = touchWorldPos - stickPosZ0;
					//算出移動多少比率, 半徑.
					Vector2 directionV2 = new Vector2(directionWorld.x, directionWorld.y);
					float directionWorldLeng = directionV2.magnitude;
					float cirRadius = oldCircleR * transform.localScale.x;
					float directionWorldRadiusRatio = directionWorldLeng/cirRadius;



					if (moveStick)
					{
						//stick 跟著移動.
						if (directionWorldLeng >= cirRadius * minStickTomovek)
						{
							//Debug.Log(sdirvec.magnitude);
							transform.position += directionWorld * 0.1f;
						}

					}
					

					//香菇頭.
					stickHead.transform.position = transform.position + directionWorld.normalized * Math.Min(directionWorldRadiusRatio, 1) * cirRadius;


					//collider 跟著移動.
					circleC2d.gameObject.transform.position = new Vector3(touchWorldPos.x, touchWorldPos.y, circleC2d.gameObject.transform.position.z);



					//debugtext.text = directionWorldRadiusRatio.ToString();

					if (directionWorldRadiusRatio > minToMove)
					{
						if (onMoveEvent != null)
						{
							onMoveEvent(directionV2.normalized);
						}		
					}


				}


				//放開.
				if (t.phase == TouchPhase.Ended || (t.phase == TouchPhase.Canceled))
				{
					//collider 移回去原位.
					SetUnTouched();
					if (onUpEvent != null)
					{
						onUpEvent(Vector2.zero);
					}

					//長放或短放.
					if (holdTime >= minLongUpTime)
					{			
						if (onLongUpEvent != null)
						{
							onLongUpEvent(Vector2.zero);
						}
					}
					else
					{	
						if (onShortUpEvent != null)
						{
							onShortUpEvent(Vector2.zero);
						}	
					}

				}


				//按住事件.
				if (onHoldEvent != null && holdTime > 0)
				{
					onHoldEvent(new Vector2(holdTime, holdTime));
				}



				//debugtext.text = touchID.ToString();

				break;
			}
		}

		return touchID;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
