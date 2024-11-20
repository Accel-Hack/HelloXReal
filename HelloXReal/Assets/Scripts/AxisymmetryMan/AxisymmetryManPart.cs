using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent class of an AxsymmetryMan's body part.
public class AxisymmetryManPart : MonoBehaviour
{
    protected GameObject meshPrefab;    // Prefab of the part.

    public void Initialize(GameObject meshPrefab)
    {
        // meshPrefab must be normalized in (1, 1, 1) scale.
        this.meshPrefab = meshPrefab;
    }
}
