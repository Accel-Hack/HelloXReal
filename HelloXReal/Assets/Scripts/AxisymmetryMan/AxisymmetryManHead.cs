using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Head of AxisymmetryMan.
public class AxisymmetryManHead : AxisymmetryManPart
{
    public void Place(Vector3 rightEar, Vector3 leftEar, Vector3 nose)
    {
        Vector3 front = nose - (rightEar + leftEar) / 2;
        Vector3 right = rightEar - leftEar;
        Vector3 up = Vector3.Cross(right, front);
        float xScale = right.magnitude;
        float yScale = xScale * 1.2f;
        float zScale = xScale;
        Vector3 position = (rightEar + leftEar) / 2;

        transform.localPosition = position;
        transform.rotation = Quaternion.LookRotation(front, up);
        transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}
