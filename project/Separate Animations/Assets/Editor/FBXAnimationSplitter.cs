using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class FBXAnimationSplitter : EditorWindow
{
    private const int BatchSize = 10; // Process 10 files at a time
    private static Queue<string> fbxQueue;
    private static string outputFolder = "ExtractedAnimations";

    [MenuItem("Tools/Split Animations")]
    static void Init()
    {
        FBXAnimationSplitter window = (FBXAnimationSplitter)EditorWindow.GetWindow(typeof(FBXAnimationSplitter));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Start Splitting Animations"))
        {
            StartSplittingAnimations();
        }
    }

    static void StartSplittingAnimations()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Fbx");

        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"Directory does not exist: {directoryPath}");
            return;
        }

        string[] fbxFiles = Directory.GetFiles(directoryPath, "*.fbx");
        fbxQueue = new Queue<string>(fbxFiles);

        // Ensure output directory exists
        string outputPath = Path.Combine(Application.dataPath, outputFolder);
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        EditorApplication.update += ProcessBatch;
    }

    static void ProcessBatch()
    {
        int processedInThisBatch = 0;

        while (fbxQueue.Count > 0 && processedInThisBatch < BatchSize)
        {
            string fbxPath = fbxQueue.Dequeue();
            ProcessSingleFile(fbxPath);
            processedInThisBatch++;

            // Force a garbage collection to free up memory
            System.GC.Collect();
        }

        if (fbxQueue.Count == 0)
        {
            EditorApplication.update -= ProcessBatch;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("All animations have been processed.");
        }
    }

    static void ProcessSingleFile(string fbxPath)
    {
        string assetPath = "Assets" + fbxPath.Substring(Application.dataPath.Length);
        Debug.Log($"Processing: {assetPath}");

        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);

        if (clip != null)
        {
            string fileName = Path.GetFileNameWithoutExtension(fbxPath).Split(" ")[1];
            string newAssetPath = $"Assets/{outputFolder}/{fileName}.anim";

            AnimationClip newClip = new AnimationClip();
            EditorUtility.CopySerialized(clip, newClip);
            AssetDatabase.CreateAsset(newClip, newAssetPath);
            Debug.Log($"Created new animation: {newAssetPath}");
        }
        else
        {
            Debug.LogWarning($"No AnimationClip found in: {assetPath}");
        }

        // Unload unused assets to free up memory
        Resources.UnloadUnusedAssets();
    }
}