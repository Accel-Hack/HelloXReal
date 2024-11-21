using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonAxisy : MonoBehaviour
{
    [SerializeField] AxisymmetryMan axisy;

    // Called by button OnClick event.
    public void OnClick()
    {
        axisy.Play();
    }
}
