using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButtonMulti : MonoBehaviour
{
    [SerializeField] MultiAxisyPlayer player;

    // Called by button OnClick event.
    public void OnClick()
    {
        player.Stop();
    }
}
