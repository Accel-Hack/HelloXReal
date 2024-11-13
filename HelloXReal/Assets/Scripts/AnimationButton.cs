using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationButton : MonoBehaviour
{
    // Name of animation file (output of mediapipe) corresponding to this instance.
    private string fileName = null;
    private StickmanLoader stickmanLoader = null;

    public void Initialize(string fileName, StickmanLoader stickmanLoader)
    {
        this.fileName = fileName;
        this.stickmanLoader = stickmanLoader;
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = fileName;
    }

    // This method is to be registered on Button.OnClick on Unity Editor.
    public void DecideAnimation()
    {
        string url = "http://192.168.50.110:8000/download_animation/" + this.fileName;
        StartCoroutine(stickmanLoader.LoadAnimation(url));
    }
}
