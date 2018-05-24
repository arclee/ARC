using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(arcScenePlayTimeMgr))]
public class arcScenePlayTimeMgrEditor : Editor
{
    //static int TimeID = 0;
    //static string TimeName = "";
    //static List<int> RemoveList = new List<int>();
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

#if false
        arcScenePlayTimeMgr spt = target as arcScenePlayTimeMgr;

        RemoveList.Clear();
        EditorGUILayout.BeginVertical();
        TimeID = EditorGUILayout.IntField("ID", TimeID);
        TimeName = EditorGUILayout.TextField("Name", TimeName);
        if (GUILayout.Button("Add"))
        {
            spt.CrateTime(TimeID, TimeName);
        }
        EditorGUILayout.EndVertical();


        //-============
        EditorGUILayout.BeginVertical();
        foreach (KeyValuePair<int, arcScenePlayTimeMgr.TimeData> kp in spt.TimeList)
        {
            EditorGUILayout.Space();            
            kp.Value.Enable = EditorGUILayout.Toggle("Enable", kp.Value.Enable);
            kp.Value.ID = EditorGUILayout.IntField("ID", kp.Value.ID);
            kp.Value.Name = EditorGUILayout.TextField("Name", TimeName);
            kp.Value.scale = EditorGUILayout.FloatField("Scale", kp.Value.scale);
            //GUILayout.Label(kp.Value.Name);

            if (GUILayout.Button("X"))
            {
                RemoveList.Add(kp.Key);
            }
        }

        for (int i = 0; i < RemoveList.Count; i++)
        {
            spt.TimeList.Remove(RemoveList[i]);
        }

        EditorGUILayout.EndVertical();
#endif
    }
}
