using UnityEngine;
using System.Collections;

public class arcSinShake : MonoBehaviour {

	public enum ShakeState
	{
		eNone,
		eStarting,
		eShaking,
		eEnding,
	}


	public Vector2 Speed = Vector2.zero;
	public Vector2 Amp = Vector2.zero;


	public float StartingTime = 0;
	public float ShakingTime = 0;
	public float EndingTime = 0;
	public float CurrentTime = 0;
	public bool IsShaking = false;
	public Vector3 StartPos;
	public ShakeState CurrentState = ShakeState.eNone;
	// Use this for initialization
	void Start () {
	
	}
	
	public void StartShake()
	{
		if (IsShaking)
		{
			return;
		}
		
		StartPos = transform.position;
		IsShaking = true;
		CurrentState = ShakeState.eStarting;
		CurrentTime = 0;
	}


	void StopShake(bool fadeOut = true)
	{
		if (!IsShaking)
		{
			return;
		}

		if (fadeOut)
		{
			if (CurrentState != ShakeState.eEnding)
			{
				CurrentState = ShakeState.eEnding;
				CurrentTime = 0;
			}

		}
		else
		{
			IsShaking = false;
			transform.position = StartPos;
		}
	}

	void UpdateShaking()
	{
		CurrentTime += Time.deltaTime;

		switch (CurrentState)
		{
		case ShakeState.eStarting:
		{
			Calculate(CurrentTime, StartingTime, true);
			if (CurrentTime >= StartingTime)
			{
				CurrentState = ShakeState.eShaking;
				CurrentTime = 0;
			}
		break;
		}
		case ShakeState.eShaking:
		{
			Calculate(ShakingTime, ShakingTime, true);
			if (CurrentTime >= ShakingTime)
			{
				CurrentState = ShakeState.eEnding;
				CurrentTime = 0;

			}
		break;
		}
		case ShakeState.eEnding:
		{
			Calculate(CurrentTime, EndingTime, false);
			if (CurrentTime >= EndingTime)
			{
				CurrentState = ShakeState.eNone;
				CurrentTime = 0;

				IsShaking = false;
			}
		break;
		}
		default:
		{
			break;
		}
		}
	}

	void Calculate(float s, float e, bool fadeIn)
	{
		if (e == 0)
		{
			transform.position = StartPos;
			return;
		}

		if (s > e)
		{
			s = e;
		}

		float p = 0;
		if (fadeIn)
		{
			p = s/e;
		}
		else
		{
			p = 1 - (s/e);
		}
		
		float movex = p * Amp.x * (Mathf.Sin(Time.time * Speed.x));
		float movey = p * Amp.y * (Mathf.Sin(Time.time * Speed.y));
		Vector3 pos = StartPos;
		pos.x += movex;
		pos.y += movey;
		transform.position = pos;

	}

	// Update is called once per frame
	void Update ()
	{


		if (IsShaking)
		{
			UpdateShaking();
		}

	}


    //void OnGUI()
    //{
    //    if (GUILayout.Button("shake"))
    //    {
    //        StartShake();
    //    }
    //    if (GUILayout.Button("stop fade"))
    //    {
    //        StopShake();
    //    }
    //    if (GUILayout.Button("shake on fade"))
    //    {
    //        StopShake(false);
    //    }
    //}


}
