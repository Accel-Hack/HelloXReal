using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisyTest : MonoBehaviour
{
    [SerializeField] AxisymmetryMan a;
    [SerializeField] AxisymmetryMan b;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        SequenceLoader loader = FindObjectOfType<SequenceLoader>();
        yield return loader.LoadSequence("http://192.168.50.110:8000/download_animation/video5.txt");
        List<List<Vector3>> sequence5 = loader.GetSequence();
        a.SetSequence(sequence5);
        a.Play();

        yield return loader.LoadSequence("http://192.168.50.110:8000/download_animation/video8.txt");
        List<List<Vector3>> sequence8 = loader.GetSequence();
        b.SetSequence(sequence8);
        b.Play();
    }
}
