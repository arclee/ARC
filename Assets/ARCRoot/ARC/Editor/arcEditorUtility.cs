using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[InitializeOnLoad]
public static class arcEditorUtility
{
	
	[MenuItem(arcMenu.WindowRoot + "About", false, 14000)]
	public static void AboutARC()
	{
		EditorUtility.DisplayDialog("About ARC",
		                            "Arc's package for game!",
		                            "Ok");
	}

	
	[MenuItem(arcMenu.WindowRoot + "Open PersistentDataPath", false)]
	public static void OpenPersistentDataPath()
	{
		EditorUtility.RevealInFinder(Application.persistentDataPath);
	}
	
	[MenuItem(arcMenu.WindowRoot + "Open Assets Store Download Path", false)]
	public static void OpenAssetsStoreDownloadPath()
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		EditorUtility.RevealInFinder(path + "/Library/Unity/Asset Store-5.x/");

	}

	public static void OpenInMac(string path)
	{
		bool openInsidesOfFolder = false;
		
		// try mac
		string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes
		
		if ( System.IO.Directory.Exists(macPath) ) // if path requested is a folder, automatically open insides of that folder
		{
			openInsidesOfFolder = true;
		}
		
		if ( !macPath.StartsWith("\"") )
		{
			macPath = "\"" + macPath;
		}
		
		if ( !macPath.EndsWith("\"") )
		{
			macPath = macPath + "\"";
		}
		
		string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

		try
		{
			System.Diagnostics.Process.Start("open", arguments);
		}
		catch ( System.ComponentModel.Win32Exception e )
		{
			// tried to open mac finder in windows
			// just silently skip error
			// we currently have no platform define for the current OS we are in, so we resort to this
			e.HelpLink = ""; // do anything with this variable to silence warning about not using it
		}
	}
}
