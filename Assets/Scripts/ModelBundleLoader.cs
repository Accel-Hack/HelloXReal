using System.IO;
using UnityEngine;

public class ModelBundleLoader : MonoBehaviour
{
    void Start()
    {
        // If it is release version, path should involve Application.streamingAssetsPath.
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "external"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("model.fbx");
        Instantiate(prefab, new Vector3(0, 0, 10), Quaternion.identity);
    }
}