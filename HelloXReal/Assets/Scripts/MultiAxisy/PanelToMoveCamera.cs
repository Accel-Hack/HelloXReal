using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToMoveCamera : PanelToCameraWork
{
    private const float MOVE_SPEED = 0.05f;

    private void Update()
    {
        switch (base.status)
        {
            case TouchStatus.OnDrag:
                base.cameraTrans.Translate(Time.deltaTime * MOVE_SPEED * new Vector3(base.deltaPosition.x, 0, base.deltaPosition.y), Space.Self);
                break;
        }
    }
}
