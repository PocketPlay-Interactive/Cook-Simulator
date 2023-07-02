using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEditor
{

    [MenuItem("Scene/Loading %s1")]
    static void OpenSceneWorldCup()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        EditorSceneManager.OpenScene("Assets/_CAT/Loading.unity");
    }

    [MenuItem("Scene/Game %s2")]
    static void OpenSceneSortBall()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        EditorSceneManager.OpenScene("Assets/_CAT/Game.unity");
    }

    [MenuItem("Tools/Create Prefabs")]
    static void CreatePrefabs()
    {
        // Lấy danh sách các đối tượng đang được chọn
        GameObject[] selectedObjects = Selection.gameObjects;

        // In ra tên của các đối tượng đang được chọn
        foreach (GameObject obj in selectedObjects)
        {
            string path = $"Assets/Resources/prefabs/{obj.name}.prefab";
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existingPrefab != null)
            {
                // Cập nhật Prefab đã có sẵn nếu nó đã tồn tại
                PrefabUtility.ReplacePrefab(obj, existingPrefab, ReplacePrefabOptions.ReplaceNameBased);
            }
            else
            {
                //Tạo Prefab từ đối tượng
                PrefabUtility.CreatePrefab($"Assets/Resources/prefabs/{obj.name}.prefab", obj);
            }



            //// Xóa đối tượng trong cảnh
            //DestroyImmediate(obj);
        }
    }

    [MenuItem("Tools/Change Name")]
    static void ChangeNamePrefabs()
    {
        // Lấy danh sách các đối tượng đang được chọn
        GameObject[] selectedObjects = Selection.gameObjects;

        // In ra tên của các đối tượng đang được chọn
        foreach (GameObject obj in selectedObjects)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if(sr != null && sr.sprite != null)
            {
                var str = sr.sprite.name;
                var splitStr = str.Split('-');
                var name = "_icon-" + splitStr[0];
                obj.name = name;
            }
            else
            {
                Debug.Log(obj.name);
            }
        }
    }
}
