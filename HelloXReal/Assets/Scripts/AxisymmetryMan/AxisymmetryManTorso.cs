using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Torso of AxisymmetryMan.
public class AxisymmetryManTorso : AxisymmetryManPart
{
    public void Place(Vector3 rightShoulder, Vector3 leftShoulder, Vector3 waist)
    {
        Vector3 front = Vector3.Cross(leftShoulder - rightShoulder, waist - rightShoulder);
        Vector3 up = (rightShoulder + leftShoulder) / 2 - waist;
        Vector3 right = Vector3.Cross(up, front);
        Debug.Log(front);
        Debug.Log(right);
    }
}
