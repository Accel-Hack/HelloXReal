using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    [SerializeField] StickmanLoader stickmanLoader;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetComponent<Uploader>().UploadFile("pompompurin.txt"));
    }
}
