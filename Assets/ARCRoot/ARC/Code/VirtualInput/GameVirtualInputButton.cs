using UnityEngine;
using System.Collections;
using System;

public class GameVirtualInputButton : GameVirtualInputBase
{
	
	
	//event.
	public event Action<Vector2> onDownEvent;
	public event Action<Vector2> onShortUpEvent;
	public event Action<Vector2> onLongUpEvent;
	public event Action<Vector2> onUpEvent;
	public event Action<Vector2> onHoldEvent;
	
	
	//Objects.
	public Camera tartegCamera;
	public CircleCollider2D circleC2d;
	public SpriteRenderer btnSprite;
	//GUISettings.
	public float minLongUpTime = 0.1f;
	public float colliderTouchScale = 3.0f;
	
	float oldCircleR;
	
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

		//touchID = -1;		
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
					btnSprite.color = Color.red;
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
					//Vector3 directionWorld = touchWorldPos - stickPosZ0;
					//算出移動多少比率, 半徑.
					//Vector2 directionV2 = new Vector2(directionWorld.x, directionWorld.y);
					//float directionWorldLeng = directionV2.magnitude;
					//float cirRadius = oldCircleR * transform.localScale.x;
					//float directionWorldRadiusRatio = directionWorldLeng/cirRadius;
					
					//debugtext.text = directionWorldRadiusRatio.ToString();

					
					//collider 跟著移動.
					circleC2d.gameObject.transform.position = new Vector3(touchWorldPos.x, touchWorldPos.y, circleC2d.gameObject.transform.position.z);
										
				}
				
				
				//放開.
				if (t.phase == TouchPhase.Ended || (t.phase == TouchPhase.Canceled))
				{
					btnSprite.color = Color.white;
					if (onUpEvent != null)
					{
						onUpEvent(new Vector2(holdTime, holdTime));
					}
					
					//長放或短放.
					if (holdTime >= minLongUpTime)
					{
						Debug.Log("onLongUpEvent" + holdTime.ToString());
						if (onLongUpEvent != null)
						{
							onLongUpEvent(new Vector2(holdTime, holdTime));
						}
					}
					else
					{	
						Debug.Log("onShortUpEvent" + holdTime.ToString());
						if (onShortUpEvent != null)
						{
							onShortUpEvent(new Vector2(holdTime, holdTime));
						}	
					}
					
					//collider 移回去原位.
					SetUnTouched();
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
