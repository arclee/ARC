using UnityEngine;
using UnityEditor;
using System.Collections;

public class arcCancelableProgressBar
{
	int mProgressUpdateCount = 500;
	int mProgressCount = 0;

	string mTitle = "";
	string mMsg = "";
	public arcCancelableProgressBar(string title, string msg)
	{
		mTitle = title;
		mMsg = msg;
	}

	public void SetUpdateCount(int count)
	{
		mProgressUpdateCount = count;
	}

	public void Show()
	{
		EditorUtility.DisplayCancelableProgressBar(mTitle,	mMsg, 0);
	}

	public void Close()
	{
		EditorUtility.ClearProgressBar();
	}
	
	public bool Update(float porcrate)
	{
		if (mProgressCount++ >= mProgressUpdateCount)
		{
			mProgressCount = 0;
			if(EditorUtility.DisplayCancelableProgressBar(mTitle,	mMsg, porcrate))
			{
				return true;
			}
		}
		return false;
	}

	public bool Update(int curretproc, int totalproc)
	{
		float procrate = (float)curretproc/(float)totalproc;
		return Update(procrate);
	}
}
