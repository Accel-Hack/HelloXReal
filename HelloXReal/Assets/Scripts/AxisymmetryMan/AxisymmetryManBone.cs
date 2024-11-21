using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Limb, finger and neck.
public class AxisymmetryManBone : MonoBehaviour
{
    public void Place(Vector3 head, Vector3 tail)
    {
        float length = (tail - head).magnitude;
        float thickness = length / 3;
        Vector3 position = (head + tail) / 2;

        transform.localPosition = position;
        transform.LookAt(tail - head + transform.position);
        transform.localScale = new Vector3(thickness, thickness, length);
    }
}
