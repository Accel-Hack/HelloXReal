using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class StickmanLoader : MonoBehaviour
{
    [SerializeField] StickmanCreater stickmanCreater;
    [SerializeField] AnimationSelecter animationSelecter;

    // Class to read json.
    [System.Serializable]
    public class FileList
    {
        public List<string> files;
    }

    // String to contain loaded data.
    private string result;

    public IEnumerator LoadAnimationList()
    {
        string url = "http://192.168.50.110:8000/list_files";
        Coroutine coroutine = StartCoroutine(DownloadText(url));
        yield return coroutine;

        // this.result is updated when the coroutine finishes.

        // Read json as a FileList.
        this.SetAnimations(this.result);
    }

    public void SetAnimations(string files)
    {
        // Read json as a FileList.
        List<string> fileList = JsonUtility.FromJson<FileList>(files).files;
        Debug.Log("?");
        animationSelecter.SetAnimations(fileList);
    }

    public IEnumerator LoadAnimation(string url)
    {
        Coroutine coroutine = StartCoroutine(DownloadText(url));
        yield return coroutine;

        // this.result is updated when the coroutine finishes.

        stickmanCreater.InitializeWithSequence(this.result);
    }

    private IEnumerator DownloadText(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))  // Create request.
        {
            // Send request and wait until download completed.
            yield return request.SendWebRequest();

            // Error check
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                this.result = null;
                yield break;
            }
            else
            {
                // Download suceed.
                this.result = request.downloadHandler.text;
            }
        }
    }
}
