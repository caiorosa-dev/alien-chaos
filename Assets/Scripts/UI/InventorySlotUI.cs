using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public InventorySlot slot;
    [SerializeField]
    GameObject ImageObject;
    [SerializeField]
    GameObject TextObject;
    // Start is called before the first frame update
    void Start()
    {
        ImageObject.GetComponent<Image>().sprite = slot.item.icon;
        TextObject.GetComponent<TextMeshProUGUI>().text = slot.amount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
