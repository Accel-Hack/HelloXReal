using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NRKernal;

public class ControllerIndicator : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "x: " + NRInput.GetTouch().x + "\ny: " + NRInput.GetTouch().y;
    }
}
