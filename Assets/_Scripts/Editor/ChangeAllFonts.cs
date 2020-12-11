using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAllFonts : EditorWindow
{
    private Font _newFont;
    private GameObject[] _prefabs;
    
    [MenuItem("Window/ChangeFonts")]
    private static void OpenWindow()
    {
        var window = GetWindow<ChangeAllFonts>();
        window.titleContent = new GUIContent("Font Changer");
        window.name = "FontChanger";
        window.Show();
    }

    private void OnGUI()
    {
        _newFont = (Font)EditorGUILayout.ObjectField(_newFont, typeof(Font), true);
        if (_newFont == null)
        {
            EditorGUILayout.HelpBox("Choose font", MessageType.Warning);
            return;
        }
        if (GUILayout.Button("Change font"))
        {
            float progress = 0;
            int objectscount;
            int counter = 0;
            EditorUtility.DisplayProgressBar("Compiling", "Wait for change fonts", progress);
            var texts = GetSceneObjectsNonGeneric();
            _prefabs = GetAllPrefabsFromFolder();
            objectscount = texts.Count + _prefabs.Length;
            //Change all prefabs
            foreach (var prefab in _prefabs)
            {
                Text[] prefabTexts = prefab.GetComponentsInChildren<Text>();
                foreach (var prefabText in prefabTexts)
                {
                    if (prefabText != null)
                    {
                        prefabText.font = _newFont;
                        PrefabUtility.SavePrefabAsset(prefab);
                    }
                }

                counter++;
                progress = 1 / objectscount * counter;
            }
            //Change all scene objects
            foreach (var textObj in texts)
            {
                var text = (Text) textObj;
                text.font = _newFont;
                EditorUtility.SetDirty(text);
            }

            var mainScene = EditorSceneManager.GetSceneByName("Main Scene");
            EditorSceneManager.SaveScene(mainScene);
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }
    }
    
    List<UnityEngine.Object> GetSceneObjectsNonGeneric()
    {
        List<UnityEngine.Object> objectsInScene = new List<UnityEngine.Object>();

        foreach (UnityEngine.Object go in Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)))
        {
            Text cGO = go as Text;
            if (cGO != null && !EditorUtility.IsPersistent(cGO.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }

    GameObject[] GetAllPrefabsFromFolder()
    {
        var list = Resources.LoadAll<GameObject>("Prefabs");
        return list;
    }
}
