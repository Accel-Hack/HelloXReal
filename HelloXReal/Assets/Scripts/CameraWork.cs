using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2) {
                transform.position += new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y) * 0.01f;
            }
        }
    }
}
