using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SequenceLoader : MonoBehaviour
{
    private string loadedSequenceString = null;

    public IEnumerator LoadSequence(string url)
    {
        Coroutine coroutine = StartCoroutine(DownloadText(url));
        yield return coroutine;

        // this.loadedSequenceString is updated when the coroutine finishes.
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
                this.loadedSequenceString = null;
                yield break;
            }
            else
            {
                // Download suceed.
                this.loadedSequenceString = request.downloadHandler.text;
            }
        }
    }

    public List<List<Vector3>> GetSequence()
    {
        return MediaPipeParser.ReadSequence(this.loadedSequenceString);
    }
}
