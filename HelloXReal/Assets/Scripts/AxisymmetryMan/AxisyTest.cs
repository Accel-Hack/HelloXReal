using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisyTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AxisymmetryManTorso torso = FindObjectOfType<AxisymmetryManTorso>();
        Vector3 rightShoulder = new(1, 1, 0);
        Vector3 leftShoulder = new(-1, 1, 0);
        Vector3 weist = new(0, -1, 0);
        torso.Place(rightShoulder, leftShoulder, weist);
    }
}
