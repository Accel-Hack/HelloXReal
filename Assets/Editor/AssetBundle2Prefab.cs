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
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "external"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            myLoadedAssetBundle.Unload(false);
            return;
        }

        // Get fbx as GameObject from loaded AssetBundle.
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("model");

        // Object[] assets = myLoadedAssetBundle.LoadAssetWithSubAssets<Object>();
        // foreach (Object asset in assets) {
        //     // string path = "Assets/FromAssetBundle/" + asset.name + ".obj";
        //     Debug.Log(asset.name);
        //     // PrefabUtility.SaveAsPrefabAsset(asset, "Assets/FromAssetBundle/" + asset.name + ".prefab");
        //     // AssetDatabase.CreateAsset(asset, path);
        // }

        

        // PrefabUtility.SaveAsPrefabAsset(prefab, "Assets/FromAssetBundle/loaded_model.prefab");

        // // Instantiate fbx as GameObject.
        GameObject instance = Instantiate(prefab, new Vector3(0, 0, 9), Quaternion.identity);

        // bool flag;
        // string path = "Assets/loaded_model.prefab";
        // PrefabUtility.SaveAsPrefabAsset(instance, path, out flag);
        // if (!flag) {
        //     Debug.Log("Failed to save as prefab...");
        //     myLoadedAssetBundle.Unload(false);
        //     return;
        // }
        // AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);

        myLoadedAssetBundle.Unload(false);
    }
}
