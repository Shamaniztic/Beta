using UnityEngine;
using UnityEditor;

public class CreateTilePrefabsFromSlices : EditorWindow
{
    [MenuItem("Tools/Create Prefabs from Sliced Sprites")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateTilePrefabsFromSlices));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create Prefabs from Slices"))
        {
            CreatePrefabsFromSlices();
        }
    }

    private static void CreatePrefabsFromSlices()
    {
        string path = EditorUtility.OpenFilePanel("Select Sprite Sheet", "", "png,jpg");
        if (string.IsNullOrEmpty(path)) return;

        string assetPath = path.Substring(path.IndexOf("Assets"));
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        string prefabsDirectory = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabsDirectory))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        foreach (Object asset in assets)
        {
            if (asset is Sprite)
            {
                Sprite sprite = asset as Sprite;
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;
                string prefabPath = prefabsDirectory + "/" + sprite.name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
                DestroyImmediate(go);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created prefabs for all slices in the sprite sheet.");
    }
}
