using UnityEngine;
using UnityEditor;
using System;

public class SpriteImportOverrideForPixelArt : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;

        String name = importer.assetPath.ToLower();

        if (name.Substring(name.Length - 4, 4) == ".png")
        {
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;

            Debug.Log("Sprite import detected (\".png\"). Setting Filter mode to Point and disabling Mip Maps.");
        }

        
    }
}