using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelToCameraWork : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Transform cameraTrans;

    protected enum TouchStatus {
        OnDrag,
        NotOnDrag
    }
    protected TouchStatus status = TouchStatus.NotOnDrag;
    protected Vector3 onBeginDragPosition = Vector3.zero;
    protected Vector3 currentDragPosition = Vector3.zero;
    protected Vector3 deltaPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        this.cameraTrans = FindObjectOfType<Camera>().transform;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        this.status = TouchStatus.OnDrag;
        this.onBeginDragPosition = eventData.position;
        this.currentDragPosition = eventData.position;
        this.UpdateDeltaPosition();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        this.status = TouchStatus.OnDrag;
        this.currentDragPosition = eventData.position;
        this.UpdateDeltaPosition();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        this.status = TouchStatus.NotOnDrag;
        this.currentDragPosition = eventData.position;
        this.UpdateDeltaPosition();
    }

    private void UpdateDeltaPosition()
    {
        this.deltaPosition = this.currentDragPosition - this.onBeginDragPosition;
    }
}
