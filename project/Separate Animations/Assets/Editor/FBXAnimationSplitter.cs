using UnityEditor;
using UnityEngine;

public class FBXAnimationSplitter : Editor
{
    [MenuItem("Tools/Split Animations")]
    static void SplitAnimations()
    {
        // Set the directory path where the FBX files are located
        string directoryPath = "Assets/fbx";

        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

        // Get all FBX files in the directory
        string[] fbxFiles = System.IO.Directory.GetFiles(directoryPath, "*.fbx");

        foreach (string fbxPath in fbxFiles)
        {
            // Load the FBX file as a GameObject
            GameObject fbxAsset = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);

            // Get the ModelImporter to access the animation data
            ModelImporter modelImporter = AssetImporter.GetAtPath(fbxPath) as ModelImporter;

            if (modelImporter != null && modelImporter.clipAnimations.Length > 0)
            {
                foreach (var clip in modelImporter.clipAnimations)
                {
                    // Modify or extract animation clips here
                    Debug.Log($"Processing animation {clip.name} from {fbxPath}");
                }

                // Optionally, save changes
                AssetDatabase.ImportAsset(fbxPath);
            }
        }
    }
}
