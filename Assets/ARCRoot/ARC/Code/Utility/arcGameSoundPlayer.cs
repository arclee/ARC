using UnityEngine;
using System.Collections;

public class arcGameSoundPlayer : MonoBehaviour {

	public static arcGameSoundPlayer instance;
	public enum BGMSourceID
	{
		BGMN = 0,
		BGM1 = 1,
		BGM2
	}

	//用來淡入淡出用.
	public AudioSource BGMSource1;
	public AudioSource BGMSource2;
	BGMSourceID CurrentBGMSourceID = BGMSourceID.BGMN;


	//所有的 bgm, 手動拉進來.
	public AudioClip[] BGMS;
	//淡入淡出時間.
	public float fadeInTime = 1.0f;
	//目前播那一個 audio clip.
	int playBGMidx = 0;
	
	//是否使用淡入淡出.
	public bool UseFade = true;
	//準備要播的 bgm 最終音量.
	public float BGMSourceFadeFinalVol;
	//要被換掉的 bgm 當時的音量.
	public float BGMSourceOldFadeVol;
	//開始淡入淡出了沒.
	bool startfade = false;
	//目前淡入淡出值.
	float currentFadeInVal;
	
	public AudioClip[] FxClips;
	public AudioSource[] FXSources;

	//debug gui.
	public int GUIScreenSizeX = 845;
	public int GUIScreenSizeY = 480;
	
	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (startfade)
		{
			currentFadeInVal += Time.deltaTime;
			//算比出率.
			float r = currentFadeInVal/fadeInTime;
			if (CurrentBGMSourceID == BGMSourceID.BGM1)
			{
				//音量變化.二者相反.
				BGMSource1.volume = BGMSourceFadeFinalVol * r;
				BGMSource2.volume = BGMSourceOldFadeVol * (1 - r);
			}
			else if (CurrentBGMSourceID == BGMSourceID.BGM2)
			{
				//音量變化.二者相反.
				BGMSource2.volume = BGMSourceFadeFinalVol * r;
				BGMSource1.volume = BGMSourceOldFadeVol * (1 - r);			
			}

			//淡入淡出結束.
			if (currentFadeInVal >= fadeInTime)
			{
				startfade = false;

				//停止舊的 bgm.
				if (CurrentBGMSourceID == BGMSourceID.BGM1)
				{
					BGMSource2.Stop();
				}
				else if (CurrentBGMSourceID == BGMSourceID.BGM2)
				{
					BGMSource1.Stop();
				}
			}

		}
	}

	void PlayBGM(int idx)
	{
		//切換 bgm.
		if (CurrentBGMSourceID == BGMSourceID.BGMN)
		{
			CurrentBGMSourceID = BGMSourceID.BGM1;
			BGMSource1.clip = BGMS[playBGMidx];
			BGMSource1.Play();

			//不使用淡入淡出就直接設音量及停止舊的.
			if (!UseFade)
			{
				BGMSource1.volume = BGMSourceFadeFinalVol;
				BGMSource2.Stop();
			}

		}
		else
		{
			if (CurrentBGMSourceID == BGMSourceID.BGM1)
			{
				//舊 bgm 由目前音量開始淡出.
				BGMSourceOldFadeVol = BGMSource1.volume;
				CurrentBGMSourceID = BGMSourceID.BGM2;
				BGMSource2.clip = BGMS[playBGMidx];
				BGMSource2.Play();
				
				//不使用淡入淡出就直接設音量及停止舊的.
				if (!UseFade)
				{
					BGMSource2.volume = BGMSourceFadeFinalVol;
					BGMSource1.Stop();
				}
			}
			else if (CurrentBGMSourceID == BGMSourceID.BGM2)
			{
				BGMSourceOldFadeVol = BGMSource2.volume;
				CurrentBGMSourceID = BGMSourceID.BGM1;
				BGMSource1.clip = BGMS[playBGMidx];
				BGMSource1.Play();

				if (!UseFade)
				{
					BGMSource1.volume = BGMSourceFadeFinalVol;
					BGMSource2.Stop();
				}
			}
		}
		
		if (UseFade)
		{
			currentFadeInVal = 0;
			startfade = true;
		}
	}
	/*
	//debug gui.
	void OnGUI()
	{
		arcUtility.GUIMatrixAutoScale(GUIScreenSizeX, GUIScreenSizeY);

		if (GUILayout.Button("play bgm"))
		{
			if (BGMS.Length > 0)
			{
				playBGMidx++;
				playBGMidx = playBGMidx%BGMS.Length;
				PlayBGM(playBGMidx);
			}
		}
	}
*/

	public void PlayFX(int idx)
	{
		//FXSources[0].clip = FxClips[idx];
		FXSources[0].PlayOneShot(FxClips[idx]);
	}
}
