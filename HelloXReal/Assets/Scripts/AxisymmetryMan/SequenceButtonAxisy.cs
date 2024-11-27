using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SequenceButtonAxisy : MonoBehaviour
{
    // Name of animation file (output of mediapipe) corresponding to this instance.
    private string fileName = null;
    private SequenceLoader sequenceLoader = null;
    private AxisymmetryMan axisy = null;

    public void Initialize(string fileName)
    {
        this.fileName = fileName;
        this.sequenceLoader = FindObjectOfType<SequenceLoader>();
        this.axisy = FindObjectOfType<AxisymmetryMan>();
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = fileName;
    }

    // This method is to be registered on Button.OnClick on Unity Editor.
    public void OnClick()
    {
        StartCoroutine(ChooseSequence());
    }

    private IEnumerator ChooseSequence()
    {
        string url = "http://192.168.50.110:8000/download_animation/" + this.fileName;
        yield return this.sequenceLoader.LoadSequence(url);
        List<List<Vector3>> sequence = this.sequenceLoader.GetSequence();
        this.axisy.SetSequence(sequence);
    }
}
