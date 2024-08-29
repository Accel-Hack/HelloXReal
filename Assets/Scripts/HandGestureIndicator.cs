using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NRKernal;

public class HandGestureIndicator : MonoBehaviour
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
        HandState handState = NRInput.Hands.GetHandState(HandEnum.LeftHand);
        switch (handState.currentGesture) {
            case HandGesture.Pinch:
                text.text = "Pinch";
                break;
            case HandGesture.Point:
                text.text = "Point";
                break;
            default:
                text.text = "Other";
                break;
        }
    }
}