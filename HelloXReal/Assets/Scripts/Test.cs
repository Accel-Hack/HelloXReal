using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    [SerializeField] HideButton hide;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            hide.Hide();
        }
    }
}
