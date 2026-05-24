using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class SetBuildingColliderType : EditorWindow
{
    [MenuItem("Tools/Set Building Collider Types")]
    public static void Run()
    {
        string buildingsFolder = "Assets/Map/Sprites/Buildings";
        int grid = 0;
        int none = 0;

        // Surenkam visus tile asset'us
        string[] tileGuids = AssetDatabase.FindAssets("t:Tile");

        foreach (string guid in tileGuids)
        {
            string tilePath = AssetDatabase.GUIDToAssetPath(guid);
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
            if (tile == null || tile.sprite == null) continue;

            string spritePath = AssetDatabase.GetAssetPath(tile.sprite);

            if (spritePath.StartsWith(buildingsFolder))
            {
                tile.colliderType = Tile.ColliderType.Grid;
                grid++;
            }
            else
            {
                tile.colliderType = Tile.ColliderType.None;
                none++;
            }

            EditorUtility.SetDirty(tile);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Baigta! Buildings (Grid): {grid}, Kiti (None): {none}");
    }
}
