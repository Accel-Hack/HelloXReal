using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedDataMulti
{
    public VideoData videoData = null;

    public string sequenceName;

    public enum Status {
        Loadable,
        Loading,
        Loaded
    }
    public Status currentStatus = Status.Loading;

    public LoadedDataMulti(string sequenceName)
    {
        this.sequenceName = sequenceName;
    }

    public void SetVideoData(VideoData videoData)
    {
        this.videoData = videoData;
        this.currentStatus = Status.Loaded;
    }
}
