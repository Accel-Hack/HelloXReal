using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    [SerializeField] StickmanCreater stickmanCreater;

    // This method is to be registered on Button.OnClick on Unity Editor.
    public void OnClick()
    {
        stickmanCreater.Replay();
    }
}
