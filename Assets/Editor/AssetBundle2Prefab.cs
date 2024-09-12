using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundle2Prefab: MonoBehaviour
{
    [MenuItem("Assets/Construct Prefab")]
    static void Perform() 
    {
        // Load AssetBundle from local files.
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "external"));

        // Empty gameobject to be parent of fbx.
        // GameObject parent = new GameObject("ModelParent");

        // Get fbx as GameObject from loaded AssetBundle.
        var fbx = myLoadedAssetBundle.LoadAsset<GameObject>("model.fbx");
        Debug.Log("*\n" + fbx.GetComponentInChildren<MeshFilter>().mesh + "\n*");
        GameObject instance = Instantiate(fbx);

        // fbx.transform.SetParent(parent.transform, false);

        PrefabUtility.SaveAsPrefabAssetAndConnect(instance, "Assets/model.prefab", InteractionMode.UserAction);
    }
}
