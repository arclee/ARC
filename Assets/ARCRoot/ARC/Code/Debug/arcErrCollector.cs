using UnityEngine;
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
		public string msg;
		public string filepathname;
	}

	static public List<ErrorData> mNullErrorObjs = new List<ErrorData>();

	static public void Clear()
	{
		mNullErrorObjs.Clear();

	}

	static public void Add(string errmsg, GameObject obj, string filepathname)
	{
		if (!mEnable)
		{
			return;
		}
		ErrorData ed = new ErrorData();
		ed.obj = obj;
		ed.msg = errmsg;
		ed.filepathname = filepathname;

		mNullErrorObjs.Add(ed);

	}


}
