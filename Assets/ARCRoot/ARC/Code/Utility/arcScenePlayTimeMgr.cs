using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class arcScenePlayTimeMgr : arcSceneSingleton<arcScenePlayTimeMgr>
{
    [Serializable]
    public class TimeData
    {
        public bool Enable;
        public int ID;
        public string Name;
        public float DeltaTime;
        public float TotalTime;
        //多人操作暫停時, 以最後一個人解除為主.
        public int pauseCount;
        public float scale = 1;
        public TimeData(int id, string name)
        {
            ID = id;
            Name = name;
            Reset();
        }

        public void Reset()
        {
            Enable = true;
            DeltaTime = 0;
            TotalTime = 0;
            pauseCount = 0;
            scale = 1;
        }

        public void PauseWithCount()
        {
            DeltaTime = 0;
            pauseCount++;
        }

        public void PlayWithCount()
        {
            pauseCount--;
            pauseCount = Math.Max(pauseCount, 0);
        }

        public bool isPlaying()
        {
            return pauseCount == 0;
        }
    }
    
    public TimeData[] TimeList;


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < TimeList.Length; i++)
        {
            if (TimeList[i].Enable && TimeList[i].isPlaying())
            {
                TimeList[i].DeltaTime = Time.deltaTime * TimeList[i].scale;
                TimeList[i].TotalTime += TimeList[i].DeltaTime;
            }
            else
            {
                TimeList[i].DeltaTime = 0;
            }
        }
    }

    public TimeData GetTimeData(int id)
    {
        if (id < TimeList.Length)
        {
            return TimeList[id];
        }

        return null;
    }

    public TimeData GetTimeData(string name)
    {
        for (int i = 0; i < TimeList.Length; i++)
        {
            if (TimeList[i].Name == name)
            {
                return TimeList[i];
            }
        }
 
        return null;
    }

    //public TimeData CrateTime(int id, string name)
    //{
    //    TimeData td = GetTimeData(id);

    //    if (td != null)
    //    {
    //        return td;
    //    }

    //    TimeData timedata = new TimeData(id, name);

    //    TimeList.Add(id, timedata);

    //    return timedata;
    //}

    public float GetDeltaTime(int id)
    {
        TimeData td = GetTimeData(id);

        if (td != null)
        {
            return td.DeltaTime;
        }

        Debug.Log("Time not found:" + id);

        return 0;
    }

    public float GetDeltaTime(string name)
    {
        TimeData td = GetTimeData(name);

        if (td != null)
        {
            return td.DeltaTime;
        }

        Debug.Log("Time not found:" + name);

        return 0;
    }

    public float GetTotalTime(int id)
    {
        TimeData td = GetTimeData(id);

        if (td != null)
        {
            return td.TotalTime;
        }

        Debug.Log("Time not found:" + id);

        return 0;
    }

    public float GetTotalTime(string name)
    {
        TimeData td = GetTimeData(name);

        if (td != null)
        {
            return td.TotalTime;
        }

        Debug.Log("Time not found:" + name);

        return 0;
    }

    public void SetEnable(int id, bool enable)
    {
        TimeData td = GetTimeData(id);
        if (td != null)
        {
            td.Enable = enable;
        }
    }
    public void SetEnable(string name, bool enable)
    {
        TimeData td = GetTimeData(name);
        if (td != null)
        {
            td.Enable = enable;
        }
    }

    public void SetScale(int id, float scale)
    {
        TimeData td = GetTimeData(id);

        if (td != null)
        {
            td.scale = scale;
        }
    }
    public void SetScale(string name, float scale)
    {
        TimeData td = GetTimeData(name);

        if (td != null)
        {
            td.scale = scale;
        }
    }
    

    public void GetBackupEnable(ref TimeData[] backuplist)
    {
        backuplist = TimeList;        
    }

    public void SetRestoreEnable(ref TimeData[] backuplist)
    {
        for (int i = 0; i < backuplist.Length; i++)
        {
            TimeList[i].Enable = backuplist[i].Enable;
        }
    }
}
