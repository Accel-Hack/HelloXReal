using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Getbundle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DownLoad()); 
	}

    IEnumerator DownLoad()
    {
        string bundleUrl = Application.streamingAssetsPath + "/testbundle";
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundleUrl);
        while (!request.isDone)
        {
            yield return null;
        }
        AssetBundle assetBundle = request.assetBundle;
        AssetBundleRequest image = assetBundle.LoadAssetAsync<GameObject>("Sphere");
        Instantiate(image.asset, new Vector3(0, 0, 10), Quaternion.identity);
    }
}