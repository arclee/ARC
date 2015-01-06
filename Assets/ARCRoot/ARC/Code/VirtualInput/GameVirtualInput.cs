using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameVirtualInputBase : MonoBehaviour
{
	
	public int touchID = -1;
	virtual public int InputUpdate()
	{
		return -1;
	}

}

public class GameVirtualInput : MonoBehaviour
{
	public delegate void StickEvent(Vector2 axis);

	public static GameVirtualInput instance;

	public List<GameVirtualInputBase> inputList = new List<GameVirtualInputBase>();

	public enum StickInputEventType
	{
		DOWN = 0,
		SHORT_UP,
		LONG_UP,
		UP,
		MOVE
	};
	
	public enum ButtonInputEventType
	{
		DOWN = 0,
		SHORT_UP,
		LONG_UP,
		UP,
		HOLD
	};
	//public Dictionary<int, GameVirtualInputBase> touchedDict = new Dictionary<int, GameVirtualInputBase>();

	//public List<GameVirtualInputBase> touchedList = new List<GameVirtualInputBase>();

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyObject(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Debug.Log(transform.GetChild(i).gameObject.name);
		}
	}

	public GameVirtualInputBase isTouched(int touchid)
	{
		for (int i = 0; i < inputList.Count; i++)
		{
			if (inputList[i].touchID == touchid)
			{
				return inputList[i];
			}
		}

		return null;
	}

	// Update is called once per frame
	void Update ()
	{
		//每個 frame 重取 ID.
		for (int i = 0; i < inputList.Count; i++)
		{
			inputList[i].touchID = -1;
		}	

		//更新.
		for (int i = 0; i < inputList.Count; i++)
		{
			inputList[i].InputUpdate();
		}	
	}

	public void RegStickInputEvent(int InputId, GameVirtualInput.StickInputEventType et, Action<Vector2> se)
	{
		int fid = -1;
		for (int i = 0; i < inputList.Count; i++)
		{
			if (inputList[i].GetType() == typeof(GameVirtualInputStick))
			{
				GameVirtualInputStick gvis = (GameVirtualInputStick)inputList[i];
				fid++;
				if (fid == InputId)
				{
					switch (et)
					{
					case StickInputEventType.DOWN:
					{
						//防止加二次.
						gvis.onDownEvent -= se;
						gvis.onDownEvent += se;
						break;
					}
					case StickInputEventType.UP:
					{
						gvis.onUpEvent -= se;
						gvis.onUpEvent += se;
						break;
					}
					case StickInputEventType.SHORT_UP:
					{
						gvis.onShortUpEvent -= se;
						gvis.onShortUpEvent += se;
						break;
					}
					case StickInputEventType.LONG_UP:
					{
						gvis.onLongUpEvent -= se;
						gvis.onLongUpEvent += se;
						break;
					}
					case StickInputEventType.MOVE:
					{
						gvis.onMoveEvent -= se;
						gvis.onMoveEvent += se;
						break;
					}
					default:
					{
						Debug.Log("RegStickInputEvent no set Case:" + et.ToString());
						break;
					}
					}
				}
			}
		}
	}
	
	public void UnRegStickInputEvent(int InputId, StickInputEventType et, Action<Vector2> se)
	{
		int fid = -1;
		for (int i = 0; i < inputList.Count; i++)
		{
			if (inputList[i].GetType() == typeof(GameVirtualInputStick))
			{
				GameVirtualInputStick gvis = (GameVirtualInputStick)inputList[i];
				fid++;
				if (fid == InputId)
				{
					switch (et)
					{
					case StickInputEventType.DOWN:
					{
						gvis.onDownEvent -= se;
						break;
					}
					case StickInputEventType.UP:
					{
						gvis.onUpEvent -= se;
						break;
					}
					case StickInputEventType.SHORT_UP:
					{
						gvis.onShortUpEvent -= se;
						break;
					}
					case StickInputEventType.LONG_UP:
					{
						gvis.onLongUpEvent -= se;
						break;
					}
					case StickInputEventType.MOVE:
					{
						gvis.onMoveEvent -= se;
						break;
					}
					default:
					{
						Debug.Log("RegStickInputEvent no set Case:" + et.ToString());
						break;
					}
					}
				}
			}
		}
	}
	
	public void RegButtonInputEvent(int InputId, GameVirtualInput.ButtonInputEventType et, Action<Vector2> se)
	{
		int fid = -1;
		for (int i = 0; i < inputList.Count; i++)
		{
			if (inputList[i].GetType() == typeof(GameVirtualInputButton))
			{
				GameVirtualInputButton gvis = (GameVirtualInputButton)inputList[i];
				fid++;
				if (fid == InputId)
				{
					switch (et)
					{
					case ButtonInputEventType.DOWN:
					{
						//防止加二次.
						gvis.onDownEvent -= se;
						gvis.onDownEvent += se;
						break;
					}
					case ButtonInputEventType.UP:
					{
						gvis.onUpEvent -= se;
						gvis.onUpEvent += se;
						break;
					}
					case ButtonInputEventType.SHORT_UP:
					{
						gvis.onShortUpEvent -= se;
						gvis.onShortUpEvent += se;
						break;
					}
					case ButtonInputEventType.LONG_UP:
					{
						gvis.onLongUpEvent -= se;
						gvis.onLongUpEvent += se;
						break;
					}
					case ButtonInputEventType.HOLD:
					{
						gvis.onHoldEvent -= se;
						gvis.onHoldEvent += se;
						break;
					}
					default:
					{
						Debug.Log("RegButtonInputEvent no set Case:" + et.ToString());
						break;
					}
					}
				}
			}
		}
	}

	
	public void UnRegButtonInputEvent(int InputId, GameVirtualInput.ButtonInputEventType et, Action<Vector2> se)
	{
		int fid = -1;
		for (int i = 0; i < inputList.Count; i++)
		{
			if (inputList[i].GetType() == typeof(GameVirtualInputButton))
			{
				GameVirtualInputButton gvis = (GameVirtualInputButton)inputList[i];
				fid++;
				if (fid == InputId)
				{
					switch (et)
					{
					case ButtonInputEventType.DOWN:
					{
						//防止加二次.
						gvis.onDownEvent -= se;
						break;
					}
					case ButtonInputEventType.UP:
					{
						gvis.onUpEvent -= se;
						break;
					}
					case ButtonInputEventType.SHORT_UP:
					{
						gvis.onShortUpEvent -= se;
						break;
					}
					case ButtonInputEventType.LONG_UP:
					{
						gvis.onLongUpEvent -= se;
						break;
					}
					case ButtonInputEventType.HOLD:
					{
						gvis.onHoldEvent -= se;
						break;
					}
					default:
					{
						Debug.Log("RegButtonInputEvent no set Case:" + et.ToString());
						break;
					}
					}
				}
			}
		}
	}
}

