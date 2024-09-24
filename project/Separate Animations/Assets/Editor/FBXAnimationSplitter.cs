using UnityEditor;
using UnityEngine;
using System.IO;

public class FBXAnimationSplitter : Editor
{
    [MenuItem("Tools/Split Animations")]
    static void SplitAnimations()
    {
        // Set the directory path where the FBX files are located
        string directoryPath = Path.Combine(Application.dataPath, "Fbx");

        // Check if the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"Directory does not exist: {directoryPath}");
            return;
        }

        // Get all FBX files in the directory
        string[] fbxFiles = Directory.GetFiles(directoryPath, "*.fbx");


        foreach (string fbxPath in fbxFiles)
        {
        // Convert the full path to a relative path that Unity uses
            string assetPath = "Assets" + fbxPath.Substring(Application.dataPath.Length);
            Debug.Log($"Processing: {assetPath}");

            // Load the FBX file as an AnimationClip
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
            
            if (clip != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(fbxPath).Split(" ")[1];
                // Use the relative path for the new asset
                string newAssetPath = $"Assets/ExtractedAnimations/{fileName}.anim";

                AnimationClip newClip = new AnimationClip();
                EditorUtility.CopySerialized(clip, newClip);
                AssetDatabase.CreateAsset(newClip, newAssetPath);
                Debug.Log($"Created new animation: {newAssetPath}");
            }
            else
            {
                Debug.LogWarning($"No AnimationClip found in: {assetPath}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
