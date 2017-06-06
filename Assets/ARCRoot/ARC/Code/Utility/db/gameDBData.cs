using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameDBData
{
	public arcCSVReader csvreader;

	public Dictionary<string, int> colidx = new Dictionary<string, int>();
	int cacheRowIdx = 0;

	public gameDBData(arcCSVReader csvr)
	{
		csvreader = csvr;
		CacheColIdx();
	}

	public int GetRowCountY()
	{
		return csvreader.GetRowCountY() - 1;
	}
	public int GetColCountX()
	{
		return csvreader.GetColCountX();
	}


	public void CacheColIdx()
	{
		for (int i = 0; i < csvreader.mData[0].Length; i++)
		{
			if (!colidx.ContainsKey(csvreader.mData[0][i]))
			{
				colidx.Add(csvreader.mData[0][i], i);
			}
		}
	}

	public void DebugOutput()
	{
		string de = csvreader.mFileName + " keys\n";
		foreach (KeyValuePair<string, int> kp in colidx)
		{
			de += kp.Key + " " + kp.Value + "\n";
		}
		Debug.Log(de);

	}

//	public bool GetVal(ref string outstr, string keyname, string colname)
//	{
//		return csvreader.GetVal(ref outstr, keyname, colidx[colname]);
//	}

	public bool GetVal(ref string outstr, string keyname, string colname)
	{
		
		if (!colidx.ContainsKey(colname))
		{
			Debug.Log("no col name : " + colname);
			return false;
		}

		if ((csvreader.mData.Count > 0)
		    && (csvreader.mData[cacheRowIdx][0] == keyname)
		    )
		{
			outstr = csvreader.mData[cacheRowIdx][colidx[colname]];
			return true;
		}

		for(int i = 0;i < csvreader.mData.Count; i++)
		{

			if (csvreader.mData[i][0] == keyname)
			{
				cacheRowIdx = i;
				outstr = csvreader.mData[i][colidx[colname]];
				return true;
			}
		}
		
		return false;

	}
}
