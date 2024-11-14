using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButton : MonoBehaviour
{
    [SerializeField] Canvas canvasToHide;
    [SerializeField] Canvas unhideCanvas;

    public void Hide()
    {
        unhideCanvas.enabled = true;
        canvasToHide.enabled = false;
    }
}
