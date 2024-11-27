using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NRKernal;

public class PlantNameIndicator : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] FlowerController flowerController;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = flowerController.CurrentFlowerName();
    }
}