using UnityEngine;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using System.Collections.Generic;

public class SetSpritePhysicsShapes : EditorWindow
{
    [MenuItem("Tools/Fix Building Colliders")]
    public static void Run()
    {
        string[] noneFolders = new[]
        {
            "Assets/Map/Sprites/Road",
            "Assets/Map/Sprites/Nature",
            "Assets/Map/Sprites/Water",
            "Assets/Map/Sprites/Extra"
        };

        int cleared = 0;

        foreach (string folder in noneFolders)
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null) continue;

                var factory = new SpriteDataProviderFactories();
                factory.Init();
                var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
                dataProvider.InitSpriteEditorDataProvider();

                var physicsProvider = dataProvider.GetDataProvider<ISpritePhysicsOutlineDataProvider>();
                var spriteRects = dataProvider.GetSpriteRects();

                foreach (var rect in spriteRects)
                {
                    physicsProvider.SetOutlines(rect.spriteID, new List<Vector2[]>());
                }

                dataProvider.Apply();
                importer.SaveAndReimport();
                cleared++;
            }
        }

        Debug.Log($"Baigta! Išvalyti {cleared} sprites'ų physics shapes.");
    }
}
