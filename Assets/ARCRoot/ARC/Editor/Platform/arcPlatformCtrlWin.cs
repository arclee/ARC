using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class arcPlatformCtrlWin  : EditorWindow
{


	public static string arcPlatformCacheDir = "/../arcPlatformCache/";
	
	public static string arcUnityLibraryDir = "/../Library/";
	public static string arcUnityLibraryDirName = "/../Library";
	//progres bar.
	arcCancelableProgressBar mPorgBar = new arcCancelableProgressBar("working...", "Click \"Cancel\" to stop");

	int lineheight = 50;
	int newline = 50;
	int fieldwidth = 100;

	bool isCopying = false;
	int filestocopycount = 0;
	int copyedfilecount = 0;
	bool userestopcopy = false;
	[MenuItem (arcMenu.WindowRoot + "Platfrom/PlatfromCtrl")]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		arcPlatformCtrlWin window = (arcPlatformCtrlWin)EditorWindow.GetWindow (typeof (arcPlatformCtrlWin));
		window.Show();
	}

	string GetCacheDir(BuildTarget targetname)
	{
		
		return Application.dataPath + arcPlatformCacheDir + targetname.ToString();
	}

	string GetLibDir()
	{
		return Application.dataPath + arcUnityLibraryDir;
	}
	string GetLibDirName()
	{
		return Application.dataPath + arcUnityLibraryDirName;
	}

	void ReLinkToCache(BuildTarget targetname)
	{
		if (System.IO.Directory.Exists(GetLibDirName()))
		{
			Debug.Log("Lib Exists " + GetLibDirName());
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.FileName = @"rm";

			DirectoryInfo directory_info = new DirectoryInfo(GetLibDirName());
			Debug.Log(directory_info.Attributes);
			if ((directory_info.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
			{
				Debug.Log("Delete symblink");
				p.StartInfo.Arguments =  @" " + GetLibDirName();
			}
			else
			{
				Debug.Log("del libdir");
				p.StartInfo.Arguments =  @" -rf " + GetLibDirName();
			}
			//System.Diagnostics.Process p = System.Diagnostics.Process.Start(@"rm -rf " + GetLibDir());
			p.Start();
			//Debug.Log(p.ExitCode);
		
		}

		while(System.IO.Directory.Exists(GetLibDirName()))
		{

		}

		System.Diagnostics.Process lnp = new System.Diagnostics.Process();
		lnp.StartInfo.FileName = @"ln";
		lnp.StartInfo.Arguments =  @"-s " + GetCacheDir(targetname) + " " + GetLibDirName();
		lnp.Start();

		
		while(!System.IO.Directory.Exists(GetLibDirName()))
		{
			
		}

		
		DirectoryInfo directory_infofinal = new DirectoryInfo(GetLibDirName());
		Debug.Log(directory_infofinal.Attributes);
		
	}

	void SwitchPlatform(BuildTarget targetname)
	{
		//backup old cache. ex:android.
		DirectoryInfo directory_info = new DirectoryInfo(GetLibDirName());
		
		if ((directory_info.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
		{
		}
		else
		{
			Debug.Log("Backup old");
			CreatCache(EditorUserBuildSettings.activeBuildTarget);
		}


		//create new cache. ex:web.
		string arcdir = GetCacheDir(targetname);
		if (!System.IO.Directory.Exists(arcdir))
		{
			Debug.Log("create new from old");
			System.IO.Directory.CreateDirectory(arcdir);
			CreatCache(targetname);

		}

		ReLinkToCache(targetname);

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetname);
	}

	bool CreatCache(BuildTarget targetname)
	{
		
		string arcdir = GetCacheDir(targetname);
		if (!System.IO.Directory.Exists(arcdir))
		{
			System.IO.Directory.CreateDirectory(arcdir);

		}
		
		isCopying = true;
		userestopcopy = false;
		copyedfilecount = 0;
		filestocopycount = GetFileCount(GetLibDir(), true);
		mPorgBar.Show();
		DirectoryCopy(GetLibDir(), arcdir, true);
		mPorgBar.Close();
		isCopying = false;
		//Debug.Log(arcdir);

		return true;
	}
	
	bool DeleteCache(BuildTarget targetname)
	{
		
		string arcdir = GetCacheDir(targetname);
		if (System.IO.Directory.Exists(arcdir))
		{
			System.IO.Directory.Delete(arcdir, true);
		}
		Debug.Log(arcdir);
		
		return true;
	}

	void ShowPlatformButton(ref Rect rect, BuildTarget targetname)
	{
		rect.y += newline;
		GUI.Label(rect, targetname.ToString());

		float oldx = rect.position.x;
		rect.x += fieldwidth;

		if (!isCopying)
		{
			if (EditorUserBuildSettings.activeBuildTarget == targetname)
			{
				string arcdir = GetCacheDir(targetname);
				if (!System.IO.Directory.Exists(arcdir))
				{
					if (GUI.Button(rect, "Create Cache"))
					{
						CreatCache(targetname);
					}
				}
				else
				{
//					if (GUI.Button(rect, "Delete Cache"))
//					{
//						DeleteCache(targetname);
//					}

				}
			}
			else
			{
				GUI.Label(rect, "");
			}
		}

		
		rect.x += fieldwidth;
		if (EditorUserBuildSettings.activeBuildTarget != targetname)
		{
			//int filecount = GetFileCount(GetCacheDir(targetname), true);
			//if (filecount > 1)
			{
				if (GUI.Button(rect, "switch"))
				{
					SwitchPlatform(targetname);
				}
			}
		}
		else
		{
			GUI.Label(rect, "Current");
		}

//		rect.x += fieldwidth;
//		if (GUI.Button(rect, "switch2"))
//		{
//			EditorUserBuildSettings.SwitchActiveBuildTarget(targetname);
//		}

		rect.x = oldx;
	}


	void DrawProgress(Rect rect, float progress)
	{

		GUI.Box(rect, "");
		rect.size = new Vector2(rect.size.x * progress, rect.size.y * .5f);
		GUI.Box(rect, "");
	}

	int GetFileCount(string sourceDirName, bool copySubDirs)
	{
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);
		
		int filescount = 0;
		if (!dir.Exists)
		{
			return filescount;
		}

		filescount += dir.GetFiles().Length;
		
		if (copySubDirs)
		{
			DirectoryInfo[] dirs = dir.GetDirectories();
			for (int i = 0; i< dirs.Length; i++)
			{
				DirectoryInfo subdir = dirs[i];
				filescount += GetFileCount(subdir.FullName, copySubDirs);
			}
		}

		return filescount;
	}

	void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
	{
		// Get the subdirectories for the specified directory.
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);
		
		if (!dir.Exists)
		{
			isCopying = false;
			throw new DirectoryNotFoundException(
				"Source directory does not exist or could not be found: "
				+ sourceDirName);
		}

		// If the destination directory doesn't exist, create it.
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}



		// Get the files in the directory and copy them to the new location.
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			string temppath = Path.Combine(destDirName, file.Name);
			file.CopyTo(temppath, true);
			copyedfilecount++;

			if (mPorgBar.Update(copyedfilecount, filestocopycount))
			{
				userestopcopy = true;
			}
		
			if (userestopcopy)
			{
				break;
			}
		}
		
		// If copying subdirectories, copy them and their contents to new location.
		if (copySubDirs)
		{
			DirectoryInfo[] dirs = dir.GetDirectories();
			foreach (DirectoryInfo subdir in dirs)
			{
				string temppath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath, copySubDirs);

			}
		}

	}

	void OnGUI ()
	{
		Rect rect = new Rect(0, 0, 100, lineheight);
		GUI.Label(rect, "PlatformCtr");
	

		ShowPlatformButton(ref rect, BuildTarget.WebPlayer);
		ShowPlatformButton(ref rect, BuildTarget.Android);
		ShowPlatformButton(ref rect, BuildTarget.iOS);


		
	}
}
