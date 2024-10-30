using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] StickmanCreater stickmanCreater;

    public void OnClick()
    {
        stickmanCreater.Replay();
    }
}
