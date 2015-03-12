using UnityEngine;
using System.Collections;
using System;
//8向性角色方向判斷.
[Serializable]
public class arcEightDirDetector
{
	//本身該以那一面顯示出來.
	public enum ShowState
	{
		UNKNOW,
		FACE,
		FACE_LEFT,
		FACE_RIGHT,
		BACK,
		BACK_LEFT,
		BACK_RIGHT,
		LEFT,
		RIGHT,
	}

	public ShowState currentShowState = ShowState.UNKNOW;

	// -forward 才是顯示用的正面.
	public bool backwardIsFace = false;
	// -right 才是顯示的右邊.
	public bool leftIsRight = false;

	//設定八面夾角大小. 徑度 0~1.
	//						-1
	//			FDotLimit	|		-FDotLimit
	//						|
	//	FLDotLimit			|			-FLDotLimit
	//						|
	//0	--------------------+-------------------	0
	//						|
	//	BLDotLimit			|			-BLDotLimit
	//						|
	//			BDotLimit	|		-BDotLimit
	//						1
	public float FDotLimit = 0.66f; 	//左上1.
	public float FLDotLimit = 0.33f; 	//左上2
	public float BLDotLimit = 0.33f;	//左下1
	public float BDotLimit = 0.66f;		//左小2


	// 與 camera forwar 算出來的值.
	public float dotvalF;
	// 與 camera right 算出來的值.
	public float dotvalR;

	public bool Update(Transform transform)
	{
		ShowState laststate = currentShowState;

		
		//都是以看向同方向來算.
		Vector3 campos = Camera.main.transform.position;
		Vector3 charpos = transform.position;
		Vector3 charforward = transform.forward;
		if (backwardIsFace)
		{
			charforward *= -1;
		}

		Vector3 charright = transform.right;
		if (leftIsRight)
		{
			charright *= -1;
		}
		//campos.y = 0;
		charpos.y = 0;
		charforward.y = 0;
		charforward.Normalize();
		charright.y = 0;
		charright.Normalize();


		Vector3 camlook = transform.position - campos;
		camlook.y = 0;
		camlook.Normalize();

		dotvalF = Vector3.Dot(camlook, charforward);
		//angF = Mathf.Rad2Deg * Mathf.Acos(dotvalF);
		
		dotvalR = Vector3.Dot(camlook, charright);
		//angR = Mathf.Rad2Deg * Mathf.Acos(dotvalR);


		if (Mathf.Abs(dotvalF) <= FLDotLimit && Mathf.Abs(dotvalF) <= BLDotLimit)
		{
			//左右.
			if (dotvalR < 0)
			{
				//鏡頭在左邊.看向同方向.
				currentShowState = ShowState.RIGHT;
			}
			else
			{
				//鏡頭在右邊.看向同方向.
				currentShowState = ShowState.LEFT;
			}
		}
		else if (dotvalF > BLDotLimit  && dotvalF <= BDotLimit)
		{
			//後左右.
			//鏡頭在背後.看向同方向.
			if (dotvalR < 0)
			{
				//鏡頭在左邊.看向同方向.
				currentShowState = ShowState.BACK_RIGHT;
			}
			else
			{
				//鏡頭在右邊.看向同方向.
				currentShowState = ShowState.BACK_LEFT;
			}
			
		}
		else if (dotvalF > BDotLimit)
		{
			//後.
			//鏡頭在背後.看向同方向.
			currentShowState = ShowState.BACK;
		}
		else if (dotvalF < -FLDotLimit && dotvalF >= -FDotLimit)
		{
			//前左右.
			//鏡頭在前方.看向同方向.			
			if (dotvalR < 0)
			{
				//鏡頭在左邊.看向同方向.
				currentShowState = ShowState.FACE_RIGHT;
			}
			else
			{
				//鏡頭在右邊.看向同方向.
				currentShowState = ShowState.FACE_LEFT;
			}
		}
		else if (dotvalF < -FDotLimit)
		{
			//前.
			//鏡頭在前方.看向同方向.	
			currentShowState = ShowState.FACE;	
		}

		//狀態有改變.
		if (laststate != currentShowState)
		{
			return true;
		}

		return false;	
	}
}
