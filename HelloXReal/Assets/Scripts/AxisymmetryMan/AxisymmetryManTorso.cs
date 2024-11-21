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
        Vector3 right = rightShoulder - leftShoulder;
        float xScale = right.magnitude * 0.9f;
        float yScale = up.magnitude;
        float zScale = xScale / 3;
        Vector3 position = (rightShoulder + leftShoulder + waist) / 3;

        transform.localPosition = position;
        transform.rotation = Quaternion.LookRotation(front, up);
        transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}
