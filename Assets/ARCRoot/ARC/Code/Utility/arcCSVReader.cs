using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

public class arcCSVReader
{

	string mFileName = "";

	TextAsset mTextAsset = null;

	public List<string[]> mData = new List<string[]>();

	public bool LoadFromResource(string filename)
	{
		mFileName = filename;
		mData.Clear();
		mTextAsset = null;
		mTextAsset = Resources.Load(mFileName) as TextAsset;

		if (mTextAsset == null)
		{
			return false;
		}

		Parse();
		mTextAsset = null;
		return true;

	}

	void Parse()
	{
		//取出每一行.
		string [] lineArray = mTextAsset.text.Split(new string[] {"\n", "\r\n"}, System.StringSplitOptions.RemoveEmptyEntries);

		//存放.
		for(int i =0;i < lineArray.Length; i++)
		{
			//略過.
			if (lineArray[i].Length > 0)
			{
				if (lineArray[i][0] == '#' || lineArray[i][0] == '\r' || lineArray[i][0] == '\n')
				{
				    continue;
				}
			}
			
			if (lineArray[i].Length > 1)
			{
				if ((lineArray[i][0] == '\\' && lineArray[i][1] == '\\') || (lineArray[i][0]=='/' && lineArray[i][1]=='/'))
				{
					continue;
				}
			}

			mData.Add(SplitCsvLine(lineArray[i]));
		}

	}

	// splits a CSV row 
	static public string[] SplitCsvLine(string line)
	{
		string pattern = @"
		    # Match one value in valid CSV string.
		    (?!\s*$)                                      # Don't match empty last value.
		    \s*                                           # Strip whitespace before value.
		    (?:                                           # Group for value alternatives.
		      '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
		    | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
		    | (?<val>[^,'""\s]*(?:\s+[^,'""\s]+)*)    		# or $3: Non-comma, non-quote stuff.
		    )                                             # End group of value alternatives.
		    \s*                                           # Strip whitespace after value.
		    (?:,|$)                                       # Field ends on comma or EOS.
		    ";

		//[^,'""\s] 非 , ' " \s 的字都選起來

		string[] values = (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(
							line
							, pattern
			, System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline)
		                   select m.Groups[1].Value).ToArray();

		return values;
	}

	
	// outputs the content of a 2D array, useful for checking the importer
	public void DebugOutput()
	{		
		string textOutput = ""; 
		for(int i =0;i < mData.Count; i++)
		{
			for (int j = 0; j < mData[i].Length; j++)
			{
				textOutput += mData[i][j];
				textOutput += ","; 
			}
			textOutput += "\n"; 
		}
		Debug.Log(textOutput);
	}
}
