using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisyTest : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        SequenceLoader loader = FindObjectOfType<SequenceLoader>();
        yield return loader.LoadSequence("http://192.168.50.110:8000/download_animation/video5.txt");
        List<List<Vector3>> sequence = loader.GetSequence();
        AxisymmetryMan axisy = FindObjectOfType<AxisymmetryMan>();
        axisy.SetSequence(sequence);
        axisy.Play();
    }
}
