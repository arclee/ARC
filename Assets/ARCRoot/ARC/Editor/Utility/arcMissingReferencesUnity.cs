using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.SceneManagement;


/// <summary>
/// A helper editor script for finding missing references to objects.
/// 修改自 https://github.com/liortal53/MissingReferencesUnity/blob/master/Assets/MissingReferencesFinder/Editor/MissingReferencesFinder.cs
/// 加入找  missing method 功能. 
/// </summary>
public class arcMissingReferencesFinder : MonoBehaviour
{
    private const string MENU_ROOT = "Tools/Missing References/";

    /// <summary>
    /// Finds all missing references to objects in the currently loaded scene.
    /// </summary>
    [MenuItem(MENU_ROOT + "Search in scene", false, 50)]
    public static void FindMissingReferencesInCurrentScene()
    {
        var sceneObjects = GetSceneObjects();
        //FindMissingReferences(EditorApplication.currentScene, sceneObjects);
        FindMissingReferences(EditorSceneManager.GetActiveScene().name, sceneObjects);
        
        
    }

    /// <summary>
    /// Finds all missing references to objects in all enabled scenes in the project.
    /// This works by loading the scenes one by one and checking for missing object references.
    /// </summary>
    [MenuItem(MENU_ROOT + "Search in all scenes", false, 51)]
    public static void MissingSpritesInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
        {
            //EditorApplication.OpenScene(scene.path);
            EditorSceneManager.OpenScene(scene.path);
            FindMissingReferencesInCurrentScene();
        }
    }

    /// <summary>
    /// Finds all missing references to objects in assets (objects from the project window).
    /// </summary>
    [MenuItem(MENU_ROOT + "Search in assets", false, 52)]
    public static void MissingSpritesInAssets()
    {
        var allAssets = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith("Assets/")).ToArray();
        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

        FindMissingReferences("Project", objs);
    }

    private static void FindMissingReferences(string context, GameObject[] objects)
    {
        foreach (var go in objects)
        {
            var components = go.GetComponents<Component>();

            foreach (var c in components)
            {
                // Missing components will be null, we can't find their type, etc.
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + GetFullPath(go), go);
                    continue;
                }

                SpecificComponentsCheck(go, c);

                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();

                // Iterate over the components' properties.
                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                        }
                    }
                }
            }
        }
    }

    private static void SpecificComponentsCheck(GameObject go, Component c)
    {

        if (c.GetType() == typeof(EventTrigger))
        {
            EventTrigger Comp = c as EventTrigger;

            for (int i = 0; i < Comp.triggers.Count; i++)
            {
                for (int j = 0; j < Comp.triggers[i].callback.GetPersistentEventCount(); j++)
                {
                    string MethoName = Comp.triggers[i].callback.GetPersistentMethodName(j);
                    System.Reflection.MethodInfo MI = Comp.triggers[i].callback.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                    if (MI == null)
                    {
                        Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                    }
                }
            }
        }
        else if (c.GetType() == typeof(Button))
        {
            Button Comp = c as Button;     
            for (int j = 0; j < Comp.onClick.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onClick.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onClick.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }
        }
        else if (c.GetType() == typeof(Toggle))
        {
            Toggle Comp = c as Toggle;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onValueChanged.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }
        }
        else if (c.GetType() == typeof(Slider))
        {
            Slider Comp = c as Slider;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onValueChanged.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }
        }
        else if (c.GetType() == typeof(Scrollbar))
        {
            Scrollbar Comp = c as Scrollbar;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onValueChanged.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }
        }
        else if (c.GetType() == typeof(Dropdown))
        {
            Dropdown Comp = c as Dropdown;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onValueChanged.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }
        }
        else if (c.GetType() == typeof(InputField))
        {
            InputField Comp = c as InputField;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onValueChanged.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }

            for (int j = 0; j < Comp.onEndEdit.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onEndEdit.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onEndEdit.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }
        }
        else if (c.GetType() == typeof(ScrollRect))
        {
            ScrollRect Comp = c as ScrollRect;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                System.Reflection.MethodInfo MI = Comp.onValueChanged.GetPersistentTarget(j).GetType().GetMethod(MethoName);
                if (MI == null)
                {
                    Debug.LogError("Missing Method : " + MethoName + " in GO: " + GetFullPath(go), go);
                }
            }

        }
    
    
    }

    private static GameObject[] GetSceneObjects()
    {
        // Use this method since GameObject.FindObjectsOfType will not return disabled objects.
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
                   && go.hideFlags == HideFlags.None).ToArray();
    }

    private static void ShowError(string context, GameObject go, string componentName, string propertyName)
    {
        var ERROR_TEMPLATE = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";

        Debug.LogError(string.Format(ERROR_TEMPLATE, GetFullPath(go), componentName, propertyName, context), go);
    }

    private static string GetFullPath(GameObject go)
    {
        return go.transform.parent == null
            ? go.name
                : GetFullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}