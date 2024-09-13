using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    GameObject inventorySlotUI;
    List<GameObject> inventorySlots = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.Instance.onInventoryChange += renderInventory;
    }

    void renderInventory()
    {
        foreach (GameObject item in inventorySlots)
        {
            Destroy(item);
        }
        inventorySlots.Clear();
        List<InventorySlot> inventory = InventoryManager.Instance.inventory;
        int i = 0;
        foreach (InventorySlot slot in inventory)
        {
            inventorySlotUI.GetComponent<InventorySlotUI>().slot = slot;
            GameObject newInstance = Instantiate(inventorySlotUI);
            newInstance.transform.SetParent(this.gameObject.transform);
            newInstance.transform.localPosition = new Vector2(i * 60, 0);
            inventorySlots.Add(newInstance);
            i++;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
