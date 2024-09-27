using System.IO;
using UnityEngine;

// Load prefab from AssetBundle and instantiate it.
public class ModelBundleLoader : MonoBehaviour
{
    void Start()
    {
        // Load AssetBundle from local files.
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "external"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        // Get fbx as GameObject from loaded AssetBundle.
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("model.prefab");

        // Instantiate fbx as GameObject.
        GameObject model = Instantiate(prefab, new Vector3(0, 0, 9), Quaternion.identity);
    }
}