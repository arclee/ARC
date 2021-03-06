﻿using UnityEngine;
using System.Collections;

public class arcSceneSoundPlayer : arcSceneSingleton<arcSceneSoundPlayer> {

	public enum BGMSourceID
	{
		BGM_NONE = 0,
		BGM1 = 1,
		BGM2
	}

    //用來淡入淡出用.
    public AudioSource BGMSource1;
	public AudioSource BGMSource2;
	BGMSourceID CurrentBGMSourceID = BGMSourceID.BGM_NONE;
    AudioSource BGMCurrentSource;


    //所有的 bgm, 手動拉進來.
    public AudioClip[] BGMS;
	//淡入淡出時間.
	public float fadeInTime = 1.0f;
	//目前播那一個 audio clip.
	int playBGMidx = -1;
	
	//是否使用淡入淡出.
	public bool UseFade = true;
	//準備要播的 bgm 最終音量.
	public float BGMSourceFadeFinalVol = 1;
	//要被換掉的 bgm 當時的音量.
	public float BGMSourceOldFadeVol = 0;
	//開始淡入淡出了沒.
	bool startfade = false;
	//目前淡入淡出值.
	float currentFadeInVal;

	int playFXidx = 0;
	public AudioClip[] FxClips;
	public AudioSource[] FXSources;

	//debug gui.
	public int GUIScreenSizeX = 845;
	public int GUIScreenSizeY = 480;
	
//	public override void DerivedAwake()
//	{
//		if (instance == null)
//		{
//			instance = this;
//			DontDestroyOnLoad(gameObject);
//		}
//		else
//		{
//			Destroy(gameObject);
//		}
//	}

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

	public int GetBGMCurrentTime()
	{
		if (CurrentBGMSourceID == BGMSourceID.BGM1)
		{
			return BGMSource1.timeSamples;
		}
		else if (CurrentBGMSourceID == BGMSourceID.BGM2)
		{
			return BGMSource2.timeSamples;
		}
		
		return 0;
	}

    public void StopBGM()
    {
        if (BGMCurrentSource)
        {
            BGMCurrentSource.Stop();
        }
    }

    public void PauseBGM()
    {
        if (BGMCurrentSource)
        {
            BGMCurrentSource.Pause();
        }
    }

    public void UnPauseBGM()
    {
        if (BGMCurrentSource)
        {
            BGMCurrentSource.UnPause();
        }

    }

	public int GetCurrentBGMIdx()
	{
		return playBGMidx;
	}

    public float GetBGMLengthFromList(int idx)
    {
        return BGMS[idx].length;
    }

    public void PlayBGMCurrentSource(int idx, int starttime = 0, bool restart = false)
    {
        // 如果沒 source, 就用第一個.
        if (CurrentBGMSourceID == BGMSourceID.BGM_NONE)
        {
            playBGMidx = idx;
            CurrentBGMSourceID = BGMSourceID.BGM1;
            BGMCurrentSource = BGMSource1;

            BGMSource1.clip = BGMS[idx];
            BGMSource1.timeSamples = starttime;
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
            if (BGMCurrentSource.isPlaying)
            {
                if (restart)
                {
                    BGMCurrentSource.timeSamples = starttime;
                    BGMCurrentSource.Play();
                }
            }
            else
            {
                BGMCurrentSource.timeSamples = starttime;
                BGMCurrentSource.Play();
            }
        }

    }

    // 切換背景音樂.
    // 因為會淡出淡出, 所以 source 會換來換去.
	public void PlayBGMSwitch(int idx, int starttime = 0, bool restart = false)
	{
        Debug.Log("PlayBGMSwitch");
		if (playBGMidx == idx)
		{
			return;
		}
		playBGMidx = idx;
		//切換 bgm.
		if (CurrentBGMSourceID == BGMSourceID.BGM_NONE)
		{
			CurrentBGMSourceID = BGMSourceID.BGM1;
            BGMCurrentSource = BGMSource1;

            BGMSource1.clip = BGMS[playBGMidx];
			BGMSource1.timeSamples = starttime;
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
                BGMCurrentSource = BGMSource2;
                BGMSource2.clip = BGMS[playBGMidx];
				BGMSource2.timeSamples = starttime;
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
                BGMCurrentSource = BGMSource1;
                BGMSource1.clip = BGMS[playBGMidx];
				BGMSource1.timeSamples = starttime;
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
//		for (int i = 0; i < FXSources.Length; i++)
//		{
//			//if (!FXSources[i].isPlaying)
//			{
//				FXSources[i].PlayOneShot(FxClips[idx]);
//			}
//		}
		FXSources[playFXidx].PlayOneShot(FxClips[idx]);
        playFXidx++;
		playFXidx %= FXSources.Length;
	}

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("play"))
    //    {
            
    //        BGMSource1.Play();

    //    }
    //    if (GUILayout.Button("stop"))
    //    {
    //        BGMSource1.Stop();

    //    }
    //}
}
