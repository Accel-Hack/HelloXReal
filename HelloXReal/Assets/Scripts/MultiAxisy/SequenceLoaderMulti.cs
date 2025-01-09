using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

// Load a sequence of the selected video.
// The video is selected on SelectVideoDropDownMulti.
public class SequenceLoaderMulti : MonoBehaviour
{
    private const int BUFFER_SIZE = 3;

    // SequenceLoaderMulti caches loaded datas in a ring buffer.
    private LoadedDataMulti[] loadedDatas = new LoadedDataMulti[BUFFER_SIZE];
    private int currentOldestIndex = 0;

    // Index in this.loadedDatas which is to be read by the MultiAxisyPlayer through the PlayButtonMulti.
    private int currentIndex = -1;

    public event Action<string> StartLoadingAction; // Action called with a string argument.
    public event Action<LoadedDataMulti> EndLoadingAction;

    public void LoadSequence(string sequenceName)
    {
        this.StartLoadingAction.Invoke(sequenceName);
        int index = this.ExistingIndex(sequenceName);
        if (index != -1)    // If the data with sequenceName already exists, set this.currentIndex without loading data.
        {
            this.currentIndex = index;
            this.EndLoadingAction.Invoke(this.loadedDatas[index]);
        }
        else
        {
            this.loadedDatas[currentOldestIndex] = new LoadedDataMulti(sequenceName);
            string url = "http://192.168.50.110:8000/download_sequence/" + sequenceName;
            StartCoroutine(LoadSequenceCoroutine(url, this.loadedDatas[this.currentOldestIndex]));
            this.currentIndex = this.currentOldestIndex;
            this.currentOldestIndex = (this.currentOldestIndex + 1) % BUFFER_SIZE;
        }
    }

    private IEnumerator LoadSequenceCoroutine(string url, LoadedDataMulti loadingData)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))  // Instantiate the web request.
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // The type of UnityWebRequest.downloadHandler.data is byte[].
                VideoData videoData = new VideoData(request.downloadHandler.data);
                loadingData.SetVideoData(videoData);
                this.EndLoadingAction.Invoke(loadingData);
            }
        }
    }

    // If a data with sequenceName already exists in this.loadedDatas, return the index.
    private int ExistingIndex(string sequenceName)
    {
        for (int i = 0; i < this.loadedDatas.Length; i++)
        {
            if (this.loadedDatas[i] == null) continue;
            if (this.loadedDatas[i].sequenceName == sequenceName) return i;
        }

        // Returning -1 means that there is no data with sequenceName.
        return -1;
    }

    // TODO! When the video with the same name of this.loadedData or when the video is deleted, the cash must be deleted.

    public VideoData GetSelectedVideoData()
    {
        return this.loadedDatas[this.currentIndex].videoData;
    }

    public LoadedDataMulti.Status GetSequenceStatus(string sequenceName)
    {
        foreach (LoadedDataMulti data in this.loadedDatas)
        {
            if (data.sequenceName == sequenceName)
            {
                return data.currentStatus;
            }
        }
        return LoadedDataMulti.Status.Loadable;
    }
}
