using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public sealed class arcErrCollector : arcSingleton<arcErrCollector>
{
	static public bool mEnable = true;

	private arcErrCollector()
	{

	}

	public class ErrorData
	{
		public GameObject obj = null;
		public String msg;
	}

	static public List<ErrorData> mNullErrorObjs = new List<ErrorData>();

	static public void Clear()
	{
		mNullErrorObjs.Clear();

	}

	static public void Add(GameObject obj, String errmsg)
	{
		if (!mEnable)
		{
			return;
		}
		ErrorData ed = new ErrorData();
		ed.obj = obj;
		ed.msg = errmsg;

		mNullErrorObjs.Add(ed);

	}


}
