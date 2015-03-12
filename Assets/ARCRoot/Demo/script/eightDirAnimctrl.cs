using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class eightDirAnimctrl : MonoBehaviour {

	public SpriteRenderer targetSprite;

	public Sprite[] sprites;
	public arcEightDirDetector eightDirDect = new arcEightDirDetector();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateAnim();	
	}
	
	void UpdateAnim()
	{
		if (eightDirDect.Update(transform))
		{
			switch (eightDirDect.currentShowState)
			{
			case arcEightDirDetector.ShowState.BACK:
			{
				//PlayAnim("_B");
				targetSprite.sprite = sprites[0];
				break;
			}
			case arcEightDirDetector.ShowState.BACK_LEFT:
			{
				//PlayAnim("_BL");
				targetSprite.transform.localScale = new Vector3(1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[1];
				break;
			}
			case arcEightDirDetector.ShowState.BACK_RIGHT:
			{
				//PlayAnim("_BL");
				targetSprite.transform.localScale = new Vector3(-1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[1];
				break;
			}
			case arcEightDirDetector.ShowState.FACE:
			{
				targetSprite.transform.localScale = new Vector3(1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[2];
				//PlayAnim("_F");
				break;
			}
			case arcEightDirDetector.ShowState.FACE_LEFT:
			{
				targetSprite.transform.localScale = new Vector3(1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[3];
				//PlayAnim("_FL");
				break;
			}
			case arcEightDirDetector.ShowState.FACE_RIGHT:
			{
				targetSprite.transform.localScale = new Vector3(-1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[3];
				//PlayAnim("_FL");
				break;
			}
			case arcEightDirDetector.ShowState.LEFT:
			{
				targetSprite.transform.localScale = new Vector3(1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[4];
				//PlayAnim("_L");
				break;
			}
			case arcEightDirDetector.ShowState.RIGHT:
			{
				targetSprite.transform.localScale = new Vector3(-1, targetSprite.transform.localScale.y, targetSprite.transform.localScale.z);
				targetSprite.sprite = sprites[4];
				//PlayAnim("_L");
				break;
			}
			case arcEightDirDetector.ShowState.UNKNOW:
			{
				break;
			}
			}
		}
		
	}
}
