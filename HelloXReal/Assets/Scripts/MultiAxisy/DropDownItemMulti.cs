using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
