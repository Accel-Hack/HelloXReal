using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
        BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
