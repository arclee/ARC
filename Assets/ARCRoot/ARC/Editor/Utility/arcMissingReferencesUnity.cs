using System.Collections;
using System.Linq;
using System.Reflection;
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
                    Object TObj = Comp.triggers[i].callback.GetPersistentTarget(j);
                    checkMethords(go, TObj, MethoName);
                }
            }
        }
        else if (c.GetType() == typeof(Button))
        {
            Button Comp = c as Button;

            for (int j = 0; j < Comp.onClick.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onClick.GetPersistentMethodName(j);
                Object TObj = Comp.onClick.GetPersistentTarget(j);                
                checkMethords(go, TObj, MethoName);
            }
        }
        else if (c.GetType() == typeof(Toggle))
        {
            Toggle Comp = c as Toggle;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                Object TObj = Comp.onValueChanged.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }
        }
        else if (c.GetType() == typeof(Slider))
        {
            Slider Comp = c as Slider;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                Object TObj = Comp.onValueChanged.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }
        }
        else if (c.GetType() == typeof(Scrollbar))
        {
            Scrollbar Comp = c as Scrollbar;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                Object TObj = Comp.onValueChanged.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }
        }
        else if (c.GetType() == typeof(Dropdown))
        {
            Dropdown Comp = c as Dropdown;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                Object TObj = Comp.onValueChanged.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }
        }
        else if (c.GetType() == typeof(InputField))
        {
            InputField Comp = c as InputField;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                Object TObj = Comp.onValueChanged.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }

            for (int j = 0; j < Comp.onEndEdit.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onEndEdit.GetPersistentMethodName(j);
                Object TObj = Comp.onEndEdit.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }
        }
        else if (c.GetType() == typeof(ScrollRect))
        {
            ScrollRect Comp = c as ScrollRect;
            for (int j = 0; j < Comp.onValueChanged.GetPersistentEventCount(); j++)
            {
                string MethoName = Comp.onValueChanged.GetPersistentMethodName(j);
                Object TObj = Comp.onValueChanged.GetPersistentTarget(j);
                checkMethords(go, TObj, MethoName);
            }

        }       
    }

    private static void checkMethords(GameObject go, UnityEngine.Object tob, string methordname)
    {
        try
        {
            if (tob == null)
            {
                Debug.LogError("Missing Method : " + methordname + " in GO: " + GetFullPath(go), go);
                return;
            }

            string objectFullNameWithNamespace = tob.GetType().FullName;
            if (!classExist(tob.GetType().Assembly, objectFullNameWithNamespace))
            {
                Debug.LogError("Missing Method : " + methordname + " in GO: " + GetFullPath(go), go);
                return;
            }

            bool isfind = false;
            int methordcount = 0;
            MethodInfo[] MI = tob.GetType().GetMethods();
            for (int i = 0; i < MI.Length; i++)
            {
                if (MI[i].Name == methordname && MI[i].IsPublic)
                {
                    methordcount++;
                    isfind = true;
                }
            }
            
            if (methordcount > 1)
            {
                Debug.LogWarning("Missing Method (multi same name) YOU SHOULD CHECK: " + methordname + " in GO: " + GetFullPath(go), go);
            }

            if (!isfind)
            {
                Debug.LogError("Missing Method : " + methordname + " in GO: " + GetFullPath(go), go);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
        

    // 來自 https://stackoverflow.com/questions/42784338/unity-missing-warning-when-button-has-missing-onclick .
    //Checks if class exit or has been renamed
    private static bool classExist(Assembly asm, string className)
    {        
        System.Type[] tps = asm.GetTypes();
        for (int i = 0; i < tps.Length; i++)
        {
            if (tps[i].FullName == className)
            {
                return true;
            }
        }

        return false;
    }

    //Checks if functions exist as public function
    private static bool functionExistAsPublicInTarget(UnityEngine.Object target, string functionName)
    {
        System.Type type = target.GetType();
        MethodInfo targetinfo = type.GetMethod(functionName);
        return targetinfo != null;
    }

    //Checks if functions exist as private function
    private static bool functionExistAsPrivateInTarget(UnityEngine.Object target, string functionName)
    {
        System.Type type = target.GetType();
        MethodInfo targetinfo = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.NonPublic);
        return targetinfo != null;
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