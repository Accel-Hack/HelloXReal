using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

// Load prefab from AssetBundle and instantiate it.
public class ModelBundleLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        // Load AssetBundle from local files.
        // var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "external"));

        StartCoroutine(Request());        
    }

    private IEnumerator Request()   // A coroutine runs as a subprocess.
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("http://192.168.50.110:8000/external");
        text.text = "loading";
        yield return www.SendWebRequest();  // Wait until loading completed.
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.LogError(www.error);
            text.text = www.error;
            yield break;    // End this coroutine.
        }
        AssetBundle myLoadedAssetBundle = DownloadHandlerAssetBundle.GetContent(www);

        if (myLoadedAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            text.text = "Failed to load AssetBundle!";
            yield break;
        }
        text.text = "?";

        // Get fbx as GameObject from loaded AssetBundle.
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("model.prefab");

        // Instantiate fbx as GameObject.
        GameObject model = Instantiate(prefab, new Vector3(0, 0, 9), Quaternion.identity);
    }
}