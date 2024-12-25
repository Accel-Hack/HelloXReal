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
                transform.Translate(Time.deltaTime * 0.1f * new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y), Space.Self);
            } else {
                transform.Rotate(Time.deltaTime * touch.deltaPosition.x * Vector3.up, Space.Self);
            }
        }
    }
}
