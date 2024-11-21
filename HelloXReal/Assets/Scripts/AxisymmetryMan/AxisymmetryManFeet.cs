using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Feet of AxisymmetryMan.
public class AxisymmetryManFeet : AxisymmetryManPart
{
    public void Place(Vector3 ankle, Vector3 index, Vector3 heel)
    {
        Vector3 front = index - heel;
        Vector3 perpendicularFootPoint = Vector3.Project(ankle - heel, index - heel) + heel;
        Vector3 up = ankle - perpendicularFootPoint;
        float yScale = up.magnitude;
        float zScale = front.magnitude;
        float xScale = zScale / 2;

        transform.localPosition = (ankle + index + heel) / 3;
        transform.rotation = Quaternion.LookRotation(front, up);
        transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}
