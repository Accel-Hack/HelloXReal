using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSequenceReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset textAsset = Resources.Load("sequence") as TextAsset;
        byte[] bytes = textAsset.bytes;

        VideoData videoData = VideoData(bytes);
    }
}
