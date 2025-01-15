using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToRotateCamera : PanelToCameraWork
{
    private const float ROTATE_SPEED = 0.5f;

    private void Update()
    {
        switch (base.status)
        {
            case TouchStatus.OnDrag:
                base.cameraTrans.Rotate(Time.deltaTime * ROTATE_SPEED * base.deltaPosition.x * Vector3.up, Space.Self);
                break;
        }
    }
}
