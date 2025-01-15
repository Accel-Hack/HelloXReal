using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// This script isn't used.
// This is used by attaching a gameobject, SelectVideoDropDown/Template/Viewport/Content/Item.
// This is to freeze items on the DropDown list whose video is being uploaded or processed on the server.
// Development of this function is putten off now.
public class DropDownItemMulti : MonoBehaviour
{
    private TextMeshProUGUI text;
    private SelectSequenceDropDownMulti dropDown;
    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        this.text = GetComponentInChildren<TextMeshProUGUI>();
        this.dropDown = FindObjectOfType<SelectSequenceDropDownMulti>();
        this.toggle = GetComponent<Toggle>();
    }

    private void FixedUpdate()
    {
        this.toggle.interactable = this.dropDown.IsUploading(this.text.text);
    }
}
