using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    private Vector2 leftStartPosition = Vector2.zero;
    private Vector2 rightStartPosition = Vector2.zero;

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
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        this.leftStartPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
                        Vector2 delta = touch.position - this.leftStartPosition;
                        transform.Translate(Time.deltaTime * 0.05f * new Vector3(delta.x, 0, delta.y), Space.Self);
                        break;
                }
            } else {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        this.rightStartPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
                        Vector2 delta = touch.position - this.rightStartPosition;
                        transform.Rotate(Time.deltaTime * delta.x * Vector3.up, Space.Self);
                        break;
                }
            }
        }
    }
}
