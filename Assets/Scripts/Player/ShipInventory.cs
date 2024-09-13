using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInventory : MonoBehaviour
{
    private string GenerateInventoryReport()
    {
        string InventoryReport = "Inventory\n";

        foreach (InventorySlot item in InventoryManager.Instance.inventory)
        {
            InventoryReport += item.item.name + ": " + item.amount + ", ";
        }

        return InventoryReport;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pickable" || collision.gameObject.tag == "Healing")
        {
            if (collision.gameObject.tag == "Pickable")
            {

                InventoryItem InventoryObject = (collision.gameObject.GetComponent<PickableObject>().item);
                InventoryManager.Instance.Add(InventoryObject, 1);
            }
            else
            {
                GetComponent<HealthBar>().Heal(15);
            }


            PickableObject pickableObject = collision.gameObject.GetComponent<PickableObject>();
            pickableObject.TriggerPickup();
        }
    }
}

