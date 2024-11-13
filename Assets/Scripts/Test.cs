using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    [SerializeField] AnimationSelecter animationSelecter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            Debug.Log("Reload");
            animationSelecter.SetAnimations(new List<string>(new string[]{"a", "b", "c"}));
        }
    }
}
